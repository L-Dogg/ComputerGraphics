namespace GK3
{
	partial class MainForm
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
			this.mainPictureBox = new System.Windows.Forms.PictureBox();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.pictureBox3 = new System.Windows.Forms.PictureBox();
			this.pictureBox4 = new System.Windows.Forms.PictureBox();
			this.firstChannelLabel = new System.Windows.Forms.Label();
			this.secondChannelLabel = new System.Windows.Forms.Label();
			this.thirdChannelLabel = new System.Windows.Forms.Label();
			this.separationCombobox = new System.Windows.Forms.ComboBox();
			this.pickFileButton = new System.Windows.Forms.Button();
			this.separationTypeLabel = new System.Windows.Forms.Label();
			this.runButton = new System.Windows.Forms.Button();
			this.labGroupbox = new System.Windows.Forms.GroupBox();
			this.gammaTexbox = new System.Windows.Forms.TextBox();
			this.gammaLabel = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.labPresetsCombobox = new System.Windows.Forms.ComboBox();
			this.whitePointPresetCombobox = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.Wy = new System.Windows.Forms.TextBox();
			this.Wx = new System.Windows.Forms.TextBox();
			this.By = new System.Windows.Forms.TextBox();
			this.Bx = new System.Windows.Forms.TextBox();
			this.Gy = new System.Windows.Forms.TextBox();
			this.Gx = new System.Windows.Forms.TextBox();
			this.Ry = new System.Windows.Forms.TextBox();
			this.Rx = new System.Windows.Forms.TextBox();
			this.saveFileButton = new System.Windows.Forms.Button();
			this.toGrayScaleButton = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.mainPictureBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
			this.labGroupbox.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainPictureBox
			// 
			this.mainPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.mainPictureBox.Location = new System.Drawing.Point(9, 10);
			this.mainPictureBox.Margin = new System.Windows.Forms.Padding(2);
			this.mainPictureBox.Name = "mainPictureBox";
			this.mainPictureBox.Size = new System.Drawing.Size(694, 535);
			this.mainPictureBox.TabIndex = 0;
			this.mainPictureBox.TabStop = false;
			// 
			// pictureBox2
			// 
			this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureBox2.Location = new System.Drawing.Point(766, 10);
			this.pictureBox2.Margin = new System.Windows.Forms.Padding(2);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(254, 195);
			this.pictureBox2.TabIndex = 1;
			this.pictureBox2.TabStop = false;
			// 
			// pictureBox3
			// 
			this.pictureBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureBox3.Location = new System.Drawing.Point(766, 228);
			this.pictureBox3.Margin = new System.Windows.Forms.Padding(2);
			this.pictureBox3.Name = "pictureBox3";
			this.pictureBox3.Size = new System.Drawing.Size(254, 195);
			this.pictureBox3.TabIndex = 2;
			this.pictureBox3.TabStop = false;
			// 
			// pictureBox4
			// 
			this.pictureBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureBox4.Location = new System.Drawing.Point(766, 444);
			this.pictureBox4.Margin = new System.Windows.Forms.Padding(2);
			this.pictureBox4.Name = "pictureBox4";
			this.pictureBox4.Size = new System.Drawing.Size(254, 195);
			this.pictureBox4.TabIndex = 3;
			this.pictureBox4.TabStop = false;
			// 
			// firstChannelLabel
			// 
			this.firstChannelLabel.AutoSize = true;
			this.firstChannelLabel.Location = new System.Drawing.Point(727, 10);
			this.firstChannelLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.firstChannelLabel.Name = "firstChannelLabel";
			this.firstChannelLabel.Size = new System.Drawing.Size(15, 13);
			this.firstChannelLabel.TabIndex = 4;
			this.firstChannelLabel.Text = "R";
			// 
			// secondChannelLabel
			// 
			this.secondChannelLabel.AutoSize = true;
			this.secondChannelLabel.Location = new System.Drawing.Point(727, 228);
			this.secondChannelLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.secondChannelLabel.Name = "secondChannelLabel";
			this.secondChannelLabel.Size = new System.Drawing.Size(15, 13);
			this.secondChannelLabel.TabIndex = 5;
			this.secondChannelLabel.Text = "G";
			// 
			// thirdChannelLabel
			// 
			this.thirdChannelLabel.AutoSize = true;
			this.thirdChannelLabel.Location = new System.Drawing.Point(727, 444);
			this.thirdChannelLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.thirdChannelLabel.Name = "thirdChannelLabel";
			this.thirdChannelLabel.Size = new System.Drawing.Size(14, 13);
			this.thirdChannelLabel.TabIndex = 6;
			this.thirdChannelLabel.Text = "B";
			// 
			// separationCombobox
			// 
			this.separationCombobox.FormattingEnabled = true;
			this.separationCombobox.Items.AddRange(new object[] {
            "RGB (test)",
            "YCbCr separation",
            "HSV separation",
            "Lab separation"});
			this.separationCombobox.Location = new System.Drawing.Point(500, 598);
			this.separationCombobox.Margin = new System.Windows.Forms.Padding(2);
			this.separationCombobox.Name = "separationCombobox";
			this.separationCombobox.Size = new System.Drawing.Size(138, 21);
			this.separationCombobox.TabIndex = 7;
			this.separationCombobox.SelectedIndexChanged += new System.EventHandler(this.separationCombobox_SelectedIndexChanged);
			// 
			// pickFileButton
			// 
			this.pickFileButton.Location = new System.Drawing.Point(385, 584);
			this.pickFileButton.Margin = new System.Windows.Forms.Padding(2);
			this.pickFileButton.Name = "pickFileButton";
			this.pickFileButton.Size = new System.Drawing.Size(111, 38);
			this.pickFileButton.TabIndex = 8;
			this.pickFileButton.Text = "Choose file";
			this.pickFileButton.UseVisualStyleBackColor = true;
			this.pickFileButton.Click += new System.EventHandler(this.pickFileButton_Click);
			// 
			// separationTypeLabel
			// 
			this.separationTypeLabel.AutoSize = true;
			this.separationTypeLabel.Location = new System.Drawing.Point(500, 579);
			this.separationTypeLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.separationTypeLabel.Name = "separationTypeLabel";
			this.separationTypeLabel.Size = new System.Drawing.Size(84, 13);
			this.separationTypeLabel.TabIndex = 9;
			this.separationTypeLabel.Text = "Separation type:";
			// 
			// runButton
			// 
			this.runButton.Location = new System.Drawing.Point(647, 584);
			this.runButton.Margin = new System.Windows.Forms.Padding(2);
			this.runButton.Name = "runButton";
			this.runButton.Size = new System.Drawing.Size(56, 38);
			this.runButton.TabIndex = 10;
			this.runButton.Text = "RUN";
			this.runButton.UseVisualStyleBackColor = true;
			this.runButton.Click += new System.EventHandler(this.runButton_Click);
			// 
			// labGroupbox
			// 
			this.labGroupbox.Controls.Add(this.gammaTexbox);
			this.labGroupbox.Controls.Add(this.gammaLabel);
			this.labGroupbox.Controls.Add(this.label8);
			this.labGroupbox.Controls.Add(this.label7);
			this.labGroupbox.Controls.Add(this.labPresetsCombobox);
			this.labGroupbox.Controls.Add(this.whitePointPresetCombobox);
			this.labGroupbox.Controls.Add(this.label5);
			this.labGroupbox.Controls.Add(this.label6);
			this.labGroupbox.Controls.Add(this.label4);
			this.labGroupbox.Controls.Add(this.label3);
			this.labGroupbox.Controls.Add(this.label2);
			this.labGroupbox.Controls.Add(this.label1);
			this.labGroupbox.Controls.Add(this.Wy);
			this.labGroupbox.Controls.Add(this.Wx);
			this.labGroupbox.Controls.Add(this.By);
			this.labGroupbox.Controls.Add(this.Bx);
			this.labGroupbox.Controls.Add(this.Gy);
			this.labGroupbox.Controls.Add(this.Gx);
			this.labGroupbox.Controls.Add(this.Ry);
			this.labGroupbox.Controls.Add(this.Rx);
			this.labGroupbox.Location = new System.Drawing.Point(9, 550);
			this.labGroupbox.Name = "labGroupbox";
			this.labGroupbox.Size = new System.Drawing.Size(353, 123);
			this.labGroupbox.TabIndex = 27;
			this.labGroupbox.TabStop = false;
			this.labGroupbox.Text = "Lab";
			// 
			// gammaTexbox
			// 
			this.gammaTexbox.Location = new System.Drawing.Point(259, 92);
			this.gammaTexbox.Name = "gammaTexbox";
			this.gammaTexbox.Size = new System.Drawing.Size(68, 20);
			this.gammaTexbox.TabIndex = 46;
			this.gammaTexbox.Text = "2.2";
			// 
			// gammaLabel
			// 
			this.gammaLabel.AutoSize = true;
			this.gammaLabel.Location = new System.Drawing.Point(239, 94);
			this.gammaLabel.Name = "gammaLabel";
			this.gammaLabel.Size = new System.Drawing.Size(13, 13);
			this.gammaLabel.TabIndex = 45;
			this.gammaLabel.Text = "γ";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(67, 95);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(101, 13);
			this.label8.TabIndex = 44;
			this.label8.Text = "White point presets:";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(230, 32);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(45, 13);
			this.label7.TabIndex = 43;
			this.label7.Text = "Presets:";
			// 
			// labPresetsCombobox
			// 
			this.labPresetsCombobox.FormattingEnabled = true;
			this.labPresetsCombobox.Items.AddRange(new object[] {
            "None",
            "adobeRGB(1998)",
            "appleRGB",
            "sRGB",
            "PAL/SECAM RGB",
            "NTSC RGB",
            "CIE RGB"});
			this.labPresetsCombobox.Location = new System.Drawing.Point(230, 51);
			this.labPresetsCombobox.Name = "labPresetsCombobox";
			this.labPresetsCombobox.Size = new System.Drawing.Size(121, 21);
			this.labPresetsCombobox.TabIndex = 42;
			this.labPresetsCombobox.SelectedIndexChanged += new System.EventHandler(this.labPresetsCombobox_SelectedIndexChanged);
			// 
			// whitePointPresetCombobox
			// 
			this.whitePointPresetCombobox.FormattingEnabled = true;
			this.whitePointPresetCombobox.Items.AddRange(new object[] {
            "C",
            "D50 ",
            "D65",
            "E"});
			this.whitePointPresetCombobox.Location = new System.Drawing.Point(172, 92);
			this.whitePointPresetCombobox.Name = "whitePointPresetCombobox";
			this.whitePointPresetCombobox.Size = new System.Drawing.Size(62, 21);
			this.whitePointPresetCombobox.TabIndex = 41;
			this.whitePointPresetCombobox.SelectedIndexChanged += new System.EventHandler(this.whitePointPresetCombobox_SelectedIndexChanged);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(189, 16);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(18, 13);
			this.label5.TabIndex = 40;
			this.label5.Text = "W";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(138, 16);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(14, 13);
			this.label6.TabIndex = 39;
			this.label6.Text = "B";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(84, 16);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(15, 13);
			this.label4.TabIndex = 38;
			this.label4.Text = "G";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(33, 16);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(15, 13);
			this.label3.TabIndex = 37;
			this.label3.Text = "R";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(1, 61);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(12, 13);
			this.label2.TabIndex = 36;
			this.label2.Text = "y";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(1, 35);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(12, 13);
			this.label1.TabIndex = 35;
			this.label1.Text = "x";
			// 
			// Wy
			// 
			this.Wy.Location = new System.Drawing.Point(172, 59);
			this.Wy.Name = "Wy";
			this.Wy.Size = new System.Drawing.Size(45, 20);
			this.Wy.TabIndex = 34;
			// 
			// Wx
			// 
			this.Wx.Location = new System.Drawing.Point(172, 32);
			this.Wx.Name = "Wx";
			this.Wx.Size = new System.Drawing.Size(45, 20);
			this.Wx.TabIndex = 33;
			// 
			// By
			// 
			this.By.Location = new System.Drawing.Point(121, 59);
			this.By.Name = "By";
			this.By.Size = new System.Drawing.Size(45, 20);
			this.By.TabIndex = 32;
			// 
			// Bx
			// 
			this.Bx.Location = new System.Drawing.Point(121, 32);
			this.Bx.Name = "Bx";
			this.Bx.Size = new System.Drawing.Size(45, 20);
			this.Bx.TabIndex = 31;
			// 
			// Gy
			// 
			this.Gy.Location = new System.Drawing.Point(70, 59);
			this.Gy.Name = "Gy";
			this.Gy.Size = new System.Drawing.Size(45, 20);
			this.Gy.TabIndex = 30;
			// 
			// Gx
			// 
			this.Gx.Location = new System.Drawing.Point(70, 32);
			this.Gx.Name = "Gx";
			this.Gx.Size = new System.Drawing.Size(45, 20);
			this.Gx.TabIndex = 29;
			// 
			// Ry
			// 
			this.Ry.Location = new System.Drawing.Point(19, 59);
			this.Ry.Name = "Ry";
			this.Ry.Size = new System.Drawing.Size(45, 20);
			this.Ry.TabIndex = 28;
			// 
			// Rx
			// 
			this.Rx.Location = new System.Drawing.Point(19, 32);
			this.Rx.Name = "Rx";
			this.Rx.Size = new System.Drawing.Size(45, 20);
			this.Rx.TabIndex = 27;
			// 
			// saveFileButton
			// 
			this.saveFileButton.Location = new System.Drawing.Point(385, 628);
			this.saveFileButton.Name = "saveFileButton";
			this.saveFileButton.Size = new System.Drawing.Size(111, 36);
			this.saveFileButton.TabIndex = 28;
			this.saveFileButton.Text = "Save File";
			this.saveFileButton.UseVisualStyleBackColor = true;
			this.saveFileButton.Click += new System.EventHandler(this.saveFileButton_Click);
			// 
			// toGrayScaleButton
			// 
			this.toGrayScaleButton.Location = new System.Drawing.Point(503, 628);
			this.toGrayScaleButton.Name = "toGrayScaleButton";
			this.toGrayScaleButton.Size = new System.Drawing.Size(96, 36);
			this.toGrayScaleButton.TabIndex = 29;
			this.toGrayScaleButton.Text = "To Gray Scale";
			this.toGrayScaleButton.UseVisualStyleBackColor = true;
			this.toGrayScaleButton.Click += new System.EventHandler(this.toGrayScaleButton_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1038, 676);
			this.Controls.Add(this.toGrayScaleButton);
			this.Controls.Add(this.saveFileButton);
			this.Controls.Add(this.labGroupbox);
			this.Controls.Add(this.runButton);
			this.Controls.Add(this.separationTypeLabel);
			this.Controls.Add(this.pickFileButton);
			this.Controls.Add(this.separationCombobox);
			this.Controls.Add(this.thirdChannelLabel);
			this.Controls.Add(this.secondChannelLabel);
			this.Controls.Add(this.firstChannelLabel);
			this.Controls.Add(this.pictureBox4);
			this.Controls.Add(this.pictureBox3);
			this.Controls.Add(this.pictureBox2);
			this.Controls.Add(this.mainPictureBox);
			this.Margin = new System.Windows.Forms.Padding(2);
			this.Name = "MainForm";
			this.Text = "Computer Graphics 3";
			((System.ComponentModel.ISupportInitialize)(this.mainPictureBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
			this.labGroupbox.ResumeLayout(false);
			this.labGroupbox.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox mainPictureBox;
		private System.Windows.Forms.PictureBox pictureBox2;
		private System.Windows.Forms.PictureBox pictureBox3;
		private System.Windows.Forms.PictureBox pictureBox4;
		private System.Windows.Forms.Label firstChannelLabel;
		private System.Windows.Forms.Label secondChannelLabel;
		private System.Windows.Forms.Label thirdChannelLabel;
		private System.Windows.Forms.ComboBox separationCombobox;
		private System.Windows.Forms.Button pickFileButton;
		private System.Windows.Forms.Label separationTypeLabel;
		private System.Windows.Forms.Button runButton;
		private System.Windows.Forms.GroupBox labGroupbox;
		private System.Windows.Forms.ComboBox labPresetsCombobox;
		private System.Windows.Forms.ComboBox whitePointPresetCombobox;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox Wy;
		private System.Windows.Forms.TextBox By;
		private System.Windows.Forms.TextBox Bx;
		private System.Windows.Forms.TextBox Gy;
		private System.Windows.Forms.TextBox Gx;
		private System.Windows.Forms.TextBox Ry;
		private System.Windows.Forms.TextBox Rx;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox Wx;
		private System.Windows.Forms.Label gammaLabel;
		private System.Windows.Forms.TextBox gammaTexbox;
		private System.Windows.Forms.Button saveFileButton;
		private System.Windows.Forms.Button toGrayScaleButton;
	}
}

