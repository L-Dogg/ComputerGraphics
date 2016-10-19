using GK1.Controls;
using GK1.States;
using GK1.Structures;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GK1
{
	/// <summary>
	/// Main window form.
	/// </summary>
	public partial class MainForm : Form
	{
		#region Public Fields

		/// <summary>
		/// Current control state.
		/// </summary>
		public IState CurrentState { get; set; }

		/// <summary>
		/// Polygon that was chosen by user.
		/// </summary>
		public Polygon CurrentPolygon { get; set; } = new Polygon();

		/// <summary>
		/// Segment that was clicked by user.
		/// </summary>
		public Segment CurrentSegment { get; set; }

		/// <summary>
		/// Polygons to render on bitmap;
		/// </summary>
		public List<Polygon> Polygons { get; set; } = new List<Polygon>();

		/// <summary>
		/// Context menu with relation types.
		/// </summary>
		public ContextMenuStrip RelationContextMenu { get { return this.relationContextMenu; } }

		/// <summary>
		/// Idle context menu with polygon operations.
		/// </summary>
		public ContextMenuStrip PolygonContextMenu { get { return this.polygonContextMenu; } }
		
		/// <summary>
		/// Length MessageBox for Length Relation user input.
		/// </summary>
		public Length LengthMessageBox { get; set; }
		#endregion

		private Graphics g;

		#region Public Methods

		public MainForm()
		{
			InitializeComponent();
			background.BackgroundImage = new Bitmap(background.Size.Width, background.Size.Height);
			g = Graphics.FromImage(background.BackgroundImage);
			CurrentState = new IdleState(this);
			Render();
		}

		public void SetContextMenuItems(Segment operatingLine)
		{
			if (Polygons.Count == 0)
			{
				polygonContextMenu.Items[1].Enabled = polygonContextMenu.Items[2].Enabled = false;
			}
			else
			{
				polygonContextMenu.Items[1].Enabled = polygonContextMenu.Items[2].Enabled = true;
			}

			if (operatingLine != null)
			{
				for (int i = 0; i < relationContextMenu.Items.Count - 1; i++)
					relationContextMenu.Items[i].Enabled = (operatingLine.Relation == RelationType.None);
				relationContextMenu.Items[3].Enabled = (operatingLine.Relation != RelationType.None);
            }
		}

		public void Render()
		{
			this.ClearBitmap(background.BackgroundImage as Bitmap, g);
			CurrentState.Render(background.BackgroundImage as Bitmap, g);

			this.background.Invalidate(true);
		}
		#endregion

		#region Private Methods

		private void bgMouseDown(object sender, MouseEventArgs e)
		{
			CurrentState.MouseDown(sender, e);
		}

		private void bgMouseMove(object sender, MouseEventArgs e)
		{
			CurrentState.MouseMove(sender, e);
		}

		private void background_KeyUp(object sender, KeyEventArgs e)
		{
			CurrentState.KeyUp(sender, e);
		}

		private void ClearBitmap(Bitmap bmp, Graphics g)
		{
			g.FillRectangle(Brushes.White, 0, 0, bmp.Size.Width, bmp.Size.Height);
		}
		#endregion

		private void MainForm_ResizeEnd(object sender, EventArgs e)
		{
			background.Size = new Size(this.Size.Width, this.Size.Height);
			background.BackgroundImage.Dispose();
			background.BackgroundImage = new Bitmap(background.Size.Width, background.Size.Height);
			g = Graphics.FromImage(background.BackgroundImage);

			//background.Invalidate(true);
			this.Render();
		}
	}
}
