﻿using GK1.Relations;
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
		public IRelation Relation { get; set; }
        public Vertex From { get; set; }
		public Vertex To { get; set; }

		public int Length
		{
			get
			{
				return (int) Math.Sqrt((From.X - To.X) * (From.X - To.X) + (From.Y - To.Y) * (From.Y - To.Y));
			}
		}

		public Segment(Vertex from, Vertex to)
		{
			Relation = new NoneRelation();
			From = from;
			To = to;
		}
	}
}
