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

		#region States
		private bool movingVertex = false;
		private bool drawing = false;
		private bool deletingPolygon = false;
		private bool movingPolygon = false;
		private bool userWantsToMovePolygon = false;
		#endregion

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
			// Pokaż menu kontekstowe:
			if (e.Button == MouseButtons.Right)
			{
				contextMenuStrip1.Show(this, point);
			}
			// Usuwanie wierzchołka LMB + CTRL
			else if (e.Button == MouseButtons.Left && Form.ModifierKeys == Keys.Control && WasVertexClicked(point))
			{
				polygons.Remove(operatingPolygon);
				if (operatingPolygon.points.Count == 3)
				{
					Redraw();
					return;
				}
				FindAdjacentPoints(pointToMove, operatingPolygon);
				operatingPolygon.points.Remove(pointToMove);
				operatingPolygon.lines = operatingPolygon.lines.Where((line) => { return line.start != pointToMove && line.end != pointToMove; }).ToList();
				operatingPolygon.lines.Add(new Line(adjacentPoints[0], adjacentPoints[1]));
				polygons.Add(operatingPolygon);
				Redraw();
			}
			// Zwykłe rysowanie
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
			// Tryb usuwania wielokąta (LMB na dowolną część wielokąta)
			else if (deletingPolygon && e.Button == MouseButtons.Left && (WasEdgeClicked(point) || WasVertexClicked(point)))
			{
				deletingPolygon = false;
				polygons.Remove(operatingPolygon);
				Redraw();
			}
			// Włączenie trybu przesuwania wierzchołka
			else if (!drawing && !movingPolygon && !movingVertex && e.Button == MouseButtons.Left && WasVertexClicked(point))
			{
				movingVertex = true;
				polygons.Remove(operatingPolygon);
				operatingPolygon.points.Remove(pointToMove);
				FindAdjacentPoints(pointToMove, operatingPolygon);
				operatingPolygon.lines = operatingPolygon.lines.Where((line) => { return line.start != pointToMove && line.end != pointToMove; }).ToList();
			}
			// Wyłączenie trybu przesuwania wierzchołka
			else if (movingVertex && e.Button == MouseButtons.Left)
			{
				movingVertex = false;
				operatingPolygon.points.Add(point);
				operatingPolygon.lines.Add(new Line(point, adjacentPoints[0]));
				operatingPolygon.lines.Add(new Line(point, adjacentPoints[1]));
				polygons.Add(operatingPolygon);
			}
			// Tryb dodawania wierzchołka w środku krawędzi
			else if (!drawing && !movingVertex && e.Button == MouseButtons.Left && WasEdgeClicked(point))
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
			// Rozpoczęcie przesuwania wierzchołka
			else if (!drawing && userWantsToMovePolygon && e.Button == MouseButtons.Left && WasVertexClicked(point))
			{
				userWantsToMovePolygon = false;
				movingPolygon = true;
            }
			// Zakończenie przesuwania wielokąta
			else if (!drawing && movingPolygon && e.Button == MouseButtons.Left)
			{
				movingPolygon = false;
				polygons.Add(operatingPolygon);
				Redraw();
			}
        }

		private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			if (e.ClickedItem == contextMenuStrip1.Items[0])
			{
				this.drawing = true;
			}
			if (e.ClickedItem == contextMenuStrip1.Items[1])
			{
				this.deletingPolygon = true;
			}
			if (e.ClickedItem == contextMenuStrip1.Items[2])
			{
				this.userWantsToMovePolygon = true;
				polygons.Remove(operatingPolygon);
			}
		}

		private void background1_MouseMove(object sender, MouseEventArgs e)
		{
			if (drawing && curPoly.points.Any())
			{
				Redraw();   
				Algorithms.Line(curPoly.points.Last().X, curPoly.points.Last().Y, e.Location.X, e.Location.Y, background1.BackgroundImage as Bitmap);
			}
			else if (movingVertex)
			{
				var point = new Point(e.X, e.Y);
				Redraw();
				point.Draw(background1.BackgroundImage as Bitmap);
				Algorithms.Line(adjacentPoints[0].X, adjacentPoints[0].Y, e.Location.X, e.Location.Y, background1.BackgroundImage as Bitmap);
				Algorithms.Line(adjacentPoints[1].X, adjacentPoints[1].Y, e.Location.X, e.Location.Y, background1.BackgroundImage as Bitmap);
			}
			else if (movingPolygon)
			{
				var xDiff = pointToMove.X - e.X;
				var yDiff = pointToMove.Y - e.Y;
				operatingPolygon.points.ForEach((Point p) => { p.X += xDiff; p.Y += yDiff; });
				operatingPolygon.lines.ForEach((Line line) => { line.end.X += xDiff; line.end.Y += yDiff;
																line.start.X += xDiff; line.start.Y += yDiff; });
				Redraw();
			}
		}

		#endregion

		#region Private Methods
		/// <summary>
		/// Ponowne wyrenderowanie grafiki.
		/// </summary>
		private void Redraw()
        {
			background1.BackgroundImage.Dispose();
			var bmp = new Bitmap(background1.Size.Width, background1.Size.Height);
			
			curPoly.Draw(bmp);

			// Optymalizacja (nie rysujemy wielokątów które się nie zmieniły)
			if (movingPolygon || movingVertex)
			{
				operatingPolygon.Draw(bmp);
				background1.BackgroundImage = bmp;
				return;
			}

			foreach (var poly in this.polygons)
            {
				poly.Draw(bmp);
            }
			background1.BackgroundImage = bmp;
		}

		/// <summary>
		/// Sprawdzenie czy został kliknięty pewien wierzchołek.
		/// W polach klasy zapisuje informacje, który wierzchołek jakiego wielokąta został kliknięty.
		/// </summary>
		/// <param name="clicked">Punkt kliknięcia.</param>
		/// <returns>Czy jakikolwiek wierzchołek został kliknięty.</returns>
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

		/// <summary>
		/// Sprawdzenie czy została kliknięty pewna krawędź.
		/// W polach klasy zapisuje informacje, która krawędź jakiego wielokąta został kliknięty.
		/// </summary>
		/// <param name="clicked">Punkt kliknięcia.</param>
		/// <returns>Czy jakakolwiek krawędź została kliknięta.</returns>
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

		/// <summary>
		/// Znajduje punkty będące końcami odcinków o poczatku w punkcie p.
		/// </summary>
		/// <param name="p">Początek odcinka.</param>
		/// <param name="poly">Rozważany wielokąt.</param>
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
