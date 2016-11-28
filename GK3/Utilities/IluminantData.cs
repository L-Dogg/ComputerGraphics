using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK3.Utilities
{
	public class IluminantData
	{
		public double X { get; } 
		public double Y { get; }

		public IluminantData(double x, double y)
		{
			this.X = x;
			this.Y = y;
		}
	}
}
