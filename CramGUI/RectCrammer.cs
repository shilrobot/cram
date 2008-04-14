using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Diagnostics;


namespace Cram
{
    public class RectCramOptions
    {
        public int PageW;
        public int PageH;
        public int Border;
        public bool Rotate;
        public bool ExtraTight;
        public bool EnableFallbacks;
        public int FallbackCount;
        public List<Size> FallbackSizes = new List<Size>();
        public event ProgressLog LogEvent;
        public int DebugSaveImageIndex = 0;

        public void Log(string format)
        {
            if (LogEvent != null)
                LogEvent(format);
        }

        public void Log(string format, params object[] args)
        {
            if (LogEvent != null)
                LogEvent(String.Format(format, args));
        }
    }

    public class Rect
    {
        public object Tag;
        public int X;
        public int Y;
        public bool Rotated = false;
        public int W;
        public int H;

        public Rect(int x, int y, int w, int h)
        {
            X = x;
            Y = y;
            W = w;
            H = h;
        }

        public Rect(int x, int y, int w, int h, object tag)
        {
            X = x;
            Y = y;
            W = w;
            H = h;
            Tag = tag;
        }

        public Rect CloneRect()
        {
            Rect r = new Rect(X, Y, W, H, Tag);
            r.Rotated = Rotated;
            return r;
        }

        public int RotW { get { return Rotated ? H : W; } }
        public int RotH { get { return Rotated ? W : H; } }
    }

    public class Page
    {
        public int Width;
        public int Height;
        public List<Rect> InputRects = new List<Rect>();
        public List<Rect> FittedRects;
        public List<Rect> UnfittedRects;
        public int UsedPixels;
        public int WastePixels;
        public int TotalPixels;
        public float UsagePercent;
    }

    public class FallbackSet
    {
        public List<Page> Pages = new List<Page>();
        public int WastePixels = 0;
    }

    public class RectCrammer
    {
        private static List<Rect> CloneRects(List<Rect> rects)
        {
            List<Rect> copyRects = new List<Rect>(rects.Count);
            foreach (Rect r in rects)
                copyRects.Add(r.CloneRect());
            return copyRects;
        }
        
        private static int CompareRects(Rect a, Rect b)
        {
            Debug.Assert(a != null);
            Debug.Assert(b != null);

            // Sort first by height descending, then by width descending.
            if(a.RotH > b.RotH)
                return -1;
            else if(a.RotH < b.RotH)
                return 1;
            
            if(a.RotW > b.RotW)
                return -1;
            else if(a.RotW < b.RotW)
                return 1;

            return 0;
        }

        private static Rect GetNextBestRect(List<Rect> rects, int w, int h, bool rotate)
        {
            // Look for all rects that can fit given our width, height and rotation flag
            List<Rect> possibleRects = new List<Rect>();
            foreach(Rect r in rects)
            {
                bool fitsNormal = (r.W <= w && r.H <= h);
                bool fitsRotated = rotate && (r.H <= w && r.W <= h);

                if(fitsNormal && fitsRotated)
                {
                    // Fits both ways, do longest dimension first if we can rotate
                    r.Rotated = rotate && (r.W > r.H);
                    possibleRects.Add(r);
                }
                else if(fitsNormal && !fitsRotated)
                {
                    r.Rotated = false;
                    possibleRects.Add(r);
                }
                else if(!fitsNormal && fitsRotated)
                {
                    r.Rotated = true;
                    possibleRects.Add(r);
                }
            }

            // If there are no good fits, bail
            if(possibleRects.Count == 0)
                return null;

            possibleRects.Sort(CompareRects);
            return possibleRects[0];
        }

        private static void DrawDebug(List<Rect> fitted, List<Rect> emptyRects, int x, int y, int w, int h, RectCramOptions opts)
        {
            Bitmap bmp = new Bitmap(opts.PageW, opts.PageH);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.Black);

            Pen whitePen = new Pen(Color.White);
            Brush blueBrush = new SolidBrush(Color.Blue);
            Brush greenBrush = new SolidBrush(Color.Green);
            Brush redBrush = new SolidBrush(Color.Red);

            g.FillRectangle(blueBrush, x, y, w, h);

            foreach (Rect r in emptyRects)
            {
                g.FillRectangle(redBrush, r.X, r.Y, r.W, r.H);
                g.DrawRectangle(whitePen, r.X, r.Y, r.W, r.H);
            }

            foreach (Rect r in fitted)
            {
                g.FillRectangle(greenBrush, r.X, r.Y, r.RotW, r.RotH);
                g.DrawRectangle(whitePen, r.X, r.Y, r.RotW, r.RotH);
            }

