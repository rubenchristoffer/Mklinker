using System;
using System.IO.Abstractions;
using CommandLine;
using Mklinker.Abstractions;

namespace Mklinker.Commands {

	[Verb ("list", HelpText = "Lists all the links or variables in the config")]
	class ListCommand : GlobalOptions {

		[Option('v', "variables", HelpText = "Will display variables instead", Required = false, Default = false)]
		public bool displayVariables { get; private set; }

		[Option('a', "absolute", HelpText = "Will display absolute paths with variables resolved", Required = false, Default = false)]
		public bool displayAbsolutePaths { get; private set; }

		public ListCommand() : base() {}

		public ListCommand(bool displayVariables, string path) : base(path) {
			this.displayVariables = displayVariables;
		}

		internal void Execute(IConsole console, IConfigHandler configHandler, IFileSystem fileSystem, IPathResolver pathResolver) {
			IConfig config = configHandler.LoadConfig(path);

			if (!displayVariables) {
				foreach (ConfigLink configLink in config.LinkList) {
					string absoluteTargetPathString = "";
					string absoluteSourcePathString = "";

					if (displayAbsolutePaths) {
						absoluteTargetPathString = $"\n\t\t => { pathResolver.GetAbsoluteResolvedPath(configLink.targetPath, config.Variables) }";
						absoluteSourcePathString = $"\n\t\t => { pathResolver.GetAbsoluteResolvedPath(configLink.sourcePath, config.Variables) }";
					}

					console.WriteLine($"\n" +
						$"{ configLink.linkType.ToString() } link:\n" +
						$"\t- Target: { configLink.targetPath }{ absoluteTargetPathString }\n" +
						$"\t- Source: { configLink.sourcePath }{ absoluteSourcePathString }\n");
				}
			} else {
				foreach (Variable variable in config.Variables) {
					console.WriteLine($"\n { variable.ToString() }");
				}
			}

			if (config.LinkList.Count == 0)
				console.WriteLine("Config is empty");
		}

	}

}
