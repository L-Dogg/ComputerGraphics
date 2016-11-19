using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GK3.Utilities;

namespace GK3.Processors
{
	public class HSVProcessor : IProcessor
	{
		public void Process(int a, int r_, int g_, int b_, DirectBitmap bmp1, DirectBitmap bmp2, DirectBitmap bmp3, int x, int y)
		{
			var r = r_/255.0;
			var g = g_/255.0;
			var b = b_/255.0;

			double H, S, V;

			var Cmax = V = new double[] { r, g, b }.Max();
			var Cmin = new double[] { r, g, b }.Min();
			var delta = Cmax - Cmin;
			
			if (delta == 0.0) H = 0;
			else if (Cmax == r) H = 60*((g - b)/delta%6);
			else if (Cmax == g) H = 60*((b - r)/delta + 2);
			else H = 60*((r - g)/delta + 4);

			S = Cmax == 0.0 ? 0 : delta/Cmax;
		}
	}
}
