﻿using System;
using System.Drawing;

namespace Bresenhams
{
	/// <summary>
	/// The Bresenham algorithm collection
	/// </summary>
	public static class Algorithms
	{
		private static void Swap<T>(ref T lhs, ref T rhs) { T temp; temp = lhs; lhs = rhs; rhs = temp; }

		/// <summary>
		/// The plot function delegate
		/// </summary>
		/// <param name="x">The x co-ord being plotted</param>
		/// <param name="y">The y co-ord being plotted</param>
		/// <returns>True to continue, false to stop the algorithm</returns>
		public delegate bool PlotFunction(int x, int y);

		/// <summary>
		/// Plot the line from (x0, y0) to (x1, y10
		/// </summary>
		/// <param name="x0">The start x</param>
		/// <param name="y0">The start y</param>
		/// <param name="x1">The end x</param>
		/// <param name="y1">The end y</param>
		/// <param name="bmp">Bitmap to draw on</param>
		public static void Line(int x0, int y0, int x1, int y1, Bitmap bmp)
		{
			Line(x0, y0, x1, y1, bmp, Color.Black);
		}

		/// <summary>
		/// Plot the line from (x0, y0) to (x1, y10
		/// </summary>
		/// <param name="x0">The start x</param>
		/// <param name="y0">The start y</param>
		/// <param name="x1">The end x</param>
		/// <param name="y1">The end y</param>
		/// <param name="bmp">Bitmap to draw on</param>
		/// <param name="color">Pixel color</param>
		public static void Line(int x0, int y0, int x1, int y1, Bitmap bmp, Color color)
		{
			bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
			if (steep) { Swap<int>(ref x0, ref y0); Swap<int>(ref x1, ref y1); }
			if (x0 > x1) { Swap<int>(ref x0, ref x1); Swap<int>(ref y0, ref y1); }
			int dX = (x1 - x0), dY = Math.Abs(y1 - y0), err = (dX / 2), ystep = (y0 < y1 ? 1 : -1), y = y0;

			for (int x = x0; x <= x1; ++x)
			{
				if (steep)
					bmp.SetPixel(y, x, color);
				else
					bmp.SetPixel(x, y, color);

				err = err - dY;
				if (err < 0) { y += ystep; err += dX; }
			}
		}

	}
}

