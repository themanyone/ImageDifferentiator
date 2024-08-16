namespace ImageDifferentiator
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            splitContainer1 = new SplitContainer();
            splitContainer2 = new SplitContainer();
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            cbPlayTone = new CheckBox();
            tbDetected = new TextBox();
            btFolder = new Button();
            cbSave = new CheckBox();
            label6 = new Label();
            nudPctThresh = new NumericUpDown();
            cbNormalizeDifference = new CheckBox();
            label5 = new Label();
            label4 = new Label();
            nudChechEvery = new NumericUpDown();
            cbDoCalc = new CheckBox();
            btStream = new Button();
            cbWhiteOut = new CheckBox();
            lbInfo = new Label();
            label3 = new Label();
            tbPct = new TextBox();
            label2 = new Label();
            tbOverThresh = new TextBox();
            label1 = new Label();
            nudThresh = new NumericUpDown();
            cbNorm = new CheckBox();
            btCalc = new Button();
            pictureBox3 = new PictureBox();
            btImage2 = new Button();
            btImage1 = new Button();
            openFileDialog1 = new OpenFileDialog();
            timer1 = new System.Windows.Forms.Timer(components);
            folderBrowserDialog1 = new FolderBrowserDialog();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudPctThresh).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudChechEvery).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudThresh).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(cbPlayTone);
            splitContainer1.Panel2.Controls.Add(tbDetected);
            splitContainer1.Panel2.Controls.Add(btFolder);
            splitContainer1.Panel2.Controls.Add(cbSave);
            splitContainer1.Panel2.Controls.Add(label6);
            splitContainer1.Panel2.Controls.Add(nudPctThresh);
            splitContainer1.Panel2.Controls.Add(cbNormalizeDifference);
            splitContainer1.Panel2.Controls.Add(label5);
            splitContainer1.Panel2.Controls.Add(label4);
            splitContainer1.Panel2.Controls.Add(nudChechEvery);
            splitContainer1.Panel2.Controls.Add(cbDoCalc);
            splitContainer1.Panel2.Controls.Add(btStream);
            splitContainer1.Panel2.Controls.Add(cbWhiteOut);
            splitContainer1.Panel2.Controls.Add(lbInfo);
            splitContainer1.Panel2.Controls.Add(label3);
            splitContainer1.Panel2.Controls.Add(tbPct);
            splitContainer1.Panel2.Controls.Add(label2);
            splitContainer1.Panel2.Controls.Add(tbOverThresh);
            splitContainer1.Panel2.Controls.Add(label1);
            splitContainer1.Panel2.Controls.Add(nudThresh);
            splitContainer1.Panel2.Controls.Add(cbNorm);
            splitContainer1.Panel2.Controls.Add(btCalc);
            splitContainer1.Panel2.Controls.Add(pictureBox3);
            splitContainer1.Panel2.Controls.Add(btImage2);
            splitContainer1.Panel2.Controls.Add(btImage1);
            splitContainer1.Size = new Size(956, 718);
            splitContainer1.SplitterDistance = 339;
            splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Enabled = false;
            splitContainer2.Location = new Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(pictureBox1);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(pictureBox2);
            splitContainer2.Size = new Size(956, 339);
            splitContainer2.SplitterDistance = 474;
            splitContainer2.TabIndex = 0;
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(474, 339);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.Dock = DockStyle.Fill;
            pictureBox2.Location = new Point(0, 0);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(478, 339);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 0;
            pictureBox2.TabStop = false;
            // 
            // cbPlayTone
            // 
            cbPlayTone.AutoSize = true;
            cbPlayTone.Checked = true;
            cbPlayTone.CheckState = CheckState.Checked;
            cbPlayTone.Location = new Point(14, 321);
            cbPlayTone.Name = "cbPlayTone";
            cbPlayTone.Size = new Size(76, 19);
            cbPlayTone.TabIndex = 24;
            cbPlayTone.Text = "Play Tone";
            cbPlayTone.UseVisualStyleBackColor = true;
            // 
            // tbDetected
            // 
            tbDetected.BackColor = Color.FromArgb(255, 255, 192);
            tbDetected.Location = new Point(126, 169);
            tbDetected.Name = "tbDetected";
            tbDetected.ReadOnly = true;
            tbDetected.Size = new Size(100, 23);
            tbDetected.TabIndex = 23;
            tbDetected.Text = "No Motion";
            tbDetected.TextAlign = HorizontalAlignment.Center;
            // 
            // btFolder
            // 
            btFolder.Location = new Point(161, 292);
            btFolder.Name = "btFolder";
            btFolder.Size = new Size(126, 23);
            btFolder.TabIndex = 22;
            btFolder.Text = "Output Directory";
            btFolder.UseVisualStyleBackColor = true;
            btFolder.Click += btFolder_Click;
            // 
            // cbSave
            // 
            cbSave.AutoSize = true;
            cbSave.Location = new Point(14, 296);
            cbSave.Name = "cbSave";
            cbSave.Size = new Size(141, 19);
            cbSave.TabIndex = 21;
            cbSave.Text = "Save Detected Images";
            cbSave.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(126, 199);
            label6.Name = "label6";
            label6.Size = new Size(102, 15);
            label6.TabIndex = 20;
            label6.Text = "Percent Threshold";
            // 
            // nudPctThresh
            // 
            nudPctThresh.DecimalPlaces = 6;
            nudPctThresh.Increment = new decimal(new int[] { 1, 0, 0, 196608 });
            nudPctThresh.Location = new Point(126, 217);
            nudPctThresh.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            nudPctThresh.Minimum = new decimal(new int[] { 1, 0, 0, 393216 });
            nudPctThresh.Name = "nudPctThresh";
            nudPctThresh.Size = new Size(75, 23);
            nudPctThresh.TabIndex = 19;
            nudPctThresh.Value = new decimal(new int[] { 3, 0, 0, 196608 });
            // 
            // cbNormalizeDifference
            // 
            cbNormalizeDifference.AutoSize = true;
            cbNormalizeDifference.Location = new Point(139, 246);
            cbNormalizeDifference.Name = "cbNormalizeDifference";
            cbNormalizeDifference.Size = new Size(137, 19);
            cbNormalizeDifference.TabIndex = 18;
            cbNormalizeDifference.Text = "Normalize Difference";
            cbNormalizeDifference.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(207, 130);
            label5.Name = "label5";
            label5.Size = new Size(23, 15);
            label5.TabIndex = 17;
            label5.Text = "ms";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(126, 104);
            label4.Name = "label4";
            label4.Size = new Size(71, 15);
            label4.TabIndex = 16;
            label4.Text = "Check Every";
            // 
            // nudChechEvery
            // 
            nudChechEvery.Location = new Point(126, 122);
            nudChechEvery.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            nudChechEvery.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudChechEvery.Name = "nudChechEvery";
            nudChechEvery.Size = new Size(75, 23);
            nudChechEvery.TabIndex = 15;
            nudChechEvery.Value = new decimal(new int[] { 200, 0, 0, 0 });
            // 
            // cbDoCalc
            // 
            cbDoCalc.AutoSize = true;
            cbDoCalc.Location = new Point(126, 75);
            cbDoCalc.Name = "cbDoCalc";
            cbDoCalc.Size = new Size(132, 19);
            cbDoCalc.TabIndex = 14;
            cbDoCalc.Text = "Calculate Difference";
            cbDoCalc.UseVisualStyleBackColor = true;
            // 
            // btStream
            // 
            btStream.BackColor = Color.FromArgb(255, 255, 192);
            btStream.Location = new Point(126, 42);
            btStream.Name = "btStream";
            btStream.Size = new Size(75, 23);
            btStream.TabIndex = 13;
            btStream.Text = "Stream";
            btStream.UseVisualStyleBackColor = false;
            btStream.Click += btStream_Click;
            // 
            // cbWhiteOut
            // 
            cbWhiteOut.AutoSize = true;
            cbWhiteOut.Checked = true;
            cbWhiteOut.CheckState = CheckState.Checked;
            cbWhiteOut.Location = new Point(14, 271);
            cbWhiteOut.Name = "cbWhiteOut";
            cbWhiteOut.Size = new Size(165, 19);
            cbWhiteOut.TabIndex = 12;
            cbWhiteOut.Text = "White-Out Detected Pixels";
            cbWhiteOut.UseVisualStyleBackColor = true;
            // 
            // lbInfo
            // 
            lbInfo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lbInfo.AutoSize = true;
            lbInfo.Location = new Point(14, 348);
            lbInfo.Name = "lbInfo";
            lbInfo.Size = new Size(39, 15);
            lbInfo.TabIndex = 11;
            lbInfo.Text = "Ready";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 199);
            label3.Name = "label3";
            label3.Size = new Size(47, 15);
            label3.TabIndex = 10;
            label3.Text = "Percent";
            // 
            // tbPct
            // 
            tbPct.Location = new Point(12, 217);
            tbPct.Name = "tbPct";
            tbPct.ReadOnly = true;
            tbPct.Size = new Size(75, 23);
            tbPct.TabIndex = 9;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 151);
            label2.Name = "label2";
            label2.Size = new Size(87, 15);
            label2.TabIndex = 8;
            label2.Text = "Over Threshold";
            // 
            // tbOverThresh
            // 
            tbOverThresh.Location = new Point(12, 169);
            tbOverThresh.Name = "tbOverThresh";
            tbOverThresh.ReadOnly = true;
            tbOverThresh.Size = new Size(75, 23);
            tbOverThresh.TabIndex = 7;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 104);
            label1.Name = "label1";
            label1.Size = new Size(59, 15);
            label1.TabIndex = 6;
            label1.Text = "Threshold";
            // 
            // nudThresh
            // 
            nudThresh.Location = new Point(12, 122);
            nudThresh.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudThresh.Name = "nudThresh";
            nudThresh.Size = new Size(75, 23);
            nudThresh.TabIndex = 5;
            nudThresh.Value = new decimal(new int[] { 25, 0, 0, 0 });
            // 
            // cbNorm
            // 
            cbNorm.AutoSize = true;
            cbNorm.Location = new Point(14, 246);
            cbNorm.Name = "cbNorm";
            cbNorm.Size = new Size(119, 19);
            cbNorm.TabIndex = 4;
            cbNorm.Text = "Normalize Source";
            cbNorm.UseVisualStyleBackColor = true;
            // 
            // btCalc
            // 
            btCalc.Location = new Point(12, 71);
            btCalc.Name = "btCalc";
            btCalc.Size = new Size(75, 23);
            btCalc.TabIndex = 3;
            btCalc.Text = "Calculate";
            btCalc.UseVisualStyleBackColor = true;
            btCalc.Click += btCalc_Click;
            // 
            // pictureBox3
            // 
            pictureBox3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            pictureBox3.Location = new Point(475, 3);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(478, 360);
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.TabIndex = 2;
            pictureBox3.TabStop = false;
            // 
            // btImage2
            // 
            btImage2.Location = new Point(12, 42);
            btImage2.Name = "btImage2";
            btImage2.Size = new Size(75, 23);
            btImage2.TabIndex = 1;
            btImage2.Text = "Image 2";
            btImage2.UseVisualStyleBackColor = true;
            btImage2.Click += btImage2_Click;
            // 
            // btImage1
            // 
            btImage1.Location = new Point(12, 13);
            btImage1.Name = "btImage1";
            btImage1.Size = new Size(75, 23);
            btImage1.TabIndex = 0;
            btImage1.Text = "Image 1";
            btImage1.UseVisualStyleBackColor = true;
            btImage1.Click += btImage1_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 1;
            timer1.Tick += timer1_Tick;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(956, 718);
            Controls.Add(splitContainer1);
            MinimumSize = new Size(774, 757);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            SizeChanged += Form1_SizeChanged;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudPctThresh).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudChechEvery).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudThresh).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private Button btImage2;
        private Button btImage1;
        private OpenFileDialog openFileDialog1;
        private SplitContainer splitContainer2;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private PictureBox pictureBox3;
        private Button btCalc;
        private CheckBox cbNorm;
        private Label label1;
        private NumericUpDown nudThresh;
        private Label label2;
        private TextBox tbOverThresh;
        private Label label3;
        private TextBox tbPct;
        private Label lbInfo;
        private CheckBox cbWhiteOut;
        private Button btStream;
        private System.Windows.Forms.Timer timer1;
        private CheckBox cbDoCalc;
        private Label label4;
        private NumericUpDown nudChechEvery;
        private Label label5;
        private CheckBox cbNormalizeDifference;
        private Label label6;
        private NumericUpDown nudPctThresh;
        private TextBox tbDetected;
        private Button btFolder;
        private CheckBox cbSave;
        private FolderBrowserDialog folderBrowserDialog1;
        private CheckBox cbPlayTone;
    }
}
