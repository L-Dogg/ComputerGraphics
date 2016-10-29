using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK2.Structures
{
	public class Polygon
	{
		public LinkedList<Vertex> Vertices	{ get; } = new LinkedList<Vertex>();
		public LinkedList<Segment> Segments { get; set; } = new LinkedList<Segment>();
		private static readonly Font font = new Font("Arial", 7);

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
				Algorithms.Algorithms.Line(line.From.X, line.From.Y, line.To.X, line.To.Y, bmp);
			}

			foreach (var point in Vertices)
				point.Draw(bmp);

			Vertices.First.Value.Draw(bmp, Color.Blue);
		}
	}
}
