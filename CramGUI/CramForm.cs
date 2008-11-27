using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Drawing.Drawing2D;

namespace Cram
{
    public partial class CramForm : Form
    {
        private ImageList imageList;
        private Color colorKeyColor = Color.Magenta;
        private Color backgroundColor = Color.Black;
        private List<Size> fallbackSizes = new List<Size>();

        public CramForm()
        {
            InitializeComponent();
            openFileDialog.Filter = "Image files (*.png,*.bmp,*.jpg,*.jpeg)|*.bmp;*.gif;*.jpg;*.jpeg;*.png|All files (*.*)|*.*";
            imageList = new ImageList();
            inputImageList.SmallImageList = imageList;

            fallbackSizes.Add(new Size(256, 256));
            fallbackSizes.Add(new Size(128, 128));
            fallbackSizes.Add(new Size(64, 64));
            fallbackSizes.Add(new Size(32, 32));
            UpdateFallbackList();

            RecalculateInputImageStats();
            UpdateUI();
        }

        private bool ThumbnailAbort() { return false; }

        private SizeF GetThumbnailSize(Size input)
        {
            if (input.Width == 0 || input.Height == 0)
                return new Size(0, 0);
            if (input.Width >= input.Height)
            {
                SizeF result = new Size();
                result.Width = 16;
                result.Height = 16.0f * (float)input.Height / (float)input.Width;
                return result;
            }
            else
            {
                SizeF result = new Size();
                result.Width = 16.0f * (float)input.Width / (float)input.Height;
                result.Height = 16;
                return result;
            }
        }

        // Creates a thumbnail with the right colored background (not a weird blue border)
        // and with the proper aspect ratio
        private Image MakeBetterThumbnail(Image source)
        {
            SizeF thumbSize = GetThumbnailSize(new Size(source.Width, source.Height));
            PointF destOffset = new PointF((16 - thumbSize.Width) / 2.0f,
                                        (16 - thumbSize.Height) / 2.0f);
            RectangleF destRect = new RectangleF(destOffset.X, destOffset.Y, thumbSize.Width, thumbSize.Height);

            Bitmap bmp = new Bitmap(16, 16);
            Graphics g = Graphics.FromImage(bmp);
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            g.Clear(SystemColors.Window);
            g.DrawImage(source,
                        destRect);
            return bmp;
        }

        private bool AddInputImage(string filename)
        {
            Image img;
            long size;

            try
            {
                size = new FileInfo(filename).Length;
                img = Image.FromFile(filename);
            }
            catch (Exception)
            {
                return false;
            }

            int w = img.Width,
                h = img.Height;

            if (w <= 0 || h <= 0)
                return false;

            //Image img2 = img.GetThumbnailImage(16, 16, ThumbnailAbort, IntPtr.Zero);
            Image img2 = MakeBetterThumbnail(img);
            imageList.Images.Add(img2);

            InputImageTag tag = new InputImageTag();
            tag.Filename = filename;
            tag.Width = w;
            tag.Height = h;
            tag.FileSize = (ulong)size;

            ListViewItem item = new ListViewItem();
            item.Tag = tag;
            item.Text = Path.GetFileName(filename);
            item.ImageIndex = imageList.Images.Count - 1;
            item.SubItems.Add(String.Format("{0}x{1}", w, h));
            item.SubItems.Add(String.Format("{0}", Util.FormatBytes((ulong)size)));
            inputImageList.Items.Add(item);

            img.Dispose();
            return true;
        }

        private void addInputImageButton_Click(object sender, EventArgs e)
        {
            openFileDialog.FileName = "";
            DialogResult result = openFileDialog.ShowDialog();
            if (result != DialogResult.OK)
                return;

            // TODO: Show some kind of results dialog if errors occurred

            foreach (string filename in openFileDialog.FileNames)
            {
                AddInputImage(filename);
            }

            RecalculateInputImageStats();
            UpdateUI();
        }

        private void removeInputImageButton_Click(object sender, EventArgs e)
        {
            LinkedList<ListViewItem> sel = new LinkedList<ListViewItem>();
            foreach (ListViewItem item in inputImageList.SelectedItems)
                sel.AddLast(item);
            foreach (ListViewItem item in sel)
                inputImageList.Items.Remove(item);

            RecalculateInputImageStats();
            UpdateUI();
        }

        private void RemoveAllInputImages()
        {
            inputImageList.Items.Clear();
            RecalculateInputImageStats();
            UpdateUI();
            imageList.Images.Clear();
        }

        private void removeAllInputImagesButton_Click(object sender, EventArgs e)
        {
            if (inputImageList.Items.Count > 0 &&
                MessageBox.Show("OK to remove all?", "Remove All", MessageBoxButtons.OKCancel) == DialogResult.OK)
                RemoveAllInputImages();
        }


        private void inputImageList_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateUI();
        }


