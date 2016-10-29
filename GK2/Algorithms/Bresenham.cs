using System;
using System.Drawing;

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
		
		public static void Line(int x0, int y0, int x1, int y1, Bitmap bmp)
		{
			Line(x0, y0, x1, y1, bmp, Color.Black);
		}
		
		private static void Line(int x0, int y0, int x1, int y1, Bitmap bmp, Color color)
		{
			var steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
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

			int dX = (x1 - x0), dY = Math.Abs(y1 - y0), err = (dX / 2), ystep = (y0 < y1 ? 1 : -1), y = y0;

			for (var x = x0; x <= x1; ++x)
			{
				if (steep)
				{
					// Zabezpieczenie do testów ale już nie pamiętam po co
					if (y >= bmp.Width || x < 0)
                        break;

					bmp.SetPixel(y > 0 ? y : 0, x < bmp.Height ? x : bmp.Height - 1, color);
				}
				else
				{
					// Zabezpieczenie do testów ale już nie pamiętam po co
					if (x >= bmp.Width || y < 0)
						break;

					bmp.SetPixel(x > 0 ? x : 0, y < bmp.Height ? y : bmp.Height - 1, color);
				}
				err = err - dY;

				if (err >= 0)
					continue;
				y += ystep;
				err += dX;
			}
		}

	}
}

