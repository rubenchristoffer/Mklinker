using System;
using System.Linq;
using CommandLine;
using System.IO.Abstractions;
using Mklinker.Abstractions;

namespace Mklinker.Commands {

	[Verb("config", HelpText = "Displays information about config file by default, but can create / delete / modify config file")]
	class ConfigCommand : GlobalOptions {

		[Option ('c', "create", HelpText = "Creates a new config file if it does not exist", SetName = "Create")]
		public bool create { get; private set; }

		[Option('d', "delete", HelpText = "Deleted config file if it exists", SetName = "Delete")]
		public bool delete { get; private set; }

		internal void Execute(IConfigHandler configHandler, IFileSystem fileSystem, IConfig defaultConfig) {
			if (create) {
				if (fileSystem.File.Exists(path)) {
					Console.WriteLine("Config file already exists!");
				} else {
					Console.WriteLine("Creating config file '{0}'", path);

					configHandler.SaveConfig(defaultConfig, path);
				}
			} else if (delete) {
				if (fileSystem.File.Exists(path)) {
					Console.WriteLine("Deleting config file '{0}'", path);
					fileSystem.File.Delete(path);
				} else {
					Console.WriteLine("Config file '{0}' does not exist!", path);
				}
			} else {
				if (fileSystem.File.Exists(path)) {
					IConfig config = configHandler.LoadConfig(path);

					Console.WriteLine();
					Console.WriteLine("### File info ###");
					Console.WriteLine("Full path: {0}", path);
					Console.WriteLine("Version: {0}", config.Version);
					Console.WriteLine();
					Console.WriteLine("### Link info ###");
					Console.WriteLine("Total links: " + config.LinkList.Count);
					Console.WriteLine("Junction links: " + config.LinkList.Count(l => l.linkType == ConfigLink.LinkType.Junction));
					Console.WriteLine("Symbolic links: " + config.LinkList.Count(l => l.linkType == ConfigLink.LinkType.Symbolic));
					Console.WriteLine("Hard links: " + config.LinkList.Count(l => l.linkType == ConfigLink.LinkType.Hard));
				} else {
					Console.WriteLine("Config file does not exist. You can create one with the --create option.");
				}
			}
		}

	}

}
