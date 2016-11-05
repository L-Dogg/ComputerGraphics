using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GK2.Structures;
using GK2.Utilities;

namespace GK2.States
{
	public class PolygonUnionState : IState
	{
		private static readonly Color MarkedColor = Color.Red;
		private List<Polygon> Polygons { get; set; } = new List<Polygon>(2);
		private MainForm MainForm { get; set; }

		public PolygonUnionState(MainForm mainForm)
		{
			MainForm = mainForm;
		}

		// TODO:
		private void Union()
		{

		}

		#region IState
		public void MouseDown(object sender, MouseEventArgs e)
		{
			Polygon polygon;
			var point = new Vertex(e.X, e.Y);
			
			if (!ClickChecker.WasPolygonClicked(point, MainForm.Polygons, out polygon))
				return;

			Polygons.Add(polygon);
			MainForm.Render();
			if (Polygons.Count != 2)
				return;

			Union();
			MainForm.Render();
			MainForm.CurrentState = new IdleState(MainForm);
		}

		public void MouseMove(object sender, MouseEventArgs e)
		{
			
		}

		public void KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode != Keys.Escape)
				return;

			MainForm.CurrentPolygon = new Polygon();
			MainForm.CurrentState = new IdleState(MainForm);
			MainForm.Render();
		}

		public void Render(DirectBitmap bitmap, Graphics g)
		{
			foreach (var polygon in MainForm.Polygons)
				if (!Polygons.Contains(polygon))
					polygon.Render(bitmap, MainForm.ColorFill, MainForm.BumpMapping);

			foreach (var polygon in this.Polygons)
				polygon.Render(bitmap, MarkedColor, MainForm.ColorFill, MainForm.BumpMapping);
		}
		#endregion
	}
}
