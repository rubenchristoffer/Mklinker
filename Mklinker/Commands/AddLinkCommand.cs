using System.Linq;
using CommandLine;
using System.IO.Abstractions;
using Mklinker.Abstractions;

namespace Mklinker.Commands {

	[Verb("addlink", HelpText = "Adds a new link to config file")]
	class AddLinkCommand : GlobalOptions {

		[Value(0, HelpText = "The path to new link file", Required = true)]
		public string targetPath { get; private set; }

		[Value(1, HelpText = "The path to the source file", Required = true)]
		public string sourcePath { get; private set; }

		[Value(2, Default = ConfigLink.LinkType.Default, HelpText = "The type of link you want to create. Default is Symbolic for files and Junction for directories")]
		public ConfigLink.LinkType linkType { get; private set; }

		public AddLinkCommand() : base() {}

		public AddLinkCommand(string targetPath, string sourcePath, ConfigLink.LinkType linkType, string path) : base(path) {
			this.targetPath = targetPath;
			this.sourcePath = sourcePath;
			this.linkType = linkType;
		}

		internal void Execute(IConsole console, IConfigHandler configHandler, IFileSystem fileSystem, IPathFormatter pathFormatter) {
			string formattedSourcePath = pathFormatter.GetFormattedPath(sourcePath);

			if (!fileSystem.File.Exists(formattedSourcePath) && !fileSystem.Directory.Exists(formattedSourcePath)) {
				console.WriteLine("\nThe sourcePath '{0}' is invalid because it does not exist", sourcePath);
				return;
			}

			IConfig config = configHandler.LoadConfig(path);

			if (config.LinkList.Any(link => pathFormatter.GetFormattedPath(link.targetPath).Equals(pathFormatter.GetFormattedPath(targetPath)))) {
				console.WriteLine("\nThe targetPath '{0}' is invalid because it already exists in config file", targetPath);
				return;
			}

			config.LinkList.Add(new ConfigLink(sourcePath, targetPath, linkType));
			configHandler.SaveConfig(config, path);

			console.WriteLine("\nAdded new {0} link to config file: \nTarget: '{1}'\nSource: '{2}'", linkType.ToString(), targetPath, sourcePath);
		}

	}

}
