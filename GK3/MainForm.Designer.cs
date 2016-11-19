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
			((System.ComponentModel.ISupportInitialize)(this.mainPictureBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
			this.SuspendLayout();
			// 
			// mainPictureBox
			// 
			this.mainPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.mainPictureBox.Location = new System.Drawing.Point(12, 12);
			this.mainPictureBox.Name = "mainPictureBox";
			this.mainPictureBox.Size = new System.Drawing.Size(924, 658);
			this.mainPictureBox.TabIndex = 0;
			this.mainPictureBox.TabStop = false;
			// 
			// pictureBox2
			// 
			this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureBox2.Location = new System.Drawing.Point(1021, 12);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(338, 240);
			this.pictureBox2.TabIndex = 1;
			this.pictureBox2.TabStop = false;
			// 
			// pictureBox3
			// 
			this.pictureBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureBox3.Location = new System.Drawing.Point(1021, 280);
			this.pictureBox3.Name = "pictureBox3";
			this.pictureBox3.Size = new System.Drawing.Size(338, 240);
			this.pictureBox3.TabIndex = 2;
			this.pictureBox3.TabStop = false;
			// 
			// pictureBox4
			// 
			this.pictureBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureBox4.Location = new System.Drawing.Point(1021, 547);
			this.pictureBox4.Name = "pictureBox4";
			this.pictureBox4.Size = new System.Drawing.Size(338, 240);
			this.pictureBox4.TabIndex = 3;
			this.pictureBox4.TabStop = false;
			// 
			// firstChannelLabel
			// 
			this.firstChannelLabel.AutoSize = true;
			this.firstChannelLabel.Location = new System.Drawing.Point(969, 12);
			this.firstChannelLabel.Name = "firstChannelLabel";
			this.firstChannelLabel.Size = new System.Drawing.Size(18, 17);
			this.firstChannelLabel.TabIndex = 4;
			this.firstChannelLabel.Text = "R";
			// 
			// secondChannelLabel
			// 
			this.secondChannelLabel.AutoSize = true;
			this.secondChannelLabel.Location = new System.Drawing.Point(969, 280);
			this.secondChannelLabel.Name = "secondChannelLabel";
			this.secondChannelLabel.Size = new System.Drawing.Size(19, 17);
			this.secondChannelLabel.TabIndex = 5;
			this.secondChannelLabel.Text = "G";
			// 
			// thirdChannelLabel
			// 
			this.thirdChannelLabel.AutoSize = true;
			this.thirdChannelLabel.Location = new System.Drawing.Point(969, 547);
			this.thirdChannelLabel.Name = "thirdChannelLabel";
			this.thirdChannelLabel.Size = new System.Drawing.Size(17, 17);
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
			this.separationCombobox.Location = new System.Drawing.Point(667, 736);
			this.separationCombobox.Name = "separationCombobox";
			this.separationCombobox.Size = new System.Drawing.Size(182, 24);
			this.separationCombobox.TabIndex = 7;
			this.separationCombobox.SelectedIndexChanged += new System.EventHandler(this.separationCombobox_SelectedIndexChanged);
			// 
			// pickFileButton
			// 
			this.pickFileButton.Location = new System.Drawing.Point(441, 713);
			this.pickFileButton.Name = "pickFileButton";
			this.pickFileButton.Size = new System.Drawing.Size(148, 47);
			this.pickFileButton.TabIndex = 8;
			this.pickFileButton.Text = "Choose file";
			this.pickFileButton.UseVisualStyleBackColor = true;
			this.pickFileButton.Click += new System.EventHandler(this.pickFileButton_Click);
			// 
			// separationTypeLabel
			// 
			this.separationTypeLabel.AutoSize = true;
			this.separationTypeLabel.Location = new System.Drawing.Point(667, 713);
			this.separationTypeLabel.Name = "separationTypeLabel";
			this.separationTypeLabel.Size = new System.Drawing.Size(112, 17);
			this.separationTypeLabel.TabIndex = 9;
			this.separationTypeLabel.Text = "Separation type:";
			// 
			// runButton
			// 
			this.runButton.Location = new System.Drawing.Point(861, 713);
			this.runButton.Name = "runButton";
			this.runButton.Size = new System.Drawing.Size(75, 47);
			this.runButton.TabIndex = 10;
			this.runButton.Text = "RUN";
			this.runButton.UseVisualStyleBackColor = true;
			this.runButton.Click += new System.EventHandler(this.runButton_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1384, 799);
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
			this.Name = "MainForm";
			this.Text = "Computer Graphics 3";
			((System.ComponentModel.ISupportInitialize)(this.mainPictureBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
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
	}
}

