using System;
using System.IO.Abstractions;
using System.Diagnostics;
using CommandLine;
using Mklinker.Abstractions;
using LinkType = Mklinker.ConfigLink.LinkType;

namespace Mklinker.Commands {

	[Verb ("linkall", HelpText = "Generates all links from config")]
	class LinkAllCommand : GlobalOptions {

		internal void Execute(IConsole console, IConfigHandler configHandler, IFileSystem fileSystem, ILinker linker) {
			IConfig config = configHandler.LoadConfig(path);

			console.WriteLine("\nCreating links based on config...");

			int successes = 0;

			foreach (ConfigLink configLink in config.LinkList) {
				if (!fileSystem.File.Exists(configLink.sourcePath) && !fileSystem.Directory.Exists(configLink.sourcePath)) {
					console.WriteLine("Path '{0}' does not exist!", configLink.sourcePath);
					continue;
				}

				if (linker.CreateLink(configLink))
					successes++;
			}

			console.WriteLine("\n### Finished! Created {0} / {1} links ###", successes, config.LinkList.Count);
		}

	}

}
