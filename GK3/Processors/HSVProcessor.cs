using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GK3.Utilities;

namespace GK3.Processors
{
	class HSVProcessor : IProcessor
	{
		public void Process(int a, int r, int g, int b, DirectBitmap bmp1, DirectBitmap bmp2, DirectBitmap bmp3, int x, int y)
		{
			var r_ = r/255.0;
			var g_ = g/255.0;
			var b_ = b/255.0;
		}
	}
}
