using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK1.Structures
{
	public class Vertex
	{
		private static int margin = 5;

		private Point p;

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
