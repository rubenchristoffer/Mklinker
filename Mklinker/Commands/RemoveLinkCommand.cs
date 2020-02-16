using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

namespace Mklinker.Commands {

	public class RemoveLinkCommand : ICommand {

		public void ExecuteCommand(string[] args) {
			if (args.Length != 1) {
				Console.WriteLine("\nRemoveLink command only takes 1 argument");
				Console.WriteLine("Syntax: RemoveLink [targetPath]");
				return;
			}

			ConfigLink configLink = Program.config.linkList.FirstOrDefault (link => Path.GetFullPath(link.targetPath).Equals(Path.GetFullPath(args[0])));

			if (configLink != null) {
				Program.config.linkList.Remove(configLink);
				Console.WriteLine("\nSuccessfully removed link with targetPath '{0}'", args[0]);

				Program.SaveConfig();
			} else {
				Console.WriteLine(String.Format("\nThe targetPath '{0}' is invalid because it does not exist in config", args[0]));
			}
		}

		public string GetName() {
			return "RemoveLink";
		}

	}

}
