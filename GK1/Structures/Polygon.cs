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
		private static Font font = new Font("Arial", 7);

		public void Render(Bitmap bmp, Graphics g)
		{
			foreach (var line in Segments)
			{
				var midPoint = new Point(Math.Min(line.From.X, line.To.X) + Math.Abs(line.From.X - line.To.X) / 2,
					Math.Min(line.From.Y, line.To.Y) + Math.Abs(line.From.Y - line.To.Y) / 2);
				
				if (line.Relation == RelationType.Horizontal)
				{
					g.FillEllipse(Brushes.Red, midPoint.X - 7, midPoint.Y - 7, 15, 15);					
				}
				else if (line.Relation == RelationType.Vertical)
				{
					g.FillEllipse(Brushes.Green, midPoint.X - 7, midPoint.Y - 7, 15, 15);
				}

				Algorithms.Line(line.From.X, line.From.Y, line.To.X, line.To.Y, bmp);
				
				if (line.Relation == RelationType.Length)
				{
					g.FillRectangle(Brushes.Wheat, midPoint.X - 7, midPoint.Y - 7, 18, 12);
					g.DrawString(line.Length.ToString(), font, Brushes.Black, new Point(midPoint.X - 9, midPoint.Y - 7));
				}
			}

			foreach (var point in Points)
				point.Draw(bmp);

			Points.First.Value.Draw(bmp, Color.Blue);
		}
	}
}
