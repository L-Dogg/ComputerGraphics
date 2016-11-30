
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
			
			Polygon.FillTexture = DirectBitmap.FromBitmap(new Bitmap("../../Resources/lenna.bmp"));
			_directBitmap =	new DirectBitmap(background.Size.Width, background.Size.Height);
			background.BackgroundImage = _directBitmap.Bitmap;
			_graphics = Graphics.FromImage(background.BackgroundImage);

			Polygon.BumpMap = DirectBitmap.FromBitmap(new Bitmap("../../Resources/bumpmap.bmp"));
			Polygon.NormalMap = DirectBitmap.FromBitmap(new Bitmap("../../Resources/normalmap.bmp"));

			lightColorButton.BackColor = Polygon.LightColor;
			polygonFillColorButton.BackColor = Polygon.FillColor;

			lightAnimationTimer.Tick += LightTimerProcessor;
			lightAnimationTimer.Interval = LightTimerInterval;

			colorRadiobutton.CheckedChanged += fillRadioButtons_CheckedChanged;
			textureRadiobutton.CheckedChanged += fillRadioButtons_CheckedChanged;
			bumpMapRadiobutton.CheckedChanged += fillRadioButtons_CheckedChanged;

			constantLightRadiobutton.CheckedChanged += lightRadioButtons_CheckedChanged;
			animatedLightRadiobutton.CheckedChanged += lightRadioButtons_CheckedChanged;

			PopulateScreen();
			textBox1.Text = Polygon.LightVector.Z.ToString();
			CurrentState = new IdleState(this);
			Render();
		}

		/// <summary>
		/// Adds two intersecting polygons (triangles).
		/// </summary>
		private void PopulateScreen()
		{
			// Weiler-Atherton test:
			var p = new Polygon();
			var vtx = new List<Vertex>() { new Vertex(250, 250), new Vertex(115, 215), new Vertex(450, 50) };
			p.Vertices.AddLast(new LinkedListNode<Vertex>(vtx[0]));
			p.Vertices.AddLast(new LinkedListNode<Vertex>(vtx[1]));
			p.Vertices.AddLast(new LinkedListNode<Vertex>(vtx[2]));
			p.Segments.AddLast(new LinkedListNode<Segment>(new Segment(vtx[0], vtx[1])));
			p.Segments.AddLast(new LinkedListNode<Segment>(new Segment(vtx[1], vtx[2])));
			p.Segments.AddLast(new LinkedListNode<Segment>(new Segment(vtx[2], vtx[0])));
			p.Finished = true;
			Polygons.Add(p);

			var u = new Polygon();
			var utx = new List<Vertex>() { new Vertex(350, 350), new Vertex(225, 175), new Vertex(333, 222) };
			u.Vertices.AddLast(new LinkedListNode<Vertex>(utx[0]));
			u.Vertices.AddLast(new LinkedListNode<Vertex>(utx[1]));
			u.Vertices.AddLast(new LinkedListNode<Vertex>(utx[2]));
			u.Segments.AddLast(new LinkedListNode<Segment>(new Segment(utx[0], utx[1])));
			u.Segments.AddLast(new LinkedListNode<Segment>(new Segment(utx[1], utx[2])));
			u.Segments.AddLast(new LinkedListNode<Segment>(new Segment(utx[2], utx[0])));
			u.Finished = true;
			Polygons.Add(u);

			// Light test:
			//var p = new Polygon();
			//var vtx = new List<Vertex>() { new Vertex(5, 5), new Vertex(800, 5), new Vertex(800, 500), new Vertex(5 , 500)};
			//p.Vertices.AddLast(new LinkedListNode<Vertex>(vtx[0]));
			//p.Vertices.AddLast(new LinkedListNode<Vertex>(vtx[1]));
			//p.Vertices.AddLast(new LinkedListNode<Vertex>(vtx[2]));
			//p.Vertices.AddLast(new LinkedListNode<Vertex>(vtx[3]));
			//p.Segments.AddLast(new LinkedListNode<Segment>(new Segment(vtx[0], vtx[1])));
			//p.Segments.AddLast(new LinkedListNode<Segment>(new Segment(vtx[1], vtx[2])));
			//p.Segments.AddLast(new LinkedListNode<Segment>(new Segment(vtx[2], vtx[3])));
			//p.Segments.AddLast(new LinkedListNode<Segment>(new Segment(vtx[3], vtx[0])));
			//p.Finished = true;
			//Polygons.Add(p);
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

		private void bumpMapButton_Click(object sender, EventArgs e)
		{
			var result = _openFileDialog.ShowDialog();
			if (result != DialogResult.OK)
				return;
			Polygon.BumpMap = DirectBitmap.FromBitmap(new Bitmap(_openFileDialog.FileName));
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
				Polygon.LightVector.X = 0;
				Polygon.LightVector.Y = 0;
				Polygon.LightVector.Z = 1;

				this.Render();
			}
			else if (animatedLightRadiobutton.Checked)
			{
				Polygon.LightVector.Z = 100;
				lightAnimationTimer.Start();
				Radius = (double)background.BackgroundImage.Height / 2;
			}
			textBox1.Text = Polygon.LightVector.Z.ToString();
		}

		private static double Radius;
		private static double s;

		// TODO: ustalic dobrze wspolrzedne X, Y
		private void LightTimerProcessor(object sender, EventArgs e)
		{
			s = (s + 1)%3600;
			Polygon.LightVector.X = Math.Cos(s/10)*Radius + (double)this.background.BackgroundImage.Width / 2;
			Polygon.LightVector.Y = Math.Cos(s/10)*Radius + (double)this.background.BackgroundImage.Height / 2;
			
			this.Render();
		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			double z;
			if (double.TryParse(this.textBox1.Text, out z))
				Polygon.LightVector.Z = z;

		}
		#endregion

	}
}
