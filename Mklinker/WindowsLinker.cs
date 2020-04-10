using System;
using System.Diagnostics;
using System.IO.Abstractions;
using Mklinker.Abstractions;
using LinkType = Mklinker.ConfigLink.LinkType;

namespace Mklinker {

	class WindowsLinker : ILinker {

		readonly IConsole console;
		readonly IFileSystem fileSystem;
		readonly IProcess process;

		public WindowsLinker (IConsole console, IFileSystem fileSystem, IProcess process) {
			this.console = console;
			this.fileSystem = fileSystem;
			this.process = process;
		}

		internal ProcessStartInfo GetProcessInfo(IFileSystem fileSystem, string resolvedSourcePath, string resolvedTargetPath, ConfigLink.LinkType linkType) {
			return new ProcessStartInfo {
				FileName = "cmd.exe",

				Arguments = string.Format("/c mklink {0} \"{1}\" \"{2}\"", 
				GetLinkTypeArgument(fileSystem, linkType, resolvedSourcePath),
				resolvedTargetPath,
				resolvedSourcePath),

				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false
			};
		}

		internal string GetLinkTypeArgument(IFileSystem fileSystem, LinkType linkType, string resolvedSourcePath) {
			if (fileSystem.File.Exists(resolvedSourcePath)) {
				return linkType == LinkType.Hard ? "/H" : "";
			} else if (fileSystem.Directory.Exists(resolvedSourcePath)) {
				return linkType == LinkType.Symbolic ? "/D" : "/J";
			}

			return "";
		}

		public bool CreateLink(string resolvedSourcePath, string resolvedTargetPath, ConfigLink.LinkType linkType) {
			if (!fileSystem.File.Exists(resolvedSourcePath) && !fileSystem.Directory.Exists(resolvedSourcePath)) {
				console.WriteLine("Path '{0}' does not exist!", resolvedSourcePath);

				return false;
			}

			ProcessStartInfo processStartInfo = GetProcessInfo(fileSystem, resolvedSourcePath, resolvedTargetPath, linkType);
			IProcess mklinkProcess = process.Start(processStartInfo);
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
