using GK1.Controls;
using GK1.Relations;
using GK1.Structures;
using GK1.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GK1.States
{
	class IdleState : IState
	{
		#region Private Properties
		private MainForm MainForm { get; set; }
		private bool DeletingPolygon { get; set; }
		private Polygon currentPolygon { get; set; }
		private Segment currentSegment { get; set; }
		private static NoneRelation noneRelation = new NoneRelation();
		private static HorizontalRelation horizontalRelation = new HorizontalRelation();
		private static VerticalRelation verticalRelation = new VerticalRelation();
		private static LengthRelation lengthRelation = new LengthRelation();
		private static string ErrorMessage = "Cannot add relation";
		#endregion

		public IdleState(MainForm mainForm)
		{
			MainForm = mainForm;
			MainForm.Cursor = Cursors.Default;
            MainForm.RelationContextMenu.ItemClicked += RelationContextMenu_ItemClicked;
			MainForm.PolygonContextMenu.ItemClicked += PolygonContextMenu_ItemClicked;
		}

		#region Context Menu Handlers
		private void PolygonContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			// Add
			if (e.ClickedItem == MainForm.PolygonContextMenu.Items[0])
			{
				MainForm.RelationContextMenu.ItemClicked -= RelationContextMenu_ItemClicked;
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
				MainForm.RelationContextMenu.ItemClicked -= RelationContextMenu_ItemClicked;
				MainForm.PolygonContextMenu.ItemClicked -= PolygonContextMenu_ItemClicked;
				MainForm.CurrentState = new PolygonMoveState(MainForm);
			}
		}
		
		private void RelationContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			// Horizontal
			if (e.ClickedItem == MainForm.RelationContextMenu.Items[0])
			{
				if (AddRelation(RelationType.Horizontal))
					MainForm.Render();
			}
			// Vertical
			else if (e.ClickedItem == MainForm.RelationContextMenu.Items[1])
			{
				if (AddRelation(RelationType.Vertical))
					MainForm.Render();
			}
			// Length
			if (e.ClickedItem == MainForm.RelationContextMenu.Items[2])
			{
				if (AddRelation(RelationType.Length))
					MainForm.Render();
			}
			// None
			if (e.ClickedItem == MainForm.RelationContextMenu.Items[4])
			{
				if (AddRelation(RelationType.None))
					MainForm.Render();
			}

		}
		#endregion
				
		#region Relation Processing
		
		private bool AddRelation(RelationType relationType)
		{
			MainForm.CurrentPolygon.SaveVertices();
			switch (relationType)
			{
				case RelationType.None:
					MainForm.CurrentSegment.Relation = noneRelation;
					break;
				case RelationType.Horizontal:
					MainForm.CurrentSegment.Relation = horizontalRelation;
					break;
				case RelationType.Vertical:
					MainForm.CurrentSegment.Relation = verticalRelation;
					break;
				case RelationType.Length:
					MainForm.LengthMessageBox = new Length(MainForm.CurrentSegment.Length);
					MainForm.LengthMessageBox.ShowDialog();

					if (!MainForm.LengthMessageBox.wasOK)
						return false;

					MainForm.CurrentSegment.DesiredLength = MainForm.LengthMessageBox.LengthTyped;
					MainForm.CurrentSegment.Relation = lengthRelation;
					break;
			}

			var retVal = MainForm.CurrentPolygon.Apply();
			if (!retVal)
			{
				MainForm.CurrentPolygon.LoadVertices();
				MainForm.CurrentSegment.Relation = noneRelation;
				MessageBox.Show(ErrorMessage);
			}

			return retVal;
		}

		#endregion

		#region IState
		public void MouseDown(object sender, MouseEventArgs e)
		{
			Vertex clickedVertex = new Vertex(0,0);
			Segment segment;
			Polygon polygon;
			var point = new Vertex(e.X, e.Y);
			var wasEdgeClicked = ClickChecker.WasEdgeClicked(point, MainForm.Polygons, out segment, out polygon);
			bool wasVertexClicked = false;
			if (!wasEdgeClicked)
				wasVertexClicked = ClickChecker.WasVertexClicked(point, MainForm.Polygons, out clickedVertex, out polygon);
			else
			{
				MainForm.CurrentPolygon = polygon;
				MainForm.CurrentSegment = segment;
				currentSegment = segment;
				currentPolygon = polygon;
			}

			// Context Menu
			if (e.Button == MouseButtons.Right)
			{
				MainForm.SetContextMenuItems(segment);

				if (wasEdgeClicked)
					MainForm.RelationContextMenu.Show(MainForm, new Point(point.X, point.Y));
				else
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
				var edgeToAddAfter = polygon.Segments.Where((line) => { return line.From == clickedVertex || line.To == clickedVertex; }).First();				
				polygon.Segments.AddAfter(polygon.Segments.Find(edgeToAddAfter), new Segment(adjacentPoints[0], adjacentPoints[1]));
				polygon.Segments = new LinkedList<Segment>(polygon.Segments.Where((line) => { return line.From != clickedVertex && line.To != clickedVertex; }));
				currentPolygon = polygon;
				
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

				MainForm.RelationContextMenu.ItemClicked -= RelationContextMenu_ItemClicked;
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
		
		public void Render(Bitmap bitmap, Graphics g)
		{
			foreach (var polygon in MainForm.Polygons)
				polygon.Render(bitmap, g);
        }
		
		#endregion
	}
}
