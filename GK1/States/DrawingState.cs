﻿using Bresenhams;
using GK1.Structures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GK1.States
{
	class DrawingState : IState
	{
		#region Private Properties
		private MainForm MainForm { get; set; }
		private Segment Segment { get; set; }
		private Point Point { get; set; }
		private bool Moving { get; set; }
		#endregion

		public DrawingState(MainForm mainForm)
		{
			MainForm = mainForm;
			MainForm.Cursor = Cursors.Cross;
		}

		#region IState
		public void MouseDown(object sender, MouseEventArgs e)
		{
			var point = new Point(e.X, e.Y);
			var polygon = MainForm.CurrentPolygon;

            if (e.Button == MouseButtons.Left)
			{
				Moving = false;

				if (polygon.Points.Count != 0)
				{
					// Finish drawing 
					if (polygon.Points.First.Value.ComparePoints(point))
					{
						polygon.Segments.AddLast(new Segment(polygon.Points.Last.Value, polygon.Points.First.Value));
						MainForm.Polygons.Add(polygon);
						MainForm.CurrentPolygon = new Polygon();
						MainForm.CurrentState = new IdleState(MainForm);
					}
					else
					{
						polygon.Segments.AddLast(new Segment(polygon.Points.Last.Value, point));
						polygon.Points.AddLast(point);
						MainForm.Render();
					}

					MainForm.Render();
				}
				else
				{
					polygon.Points.AddLast(point);
					MainForm.Render();
				}
			}
		}

		public void MouseMove(object sender, MouseEventArgs e)
		{
			if (MainForm.CurrentPolygon.Points.Count > 0)
			{
				Moving = true;
				Point = new Point(e.X, e.Y);
				MainForm.Render();
			}
		}

		public void Render(Bitmap bitmap, Graphics g)
		{
			if (Moving)
			{
				Algorithms.Line(MainForm.CurrentPolygon.Points.Last.Value.X, MainForm.CurrentPolygon.Points.Last.Value.Y, Point.X, Point.Y, bitmap);
			}

			MainForm.CurrentPolygon.Render(bitmap, g);

			foreach (var polygon in MainForm.Polygons)
				polygon.Render(bitmap, g);
		}

		public void KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				MainForm.CurrentPolygon = new Polygon();
				MainForm.CurrentState = new IdleState(MainForm);
				MainForm.Render();
			}
		}
		#endregion
	}
}
