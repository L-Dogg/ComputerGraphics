using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK3.Utilities
{
	public 	class Vector3
	{
		private static readonly int SIZE = 3;

		public double[] tab;

		public double X { get { return tab[0]; } set { tab[0] = value; } }
		public double Y { get { return tab[1]; } set { tab[1] = value; } }
		public double Z { get { return tab[2]; } set { tab[2] = value; } }

		public Vector3()
		{
			tab = new double[SIZE];
		}

		public Vector3(double x, double y, double z) : this()
		{
			tab[0] = x;
			tab[1] = y;
			tab[2] = z;
		}

		public double DotProduct(Vector3 v)
		{
			var sum = 0.0;
			for (var i = 0; i < SIZE; i++)
				sum += this.tab[i]*v.tab[i];

			return sum;
		}

		public Vector3 MultiplyScalar(double scalar)
		{
			return new Vector3(scalar * this.X, scalar * this.Y, scalar * this.Z);
		}
	}
}
