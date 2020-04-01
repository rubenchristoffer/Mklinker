using System;
using System.Linq;
using CommandLine;
using Mklinker.Abstractions;

namespace Mklinker.Commands {

	[Verb("config", HelpText = "Displays information about config file by default, but can create / delete / modify config file")]
	class ConfigCommand : GlobalOptions {

		[Option ('c', "create", HelpText = "Creates a new config file if it does not exist", SetName = "Create")]
		public bool create { get; internal set; }

		[Option('d', "delete", HelpText = "Deleted config file if it exists", SetName = "Delete")]
		public bool delete { get; internal set; }

		internal void Execute(IConsole console, IConfigHandler configHandler, IConfig defaultConfig) {
			if (create) {
				if (configHandler.DoesConfigExist(path)) {
					console.WriteLine("Config file already exists!");
				} else {
					console.WriteLine("Creating config file '{0}'", path);

					configHandler.SaveConfig(defaultConfig, path);
				}
			} else if (delete) {
				if (configHandler.DoesConfigExist(path)) {
					console.WriteLine("Deleting config file '{0}'", path);
					configHandler.DeleteConfig(path);
				} else {
					console.WriteLine("Config file '{0}' does not exist!", path);
				}
			} else {
				if (configHandler.DoesConfigExist(path)) {
					IConfig config = configHandler.LoadConfig(path);

					console.WriteLine();
					console.WriteLine("### File info ###");
					console.WriteLine("Full path: {0}", path);
					console.WriteLine("Version: {0}", config.Version);
					console.WriteLine();
					console.WriteLine("### Link info ###");
					console.WriteLine("Total links: " + config.LinkList.Count);
					console.WriteLine("Junction links: " + config.LinkList.Count(l => l.linkType == ConfigLink.LinkType.Junction));
					console.WriteLine("Symbolic links: " + config.LinkList.Count(l => l.linkType == ConfigLink.LinkType.Symbolic));
					console.WriteLine("Hard links: " + config.LinkList.Count(l => l.linkType == ConfigLink.LinkType.Hard));
				} else {
					console.WriteLine("Config file does not exist. You can create one with the --create option.");
				}
			}
		}

	}

}
