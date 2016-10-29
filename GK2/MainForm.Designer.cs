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
			this.background = new GK2.Controls.Background();
			this.polygonContextMenu.SuspendLayout();
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
			this.polygonContextMenu.Size = new System.Drawing.Size(198, 82);
			// 
			// addPolygonToolStripMenuItem
			// 
			this.addPolygonToolStripMenuItem.Name = "addPolygonToolStripMenuItem";
			this.addPolygonToolStripMenuItem.Size = new System.Drawing.Size(197, 26);
			this.addPolygonToolStripMenuItem.Text = "Add polygon";
			// 
			// removePolygonToolStripMenuItem
			// 
			this.removePolygonToolStripMenuItem.Name = "removePolygonToolStripMenuItem";
			this.removePolygonToolStripMenuItem.Size = new System.Drawing.Size(197, 26);
			this.removePolygonToolStripMenuItem.Text = "Remove polygon";
			// 
			// movePolygonToolStripMenuItem
			// 
			this.movePolygonToolStripMenuItem.Name = "movePolygonToolStripMenuItem";
			this.movePolygonToolStripMenuItem.Size = new System.Drawing.Size(197, 26);
			this.movePolygonToolStripMenuItem.Text = "Move polygon";
			// 
			// background
			// 
			this.background.Location = new System.Drawing.Point(0, 0);
			this.background.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
			this.background.Name = "background";
			this.background.Size = new System.Drawing.Size(1409, 679);
			this.background.TabIndex = 0;
			this.background.KeyUp += new System.Windows.Forms.KeyEventHandler(this.background_KeyUp);
			this.background.MouseDown += new System.Windows.Forms.MouseEventHandler(this.bgMouseDown);
			this.background.MouseMove += new System.Windows.Forms.MouseEventHandler(this.bgMouseMove);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1409, 679);
			this.Controls.Add(this.background);
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Name = "MainForm";
			this.Text = "Polygon Editor";
			this.ResizeEnd += new System.EventHandler(this.MainForm_ResizeEnd);
			this.Resize += new System.EventHandler(this.MainForm_Resize);
			this.polygonContextMenu.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private Background background;
		private System.Windows.Forms.ContextMenuStrip polygonContextMenu;
		private System.Windows.Forms.ToolStripMenuItem addPolygonToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem removePolygonToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem movePolygonToolStripMenuItem;
	}
}

