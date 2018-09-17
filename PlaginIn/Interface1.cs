using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaginIn
{
	
		public interface IPlugin
		{
			bool GetBool { get; set; } // нужен и не нужен

			string activate(string input);
	}
		public interface IPluginHost
		{
			bool Register(IPlugin plug);
		}
	
}
