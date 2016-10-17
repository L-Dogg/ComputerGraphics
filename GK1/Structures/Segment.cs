using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK1.Structures
{
	public class Segment
	{
		public RelationType Relation { get; set; }
        public Point From { get; set; }
		public Point To { get; set; }
		public Segment(Point from, Point to, RelationType relation = RelationType.None)
		{
			Relation = relation;
			From = from;
			To = to;
		}
	}
}
