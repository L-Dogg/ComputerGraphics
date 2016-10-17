using GK1.States;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GK1
{
	/// <summary>
	/// Main window form.
	/// </summary>
	public partial class MainForm : Form
	{
		/// <summary>
		/// Current control state.
		/// </summary>
		public IState currentState { get; set; }
		public MainForm()
		{
			InitializeComponent();
		}
	}
}
