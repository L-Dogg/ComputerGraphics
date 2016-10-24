﻿using GK1.Structures;
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

		public VertexMoveState(MainForm mainForm)
		{
			MainForm = mainForm;
			MainForm.Cursor = Cursors.NoMove2D;
		}

		#region IState
		public void MouseDown(object sender, MouseEventArgs e)
		{
			var point = new Vertex(e.X, e.Y);
			
			if (e.Button == MouseButtons.Left)
			{
				MainForm.CurrentState = new IdleState(MainForm);

				Polygon.Vertices.AddAfter(Polygon.Vertices.Find(Vertex), point);
				Polygon.Vertices.Remove(Vertex);

				var edgeToAddAfter = Polygon.Segments.First((line) => { return line.To == Vertex; });
				var edgeToAddBefore = Polygon.Segments.First((line) => { return line.From == Vertex; });

				edgeToAddAfter.To = point;
				edgeToAddBefore.From = point;

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
			Polygon.Vertices.AddAfter(Polygon.Vertices.Find(Vertex), point);
			Polygon.Vertices.Remove(Vertex);

			var edgeToAddAfter = Polygon.Segments.First((line) => { return line.To == Vertex; });
			var edgeToAddBefore = Polygon.Segments.First((line) => { return line.From == Vertex; });

			edgeToAddAfter.To = point;
			edgeToAddBefore.From = point;

			Vertex = point;

			MainForm.Render();
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
