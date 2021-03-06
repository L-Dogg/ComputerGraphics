﻿using GK2.Structures;
using System.Drawing;
using System.Windows.Forms;
using GK2.Utilities;

namespace GK2.States
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
			Polygon.Finished = true;
			Vertex = vertex;
		}

		#region IState
		public void MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left)
				return;

			Vertex.X = e.X;
			Vertex.Y = e.Y;
			Polygon.SetMovedXmin(Vertex);

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
			
			Vertex.X = e.X;
			Vertex.Y = e.Y;
			Polygon.SetMovedXmin(Vertex);

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
			MainForm.CurrentState = new IdleState(MainForm);
			MainForm.Render();
		}
		#endregion
	}
}
