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
		readonly IPathFormatter pathFormatter;

		public WindowsLinker (IConsole console, IFileSystem fileSystem, IProcess process, IPathFormatter pathFormatter) {
			this.console = console;
			this.fileSystem = fileSystem;
			this.process = process;
			this.pathFormatter = pathFormatter;
		}

		internal ProcessStartInfo GetProcessInfo(IFileSystem fileSystem, string sourcePath, string targetPath, ConfigLink.LinkType linkType) {
			return new ProcessStartInfo {
				FileName = "cmd.exe",

				Arguments = string.Format("/c mklink {0} \"{1}\" \"{2}\"", 
				GetLinkTypeArgument(fileSystem, linkType, sourcePath), 
				fileSystem.Path.GetFullPath(targetPath), 
				fileSystem.Path.GetFullPath(sourcePath)),

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
			string sourcePath = pathFormatter.GetFormattedPath(configLink.sourcePath);
			string targetPath = pathFormatter.GetFormattedPath(configLink.targetPath);

			if (!fileSystem.File.Exists(sourcePath) && !fileSystem.Directory.Exists(sourcePath)) {
				console.WriteLine("Path '{0}' does not exist!", sourcePath);

				return false;
			}

			IProcess mklinkProcess = process.Start(GetProcessInfo(fileSystem, sourcePath, targetPath, configLink.linkType));
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
