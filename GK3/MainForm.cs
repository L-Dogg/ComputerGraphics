using System;
using System.Collections.Generic;
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

	public enum LabPresets
	{
		None = 0,
		adobeRGB = 1,
	}

	public partial class MainForm : Form
	{
		private DirectBitmap mainBitmap;
		private Processor processor;
		private static Dictionary<LabPresets, LabData> presetDictionary = new Dictionary<LabPresets, LabData>();

		private static void PopulateLabData()
		{
			// Iluminant D65
			var aRgb = new LabData(0.6400, 0.3300,
									0.2100, 0.7100,
									0.1500, 0.0600,
									0.31273, 0.32902, 
									2.2);
			presetDictionary.Add(LabPresets.adobeRGB, aRgb);
		}

		public MainForm()
		{
			InitializeComponent();
			PopulateLabData();
			mainPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
			pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
			pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
			pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
			separationCombobox.SelectedIndex = 0;
			labPresetsCombobox.SelectedIndex = 1;
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
			changeLabels();

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
					// TODO: GAMMA
					LabData lab = null;
					if ((LabPresets) labPresetsCombobox.SelectedIndex == LabPresets.None)
						lab = new LabData(double.Parse(Rx.Text), double.Parse(Ry.Text), double.Parse(Gx.Text), double.Parse(Gy.Text),
							double.Parse(Bx.Text), double.Parse(By.Text), double.Parse(Wx.Text), double.Parse(Wy.Text), 2.2); // O TUTEJ GAMMA HARDKODZONA
					else
						lab = presetDictionary[(LabPresets) labPresetsCombobox.SelectedIndex];

					processor.Process(pictureBox2, pictureBox3, pictureBox4, new LabProcessor(lab));
					break;
				default:
					MessageBox.Show("Not implemented", "Sorry");
					break;
			}
		}

		private void changeLabels()
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



		private void changeLabTextboxes(LabPresets preset)
		{
			var data = presetDictionary[preset];
			Rx.Text = data.R.X.ToString();
			Ry.Text = data.R.Y.ToString();
			Gx.Text = data.G.X.ToString();
			Gy.Text = data.G.Y.ToString();
			Bx.Text = data.B.X.ToString();
			By.Text = data.B.Y.ToString();
			Wx.Text = data.W.X.ToString();
			Wy.Text = data.W.Y.ToString();
		}

		private void separationCombobox_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.labGroupbox.Enabled = (Mode) separationCombobox.SelectedIndex == Mode.Lab;
		}

		private void labPresetsCombobox_SelectedIndexChanged(object sender, EventArgs e)
		{
			changeLabTextboxes((LabPresets)labPresetsCombobox.SelectedIndex);
		}
	}
}
