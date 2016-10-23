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
		public RelationType Type
		{
			get { return RelationType.Length; }
		}

		public bool Apply(Segment segment, Polygon polygon, int length)
		{
			if (!Check(segment, polygon, length))
				return false;

			//MainForm.LengthMessageBox = new Length() { LengthTyped = segment.Length };
			//MainForm.LengthMessageBox.ShowDialog();
			//var newLength = MainForm.LengthMessageBox.LengthTyped;

			var point = segment.To;
			CalculateNewCoords(segment, length);
			polygon.Points.Find(point).Value = segment.To;
			polygon.Segments.First((line) => { return line.From == point; }).From = segment.To;
			segment.Relation = this;

			return true;
		}

		public bool Check(Segment segment, Polygon polygon, int length = 0)
		{
			return true;	
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
			seg.To = new Point((int)(seg.From.X + dX * scale), (int)(seg.From.Y + dy * scale));
		}
	}
}
