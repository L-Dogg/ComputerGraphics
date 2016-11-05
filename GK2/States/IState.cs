using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GK2.Utilities;

namespace GK2.States
{
	/// <summary>
	/// State interface.
	/// </summary>
	public interface IState
	{
		void MouseDown(object sender, MouseEventArgs e);

		void MouseMove(object sender, MouseEventArgs e);

		void KeyUp(object sender, KeyEventArgs e);

		/// <summary>
		/// Draws polygons on bitmap. Draws relation markers using graphics.
		/// </summary>
		/// <param name="bitmap">Bitmap to draw on.</param>
		/// <param name="g">Graphics for relation drawing.</param>
		void Render(DirectBitmap bitmap, Graphics g);
	}
}
