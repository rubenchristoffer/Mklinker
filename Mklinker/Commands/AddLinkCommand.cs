using System.Linq;
using CommandLine;
using System.IO.Abstractions;
using Mklinker.Abstractions;

namespace Mklinker.Commands {

	[Verb("addlink", HelpText = "Adds a new link to config file")]
	class AddLinkCommand : GlobalOptions, IDefaultCommandHandler {

		[Value(0, HelpText = "The path to new link file", Required = true)]
		public string targetPath { get; private set; }

		[Value(1, HelpText = "The path to the source file", Required = true)]
		public string sourcePath { get; private set; }

		[Value(2, Default = ConfigLink.LinkType.Default, HelpText = "The type of link you want to create. Default is Symbolic for files and Junction for directories")]
		public ConfigLink.LinkType linkType { get; private set; }

		public AddLinkCommand() {}

		public AddLinkCommand(string targetPath, string sourcePath, ConfigLink.LinkType linkType, string path) : base(path) {
			this.targetPath = targetPath;
			this.sourcePath = sourcePath;
			this.linkType = linkType;
		}

		void IDefaultCommandHandler.Execute(IConsole console, IConfigHandler configHandler, IFileSystem fileSystem) {
			if (!fileSystem.File.Exists(sourcePath) && !fileSystem.Directory.Exists(sourcePath)) {
				console.WriteLine("\nThe sourcePath '{0}' is invalid because it does not exist", sourcePath);
				return;
			}

			IConfig config = configHandler.LoadConfig(path);

			if (config.LinkList.Any(link => link.targetPath.Equals(targetPath))) {
				console.WriteLine("\nThe targetPath '{0}' is invalid because it already exists in config file", targetPath);
				return;
			}

			// Set default link type if type is not provided
			if (linkType == ConfigLink.LinkType.Default) {
				if (fileSystem.File.Exists(sourcePath)) {
					linkType = ConfigLink.LinkType.Symbolic;
				} else {
					linkType = ConfigLink.LinkType.Junction;
				}
			}

			config.LinkList.Add(new ConfigLink(sourcePath, targetPath, linkType));
			configHandler.SaveConfig(config, path);

			console.WriteLine("\nAdded new {0} link to config file: \nTarget: '{1}'\nSource: '{2}'", linkType.ToString(), targetPath, sourcePath);
		}

	}

}
