using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project1._1
{
	struct Cell
	{
		internal Cell(string token, string type, int nomer)
		{
			Token = token;
			Type = type;
			Nomer = nomer;
		}

		internal string Token { get; set; }
		internal string Type { get; set; }
		internal int Nomer { get; set; }
	}
}
