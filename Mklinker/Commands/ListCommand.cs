using System;
using System.IO.Abstractions;
using CommandLine;
using Mklinker.Abstractions;

namespace Mklinker.Commands {

	[Verb ("list", HelpText = "Lists all the links in the config")]
	class ListCommand : GlobalOptions, IDefaultAction {

		void IDefaultAction.Execute(IConfigHandler configHandler, IFileSystem fileSystem) {
			IConfig config = configHandler.LoadConfig(path);
			config.LinkList.ForEach(link => Console.WriteLine("\n" + link.ToString()));

			if (config.LinkList.Count == 0)
				Console.WriteLine("Config is empty");
		}

	}

}
