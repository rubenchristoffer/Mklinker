using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace Mklinker.Commands {

	public class BuildCommand : ICommand {

		public enum LinkType {
			Junction,
			Symbolic,
			Hard
		}

		public void ExecuteCommand(string[] args) {
			Console.WriteLine("Creating links based on config...");

			foreach (string task in Program.config) {
				Process mklinkProcess = Process.Start(GetProcessInfo(LinkType.Symbolic, args[1], args[2]));
			}

			Console.WriteLine("Finished");
		}

		public string GetName() {
			return "Build";
		}

		private ProcessStartInfo GetProcessInfo(LinkType linkType, string targetPath, string sourcePath) {
			return new ProcessStartInfo {
				FileName = "mklink",
				Arguments = string.Format("{0} {1} {2}", GetLinkTypeArgument(linkType, sourcePath), targetPath, sourcePath),
				RedirectStandardOutput = true,
				RedirectStandardError = true
			};
		}

		private string GetLinkTypeArgument(LinkType linkType, string sourcePath) {
			if (File.Exists(sourcePath)) {
				return linkType == LinkType.Hard ? " /H" : "";
			} else if (Directory.Exists(sourcePath)) {
				return linkType == LinkType.Symbolic ? " /D" : " /J";
			}

			return "";
		}

	}

}
