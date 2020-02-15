﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using LinkType = Mklinker.Config.LinkType;

namespace Mklinker.Commands {

	public class BuildCommand : ICommand {

		public void ExecuteCommand(string[] args) {
			Console.WriteLine("Creating links based on config...");

			int successes = 0;

			foreach (ConfigLink linkTask in Program.config.linkList) {
				if (!File.Exists(linkTask.sourcePath) && !Directory.Exists(linkTask.sourcePath)) {
					Console.WriteLine(String.Format("Path '{0}' does not exist!", linkTask.sourcePath));
					continue;
				}

				Process mklinkProcess = Process.Start(GetProcessInfo(linkTask));
				
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

			Console.WriteLine("Finished! Created {0} out of {1} links", successes, Program.config.linkList.Count);
		}

		public string GetName() {
			return "Build";
		}

		private ProcessStartInfo GetProcessInfo(ConfigLink configLink) {
			return new ProcessStartInfo {
				FileName = "cmd.exe",
				Arguments = string.Format("/c mklink{0} {1} {2}", GetLinkTypeArgument(configLink.linkType, configLink.sourcePath), configLink.targetPath, configLink.sourcePath),
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
