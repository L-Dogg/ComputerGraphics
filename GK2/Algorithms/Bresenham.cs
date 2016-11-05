using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
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

		private static void Line(int x0, int y0, int x1, int y1, DirectBitmap bmp, Color color)
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
			var points = BresenhamLine(x0, y0, x1, y1, increment);
			foreach (var point in points)
			{
				if (steep) DrawInBounds(point.Y, point.X, bmp, color);
				else DrawInBounds(point.X, point.Y, bmp, color);
			}
		}

		private static IEnumerable<Point> BresenhamLine(int x0, int y0, int x1, int y1, int increment)
		{
			var dx = x1 - x0;
			var dy = Math.Abs(y1 - y0);
			var d = 2 * dy - dx;
			var x = x0;
			var y = y0;
			yield return new Point(x, y);
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
					y += increment;
				}
				yield return new Point(x, y);
			}
		}

		private static void DrawInBounds(int x, int y, DirectBitmap bitmap, Color color)
		{
			if (x >= 0 && y >= 0 &&
				x < bitmap.Width && y < bitmap.Height)
			{
				bitmap.Bits[y*bitmap.Width + x] = color.ToArgb();
			}
		}
	}
}