            bmp.Save(String.Format("c:\\tmp\\cramdebug{0}.png", opts.DebugSaveImageIndex));
            ++opts.DebugSaveImageIndex;

        }

        private static void Pack(List<Rect> rects,
                                int x, int y, int w, int h,
                                RectCramOptions opts,
                                out List<Rect> fitted,
                                out List<Rect> unfitted)
        {
            Debug.Assert(rects.Count > 0, "Rectangle count must be greater than zero");
            Debug.Assert(x >= 0, "X offset must be >= 0");
            Debug.Assert(y >= 0, "Y offset must be >= 0");
            Debug.Assert(w > 0, "Width must be > 0");
            Debug.Assert(h > 0, "Width must be > 0");
            Debug.Assert(opts.Border >= 0, "Border must be > 0");

            // cx and cy are relative to x&y
            int cx = 0, cy = 0;

            rects = CloneRects(rects);

            List<Rect> possibleRects = new List<Rect>();
            fitted = new List<Rect>();
            unfitted = new List<Rect>();

            // Search for rects that can't possibly fit,
            // and add them to the unfit list. Add the ones that can maybe
            // fit to the fitted list.
            foreach (Rect r in rects)
            {
                bool fits = false;

                // If it can never fit normally
                if (r.W > w || r.H > h)
                {
                    // Can it fit rotated, and rotation is enabled?
                    if (opts.Rotate && r.H <= w && r.W <= h)
                        fits = true;
                    else
                        fits = false;
                }
                else
                    fits = true;

                if (fits)
                    possibleRects.Add(r);
                else
                    unfitted.Add(r);
            }

            // Loop until all possible rects are exhausted...
            // Note that we break if we ever can't start a new row.
            while (possibleRects.Count > 0)
            {
                // We ran out of space due to borders
                if (h - cy <= 0)
                    break;

                // TODO: Extra tight packing

                List<Rect> rowRects = new List<Rect>();

                // Get the first rect for this row
                Rect best = GetNextBestRect(possibleRects, w, h - cy, opts.Rotate);
                if (best == null)
                    break;

                // Fit in the best one first
                best.X = cx + x;
                best.Y = cy + y;
                int rowHeight = best.RotH;
                int rowWidthLeft = w - (best.RotW + opts.Border);
                cx += best.RotW + opts.Border;
                fitted.Add(best);
                possibleRects.Remove(best);
                rowRects.Add(best);

                // Now, fit in the rest in the row...
                while (true)
                {
                    // rowWidthLeft may be NEGATIVE due to borders!
                    if (rowWidthLeft <= 0)
                        break;

                    best = GetNextBestRect(possibleRects, rowWidthLeft, rowHeight, opts.Rotate);
                    if (best == null)
                        break;
                    best.X = cx + x;
                    best.Y = cy + y;
                    rowWidthLeft -= best.RotW + opts.Border;
                    cx += best.RotW + opts.Border;
                    fitted.Add(best);
                    possibleRects.Remove(best);
                    rowRects.Add(best);
                }

                if (opts.ExtraTight)
                {
                    List<Rect> emptyRects = new List<Rect>();

                    // Make a list of empty rects from left to right.
                    // Empty rects are areas in the row that are totally unused,
                    // on the bottom right side.
                    for (int i = 0; i < rowRects.Count-1; ++i)
                    {
                        Rect leftRect = rowRects[i];
                        Rect rightRect = rowRects[i + 1];

                        int emptyLeft = leftRect.X + leftRect.RotW + opts.Border;
                        int emptyTop = rightRect.Y + rightRect.RotH + opts.Border;
                        int emptyRight = x + w;
                        int emptyBottom = (y + cy) + rowHeight;

                        // skip degenerate rects
                        if (emptyLeft >= emptyRight ||
                            emptyTop >= emptyBottom)
                            continue;

                        Rect emptyRect = new Rect(emptyLeft,
                                                    emptyTop,
                                                    emptyRight - emptyLeft,
                                                    emptyBottom - emptyTop);
                        emptyRects.Add(emptyRect);
                    }

                    // Reverse order of rectangles
                    emptyRects.Reverse();

                    int rightEdge = x + w;

                    // Now, fit stuff...
                    foreach (Rect emptyRect in emptyRects)
                    {
                        if (possibleRects.Count == 0)
                            break;

                        Rect croppedRect = emptyRect;
                        if (croppedRect.X + croppedRect.W > rightEdge)
                        {
                            croppedRect.W = rightEdge - croppedRect.X;
                            if (croppedRect.W <= 0)
                                continue;
                        }

                        List<Rect> fitted2 = new List<Rect>();
                        List<Rect> unfitted2 = new List<Rect>();

                        Pack(possibleRects,
                                croppedRect.X,
                                croppedRect.Y,
                                croppedRect.W,
                                croppedRect.H,
                                opts,
                                out fitted2,
                                out unfitted2);

                        // Flip the rects so they fill from right to left
                        foreach (Rect r in fitted2)
                        {
                            int leftSide = emptyRect.X;
                            int rightSide = emptyRect.X + emptyRect.W;
                            int offsetFromLeft = r.X - emptyRect.X;
                            r.X = rightSide - offsetFromLeft - r.RotW;
                        }

                        // Readjust right edge
                        foreach (Rect r in fitted2)
                        {
                            rightEdge = Math.Min(rightEdge, r.X - opts.Border);
                        }

                        fitted.AddRange(CloneRects(fitted2));
                        possibleRects = CloneRects(unfitted2);
                    }

                    //DrawDebug(fitted, emptyRects, x, y, w, h, opts);
                }


                // Next row time
                cy += rowHeight + opts.Border;
                cx = 0;
            }

            // All rects that were possible but we couldn't find space for now become unfit rects
            unfitted.AddRange(possibleRects);
        }

