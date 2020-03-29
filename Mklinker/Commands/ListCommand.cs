using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace Mklinker.Commands {

	[Verb ("list", HelpText = "Lists all the links in the config")]
	public class ListCommand : GlobalOptions, IDefaultAction {

		public void Execute () {
			Program.LoadConfig(path);
			Program.config.linkList.ForEach(link => Console.WriteLine("\n" + link.ToString()));

			if (Program.config.linkList.Count == 0)
				Console.WriteLine("Config is empty");
		}

	}

}
