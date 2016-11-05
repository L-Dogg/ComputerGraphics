using System;
using System.Windows.Forms;

namespace GK1.Controls
{
	public partial class Length : Form
	{
		public int LengthTyped { get; private set; }
		public bool WasOk { get; private set; }
		private Length()
		{
			InitializeComponent();
			this.ControlBox = false;
		}

		public Length(int length) : this()
		{
			LengthTyped = length;
			this.lengthTextBox.Text = length.ToString();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			int val;
			if (int.TryParse(lengthTextBox.Text, out val))
				LengthTyped = val;
			WasOk = true;
			this.Close();
		}

		private void lengthTextBox_TextChanged(object sender, EventArgs e)
		{
			int val;
			if (!int.TryParse(lengthTextBox.Text, out val) || val <= 0)
				okButton.Enabled = false;
			else
				okButton.Enabled = true;
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			WasOk = false;
			this.Close();
		}
	}
}
