using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK3.Utilities
{
	public class NonInvertibleMatrixException : Exception
	{
		public override string Message => "Cannot create conversion matrix due to zero determinant.";
	}
}
