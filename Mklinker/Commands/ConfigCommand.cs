using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using System.IO.Abstractions;

namespace Mklinker.Commands {

	[Verb("config", HelpText = "Displays information about config file by default, but can create / delete / modify config file")]
	public class ConfigCommand : GlobalOptions, IDefaultAction {

		readonly IFileSystem _fileSystem;

		IFile File {
			get => _fileSystem.File;
		}

		public ConfigCommand () {}
		public ConfigCommand(IFileSystem fileSystem) { _fileSystem = fileSystem; }

		[Option ('c', "create", HelpText = "Creates a new config file if it does not exist", SetName = "Create")]
		public bool create { get; private set; }

		[Option('d', "delete", HelpText = "Deleted config file if it exists", SetName = "Delete")]
		public bool delete { get; private set; }

		public void Execute() {
			if (create) {
				if (File.Exists(path)) {
					Console.WriteLine("Config file already exists!");
				} else {
					Console.WriteLine("Creating config file '{0}'", path);

					Program.CreateNewConfig();
					Program.SaveConfig(path);
				}
			} else if (delete) {
				if (File.Exists(path)) {
					Console.WriteLine("Deleting config file '{0}'", path);
					File.Delete(path);
				} else {
					Console.WriteLine("Config file '{0}' does not exist!", path);
				}
			} else {
				if (File.Exists(path)) {
					Program.LoadConfig(path);

					Console.WriteLine();
					Console.WriteLine("### File info ###");
					Console.WriteLine("Full path: {0}", path);
					Console.WriteLine("Version: {0}", Program.config.version);
					Console.WriteLine();
					Console.WriteLine("### Link info ###");
					Console.WriteLine("Total links: " + Program.config.linkList.Count);
					Console.WriteLine("Junction links: " + Program.config.linkList.Count (l => l.linkType == ConfigLink.LinkType.Junction));
					Console.WriteLine("Symbolic links: " + Program.config.linkList.Count(l => l.linkType == ConfigLink.LinkType.Symbolic));
					Console.WriteLine("Hard links: " + Program.config.linkList.Count(l => l.linkType == ConfigLink.LinkType.Hard));
				} else {
					Console.WriteLine("Config file does not exist. You can create one with the --create option.");
				}
			}
		}

	}

}
