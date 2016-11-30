using System;
using GK3.Utilities;

namespace GK3.Processors
{
	public class LabProcessor : IProcessor
	{
		// 3x3 matrix used to transform from RGB to XYZ space.
		private readonly Matrix3 ConversionMatrix;
		private readonly Vector3 WhiteReference;
		private readonly double gamma;

		private const double Epsilon = 0.008856; // Intent is 216/24389
		private const double Kappa = 903.3; // Intent is 24389/27

		// These four vectors are already in xyz space.
		public LabProcessor(LabData data)
		{
			Vector3 w = data.W.MultiplyScalar(1/data.W.Y);
			var mtx = new Matrix3(new Vector3[] {data.R, data.G, data.B});
			var inv = mtx.Invert();
			var ret = inv.MultiplyVector(w);
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
			this.gamma = data.Gamma;
		}

		public void Process(int a, int r, int g, int b, DirectBitmap bmp1, DirectBitmap bmp2, DirectBitmap bmp3, int x, int y)
		{
			var XYZ = ConversionMatrix.MultiplyVector(
				new Vector3(GammaCorrection((double)r / 255.0, gamma), GammaCorrection((double)g / 255.0, gamma), GammaCorrection((double)b / 255.0, gamma)));

			var _x = PivotXyz(XYZ.X / WhiteReference.X);
			var _y = PivotXyz(XYZ.Y / WhiteReference.Y);
			var _z = PivotXyz(XYZ.Z / WhiteReference.Z);

			var L = Math.Max(0, 116 * _y - 16) / 100;
			var A = 500*(_x - _y) + 128;
			var B = 200*(_y - _z) + 128;

			bmp1[x, y] = (a << 24) | ((int)(L * 255) << 16) | ((int)(L * 255) << 8) | (int)(L * 255);
			bmp2[x, y] = (a << 24) | ((int) (A) << 16) | ((int) ((255 - A)) << 8) | 128;
			bmp3[x, y] = (a << 24) | ((int) (B) << 16) | 128 << 8 | (int) ((255 - B));
		}

		private static double PivotXyz(double n)
		{
			return n > Epsilon ? Math.Pow(n, 1.0 / 3.0) : (Kappa * n + 16) / 116;
		}
		
		private static double GammaCorrection(double value, double gamma)
		{
			return Math.Pow(value, gamma) * 100;
		}
	}
}
