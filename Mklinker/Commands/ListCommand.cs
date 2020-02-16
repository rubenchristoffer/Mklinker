using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mklinker.Commands {

	public class ListCommand : ICommand {

		public void ExecuteCommand (string[] args) {
			foreach (ConfigLink configLink in Program.config.linkList) {
				Console.WriteLine("\nLink type {0}:\n\t- Target: {1}\n\t- Source: {2}", configLink.linkType.ToString(), configLink.targetPath, configLink.sourcePath);
			}
		}

		public string GetName() {
			return "List";
		}

	}

}
