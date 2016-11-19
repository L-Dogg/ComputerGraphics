using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GK3.Utilities;

namespace GK3.Processors
{
	public interface IProcessor
	{
		void Process(int a, int r, int g, int b, DirectBitmap bmp1, DirectBitmap bmp2, DirectBitmap bmp3, int x, int y);
	}
}
