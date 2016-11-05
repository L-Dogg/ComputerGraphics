using System;

namespace GK2.Utilities
{
	public static class Extensions
	{
		private const double Epsilon = 1e-10;

		public static bool IsZero(this double d)
		{
			return Math.Abs(d) < Epsilon;
		}
	}
}
