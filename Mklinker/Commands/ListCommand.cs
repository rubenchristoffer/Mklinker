using System;
using System.IO.Abstractions;
using CommandLine;
using Mklinker.Abstractions;

namespace Mklinker.Commands {

	[Verb ("list", HelpText = "Lists all the links in the config")]
	class ListCommand : GlobalOptions, IDefaultCommandHandler {

		public ListCommand() : base() {}
		public ListCommand(string path) : base(path) {}

		void IDefaultCommandHandler.Execute(IConsole console, IConfigHandler configHandler, IFileSystem fileSystem) {
			IConfig config = configHandler.LoadConfig(path);
			config.LinkList.ForEach(link => console.WriteLine("\n" + link.ToString()));

			if (config.LinkList.Count == 0)
				console.WriteLine("Config is empty");
		}

	}

}
