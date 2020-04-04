﻿using System;
using System.Linq;
using System.IO.Abstractions;
using CommandLine;
using Mklinker.Abstractions;

namespace Mklinker.Commands {

	[Verb("removelink", HelpText = "Removes link from config")]
	class RemoveLinkCommand : GlobalOptions, IDefaultCommandHandler {

		[Value(0, HelpText = "The targetPath matching entry you want to delete from config.", MetaName = "targetPath", Required = true)]
		public string targetPath { get; private set; }

		public RemoveLinkCommand() : base() { }

		public RemoveLinkCommand(string targetPath, string path) : base(path) {
			this.targetPath = targetPath;
		}

		void IDefaultCommandHandler.Execute(IConsole console, IConfigHandler configHandler, IFileSystem fileSystem) {
			IConfig config = configHandler.LoadConfig(path);
			ConfigLink configLink = config.LinkList.FirstOrDefault(link => fileSystem.Path.GetFullPath(link.targetPath).Equals(fileSystem.Path.GetFullPath(targetPath)));

			if (configLink != null) {
				config.LinkList.Remove(configLink);
				console.WriteLine("\nSuccessfully removed link with targetPath '{0}'", targetPath);

				configHandler.SaveConfig(config, path);
			} else {
				console.WriteLine(String.Format("\nThe targetPath '{0}' is invalid because it does not exist in config", targetPath));
			}
		}

	}
}
