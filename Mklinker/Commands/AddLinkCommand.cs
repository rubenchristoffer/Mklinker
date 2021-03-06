﻿using System.Linq;
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

		[Option('f', "force", Default = false, HelpText = "If this flag is set it will ignore validation checks and add it no matter what", Required = false)]
		public bool force { get; private set; }

		public AddLinkCommand() : base() {}

		public AddLinkCommand(string sourcePath, string targetPath, ConfigLink.LinkType linkType, string path) : base(path) {
			this.targetPath = targetPath;
			this.sourcePath = sourcePath;
			this.linkType = linkType;
		}

		internal void Execute(IConsole console, IConfigHandler configHandler, IFileSystem fileSystem, IPathResolver pathResolver) {
			if (!configHandler.DoesConfigExist(path)) {
				console.WriteLine($"Config '{ path }' does not exist. Type 'help config' in order to see how you create a config file.", IConsole.ContentType.Negative);
				return;
			}

			IConfig config = configHandler.LoadConfig(path);

			// Force forward slash
			sourcePath = sourcePath.Replace ('\\', '/');
			targetPath = targetPath.Replace ('\\', '/');

			string formattedSourcePath = pathResolver.GetAbsoluteResolvedPath(sourcePath, config.Variables);
			string formattedTargetPath = pathResolver.GetAbsoluteResolvedPath(targetPath, config.Variables);

			if (!force && !fileSystem.File.Exists(formattedSourcePath) && !fileSystem.Directory.Exists(formattedSourcePath)) {
				console.WriteLine($"\nThe sourcePath '{sourcePath}' is invalid because it does not exist", IConsole.ContentType.Negative);
				return;
			}

			if (!force && config.LinkList.Any(link => pathResolver.GetAbsoluteResolvedPath(link.targetPath, config.Variables).Equals(formattedTargetPath))) {
				console.WriteLine($"\nThe targetPath '{targetPath}' is invalid because it already exists in config file", IConsole.ContentType.Negative);
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
