using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GK1.States
{
	/// <summary>
	/// State interface.
	/// </summary>
	public interface IState
	{
		void MouseDown(object sender, MouseEventArgs e);

		void MouseMove(object sender, MouseEventArgs e);

		void Render();
	}
}
