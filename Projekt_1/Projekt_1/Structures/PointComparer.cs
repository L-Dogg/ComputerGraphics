using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_1
{
	public static class PointExtender
	{ 
		private static int margin = 5;
		public static bool ComparePoints(this Point p, Point u)
		{
			return Math.Abs(p.X - u.X) <= margin && Math.Abs(p.X - u.X) <= margin;
		}
	}
}
