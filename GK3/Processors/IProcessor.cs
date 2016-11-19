using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GK3.Processors
{
	public interface IProcessor
	{
		void Process(PictureBox a, PictureBox b, PictureBox c);
		void BindToGui(PictureBox a, PictureBox b, PictureBox c);
	}
}
