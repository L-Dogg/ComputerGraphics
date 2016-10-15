using Bresenhams;
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

        public void Draw(Bitmap bmp)
        {
			foreach (var line in lines)
				Algorithms.Line(line.start.X, line.start.Y, line.end.X, line.end.Y, bmp);
			
			foreach (var point in points)
				point.Draw(bmp);
			
		}
	}
}
