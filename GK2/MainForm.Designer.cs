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
			this.colorRadiobutton = new System.Windows.Forms.RadioButton();
			this.textureRadiobutton = new System.Windows.Forms.RadioButton();
			this.bumpMapRadiobutton = new System.Windows.Forms.RadioButton();
			this.polygonUnionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.background = new GK2.Controls.Background();
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
			this.polygonContextMenu.Size = new System.Drawing.Size(165, 92);
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
			// lightColorButton
			// 
			this.lightColorButton.BackColor = System.Drawing.SystemColors.Window;
			this.lightColorButton.Location = new System.Drawing.Point(1075, 47);
			this.lightColorButton.Name = "lightColorButton";
			this.lightColorButton.Size = new System.Drawing.Size(34, 23);
			this.lightColorButton.TabIndex = 1;
			this.lightColorButton.UseVisualStyleBackColor = false;
			this.lightColorButton.Click += new System.EventHandler(this.colorButton_Click);
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
			this.normalVectorGroupbox.Controls.Add(this.animatedLightRadiobutton);
			this.normalVectorGroupbox.Controls.Add(this.constantLightRadiobutton);
			this.normalVectorGroupbox.Location = new System.Drawing.Point(1075, 235);
			this.normalVectorGroupbox.Name = "normalVectorGroupbox";
			this.normalVectorGroupbox.Size = new System.Drawing.Size(99, 75);
			this.normalVectorGroupbox.TabIndex = 3;
			this.normalVectorGroupbox.TabStop = false;
			this.normalVectorGroupbox.Text = "Light Vector";
			// 
			// animatedLightRadiobutton
			// 
			this.animatedLightRadiobutton.AutoSize = true;
			this.animatedLightRadiobutton.Location = new System.Drawing.Point(7, 44);
			this.animatedLightRadiobutton.Name = "animatedLightRadiobutton";
			this.animatedLightRadiobutton.Size = new System.Drawing.Size(69, 17);
			this.animatedLightRadiobutton.TabIndex = 1;
			this.animatedLightRadiobutton.Text = "Animated";
			this.animatedLightRadiobutton.UseVisualStyleBackColor = true;
			// 
			// constantLightRadiobutton
			// 
			this.constantLightRadiobutton.AutoSize = true;
			this.constantLightRadiobutton.Checked = true;
			this.constantLightRadiobutton.Location = new System.Drawing.Point(7, 20);
			this.constantLightRadiobutton.Name = "constantLightRadiobutton";
			this.constantLightRadiobutton.Size = new System.Drawing.Size(61, 17);
			this.constantLightRadiobutton.TabIndex = 0;
			this.constantLightRadiobutton.TabStop = true;
			this.constantLightRadiobutton.Text = "(0, 0, 1)";
			this.constantLightRadiobutton.UseVisualStyleBackColor = true;
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
			// polygonFillColorButton
			// 
			this.polygonFillColorButton.BackColor = System.Drawing.SystemColors.Window;
			this.polygonFillColorButton.Location = new System.Drawing.Point(1133, 47);
			this.polygonFillColorButton.Name = "polygonFillColorButton";
			this.polygonFillColorButton.Size = new System.Drawing.Size(34, 23);
			this.polygonFillColorButton.TabIndex = 5;
			this.polygonFillColorButton.UseVisualStyleBackColor = false;
			this.polygonFillColorButton.Click += new System.EventHandler(this.polygonButton_Click);
			// 
			// ligthLabel
			// 
			this.ligthLabel.AutoSize = true;
			this.ligthLabel.BackColor = System.Drawing.SystemColors.Window;
			this.ligthLabel.Location = new System.Drawing.Point(1075, 28);
			this.ligthLabel.Name = "ligthLabel";
			this.ligthLabel.Size = new System.Drawing.Size(30, 13);
			this.ligthLabel.TabIndex = 6;
			this.ligthLabel.Text = "Light";
			// 
			// fillColorLabel
			// 
			this.fillColorLabel.AutoSize = true;
			this.fillColorLabel.BackColor = System.Drawing.SystemColors.Window;
			this.fillColorLabel.Location = new System.Drawing.Point(1132, 28);
			this.fillColorLabel.Name = "fillColorLabel";
			this.fillColorLabel.Size = new System.Drawing.Size(25, 13);
			this.fillColorLabel.TabIndex = 7;
			this.fillColorLabel.Text = "  Fill";
			// 
			// fillTypeGroupbox
			// 
			this.fillTypeGroupbox.Controls.Add(this.bumpMapRadiobutton);
			this.fillTypeGroupbox.Controls.Add(this.textureRadiobutton);
			this.fillTypeGroupbox.Controls.Add(this.colorRadiobutton);
			this.fillTypeGroupbox.Location = new System.Drawing.Point(1075, 124);
			this.fillTypeGroupbox.Name = "fillTypeGroupbox";
			this.fillTypeGroupbox.Size = new System.Drawing.Size(99, 94);
			this.fillTypeGroupbox.TabIndex = 8;
			this.fillTypeGroupbox.TabStop = false;
			this.fillTypeGroupbox.Text = "Fill";
			// 
			// colorRadiobutton
			// 
			this.colorRadiobutton.AutoSize = true;
			this.colorRadiobutton.Location = new System.Drawing.Point(7, 20);
			this.colorRadiobutton.Name = "colorRadiobutton";
			this.colorRadiobutton.Size = new System.Drawing.Size(49, 17);
			this.colorRadiobutton.TabIndex = 0;
			this.colorRadiobutton.Text = "Color";
			this.colorRadiobutton.UseVisualStyleBackColor = true;
			// 
			// textureRadiobutton
			// 
			this.textureRadiobutton.AutoSize = true;
			this.textureRadiobutton.Checked = true;
			this.textureRadiobutton.Location = new System.Drawing.Point(7, 43);
			this.textureRadiobutton.Name = "textureRadiobutton";
			this.textureRadiobutton.Size = new System.Drawing.Size(61, 17);
			this.textureRadiobutton.TabIndex = 1;
			this.textureRadiobutton.TabStop = true;
			this.textureRadiobutton.Text = "Texture";
			this.textureRadiobutton.UseVisualStyleBackColor = true;
			// 
			// bumpMapRadiobutton
			// 
			this.bumpMapRadiobutton.AutoSize = true;
			this.bumpMapRadiobutton.Location = new System.Drawing.Point(7, 66);
			this.bumpMapRadiobutton.Name = "bumpMapRadiobutton";
			this.bumpMapRadiobutton.Size = new System.Drawing.Size(75, 17);
			this.bumpMapRadiobutton.TabIndex = 3;
			this.bumpMapRadiobutton.Text = "Bump map";
			this.bumpMapRadiobutton.UseVisualStyleBackColor = true;
			// 
			// polygonUnionToolStripMenuItem
			// 
			this.polygonUnionToolStripMenuItem.Name = "polygonUnionToolStripMenuItem";
			this.polygonUnionToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
			this.polygonUnionToolStripMenuItem.Text = "Polygon union";
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
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.ClientSize = new System.Drawing.Size(1179, 552);
			this.Controls.Add(this.fillTypeGroupbox);
			this.Controls.Add(this.fillColorLabel);
			this.Controls.Add(this.ligthLabel);
			this.Controls.Add(this.polygonFillColorButton);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.normalVectorGroupbox);
			this.Controls.Add(this.textureButton);
			this.Controls.Add(this.lightColorButton);
			this.Controls.Add(this.background);
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
	}
}

