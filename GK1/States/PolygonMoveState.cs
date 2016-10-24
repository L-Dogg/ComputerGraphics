﻿using GK1.Structures;
using GK1.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GK1.States
{
	// TODO: can't move out of state!!!
	class PolygonMoveState : IState
	{
		#region Private Properties
		private MainForm MainForm { get; set; }
		private Polygon Polygon { get; set; }
		private Vertex Vertex { get; set; }
		private bool Moving { get; set; }
		#endregion

		public PolygonMoveState(MainForm mainForm)
		{
			MainForm = mainForm;
			MainForm.Cursor = Cursors.NoMove2D;
			Moving = false;
		}

		#region IState
		public void MouseDown(object sender, MouseEventArgs e)
		{
			var point = new Vertex(e.X, e.Y);
			Polygon polygon;
			Vertex vertex;
			var wasVertexClicked = ClickChecker.WasVertexClicked(point, MainForm.Polygons, out vertex, out polygon);

			if (!Moving && wasVertexClicked && e.Button == MouseButtons.Left)
			{
				Vertex = vertex;
				Polygon = polygon;
				Moving = true;
			}
			else if (Moving && e.Button == MouseButtons.Left)
			{
				Moving = false;
				MainForm.CurrentState = new IdleState(MainForm);
			}
		}

		public void MouseMove(object sender, MouseEventArgs e)
		{
			if (!Moving)
				return;

			var xDiff = e.X - Vertex.X;
			var yDiff = e.Y - Vertex.Y;

			Vertex = new Vertex(Vertex.X + xDiff, Vertex.Y + yDiff);

			var pts = new LinkedList<Vertex>();
			foreach (var p in Polygon.Vertices)
				pts.AddLast(new Vertex(p.X + xDiff, p.Y + yDiff));
			Polygon.Vertices = pts;

			foreach (var line in Polygon.Segments)
			{
				line.To = new Vertex(line.To.X + xDiff, line.To.Y + yDiff);
				line.From = new Vertex(line.From.X + xDiff, line.From.Y + yDiff);
			}

			MainForm.Render();	
		}

		public void Render(Bitmap bitmap, Graphics g)
		{
			foreach (var polygon in MainForm.Polygons)
				polygon.Render(bitmap, g);
		}

		public void KeyUp(object sender, KeyEventArgs e)
		{
			
		}

		#endregion
	}
}