        private static Page MakePackedPage(List<Rect> rects, int w, int h, RectCramOptions opts)
        {
            Debug.Assert(rects.Count > 0, "Rectangle count must be greater than zero");
            Debug.Assert(w > 0, "Width must be greater than zero");
            Debug.Assert(h > 0, "Height must be greater than zero");
            Debug.Assert(opts.Border >= 0, "Border must be greater than or equal to zero");

            Page page = new Page();
            page.InputRects = rects;
            page.Width = w;
            page.Height = h;

            // Don't even bother packing if border is too big
            if (opts.Border * 2 >= page.Width ||
                opts.Border * 2 >= page.Height)
            {
                page.FittedRects = new List<Rect>();
                page.UnfittedRects = CloneRects(page.InputRects);
            }
            else
            {
                RectCrammer.Pack(rects,
                                opts.Border,
                                opts.Border,
                                page.Width - opts.Border*2,
                                page.Height - opts.Border*2,
                                opts,
                                out page.FittedRects,
                                out page.UnfittedRects);
            }

            Debug.Assert(page.FittedRects.Count + page.UnfittedRects.Count == page.InputRects.Count,
                            "Rectangle count sanity check failed");

            page.TotalPixels = page.Width * page.Height;
            page.UsedPixels = 0;
            foreach (Rect r in page.FittedRects)
                page.UsedPixels += r.W * r.H;
            page.WastePixels = page.TotalPixels - page.UsedPixels;

            Debug.Assert(page.WastePixels <= page.TotalPixels, "Waste pixels sanity check failed");
            Debug.Assert(page.UsedPixels <= page.TotalPixels, "Used pixels sanity check failed");
            Debug.Assert(page.WastePixels + page.UsedPixels == page.TotalPixels, "Total pixels sanity check failed");

            page.UsagePercent = 100.0f * (float)page.UsedPixels / (float)page.TotalPixels;

            Debug.Assert(page.UsagePercent >= 0.0f && page.UsagePercent <= 100.0f);

            return page;
        }
        
        // This is probably a sort of silly and inefficient wany of generating these, but it's
        // not that hard to write and we only want like 1-3 iterations.
        public static List<List<Size>> GenerateFallbackSets(List<Size> sortedSizes, int depth)
        {
            Debug.Assert(depth > 0, "depth must be > 0");

            List<List<Size>> results = new List<List<Size>>();

            // (We have to know n)
            for(int n=0; n < sortedSizes.Count; ++n)
            {
                Size size = sortedSizes[n];

                if (depth == 1)
                {
                    // Just return a list like [[A], [B], [C]]
                    List<Size> tmp = new List<Size>();
                    tmp.Add(size);
                    results.Add(tmp);
                }
                else
                {
                    List<Size> subset = sortedSizes.GetRange(n, sortedSizes.Count - n);
                    List<List<Size>> subFallbackSets = GenerateFallbackSets(subset, depth - 1);

                    foreach (List<Size> subFallbackSet in subFallbackSets)
                    {
                        List<Size> tmp = new List<Size>();
                        tmp.Add(size);
                        tmp.AddRange(subFallbackSet);
                        results.Add(tmp);
                    }
                }
            }

            return results;
        }

