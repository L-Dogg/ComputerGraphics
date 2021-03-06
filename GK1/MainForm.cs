﻿using GK1.Controls;
using GK1.States;
using GK1.Structures;
using System;
using System.Collections.Generic;
using System.Drawing;
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

		private Graphics _graphics;

		#region Public Methods

		public MainForm()
		{
			InitializeComponent();
			background.BackgroundImage = new Bitmap(background.Size.Width, background.Size.Height);
			_graphics = Graphics.FromImage(background.BackgroundImage);
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
				//for (int i = 0; i < relationContextMenu.Items.Count - 1; i++)
				//	relationContextMenu.Items[i].Enabled = (operatingLine.Relation.Type == RelationType.None);
				relationContextMenu.Items[4].Enabled = (operatingLine.Relation.Type != RelationType.None);
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
		#endregion
	}
}
