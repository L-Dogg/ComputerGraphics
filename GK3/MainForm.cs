using System;
using System.Drawing;
using System.Windows.Forms;
using GK3.Processors;
using GK3.Utilities;

namespace GK3
{
	public enum Mode
	{
		RGB = 0,
		YCBr = 1,
		HSV = 2,
		Lab = 3
	}

	public partial class MainForm : Form
	{
		private DirectBitmap mainBitmap;

		public MainForm()
		{
			InitializeComponent();
			mainPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
			pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
			pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
			pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
			separationCombobox.SelectedIndex = 0;
			mainBitmap = DirectBitmap.FromBitmap(new Bitmap("../../Resources/rgb_example.bmp"));
			mainPictureBox.Image = mainBitmap.Bitmap;
		}

		private void pickFileButton_Click(object sender, EventArgs e)
		{
			using (var dlg = new OpenFileDialog())
			{
				dlg.Title = "Open Image";
				dlg.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png, *bmp) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png; *.bmp";
				dlg.InitialDirectory = "../../Resources/";
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					mainBitmap.Dispose();
					mainBitmap = DirectBitmap.FromBitmap(new Bitmap(dlg.FileName));
					mainPictureBox.Image = mainBitmap.Bitmap;
				}
			}
		}

		private void runButton_Click(object sender, EventArgs e)
		{
			switch ((Mode)separationCombobox.SelectedIndex)
			{
				case Mode.RGB:
					new RgbProcessor(mainBitmap).Process(pictureBox2, pictureBox3, pictureBox4);
					this.Invalidate(true);
					break;
				default:
					MessageBox.Show("Not implemented");
					break;
			}
		}
	}
}