        // Tries to pack given a preset list of fallback sizes.
        // Returns true if we fit everything, false otherwise.
        // TODO: Do we need to return unfitted rects too here?
        //       We need to distinguish between rects that *can't* ever fit on max page size,
        //       and rects that won't fit because we have too small/few fallback pages.
        // IDEA: I think we have this information once we finish normal packing -- we just want
        //       to re-pack the last page's fittedrects, and ignore any unfit rects left over.
        public static bool PackFallbacks(List<Rect> rects, List<Size> pageSizes,
                                                RectCramOptions opts,
                                                out List<Page> resultPages)
        {
            Debug.Assert(rects.Count > 0, "Rect count must be greater than zero");
            Debug.Assert(pageSizes.Count > 0, "Page Sizes list must have at least one entry");

            resultPages = new List<Page>();

            List<Rect> unfitted = CloneRects(rects);
            foreach (Size pageSize in pageSizes)
            {
                Debug.Assert(pageSize.Width > 0);
                Debug.Assert(pageSize.Height > 0);

                Page page = MakePackedPage(unfitted, pageSize.Width, pageSize.Height, opts);
                unfitted = CloneRects(page.UnfittedRects);
                
                // Something didn't fit in the page at all, we better stop or risk looping forever
                if (page.FittedRects.Count == 0)
                    return false;

                resultPages.Add(page);

                if(unfitted.Count == 0)
                    break;
            }

            if(unfitted.Count == 0)
                return true;
            else
                return false;
        }


        private static int CompareFallbackSets(FallbackSet a, FallbackSet b)
        {
            // First sort by waste pixels, ascending
            if (a.WastePixels < b.WastePixels)
                return -1;
            else if (a.WastePixels > b.WastePixels)
                return 1;

            // Then sort by # of pages, ascending
            if (a.Pages.Count < b.Pages.Count)
                return -1;
            else if(a.Pages.Count > b.Pages.Count)
                return 1;

            return 0;
        }

        // Returns a list with duplicate elements removed.
        // Maintains the relative ordering of the first entries among non-unique values.
        private static List<T> Uniquify<T>(List<T> input)
        {
            Dictionary<T, int> unique = new Dictionary<T, int>();
            List<T> result = new List<T>();

            foreach (T entry in input)
            {
                if (unique.ContainsKey(entry))
                    continue;
                else
                {
                    unique.Add(entry, 0);
                    result.Add(entry);
                }
            }

            return result;
        }

        public static List<Page> PackMultiplePages(List<Rect> rects, RectCramOptions opts, out List<Rect> unfitted)
        {
            Debug.Assert(rects.Count > 0, "Rect count must be greater than zero");
            Debug.Assert(opts.PageW > 0, "Page width must be greater than zero");
            Debug.Assert(opts.PageH > 0, "Page height must be greater than zero");
            Debug.Assert(opts.Border >= 0, "Border size must be greater or equal to zero");

            List<Page> pages = new List<Page>();

            //----------------------------------------------------------------
            // BASIC PACKING
            //----------------------------------------------------------------
            unfitted = CloneRects(rects);
            while (unfitted.Count > 0)
            {
                Page page = MakePackedPage(unfitted, opts.PageW, opts.PageH, opts);
                unfitted = CloneRects(page.UnfittedRects);

                // Something didn't fit in the page at all, we better stop or risk looping forever
                if (page.FittedRects.Count == 0)
                    break;

                pages.Add(page);
            }

            return pages;
        }

