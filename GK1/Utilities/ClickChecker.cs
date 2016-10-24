using GK1.Structures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK1.Utilities
{
	public static class ClickChecker
	{
		/// <summary>
		/// Iterates over polygons and checks whether one of their vertices was clicked.
		/// </summary>
		/// <param name="clicked"></param>
		/// <param name="polygons"></param>
		/// <param name="clickedVertex"></param>
		/// <param name="operatingPolygon"></param>
		/// <returns>True if any of polygons' vertices was clicked.</returns>
		public static bool WasVertexClicked(Vertex clicked, IEnumerable<Polygon> polygons, out Vertex clickedVertex, out Polygon clickedPolygon)
		{
			clickedVertex = null;
			clickedPolygon = null;

            foreach (var poly in polygons)
			{
				foreach (var point in poly.Vertices)
				{
					if (point.ComparePoints(clicked))
					{
						clickedVertex = point;
						clickedPolygon = poly;
						return true;
					}
				}
			}
			return false;
		}

		public static bool WasEdgeClicked(Vertex clicked, IEnumerable<Polygon> polygons, out Segment clickedSegment, out Polygon clickedPolygon)
		{
			clickedSegment = null;
			clickedPolygon = null;

            foreach (var poly in polygons)
			{
				foreach (var segment in poly.Segments)
				{
					if (clicked.IsCloseToLine(segment))
					{
						clickedPolygon = poly;
						clickedSegment = segment;
						return true;
					}
				}
			}
			return false;
		}

		public static Vertex[] FindAdjacentPoints(Vertex p, Polygon poly)
		{
			var adjacentPoints = new Vertex[2];
			int i = 0;

			foreach (var line in poly.Segments)
			{
				if (line.From == p)
					adjacentPoints[i++] = line.To;
				else if (line.To == p)
					adjacentPoints[i++] = line.From;

				if (i == 2)
					return adjacentPoints;
			}
			return adjacentPoints;
		}
	}
}
