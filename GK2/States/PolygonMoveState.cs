using GK2.Structures;
using GK2.Utilities;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GK2.States
{
	// TODO: can't move out of state!!!
	public class PolygonMoveState : IState
	{
		#region Private Properties
		private MainForm MainForm { get; set; }
		private Polygon Polygon { get; set; }
		private Vertex Vertex { get; set; }
		private bool Moving { get; set; }
		#endregion

		public PolygonMoveState(MainForm mainForm)
		{
			MainForm = mainForm;
			MainForm.Cursor = Cursors.NoMove2D;
			Moving = false;
		}

		#region IState
		public void MouseDown(object sender, MouseEventArgs e)
		{
			var point = new Vertex(e.X, e.Y);
			Polygon polygon;
			Vertex vertex;
			var wasVertexClicked = ClickChecker.WasVertexClicked(point, MainForm.Polygons, out vertex, out polygon);

			if (!Moving && wasVertexClicked && e.Button == MouseButtons.Left)
			{
				Vertex = vertex;
				Polygon = polygon;
				Moving = true;
			}
			else if (Moving && e.Button == MouseButtons.Left)
			{
				Moving = false;
				MainForm.CurrentState = new IdleState(MainForm);
			}
		}

		public void MouseMove(object sender, MouseEventArgs e)
		{
			if (!Moving)
				return;

			var xDiff = e.X - Vertex.X;
			var yDiff = e.Y - Vertex.Y;
			
			foreach (var p in Polygon.Vertices)
			{
				p.X = p.X + xDiff;
				p.Y = p.Y + yDiff;
			}
			
			MainForm.Render();	
		}

		public void Render(DirectBitmap bitmap, Graphics g)
		{
			foreach (var polygon in MainForm.Polygons)
				polygon.Render(bitmap, MainForm.ColorFill, MainForm.BumpMapping);
		}

		public void KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode != Keys.Escape)
				return;

			MainForm.CurrentPolygon = new Polygon();
			MainForm.CurrentState = new IdleState(MainForm);
			MainForm.Render();
		}

		#endregion
	}
}
