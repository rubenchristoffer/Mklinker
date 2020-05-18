using System;
using System.IO.Abstractions;
using System.Diagnostics;
using CommandLine;
using Mklinker.Abstractions;
using System.Text;

namespace Mklinker.Commands {

	[Verb ("linkall", HelpText = "Generates all links from config")]
	class LinkAllCommand : GlobalOptions {

		public LinkAllCommand() : base () {}
		public LinkAllCommand (string path) : base (path) {}

		internal void Execute (IConsole console, IConfigHandler configHandler, IFileSystem fileSystem, ILinker linker, IPathResolver pathResolver) {
			if (!configHandler.DoesConfigExist (path)) {
				console.WriteLine ($"Config '{ path }' does not exist. Type 'help config' in order to see how you create a config file.");
				return;
			}

			IConfig config = configHandler.LoadConfig (path);

			console.WriteLine ("\nCreating links based on config...");

			int successes = 0;

			foreach (ConfigLink configLink in config.LinkList) {
				string resolvedSourcePath = pathResolver.GetAbsoluteResolvedPath (configLink.sourcePath, config.Variables);
				string resolvedTargetPath = pathResolver.GetAbsoluteResolvedPath (configLink.targetPath, config.Variables);

				CreateSubDirectories (console, fileSystem, pathResolver, config, resolvedTargetPath);

				if (linker.CreateLink (resolvedSourcePath, resolvedTargetPath, configLink.linkType))
					successes++;
			}

			console.WriteLine ("\n### Finished! Created {0} / {1} links ###", successes, config.LinkList.Count);
		}

		internal void CreateSubDirectories (IConsole console, IFileSystem fileSystem, IPathResolver pathResolver, IConfig config, string resolvedTargetPath) {
			// Create sub-dirs if they do not exist
			string path = fileSystem.Path.GetDirectoryName (resolvedTargetPath);

			if (path.Trim().Length != 0 && !fileSystem.Directory.Exists(path)) {
				fileSystem.Directory.CreateDirectory (path);
			}
		}

	}

}
