using GK1.Structures;
using System.Drawing;
using System.Windows.Forms;

namespace GK1.States
{
	// TODO BUG NA DRUGIM POLYGONIE
	public class DrawingState : IState
	{
		#region Private Properties
		private MainForm MainForm { get; set; }
		private Point Point { get; set; }
		private bool Moving { get; set; }
		#endregion

		public DrawingState(MainForm mainForm)
		{
			MainForm = mainForm;
			MainForm.Cursor = Cursors.Cross;
		}

		#region IState
		public void MouseDown(object sender, MouseEventArgs e)
		{

			if (e.Button != MouseButtons.Left)
				return;
			var point = new Vertex(e.X, e.Y);
			var polygon = MainForm.CurrentPolygon;
			
			Moving = false;

			if (polygon.Vertices.Count != 0)
			{
				// Finish drawing 
				if (polygon.Vertices.First.Value.ComparePoints(point))
				{
					polygon.Segments.AddLast(new Segment(polygon.Vertices.Last.Value, polygon.Vertices.First.Value));
					MainForm.Polygons.Add(polygon);
					MainForm.CurrentPolygon = new Polygon();
					MainForm.CurrentState = new IdleState(MainForm);
				}
				else
				{
					polygon.Segments.AddLast(new Segment(polygon.Vertices.Last.Value, point));
					polygon.Vertices.AddLast(point);
				}
			}
			else
			{
				polygon.Vertices.AddLast(point);
			}
			MainForm.Render();
			
		}

		public void MouseMove(object sender, MouseEventArgs e)
		{
			if (MainForm.CurrentPolygon.Vertices.Count == 0)
				return;

			Moving = true;
			Point = new Point(e.X, e.Y);
			MainForm.Render();
		}

		public void Render(Bitmap bitmap, Graphics g)
		{
			if (Moving)
			{
				Algorithms.Algorithms.Line(MainForm.CurrentPolygon.Vertices.Last.Value.X, MainForm.CurrentPolygon.Vertices.Last.Value.Y, Point.X, Point.Y, bitmap);
			}

			MainForm.CurrentPolygon.Render(bitmap, g);

			foreach (var polygon in MainForm.Polygons)
				polygon.Render(bitmap, g);
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
