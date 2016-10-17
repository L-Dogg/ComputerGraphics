using Projekt_1.Structures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_1
{
	public class Line
	{
		public RelationType relation;
		public Point start, end;
		public Line(Point _s, Point _e, RelationType _r = RelationType.None)
		{
			relation = _r;
			start = _s;
			end = _e;
		}
	}
}
