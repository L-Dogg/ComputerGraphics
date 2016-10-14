using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Bresenhams;

namespace Projekt_1
{
	public partial class Form1 : Form
	{
		#region Private Fields
		private Polygon curPoly = new Polygon();
		private List<Polygon> polygons = new List<Polygon>();

		private bool moving = false;
		private bool drawing = false;
		private Polygon polygonToMove;
		private Point pointToMove;
		/// <summary>
		/// Punkty, któr
		/// </summary>
		private Point[] adjacentPoints = new Point[2];

		private Graphics graphics;
		private Pen edgePen = new Pen(Color.Black);
		private Brush pointBrush = Brushes.Red;

        private const int pointSize = 5;
		#endregion 

		public Form1()
		{
			InitializeComponent();
			background1.BackgroundImage = new Bitmap(background1.Size.Width, background1.Size.Height); ;
			graphics = Graphics.FromImage(background1.BackgroundImage);
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
					curPoly.lines.Add(line);
					graphics.DrawLine(edgePen, line.start, line.end);
					Algorithms.Line(line.start.X, line.start.Y, line.end.X, line.end.Y, new Algorithms.PlotFunction((x, y) => { graphics.FillEllipse(Brushes.Red, x, y, 1, 1); return true; }));
					Redraw();

					if (curPoly.points[0].ComparePoints(point))
					{
						drawing = false;
						polygons.Add(curPoly);
						curPoly = new Polygon();
					}
					else
					{
						curPoly.points.Add(point);
						graphics.FillEllipse(pointBrush, e.X, e.Y, pointSize, pointSize);
					}

				}
				else
				{
					curPoly.points.Add(point);
					graphics.FillEllipse(pointBrush, e.X, e.Y, pointSize, pointSize);
				}
			}
			else if (!drawing && !moving && e.Button == MouseButtons.Left && WasVertexClicked())
			{
				moving = true;
				polygons.Remove(polygonToMove);
				polygonToMove.points.Remove(pointToMove);
				FindAdjacentPoints(pointToMove, polygonToMove);
				polygonToMove.lines = polygonToMove.lines.Where((line) => { return line.start != pointToMove && line.end != pointToMove; }).ToList();
			}
			else if (moving && e.Button == MouseButtons.Left)
			{
				moving = false;
				polygonToMove.points.Add(point);
				polygonToMove.lines.Add(new Line(point, adjacentPoints[0]));
				polygonToMove.lines.Add(new Line(point, adjacentPoints[1]));
				polygons.Add(polygonToMove);
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
				graphics.DrawLine(edgePen, curPoly.points.Last(), e.Location);
			}
			else if (moving)
			{
				var point = new Point(e.X, e.Y);
				Redraw();
				graphics.FillEllipse(pointBrush, e.X, e.Y, pointSize, pointSize);
				graphics.DrawLine(edgePen, adjacentPoints[0], point);
				graphics.DrawLine(edgePen, adjacentPoints[1], point);
			}
		}

		#endregion

		private void Redraw()
        {
			background1.BackgroundImage = new Bitmap(background1.Size.Width, background1.Size.Height);
			this.graphics = Graphics.FromImage(background1.BackgroundImage);
			curPoly.Draw(this.graphics);
            foreach (var poly in this.polygons)
            {
				poly.Draw(this.graphics);
            }
			if (moving)
			{
				polygonToMove.Draw(this.graphics);
			}
        }

		private bool WasVertexClicked()
		{
			foreach (var poly in this.polygons)
			{
				foreach (var point in poly.points)
				{
					if (point.ComparePoints(point))
					{
						pointToMove = point;
						polygonToMove = poly;
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
    }
}
