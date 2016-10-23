using GK1.Structures;

namespace GK1.Relations
{
	interface IRelation
	{
		RelationType RelationType { get; }
		bool Apply(Segment segment, Polygon polygon, int length = 0);
	}
}
