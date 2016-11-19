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
		public static Vector3D LightVector = new Vector3D(0.0, 0.0, 1.0);

		public static DirectBitmap FillTexture { get; set; }
		public static DirectBitmap BumpMap { get; set; }
		public static DirectBitmap NormalMap { get; set; }

		public static Color FillColor { get; set; } = Color.White;
		public static Color LightColor { get; set; } = Color.White;

		public LinkedList<Vertex> Vertices	{ get; set; } = new LinkedList<Vertex>();
		public LinkedList<Segment> Segments { get; set; } = new LinkedList<Segment>();
		public bool Finished { private get; set; } = false;

		private int MaxX => Vertices.Max(v => v.X);
		private int MinX => Vertices.Min(v => v.X);
		private int MaxY => Vertices.Max(v => v.Y);
		private int MinY => Vertices.Min(v => v.Y);

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
			double a = 0;
			var v = this.Vertices.ToList();
			for (var i = 0; i < v.Count - 1; i++)
				a += v[i].Cross(v[i + 1]);
			a += v.Last().Cross(v[0]);

			return a > 0;
		}

		private static void FillPixels(IReadOnlyList<Segment> segments, DirectBitmap directBmp, int y, bool fillColor = false, bool bumpMapping = false)
		{
			for (var i = 0; i < segments.Count() / 2; i++)
				for (var x = segments[2*i].Xmin; x <= segments[2*i + 1].Xmin; x++)
				{
					var currentBitColor = FillColor;
					if (!fillColor)
						currentBitColor = Color.FromArgb(FillTexture.Bits[((int)x) % FillTexture.Width + (y % FillTexture.Height) * FillTexture.Width]);
					
					var normalMapVector =
						new Vector3D(Color.FromArgb(NormalMap.Bits[((int) x)%NormalMap.Width + (y%NormalMap.Height)*NormalMap.Width]), true);

					if (bumpMapping)
						normalMapVector = DisturbNormalVector(normalMapVector, (int) x, y);
					
					var cos =  CalculateCosinus(LightVector, normalMapVector);
					cos = (cos > 0) ? cos : 0;


					var r = (double)LightColor.R / 255 * (double)currentBitColor.R / 255 * cos;
					var g = (double)LightColor.G / 255 * (double)currentBitColor.G / 255 * cos;
					var b = (double)LightColor.B / 255 * (double)currentBitColor.B / 255 * cos;

					directBmp.Bits[(int) x + y*directBmp.Width] = (255 << 24) | ((int) (r * 255) << 16) | ((int)(g * 255) << 8) | (int) (b * 255);
				}
		}

		private static double CalculateCosinus(Vector3D lightVector, Vector3D normalVector)
		{
			var light = new Vector3D(lightVector);
			// Jeżeli mamy inny niż domyślny wektor do światła:
			if (lightVector.X != 0.0 || lightVector.Y != 0.0 || lightVector.Z != 1.0)
			{
				light.X -= normalVector.X;
				light.Y -= normalVector.Y;
				light.Z -= normalVector.Z;
			}
			
			return (light.X*normalVector.X + light.Y*normalVector.Y + light.Z*normalVector.Z)/(light.Length * normalVector.Length);
		}

		private static Vector3D DisturbNormalVector(Vector3D v, int x, int y)
		{
			var hx = (CalculateHeight(x + 1, y) - CalculateHeight(x - 1, y)) / 2;
			var hy = (CalculateHeight(x, y + 1) - CalculateHeight(x, y - 1)) / 2;
			
			var t = new Vector3D(1, 0, -v.X);
			t.Normalize();
			var b = new Vector3D(0, 1, -v.Y);
			b.Normalize();
			
			var dx = hx * t.X + hy * b.X;
			var dy = hx * t.Y + hy * b.Y;
			var dz = hx * t.Z + hy * b.Z;
			
			v.X += dx;
			v.Y += dy;
			v.Z += dz;
			
			v.Normalize();

			return new Vector3D(v.X > 0 ? v.X : 0, v.Y > 0 ? v.Y : 0, v.Z > 0 ? v.Z : 0);
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

		public bool PointInsidePolygon(Vertex p)
		{
			if (p.X < MinX || p.X > MaxX || p.Y < MinY || p.Y > MaxY)
					return false;

			bool c = false;

			var vert = Vertices.ToList();

			for (int i = 0, j = Vertices.Count - 1; i < Vertices.Count; j = i++)
			{
				if (((vert[i].Y > p.Y) != (vert[j].Y > p.Y)) &&
				    (p.X < (vert[j].X - vert[i].X)*(p.Y - vert[i].Y)/(vert[j].Y - vert[i].Y) + vert[i].X))
					c = !c;
			}

			return c;
		}

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

		public bool IsSelfIntersecting()
		{
			Vertex v;
			var segList = this.Segments.ToList();
			for (var i = 0; i < segList.Count; i++)
				for (var j = i + 1; j < segList.Count; j++)
					if (SegmentHelper.LineSegementsIntersect(segList[i].From, segList[i].To, segList[j].From, segList[j].To, out v,
						true))
						return true;

			return false;
		}

		#endregion
	}
}
