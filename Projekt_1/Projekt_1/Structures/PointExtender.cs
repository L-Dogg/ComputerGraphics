using System;
using System.Drawing;

namespace Projekt_1
{
	public static class PointExtender
	{ 
		private static int margin = 5;
		private static Color color = Color.Red;

		public static bool ComparePoints(this Point p, Point u)
		{
			return Math.Abs(p.X - u.X) <= margin && Math.Abs(p.X - u.X) <= margin;
		}
		
		/// <summary>
		/// TODO: joy division :(((
		/// </summary>
		/// <param name="p"></param>
		/// <param name="line"></param>
		/// <returns></returns>
		public static bool IsCloseToLine(this Point p, Line line)
		{
			double distance =  Math.Abs((line.end.Y - line.start.Y) * p.X - (line.end.X - line.start.X) * p.Y + line.end.X * line.start.Y - line.end.Y * line.start.X) /
				Math.Sqrt((line.end.Y - line.start.Y) * (line.end.Y - line.start.Y) + (line.end.X - line.start.X) * (line.end.X - line.start.X));
			if (distance < 2 * margin)
				return p.OnRectangle(line);
			return false;
		}

		public static void Draw(this Point p, Bitmap bmp)
		{
			for (int i = p.X - 2; i <= p.X + 2; i++)
				for (int j = p.Y - 2; j <= p.Y + 2; j++)
					bmp.SetPixel(i, j, color);
		}

		public static bool OnRectangle(this Point p, Line line)
		{
			var minY = (line.start.Y < line.end.Y) ? line.start.Y : line.end.Y;
			var maxY = (line.start.Y < line.end.Y) ? line.end.Y : line.start.Y;
			var minX = (line.start.X < line.end.X) ? line.start.X : line.end.X;
			var maxX = (line.start.X < line.end.X) ? line.end.X : line.start.X;

			if (p.Y <= maxY && p.Y >= minY && p.X <= maxX && p.X >= minX)
				return true;
			return false;
		}
	}
}
