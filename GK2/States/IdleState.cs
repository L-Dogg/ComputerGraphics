

using GK2.Structures;
using GK2.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GK2.States
{
	public class IdleState : IState
	{
		#region Private Properties
		private MainForm MainForm { get; set; }
		private bool DeletingPolygon { get; set; }
		private Polygon CurrentPolygon { get; set; }
		private Segment CurrentSegment { get; set; }
		#endregion

		public IdleState(MainForm mainForm)
		{
			MainForm = mainForm;
			MainForm.Cursor = Cursors.Default;
			MainForm.PolygonContextMenu.ItemClicked += PolygonContextMenu_ItemClicked;
		}

		#region Context Menu Handlers
		private void PolygonContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			// Add
			if (e.ClickedItem == MainForm.PolygonContextMenu.Items[0])
			{
				MainForm.PolygonContextMenu.ItemClicked -= PolygonContextMenu_ItemClicked;
				MainForm.CurrentState = new DrawingState(MainForm);
			}
			// Remove
			if (e.ClickedItem == MainForm.PolygonContextMenu.Items[1])
			{
				this.DeletingPolygon = true;
			}
			// Move
			if (e.ClickedItem == MainForm.PolygonContextMenu.Items[2])
			{
				MainForm.PolygonContextMenu.ItemClicked -= PolygonContextMenu_ItemClicked;
				MainForm.CurrentState = new PolygonMoveState(MainForm);
			}
			// Union
			if (e.ClickedItem == MainForm.PolygonContextMenu.Items[3])
			{
				MainForm.PolygonContextMenu.ItemClicked -= PolygonContextMenu_ItemClicked;
				MainForm.CurrentState = new PolygonUnionState(MainForm);
			}
		}
        #endregion

        #region IState
        public void MouseDown(object sender, MouseEventArgs e)
		{
			var clickedVertex = new Vertex(0,0);
			Segment segment;
			Polygon polygon;
			var point = new Vertex(e.X, e.Y);
			var wasEdgeClicked = ClickChecker.WasEdgeClicked(point, MainForm.Polygons, out segment, out polygon);
			var wasVertexClicked = false;

			if (!wasEdgeClicked)
			{
				wasVertexClicked = ClickChecker.WasVertexClicked(point, MainForm.Polygons, out clickedVertex, out polygon);
			}
			else
			{
				MainForm.CurrentPolygon = polygon;
				MainForm.CurrentSegment = segment;
				CurrentSegment = segment;
				CurrentPolygon = polygon;
			}

			// Context Menu
			if (e.Button == MouseButtons.Right)
			{
				MainForm.SetContextMenuItems();
				MainForm.PolygonContextMenu.Show(MainForm, new Point(point.X, point.Y));
			}
			// Vertex deletion
			else if (!DeletingPolygon && wasVertexClicked && (Control.ModifierKeys == Keys.Control && e.Button == MouseButtons.Left))
			{
				if (polygon.Vertices.Count == 3)
				{
					MainForm.Polygons.Remove(polygon);
					MainForm.Render();
					return;
				}
				var adjacentPoints = ClickChecker.FindAdjacentPoints(clickedVertex, polygon);
				polygon.Vertices.Remove(clickedVertex);
				var edgeToAddAfter = polygon.Segments.First(line => line.From == clickedVertex || line.To == clickedVertex);				
				polygon.Segments.AddAfter(polygon.Segments.Find(edgeToAddAfter), new Segment(adjacentPoints[0], adjacentPoints[1]));
				polygon.Segments = new LinkedList<Segment>(polygon.Segments.Where((line) => line.From != clickedVertex && line.To != clickedVertex));
				CurrentPolygon = polygon;
				
				MainForm.Render();
			}
			// Polygon deletion
			else if (DeletingPolygon && (wasEdgeClicked || wasVertexClicked) && e.Button == MouseButtons.Left)
			{
				MainForm.Polygons.Remove(polygon);
				if (polygon == MainForm.CurrentPolygon)
					MainForm.CurrentPolygon = new Polygon();

                MainForm.Render();
				DeletingPolygon = false;
			}
			// Vertex addition in the middle of an edge
			else if (!DeletingPolygon && wasEdgeClicked && e.Button == MouseButtons.Left)
			{
				var midPoint = new Vertex(Math.Min(segment.From.X, segment.To.X) + Math.Abs(segment.From.X - segment.To.X) / 2,
					Math.Min(segment.From.Y, segment.To.Y) + Math.Abs(segment.From.Y - segment.To.Y) / 2);

				polygon.Segments.AddBefore(polygon.Segments.Find(segment), new Segment(segment.From, midPoint));
				polygon.Segments.AddBefore(polygon.Segments.Find(segment), new Segment(midPoint, segment.To));
				polygon.Segments.Remove(segment);

				polygon.Vertices.AddAfter(polygon.Vertices.Find(segment.From), midPoint);
				
				MainForm.Render();
			}
			// Vertex move
			else if (!DeletingPolygon && wasVertexClicked && polygon.Vertices.First.Value != clickedVertex && e.Button == MouseButtons.Left)
			{
				MainForm.PolygonContextMenu.ItemClicked -= PolygonContextMenu_ItemClicked;
				MainForm.CurrentState = new VertexMoveState(MainForm, polygon, clickedVertex);
			}
		}

		public void MouseMove(object sender, MouseEventArgs e)
		{
			
		}

		public void KeyUp(object sender, KeyEventArgs e)
		{
			if (DeletingPolygon && e.KeyCode == Keys.Escape)
				DeletingPolygon = false;
		}
		
		public void Render(DirectBitmap bitmap, Graphics g)
		{
			foreach (var polygon in MainForm.Polygons)
				polygon.Render(bitmap, MainForm.ColorFill, MainForm.BumpMapping);
		}
		
		#endregion
	}
}
