using System;
using System.Collections.Generic;
using System.Drawing;
using GK2.Utilities;

namespace GK2.Algorithms
{
	public static class Algorithms
	{
		private static void Swap(ref int lhs, ref int rhs)
		{
			var temp = lhs;
			lhs = rhs;
			rhs = temp;
		}
		
		public static void Line(int x0, int y0, int x1, int y1, DirectBitmap bmp)
		{
			Line(x0, y0, x1, y1, bmp, Color.Black);
		}

		public static void Line(int x0, int y0, int x1, int y1, DirectBitmap bmp, Color color)
		{
			var steep = Math.Abs(y0 - y1) >= Math.Abs(x0 - x1);
			if (steep)
			{
				Swap(ref x0, ref y0);
				Swap(ref x1, ref y1);
			}
			if (x0 > x1)
			{
				Swap(ref x0, ref x1);
				Swap(ref y0, ref y1);
			}

			var increment = y0 > y1 ? -1 : 1;
            var points = Bresenham(x0, y0, x1, y1, increment);

			foreach (var point in points)
				if ((steep) && (point.Y >= 0 && point.X >= 0 && point.Y < bmp.Width && point.X < bmp.Height))
						bmp.Bits[point.X * bmp.Width + point.Y] = color.ToArgb();
				else if (point.X >= 0 && point.Y >= 0 && point.X < bmp.Width && point.Y < bmp.Height)
					bmp.Bits[point.Y * bmp.Width + point.X] = color.ToArgb();
			
		}

		private static IEnumerable<Point> Bresenham(int x0, int y0, int x1, int y1, int plus)
		{
			var x = x0;
			var y = y0;
			var dx = x1 - x0;
			var dy = Math.Abs(y1 - y0);
			var d = 2 * dy - dx;

			var points = new List<Point>() {new Point(x, y)};
			while (x <= x1)
			{
				if (d <= 0)
				{
					d += 2 * dy;
					x++;
				}
				else
				{
					d += 2 * (dy - dx);
					x++;
					y += plus;
				}
				points.Add(new Point(x,y));
			}
			return points;
		}
		
	}
}

