using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using CommandLine;

namespace Mklinker.Commands {

	[Verb ("removelink", HelpText = "Removes link from config")]
	public class RemoveLinkCommand : GlobalOptions, IDefaultAction {

		[Value (0, HelpText = "The targetPath matching entry you want to delete from config.", MetaName = "targetPath", Required = true)]
		public string targetPath { get; private set; }

		public void Execute() {
			Program.LoadConfig(path);
			ConfigLink configLink = Program.config.linkList.FirstOrDefault (link => Path.GetFullPath(link.targetPath).Equals(Path.GetFullPath(targetPath)));

			if (configLink != null) {
				Program.config.linkList.Remove(configLink);
				Console.WriteLine("\nSuccessfully removed link with targetPath '{0}'", targetPath);

				Program.SaveConfig();
			} else {
				Console.WriteLine(String.Format("\nThe targetPath '{0}' is invalid because it does not exist in config", targetPath));
			}
		}

	}

}
