
using GK2.States;
using GK2.Structures;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GK2
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
		/// Idle context menu with polygon operations.
		/// </summary>
		public ContextMenuStrip PolygonContextMenu => this.polygonContextMenu;

		#endregion

		#region Private Fields
		private Graphics _graphics;
		private ColorDialog colorDialog = new ColorDialog();
		private OpenFileDialog openFileDialog = new OpenFileDialog() { Title = "Open bitmap", Filter = "bmp files (*.bmp)|*.bmp"};
		#endregion

		#region Public Methods

		public MainForm()
		{
			InitializeComponent();
			Polygon.DefaultFillTexture = (Bitmap) Image.FromFile("../../Resources/pepe.bmp");
			background.BackgroundImage = new Bitmap(background.Size.Width, background.Size.Height);
			_graphics = Graphics.FromImage(background.BackgroundImage);
			CurrentState = new IdleState(this);
			Render();
		}

		public void SetContextMenuItems()
		{
			if (Polygons.Count == 0)
			{
				polygonContextMenu.Items[1].Enabled = polygonContextMenu.Items[2].Enabled = false;
			}
			else
			{
				polygonContextMenu.Items[1].Enabled = polygonContextMenu.Items[2].Enabled = true;
			}
		}

		public void Render()
		{
			this.ClearBitmap(background.BackgroundImage as Bitmap, _graphics);
			CurrentState.Render(background.BackgroundImage as Bitmap, _graphics);
			
			this.background.Invalidate(true);
		}
		#endregion

		#region Private Methods

		private void BgMouseDown(object sender, MouseEventArgs e)
		{
			CurrentState.MouseDown(sender, e);
		}

		private void BgMouseMove(object sender, MouseEventArgs e)
		{
			CurrentState.MouseMove(sender, e);
		}

		private void background_KeyUp(object sender, KeyEventArgs e)
		{
			CurrentState.KeyUp(sender, e);
		}

		private void MainForm_ResizeEnd(object sender, EventArgs e)
		{
			if (background.BackgroundImage == null)
				return;

			background.Size = new Size(this.Size.Width, this.Size.Height);
			background.BackgroundImage.Dispose();
			background.BackgroundImage = new Bitmap(background.Size.Width, background.Size.Height);
			_graphics = Graphics.FromImage(background.BackgroundImage);

			//background.Invalidate(true);
			this.Render();
		}

		private void MainForm_Resize(object sender, EventArgs e)
		{
			MainForm_ResizeEnd(sender, e);
		}

		private void ClearBitmap(Bitmap bmp, Graphics g)
		{
			g.FillRectangle(Brushes.White, 0, 0, bmp.Size.Width, bmp.Size.Height);
		}

		private void textureButton_Click(object sender, EventArgs e)
		{
			var result = openFileDialog.ShowDialog();
			if (result != DialogResult.OK)
				return;
			Polygon.DefaultFillTexture = new Bitmap(openFileDialog.FileName);
			this.Render();
		}

		private void colorButton_Click(object sender, EventArgs e)
		{
			var result = colorDialog.ShowDialog();
			if (result != DialogResult.OK)
				return;
			Polygon.DefaultLightColor = colorDialog.Color;
			this.Render();
		}
		#endregion
	}
}
