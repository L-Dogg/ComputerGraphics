namespace GK1
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
			this.background = new GK1.Background();
			this.polygonContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.addPolygonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.removePolygonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.movePolygonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.relationContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.horizontalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.verticalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.lengthToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.noneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.polygonContextMenu.SuspendLayout();
			this.relationContextMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// background
			// 
			this.background.Location = new System.Drawing.Point(13, 13);
			this.background.Name = "background";
			this.background.Size = new System.Drawing.Size(1032, 527);
			this.background.TabIndex = 0;
			this.background.MouseDown += new System.Windows.Forms.MouseEventHandler(this.bgMouseDown);
			this.background.MouseMove += new System.Windows.Forms.MouseEventHandler(this.bgMouseMove);
			// 
			// polygonContextMenu
			// 
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
			// relationContextMenu
			// 
			this.relationContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.horizontalToolStripMenuItem,
            this.verticalToolStripMenuItem,
            this.lengthToolStripMenuItem,
            this.toolStripSeparator1,
            this.noneToolStripMenuItem});
			this.relationContextMenu.Name = "relationContextMenu";
			this.relationContextMenu.Size = new System.Drawing.Size(153, 120);
			// 
			// horizontalToolStripMenuItem
			// 
			this.horizontalToolStripMenuItem.Name = "horizontalToolStripMenuItem";
			this.horizontalToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.horizontalToolStripMenuItem.Text = "Horizontal";
			// 
			// verticalToolStripMenuItem
			// 
			this.verticalToolStripMenuItem.Name = "verticalToolStripMenuItem";
			this.verticalToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.verticalToolStripMenuItem.Text = "Vertical";
			// 
			// lengthToolStripMenuItem
			// 
			this.lengthToolStripMenuItem.Name = "lengthToolStripMenuItem";
			this.lengthToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.lengthToolStripMenuItem.Text = "Length";
			// 
			// noneToolStripMenuItem
			// 
			this.noneToolStripMenuItem.Name = "noneToolStripMenuItem";
			this.noneToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.noneToolStripMenuItem.Text = "None";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1057, 552);
			this.Controls.Add(this.background);
			this.Name = "MainForm";
			this.Text = "Polygon Editor";
			this.polygonContextMenu.ResumeLayout(false);
			this.relationContextMenu.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private Background background;
		private System.Windows.Forms.ContextMenuStrip polygonContextMenu;
		private System.Windows.Forms.ToolStripMenuItem addPolygonToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem removePolygonToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem movePolygonToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip relationContextMenu;
		private System.Windows.Forms.ToolStripMenuItem horizontalToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem verticalToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem lengthToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem noneToolStripMenuItem;
	}
}

