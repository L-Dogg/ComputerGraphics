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
		private Processor processor;

		public MainForm()
		{
			InitializeComponent();
			mainPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
			pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
			pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
			pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
			separationCombobox.SelectedIndex = 0;
			mainBitmap = DirectBitmap.FromBitmap(new Bitmap("../../Resources/barn.bmp"));
			mainPictureBox.Image = mainBitmap.Bitmap;
			processor = new Processor(mainBitmap);
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
					processor = new Processor(mainBitmap);
				}
			}
		}

		private void runButton_Click(object sender, EventArgs e)
		{
			switch ((Mode)separationCombobox.SelectedIndex)
			{
				case Mode.RGB:
					processor.Process(pictureBox2, pictureBox3, pictureBox4, new RgbProcessor());
					break;
				case Mode.YCBr:
					processor.Process(pictureBox2, pictureBox3, pictureBox4, new YCbCrProcessor());
					break;
				case Mode.HSV:
					processor.Process(pictureBox2, pictureBox3, pictureBox4, new HSVProcessor());
					break;
				case Mode.Lab:
					//processor.Process(pictureBox2, pictureBox3, pictureBox4, new LabProcessor());
					MessageBox.Show("Not implemented", "Sorry");
					break;
				default:
					MessageBox.Show("Not implemented", "Sorry");
					break;
			}
		}

		private void separationCombobox_SelectedIndexChanged(object sender, EventArgs e)
		{
			switch ((Mode)separationCombobox.SelectedIndex)
			{
				case Mode.RGB:
					firstChannelLabel.Text = "R";
					secondChannelLabel.Text = "G";
					thirdChannelLabel.Text = "B";
					break;
				case Mode.YCBr:
					firstChannelLabel.Text = "Y";
					secondChannelLabel.Text = "Cb";
					thirdChannelLabel.Text = "Cr";
					break;
				case Mode.HSV:
					firstChannelLabel.Text = "H";
					secondChannelLabel.Text = "S";
					thirdChannelLabel.Text = "V";
					break;
				case Mode.Lab:
					firstChannelLabel.Text = "L";
					secondChannelLabel.Text = "a";
					thirdChannelLabel.Text = "b";
					break;
			}
		}
	}
}
