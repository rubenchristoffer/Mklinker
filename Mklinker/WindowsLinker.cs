using System;
using System.Diagnostics;
using System.IO.Abstractions;
using Mklinker.Abstractions;
using LinkType = Mklinker.ConfigLink.LinkType;

namespace Mklinker {

	class WindowsLinker : ILinker {

		readonly IConfigHandler configHandler;
		readonly IFileSystem fileSystem;

		public WindowsLinker (IConfigHandler configHandler, IFileSystem fileSystem) {
			this.configHandler = configHandler;
			this.fileSystem = fileSystem;
		}

		ProcessStartInfo GetProcessInfo(IFileSystem fileSystem, ConfigLink configLink) {
			return new ProcessStartInfo {
				FileName = "cmd.exe",
				Arguments = string.Format("/c mklink{0} {1} {2}", GetLinkTypeArgument(fileSystem, configLink.linkType, configLink.sourcePath), fileSystem.Path.GetFullPath(configLink.targetPath), fileSystem.Path.GetFullPath(configLink.sourcePath)),
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false
			};
		}

		string GetLinkTypeArgument(IFileSystem fileSystem, LinkType linkType, string sourcePath) {
			if (fileSystem.File.Exists(sourcePath)) {
				return linkType == LinkType.Hard ? " /H" : "";
			} else if (fileSystem.Directory.Exists(sourcePath)) {
				return linkType == LinkType.Symbolic ? " /D" : " /J";
			}

			return "";
		}

		bool ILinker.CreateLink(ConfigLink configLink) {
			Process mklinkProcess = Process.Start(GetProcessInfo(fileSystem, configLink));
			bool success = false;

			while (!mklinkProcess.StandardOutput.EndOfStream) {
				string output = mklinkProcess.StandardOutput.ReadLine();
				success = output.ToLower().Contains("created");

				Console.WriteLine(output);
			}

			while (!mklinkProcess.StandardError.EndOfStream) {
				success = false;
				Console.WriteLine(mklinkProcess.StandardError.ReadLine());
			}

			return success;
		}

	}

}
