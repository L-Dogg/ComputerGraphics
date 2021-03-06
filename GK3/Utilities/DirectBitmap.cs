﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace GK3.Utilities
{
	public class DirectBitmap : IDisposable
	{
		public Bitmap Bitmap { get; }
		public int[] Bits { get; }
		public bool Disposed { get; private set; }
		public int Height { get; private set; }
		public int Width { get; private set; }

		protected GCHandle BitsHandle { get; }

		public static DirectBitmap FromBitmap(Bitmap bitmap)
		{
			var directBitmap = new DirectBitmap(bitmap.Width, bitmap.Height);
			var graphics = Graphics.FromImage(directBitmap.Bitmap);
			graphics.DrawImage(bitmap, Point.Empty);
			graphics.Dispose();
			return directBitmap;
		}

		public DirectBitmap(int width, int height)
		{
			Width = width;
			Height = height;
			Bits = new int[width * height];
			BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
			Bitmap = new Bitmap(width, height, width * 4, PixelFormat.Format32bppPArgb, BitsHandle.AddrOfPinnedObject());
		}

		public void Dispose()
		{
			if (Disposed) return;
			Disposed = true;
			Bitmap.Dispose();
			BitsHandle.Free();
		}

		public int this[int x, int y]
		{
			get { return Bits[y*this.Width + x]; }
			set { Bits[y*this.Width + x] = value; }
		}
	}
}
