using GK2.Structures;

namespace GK2.Utilities
{
	public class SegmentHelper
	{
		public static bool LineSegementsIntersect(Vertex p, Vertex p2, Vertex q, Vertex q2,
			out Vertex intersection, bool considerCollinearOverlapAsIntersect = true)
		{
			intersection = new Vertex();
			if (p == q || p == q2 || p2 == q || p2 == q2) return false;

			var r = p2 - p;
			var s = q2 - q;
			var rxs = r.Cross(s);
			var qpxr = (q - p).Cross(r);
			
			if (rxs.IsZero() && qpxr.IsZero())
			{
				if (considerCollinearOverlapAsIntersect)
				{
					p.X += 2;
					p2.X -= 2;
					p.Y += 2;
					p2.Y -= 2;

					return LineSegementsIntersect(p, p2, q, q2, out intersection, false);
				}
				
				return false;
			}
			
			if (rxs.IsZero() && !qpxr.IsZero())
				return false;
			
			var t = (q - p).Cross(s) / rxs;
			
			var u = (q - p).Cross(r) / rxs;
			
			if (!rxs.IsZero() && (0 <= t && t <= 1) && (0 <= u && u <= 1))
			{
				intersection = p + t * r;
				return true;
			}
			
			return false;
		}
	}
}
