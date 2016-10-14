﻿using System;
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

        public void Draw(Graphics g)
        {
			foreach (var line in lines)
				g.DrawLine(Pens.Black, line.start, line.end);

			foreach (var point in points)
				g.FillEllipse(Brushes.Red, point.X, point.Y, 5, 5);
		}
	}
}
