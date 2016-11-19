using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GK3.Utilities;

namespace GK3.Processors
{
	public class RgbProcessor : IProcessor
	{
		public void Process(int a, int r, int g, int b, DirectBitmap bmp1, DirectBitmap bmp2, DirectBitmap bmp3, int x, int y)
		{
			bmp1[x, y] = (a << 24) + (r << 16);
			bmp2[x, y] = (a << 24) + (g << 8);
			bmp3[x, y] = (a << 24) + b;
		}
	}
}
