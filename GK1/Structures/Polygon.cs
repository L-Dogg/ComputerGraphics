using System;
using System.Collections.Generic;
using System.Drawing;

namespace GK1.Structures
{
	public class Polygon
	{
		public LinkedList<Vertex> Vertices	{ get; } = new LinkedList<Vertex>();
		public LinkedList<Segment> Segments { get; set; } = new LinkedList<Segment>();
		private static readonly Font Font = new Font("Arial", 7);

		#region Relations
		private bool Check(LinkedListNode<Segment> node)
		{
			SaveVertices();

            var forward = Segments.First;
			var backward = Segments.Last;

			while (forward != null && forward != node)
			{
				forward.Value.Relation.Apply(forward.Value, this, forward.Value.DesiredLength > 0 ? forward.Value.DesiredLength : 0, true);
				forward = forward.Next;
			}
			while (backward != null && backward != node.Next)
			{
				backward.Value.Relation.Apply(backward.Value, this, backward.Value.DesiredLength > 0 ? backward.Value.DesiredLength : 0, false);
				backward = backward.Previous;
			}

			foreach(var seg in Segments)
			{
				if (!seg.Relation.Check(seg, this))
				{
					LoadVertices();
					return false;
                }
			}
			
			ClearVertices();

			return true;
		}

		public bool Apply()
		{
			var iter = Segments.First;
			while(iter != null)
			{
				if (Check(iter))
					return true;

				iter = iter.Next;
			}

			return false;

		}
		#endregion

		#region Vertices Stacks Operations
		public void SaveVertices()
		{
			foreach (var v in Vertices)
				v.Previous.Push(new Point(v.X, v.Y));
		}

		public void LoadVertices()
		{
			foreach (var v in Vertices)
				v.LoadPrevious();
		}

		private void ClearVertices()
		{
			foreach (var v in Vertices)
				v.Previous.Clear();
		}
		#endregion

		public void Render(Bitmap bmp, Graphics g)
		{
			foreach (var line in Segments)
			{
				var midPoint = new Point(Math.Min(line.From.X, line.To.X) + Math.Abs(line.From.X - line.To.X) / 2,
					Math.Min(line.From.Y, line.To.Y) + Math.Abs(line.From.Y - line.To.Y) / 2);
				
				if (line.Relation.Type == RelationType.Horizontal)
				{
					g.FillEllipse(Brushes.Red, midPoint.X - 7, midPoint.Y - 7, 15, 15);					
				}
				else if (line.Relation.Type == RelationType.Vertical)
				{
					g.FillEllipse(Brushes.Green, midPoint.X - 7, midPoint.Y - 7, 15, 15);
				}

				Algorithms.Algorithms.Line(line.From.X, line.From.Y, line.To.X, line.To.Y, bmp);
				
				if (line.Relation.Type == RelationType.Length)
				{
					g.FillRectangle(Brushes.Wheat, midPoint.X - 7, midPoint.Y - 7, 18, 12);
					g.DrawString(line.DesiredLength.ToString(), Font, Brushes.Black, new Point(midPoint.X - 9, midPoint.Y - 7));
				}
			}

			foreach (var point in Vertices)
				point.Draw(bmp);

			Vertices.First.Value.Draw(bmp, Color.Blue);
		}
	}
}
