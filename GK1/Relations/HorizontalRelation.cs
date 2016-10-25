using System.Linq;
using GK1.Structures;

namespace GK1.Relations
{
	public class HorizontalRelation : IRelation
	{
		public RelationType Type => RelationType.Horizontal;
		
		public bool Apply(Segment segment, Polygon polygon, int length = 0, bool forward = true)
		{
			if (polygon == null)
				return false;

			var edges = polygon.Segments.Where((line) => line.From == segment.To || line.To == segment.From);
			if (edges.Any((line) => line.Relation.Type == RelationType.Horizontal))
				return false;

			if (forward)
			{
				segment.To.Y = segment.From.Y;
				segment.Relation = this;
			}
			else
			{
				segment.From.Y = segment.To.Y;
				segment.Relation = this;
			}

			return true;
		}

		public bool Check(Segment segment, Polygon polygon, int length = 0)
		{
			return segment.From.Y == segment.To.Y;
		}
	}
}
