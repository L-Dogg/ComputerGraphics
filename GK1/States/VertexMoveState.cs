using GK1.Structures;
using System.Drawing;
using System.Windows.Forms;

namespace GK1.States
{
	public class VertexMoveState : IState
	{
		#region Private Properties
		private MainForm MainForm { get; }
		private Vertex Vertex { get; }
		private Polygon Polygon { get; }
		#endregion

		public VertexMoveState(MainForm mainForm, Polygon polygon, Vertex vertex)
		{
			MainForm = mainForm;
			MainForm.Cursor = Cursors.NoMove2D;
			Polygon = polygon;
			Vertex = vertex;

			Polygon.SaveVertices();
		}

		#region IState
		public void MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left)
				return;

			Vertex.X = e.X;
			Vertex.Y = e.Y;

			Polygon.Apply();

			MainForm.CurrentState = new IdleState(MainForm);
			MainForm.Render();
		}

		public void MouseMove(object sender, MouseEventArgs e)
		{
			if (Polygon == null)
			{
				MainForm.CurrentState = new IdleState(MainForm);
				MainForm.Render();
				return;
			}
			
			Vertex.Previous.Push(new Point(Vertex.X, Vertex.Y));
			Vertex.X = e.X;
			Vertex.Y = e.Y;

			if (!Polygon.Apply())
			{
				Polygon.LoadVertices();
			}
			else
			{
				MainForm.Render();
			}
		}

		public void Render(Bitmap bitmap, Graphics g)
		{
			foreach (var polygon in MainForm.Polygons)
				polygon.Render(bitmap, g);
		}

		public void KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				if (!Polygon.Apply())
				{
					Polygon.LoadVertices();
				}
				MainForm.CurrentState = new IdleState(MainForm);
				MainForm.Render();
			}
		}
		#endregion
	}
}
