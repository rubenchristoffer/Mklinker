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

		internal void Execute(IConfigHandler configHandler, IConfig defaultConfig) {
			if (create) {
				if (configHandler.DoesConfigExist(path)) {
					Console.WriteLine("Config file already exists!");
				} else {
					Console.WriteLine("Creating config file '{0}'", path);

					configHandler.SaveConfig(defaultConfig, path);
				}
			} else if (delete) {
				if (configHandler.DoesConfigExist(path)) {
					Console.WriteLine("Deleting config file '{0}'", path);
					configHandler.DeleteConfig(path);
				} else {
					Console.WriteLine("Config file '{0}' does not exist!", path);
				}
			} else {
				if (configHandler.DoesConfigExist(path)) {
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
