using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Mklinker.Commands {

	public class AddLinkCommand : ICommand {

		public void ExecuteCommand(string[] args) {
			if (args.Length < 2) {
				Console.WriteLine("At least 2 arguments are required this command");
				return;
			}

			string targetPath = args[0];
			string sourcePath = args[1];
			ConfigLink.LinkType linkType = ConfigLink.LinkType.None;

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
			Console.WriteLine(String.Format("Added new {0} link to config file", linkType.ToString()));
		}

		public string GetName() {
			return "AddLink";
		}

	}

}
