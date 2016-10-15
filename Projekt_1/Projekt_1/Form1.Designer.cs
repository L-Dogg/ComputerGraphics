namespace Projekt_1
{
	partial class Form1
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
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.dodajWielokątToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.usuńWielokątToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
			this.background1 = new Projekt_1.Background();
			this.contextMenuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dodajWielokątToolStripMenuItem,
            this.usuńWielokątToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(153, 48);
			this.contextMenuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStrip1_ItemClicked);
			// 
			// dodajWielokątToolStripMenuItem
			// 
			this.dodajWielokątToolStripMenuItem.Name = "dodajWielokątToolStripMenuItem";
			this.dodajWielokątToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.dodajWielokątToolStripMenuItem.Text = "Dodaj wielokąt";
			// 
			// usuńWielokątToolStripMenuItem
			// 
			this.usuńWielokątToolStripMenuItem.Name = "usuńWielokątToolStripMenuItem";
			this.usuńWielokątToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.usuńWielokątToolStripMenuItem.Text = "Usuń wielokąt";
			// 
			// background1
			// 
			this.background1.Location = new System.Drawing.Point(3, 2);
			this.background1.Name = "background1";
			this.background1.Size = new System.Drawing.Size(1151, 582);
			this.background1.TabIndex = 1;
			this.background1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.background1_MouseDown);
			this.background1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.background1_MouseMove);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1157, 586);
			this.Controls.Add(this.background1);
			this.Margin = new System.Windows.Forms.Padding(2);
			this.Name = "Form1";
			this.Text = "Grafika Komputerowa - Szymon Adach";
			this.contextMenuStrip1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem dodajWielokątToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem usuńWielokątToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Background background1;
    }
}

