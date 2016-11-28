using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK3.Utilities
{
	public class LabData
	{
		public Vector3 R { get; }
		public Vector3 G { get; }
		public Vector3 B { get; }
		public Vector3 W { get; }
		public double Gamma { get; }

		public LabData(double Rx, double Ry, double Gx, double Gy, double Bx, double By, double Wx, double Wy, double gamma)
		{
			this.Gamma = gamma;
			this.R = new Vector3(Rx, Ry, 1 - Rx - Ry);
			this.G = new Vector3(Gx, Gy, 1 - Gx - Gy);
			this.B = new Vector3(Bx, By, 1 - Bx - By);
			this.W = new Vector3(Wx, Wy, 1 - Wx - Wy);
		}
	}
}
