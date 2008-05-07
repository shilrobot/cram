namespace Cram
{
    partial class CramForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CramForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.removeAllInputImagesButton = new System.Windows.Forms.Button();
            this.inputImagesStatsLabel = new System.Windows.Forms.Label();
            this.inputImageList = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.removeInputImageButton = new System.Windows.Forms.Button();
            this.addInputImageButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.colorKeyChooser = new System.Windows.Forms.PictureBox();
            this.disableCKMode = new System.Windows.Forms.RadioButton();
            this.specificCKMode = new System.Windows.Forms.RadioButton();
            this.automaticCKMode = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.backgroundColorChooser = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.borderSizeUpDown = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.uniqueCheck = new System.Windows.Forms.CheckBox();
            this.alphaUpDown = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.fallbackPagesUpDown = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.removeFallbackButton = new System.Windows.Forms.Button();
            this.addFallbackButton = new System.Windows.Forms.Button();
            this.fallbackSizeListBox = new System.Windows.Forms.ListBox();
            this.enableFallbackCheck = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.spritePageSizeCombo = new System.Windows.Forms.ComboBox();
            this.rotateCheck = new System.Windows.Forms.CheckBox();
            this.cropCheck = new System.Windows.Forms.CheckBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.label8 = new System.Windows.Forms.Label();
            this.outputFilenameEdit = new System.Windows.Forms.TextBox();
            this.outputBrowseButton = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cleanCheck = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.outputTextBox = new System.Windows.Forms.TextBox();
            this.generateButton = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutSpriteCrammerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shortNameMode = new System.Windows.Forms.RadioButton();
            this.fullPathMode = new System.Windows.Forms.RadioButton();
            this.relativePathMode = new System.Windows.Forms.RadioButton();
            this.browseRelativeFilenameBtn = new System.Windows.Forms.Button();
            this.relativePathEdit = new System.Windows.Forms.TextBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.colorKeyChooser)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.backgroundColorChooser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.borderSizeUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.alphaUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fallbackPagesUpDown)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.removeAllInputImagesButton);
            this.groupBox1.Controls.Add(this.inputImagesStatsLabel);
            this.groupBox1.Controls.Add(this.inputImageList);
            this.groupBox1.Controls.Add(this.removeInputImageButton);
            this.groupBox1.Controls.Add(this.addInputImageButton);
            this.groupBox1.Location = new System.Drawing.Point(12, 31);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(301, 447);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Input Images";
            // 
            // removeAllInputImagesButton
            // 
            this.removeAllInputImagesButton.Location = new System.Drawing.Point(168, 20);
            this.removeAllInputImagesButton.Name = "removeAllInputImagesButton";
            this.removeAllInputImagesButton.Size = new System.Drawing.Size(75, 23);
            this.removeAllInputImagesButton.TabIndex = 2;
            this.removeAllInputImagesButton.Text = "Remove All";
            this.removeAllInputImagesButton.UseVisualStyleBackColor = true;
            this.removeAllInputImagesButton.Click += new System.EventHandler(this.removeAllInputImagesButton_Click);
            // 
            // inputImagesStatsLabel
            // 
            this.inputImagesStatsLabel.AutoSize = true;
            this.inputImagesStatsLabel.Location = new System.Drawing.Point(3, 424);
            this.inputImagesStatsLabel.Name = "inputImagesStatsLabel";
            this.inputImagesStatsLabel.Size = new System.Drawing.Size(105, 13);
            this.inputImagesStatsLabel.TabIndex = 3;
            this.inputImagesStatsLabel.Text = "{Some statistics info}";
            // 
            // inputImageList
            // 
            this.inputImageList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.inputImageList.FullRowSelect = true;
            this.inputImageList.Location = new System.Drawing.Point(6, 49);
            this.inputImageList.Name = "inputImageList";
            this.inputImageList.Size = new System.Drawing.Size(289, 372);
            this.inputImageList.TabIndex = 3;
            this.inputImageList.UseCompatibleStateImageBehavior = false;
            this.inputImageList.View = System.Windows.Forms.View.Details;
            this.inputImageList.SelectedIndexChanged += new System.EventHandler(this.inputImageList_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Filename";
            this.columnHeader1.Width = 140;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Size";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader2.Width = 68;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "On Disk";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // removeInputImageButton
            // 
            this.removeInputImageButton.Location = new System.Drawing.Point(87, 20);
            this.removeInputImageButton.Name = "removeInputImageButton";
            this.removeInputImageButton.Size = new System.Drawing.Size(75, 23);
            this.removeInputImageButton.TabIndex = 1;
            this.removeInputImageButton.Text = "Remove";
            this.removeInputImageButton.UseVisualStyleBackColor = true;
            this.removeInputImageButton.Click += new System.EventHandler(this.removeInputImageButton_Click);
            // 
            // addInputImageButton
            // 
            this.addInputImageButton.Location = new System.Drawing.Point(6, 20);
            this.addInputImageButton.Name = "addInputImageButton";
            this.addInputImageButton.Size = new System.Drawing.Size(75, 23);
            this.addInputImageButton.TabIndex = 0;
            this.addInputImageButton.Text = "Add...";
            this.addInputImageButton.UseVisualStyleBackColor = true;
            this.addInputImageButton.Click += new System.EventHandler(this.addInputImageButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.colorKeyChooser);
            this.groupBox2.Controls.Add(this.disableCKMode);
            this.groupBox2.Controls.Add(this.specificCKMode);
            this.groupBox2.Controls.Add(this.automaticCKMode);
            this.groupBox2.Location = new System.Drawing.Point(320, 31);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(301, 89);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Color Key";
            // 
            // colorKeyChooser
            // 
            this.colorKeyChooser.BackColor = System.Drawing.Color.Red;
            this.colorKeyChooser.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.colorKeyChooser.Location = new System.Drawing.Point(103, 41);
            this.colorKeyChooser.Name = "colorKeyChooser";
            this.colorKeyChooser.Size = new System.Drawing.Size(59, 23);
            this.colorKeyChooser.TabIndex = 4;
            this.colorKeyChooser.TabStop = false;
            this.colorKeyChooser.Click += new System.EventHandler(this.colorKeyChooser_Click);
            // 
            // disableCKMode
            // 
            this.disableCKMode.AutoSize = true;
            this.disableCKMode.Location = new System.Drawing.Point(9, 66);
            this.disableCKMode.Name = "disableCKMode";
            this.disableCKMode.Size = new System.Drawing.Size(60, 17);
            this.disableCKMode.TabIndex = 3;
            this.disableCKMode.Text = "Disable";
            this.disableCKMode.UseVisualStyleBackColor = true;
            // 
            // specificCKMode
            // 
            this.specificCKMode.AutoSize = true;
            this.specificCKMode.Location = new System.Drawing.Point(9, 43);
            this.specificCKMode.Name = "specificCKMode";
            this.specificCKMode.Size = new System.Drawing.Size(92, 17);
            this.specificCKMode.TabIndex = 1;
            this.specificCKMode.Text = "Specific color:";
            this.specificCKMode.UseVisualStyleBackColor = true;
            this.specificCKMode.CheckedChanged += new System.EventHandler(this.specificCKMode_CheckedChanged);
            // 
            // automaticCKMode
            // 
            this.automaticCKMode.AutoSize = true;
            this.automaticCKMode.Checked = true;
            this.automaticCKMode.Location = new System.Drawing.Point(9, 20);
            this.automaticCKMode.Name = "automaticCKMode";
            this.automaticCKMode.Size = new System.Drawing.Size(229, 17);
            this.automaticCKMode.TabIndex = 0;
            this.automaticCKMode.TabStop = true;
            this.automaticCKMode.Text = "Automatic (guesses based on corner pixels)";
            this.automaticCKMode.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.backgroundColorChooser);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.borderSizeUpDown);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.uniqueCheck);
            this.groupBox3.Controls.Add(this.alphaUpDown);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.fallbackPagesUpDown);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.removeFallbackButton);
            this.groupBox3.Controls.Add(this.addFallbackButton);
            this.groupBox3.Controls.Add(this.fallbackSizeListBox);
            this.groupBox3.Controls.Add(this.enableFallbackCheck);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.spritePageSizeCombo);
            this.groupBox3.Controls.Add(this.rotateCheck);
            this.groupBox3.Controls.Add(this.cropCheck);
            this.groupBox3.Location = new System.Drawing.Point(320, 126);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(301, 342);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Options";
            // 
            // backgroundColorChooser
            // 
            this.backgroundColorChooser.BackColor = System.Drawing.Color.Red;
            this.backgroundColorChooser.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.backgroundColorChooser.Location = new System.Drawing.Point(105, 56);
            this.backgroundColorChooser.Name = "backgroundColorChooser";
            this.backgroundColorChooser.Size = new System.Drawing.Size(59, 23);
            this.backgroundColorChooser.TabIndex = 5;
            this.backgroundColorChooser.TabStop = false;
            this.backgroundColorChooser.Click += new System.EventHandler(this.backgroundColorChooser_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(201, 245);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "pages:";
            // 
            // borderSizeUpDown
            // 
            this.borderSizeUpDown.Location = new System.Drawing.Point(74, 86);
            this.borderSizeUpDown.Name = "borderSizeUpDown";
            this.borderSizeUpDown.Size = new System.Drawing.Size(46, 20);
            this.borderSizeUpDown.TabIndex = 2;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 89);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(62, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Border size:";
            // 
            // uniqueCheck
            // 
            this.uniqueCheck.AutoSize = true;
            this.uniqueCheck.Checked = true;
            this.uniqueCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.uniqueCheck.Location = new System.Drawing.Point(9, 318);
            this.uniqueCheck.Name = "uniqueCheck";
            this.uniqueCheck.Size = new System.Drawing.Size(150, 17);
            this.uniqueCheck.TabIndex = 10;
            this.uniqueCheck.Text = "Eliminate duplicate images";
            this.uniqueCheck.UseVisualStyleBackColor = true;
            // 
            // alphaUpDown
            // 
            this.alphaUpDown.Location = new System.Drawing.Point(213, 58);
            this.alphaUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.alphaUpDown.Name = "alphaUpDown";
            this.alphaUpDown.Size = new System.Drawing.Size(46, 20);
            this.alphaUpDown.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(170, 60);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Alpha:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 60);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Background color:";
            // 
            // fallbackPagesUpDown
            // 
            this.fallbackPagesUpDown.Location = new System.Drawing.Point(204, 265);
            this.fallbackPagesUpDown.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.fallbackPagesUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.fallbackPagesUpDown.Name = "fallbackPagesUpDown";
            this.fallbackPagesUpDown.Size = new System.Drawing.Size(46, 20);
            this.fallbackPagesUpDown.TabIndex = 7;
            this.fallbackPagesUpDown.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 148);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Fallback page sizes:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(201, 232);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Max. fallback";
            // 
            // removeFallbackButton
            // 
            this.removeFallbackButton.Location = new System.Drawing.Point(201, 193);
            this.removeFallbackButton.Name = "removeFallbackButton";
            this.removeFallbackButton.Size = new System.Drawing.Size(75, 23);
            this.removeFallbackButton.TabIndex = 5;
            this.removeFallbackButton.Text = "Remove";
            this.removeFallbackButton.UseVisualStyleBackColor = true;
            this.removeFallbackButton.Click += new System.EventHandler(this.removeFallbackButton_Click);
            // 
            // addFallbackButton
            // 
            this.addFallbackButton.Location = new System.Drawing.Point(201, 164);
            this.addFallbackButton.Name = "addFallbackButton";
            this.addFallbackButton.Size = new System.Drawing.Size(75, 23);
            this.addFallbackButton.TabIndex = 4;
            this.addFallbackButton.Text = "Add...";
            this.addFallbackButton.UseVisualStyleBackColor = true;
            this.addFallbackButton.Click += new System.EventHandler(this.addFallbackButton_Click);
            // 
            // fallbackSizeListBox
            // 
            this.fallbackSizeListBox.FormattingEnabled = true;
            this.fallbackSizeListBox.Location = new System.Drawing.Point(20, 164);
            this.fallbackSizeListBox.Name = "fallbackSizeListBox";
            this.fallbackSizeListBox.Size = new System.Drawing.Size(175, 121);
            this.fallbackSizeListBox.TabIndex = 3;
            // 
            // enableFallbackCheck
            // 
            this.enableFallbackCheck.AutoSize = true;
            this.enableFallbackCheck.Checked = true;
            this.enableFallbackCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.enableFallbackCheck.Location = new System.Drawing.Point(9, 118);
            this.enableFallbackCheck.Name = "enableFallbackCheck";
            this.enableFallbackCheck.Size = new System.Drawing.Size(196, 17);
            this.enableFallbackCheck.TabIndex = 7;
            this.enableFallbackCheck.Text = "Use smaller final pages, if beneficial:";
            this.enableFallbackCheck.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Sprite page size:";
            // 
            // spritePageSizeCombo
            // 
            this.spritePageSizeCombo.FormattingEnabled = true;
            this.spritePageSizeCombo.Items.AddRange(new object[] {
            "1024x1024",
            "512x512",
            "256x256",
            "128x128"});
            this.spritePageSizeCombo.Location = new System.Drawing.Point(97, 25);
            this.spritePageSizeCombo.Name = "spritePageSizeCombo";
            this.spritePageSizeCombo.Size = new System.Drawing.Size(85, 21);
            this.spritePageSizeCombo.TabIndex = 0;
            this.spritePageSizeCombo.Text = "512x512";
            // 
            // rotateCheck
            // 
            this.rotateCheck.AutoSize = true;
            this.rotateCheck.Checked = true;
            this.rotateCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.rotateCheck.Location = new System.Drawing.Point(168, 295);
            this.rotateCheck.Name = "rotateCheck";
            this.rotateCheck.Size = new System.Drawing.Size(89, 17);
            this.rotateCheck.TabIndex = 9;
            this.rotateCheck.Text = "Allow rotation";
            this.rotateCheck.UseVisualStyleBackColor = true;
            // 
            // cropCheck
            // 
            this.cropCheck.AutoSize = true;
            this.cropCheck.Checked = true;
            this.cropCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cropCheck.Location = new System.Drawing.Point(9, 295);
            this.cropCheck.Name = "cropCheck";
            this.cropCheck.Size = new System.Drawing.Size(151, 17);
            this.cropCheck.TabIndex = 8;
            this.cropCheck.Text = "Crop out transparent areas";
            this.cropCheck.UseVisualStyleBackColor = true;
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            this.openFileDialog.Multiselect = true;
            this.openFileDialog.Title = "Select Input Images";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 20);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(109, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Output XML filename:";
            // 
            // outputFilenameEdit
            // 
            this.outputFilenameEdit.Location = new System.Drawing.Point(9, 36);
            this.outputFilenameEdit.Name = "outputFilenameEdit";
            this.outputFilenameEdit.Size = new System.Drawing.Size(205, 20);
            this.outputFilenameEdit.TabIndex = 1;
            // 
            // outputBrowseButton
            // 
            this.outputBrowseButton.Location = new System.Drawing.Point(220, 34);
            this.outputBrowseButton.Name = "outputBrowseButton";
            this.outputBrowseButton.Size = new System.Drawing.Size(75, 23);
            this.outputBrowseButton.TabIndex = 5;
            this.outputBrowseButton.Text = "Browse...";
            this.outputBrowseButton.UseVisualStyleBackColor = true;
            this.outputBrowseButton.Click += new System.EventHandler(this.outputBrowseButton_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 59);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(270, 13);
            this.label9.TabIndex = 6;
            this.label9.Text = "Sprite pages will be named <xml-base-name>####.png.";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.cleanCheck);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.outputBrowseButton);
            this.groupBox4.Controls.Add(this.outputFilenameEdit);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Location = new System.Drawing.Point(12, 484);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(302, 105);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Output Files";
            // 
            // cleanCheck
            // 
            this.cleanCheck.AutoSize = true;
            this.cleanCheck.Location = new System.Drawing.Point(9, 81);
            this.cleanCheck.Name = "cleanCheck";
            this.cleanCheck.Size = new System.Drawing.Size(171, 17);
            this.cleanCheck.TabIndex = 7;
            this.cleanCheck.Text = "Delete existing sprite page files";
            this.cleanCheck.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.outputTextBox);
            this.groupBox5.Controls.Add(this.generateButton);
            this.groupBox5.Location = new System.Drawing.Point(627, 31);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(300, 558);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Run";
            // 
            // outputTextBox
            // 
            this.outputTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.outputTextBox.Location = new System.Drawing.Point(7, 60);
            this.outputTextBox.Multiline = true;
            this.outputTextBox.Name = "outputTextBox";
            this.outputTextBox.ReadOnly = true;
            this.outputTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.outputTextBox.Size = new System.Drawing.Size(285, 491);
            this.outputTextBox.TabIndex = 2;
            // 
            // generateButton
            // 
            this.generateButton.Location = new System.Drawing.Point(7, 18);
            this.generateButton.Name = "generateButton";
            this.generateButton.Size = new System.Drawing.Size(285, 36);
            this.generateButton.TabIndex = 0;
            this.generateButton.Text = "Generate sprite pages";
            this.generateButton.UseVisualStyleBackColor = true;
            this.generateButton.Click += new System.EventHandler(this.generateButton_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(939, 24);
            this.menuStrip1.TabIndex = 8;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openSettingsToolStripMenuItem,
            this.saveSettingsToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openSettingsToolStripMenuItem
            // 
            this.openSettingsToolStripMenuItem.Name = "openSettingsToolStripMenuItem";
            this.openSettingsToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.openSettingsToolStripMenuItem.Text = "&Load Settings...";
            this.openSettingsToolStripMenuItem.Click += new System.EventHandler(this.openSettingsToolStripMenuItem_Click);
            // 
            // saveSettingsToolStripMenuItem
            // 
            this.saveSettingsToolStripMenuItem.Name = "saveSettingsToolStripMenuItem";
            this.saveSettingsToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.saveSettingsToolStripMenuItem.Text = "&Save Settings...";
            this.saveSettingsToolStripMenuItem.Click += new System.EventHandler(this.saveSettingsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(160, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutSpriteCrammerToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutSpriteCrammerToolStripMenuItem
            // 
            this.aboutSpriteCrammerToolStripMenuItem.Name = "aboutSpriteCrammerToolStripMenuItem";
            this.aboutSpriteCrammerToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.aboutSpriteCrammerToolStripMenuItem.Text = "&About Sprite Crammer...";
            this.aboutSpriteCrammerToolStripMenuItem.Click += new System.EventHandler(this.aboutSpriteCrammerToolStripMenuItem_Click);
            // 
            // shortNameMode
            // 
            this.shortNameMode.AutoSize = true;
            this.shortNameMode.Checked = true;
            this.shortNameMode.Location = new System.Drawing.Point(6, 19);
            this.shortNameMode.Name = "shortNameMode";
            this.shortNameMode.Size = new System.Drawing.Size(246, 17);
            this.shortNameMode.TabIndex = 8;
            this.shortNameMode.TabStop = true;
            this.shortNameMode.Text = "Use only image filenames in XML file (no paths)";
            this.shortNameMode.UseVisualStyleBackColor = true;
            // 
            // fullPathMode
            // 
            this.fullPathMode.AutoSize = true;
            this.fullPathMode.Location = new System.Drawing.Point(6, 42);
            this.fullPathMode.Name = "fullPathMode";
            this.fullPathMode.Size = new System.Drawing.Size(89, 17);
            this.fullPathMode.TabIndex = 9;
            this.fullPathMode.Text = "Use full paths";
            this.fullPathMode.UseVisualStyleBackColor = true;
            // 
            // relativePathMode
            // 
            this.relativePathMode.AutoSize = true;
            this.relativePathMode.Location = new System.Drawing.Point(6, 65);
            this.relativePathMode.Name = "relativePathMode";
            this.relativePathMode.Size = new System.Drawing.Size(168, 17);
            this.relativePathMode.TabIndex = 10;
            this.relativePathMode.Text = "Use paths relative to directory:";
            this.relativePathMode.UseVisualStyleBackColor = true;
            // 
            // browseRelativeFilenameBtn
            // 
            this.browseRelativeFilenameBtn.Location = new System.Drawing.Point(217, 86);
            this.browseRelativeFilenameBtn.Name = "browseRelativeFilenameBtn";
            this.browseRelativeFilenameBtn.Size = new System.Drawing.Size(75, 23);
            this.browseRelativeFilenameBtn.TabIndex = 12;
            this.browseRelativeFilenameBtn.Text = "Browse...";
            this.browseRelativeFilenameBtn.UseVisualStyleBackColor = true;
            this.browseRelativeFilenameBtn.Click += new System.EventHandler(this.browseRelativeFilenameBtn_Click);
            // 
            // relativePathEdit
            // 
            this.relativePathEdit.Location = new System.Drawing.Point(20, 88);
            this.relativePathEdit.Name = "relativePathEdit";
            this.relativePathEdit.Size = new System.Drawing.Size(191, 20);
            this.relativePathEdit.TabIndex = 11;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.browseRelativeFilenameBtn);
            this.groupBox6.Controls.Add(this.shortNameMode);
            this.groupBox6.Controls.Add(this.relativePathEdit);
            this.groupBox6.Controls.Add(this.fullPathMode);
            this.groupBox6.Controls.Add(this.relativePathMode);
            this.groupBox6.Location = new System.Drawing.Point(320, 474);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(301, 115);
            this.groupBox6.TabIndex = 9;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Image Path Mode";
            // 
            // CramForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(939, 600);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "CramForm";
            this.Text = "Sprite Crammer";
            this.Load += new System.EventHandler(this.CramForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.colorKeyChooser)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.backgroundColorChooser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.borderSizeUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.alphaUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fallbackPagesUpDown)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label inputImagesStatsLabel;
        private System.Windows.Forms.ListView inputImageList;
        private System.Windows.Forms.Button removeInputImageButton;
        private System.Windows.Forms.Button addInputImageButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton disableCKMode;
        private System.Windows.Forms.RadioButton specificCKMode;
        private System.Windows.Forms.RadioButton automaticCKMode;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox rotateCheck;
        private System.Windows.Forms.CheckBox cropCheck;
        private System.Windows.Forms.CheckBox enableFallbackCheck;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox spritePageSizeCombo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button removeFallbackButton;
        private System.Windows.Forms.Button addFallbackButton;
        private System.Windows.Forms.ListBox fallbackSizeListBox;
        private System.Windows.Forms.NumericUpDown fallbackPagesUpDown;
        private System.Windows.Forms.NumericUpDown alphaUpDown;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox uniqueCheck;
        private System.Windows.Forms.NumericUpDown borderSizeUpDown;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button removeAllInputImagesButton;
        private System.Windows.Forms.PictureBox colorKeyChooser;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox backgroundColorChooser;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox outputFilenameEdit;
        private System.Windows.Forms.Button outputBrowseButton;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox outputTextBox;
        private System.Windows.Forms.Button generateButton;
        private System.Windows.Forms.CheckBox cleanCheck;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem aboutSpriteCrammerToolStripMenuItem;
        private System.Windows.Forms.RadioButton relativePathMode;
        private System.Windows.Forms.RadioButton fullPathMode;
        private System.Windows.Forms.RadioButton shortNameMode;
        private System.Windows.Forms.Button browseRelativeFilenameBtn;
        private System.Windows.Forms.TextBox relativePathEdit;
        private System.Windows.Forms.GroupBox groupBox6;
    }
}

