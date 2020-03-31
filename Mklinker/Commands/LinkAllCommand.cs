using System;
using System.IO.Abstractions;
using System.Diagnostics;
using CommandLine;
using LinkType = Mklinker.ConfigLink.LinkType;

namespace Mklinker.Commands {

	[Verb ("linkall", HelpText = "Generates all links from config")]
	public class LinkAllCommand : GlobalOptions, IDefaultAction {

		// TODO: Abstract this
		private ProcessStartInfo GetProcessInfo(IFileSystem fileSystem, ConfigLink configLink) {
			return new ProcessStartInfo {
				FileName = "cmd.exe",
				Arguments = string.Format("/c mklink{0} {1} {2}", GetLinkTypeArgument(fileSystem, configLink.linkType, configLink.sourcePath), fileSystem.Path.GetFullPath(configLink.targetPath), fileSystem.Path.GetFullPath(configLink.sourcePath)),
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false
			};
		}

		private string GetLinkTypeArgument(IFileSystem fileSystem, LinkType linkType, string sourcePath) {
			if (fileSystem.File.Exists(sourcePath)) {
				return linkType == LinkType.Hard ? " /H" : "";
			} else if (fileSystem.Directory.Exists(sourcePath)) {
				return linkType == LinkType.Symbolic ? " /D" : " /J";
			}

			return "";
		}

		void IDefaultAction.Execute(IConfigHandler configHandler, IFileSystem fileSystem) {
			IConfig config = configHandler.LoadConfig(path);

			Console.WriteLine("\nCreating links based on config...");

			int successes = 0;

			foreach (ConfigLink linkTask in config.LinkList) {
				if (!fileSystem.File.Exists(linkTask.sourcePath) && !fileSystem.Directory.Exists(linkTask.sourcePath)) {
					Console.WriteLine("Path '{0}' does not exist!", linkTask.sourcePath);
					continue;
				}

				// TODO: Make abstraction of this
				Process mklinkProcess = Process.Start(GetProcessInfo(fileSystem, linkTask));

				while (!mklinkProcess.StandardOutput.EndOfStream) {
					string output = mklinkProcess.StandardOutput.ReadLine();

					if (output.ToLower().Contains("created"))
						successes++;

					Console.WriteLine(output);
				}

				while (!mklinkProcess.StandardError.EndOfStream) {
					Console.WriteLine(mklinkProcess.StandardError.ReadLine());
				}
			}

			Console.WriteLine("\n### Finished! Created {0} / {1} links ###", successes, config.LinkList.Count);
		}

	}

}
