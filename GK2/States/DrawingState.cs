﻿
using GK2.Structures;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GK2.States
{
	// TODO BUG NA DRUGIM POLYGONIE
	public class DrawingState : IState
	{
		#region Private Properties
		private MainForm MainForm { get; set; }
		private Point Point { get; set; }
		private static bool Moving { get; set; }


		#region Lab part
		private static bool ToggleRelationHelper { get; set; } = false;
		private static readonly string RelationHelperMessage = "Relation helper is: ";
		private static readonly int Margin = 3;
		#endregion
		#endregion

		public DrawingState(MainForm mainForm)
		{
			MainForm = mainForm;
			MainForm.Cursor = Cursors.Cross;
		}

		#region IState
		public void MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left)
				return;
			var point = new Vertex(e.X, e.Y);
			var polygon = MainForm.CurrentPolygon;
			
			Moving = false;

			if (polygon.Vertices.Count != 0)
			{
				// Finish drawing 
				if (polygon.Vertices.First.Value.ComparePoints(point))
				{
					polygon.Segments.AddLast(new Segment(polygon.Vertices.Last.Value, polygon.Vertices.First.Value));
					MainForm.Polygons.Add(polygon);
					polygon.Finished = true;
					MainForm.CurrentPolygon = new Polygon();
					MainForm.CurrentState = new IdleState(MainForm);
				}
				else
				{
					polygon.Segments.AddLast(new Segment(polygon.Vertices.Last.Value, point));
					polygon.Vertices.AddLast(point);
				}
				
            }
			else
			{
				polygon.Vertices.AddLast(point);
			}
			MainForm.Render();
			
		}

		public void MouseMove(object sender, MouseEventArgs e)
		{
			if (MainForm.CurrentPolygon.Vertices.Count == 0)
				return;

			Moving = true;
			Point = new Point(e.X, e.Y);
			MainForm.Render();
		}

		public void Render(Bitmap bitmap, Graphics g)
		{
			if (Moving)
			{

				if (ToggleRelationHelper)
				{

					var midPoint = new Point(Math.Min(MainForm.CurrentPolygon.Vertices.Last.Value.X, Point.X) + Math.Abs(MainForm.CurrentPolygon.Vertices.Last.Value.X - Point.X) / 2,
						Math.Min(MainForm.CurrentPolygon.Vertices.Last.Value.Y, Point.Y) + Math.Abs(MainForm.CurrentPolygon.Vertices.Last.Value.Y - Point.Y) / 2);

					// Horizontal
					if (Math.Abs(MainForm.CurrentPolygon.Vertices.Last.Value.Y - Point.Y) <= Margin)
					{
						g.FillEllipse(Brushes.Red, midPoint.X - 7, midPoint.Y - 7, 15, 15);
					}
					// Vertical
					else if (Math.Abs(MainForm.CurrentPolygon.Vertices.Last.Value.X - Point.X) <= Margin)
					{
						g.FillEllipse(Brushes.Green, midPoint.X - 7, midPoint.Y - 7, 15, 15);
					}
                }
				Algorithms.Algorithms.Line(MainForm.CurrentPolygon.Vertices.Last.Value.X, MainForm.CurrentPolygon.Vertices.Last.Value.Y, Point.X, Point.Y, bitmap);
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
			else if(e.KeyCode == Keys.F12)
			{
				ToggleRelationHelper = !ToggleRelationHelper;
				MessageBox.Show(RelationHelperMessage + (ToggleRelationHelper ? "ON" : "OFF"));
			}
		}
		#endregion
	}
}
