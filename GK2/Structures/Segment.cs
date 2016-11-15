
using System;

namespace GK2.Structures
{
	public class Segment
	{
		public Vertex From { get; set; }
		public Vertex To { get; set; }

        public int Ymax => Math.Max(From.Y, To.Y);
	    public int Ymin => Math.Min(From.Y, To.Y);
		public double Xmin { get; set; }
		
		public double StartXmin => (Ymin == From.Y) ? From.X : To.X;
		public double DxDy => ((double)(To.X - From.X))/(To.Y - From.Y);

		//public int Length => (int) Math.Sqrt((From.X - To.X) * (From.X - To.X) + (From.Y - To.Y) * (From.Y - To.Y));
		//public int DesiredLength { get; set; } = 0;

	    public Segment(Vertex from, Vertex to)
	    {
			From = from;
			To = to;
		    Xmin = StartXmin;
	    }
	}
}
