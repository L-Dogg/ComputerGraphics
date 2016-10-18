using GK1.Structures;
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
		private Polygon Polygon { get; set; }
		private Point Vertex { get; set; }
		private bool Moving { get; set; }
		#endregion

		public VertexMoveState(MainForm mainForm)
		{
			MainForm = mainForm;
			MainForm.Cursor = Cursors.NoMove2D;
		}

		#region IState
		public void MouseDown(object sender, MouseEventArgs e)
		{
			
		}

		public void MouseMove(object sender, MouseEventArgs e)
		{
			throw new NotImplementedException();
		}

		public void Render(Bitmap bitmap, Graphics g)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}
