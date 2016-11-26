using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK3.Utilities
{
	/// <summary>
	/// Helper class for 3x3 matrices.
	/// </summary>
	public class Matrix3
	{
		private static int SIZE = 3;

		public double[,] tab;

		public Matrix3()
		{
			this.tab = new double[SIZE, SIZE];
		}

		public Matrix3(double[,] array) : this()
		{
			Array.Copy(array, tab, array.Length);
		}

		public Matrix3(Vector3[] cols) : this()
		{
			for (var i = 0; i < SIZE; i++)
				for (var j = 0; j < SIZE; j++)
					tab[j, i] = cols[i].tab[j];
		}

		public Vector3 MultiplyVector(Vector3 v)
		{
			var ret = new Vector3();

			for (var i = 0; i < SIZE; i++)
				ret.tab[i] = this.tab[i, 0]*v.X + this.tab[i, 1]*v.Y + this.tab[i, 2]*v.Z;

			return v;
		}

		public Matrix3 MultiplyScalar(double s)
		{
			var m = new Matrix3(this.tab);

			for (var i = 0; i < SIZE; i++)
				for (var j = 0; j < SIZE; j++)
					m.tab[i, j] *= s;

			return m;
		}

		public Matrix3 MultiplyMatrix3(Matrix3 m)
		{
			var ret = new Matrix3();

			for (var i = 0; i < SIZE; i++)
				for (var j = 0; j < SIZE; j++)
					for (var k = 0; k < SIZE; k++)
						ret.tab[i, j] += this.tab[i, k]*m.tab[k, j];

			return ret;
		}

		public Matrix3 Invert()
		{
			var ret = new Matrix3();

			var minor00 = tab[1, 1] * tab[2, 2] - tab[1, 2] * tab[2, 1];
			var minor01 = tab[1, 2] * tab[2, 0] - tab[1, 0] * tab[2, 2];
			var minor02 = tab[1, 0] * tab[2, 1] - tab[1, 1] * tab[2, 0];

			// calculate the determinant
			var determinant = tab[0, 0] * minor00
								+ tab[0, 1] * minor01
								+ tab[0, 2] * minor02;

			// check if the input is a singular matrix (non-invertable)
			// (note that the epsilon here was arbitrarily chosen)
			if (determinant > -0.000001f && determinant < 0.000001f)
				return null;

			// the inverse of inMat is (1 / determinant) * adjoint(inMat)
			var invDet = 1.0f / determinant;
			ret.tab[0, 0] = invDet * minor00;
			ret.tab[0, 1] = invDet * (tab[2, 1] * tab[0, 2] - tab[2, 2] * tab[0, 1]);
			ret.tab[0, 2] = invDet * (tab[0, 1] * tab[1, 2] - tab[0, 2] * tab[1, 1]);

			ret.tab[1, 0] = invDet * minor01;
			ret.tab[1, 1] = invDet * (tab[2, 2] * tab[0, 0] - tab[2, 0] * tab[0, 2]);
			ret.tab[1, 2] = invDet * (tab[0, 2] * tab[1, 0] - tab[0, 0] * tab[1, 2]);

			ret.tab[2, 0] = invDet * minor02;
			ret.tab[2, 1] = invDet * (tab[2, 0] * tab[0, 1] - tab[2, 1] * tab[0, 0]);
			ret.tab[2, 2] = invDet * (tab[0, 0] * tab[1, 1] - tab[0, 1] * tab[1, 0]);

			return ret;
		}
	}
}
