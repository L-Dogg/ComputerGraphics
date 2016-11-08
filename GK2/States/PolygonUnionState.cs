using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using GK2.Structures;
using GK2.Utilities;

namespace GK2.States
{
	public class PolygonUnionState : IState
	{
		private static readonly Color MarkedColor = Color.Red;
		private List<Polygon> Polygons { get; set; } = new List<Polygon>(2);
		private MainForm MainForm { get; set; }

		public PolygonUnionState(MainForm mainForm)
		{
			MainForm = mainForm;
		}

		#region Weiler - Atherton Algorithm
		// TODO:
		private void Union()
		{
			Polygons[0].NormalizePolygon();
			Polygons[1].NormalizePolygon();

			var intersections = CalculateIntersections();
			if (!intersections.Any())
				return;
			
			AddIntersections(intersections);

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
					p.Segments.AddFirst(new Segment(p.Vertices.Last.Value, current.Value));
					p.Vertices.AddFirst(current.Value);

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
				p.Segments.AddFirst(new Segment(p.Vertices.Last.Value, p.Vertices.First.Value));
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
			foreach (var clipSeg in Polygons[0].Segments)
			{
				foreach (var subSeg in Polygons[1].Segments)
				{
					if (SegmentHelper.LineSegementsIntersect(clipSeg.From, clipSeg.To, subSeg.From, subSeg.To, out v))
						intersecting.Add(v);
				}
			}

			return intersecting;
		}

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

			Union();
			MainForm.CurrentState = new IdleState(MainForm);
			MainForm.Render();
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
