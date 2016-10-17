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
		/// <param name="clickedPoint"></param>
		/// <param name="operatingPolygon"></param>
		/// <returns>True if any of polygons' vertices was clicked.</returns>
		public static bool WasVertexClicked(Point clicked, IEnumerable<Polygon> polygons, out Point clickedPoint, out Polygon clickedPolygon)
		{
			clickedPoint = new Point(0, 0);
			clickedPolygon = null;

            foreach (var poly in polygons)
			{
				foreach (var point in poly.Points)
				{
					if (point.ComparePoints(clicked))
					{
						clickedPoint = point;
						clickedPolygon = poly;
						return true;
					}
				}
			}
			return false;
		}

		public static bool WasEdgeClicked(Point clicked, IEnumerable<Polygon> polygons, out Segment clickedSegment, out Polygon clickedPolygon)
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

		public static Point[] FindAdjacentPoints(Point p, Polygon poly)
		{
			var adjacentPoints = new Point[2];
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
