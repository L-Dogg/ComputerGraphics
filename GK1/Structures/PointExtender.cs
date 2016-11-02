using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK1.Structures
{
	public static class PointExtender
	{
		private static int _margin = 5;
		private static Color _color = Color.Red;

		public static bool ComparePoints(this Point p, Point u)
		{
			return Math.Abs(p.X - u.X) <= _margin && Math.Abs(p.Y - u.Y) <= _margin;
		}

		public static bool ComparePoints(this Point p, Vertex u)
		{
			return Math.Abs(p.X - u.X) <= _margin && Math.Abs(p.Y - u.Y) <= _margin;
		}
		
		public static bool IsCloseToLine(this Point p, Segment line)
		{
			double distance = Math.Abs((line.To.Y - line.From.Y) * p.X - (line.To.X - line.From.X) * p.Y + line.To.X * line.From.Y - line.To.Y * line.From.X) /
				Math.Sqrt((line.To.Y - line.From.Y) * (line.To.Y - line.From.Y) + (line.To.X - line.From.X) * (line.To.X - line.From.X));
			if (distance < 2 * _margin && !p.ComparePoints(line.From) && !p.ComparePoints(line.To))
				return p.OnRectangle(line);
			return false;
		}

		public static void Draw(this Point p, Bitmap bmp)
		{
			p.Draw(bmp, _color);
		}

		public static void Draw(this Point p, Bitmap bmp, Color col)
		{
			if (p.X - 2 < 0 || p.X + 2 >= bmp.Width || p.Y - 2 < 0 || p.Y + 2 >= bmp.Height)
				return;

			for (int i = (p.X - 2); i <= (p.X + 2); i++)
				for (int j = (p.Y - 2); j <= (p.Y + 2); j++)
					bmp.SetPixel(i, j, col);
		}

		public static bool OnRectangle(this Point p, Segment line)
		{
			var minY = (line.From.Y < line.To.Y) ? line.From.Y : line.To.Y;
			var maxY = (line.From.Y < line.To.Y) ? line.To.Y : line.From.Y;
			var minX = (line.From.X < line.To.X) ? line.From.X : line.To.X;
			var maxX = (line.From.X < line.To.X) ? line.To.X : line.From.X;

			if (p.Y <= (maxY + _margin) && (p.Y >= minY - _margin) && (p.X <= maxX + _margin) && (p.X >= minX - _margin))
				return true;
			return false;
		}
	}
}
