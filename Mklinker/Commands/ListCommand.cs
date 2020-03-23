using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace Mklinker.Commands {

	[Verb ("list", HelpText = "Lists all the links in the config")]
	public class ListCommand : IDefaultAction {

		public void Execute () {
			Program.config.linkList.ForEach(link => Console.WriteLine("\n" + link.ToString()));
		}

	}

}
