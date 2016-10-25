using System;
using System.Collections.Generic;
using System.Drawing;

namespace GK1.Structures
{
	public class Vertex
	{
		private static readonly int margin = 8;

		private Point p;
		public Stack<Point> Previous { get; set; }
		public int X
		{
			get { return p.X; }
			set { p = new Point(value, p.Y); }
		}
		public int Y
		{
            get { return p.Y; }
			set { p = new Point(p.X, value); }
		}

		public Vertex(int x, int y)
		{
			p = new Point(x, y);
			Previous = new Stack<Point>();
		}

		public void LoadPrevious()
		{
			if (Previous.Count > 0)
				p = Previous.Pop();		
		}

		public static bool operator ==(Vertex v1, Vertex v2)
		{
			return v1.ComparePoints(v2);
		}

		public static bool operator !=(Vertex v1, Vertex v2)
		{
			return !v1.ComparePoints(v2);
		}
		
		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is Vertex))
			{
				return false;
			}

			var v2 = obj as Vertex;
			return ComparePoints(v2);
		}

		public override int GetHashCode()
		{
			return p.GetHashCode();
		}

		#region Backward Compatibility
		public bool ComparePoints(Vertex u)
		{
			return Math.Abs(p.X - u.X) <= margin && Math.Abs(p.Y - u.Y) <= margin;
		}

		public bool IsCloseToLine(Segment line)
		{
			return p.IsCloseToLine(line);
		}

		public void Draw(Bitmap bmp)
		{
			p.Draw(bmp, Color.Red);
		}

		public void Draw(Bitmap bmp, Color col)
		{
			p.Draw(bmp, col);
		}

		public bool OnRectangle(Segment line)
		{
			return p.OnRectangle(line);
		}
		#endregion
	}
}
