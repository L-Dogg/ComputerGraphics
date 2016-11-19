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

			var Cmax = V = new [] { r, g, b }.Max();
			var Cmin = new [] { r, g, b }.Min();
			var delta = Cmax - Cmin;
			
			if (delta == 0.0) H = 0;
			else if (Cmax == r) H = 60*((g - b)/delta%6);
			else if (Cmax == g) H = 60*((b - r)/delta + 2);
			else H = 60*((r - g)/delta + 4);

			S = Cmax == 0.0 ? 0 : delta/Cmax;
			var H_ = (int) (H/360*255);
			var S_ = (int) (S*255);
			var V_ = (int) (V*255);
			bmp1[x, y] = a << 24 | (H_ << 16) | (H_ << 8) | H_;
			bmp2[x, y] = a << 24 | (S_ << 16) | (S_ << 8) | S_;
			bmp3[x, y] = a << 24 | (V_ << 16) | (V_ << 8) | V_;
		}
	}
}
