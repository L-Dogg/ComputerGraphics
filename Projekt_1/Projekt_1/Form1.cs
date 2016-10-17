using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Bresenhams;
using System;
using Projekt_1.Structures;

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
		private bool relationChanged = false;
		#endregion

		#region Operating fields
		private Polygon operatingPolygon;
		private Point operatingPoint;
		private Line operatingLine;
		/// <summary>
		/// Punkty, które leżą przy krawędziach kończących się w obecenie przeciąganym punkcie.
		/// </summary>
		private Point[] adjacentPoints = new Point[2];
		/// <summary>
		/// Sąsiednie krawędzie. [0] - współdzieląca start, [1] - współdzieląca end.
		/// </summary>
		private Line[] adjacentEdges = new Line[2];
		private int operatingLineIndex = 0;
		#endregion

		private Pen edgePen = new Pen(Color.Black);
		private Brush pointBrush = Brushes.Red;
		private const string existingRelationMessage = "Relacja dla tej krawędzi została już zdefiniowana";
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
				var wasEdgeClicked = WasEdgeClicked(point);
                SetContextMenuItems();
				// Menu kotekstowe dla relacji
				if (wasEdgeClicked)
				{
					relationsContextMenu.Show(this, point);
				}
				// Podstawowe menu kontekstowe
				else
				{
					contextMenuStrip1.Show(this, point);
				}
				
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
				FindAdjacentPoints(operatingPoint, operatingPolygon);
				operatingPolygon.points.Remove(operatingPoint);
				var edgeToAddAfter = operatingPolygon.lines.Where((line) => { return line.start == operatingPoint || line.end == operatingPoint; }).First();
				operatingPolygon.lines = new LinkedList<Line>(operatingPolygon.lines.Where((line) => { return line.start != operatingPoint && line.end != operatingPoint; }));
				operatingPolygon.lines.AddAfter(new LinkedListNode<Line>(edgeToAddAfter), new Line(adjacentPoints[0], adjacentPoints[1]));
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
						curPoly.lines.AddLast(new Line(curPoly.points.Last(), curPoly.points.First()));
						polygons.Add(curPoly);
						curPoly = new Polygon();
					}
					else
					{
						curPoly.lines.AddLast(line);
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
			// Rozpoczęcie przesuwania wielokąta
			else if (!drawing && userWantsToMovePolygon && e.Button == MouseButtons.Left && WasVertexClicked(point))
			{
				userWantsToMovePolygon = false;
				polygons.Remove(operatingPolygon);
				movingPolygon = true;
            }
			// Zakończenie przesuwania wielokąta
			else if (!drawing && movingPolygon && e.Button == MouseButtons.Left)
			{
				movingPolygon = false;
				polygons.Add(operatingPolygon);
				Redraw();
			}
			// Tryb usuwania wielokąta (LMB na dowolną część wielokąta)
			else if (deletingPolygon && e.Button == MouseButtons.Left && (WasEdgeClicked(point) || WasVertexClicked(point)))
			{
				deletingPolygon = false;
				polygons.Remove(operatingPolygon);
				Redraw();
			}
			// Włączenie trybu przesuwania wierzchołka
			else if (!drawing && !userWantsToMovePolygon && !movingPolygon && !movingVertex && e.Button == MouseButtons.Left && WasVertexClicked(point))
			{
				// Wierzchołek zerowy jest "usztywniony"
				if (operatingPoint == operatingPolygon.points[0])
					return;

				movingVertex = true;
				polygons.Remove(operatingPolygon);
				operatingPolygon.points.Remove(operatingPoint);
				FindAdjacentPoints(operatingPoint, operatingPolygon);
				operatingLine = operatingPolygon.lines.Where((line) => { return line.start == operatingPoint || line.end == operatingPoint; }).First();
                operatingPolygon.lines = new LinkedList<Line>(operatingPolygon.lines.Where((line) => { return line.start != operatingPoint && line.end != operatingPoint; }).ToList());
			}
			// Wyłączenie trybu przesuwania wierzchołka
			else if (movingVertex && e.Button == MouseButtons.Left)
			{
				movingVertex = false;
				operatingPolygon.points.Add(point);
				// EXCEPTION!!!
				operatingPolygon.lines.AddAfter(new LinkedListNode<Line>(operatingLine), new Line(point, adjacentPoints[0]));
				operatingPolygon.lines.AddAfter(new LinkedListNode<Line>(operatingLine), new Line(point, adjacentPoints[1]));
				polygons.Add(operatingPolygon);
			}
			// Tryb dodawania wierzchołka w środku krawędzi
			else if (!drawing && !movingVertex && e.Button == MouseButtons.Left && WasEdgeClicked(point))
			{
				var midPoint = new Point(Math.Min(operatingLine.start.X, operatingLine.end.X) + Math.Abs(operatingLine.start.X - operatingLine.end.X) / 2, 
					Math.Min(operatingLine.start.Y, operatingLine.end.Y) +  Math.Abs(operatingLine.start.Y - operatingLine.end.Y) / 2);
				operatingPolygon.lines.AddBefore(new LinkedListNode<Line>(operatingLine), new Line(midPoint, operatingLine.start));
				operatingPolygon.lines.AddBefore(new LinkedListNode<Line>(operatingLine), new Line(midPoint, operatingLine.end));
				operatingPolygon.lines.Remove(operatingLine);
				operatingPolygon.points.Add(midPoint);
				Redraw();
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
				var xDiff = e.X - operatingPoint.X;
				var yDiff = e.Y - operatingPoint.Y;
				operatingPoint.X += xDiff;
				operatingPoint.Y += yDiff;
				var pts = new List<Point>();
				foreach (var p in operatingPolygon.points)
					pts.Add(new Point(p.X + xDiff, p.Y + yDiff));
				operatingPolygon.points = pts;
				foreach (var line in operatingPolygon.lines)
				{
					line.end.X += xDiff;
					line.end.Y += yDiff;
					line.start.X += xDiff;
					line.start.Y += yDiff;
				}
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
			if (movingPolygon || movingVertex || relationChanged)
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
						operatingPoint = point;
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
						operatingLine = line;
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
		
		private void FindAdjacentEdges(Line line, Polygon poly)
		{
			int i = 0;
			foreach(Line li in poly.lines)
			{
				if (li != line && (li.end == line.start || li.start == line.start))
				{
					i++;
					adjacentEdges[0] = li;
				}
				else if (li != line && (li.start == line.end || li.end == line.end))
                {
					i++;
					adjacentEdges[1] = li;
				}

				if (i == 2)
					return;
			}
		}

		private void HorizontalProcessing()
		{
			FindAdjacentEdges(operatingLine, operatingPolygon);
			if (adjacentEdges[0].relation == RelationType.Horizontal || adjacentEdges[1].relation == RelationType.Horizontal)
			{
				MessageBox.Show("Nie można dodać relacji - sąsiednia krawędź również jest pozioma.");
				return;
			}
			operatingLine.relation = RelationType.Horizontal;
			operatingPolygon.points.Remove(operatingLine.start);
			var horPoint = new Point(operatingLine.start.X, operatingLine.end.Y);
			operatingPolygon.points.Add(horPoint);
			adjacentEdges[0].end = horPoint;
			operatingLine.start = horPoint;
		}

		/// <summary>
		/// W zależności od liczby wielokątów zmienia wygląd menu kontekstowego.
		/// W zależności od ustawienia relacji zmienia wygląd menu kontekstowego.
		/// </summary>
		private void SetContextMenuItems()
		{
			if (polygons.Count == 0)
			{
				contextMenuStrip1.Items[1].Enabled = contextMenuStrip1.Items[2].Enabled = false;
			}
			else
			{
				contextMenuStrip1.Items[1].Enabled = contextMenuStrip1.Items[2].Enabled = true;
			}

			if (operatingLine != null)
			{
				for (int i = 0; i < relationsContextMenu.Items.Count; i++)
					relationsContextMenu.Items[i].Enabled = (operatingLine.relation == RelationType.None);
            }
		}
		#endregion

		#region ContextMenu Handlers
		private void relationsContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			relationChanged = true;
			// Pozioma
			if (e.ClickedItem == relationsContextMenu.Items[0])
			{
				HorizontalProcessing();
				Redraw();
			}
			// Pionowa
			if (e.ClickedItem == relationsContextMenu.Items[1])
			{

			}
			// Długość
			if (e.ClickedItem == relationsContextMenu.Items[2])
			{

			}
		}

		private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			// Dodaj
			if (e.ClickedItem == contextMenuStrip1.Items[0])
			{
				this.drawing = true;
			}
			// Usuń
			if (e.ClickedItem == contextMenuStrip1.Items[1])
			{
				this.deletingPolygon = true;
			}
			// Przesuń
			if (e.ClickedItem == contextMenuStrip1.Items[2])
			{
				this.userWantsToMovePolygon = true;
			}
		}
		#endregion
	}
}
