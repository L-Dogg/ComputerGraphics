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
			this.polygonUnionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.lightColorButton = new System.Windows.Forms.Button();
			this.textureButton = new System.Windows.Forms.Button();
			this.normalVectorGroupbox = new System.Windows.Forms.GroupBox();
			this.animatedLightRadiobutton = new System.Windows.Forms.RadioButton();
			this.constantLightRadiobutton = new System.Windows.Forms.RadioButton();
			this.label1 = new System.Windows.Forms.Label();
			this.polygonFillColorButton = new System.Windows.Forms.Button();
			this.ligthLabel = new System.Windows.Forms.Label();
			this.fillColorLabel = new System.Windows.Forms.Label();
			this.fillTypeGroupbox = new System.Windows.Forms.GroupBox();
			this.bumpMapRadiobutton = new System.Windows.Forms.RadioButton();
			this.textureRadiobutton = new System.Windows.Forms.RadioButton();
			this.colorRadiobutton = new System.Windows.Forms.RadioButton();
			this.background = new GK2.Controls.Background();
			this.bumpMapButton = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.polygonContextMenu.SuspendLayout();
			this.normalVectorGroupbox.SuspendLayout();
			this.fillTypeGroupbox.SuspendLayout();
			this.SuspendLayout();
			// 
			// polygonContextMenu
			// 
			this.polygonContextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.polygonContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addPolygonToolStripMenuItem,
            this.removePolygonToolStripMenuItem,
            this.movePolygonToolStripMenuItem,
            this.polygonUnionToolStripMenuItem});
			this.polygonContextMenu.Name = "polygonContextMenu";
			this.polygonContextMenu.Size = new System.Drawing.Size(219, 108);
			// 
			// addPolygonToolStripMenuItem
			// 
			this.addPolygonToolStripMenuItem.Name = "addPolygonToolStripMenuItem";
			this.addPolygonToolStripMenuItem.Size = new System.Drawing.Size(218, 26);
			this.addPolygonToolStripMenuItem.Text = "Add polygon";
			// 
			// removePolygonToolStripMenuItem
			// 
			this.removePolygonToolStripMenuItem.Name = "removePolygonToolStripMenuItem";
			this.removePolygonToolStripMenuItem.Size = new System.Drawing.Size(218, 26);
			this.removePolygonToolStripMenuItem.Text = "Remove polygon";
			// 
			// movePolygonToolStripMenuItem
			// 
			this.movePolygonToolStripMenuItem.Name = "movePolygonToolStripMenuItem";
			this.movePolygonToolStripMenuItem.Size = new System.Drawing.Size(218, 26);
			this.movePolygonToolStripMenuItem.Text = "Move polygon";
			// 
			// polygonUnionToolStripMenuItem
			// 
			this.polygonUnionToolStripMenuItem.Name = "polygonUnionToolStripMenuItem";
			this.polygonUnionToolStripMenuItem.Size = new System.Drawing.Size(218, 26);
			this.polygonUnionToolStripMenuItem.Text = "Polygon intersection";
			// 
			// lightColorButton
			// 
			this.lightColorButton.BackColor = System.Drawing.SystemColors.Window;
			this.lightColorButton.Location = new System.Drawing.Point(1433, 58);
			this.lightColorButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.lightColorButton.Name = "lightColorButton";
			this.lightColorButton.Size = new System.Drawing.Size(45, 28);
			this.lightColorButton.TabIndex = 1;
			this.lightColorButton.UseVisualStyleBackColor = false;
			this.lightColorButton.Click += new System.EventHandler(this.colorButton_Click);
			// 
			// textureButton
			// 
			this.textureButton.Location = new System.Drawing.Point(1433, 117);
			this.textureButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.textureButton.Name = "textureButton";
			this.textureButton.Size = new System.Drawing.Size(132, 28);
			this.textureButton.TabIndex = 2;
			this.textureButton.Text = "Texture";
			this.textureButton.UseVisualStyleBackColor = true;
			this.textureButton.Click += new System.EventHandler(this.textureButton_Click);
			// 
			// normalVectorGroupbox
			// 
			this.normalVectorGroupbox.BackColor = System.Drawing.SystemColors.Window;
			this.normalVectorGroupbox.Controls.Add(this.animatedLightRadiobutton);
			this.normalVectorGroupbox.Controls.Add(this.constantLightRadiobutton);
			this.normalVectorGroupbox.Location = new System.Drawing.Point(1433, 359);
			this.normalVectorGroupbox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.normalVectorGroupbox.Name = "normalVectorGroupbox";
			this.normalVectorGroupbox.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.normalVectorGroupbox.Size = new System.Drawing.Size(132, 92);
			this.normalVectorGroupbox.TabIndex = 3;
			this.normalVectorGroupbox.TabStop = false;
			this.normalVectorGroupbox.Text = "Light Vector";
			// 
			// animatedLightRadiobutton
			// 
			this.animatedLightRadiobutton.AutoSize = true;
			this.animatedLightRadiobutton.Location = new System.Drawing.Point(9, 54);
			this.animatedLightRadiobutton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.animatedLightRadiobutton.Name = "animatedLightRadiobutton";
			this.animatedLightRadiobutton.Size = new System.Drawing.Size(88, 21);
			this.animatedLightRadiobutton.TabIndex = 1;
			this.animatedLightRadiobutton.Text = "Animated";
			this.animatedLightRadiobutton.UseVisualStyleBackColor = true;
			// 
			// constantLightRadiobutton
			// 
			this.constantLightRadiobutton.AutoSize = true;
			this.constantLightRadiobutton.Checked = true;
			this.constantLightRadiobutton.Location = new System.Drawing.Point(9, 25);
			this.constantLightRadiobutton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.constantLightRadiobutton.Name = "constantLightRadiobutton";
			this.constantLightRadiobutton.Size = new System.Drawing.Size(79, 21);
			this.constantLightRadiobutton.TabIndex = 0;
			this.constantLightRadiobutton.TabStop = true;
			this.constantLightRadiobutton.Text = "(0, 0, 1)";
			this.constantLightRadiobutton.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.SystemColors.Window;
			this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label1.Location = new System.Drawing.Point(1423, 0);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(3, 679);
			this.label1.TabIndex = 4;
			// 
			// polygonFillColorButton
			// 
			this.polygonFillColorButton.BackColor = System.Drawing.SystemColors.Window;
			this.polygonFillColorButton.Location = new System.Drawing.Point(1511, 58);
			this.polygonFillColorButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.polygonFillColorButton.Name = "polygonFillColorButton";
			this.polygonFillColorButton.Size = new System.Drawing.Size(45, 28);
			this.polygonFillColorButton.TabIndex = 5;
			this.polygonFillColorButton.UseVisualStyleBackColor = false;
			this.polygonFillColorButton.Click += new System.EventHandler(this.polygonButton_Click);
			// 
			// ligthLabel
			// 
			this.ligthLabel.AutoSize = true;
			this.ligthLabel.BackColor = System.Drawing.SystemColors.Window;
			this.ligthLabel.Location = new System.Drawing.Point(1433, 34);
			this.ligthLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.ligthLabel.Name = "ligthLabel";
			this.ligthLabel.Size = new System.Drawing.Size(39, 17);
			this.ligthLabel.TabIndex = 6;
			this.ligthLabel.Text = "Light";
			// 
			// fillColorLabel
			// 
			this.fillColorLabel.AutoSize = true;
			this.fillColorLabel.BackColor = System.Drawing.SystemColors.Window;
			this.fillColorLabel.Location = new System.Drawing.Point(1509, 34);
			this.fillColorLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.fillColorLabel.Name = "fillColorLabel";
			this.fillColorLabel.Size = new System.Drawing.Size(33, 17);
			this.fillColorLabel.TabIndex = 7;
			this.fillColorLabel.Text = "  Fill";
			// 
			// fillTypeGroupbox
			// 
			this.fillTypeGroupbox.Controls.Add(this.bumpMapRadiobutton);
			this.fillTypeGroupbox.Controls.Add(this.textureRadiobutton);
			this.fillTypeGroupbox.Controls.Add(this.colorRadiobutton);
			this.fillTypeGroupbox.Location = new System.Drawing.Point(1433, 236);
			this.fillTypeGroupbox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.fillTypeGroupbox.Name = "fillTypeGroupbox";
			this.fillTypeGroupbox.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.fillTypeGroupbox.Size = new System.Drawing.Size(132, 116);
			this.fillTypeGroupbox.TabIndex = 8;
			this.fillTypeGroupbox.TabStop = false;
			this.fillTypeGroupbox.Text = "Fill";
			// 
			// bumpMapRadiobutton
			// 
			this.bumpMapRadiobutton.AutoSize = true;
			this.bumpMapRadiobutton.Location = new System.Drawing.Point(9, 81);
			this.bumpMapRadiobutton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.bumpMapRadiobutton.Name = "bumpMapRadiobutton";
			this.bumpMapRadiobutton.Size = new System.Drawing.Size(96, 21);
			this.bumpMapRadiobutton.TabIndex = 3;
			this.bumpMapRadiobutton.Text = "Bump map";
			this.bumpMapRadiobutton.UseVisualStyleBackColor = true;
			// 
			// textureRadiobutton
			// 
			this.textureRadiobutton.AutoSize = true;
			this.textureRadiobutton.Checked = true;
			this.textureRadiobutton.Location = new System.Drawing.Point(9, 53);
			this.textureRadiobutton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.textureRadiobutton.Name = "textureRadiobutton";
			this.textureRadiobutton.Size = new System.Drawing.Size(77, 21);
			this.textureRadiobutton.TabIndex = 1;
			this.textureRadiobutton.TabStop = true;
			this.textureRadiobutton.Text = "Texture";
			this.textureRadiobutton.UseVisualStyleBackColor = true;
			// 
			// colorRadiobutton
			// 
			this.colorRadiobutton.AutoSize = true;
			this.colorRadiobutton.Location = new System.Drawing.Point(9, 25);
			this.colorRadiobutton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.colorRadiobutton.Name = "colorRadiobutton";
			this.colorRadiobutton.Size = new System.Drawing.Size(62, 21);
			this.colorRadiobutton.TabIndex = 0;
			this.colorRadiobutton.Text = "Color";
			this.colorRadiobutton.UseVisualStyleBackColor = true;
			// 
			// background
			// 
			this.background.BackColor = System.Drawing.Color.White;
			this.background.Dock = System.Windows.Forms.DockStyle.Fill;
			this.background.Location = new System.Drawing.Point(0, 0);
			this.background.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
			this.background.Name = "background";
			this.background.Size = new System.Drawing.Size(1572, 679);
			this.background.TabIndex = 0;
			this.background.KeyUp += new System.Windows.Forms.KeyEventHandler(this.background_KeyUp);
			this.background.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BgMouseDown);
			this.background.MouseMove += new System.Windows.Forms.MouseEventHandler(this.BgMouseMove);
			// 
			// bumpMapButton
			// 
			this.bumpMapButton.Location = new System.Drawing.Point(1433, 180);
			this.bumpMapButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.bumpMapButton.Name = "bumpMapButton";
			this.bumpMapButton.Size = new System.Drawing.Size(132, 28);
			this.bumpMapButton.TabIndex = 9;
			this.bumpMapButton.Text = "Bump Map";
			this.bumpMapButton.UseVisualStyleBackColor = true;
			this.bumpMapButton.Click += new System.EventHandler(this.bumpMapButton_Click);
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(1433, 479);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(100, 22);
			this.textBox1.TabIndex = 10;
			this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(1433, 459);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(52, 17);
			this.label2.TabIndex = 11;
			this.label2.Text = "Light Z";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.ClientSize = new System.Drawing.Size(1572, 679);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.bumpMapButton);
			this.Controls.Add(this.fillTypeGroupbox);
			this.Controls.Add(this.fillColorLabel);
			this.Controls.Add(this.ligthLabel);
			this.Controls.Add(this.polygonFillColorButton);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.normalVectorGroupbox);
			this.Controls.Add(this.textureButton);
			this.Controls.Add(this.lightColorButton);
			this.Controls.Add(this.background);
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Name = "MainForm";
			this.Text = "Polygon Editor";
			this.ResizeEnd += new System.EventHandler(this.MainForm_ResizeEnd);
			this.Resize += new System.EventHandler(this.MainForm_Resize);
			this.polygonContextMenu.ResumeLayout(false);
			this.normalVectorGroupbox.ResumeLayout(false);
			this.normalVectorGroupbox.PerformLayout();
			this.fillTypeGroupbox.ResumeLayout(false);
			this.fillTypeGroupbox.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Background background;
		private System.Windows.Forms.ContextMenuStrip polygonContextMenu;
		private System.Windows.Forms.ToolStripMenuItem addPolygonToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem removePolygonToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem movePolygonToolStripMenuItem;
		private System.Windows.Forms.Button lightColorButton;
		private System.Windows.Forms.Button textureButton;
		private System.Windows.Forms.GroupBox normalVectorGroupbox;
		private System.Windows.Forms.RadioButton animatedLightRadiobutton;
		private System.Windows.Forms.RadioButton constantLightRadiobutton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button polygonFillColorButton;
		private System.Windows.Forms.Label ligthLabel;
		private System.Windows.Forms.Label fillColorLabel;
		private System.Windows.Forms.GroupBox fillTypeGroupbox;
		private System.Windows.Forms.RadioButton colorRadiobutton;
		private System.Windows.Forms.RadioButton textureRadiobutton;
		private System.Windows.Forms.RadioButton bumpMapRadiobutton;
		private System.Windows.Forms.ToolStripMenuItem polygonUnionToolStripMenuItem;
		private System.Windows.Forms.Button bumpMapButton;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label2;
	}
}