        private void RecalculateInputImageStats()
        {
            ulong pixels = 0;
            ulong size = 0;

            foreach(ListViewItem item in inputImageList.Items)
            {
                InputImageTag tag = (InputImageTag)item.Tag;
                pixels += (ulong)tag.Width * (ulong)tag.Height;
                size += tag.FileSize;
            }

            int count = inputImageList.Items.Count;
            inputImagesStatsLabel.Text = String.Format("{0} image{1} - {2} @ 32 bpp - {3} on disk",
                                                        count,
                                                        count == 1 ? "" : "s",
                                                        Util.FormatBytes(pixels*4),
                                                        Util.FormatBytes(size));
        }

        private void UpdateUI()
        {
            removeInputImageButton.Enabled = inputImageList.SelectedItems.Count > 0;
            removeAllInputImagesButton.Enabled = inputImageList.Items.Count > 0;

            colorKeyChooser.BackColor = colorKeyColor;
            backgroundColorChooser.BackColor = backgroundColor;                
        }

        private void specificCKMode_CheckedChanged(object sender, EventArgs e)
        {
            UpdateUI();
        }


        private void colorKeyChooser_Click(object sender, EventArgs e)
        {
            using (ColorDialog dialog = new ColorDialog())
            {
                dialog.Color = colorKeyColor;
                dialog.FullOpen = true;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    colorKeyColor = dialog.Color;
                    UpdateUI();
                }
            }
        }

        private void backgroundColorChooser_Click(object sender, EventArgs e)
        {
            using (ColorDialog dialog = new ColorDialog())
            {
                dialog.Color = backgroundColor;
                dialog.FullOpen = true;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    backgroundColor = dialog.Color;
                    UpdateUI();
                }
            }
        }

        private void SetControlsFromSettings(CramSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");

            // TODO: Show errors if images don't load

            List<string> errored = new List<string>();
            RemoveAllInputImages();
            foreach (string s in settings.SourceImages)
            {
                if (!AddInputImage(s))
                    errored.Add(s);
            }

            if (errored.Count > 0)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(String.Format("Error loading {0} images:", errored.Count));
                foreach (string s in errored)
                {
                    builder.Append(String.Format("\n{0}", s));
                }
                MessageBox.Show(builder.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            specificCKMode.Checked = (settings.ColorKeyMode == ColorKeyMode.Manual);
            automaticCKMode.Checked = (settings.ColorKeyMode == ColorKeyMode.Automatic);
            disableCKMode.Checked = (settings.ColorKeyMode == ColorKeyMode.Disabled);
            colorKeyColor = settings.ColorKeyColor;

            spritePageSizeCombo.Text = String.Format("{0}x{1}",
                                                        settings.PageSize.Width,
                                                        settings.PageSize.Height);

            backgroundColor = Color.FromArgb(255,
                                            settings.BackgroundColor.R,
                                            settings.BackgroundColor.G,
                                            settings.BackgroundColor.B);
            alphaUpDown.Value = settings.BackgroundColor.A;

            borderSizeUpDown.Value = settings.Border;

            enableFallbackCheck.Checked = settings.AllowFallbacks;
            fallbackPagesUpDown.Value = settings.MaxFallbacks;

            fallbackSizes = settings.FallbackSizes;
            UpdateFallbackList();

            cropCheck.Checked = settings.Crop;
            uniqueCheck.Checked = settings.Unique;
            rotateCheck.Checked = settings.Rotate;

            outputFilenameEdit.Text = settings.XmlFilename;

            shortNameMode.Checked = (settings.PathMode == PathMode.Short);
            fullPathMode.Checked = (settings.PathMode == PathMode.Full);
            relativePathMode.Checked = (settings.PathMode == PathMode.Relative);

            relativePathEdit.Text = settings.RelativePathBase;

            cleanCheck.Checked = settings.Clean;

            RecalculateInputImageStats();
            UpdateUI();
        }

        private bool GetSettingsFromControls(out CramSettings settings)
        {
            settings = new CramSettings();

            foreach (ListViewItem item in inputImageList.Items)
            {
                InputImageTag tag = (InputImageTag)item.Tag;
                settings.SourceImages.Add(tag.Filename);
            }

            if (specificCKMode.Checked)
            {
                settings.ColorKeyMode = ColorKeyMode.Manual;
                settings.ColorKeyColor = colorKeyColor;
            }
            else if (automaticCKMode.Checked)
            {
                settings.ColorKeyMode = ColorKeyMode.Automatic;
                settings.ColorKeyColor = colorKeyColor;
            }
            else
            {
                settings.ColorKeyMode = ColorKeyMode.Disabled;
                settings.ColorKeyColor = colorKeyColor;
            }

            if (!Util.ParseSize(spritePageSizeCombo.Text, out settings.PageSize))
            {
                MessageBox.Show(String.Format("Invalid sprite page size: \"{0}\"", spritePageSizeCombo.Text));
                return false;
            }

            settings.BackgroundColor =
                Color.FromArgb((int)alphaUpDown.Value, backgroundColor.R, backgroundColor.G, backgroundColor.B);

            settings.Border = (int)borderSizeUpDown.Value;

            settings.AllowFallbacks = enableFallbackCheck.Checked;
            settings.MaxFallbacks = (int)fallbackPagesUpDown.Value;
            settings.FallbackSizes = fallbackSizes;

            settings.Crop = cropCheck.Checked;
            settings.Unique = uniqueCheck.Checked;
            settings.Rotate = rotateCheck.Checked;

            settings.XmlFilename = outputFilenameEdit.Text;

            if (shortNameMode.Checked)
            {
                settings.PathMode = PathMode.Short;
                settings.RelativePathBase = "";
            }
            else if (fullPathMode.Checked)
            {
                settings.PathMode = PathMode.Full;
                settings.RelativePathBase = "";
            }
            else //if (relativePathMode.Checked)
            {
                settings.PathMode = PathMode.Relative;
                settings.RelativePathBase = relativePathEdit.Text;
            }

            settings.Clean = cleanCheck.Checked;

            return true;
        }

        private void generateButton_Click(object sender, EventArgs e)
        {
            CramSettings settings;
            if (!GetSettingsFromControls(out settings))
                return;

            outputTextBox.Clear();
            SpriteCrammer crammer = new SpriteCrammer();
            crammer.LogEvent += new ProgressLog(crammer_LogEvent);
            crammer.Settings = settings;
            crammer.Run();
        }

        void crammer_LogEvent(string s)
        {
            outputTextBox.AppendText(s+"\r\n");
            outputTextBox.Update();
        }

        private void outputBrowseButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog() ;
            dlg.DefaultExt = ".xml";
            dlg.Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
            dlg.FileName = outputFilenameEdit.Text;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                outputFilenameEdit.Text = dlg.FileName;
            }
        }

        private void browseRelativeFilenameBtn_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (relativePathEdit.Text != "")
            {
                dlg.SelectedPath = relativePathEdit.Text;
            }
            else if (outputFilenameEdit.Text != "")
            {
                dlg.SelectedPath = Path.GetDirectoryName(outputFilenameEdit.Text);
            }
            dlg.ShowNewFolderButton = true;
            if (dlg.ShowDialog() == DialogResult.OK)
                relativePathEdit.Text = dlg.SelectedPath;
        }

        private void addFallbackButton_Click(object sender, EventArgs e)
        {
            AddFallback dlg = new AddFallback();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Size size;
                string str = dlg.FallbackSize;
                if (!Util.ParseSize(str, out size))
                {
                    MessageBox.Show(String.Format("Invalid sprite page size: \"{0}\"", str));
                }
                else
                {
                    if (fallbackSizes.IndexOf(size) == -1)
                    {
                        fallbackSizes.Add(size);
                        UpdateFallbackList();
                    }
                }
            }
        }

        private void removeFallbackButton_Click(object sender, EventArgs e)
        {
            foreach (object o in fallbackSizeListBox.SelectedItems)
            {
                string s = (string)o;
                Size size;
                Util.ParseSize(s, out size);
                fallbackSizes.Remove(size);
            }
            UpdateFallbackList();
        }

        private void UpdateFallbackList()
        {
            fallbackSizeListBox.Items.Clear();
            fallbackSizes.Sort(Util.CompareFallbackSizes);
            foreach (Size s in fallbackSizes)
            {
                fallbackSizeListBox.Items.Add(String.Format("{0}x{1}", s.Width, s.Height));
            }
        }

        private void CramForm_Load(object sender, EventArgs e)
        {
            this.Text = this.Text + " " + VersionInfo.Version;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutSpriteCrammerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm form = new AboutForm();
            form.ShowDialog();
        }

        private void openSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Sprite Crammer settings (*.cram)|*.cram|All Files (*.*)|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                CramSettings settings = CramSettings.Load(dlg.FileName);
                if (settings == null)
                {
                    MessageBox.Show(String.Format("Error loading settings file: {0}", dlg.FileName),
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
                else
                {
                    SetControlsFromSettings(settings);
                }
            }
        }

        private void saveSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CramSettings settings;
            if (!GetSettingsFromControls(out settings))
                return;

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.DefaultExt = ".cram";
            dlg.Filter = "Sprite Crammer settings (*.cram)|*.cram|All Files (*.*)|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (!settings.Save(dlg.FileName))
                {
                    MessageBox.Show(String.Format("Error saving settings file: {0}", dlg.FileName),
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
        }

    }

    public class InputImageTag
    {
        public string Filename;
        public int Width;
        public int Height;
        public ulong FileSize;
    }
}