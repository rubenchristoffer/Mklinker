using System.Linq;
using CommandLine;
using System.IO.Abstractions;
using Mklinker.Abstractions;

namespace Mklinker.Commands {

	[Verb("addlink", HelpText = "Adds a new link to config file")]
	class AddLinkCommand : GlobalOptions {

		[Value(0, HelpText = "The path to the source file", Required = true)]
		public string sourcePath { get; private set; }

		[Value(1, HelpText = "The path to new link file", Required = true)]
		public string targetPath { get; private set; }

		[Value(2, Default = ConfigLink.LinkType.Default, HelpText = "The type of link you want to create. Default is Symbolic for files and Junction for directories")]
		public ConfigLink.LinkType linkType { get; private set; }

		public AddLinkCommand() : base() {}

		public AddLinkCommand(string sourcePath, string targetPath, ConfigLink.LinkType linkType, string path) : base(path) {
			this.targetPath = targetPath;
			this.sourcePath = sourcePath;
			this.linkType = linkType;
		}

		internal void Execute(IConsole console, IConfigHandler configHandler, IFileSystem fileSystem, IPathResolver pathResolver) {
			IConfig config = configHandler.LoadConfig(path);
			string formattedSourcePath = pathResolver.GetAbsoluteResolvedPath(sourcePath, config.Variables);
			string formattedTargetPath = pathResolver.GetAbsoluteResolvedPath(targetPath, config.Variables);

			if (!fileSystem.File.Exists(formattedSourcePath) && !fileSystem.Directory.Exists(formattedSourcePath)) {
				console.WriteLine("\nThe sourcePath '{0}' is invalid because it does not exist", sourcePath);
				return;
			}

			if (config.LinkList.Any(link => pathResolver.GetAbsoluteResolvedPath(link.targetPath, config.Variables).Equals(formattedTargetPath))) {
				console.WriteLine("\nThe targetPath '{0}' is invalid because it already exists in config file", targetPath);
				return;
			}

			config.LinkList.Add(new ConfigLink(sourcePath, targetPath, linkType));
			configHandler.SaveConfig(config, path);

			console.WriteLine($"\nAdded new { linkType.ToString() } link to config file: \n" +
				$"Source: '{ sourcePath }'\n" +
				$"Target: '{ targetPath }'");
		}

	}

}
