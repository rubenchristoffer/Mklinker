using System;
using System.IO.Abstractions;
using CommandLine;
using Mklinker.Abstractions;

namespace Mklinker.Commands {

	[Verb ("list", HelpText = "Lists all the links in the config")]
	class ListCommand : GlobalOptions, IDefaultCommandHandler {

		[Option('v', "variables", HelpText = "Will display variables instead", Required = false, Default = false)]
		public bool displayVariables { get; private set; }

		public ListCommand() : base() {}

		public ListCommand(bool displayVariables, string path) : base(path) {
			this.displayVariables = displayVariables;
		}

		void IDefaultCommandHandler.Execute(IConsole console, IConfigHandler configHandler, IFileSystem fileSystem) {
			IConfig config = configHandler.LoadConfig(path);

			if (!displayVariables) {
				config.LinkList.ForEach(link => console.WriteLine("\n" + link.ToString()));
			} else {
				config.Variables.ForEach(variable => console.WriteLine("\n" + variable.ToString()));
			}

			if (config.LinkList.Count == 0)
				console.WriteLine("Config is empty");
		}

	}

}
