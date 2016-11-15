using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using GK2.Structures;
using GK2.Utilities;

namespace GK2.States
{
	public class PolygonIntersectionState : IState
	{
		private static readonly Color MarkedColor = Color.Red;
		private List<Polygon> Polygons { get; set; } = new List<Polygon>(2);
		private MainForm MainForm { get; set; }

		public PolygonIntersectionState(MainForm mainForm)
		{
			MainForm = mainForm;
		}

		#region Weiler - Atherton Algorithm
		// TODO:
		private void Intersection()
		{
			if (Polygons[0] == Polygons[1])
				return;

			Polygons[0].NormalizePolygon();
			Polygons[1].NormalizePolygon();

			var entryVertices = new List<Vertex>();
			var intersections = CalculateIntersections(entryVertices);

			if (!intersections.Any())
			{
				// Przypadek gdy jeden jest wewnatrz drugiego:
				if (Polygons[0].PointInsidePolygon(Polygons[1].Vertices.First()))
				{
					MainForm.Polygons.Remove(Polygons[0]);
				}
				else if (Polygons[1].PointInsidePolygon(Polygons[0].Vertices.First()))
				{
					MainForm.Polygons.Remove(Polygons[1]);
				}
				return;
			}
			
			while (entryVertices.Any())
			{
				var i = 1; // ktory polygon

				var first = Polygons[i].Vertices.Find(entryVertices.First());
				var current = first.Next ?? Polygons[i].Vertices.First;

				entryVertices.Remove(first.Value);
				var p = new Polygon() { Finished = true };

				p.Vertices.AddLast(first.Value);

				while (current.Value != first.Value)
				{
					p.Segments.AddLast(new Segment(p.Vertices.Last.Value, current.Value));
					p.Vertices.AddLast(current.Value);

					if (intersections.Contains(current.Value))
					{
						if (entryVertices.Contains(current.Value))
							entryVertices.Remove(current.Value);

						i = (i + 1)%2;
						current = Polygons[i].Vertices.Find(current.Value);
					}

					current = current.Next ?? Polygons[i].Vertices.First;
				}

				// zakonczenie polygona:
				p.Segments.AddLast(new Segment(p.Vertices.Last.Value, p.Vertices.First.Value));
				MainForm.Polygons.Add(p);
			}

			MainForm.Polygons.Remove(Polygons[0]);
			MainForm.Polygons.Remove(Polygons[1]);
		}

		private List<Vertex> CalculateIntersections(List<Vertex> entryVertices)
		{
			var intersecting = new List<Vertex>();
			Vertex v;
			var window = Polygons[0];
			var subject = Polygons[1];
			
			window.Vertices.AddLast(window.Vertices.First.Value);
			subject.Vertices.AddLast(subject.Vertices.First.Value);
			var winVert = window.Vertices.First;
			while (winVert.Next != null)
			{
				var subVert = subject.Vertices.First;
				while (subVert.Next != null)
				{
					if (SegmentHelper.LineSegementsIntersect(winVert.Value, winVert.Next.Value, subVert.Value, subVert.Next.Value,
						out v))
					{
						window.Vertices.AddAfter(winVert, v);
						subject.Vertices.AddAfter(subVert, v);
						intersecting.Add(v);

						if ((winVert.Value - winVert.Next.Value) * (subVert.Next.Value - winVert.Value) < 0)
							entryVertices.Add(v);

					}
					subVert = subVert.Next;
				}
				winVert = winVert.Next;
			}

			window.Vertices.RemoveLast();
			subject.Vertices.RemoveLast();

			foreach (var polygon in Polygons)
			{
				polygon.Segments.Clear();
				var vert = polygon.Vertices.First;

				while (vert != polygon.Vertices.Last)
				{
					polygon.Segments.AddLast(new Segment(vert.Value, vert.Next.Value));
					vert = vert.Next;
				}
				polygon.Segments.AddLast(new Segment(polygon.Vertices.Last.Value, polygon.Vertices.First.Value));
				polygon.NormalizePolygon();
			}

			return intersecting;
		}

		#endregion

		#region IState
		public void MouseDown(object sender, MouseEventArgs e)
		{
			Polygon polygon;
			var point = new Vertex(e.X, e.Y);
			
			if (!ClickChecker.WasPolygonClicked(point, MainForm.Polygons, out polygon))
				return;

			Polygons.Add(polygon);
			MainForm.Render();
			if (Polygons.Count != 2)
				return;

			Intersection();
			this.Polygons.Clear();
			MainForm.Render();
			MainForm.CurrentPolygon = new Polygon();
			MainForm.CurrentState = new IdleState(MainForm);
		}

		public void MouseMove(object sender, MouseEventArgs e)
		{
			
		}

		public void KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode != Keys.Escape)
				return;

			MainForm.CurrentPolygon = new Polygon();
			MainForm.CurrentState = new IdleState(MainForm);
			MainForm.Render();
		}

		public void Render(DirectBitmap bitmap, Graphics g)
		{
			foreach (var polygon in MainForm.Polygons)
				if (!Polygons.Contains(polygon))
					polygon.Render(bitmap, MainForm.ColorFill, MainForm.BumpMapping);

			foreach (var polygon in this.Polygons)
				polygon.Render(bitmap, MarkedColor, MainForm.ColorFill, MainForm.BumpMapping);
		}
		#endregion
	}
}
