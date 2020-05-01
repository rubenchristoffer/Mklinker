using System.Linq;
using CommandLine;
using System.IO.Abstractions;
using Mklinker.Abstractions;

namespace Mklinker.Commands {

	[Verb ("addlinks", HelpText = "Adds multiple new links to config file")]
	class AddLinksCommand : GlobalOptions {

		[Value (0, HelpText = "The path to the source directory", Required = true)]
		public string sourceDirectoryPath { get; private set; }

		[Value (1, HelpText = "The path to the target directory", Required = true)]
		public string targetDirectoryPath { get; private set; }

		[Value (2, Default = ConfigLink.LinkType.Default, HelpText = "The type of link you want to create. Default is Symbolic for files and Junction for directories")]
		public ConfigLink.LinkType linkType { get; private set; }

		[Option ('f', "force", Default = false, HelpText = "If this flag is set it will ignore validation checks and add it no matter what", Required = false)]
		public bool force { get; private set; }

		public AddLinksCommand () : base () { }

		public AddLinksCommand (string sourcePath, string targetPath, ConfigLink.LinkType linkType, string path) : base (path) {
			this.targetDirectoryPath = targetPath;
			this.sourceDirectoryPath = sourcePath;
			this.linkType = linkType;
		}

		internal void Execute (IConsole console, IConfigHandler configHandler, IFileSystem fileSystem, IPathResolver pathResolver) {
			
		}

	}

}
