using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GK1.Structures;
using System.Drawing;

namespace GK1.Relations
{
	class HorizontalRelation : IRelation
	{
		public RelationType Type
		{
			get	{ return RelationType.Horizontal; }
		}

		public bool Apply(Segment segment, Polygon polygon, int length = 0)
		{
			if (polygon == null)
				return false;

			var edges = polygon.Segments.Where((line) => { return line.From == segment.To || line.To == segment.From; });
			if (edges.Any((line) => { return line.Relation.Type == RelationType.Horizontal; }))
				return false;

			var point = new Vertex(segment.From.X, segment.To.Y);
			polygon.Vertices.Find(segment.From).Value = point;
			polygon.Segments.First((line) => { return line.To == segment.From; }).To = point;
			segment.From = point;
			segment.Relation = this;

			return true;
		}

		public bool Check(Segment segment, Polygon polygon, int length = 0)
		{
			return segment.From.Y == segment.To.Y;
		}
	}
}
