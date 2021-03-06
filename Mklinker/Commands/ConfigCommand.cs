﻿using System;
using System.Linq;
using CommandLine;
using Mklinker.Abstractions;

namespace Mklinker.Commands {

	[Verb("config", HelpText = "Displays information about config file by default, but can create / delete / modify config file")]
	class ConfigCommand : GlobalOptions {

		[Option ('c', "create", HelpText = "Creates a new config file if it does not exist", SetName = "Create")]
		public bool create { get; private set; }

		[Option('d', "delete", HelpText = "Deleted config file if it exists", SetName = "Delete")]
		public bool delete { get; private set; }

		public ConfigCommand() : base() {}

		public ConfigCommand (bool create, bool delete, string path) : base(path) {
			this.create = create;
			this.delete = delete;
		}

		internal void Execute(IConsole console, IConfigHandler configHandler, IConfig defaultConfig, IPathResolver pathResolver) {
			if (create) {
				if (configHandler.DoesConfigExist(path)) {
					console.WriteLine("Config already exists!", IConsole.ContentType.Negative);
				} else {
					console.WriteLine("Creating config '{0}'", path);

					configHandler.SaveConfig(defaultConfig, path);
				}
			} else if (delete) {
				if (configHandler.DoesConfigExist(path)) {
					console.WriteLine("Deleting config '{0}'", path);
					configHandler.DeleteConfig(path);
				} else {
					console.WriteLine($"Config '{path}' does not exist!", IConsole.ContentType.Negative);
				}
			} else {
				if (configHandler.DoesConfigExist(path)) {
					IConfig config = configHandler.LoadConfig(path);

					console.WriteLine();
					console.WriteLine("### Metadata info ###", IConsole.ContentType.Header);
					console.WriteLine($"Path: {path}");
					console.WriteLine($"Full path: {pathResolver.GetAbsoluteResolvedPath(path, config.Variables)}");
					console.WriteLine($"Version: {config.Version}");
					console.WriteLine();

					console.WriteLine("### Variable info ###", IConsole.ContentType.Header);
					console.WriteLine("Total variables: " + config.Variables.Count);
					console.WriteLine();

					console.WriteLine("### Link info ###", IConsole.ContentType.Header);
					console.WriteLine("Total links: " + config.LinkList.Count);
					console.WriteLine("Junction links: " + config.LinkList.Count(l => l.linkType == ConfigLink.LinkType.Junction));
					console.WriteLine("Symbolic links: " + config.LinkList.Count(l => l.linkType == ConfigLink.LinkType.Symbolic));
					console.WriteLine("Hard links: " + config.LinkList.Count(l => l.linkType == ConfigLink.LinkType.Hard));
				} else {
					console.WriteLine("Config does not exist. You can create one with the --create option.", IConsole.ContentType.Negative);
				}
			}
		}

	}

}
