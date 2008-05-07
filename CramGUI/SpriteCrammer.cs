using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Security.Cryptography;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Cram
{
    class SourceImage
    {
        public string Filename = "";
        public string ShortName = "";
        public SourceImage DuplicateOf = null;
        public int Width = 0;
        public int Height = 0;
        public int CropX = 0;
        public int CropY = 0;
        public int CropW = 0;
        public int CropH = 0;
        public int[] Data;
        public int Stride = 0;
        public int PageId = 0;
        public Rect Rect = null;
    }

    public delegate void ProgressLog(string s);

    public class SpriteCramException : Exception
    {
        public SpriteCramException() : base() {}
        public SpriteCramException(string s) : base(s) {}
    }

    public class SpriteCrammer
    {
        public event ProgressLog LogEvent;
        public CramSettings Settings;
        private List<SourceImage> SourceImages = new List<SourceImage>();
        private Dictionary<string, SourceImage> uniqueTable = new Dictionary<string, SourceImage>();
        private int numDups = 0;
        private int numUnique = 0;
        private int numFailed = 0;
        private List<Page> pages;
        private List<String> pageFilenames = new List<String>();

        // Indicates a "fatal" error that should return a non-zero (i.e., failed) result to shell
        private void FatalError()
        {
            throw new SpriteCramException();
        }

        public bool Run()
        {
            try
            {
                RunInternal();
                return true;
            }
            catch (SpriteCramException)
            {
                Log("Generation failed.");
                return false;
            }
            catch (Exception e)
            {
               Log("*** An exception occurred! ***");
               Log(e.Message);
               return false;
            }
        }

        public void RunInternal()
        {
            int startTicks = Environment.TickCount;

            if (Settings.SourceImages.Count == 0)
            {
                Log("No input images, aborting.");
                FatalError();
            }

            if (Settings.AllowFallbacks && Settings.FallbackSizes.Count == 0)
            {
                Log("No fallback sizes, aborting.");
                FatalError();
            }

            if (Settings.XmlFilename == "" ||
                Settings.XmlFilename == null)
            {
                Log("No XML output filename, aborting.");
                FatalError();
            }

            Log("Processing input images...");

            foreach (string srcImg in Settings.SourceImages)
            {
                ProcessImage(srcImg);
            }
            
            if (SourceImages.Count == 0)
            {
                Log("No input images left after processing, aborting.");
                FatalError();
            }

            // Check for duplicate images
            // TODO: Should this be case insensitive...? Seems like an edge case.
            Dictionary<string, bool> dupNameCheckDict = new Dictionary<string, bool>();
            foreach (SourceImage img in SourceImages)
            {
                if (dupNameCheckDict.ContainsKey(img.ShortName))
                    Log("Warning: Duplicate filename: {0}", img.ShortName);
                else
                    dupNameCheckDict[img.ShortName] = true;
            }

            long before = 0;
            long after = 0;
            foreach (SourceImage img in SourceImages)
            {
                before += img.Width * img.Height;

                // Only count images that aren't duplicates!
                if(img.DuplicateOf == null)
                    after += img.CropW * img.CropH;
            }

            //Log("Percent left after cropping & dup. removal: {0:#.#}%", 100.0 * (double)after / (double)before);

            Log("Packing...");

            // Now, actually pack stuff...
            List<Rect> rects = new List<Rect>();
            foreach (SourceImage img in SourceImages)
            {
                if (img.DuplicateOf == null)
                {
                    // TODO: Deal w/ dups
                    Rect r = new Rect(0, 0, img.CropW, img.CropH);
                    r.Tag = img;
                    rects.Add(r);
                }
            }


            List<Rect> unfitted;
            RectCramOptions opts = new RectCramOptions();
            opts.PageW = Settings.PageSize.Width;
            opts.PageH = Settings.PageSize.Height;
            opts.Border = Settings.Border;
            opts.Rotate = Settings.Rotate;
            opts.ExtraTight = true;
            opts.EnableFallbacks = Settings.AllowFallbacks;
            opts.FallbackCount = Settings.MaxFallbacks;
            opts.FallbackSizes = Settings.FallbackSizes;
            opts.LogEvent += this.Log;

            pages = RectCrammer.PackMultiplePagesWithFallback(rects,
                                                opts,
                                                out unfitted);

            foreach (Rect r in unfitted)
            {
                Log("Error: Couldn't fit rect: {0} ({1}x{2})",
                        ((SourceImage)(r.Tag)).ShortName,
                        r.W, r.H);
            }

            // Clean pages, if any exist
            if (Settings.Clean)
            {
                Log("Removing old sprite pages...");
                List<string> matchingPages = FindMatchingPages(Settings.XmlFilename);
                foreach (string s in matchingPages)
                {
                    try
                    {
                        File.Delete(s);
                        Log("Removed {0}", Path.GetFileName(s));
                    }
                    catch (Exception)
                    {
                        Log("Error: Couldn't remove old page: {0}", s);
                    }
                }
            }

            // Save pages
            int pageId = 0;
            foreach (Page page in pages)
            {
                string partialFilename = "";
                string filename = "";

                try
                {
                    partialFilename = Path.GetFileNameWithoutExtension(Settings.XmlFilename) +
                                                    String.Format("{0:0000}.png", pageId);

                    filename = Path.Combine(Path.GetDirectoryName(Settings.XmlFilename),
                                                    partialFilename);
                }
                catch (Exception)
                {
                    Log("Error saving page: Invalid XML filename, cannot generate page filename");
                    FatalError();
                }

                pageFilenames.Add(partialFilename);

                WritePage(filename, page);
                ++pageId;
            }

            pageId = 0;
            foreach (Page page in pages)
            {
                foreach (Rect rect in page.FittedRects)
                {
                    SourceImage img = (SourceImage)rect.Tag;
                    img.Rect = rect;
                    img.PageId = pageId;
                }

                ++pageId;
            }

            // Save the XML
            SaveXML();

            int endTicks = Environment.TickCount;

            // Perform some statistics gathering stuff


            long newSpaceUsed = 0;
            long wasted = 0;
            long used = 0;
            foreach (Page page in pages)
            {
                newSpaceUsed += page.TotalPixels;
                wasted += page.WastePixels;
                used += page.UsedPixels;
            }


            Log("=========================================");
            Log("STATISTICS");
            Log("=========================================");

            Log("Time: {0:0.#} sec", (endTicks-startTicks)/1000.0);
            Log("Frames:");
            Log("    Unique: {0}", numUnique - unfitted.Count);
            Log("    Duplicate: {0}", numDups);
            Log("    Too big: {0}", unfitted.Count);
            Log("    Failed: {0}", numFailed);
            Log("    Total: {0}", Settings.SourceImages.Count);
            Log("VRAM: {0} @ 32 bpp",
                   Util.FormatBytes((ulong)newSpaceUsed*4));
            Log("    Used: {0:#.#}% / {1} @ 32 bpp",
                   100.0 * (double)used / (double)newSpaceUsed,
                   Util.FormatBytes((ulong)(used * 4)));
            Log("    Waste: {0:#.#}% / {1} @ 32 bpp",
                   100.0 * (double)wasted / (double)newSpaceUsed,
                   Util.FormatBytes((ulong)(wasted * 4)));
            Log("    Reduction: {0:#.#}% / {1} {2} @ 32 bpp",
                    100.0 * (double)(before - newSpaceUsed) / (double)before,
                    Util.FormatBytes((ulong)Math.Abs(before - newSpaceUsed)*4),
                    before >= newSpaceUsed ? "shaved" : "added");

            if (pages.Count > 0)
            {
                int n = 0;
                Log("Pages ({0} total):", pages.Count);
                foreach (Page p in pages)
                {
                    Log("    #{0}: Size={1}x{2}, Frames={3}, Usage={4:#.#}%",
                         n, p.Width, p.Height, p.FittedRects.Count, p.UsagePercent);
                    ++n;
                }
            }
            else
            {
                Log("No pages generated");
                FatalError();
            }

        }


        private List<string> FindMatchingPages(string xmlPath)
        {
            List<string> matches = new List<string>();
            string dir = Path.GetDirectoryName(xmlPath);
            if(!Directory.Exists(dir))
                return matches;
            string basename = Path.GetFileNameWithoutExtension(xmlPath).ToLower();
            FileInfo[] fileInfos = new DirectoryInfo(dir).GetFiles();
            // TODO: Make this work case sensitively on Linux, etc...?

            Regex regex = new System.Text.RegularExpressions.Regex("^[0-9][0-9][0-9][0-9]$");

            foreach (FileInfo fileInfo in fileInfos)
            {
                string fileName = fileInfo.Name.ToLower();

                if (!fileName.StartsWith(basename))
                    continue;

                fileName = fileName.Substring(basename.Length);

                if (!fileName.EndsWith(".png"))
                    continue;

                fileName = fileName.Substring(0, fileName.Length - ".png".Length);
                if (fileName.Length != 4)
                    continue;

                if (regex.Match(fileName).Success)
                    matches.Add(fileInfo.FullName);            
            }

            return matches;
        }

        private void ProcessImage(string filename)
        {
            // Try to load the image from file
            Image origImg;

            SourceImage srcImg = new SourceImage();
            srcImg.Filename = filename;
            
            // Some of these path conversions can fail, if they do, fallback to the
            // default path
            try
            {
                switch (Settings.PathMode)
                {
                    case PathMode.Short:
                        srcImg.ShortName = Path.GetFileName(filename);
                        break;
                    case PathMode.Full:
                        srcImg.ShortName = Path.GetFullPath(filename);
                        break;
                    case PathMode.Relative:
                        srcImg.ShortName = PathUtil.RelativePathTo(Path.GetFullPath(Settings.RelativePathBase),
                                                                        Path.GetFullPath(filename));
                        break;
                }
            }
            catch (Exception)
            {
                Log("Error adjusting path, using original: {0}", filename);
                srcImg.ShortName = filename;
            }


            try
            {
                origImg = Image.FromFile(srcImg.Filename);
            }
            catch (Exception)
            {
                Log("Error opening file, skipping: {0}", srcImg.ShortName);
                ++numFailed;
                return;
            }

            // Probably can't happen, but just in case
            if (origImg.Width == 0 || origImg.Height == 0)
            {
                Log("Image has a zero dimension, skipping: {0}", srcImg.Filename);
                ++numFailed;
                return;
            }

            // TODO: Only do this if necessary?

            // If that succeeded, then copy it to a bitmap with a known pixel format (ARGB)
            // to do some work on it
            Bitmap bmp = new Bitmap(origImg.Width, origImg.Height, PixelFormat.Format32bppArgb);
            using (Graphics graphics = Graphics.FromImage(bmp))
            {
                graphics.Clear(Color.Transparent);
                graphics.DrawImage(origImg, 0, 0, origImg.Width, origImg.Height);
            }

            // Clean up original image
            origImg.Dispose();

            // Copy pixels out toa raw int array -- this is about 400x faster than GetPixel()
            int pixels = bmp.Width * bmp.Height;
            int[] pixelData = new int[pixels];
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                                            ImageLockMode.ReadOnly,
                                            PixelFormat.Format32bppArgb);
            System.Runtime.InteropServices.Marshal.Copy(data.Scan0, pixelData, 0, pixels);
            bmp.UnlockBits(data);

            // Fill in some fields on the source image info block
            srcImg.Data = pixelData;
            srcImg.Stride = data.Stride / 4;
            srcImg.Width = bmp.Width;
            srcImg.Height = bmp.Height;
            srcImg.CropW = srcImg.Width;
            srcImg.CropH = srcImg.Height;

            bmp.Dispose();

            // Perform colorkeying
            ColorKey(srcImg);

            // Perform cropping of transparent areas
            if (!Crop(srcImg))
            {
                Log("Image has zero dimension after cropping, skipping: {0}", srcImg.ShortName);
                ++numFailed;
                return;
            }

            DumpImage(srcImg);

            if (Settings.Unique)
            {
                string md5 = ComputeHash(srcImg);
                if (uniqueTable.ContainsKey(md5))
                {
                    // TODO: Check if the images are really exactly the same!
                    SourceImage dupOf = uniqueTable[md5];
                    srcImg.DuplicateOf = dupOf;
                    Log("Duplicate found! {0} has same hash as {1}: {2}", srcImg.ShortName, dupOf.ShortName, md5);
                    ++numDups;
                }
                else
                {
                    uniqueTable[md5] = srcImg;
                    ++numUnique;
                }
            }
            else
                ++numUnique;

            // Save this to the source image list
            SourceImages.Add(srcImg);
        }

        private int DetectColorKey(SourceImage srcImg)
        {
            int w = srcImg.Width, h = srcImg.Height;
            // Lol repetitive code :(
            int a = srcImg.Data[0],
                    b = srcImg.Data[w - 1],
                    c = srcImg.Data[(h - 1)*(srcImg.Stride)],
                    d = srcImg.Data[(h - 1)*(srcImg.Stride) + w - 1];

            // If any of them are partially transparent, assume image had an alpha channel and thus
            // colorkey is 100% transparent -- i.e., don't do anything
            if (((a & 0xFF000000)>>24) != 255 ||
                ((b & 0xFF000000)>>24) != 255 ||
                ((c & 0xFF000000)>>24) != 255 ||
                ((d & 0xFF000000)>>24) != 255)
                return 0;

            int aScore = 0, bScore = 0, cScore = 0, dScore = 0;
            aScore = ((b == a) ? 1 : 0) +
                    ((c == a) ? 1 : 0) +
                    ((d == a) ? 1 : 0);

            bScore = ((a == b) ? 1 : 0) +
                    ((c == b) ? 1 : 0) +
                    ((d == b) ? 1 : 0);

            cScore = ((a == c) ? 1 : 0) +
                    ((b == c) ? 1 : 0) +
                    ((d == c) ? 1 : 0);

            dScore = ((a == d) ? 1 : 0) +
                    ((b == d) ? 1 : 0) +
                    ((c == d) ? 1 : 0);

            if (aScore >= bScore &&
                aScore >= cScore &&
                aScore >= dScore)
                return a;
            else if (bScore >= aScore &&
                 bScore >= cScore &&
                 bScore >= dScore)
                return b;
            else if (cScore >= aScore &&
                cScore >= bScore &&
                cScore >= dScore)
                return c;
            else
                return d;
        }

        private void ColorKey(SourceImage srcImg)
        {
            // Bail if we're not colorkeying at all
            if (Settings.ColorKeyMode == ColorKeyMode.Disabled)
                return;

            int colorKey;

            // Figure out color key
            if (Settings.ColorKeyMode == ColorKeyMode.Automatic)
            {
                colorKey = DetectColorKey(srcImg);
            }
            else // specific
            {
                colorKey = Settings.ColorKeyColor.ToArgb();
            }

            if ((colorKey & 0xFF000000) == 0)
                return;

            // Replace pixels matching colorkey w/ transparent
            // (NOTE: This is probably not fast)
            for (int y = 0; y < srcImg.Height; ++y)
                for (int x = 0; x < srcImg.Width; ++x)
                {
                    int c = srcImg.Data[x + srcImg.Stride*y];
                    if (c == colorKey)
                        srcImg.Data[x + srcImg.Stride * y] = 0;
                }
        }

        // Return true if image was nonzero sized
        private bool Crop(SourceImage srcImg)
        {
            // Bail if we're not cropping
            if (!Settings.Crop)
                return true;

            bool atTop = true;
            int topBlankRows = 0;
            int bottomBlankRows = 0;
            int leftBlankCols = srcImg.Width;
            int rightBlankCols = srcImg.Height;
            bool anySolidRows = false;

            // Basically, for each row we scan left to right to find the first solid pixel.
            // If there's none, then we treat this as an empty row, and thus affect the min/max
            // Y values. Otherwise, we scan from the right to get a right bound, then
            // use this to affect the min/max X values.
            for (int y = 0; y < srcImg.Height; ++y)
            {
                // First, start counting blanks from the left.
                // If we hit a solid pixel, save its position
                int x;
                for (x = 0; x < srcImg.Width; ++x)
                {
                    int c = srcImg.Data[x + y * srcImg.Stride];
                    if ((c & 0xFF000000) != 0)
                        break;
                }

                // If the row was totally blank
                if (x == srcImg.Width)
                {
                    if (atTop)
                        topBlankRows++;
                    else
                        bottomBlankRows++;
                }
                // Otherwise, we got a solid row.
                // First solid pixel is in column 'x'
                else
                {
                    atTop = false;
                    bottomBlankRows = 0;

                    int rowLeftBlankCols = x;
                    // If we hit any solid pixels, start counting from the right to 
                    // see what the max X is
                    for (x = srcImg.Width - 1; x > rowLeftBlankCols; --x)
                    {
                        int c = srcImg.Data[x + y * srcImg.Stride];
                        if ((c & 0xFF000000) != 0)
                            break;
                    }

                    int rowRightBlankCols = srcImg.Width - x - 1;

                    leftBlankCols = Math.Min(leftBlankCols, rowLeftBlankCols);
                    rightBlankCols = Math.Min(rightBlankCols, rowRightBlankCols);
                    anySolidRows = true;
                }
            }
            
            // Now, perform the cropping crap (or at least set up the proper vars)
            if (anySolidRows)
            {
                srcImg.CropX = leftBlankCols;
                srcImg.CropY = topBlankRows;
                srcImg.CropW = srcImg.Width - leftBlankCols - rightBlankCols;
                srcImg.CropH = srcImg.Height - topBlankRows - bottomBlankRows;
                return true;
            }
            else
            {
                srcImg.CropX = 0;
                srcImg.CropY = 0;
                srcImg.CropW = 0;
                srcImg.CropH = 0;
                return false;
            }    
        }

        private string EscapeXML(string s)
        {
            return System.Security.SecurityElement.Escape(s);
        }

        private void SaveXML()
        {
            const string NL = "\r\n";
            string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + NL;
            xml += String.Format("<cram-output version=\"1\" pages=\"{0}\">" + NL,
                                    pages.Count);

            int n=0;
            foreach (Page p in pages)
            {
                xml += String.Format("  <page id=\"{0}\" src=\"{1}\" size=\"{2},{3}\"/>" + NL,
                                        n,
                                        EscapeXML(pageFilenames[n]),
                                        p.Width,
                                        p.Height);
                ++n;
            }

            foreach (SourceImage img in SourceImages)
            {
                string src = img.ShortName;
                SourceImage actualImg = img;
                if (img.DuplicateOf != null)
                    actualImg = img.DuplicateOf;
                if (actualImg.Rect == null)
                {
                    xml += String.Format("  <!-- {0} failed -->" + NL,
                                        EscapeXML(src));
                }
                else
                {
                    xml += String.Format("  <frame src=\"{0}\" page=\"{10}\" pos=\"{1},{2}\" size=\"{3},{4}\" " +
                                        "offset=\"{5},{6}\" srcSize=\"{7},{8}\" rotated=\"{9}\"/>" + NL,
                                        EscapeXML(src),
                                        actualImg.Rect.X,
                                        actualImg.Rect.Y,
                                        actualImg.Rect.W,
                                        actualImg.Rect.H,
                                        actualImg.CropX,
                                        actualImg.CropY,
                                        actualImg.Width,
                                        actualImg.Height,
                                        actualImg.Rect.Rotated ? "1" : "0",
                                        actualImg.PageId);
                }

            }

            xml += "</cram-output>" + NL;

            try
            {
                using (StreamWriter writer = new StreamWriter(Settings.XmlFilename))
                {
                    writer.Write(xml);
                }
            }
            catch (Exception)
            {
                Log("Error saving file: {0}", Settings.XmlFilename);
                FatalError();
            }
        }

        private Bitmap DataToBitmap(SourceImage image)
        {
            Bitmap bmp = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);

            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                                        ImageLockMode.WriteOnly,
                                        PixelFormat.Format32bppArgb);

            System.Runtime.InteropServices.Marshal.Copy(image.Data, 0, data.Scan0, image.Width*image.Height);
            bmp.UnlockBits(data);

            return bmp;
        }

        private void WritePage(string filename, Page page)
        {
            Bitmap pageBmp = new Bitmap(page.Width, page.Height, PixelFormat.Format32bppArgb);

            using (Graphics graphics = Graphics.FromImage(pageBmp))
            {
                graphics.Clear(Settings.BackgroundColor);

                foreach (Rect rect in page.FittedRects)
                {
                    SourceImage srcImg = (SourceImage)rect.Tag;
                    Bitmap frameBmp = DataToBitmap(srcImg);

                    int cropX = srcImg.CropX,
                        cropY = srcImg.CropY,
                        cropW = srcImg.CropW,
                        cropH = srcImg.CropH;

                    if (rect.Rotated)
                    {
                        frameBmp.RotateFlip(RotateFlipType.Rotate270FlipNone);

                        cropX = srcImg.CropY;
                        cropY = srcImg.Width - srcImg.CropX - srcImg.CropW;

                        cropW = srcImg.CropH;
                        cropH = srcImg.CropW;
                    }

                    Rectangle srcRect = new Rectangle(cropX, cropY, cropW, cropH);
                    graphics.DrawImage(frameBmp, rect.X, rect.Y, srcRect, GraphicsUnit.Pixel);

                    frameBmp.Dispose();
                }
            }

            try
            {
                pageBmp.Save(filename);
            }
            catch (Exception)
            {
                Log("Error saving file: {0}", filename);
                FatalError();
            }
            finally
            {
                pageBmp.Dispose();
            }
        }

        private void DumpImage(SourceImage srcImg)
        {
            return;
            /*
            Bitmap bmp = new Bitmap(srcImg.CropW, srcImg.CropH, PixelFormat.Format32bppArgb);
            for(int x=0; x<srcImg.CropW; ++x)
                for (int y = 0; y < srcImg.CropH; ++y)
                {
                    int sx = x + srcImg.CropX;
                    int sy = y + srcImg.CropY;
                    int color = srcImg.Data[sx + sy*srcImg.Stride];
                    bmp.SetPixel(x, y, Color.FromArgb(color));
                }
            bmp.Save(@"c:\tmp\cram\" + srcImg.ShortName);
            */
        }

        private string ComputeHash(SourceImage srcImg)
        {

            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            // NOTE: This assumes that stride is always equal to width and thus
            // rows don't have junk in them

            // TODO: Note that this is a bit of a waste of time :(
            // I miss pointer casts, maybe I can learn to do it with 'unsafe'...
            byte[] buf = new byte[srcImg.CropW * srcImg.CropH * 4];

            // Only convert the area inside the crop rect
            int i = 0;
            for (int x=0; x < srcImg.CropW; ++x)
                for (int y = 0; y < srcImg.CropH; ++y)
                {
                    int col = srcImg.Data[srcImg.CropX + x + (srcImg.CropY + y)*srcImg.Stride];
                    // Treat blank pixels as equal
                    if (((col & 0xFF000000) >> 24) == 0)
                    {
                        buf[i * 4] = buf[i * 4 + 1] = buf[i * 4 + 2] = buf[i * 4 + 3] = 0;
                    }
                    else
                    {
                        buf[i * 4] = (byte)(col & 0x000000FF);
                        buf[i * 4 + 1] = (byte)((col & 0x0000FF00) >> 8);
                        buf[i * 4 + 2] = (byte)((col & 0x00FF0000) >> 16);
                        buf[i * 4 + 3] = (byte)((col & 0xFF000000) >> 24);
                    }
                    ++i;
                }

            byte[] hash = provider.ComputeHash(buf);

            StringBuilder builder = new StringBuilder();
            foreach (byte b in hash)
                builder.AppendFormat("{0:x2}", b);
            return builder.ToString();
            /*
            Log("   MD5: {0}", builder.ToString());
            Log("   Pixels: {0}", i);
            */
        }

        private void Log(string format)
        {
            if (LogEvent != null)
                LogEvent(format);
        }

        private void Log(string format, params object[] args)
        {
            if (LogEvent != null)
                LogEvent(String.Format(format, args));
        }
    }
}
