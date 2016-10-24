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
		public RelationType Type
		{
			get { return RelationType.None; }
		}

		public bool Apply(Segment segment, Polygon polygon, int length = 0)
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
