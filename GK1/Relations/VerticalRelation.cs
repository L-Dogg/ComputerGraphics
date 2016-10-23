using System.Linq;
using GK1.Structures;
using System.Drawing;

namespace GK1.Relations
{
	class VerticalRelation : IRelation
	{
		public RelationType RelationType
		{
			get { return RelationType.Vertical; }
		}

		public bool Apply(Segment segment, Polygon polygon, int length = 0)
		{
			if (polygon == null)
				return false;

			var edges = polygon.Segments.Where((line) => { return line.From == segment.To || line.To == segment.From; });
			if (edges.Any((line) => { return line.Relation == RelationType.Vertical; }))
				return false;

			var point = new Point(segment.To.X, segment.From.Y);
			polygon.Points.Find(segment.From).Value = point;
			polygon.Segments.First((line) => { return line.To == segment.From; }).To = point;
			segment.From = point;
			segment.Relation = RelationType.Vertical;

			return true;
		}
	}
}
