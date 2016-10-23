using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GK1.Structures;

namespace GK1.Relations
{
	class NoneRelation : IRelation
	{
		public RelationType RelationType
		{
			get { return RelationType.None; }
		}

		public bool Apply(Segment segment, Polygon polygon, int length = 0)
		{
			segment.Relation = RelationType.None;
			return true;
		}
	}
}
