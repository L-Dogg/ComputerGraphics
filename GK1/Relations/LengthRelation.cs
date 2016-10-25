using System;
using GK1.Structures;

namespace GK1.Relations
{
	class LengthRelation : IRelation
	{
		private static int margin = 2;

		public RelationType Type => RelationType.Length;
		
		public bool Apply(Segment segment, Polygon polygon, int length, bool forward)
		{

			CalculateNewCoords(segment, length, forward);
			
			segment.Relation = this;
			segment.DesiredLength = length;
			return true;
		}

		public bool Check(Segment segment, Polygon polygon, int length = 0)
		{
			return Math.Abs(segment.DesiredLength - segment.Length) <= margin;
		}

		/// <summary>
		/// Calculates new coordinates of seg.To for length relation
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="length">New length</param>
		private void CalculateNewCoords(Segment seg, int newLen, bool forward)
		{
			if (forward)
			{
				var dX = seg.To.X - seg.From.X;
				var dy = seg.To.Y - seg.From.Y;

				var scale = Math.Sqrt((double)(newLen * newLen) / (double)(dX * dX + dy * dy));
				seg.To.X = (int)(seg.From.X + dX * scale);
				seg.To.Y = (int)(seg.From.Y + dy * scale);
			}
			else
			{
				var dX = seg.From.X - seg.To.X;
				var dy = seg.From.Y - seg.To.Y;

				var scale = Math.Sqrt((double)(newLen * newLen) / (double)(dX * dX + dy * dy));
				seg.From.X = (int)(seg.To.X + dX * scale);
				seg.From.Y = (int)(seg.To.Y + dy * scale);
			}
		}
	}
}
