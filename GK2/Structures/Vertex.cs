using System;
using System.Drawing;
using GK2.Utilities;

namespace GK2.Structures
{
	public class Vertex
	{
		private static readonly int Margin = 8;

		private Point _p;
		public int X
		{
			get { return _p.X; }
			set { _p = new Point(value, _p.Y); }
		}
		public int Y
		{
            get { return _p.Y; }
			set { _p = new Point(_p.X, value); }
		}

		public Vertex(int x, int y)
		{
			_p = new Point(x, y);
		}

		public Vertex()
		{
			_p = new Point();
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
			return _p.GetHashCode();
		}

		#region CopyPaste
		public static Vertex operator -(Vertex v, Vertex w)
		{
			return new Vertex(v.X - w.X, v.Y - w.Y);
		}

		public static Vertex operator +(Vertex v, Vertex w)
		{
			return new Vertex(v.X + w.X, v.Y + w.Y);
		}

		public static double operator *(Vertex v, Vertex w)
		{
			return v.X * w.X + v.Y * w.Y;
		}

		public static Vertex operator *(Vertex v, double mult)
		{
			return new Vertex((int) (v.X * mult), (int)(v.Y * mult));
		}

		public static Vertex operator *(double mult, Vertex v)
		{
			return new Vertex((int)(v.X * mult), (int)(v.Y * mult));
		}

		public double Cross(Vertex v)
		{
			return X * v.Y - Y * v.X;
		}
		#endregion

		#region Backward Compatibility
		public bool ComparePoints(Vertex u)
		{
			return Math.Abs(_p.X - u.X) <= Margin && Math.Abs(_p.Y - u.Y) <= Margin;
		}

		public bool IsCloseToLine(Segment line)
		{
			return _p.IsCloseToLine(line);
		}

		public void Draw(DirectBitmap bmp)
		{
			_p.Draw(bmp, Color.Red);
		}

		public void Draw(DirectBitmap bmp, Color col)
		{
			_p.Draw(bmp, col);
		}

		public bool OnRectangle(Segment line)
		{
			return _p.OnRectangle(line);
		}
		#endregion
	}
}
