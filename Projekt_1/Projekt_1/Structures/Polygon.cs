using Bresenhams;
using Projekt_1.Structures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_1
{
	class Polygon
	{
		public List<Point> points = new List<Point>();
		public List<Line> lines = new List<Line>();

		private static Color horizontalColor = Color.Yellow;
		private static Color regularColor = Color.Black;

		public void Draw(Bitmap bmp)
        {
			foreach (var line in lines)
			{
				if (line.relation == RelationType.Horizontal)
					Algorithms.Line(line.start.X, line.start.Y, line.end.X, line.end.Y, bmp, horizontalColor);
				else
					Algorithms.Line(line.start.X, line.start.Y, line.end.X, line.end.Y, bmp, regularColor);
			}
			
			foreach (var point in points)
				point.Draw(bmp);
			
		}
	}
}
