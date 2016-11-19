using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GK3.Utilities;

namespace GK3.Processors
{
	public class Processor
	{
		private readonly DirectBitmap directBitmap;
		private readonly DirectBitmap rBitmap;
		private readonly DirectBitmap gBitmap;
		private readonly DirectBitmap bBitmap;

		public Processor(DirectBitmap directBitmap)
		{
			this.directBitmap = directBitmap;
			rBitmap = new DirectBitmap(directBitmap.Width, directBitmap.Height);
			gBitmap = new DirectBitmap(directBitmap.Width, directBitmap.Height);
			bBitmap = new DirectBitmap(directBitmap.Width, directBitmap.Height);
		}

		public void Process(PictureBox pb1, PictureBox pb2, PictureBox pb3, IProcessor processor)
		{
			for (var x = 0; x < directBitmap.Width; x++)
			{
				for (var y = 0; y < directBitmap.Height; y++)
				{
					var color = directBitmap[x, y];
					var a = ((byte) (color >> 24)) & 255;
					var r = ((byte) (color >> 16)) & 255;
					var g = ((byte) (color >> 8)) & 255;
					var b = ((byte) color) & 255;

					processor.Process(a, r, g, b, rBitmap, gBitmap, bBitmap, x, y);
				}
			}
			BindToGui(pb1, pb2, pb3);
		}

		private void BindToGui(PictureBox a, PictureBox b, PictureBox c)
		{
			a.Image = rBitmap.Bitmap;
			b.Image = gBitmap.Bitmap;
			c.Image = bBitmap.Bitmap;
		}
	}
}
