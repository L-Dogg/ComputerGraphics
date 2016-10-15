using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Bresenhams;
using System;

namespace Projekt_1
{
	public partial class Form1 : Form
	{
		#region Private Fields
		private Polygon curPoly = new Polygon();
		private List<Polygon> polygons = new List<Polygon>();

		private bool moving = false;
		private bool drawing = false;
		private Polygon operatingPolygon;
		private Point pointToMove;
		private Line lineToDivide;
		/// <summary>
		/// Punkty, które leżą przy krawędziach kończących się w obecenie przeciąganym punkcie.
		/// </summary>
		private Point[] adjacentPoints = new Point[2];

		private Pen edgePen = new Pen(Color.Black);
		private Brush pointBrush = Brushes.Red;

        private const int pointSize = 5;
		#endregion 

		public Form1()
		{
			InitializeComponent();
			background1.BackgroundImage = new Bitmap(background1.Size.Width, background1.Size.Height); ;
		}

		#region MouseEvents

		private void background1_MouseDown(object sender, MouseEventArgs e)
		{
			var point = new Point(e.X, e.Y);
			if (e.Button == MouseButtons.Right)
			{
				contextMenuStrip1.Show(this, point);
			}
			else if (drawing && e.Button == MouseButtons.Left)
			{
				if (curPoly.points.Count != 0)
				{
					var line = new Line(curPoly.points.Last(), point);
					Algorithms.Line(line.start.X, line.start.Y, line.end.X, line.end.Y, background1.BackgroundImage as Bitmap);

					if (curPoly.points[0].ComparePoints(point))
					{
						drawing = false;
						curPoly.lines.Add(new Line(curPoly.points.Last(), curPoly.points.First()));
						polygons.Add(curPoly);
						curPoly = new Polygon();
					}
					else
					{
						curPoly.lines.Add(line);
						curPoly.points.Add(point);
						point.Draw(background1.BackgroundImage as Bitmap);
					}

					Redraw();
				}
				else
				{
					curPoly.points.Add(point);
					point.Draw(background1.BackgroundImage as Bitmap);
				}
			}
			else if (!moving && !drawing && e.Button == MouseButtons.Left && WasEdgeClicked(point))
			{
				operatingPolygon.lines.Remove(lineToDivide);
				var midPoint = new Point(Math.Min(lineToDivide.start.X, lineToDivide.end.X) + Math.Abs(lineToDivide.start.X - lineToDivide.end.X) / 2, 
					Math.Min(lineToDivide.start.Y, lineToDivide.end.Y) +  Math.Abs(lineToDivide.start.Y - lineToDivide.end.Y) / 2);
				operatingPolygon.lines.Add(new Line(midPoint, lineToDivide.end));
				operatingPolygon.lines.Add(new Line(midPoint, lineToDivide.start));
				operatingPolygon.points.Add(midPoint);
				polygons.Remove(operatingPolygon);
				polygons.Add(operatingPolygon);
				Redraw();
			}
			else if (!drawing && !moving && e.Button == MouseButtons.Left && WasVertexClicked(point))
			{
				moving = true;
				polygons.Remove(operatingPolygon);
				operatingPolygon.points.Remove(pointToMove);
				FindAdjacentPoints(pointToMove, operatingPolygon);
				operatingPolygon.lines = operatingPolygon.lines.Where((line) => { return line.start != pointToMove && line.end != pointToMove; }).ToList();
			}
			else if (moving && e.Button == MouseButtons.Left)
			{
				moving = false;
				operatingPolygon.points.Add(point);
				operatingPolygon.lines.Add(new Line(point, adjacentPoints[0]));
				operatingPolygon.lines.Add(new Line(point, adjacentPoints[1]));
				polygons.Add(operatingPolygon);
			}
        }

		private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			if (e.ClickedItem == contextMenuStrip1.Items[0])
			{
				this.drawing = true;
			}
		}

		private void background1_MouseMove(object sender, MouseEventArgs e)
		{
			if (drawing && curPoly.points.Any())
			{
				Redraw();   
				Algorithms.Line(curPoly.points.Last().X, curPoly.points.Last().Y, e.Location.X, e.Location.Y, background1.BackgroundImage as Bitmap);
			}
			else if (moving)
			{
				var point = new Point(e.X, e.Y);
				Redraw();
				point.Draw(background1.BackgroundImage as Bitmap);
				Algorithms.Line(adjacentPoints[0].X, adjacentPoints[0].Y, e.Location.X, e.Location.Y, background1.BackgroundImage as Bitmap);
				Algorithms.Line(adjacentPoints[1].X, adjacentPoints[1].Y, e.Location.X, e.Location.Y, background1.BackgroundImage as Bitmap);
			}
		}

		#endregion

		#region Private Methods
		private void Redraw()
        {
			background1.BackgroundImage.Dispose();
			var bmp = new Bitmap(background1.Size.Width, background1.Size.Height);
			
			curPoly.Draw(bmp);

            foreach (var poly in this.polygons)
            {
				poly.Draw(bmp);
            }
			if (moving)
			{
				operatingPolygon.Draw(bmp);
			}
			background1.BackgroundImage = bmp;
        }

		private bool WasVertexClicked(Point clicked)
		{
			foreach (var poly in this.polygons)
			{
				foreach (var point in poly.points)
				{
					if (point.ComparePoints(clicked))
					{
						pointToMove = point;
						operatingPolygon = poly;
						return true;
					}
				}
			}
			return false;			
		}

		private bool WasEdgeClicked(Point clicked)
		{
			foreach (var poly in polygons)
			{
				foreach (var line in poly.lines)
				{
					if (clicked.IsCloseToLine(line))
					{
						operatingPolygon = poly;
						lineToDivide = line;
						return true;
					}
				}
			}
			return false;
		}

		private void FindAdjacentPoints(Point p, Polygon poly)
		{
			int i = 0;
			foreach (var line in poly.lines)
			{
				if (line.start == p)
					adjacentPoints[i++] = line.end;
				else if (line.end == p)
					adjacentPoints[i++] = line.start;

				if (i == 2)
					return;
			}
		}
		#endregion
	}
}
