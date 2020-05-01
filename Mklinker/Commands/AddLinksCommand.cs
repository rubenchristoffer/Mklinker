using System.Linq;
using CommandLine;
using System.IO.Abstractions;
using Mklinker.Abstractions;
using System.Text.RegularExpressions;

namespace Mklinker.Commands {

	[Verb ("addlinks", HelpText = "Adds multiple new links at once to config file with optional filtering")]
	class AddLinksCommand : GlobalOptions {

		[Value (0, HelpText = "The path to the source directory", Required = true)]
		public string sourceDirectoryPath { get; private set; }

		[Value (1, HelpText = "The path to the target directory", Required = true)]
		public string targetDirectoryPath { get; private set; }

		[Value (2, Default = ConfigLink.LinkType.Default, HelpText = "The type of link you want to create. Default is Symbolic for files and Junction for directories")]
		public ConfigLink.LinkType linkType { get; private set; }

		[Option('r', "regex", Default = @"[\s\S]*", HelpText = "Regex filter deciding which files / directories to add links for in source folder. Default will match everything", Required = false)]
		public string regexFilter { get; private set; }

		[Option('s', "subdirs", Default = false, HelpText = "Determines if files / directories from subdirectories are included as well", Required = false)]
		public bool includeSubdirectories { get; private set; }

		[Option('d', "dirs", Default = false, HelpText = "Determines if directory links should be created instead of file links", Required = false)]
		public bool linkDirectories { get; private set; }

		public AddLinksCommand () : base () { }

		public AddLinksCommand (string sourcePath, string targetPath, ConfigLink.LinkType linkType, string path) : base (path) {
			this.targetDirectoryPath = targetPath;
			this.sourceDirectoryPath = sourcePath;
			this.linkType = linkType;
		}

		internal void Execute (IConsole console, IConfigHandler configHandler, IFileSystem fileSystem, IPathResolver pathResolver) {
			if (!configHandler.DoesConfigExist(path)) {
				console.WriteLine($"Config '{ path }' does not exist. Type 'help config' in order to see how you create a config file.");
				return;
			}

			IConfig config = configHandler.LoadConfig(path);
			
			foreach (string file in fileSystem.Directory.GetFiles(pathResolver.GetAbsoluteResolvedPath(sourceDirectoryPath, config.Variables))) {
				if (Regex.IsMatch(file, regexFilter)) {
					AddLinkCommand addLinkCommand = new AddLinkCommand(
						fileSystem.Path.Combine(sourceDirectoryPath, file),
						fileSystem.Path.Combine(targetDirectoryPath, file), 
						linkType, 
						path);

					addLinkCommand.Execute(console, configHandler, fileSystem, pathResolver);
				}
			}
		}

	}

}
