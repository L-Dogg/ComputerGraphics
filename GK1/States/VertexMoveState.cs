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
	class VertexMoveState : IState
	{
		#region Private Properties
		private MainForm MainForm { get; set; }
		#endregion

		#region Public Properties
		public Vertex Vertex { get; set; }
		public Polygon Polygon { get; set; }
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
			var point = new Vertex(e.X, e.Y);
			
			if (e.Button == MouseButtons.Left)
			{
				Vertex.X = e.X;
				Vertex.Y = e.Y;

				Polygon.Apply();

				MainForm.CurrentState = new IdleState(MainForm);
				MainForm.Render();
			}
		}

		public void MouseMove(object sender, MouseEventArgs e)
		{
			if (Polygon == null)
			{
				MainForm.CurrentState = new IdleState(MainForm);
				MainForm.Render();
            }

			var point = new Vertex(e.X, e.Y);
			
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
			
		}
		#endregion
	}
}
