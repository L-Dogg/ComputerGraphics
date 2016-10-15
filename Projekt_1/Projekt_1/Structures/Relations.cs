using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_1.Structures
{
	public enum RelationType
	{
		/// <summary>
		/// Bez relacji.
		/// </summary>
		None,
		/// <summary>
		/// Relacja pozioma.
		/// </summary>
		Horizontal,
		/// <summary>
		/// Relacja pionowa.
		/// </summary>
		Vertical,
		/// <summary>
		/// Relacja długości.
		/// </summary>
		Length,
	}
}
