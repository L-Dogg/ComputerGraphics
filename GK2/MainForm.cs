
using GK2.States;
using GK2.Structures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using GK2.Utilities;

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

		public bool ColorFill => this.colorRadiobutton.Checked;
		public bool TextureFill => this.textureRadiobutton.Checked;
		public bool BumpMapping => this.bumpMapRadiobutton.Checked;

		#endregion

		#region Private Fields
		private Graphics _graphics;
		private DirectBitmap _directBitmap;
		private readonly ColorDialog _colorDialog = new ColorDialog();
		private readonly OpenFileDialog _openFileDialog = new OpenFileDialog() { Title = "Open bitmap", Filter = "bmp files (*.bmp)|*.bmp"};
		#endregion

		private static Timer lightAnimationTimer = new Timer();

		private static int LightTimerInterval = 100;

		#region Public Methods

		public MainForm()
		{
			InitializeComponent();

            Polygon.FillTexture = DirectBitmap.FromBitmap(new Bitmap("../../Resources/pepe.bmp"));
			_directBitmap =	new DirectBitmap(background.Size.Width, background.Size.Height);
			background.BackgroundImage = _directBitmap.Bitmap;
			_graphics = Graphics.FromImage(background.BackgroundImage);

			Polygon.HeightMap = DirectBitmap.FromBitmap(new Bitmap("../../Resources/heightmap.bmp"));

			lightColorButton.BackColor = Polygon.LightColor;
			polygonFillColorButton.BackColor = Polygon.FillColor;

			lightAnimationTimer.Tick += new EventHandler(LightTimerProcessor);
			lightAnimationTimer.Interval = LightTimerInterval;

			colorRadiobutton.CheckedChanged += new EventHandler(fillRadioButtons_CheckedChanged);
			textureRadiobutton.CheckedChanged += new EventHandler(fillRadioButtons_CheckedChanged);
			bumpMapRadiobutton.CheckedChanged += new EventHandler(fillRadioButtons_CheckedChanged);

			constantLightRadiobutton.CheckedChanged += new EventHandler(lightRadioButtons_CheckedChanged);
			animatedLightRadiobutton.CheckedChanged += new EventHandler(lightRadioButtons_CheckedChanged);

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

			polygonContextMenu.Items[3].Enabled = Polygons.Count >= 2;
		}

		public void Render()
		{
			this.ClearBitmap(_directBitmap.Bitmap, _graphics);

			//_directBitmap.Dispose();
			//_directBitmap = new DirectBitmap(background.Size.Width, background.Size.Height);
			//background.BackgroundImage = _directBitmap.Bitmap;

			CurrentState.Render(_directBitmap, _graphics);
			
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
			
			_directBitmap.Dispose();
			_directBitmap = new DirectBitmap(background.Size.Width, background.Size.Height);
			background.BackgroundImage = _directBitmap.Bitmap;
			_graphics = Graphics.FromImage(background.BackgroundImage);

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
			var result = _openFileDialog.ShowDialog();
			if (result != DialogResult.OK)
				return;
			Polygon.FillTexture = DirectBitmap.FromBitmap(new Bitmap(_openFileDialog.FileName));
			this.Render();
		}

		private void colorButton_Click(object sender, EventArgs e)
		{
			var result = _colorDialog.ShowDialog();
			if (result != DialogResult.OK)
				return;
			Polygon.LightColor = _colorDialog.Color;
			this.lightColorButton.BackColor = _colorDialog.Color;
			this.Render();
		}

		private void polygonButton_Click(object sender, EventArgs e)
		{
			var result = _colorDialog.ShowDialog();
			if (result != DialogResult.OK)
				return;
			Polygon.FillColor = _colorDialog.Color;
			this.polygonFillColorButton.BackColor = _colorDialog.Color;
			this.Render();
		}

		private void fillRadioButtons_CheckedChanged(object sender, EventArgs e)
		{
			this.Render();
		}

		private void lightRadioButtons_CheckedChanged(object sender, EventArgs e)
		{
			if (constantLightRadiobutton.Checked)
			{
				lightAnimationTimer.Stop();
				Polygon.LightX = 0;
				Polygon.LightY = 0;
				Polygon.LightZ = 1;

				this.Render();
			}
			else if (animatedLightRadiobutton.Checked)
			{
				lightAnimationTimer.Start();
			}
		}

		private static double Radius = 1.0;
		private static double s = 0;
		private static double t = -90;

		private void LightTimerProcessor(object sender, EventArgs e)
		{
			s = (s + 1)%360;
			t = (t + 1 <= 90) ? t + 1 : -90;
			//t = (t + 1)%180;

			Polygon.LightX = Radius * Math.Cos(s * Math.PI / 180) * Math.Sin(t * Math.PI / 180);
			Polygon.LightY = Radius * Math.Cos(s * Math.PI / 180) * Math.Sin(t * Math.PI / 180);
			Polygon.LightZ = Radius * Math.Cos(t * Math.PI / 180);

			this.Render();
		}
		#endregion

	}
}
