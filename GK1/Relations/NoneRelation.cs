using GK1.Structures;

namespace GK1.Relations
{
	public class NoneRelation : IRelation
	{
		public RelationType Type => RelationType.None;
		
		public bool Apply(Segment segment, Polygon polygon, int length = 0, bool forward = true)
		{
			segment.Relation = this;
			segment.DesiredLength = 0;
			return true;
		}

		public bool Check(Segment segment, Polygon polygon, int length = 0)
		{
			return true;
		}
	}
}
