using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace Mklinker.Commands {

	public class AddLinkCommand : ICommand {

		public void ExecuteCommand(string[] args) {
			if (args.Length < 2) {
				Console.WriteLine("\nAt least 2 arguments are required this command");
				Console.WriteLine("Syntax: addlink [target] [source] <link type>");
				Console.WriteLine("Link Types: j (junction), s (symbolic), h (hard)");
				Console.WriteLine("Example: addlink myTargetFolder mySourceFolder j");
				return;
			}

			string targetPath = args[0];
			string sourcePath = args[1];
			ConfigLink.LinkType linkType = ConfigLink.LinkType.None;

			if (!File.Exists(sourcePath) && !Directory.Exists(sourcePath)) {
				Console.WriteLine(String.Format("\nThe sourcePath '{0}' is invalid because it does not exist", sourcePath));
				return;
			}

			if (Program.config.linkList.Any (link => link.targetPath.Equals(targetPath))) {
				Console.WriteLine(String.Format("\nThe targetPath '{0}' is invalid because it already exists in config file", targetPath));
				return;
			}

			// Check if explicit link type is provided
			if (args.Length > 2) {
				if (args[2].ToLower().Contains("j")) {
					linkType = ConfigLink.LinkType.Junction;
				} else if (args[2].ToLower().Contains("s")) {
					linkType = ConfigLink.LinkType.Symbolic;
				} else if (args[2].ToLower().Contains("h")) {
					linkType = ConfigLink.LinkType.Hard;
				}
			}

			// Set default link type if type is not provided
			if (linkType == ConfigLink.LinkType.None) {
				if (File.Exists(sourcePath)) {
					linkType = ConfigLink.LinkType.Symbolic;
				} else {
					linkType = ConfigLink.LinkType.Junction;
				}
			}

			Program.config.linkList.Add(new ConfigLink(sourcePath, targetPath, linkType));
			Program.SaveConfig();

			Console.WriteLine(String.Format("\nAdded new {0} link to config file: \nTarget: '{1}'\nSource: '{2}'", linkType.ToString(), targetPath, sourcePath));
		}

		public string GetName() {
			return "AddLink";
		}

	}

}
