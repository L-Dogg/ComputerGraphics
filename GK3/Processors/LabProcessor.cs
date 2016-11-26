using System;
using GK3.Utilities;

namespace GK3.Processors
{
	public class LabProcessor : IProcessor
	{
		// 3x3 matrix used to transform from RGB to XYZ space.
		private Matrix3 ConversionMatrix;

		// Those four vectors are already in xyz coords where x + y + z = 1.
		public LabProcessor(Vector3 r, Vector3 g, Vector3 b, Vector3 w)
		{
			// Convert w to XYZ coords.
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
		}

		public void Process(int a, int r, int g, int b, DirectBitmap bmp1, DirectBitmap bmp2, DirectBitmap bmp3, int x, int y)
		{
			var rgb = new Vector3(r, g, b);
			var XYZ = ConversionMatrix.MultiplyVector(rgb);
		}
	}
}
