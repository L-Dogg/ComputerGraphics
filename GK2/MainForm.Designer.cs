using GK2.Controls;

namespace GK2
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
			this.components = new System.ComponentModel.Container();
			this.polygonContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.addPolygonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.removePolygonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.movePolygonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.colorButton = new System.Windows.Forms.Button();
			this.textureButton = new System.Windows.Forms.Button();
			this.normalVectorGroupbox = new System.Windows.Forms.GroupBox();
			this.radioButton2 = new System.Windows.Forms.RadioButton();
			this.radioButton1 = new System.Windows.Forms.RadioButton();
			this.background = new GK2.Controls.Background();
			this.label1 = new System.Windows.Forms.Label();
			this.polygonContextMenu.SuspendLayout();
			this.normalVectorGroupbox.SuspendLayout();
			this.SuspendLayout();
			// 
			// polygonContextMenu
			// 
			this.polygonContextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.polygonContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addPolygonToolStripMenuItem,
            this.removePolygonToolStripMenuItem,
            this.movePolygonToolStripMenuItem});
			this.polygonContextMenu.Name = "polygonContextMenu";
			this.polygonContextMenu.Size = new System.Drawing.Size(165, 70);
			// 
			// addPolygonToolStripMenuItem
			// 
			this.addPolygonToolStripMenuItem.Name = "addPolygonToolStripMenuItem";
			this.addPolygonToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
			this.addPolygonToolStripMenuItem.Text = "Add polygon";
			// 
			// removePolygonToolStripMenuItem
			// 
			this.removePolygonToolStripMenuItem.Name = "removePolygonToolStripMenuItem";
			this.removePolygonToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
			this.removePolygonToolStripMenuItem.Text = "Remove polygon";
			// 
			// movePolygonToolStripMenuItem
			// 
			this.movePolygonToolStripMenuItem.Name = "movePolygonToolStripMenuItem";
			this.movePolygonToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
			this.movePolygonToolStripMenuItem.Text = "Move polygon";
			// 
			// colorButton
			// 
			this.colorButton.Location = new System.Drawing.Point(1075, 47);
			this.colorButton.Name = "colorButton";
			this.colorButton.Size = new System.Drawing.Size(99, 23);
			this.colorButton.TabIndex = 1;
			this.colorButton.Text = "Light color";
			this.colorButton.UseVisualStyleBackColor = true;
			this.colorButton.Click += new System.EventHandler(this.colorButton_Click);
			// 
			// textureButton
			// 
			this.textureButton.Location = new System.Drawing.Point(1075, 95);
			this.textureButton.Name = "textureButton";
			this.textureButton.Size = new System.Drawing.Size(99, 23);
			this.textureButton.TabIndex = 2;
			this.textureButton.Text = "Texture";
			this.textureButton.UseVisualStyleBackColor = true;
			this.textureButton.Click += new System.EventHandler(this.textureButton_Click);
			// 
			// normalVectorGroupbox
			// 
			this.normalVectorGroupbox.BackColor = System.Drawing.SystemColors.Window;
			this.normalVectorGroupbox.Controls.Add(this.radioButton2);
			this.normalVectorGroupbox.Controls.Add(this.radioButton1);
			this.normalVectorGroupbox.Location = new System.Drawing.Point(1075, 155);
			this.normalVectorGroupbox.Name = "normalVectorGroupbox";
			this.normalVectorGroupbox.Size = new System.Drawing.Size(99, 100);
			this.normalVectorGroupbox.TabIndex = 3;
			this.normalVectorGroupbox.TabStop = false;
			this.normalVectorGroupbox.Text = "Light Vector";
			// 
			// radioButton2
			// 
			this.radioButton2.AutoSize = true;
			this.radioButton2.Location = new System.Drawing.Point(7, 44);
			this.radioButton2.Name = "radioButton2";
			this.radioButton2.Size = new System.Drawing.Size(69, 17);
			this.radioButton2.TabIndex = 1;
			this.radioButton2.Text = "Animated";
			this.radioButton2.UseVisualStyleBackColor = true;
			// 
			// radioButton1
			// 
			this.radioButton1.AutoSize = true;
			this.radioButton1.Checked = true;
			this.radioButton1.Location = new System.Drawing.Point(7, 20);
			this.radioButton1.Name = "radioButton1";
			this.radioButton1.Size = new System.Drawing.Size(61, 17);
			this.radioButton1.TabIndex = 0;
			this.radioButton1.TabStop = true;
			this.radioButton1.Text = "(0, 0, 1)";
			this.radioButton1.UseVisualStyleBackColor = true;
			// 
			// background
			// 
			this.background.BackColor = System.Drawing.Color.White;
			this.background.Dock = System.Windows.Forms.DockStyle.Fill;
			this.background.Location = new System.Drawing.Point(0, 0);
			this.background.Margin = new System.Windows.Forms.Padding(4);
			this.background.Name = "background";
			this.background.Size = new System.Drawing.Size(1179, 552);
			this.background.TabIndex = 0;
			this.background.KeyUp += new System.Windows.Forms.KeyEventHandler(this.background_KeyUp);
			this.background.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BgMouseDown);
			this.background.MouseMove += new System.Windows.Forms.MouseEventHandler(this.BgMouseMove);
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.SystemColors.Window;
			this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label1.Location = new System.Drawing.Point(1067, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(2, 552);
			this.label1.TabIndex = 4;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.AppWorkspace;
			this.ClientSize = new System.Drawing.Size(1179, 552);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.normalVectorGroupbox);
			this.Controls.Add(this.textureButton);
			this.Controls.Add(this.colorButton);
			this.Controls.Add(this.background);
			this.Name = "MainForm";
			this.Text = "Polygon Editor";
			this.ResizeEnd += new System.EventHandler(this.MainForm_ResizeEnd);
			this.Resize += new System.EventHandler(this.MainForm_Resize);
			this.polygonContextMenu.ResumeLayout(false);
			this.normalVectorGroupbox.ResumeLayout(false);
			this.normalVectorGroupbox.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private Background background;
		private System.Windows.Forms.ContextMenuStrip polygonContextMenu;
		private System.Windows.Forms.ToolStripMenuItem addPolygonToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem removePolygonToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem movePolygonToolStripMenuItem;
		private System.Windows.Forms.Button colorButton;
		private System.Windows.Forms.Button textureButton;
		private System.Windows.Forms.GroupBox normalVectorGroupbox;
		private System.Windows.Forms.RadioButton radioButton2;
		private System.Windows.Forms.RadioButton radioButton1;
		private System.Windows.Forms.Label label1;
	}
}

