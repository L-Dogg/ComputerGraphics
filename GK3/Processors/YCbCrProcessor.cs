﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GK3.Utilities;

namespace GK3.Processors
{
	public class YCbCrProcessor : IProcessor
	{
		public void Process(int a, int r, int g, int b, DirectBitmap bmp1, DirectBitmap bmp2, DirectBitmap bmp3, int x, int y)
		{
			//TODO: Get the proper formula
			var _y = (0.299 * r + 0.587 * g + 0.114 * b);
			var cb = (-0.14713 * r - 0.28886 * g + 0.436 * b) + 128;
			var cr = (0.615 * r - 0.51499 * g - 0.10001 * b) + 128;

			// TODO: fix Cb and Cr channels!!!
			//https://en.wikipedia.org/wiki/File:Barn-yuv.png#Generation
			/*
				The first channel of the blue/yellow map has to be set to U as well 
				and the third channel of the red/green map has to be set to 0.5.
				$yuv_img->setPixel($x, $height+$y, cached_allocate($yuv_img, $Y, $Y, $Y));
				$yuv_img->setPixel($x, 2*$height+$y, cached_allocate($yuv_img, 0, 255-$U, $U));
				$yuv_img->setPixel($x, 3*$height+$y, cached_allocate($yuv_img, $V, 255-$V, 0));
			*/
			_y = Math.Round(_y);
			cb = Math.Round(cb);
			cr = Math.Round(cr);
			bmp1[x, y] = (a << 24) | ((int)_y << 16) | ((int)_y << 8) | (int)_y;
			bmp2[x, y] = (a << 24) | ((int)cb << 16) | ((int)(255 - cb) << 8) | (int)cb;
			bmp3[x, y] = (a << 24) | ((int)cr << 16) | ((int)(255 - cr) << 8) | 128;
		}
	}
}