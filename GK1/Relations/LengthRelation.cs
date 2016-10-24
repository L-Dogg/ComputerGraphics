using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GK1.Structures;
using System.Drawing;

namespace GK1.Relations
{
	class LengthRelation : IRelation
	{
		private static int margin = 8;

		public RelationType Type
		{
			get { return RelationType.Length; }
		}

		public bool Apply(Segment segment, Polygon polygon, int length)
		{			
			var point = new Vertex(segment.To.X, segment.To.Y);
			CalculateNewCoords(segment, length);
			//polygon.Segments.First((line) => { return line.From == point; }).From = segment.To;
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
		private void CalculateNewCoords(Segment seg, int newLen)
		{
			var dX = seg.To.X - seg.From.X;
			var dy = seg.To.Y - seg.From.Y;

			var scale = Math.Sqrt((double)(newLen * newLen) / (double)(dX * dX + dy * dy));
			seg.To.X = (int)(seg.From.X + dX * scale);
			seg.To.Y = (int)(seg.From.Y + dy * scale);
		}
	}
}
