using System;
using System.Collections.Generic;
using System.Text;
using Mklinker.Abstractions;
using System.IO.Abstractions;
using System.Diagnostics;
using static Mklinker.ConfigLink;

namespace Mklinker {

	class UnixLinker : ILinker {

		readonly IConsole console;
		readonly IFileSystem fileSystem;
		readonly IProcess process;

		public UnixLinker(IConsole console, IFileSystem fileSystem, IProcess process) {
			this.console = console;
			this.fileSystem = fileSystem;
			this.process = process;
		}

		internal ProcessStartInfo GetProcessInfo(IFileSystem fileSystem, string resolvedSourcePath, string resolvedTargetPath, ConfigLink.LinkType linkType) {
			return new ProcessStartInfo {
				FileName = "ln",
				Arguments = $"\"{ resolvedSourcePath }\"" +
				$" \"{ resolvedTargetPath }\"" +
				$" \"{ GetLinkTypeArgument(fileSystem, linkType, resolvedSourcePath) }\"",
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false
			};
		}

		internal string GetLinkTypeArgument(IFileSystem fileSystem, LinkType linkType, string resolvedSourcePath) {
			if (fileSystem.File.Exists(resolvedSourcePath)) {
				return linkType == LinkType.Hard ? "" : "-s";
			} else if (fileSystem.Directory.Exists(resolvedSourcePath)) {
				return "-s";
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
			bool success = true;

			while (!mklinkProcess.StandardOutput.EndOfStream) {
				success = false;
				console.WriteLine(mklinkProcess.StandardOutput.ReadLine());
			}

			while (!mklinkProcess.StandardError.EndOfStream) {
				success = false;
				console.WriteLine(mklinkProcess.StandardError.ReadLine());
			}

			return success;
		}
	}

}
