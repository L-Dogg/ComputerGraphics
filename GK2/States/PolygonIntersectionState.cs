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

			var intersections = CalculateIntersections();
			if (!intersections.Any())
				return;
			
			var entryVertices = FindEntryVertices(intersections);
			
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

		private void AddIntersections(List<Vertex> intersections)
		{
			foreach (var polygon in Polygons)
			{
				var tmpVert = new LinkedList<Vertex>();
				var tmpSeg = new LinkedList<Segment>();
				foreach (var vertex in intersections)
				{
					foreach (var segment in polygon.Segments)
					{
						tmpVert.AddLast(segment.From);
						if (!vertex.IsCloseToLine(segment))
						{
							tmpSeg.AddLast(segment);
						}
						else
						{
							tmpSeg.AddLast(new Segment(segment.From, vertex));
							tmpSeg.AddLast(new Segment(vertex, segment.To));

							tmpVert.AddLast(vertex);
						}
					}
				}
				polygon.Segments = tmpSeg;
				polygon.Vertices = tmpVert;
				polygon.NormalizePolygon();
			}
		} 

		private List<Vertex> CalculateIntersections()
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

		// TODO: ZLE
		private List<Vertex> FindEntryVertices(List<Vertex> intersections)
		{
			var entryVertices = new List<Vertex>();
			var subject = Polygons[1];
			var clip = Polygons[0];

			foreach (var segment in subject.Segments)
			{
				if (intersections.Contains(segment.From) && intersections.Contains(segment.To))
					entryVertices.Add(segment.From);
			}

			return entryVertices;
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
