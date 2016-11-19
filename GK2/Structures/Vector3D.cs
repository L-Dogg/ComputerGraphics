using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK2.Structures
{
	public class Vector3D
	{
		public double X { get; set; }
		public double Y { get; set; }
		public double Z { get; set; }

		public double Length => Math.Sqrt(X*X + Y*Y + Z*Z);

		public Vector3D(double x, double y, double z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public Vector3D(Vector3D v)
		{
			X = v.X;
			Y = v.Y;
			Z = v.Z;
		}

		public Vector3D(Color color, bool normal = false)
		{
			if (normal)
			{
				X = (double)(color.R - 127) / 127;
				Y = (double)(color.G - 127) / 127;
				Z = (double)color.B / 255;
				return;
			}
			
			X = (double)color.R/255;
			Y = (double)color.G/255;
			Z = (double)color.B/255;

		}

		public void Normalize()
		{
			var len = Length;
			X /= len;
			Y /= len;
			Z /= len;
		}
	}
}
