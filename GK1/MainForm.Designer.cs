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
			this.background1 = new GK1.Background();
			this.idleContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.addPolygonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.removePolygonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.movePolygonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.relationContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.horizontalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.verticalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.lengthToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.idleContextMenu.SuspendLayout();
			this.relationContextMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// background1
			// 
			this.background1.Location = new System.Drawing.Point(13, 13);
			this.background1.Name = "background1";
			this.background1.Size = new System.Drawing.Size(1032, 527);
			this.background1.TabIndex = 0;
			// 
			// idleContextMenu
			// 
			this.idleContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addPolygonToolStripMenuItem,
            this.removePolygonToolStripMenuItem,
            this.movePolygonToolStripMenuItem});
			this.idleContextMenu.Name = "idleContextMenu";
			this.idleContextMenu.Size = new System.Drawing.Size(165, 70);
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
            this.lengthToolStripMenuItem});
			this.relationContextMenu.Name = "relationContextMenu";
			this.relationContextMenu.Size = new System.Drawing.Size(153, 92);
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
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1057, 552);
			this.Controls.Add(this.background1);
			this.Name = "MainForm";
			this.Text = "Form1";
			this.idleContextMenu.ResumeLayout(false);
			this.relationContextMenu.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private Background background1;
		private System.Windows.Forms.ContextMenuStrip idleContextMenu;
		private System.Windows.Forms.ToolStripMenuItem addPolygonToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem removePolygonToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem movePolygonToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip relationContextMenu;
		private System.Windows.Forms.ToolStripMenuItem horizontalToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem verticalToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem lengthToolStripMenuItem;
	}
}

