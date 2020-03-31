using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace Mklinker.Commands {

	[Verb ("list", HelpText = "Lists all the links in the config")]
	public class ListCommand : GlobalOptions, IDefaultAction {

		void IDefaultAction.Execute(IConfigHandler configHandler, IFileSystem fileSystem) {
			IConfig config = configHandler.LoadConfig(path);
			config.LinkList.ForEach(link => Console.WriteLine("\n" + link.ToString()));

			if (config.LinkList.Count == 0)
				Console.WriteLine("Config is empty");
		}

	}

}
