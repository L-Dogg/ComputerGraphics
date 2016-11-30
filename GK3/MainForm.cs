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
		appleRGB = 2,
		sRGB = 3,
		PAL_SECAM = 4,
		NTSC = 5,
		cieRGB = 6,
	}

	public enum IluminantPresets
	{
		C = 0,
		D50 ,
		D65,
		E,
	}

	public partial class MainForm : Form
	{
		private DirectBitmap mainBitmap;
		private Processor processor;
		private static Dictionary<LabPresets, LabData> presetDictionary = new Dictionary<LabPresets, LabData>();
		private static Dictionary<IluminantPresets, IluminantData> iluminantDictionary = new Dictionary<IluminantPresets, IluminantData>();

		private bool CoordsChanged = false;

		// Source:
		// http://www.brucelindbloom.com/index.html?WorkingSpaceInfo.html
		private static void PopulateLabData()
		{
			#region Lab presets
			var adobeRgb = new LabData(0.6400, 0.3300,
									0.2100, 0.7100,
									0.1500, 0.0600,
									0.31273, 0.32902, 
									2.2,
									IluminantPresets.D65);
			presetDictionary.Add(LabPresets.adobeRGB, adobeRgb);
			
			var appleRgb = new LabData(0.6250, 0.3400,
									0.2800, 0.5950,
									0.1550, 0.0700,
									0.31273, 0.32902,
									1.8,
									IluminantPresets.D65);
			presetDictionary.Add(LabPresets.appleRGB, appleRgb);

			var sRGB = new LabData(0.6400, 0.3300,
								0.3000, 0.6000,
								0.1500, 0.0600,
								0.31273, 0.32902,
								2.2,
								IluminantPresets.D65);

			presetDictionary.Add(LabPresets.sRGB, sRGB);

			var palSecamRGB = new LabData(0.6400, 0.3300,
										0.2900, 0.6000,
										0.1500, 0.0600,
										0.31273, 0.32902,
										2.2,
										IluminantPresets.D65);
			presetDictionary.Add(LabPresets.PAL_SECAM, palSecamRGB);

			var ntscRGB = new LabData(0.6700, 0.3300,
									0.2100, 0.7100,
									0.1400, 0.0800,
									0.31006, 0.31615,
                                    2.2,
									IluminantPresets.C);
			presetDictionary.Add(LabPresets.NTSC, ntscRGB);

			var cieRGB = new LabData(0.7350, 0.2650,
									0.2740, 0.7170, 
									0.1670, 0.0090,
									0.33333, 0.33333,
									2.2,
									IluminantPresets.E);
			presetDictionary.Add(LabPresets.cieRGB, cieRGB);
			#endregion

			#region Iluminant presets

			iluminantDictionary.Add(IluminantPresets.C, new IluminantData(0.31006, 0.31615));
			iluminantDictionary.Add(IluminantPresets.D50, new IluminantData(0.34567, 0.35850));
			iluminantDictionary.Add(IluminantPresets.D65, new IluminantData(0.31273, 0.32902));
			iluminantDictionary.Add(IluminantPresets.E, new IluminantData(0.33333, 0.33333));

			#endregion
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

			this.Rx.TextChanged += Rx_TextChanged;
			this.Ry.TextChanged += Rx_TextChanged;
			this.Gx.TextChanged += Rx_TextChanged;
			this.Gy.TextChanged += Rx_TextChanged;
			this.Bx.TextChanged += Rx_TextChanged;
			this.By.TextChanged += Rx_TextChanged;
			this.Wx.TextChanged += Rx_TextChanged;
			this.Wy.TextChanged += Rx_TextChanged;
		}

		private void Rx_TextChanged(object sender, EventArgs e)
		{
			CoordsChanged = true;
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
					LabData lab = null;
					if (CoordsChanged || (LabPresets) labPresetsCombobox.SelectedIndex == LabPresets.None)
						lab = new LabData(double.Parse(Rx.Text), double.Parse(Ry.Text), double.Parse(Gx.Text), double.Parse(Gy.Text),
							double.Parse(Bx.Text), double.Parse(By.Text), double.Parse(Wx.Text), double.Parse(Wy.Text), double.Parse(gammaTexbox.Text), null);
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
			gammaTexbox.Text = data.Gamma.ToString();
			this.whitePointPresetCombobox.SelectedIndex = (int) data.Iluminant.Value;
		}

		private void changeWhiteTextboxes(IluminantPresets preset)
		{
			var data = iluminantDictionary[preset];
			Wx.Text = data.X.ToString();
			Wy.Text = data.Y.ToString();
		}

		private void separationCombobox_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.labGroupbox.Enabled = (Mode) separationCombobox.SelectedIndex == Mode.Lab;
		}

		private void labPresetsCombobox_SelectedIndexChanged(object sender, EventArgs e)
		{
			CoordsChanged = false;
			var idx = (LabPresets)labPresetsCombobox.SelectedIndex;
			if (idx != LabPresets.None)
				changeLabTextboxes(idx);
		}

		private void whitePointPresetCombobox_SelectedIndexChanged(object sender, EventArgs e)
		{
			changeWhiteTextboxes((IluminantPresets) whitePointPresetCombobox.SelectedIndex);
		}
	}
}
