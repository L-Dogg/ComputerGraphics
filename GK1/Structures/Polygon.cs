using Bresenhams;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK1.Structures
{
	public class Polygon
	{
		public LinkedList<Point> Points	{ get; set; } = new LinkedList<Point>();
		public LinkedList<Segment> Segments { get; set; } = new LinkedList<Segment>();
		
		public void Render(Bitmap bmp, Graphics g)
		{
			foreach (var line in Segments)
				Algorithms.Line(line.From.X, line.From.Y, line.To.X, line.To.Y, bmp);

			foreach (var point in Points)
				point.Draw(bmp);
		}
	}
}
