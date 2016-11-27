using System;
using GK3.Utilities;

namespace GK3.Processors
{
	public class LabProcessor : IProcessor
	{
		// 3x3 matrix used to transform from RGB to XYZ space.
		private readonly Matrix3 ConversionMatrix;
		private Vector3 WhiteReference;
		private double gamma;

		// TODO: WTF IS THAT
		private const double Epsilon = 0.008856; // Intent is 216/24389
		private const double Kappa = 903.3; // Intent is 24389/27

		// These four vectors are already in xyz space.
		public LabProcessor(Vector3 r, Vector3 g, Vector3 b, Vector3 w, double gamma)
		{
			w = w.MultiplyScalar(1/w.Y);
			var mtx = new Matrix3(new Vector3[] {r, g, b});
			var vec = mtx.Invert().MultiplyVector(w);
			var tmp = new Matrix3
			{
				tab =
				{
					[0, 0] = vec.X,
					[1, 1] = vec.Y,
					[2, 2] = vec.Z
				}
			};
			ConversionMatrix = mtx.MultiplyMatrix3(tmp);
			WhiteReference = w.MultiplyScalar(100);
			this.gamma = gamma;
		}

		public void Process(int a, int r, int g, int b, DirectBitmap bmp1, DirectBitmap bmp2, DirectBitmap bmp3, int x, int y)
		{
			var XYZ = ConversionMatrix.MultiplyVector(
				new Vector3(GammaCorrection(r, gamma), GammaCorrection(g, gamma), GammaCorrection(b, gamma)));

			var _x = PivotXyz(XYZ.X / WhiteReference.X);
			var _y = PivotXyz(XYZ.Y / WhiteReference.Y);
			var _z = PivotXyz(XYZ.Z / WhiteReference.Z);

			var L = Math.Max(0, 116*_y - 16);
			var A = 500*(_x - _y);
			var B = 200*(_y - _z);
		}

		private static double PivotXyz(double n)
		{
			return n > Epsilon ? Math.Pow(n, 1.0 / 3.0) : (Kappa * n + 16) / 116;
		}
		
		private static double GammaCorrection(double value, double gamma)
		{
			return Math.Pow(value, gamma);
		}
	}
}
