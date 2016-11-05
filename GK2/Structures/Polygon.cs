using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GK2.Structures
{
	public class Polygon
	{
		public static Bitmap DefaultFillTexture { get; set; }
		public static Color DefaultLightColor { get; set; } = Color.FromArgb(127, 127, 255);


		public LinkedList<Vertex> Vertices	{ get; } = new LinkedList<Vertex>();
		public LinkedList<Segment> Segments { get; set; } = new LinkedList<Segment>();
		public bool Finished { private get; set; } = false;
		
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

		private void FillPolygon(Color color, Bitmap bmp)
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
				FillPixels(activeEt, color, bmp, y);
								
				y++;

				foreach (var segment in activeEt)
					segment.Xmin += segment.DxDy;
			}
		}

		private static void FillPixels(IReadOnlyList<Segment> segments, Color color, Bitmap bmp, int y)
		{
			for (var i = 0; i < segments.Count() / 2; i++)
				for (var x = segments[2 * i].Xmin; x <= segments[2 * i + 1].Xmin; x++)
					bmp.SetPixel((int)x, y, DefaultFillTexture.GetPixel(((int) x) % DefaultFillTexture.Width , y % DefaultFillTexture.Height));
		}

		public void Render(Bitmap bmp, Graphics g)
		{
		    if (Finished)
		    {
                GenerateEdgeTable();
                FillPolygon(Color.BlueViolet, bmp);
		    }


            foreach (var line in Segments)
			{
				Algorithms.Algorithms.Line(line.From.X, line.From.Y, line.To.X, line.To.Y, bmp);
			}

			foreach (var point in Vertices)
				point.Draw(bmp);

			Vertices.First.Value.Draw(bmp, Color.Blue);
		}

		public void SetMovedXmin(Vertex v)
		{
			var changedSegments = Segments.Where(segment => segment.From == v || segment.To == v);
			foreach (var seg in changedSegments)
			{
				seg.Xmin = seg.StartXmin;
			}
		}
	}
}
