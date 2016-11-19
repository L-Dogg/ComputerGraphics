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
		private DirectBitmap directBitmap;
		private DirectBitmap rBitmap;
		private DirectBitmap gBitmap;
		private DirectBitmap bBitmap;
		
		public RgbProcessor(DirectBitmap directBitmap)
		{
			this.directBitmap = directBitmap;
			rBitmap = new DirectBitmap(directBitmap.Width, directBitmap.Height);
			gBitmap = new DirectBitmap(directBitmap.Width, directBitmap.Height);
			bBitmap = new DirectBitmap(directBitmap.Width, directBitmap.Height);
		}

		public void Process(PictureBox pb1, PictureBox pb2, PictureBox pb3)
		{
			for (var x = 0; x < directBitmap.Width; x++)
			{
				for (var y = 0; y < directBitmap.Height; y++)
				{
					var color = directBitmap[x, y];
					int a = ((byte) (color >> 24)) & 255;
					var r = ((byte)(color >> 16)) & 255;
					var g = ((byte)(color >> 8)) & 255;
					var b = ((byte)color) & 255;

					rBitmap[x, y] = (a << 24) + (r << 16);
					gBitmap[x, y] = (a << 24) + (g << 8);
					bBitmap[x, y] = (a << 24) + b;
				}
			}

			BindToGui(pb1, pb2, pb3);
		}

		public void BindToGui(PictureBox a, PictureBox b, PictureBox c)
		{
			a.Image = rBitmap.Bitmap;
			b.Image = gBitmap.Bitmap;
			c.Image = bBitmap.Bitmap;
		}
	}
}
