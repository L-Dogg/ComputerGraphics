using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GK2.Utilities;

namespace GK2.Structures
{
	public class Polygon
	{
		#region Public Fields
		public static double LightX { get; set; } = 0;
		public static double LightY { get; set; } = 0;
		public static double LightZ { get; set; } = 1;

		public static DirectBitmap FillTexture { get; set; }
		public static DirectBitmap BumpMap { get; set; }
		public static DirectBitmap NormalMap { get; set; }

		public static Color FillColor { get; set; } = Color.White;
		public static Color LightColor { get; set; } = Color.White;

		public LinkedList<Vertex> Vertices	{ get; set; } = new LinkedList<Vertex>();
		public LinkedList<Segment> Segments { get; set; } = new LinkedList<Segment>();
		public bool Finished { private get; set; } = false;
		#endregion

		#region Private Methods

		private Dictionary<int, List<Segment>> _edgeTable = new Dictionary<int, List<Segment>>();
		
		private void GenerateEdgeTable()
	    {
			_edgeTable = new Dictionary<int, List<Segment>>();

			foreach (var segment in Segments)
		    {
				if (!_edgeTable.ContainsKey(segment.Ymin))
					_edgeTable.Add(segment.Ymin, new List<Segment>());

				_edgeTable[segment.Ymin].Add(segment);
			}

	    }

		private void FillPolygon(DirectBitmap bmp, bool fillColor = false, bool bumpMapping = false)
		{
			var activeEt = new List<Segment>();
			var edgeTableElements = Segments.Count;
			var y = _edgeTable.Keys.Min();
			while (activeEt.Count > 0 || edgeTableElements > 0)
			{
				if (edgeTableElements > 0 && _edgeTable.ContainsKey(y))
				{
					activeEt.AddRange(_edgeTable[y]);
					edgeTableElements -= _edgeTable[y].Count;
					_edgeTable[y].Clear();
				}

				foreach (var segment in activeEt.Where(seg => seg.Ymax == y))
					segment.Xmin = segment.StartXmin;
				activeEt.RemoveAll(seg => seg.Ymax == y);

				activeEt.Sort((s1, s2) => s1.Xmin.CompareTo(s2.Xmin));
				FillPixels(activeEt, bmp, y, fillColor, bumpMapping);
								
				y++;

				foreach (var segment in activeEt)
					segment.Xmin += segment.DxDy;
			}
		}

		private bool CheckIfClockwise()
		{
			return Segments.Sum(segment => (segment.To.X - segment.From.X) * (segment.To.Y + segment.From.Y)) > 0;
		}

		private static void FillPixels(IReadOnlyList<Segment> segments, DirectBitmap directBmp, int y, bool fillColor = false, bool bumpMapping = false)
		{
			for (var i = 0; i < segments.Count() / 2; i++)
				for (var x = segments[2*i].Xmin; x <= segments[2*i + 1].Xmin; x++)
				{
					var currentBitColor = FillColor;
					var bumpMapColor = Color.White;
					if (!fillColor)
						currentBitColor = Color.FromArgb(FillTexture.Bits[((int)x) % FillTexture.Width + (y % FillTexture.Height) * FillTexture.Width]);


					var normalMapColor = Color.FromArgb(NormalMap.Bits[((int)x) % NormalMap.Width + (y % NormalMap.Height) * NormalMap.Width]);

					if (bumpMapping)
					{
						normalMapColor = DisturbNormalVector(normalMapColor, (int) x, y);
					}

					var cos =  CalculateCosinus(LightX, LightY, LightZ, normalMapColor);
					cos = (cos > 0) ? cos : 0;

					var r = (double)LightColor.R / 255 * (double)currentBitColor.R / 255 * cos;
					var g = (double)LightColor.G / 255 * (double)currentBitColor.G / 255 * cos;
					var b = (double)LightColor.B / 255 * (double)currentBitColor.B / 255 * cos;

					directBmp.Bits[(int) x + y*directBmp.Width] = Color.FromArgb((int) (r*255), (int) (g*255), (int) (b*255)).ToArgb();
				}
		}

