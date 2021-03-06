﻿using System.Linq;
using GK1.Structures;

namespace GK1.Relations
{
	public class VerticalRelation : IRelation
	{
		public RelationType Type => RelationType.Vertical;
		
		public bool Apply(Segment segment, Polygon polygon, int length = 0, bool forward = true)
		{

			if (polygon == null)
				return false;

			var edges = polygon.Segments.Where((line) => line.From == segment.To || line.To == segment.From);
			if (edges.Any((line) => line.Relation.Type == RelationType.Vertical))
				return false;

			if (forward)
			{
				segment.To.X = segment.From.X;
				segment.Relation = this;
			}
			else
			{
				segment.From.X = segment.To.X;
				segment.Relation = this;
			}

			return true;
		}

		public bool Check(Segment segment, Polygon polygon, int length = 0)
		{
			return segment.From.X == segment.To.X;
		}
	}
}