        public static List<Page> PackMultiplePagesWithFallback(List<Rect> rects, RectCramOptions opts, out List<Rect> unfitted)
        {
            List<Page> pages = PackMultiplePages(rects, opts, out unfitted);

            // Exit early if there's no reason to do fallbacks
            if (!opts.EnableFallbacks || pages.Count == 0)
            {
                opts.Log("Not calculating fallbacks");
                return pages;
            }

            opts.Log("Calculating fallbacks...");

            // Sanity checks, setup, etc.
            Debug.Assert(opts.FallbackCount > 0, "FallbackCount must be > 0");
            Debug.Assert(opts.FallbackSizes != null, "FallbackSizes cannot be null");
            Debug.Assert(opts.FallbackSizes.Count > 0, "FallbackSizes must have > 0 items");

            foreach (Size size in opts.FallbackSizes)
            {
                Debug.Assert(size.Width > 0, "Fallback width must be > 0");
                Debug.Assert(size.Height > 0, "Fallback height must be > 0");
            }

            Page lastPage = pages[pages.Count - 1];
            List<Rect> fallbackRects = CloneRects(lastPage.FittedRects);

            // Compute required pixels (to avoid computing packing for fallback sets that could never work)
            int pixelsRequired = 0;
            foreach (Rect r in fallbackRects)
                pixelsRequired += r.W * r.H;
            // DISABLED LOGGING: opts.Log("Pixels required = {0}", pixelsRequired);

            // TODO: Do something if a fallback is a duplicate of default page size,
            //       or exceeds default page size, or something?

            // Remove duplicate fallback sizes & sort fallbacks by area
            List<Size> sortedFallbacks = Uniquify(opts.FallbackSizes);
            sortedFallbacks.Sort(Util.CompareFallbackSizes);

            // Generate fallback size sets (a list of lists of sizes)
            List<List<Size>> fallbackSetsListForm = GenerateFallbackSets(sortedFallbacks, opts.FallbackCount);
            Debug.Assert(fallbackSetsListForm.Count > 0, "Should be > 0 fallback sets generated");

            // Now, make proper fallback sets, fitted & all
            List<FallbackSet> fallbackSets = new List<FallbackSet>();

            // Add one fallback for just the last page as it is -- if this is the best
            // option, we will use it. Note that we don't have to repack it!
            FallbackSet defaultFB = new FallbackSet();
            defaultFB.Pages.Add(lastPage);
            defaultFB.WastePixels = lastPage.WastePixels;
            // DISABLED LOGGING: opts.Log("Default option waste = {0}", defaultFB.WastePixels);
            fallbackSets.Add(defaultFB);

            // Compute packing for the rest
            foreach (List<Size> fallbackSizes in fallbackSetsListForm)
            {
                StringBuilder builder = new StringBuilder();
                foreach (Size size in fallbackSizes)
                    builder.Append(String.Format("{2}{0}x{1}",
                                                size.Width,
                                                size.Height,
                                                builder.Length > 0 ? ", " : ""));
                //opts.Log("Trying fallback list: [{0}]", builder.ToString());

                // Compute total pixels in this fallback set
                int totalPixels = 0;
                foreach (Size size in fallbackSizes)
                    totalPixels += size.Width * size.Height;
                // DISABLED LOGGING: opts.Log("    Total pixels = {0}", totalPixels);

                // Skip ones that can't possibly hold required number of pixels
                // This is rough but it works basically OK
                // If we were more sophisticated we would take borders into account, too...
                if (pixelsRequired > totalPixels)
                {
                    // DISABLED LOGGING: opts.Log("    Cannot possibly fit required pixels, aborting", totalPixels);
                    continue;
                }

                FallbackSet fallbackSet = new FallbackSet();

                // Perform packing loop, etc.
                List<Rect> fbUnfitted = CloneRects(fallbackRects);

                // TODO: Goddammit, use our rect sizes here instead! #$#@!~
                int n = 0;
                while (fbUnfitted.Count > 0 && n < fallbackSizes.Count)
                {
                    Size size = fallbackSizes[n];
                    Page page = MakePackedPage(fbUnfitted, size.Width, size.Height, opts);
                    fbUnfitted = CloneRects(page.UnfittedRects);

                    // Something didn't fit in the page at all, we better stop or risk looping forever
                    if (page.FittedRects.Count == 0)
                        break;

                    fallbackSet.Pages.Add(page);
                    ++n;
                }
                //fallbackSet.Pages = PackMultiplePages(fbUnfitted, opts, out fbUnfitted);

                if (fbUnfitted.Count > 0 || fallbackSet.Pages.Count == 0)
                {
                    // DISABLED LOGGING: opts.Log("    Packing failed, aborting");
                    continue;
                }

                // Compute waste
                int waste = 0;
                foreach (Page p in fallbackSet.Pages)
                    waste += p.WastePixels;
                fallbackSet.WastePixels = waste;
                // DISABLED LOGGING: opts.Log("    Waste pixels = {0}", waste);

                fallbackSets.Add(fallbackSet);
            }

            // Now, we have to rank fallbacks
            fallbackSets.Sort(CompareFallbackSets);

            // Whee, best fallback set is the winner
            FallbackSet winner = fallbackSets[0];
            {
                StringBuilder builder = new StringBuilder();
                foreach (Page p in winner.Pages)
                    builder.Append(String.Format("{2}{0}x{1}",
                                                    p.Width,
                                                    p.Height,
                                                    builder.Length > 0 ? ", " : ""));
                opts.Log("Winner: [{0}]", builder.ToString());
            }

            // Remove last page & replace with fallback result page
            List<Page> result = new List<Page>(pages);
            result.Remove(lastPage);
            result.AddRange(winner.Pages);
            return result;
        }
    }

}