		private static double CalculateCosinus(double x, double y, double z, Color color)
		{
			var nX = (double) (color.R) / 255;
			var nY = (double) (color.G) / 255;
			var nZ = (double) color.B / 255;

			return (x*nX + y*nY + z*nZ)/(Math.Sqrt(x*x + y*y + z*z) * Math.Sqrt(nX*nX + nY*nY + nZ*nZ));
		}

		private static Color DisturbNormalVector(Color color, int x, int y)
		{
			var nX = (double)(color.R - 127) / 127;
			var nY = (double)(color.G - 127) / 127;
			var nZ = (double)color.B / 255;

			var hx = (CalculateHeight(x + 1, y) - CalculateHeight(x - 1, y)) / 2;
			var hy = (CalculateHeight(x, y + 1) - CalculateHeight(x, y - 1)) / 2;
			
			double tX = 1; double tY = 0; double tZ = -nX;
			double bX = 0; double bY = 1; double bZ = -nY;

			var tLen = Math.Sqrt(tX * tX + tY * tY + tZ * tZ);
			var bLen = Math.Sqrt(bX * bX + bY * bY + bZ * bZ);

			tX /= tLen;
			tY /= tLen;
			tZ /= tLen;

			bX /= bLen;
			bY /= bLen;
			bZ /= bLen;

			var dx = hx * tX + hy * bX;
			var dy = hx * tY + hy * bY;
			var dz = hx * tZ + hy * bZ;
			
			nX += dx;
			nY += dy;
			nZ += dz;

			var normalLength = Math.Sqrt(nX * nX + nY * nY + nZ * nZ);

			nX /= normalLength;
			nY /= normalLength;
			nZ /= normalLength;
			
			return Color.FromArgb((int)(nX*127 + 127) < 0 ? 0 : (int)(nX * 127 + 127), (int)(nY*127 + 127) < 0 ? 0 : (int)(nY * 127 + 127), (int)(nZ*255) < 0 ? 0 : (int)(nZ*255));
		}

		private static double CalculateHeight(int x, int y)
		{
			var heightColor = Color.FromArgb(BumpMap.Bits[(y % BumpMap.Height)*BumpMap.Width + x % BumpMap.Width]);

			var heightX = (double)heightColor.R / 255;
			var heightY = (double)heightColor.G / 255;
			var heightZ = (double)heightColor.B / 255;

			return (heightX + heightY + heightZ) / 3;
		}


		#endregion

		#region Public Methods
		public void Render(DirectBitmap bmp, Color lineColor, bool fillColor = false, bool bumpMap = false)
		{
		    if (Finished)
		    {		
                GenerateEdgeTable();
                FillPolygon(bmp, fillColor, bumpMap);
		    }


            foreach (var line in Segments)
			{
				Algorithms.Algorithms.Line(line.From.X, line.From.Y, line.To.X, line.To.Y, bmp, lineColor);
			}

			foreach (var point in Vertices)
				point.Draw(bmp);

			Vertices.First.Value.Draw(bmp, Color.Blue);
		}

		public void Render(DirectBitmap bmp, bool fillColor = false, bool bumpMap = false)
		{
			this.Render(bmp, Color.Black, fillColor, bumpMap);
		}

		public void SetMovedXmin(Vertex v)
		{
			var changedSegments = Segments.Where(segment => segment.From == v || segment.To == v);
			foreach (var seg in changedSegments)
			{
				seg.Xmin = seg.StartXmin;
			}
		}

		/// <summary>
		/// Normalizes polygon - all polygon's vertices and edges must be in CCW order.
		/// </summary>
		public void NormalizePolygon()
		{
			if (!CheckIfClockwise())
				return;

			Segments = new LinkedList<Segment>(Segments.Reverse());
			foreach (var seg in this.Segments)
			{
				var tmp = seg.From;
				seg.From = seg.To;
				seg.To = tmp;

			}
			Vertices = new LinkedList<Vertex>(Vertices.Reverse());
		}

		#endregion
	}
}
