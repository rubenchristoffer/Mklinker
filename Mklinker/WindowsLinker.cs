using System;
using System.Diagnostics;
using System.IO.Abstractions;
using Mklinker.Abstractions;
using LinkType = Mklinker.ConfigLink.LinkType;

namespace Mklinker {

	class WindowsLinker : ILinker {

		readonly IConsole console;
		readonly IConfigHandler configHandler;
		readonly IFileSystem fileSystem;
		readonly IProcess process;

		public WindowsLinker (IConsole console, IConfigHandler configHandler, IFileSystem fileSystem, IProcess process) {
			this.console = console;
			this.configHandler = configHandler;
			this.fileSystem = fileSystem;
			this.process = process;
		}

		internal ProcessStartInfo GetProcessInfo(IFileSystem fileSystem, ConfigLink configLink) {
			return new ProcessStartInfo {
				FileName = "cmd.exe",

				Arguments = string.Format("/c mklink {0} \"{1}\" \"{2}\"", 
				GetLinkTypeArgument(fileSystem, configLink.linkType, configLink.sourcePath), 
				fileSystem.Path.GetFullPath(configLink.targetPath), 
				fileSystem.Path.GetFullPath(configLink.sourcePath)),

				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false
			};
		}

		internal string GetLinkTypeArgument(IFileSystem fileSystem, LinkType linkType, string sourcePath) {
			if (fileSystem.File.Exists(sourcePath)) {
				return linkType == LinkType.Hard ? "/H" : "";
			} else if (fileSystem.Directory.Exists(sourcePath)) {
				return linkType == LinkType.Symbolic ? "/D" : "/J";
			}

			return "";
		}

		bool ILinker.CreateLink(ConfigLink configLink) {
			IProcess mklinkProcess = process.Start(GetProcessInfo(fileSystem, configLink));
			bool success = false;
			
			while (!mklinkProcess.StandardOutput.EndOfStream) {
				string output = mklinkProcess.StandardOutput.ReadLine();
				success = output.ToLower().Contains("created");

				console.WriteLine(output);
			}

			while (!mklinkProcess.StandardError.EndOfStream) {
				success = false;
				console.WriteLine(mklinkProcess.StandardError.ReadLine());
			}

			return success;
		}

	}

}
