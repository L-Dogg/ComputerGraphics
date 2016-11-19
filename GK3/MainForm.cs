using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GK3
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
			mainPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
			pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
			pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
			pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
			separationCombobox.SelectedIndex = 0;
		}

		private void pickFileButton_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog dlg = new OpenFileDialog())
			{
				dlg.Title = "Open Image";
				dlg.Filter = "bmp files (*.bmp)|*.bmp";

				if (dlg.ShowDialog() == DialogResult.OK)
				{
					mainPictureBox.Image = new Bitmap(dlg.FileName);
				}
			}
		}
	}
}
