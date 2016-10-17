using GK1.Structures;
using GK1.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GK1.States
{
	enum InnerState
	{
		Regular,
		DeletedVertex,
		DeletedPolygon,
		AddedVertex
	}

	class IdleState : IState
	{
		#region Private Properties
		private MainForm MainForm { get; set; }
		private Bitmap Bitmap { get; set; }
		private bool DeletingPolygon { get; set; }
		private Polygon currentPolygon { get; set; }
		private InnerState InnerState { get; set; } = InnerState.Regular;
		#endregion

		public IdleState(MainForm mainForm, Bitmap bitmap)
		{
			MainForm = mainForm;
			MainForm.RelationContextMenu.ItemClicked += RelationContextMenu_ItemClicked;
			MainForm.PolygonContextMenu.ItemClicked += PolygonContextMenu_ItemClicked;
			Bitmap = bitmap;
		}

		#region Context Menu Handlers
		private void PolygonContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			// Add
			if (e.ClickedItem == MainForm.PolygonContextMenu.Items[0])
			{
				MainForm.CurrentState = new PolygonMoveState(); // TODO CONSTRUCTOR
			}
			// Remove
			if (e.ClickedItem == MainForm.PolygonContextMenu.Items[1])
			{
				this.DeletingPolygon = true;
			}
			// Move
			if (e.ClickedItem == MainForm.PolygonContextMenu.Items[2])
			{
				MainForm.CurrentState = new DrawingState(); // TODO CONSTRUCTOR
			}
		}

		private void RelationContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			// Horizontal
			if (e.ClickedItem == MainForm.RelationContextMenu.Items[0])
			{

			}
			// Vertical
			if (e.ClickedItem == MainForm.RelationContextMenu.Items[1])
			{

			}
			// Length
			if (e.ClickedItem == MainForm.RelationContextMenu.Items[2])
			{

			}
		}
		#endregion

		public void MouseDown(object sender, MouseEventArgs e)
		{
			// Tryb usuwania wielokąta (LMB na dowolną część wielokąta)
			// Tryb dodawania wierzchołka w środku krawędzi
			Point clickedVertex;
			Segment segment;
			Polygon polygon;
			var point = new Point(e.X, e.Y);
			var wasEdgeClicked = ClickChecker.WasEdgeClicked(point, MainForm.Polygons, out segment, out polygon);
			var wasVertexClicked = ClickChecker.WasVertexClicked(point, MainForm.Polygons, out clickedVertex, out polygon);

			// Context Menu
			if (e.Button == MouseButtons.Right)
			{
				MainForm.SetContextMenuItems(segment);

				if (wasEdgeClicked)
					MainForm.RelationContextMenu.Show(MainForm, point);
				else
					MainForm.PolygonContextMenu.Show(MainForm, point);
			}
			// Vertex deletion
			else if (!DeletingPolygon && Control.ModifierKeys == Keys.Control && e.Button == MouseButtons.Left)
			{
				if (polygon.Points.Count == 3)
				{
					MainForm.Polygons.Remove(polygon);
					MainForm.Render();
					return;
				}
				var adjacentPoints = ClickChecker.FindAdjacentPoints(clickedVertex, polygon);
				polygon.Points.Remove(clickedVertex);
				var edgeToAddAfter = polygon.Segments.Where((line) => { return line.From == clickedVertex; }).First();
				polygon.Segments.AddAfter(new LinkedListNode<Segment>(edgeToAddAfter), new Segment(adjacentPoints[0], adjacentPoints[1]));
				polygon.Segments = new LinkedList<Segment>(polygon.Segments.Where((line) => { return line.From != clickedVertex && line.To != clickedVertex; }));

				InnerState = InnerState.DeletedVertex;
				currentPolygon = polygon;
			}
			// Polygon deletion
			else if (DeletingPolygon && e.Button == MouseButtons.Left)
			{
				MainForm.Polygons.Remove(polygon);
				InnerState = InnerState.DeletedPolygon;
				MainForm.Render();
			}
		}

		public void MouseMove(object sender, MouseEventArgs e)
		{
			
		}
		
		public void Render(Bitmap bitmap, Graphics g)
		{
			// Optimized - not drawing polygons that hasn't changed.
			if (InnerState == InnerState.DeletedVertex)
			{
				currentPolygon.Render(bitmap, g);
				InnerState = InnerState.Regular;
				return;
			}

			foreach (var polygon in MainForm.Polygons)
				polygon.Render(bitmap, g);
        }
	}
}
