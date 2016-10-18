namespace GK1.Controls
{
	partial class Length
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
			this.lengthTextBox = new System.Windows.Forms.TextBox();
			this.okButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lengthTextBox
			// 
			this.lengthTextBox.Location = new System.Drawing.Point(64, 12);
			this.lengthTextBox.Name = "lengthTextBox";
			this.lengthTextBox.Size = new System.Drawing.Size(100, 20);
			this.lengthTextBox.TabIndex = 0;
			this.lengthTextBox.TextChanged += new System.EventHandler(this.lengthTextBox_TextChanged);
			// 
			// okButton
			// 
			this.okButton.Location = new System.Drawing.Point(75, 38);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 1;
			this.okButton.Text = "OK";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new System.EventHandler(this.button1_Click);
			// 
			// Length
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(227, 64);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.lengthTextBox);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Length";
			this.Text = "Length";
			this.Load += new System.EventHandler(this.Length_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox lengthTextBox;
		private System.Windows.Forms.Button okButton;
	}
}