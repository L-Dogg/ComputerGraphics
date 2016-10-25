using GK1.Structures;

namespace GK1.Relations
{
	public interface IRelation
	{
		RelationType Type { get; }
		bool Apply(Segment segment, Polygon polygon, int length = 0, bool forward = true);
		bool Check(Segment segment, Polygon polygon, int length = 0);
	}
}
