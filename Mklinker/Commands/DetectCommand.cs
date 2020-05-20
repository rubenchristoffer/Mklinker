using System.Linq;
using CommandLine;
using System.IO.Abstractions;
using Mklinker.Abstractions;
using System.Collections.Generic;
using System.IO;

namespace Mklinker.Commands {

    [Verb("detect", HelpText = "Detect if circular paths exist for a given root folder")]
    class DetectCommand {

        private HashSet<string> visitedFolders;

        [Value (0, Default = ".", HelpText = "The root folder that should be scanned. Default will scan current working directory", Required = false)]
        public string rootFolder { get; private set; }

        internal void Execute (IConsole console, IFileSystem fileSystem) {
            ScanSubfolders(console, fileSystem, rootFolder);
        }

        internal void ScanSubfolders (IConsole console, IFileSystem fileSystem, string rootFolder) {
            foreach (string directory in fileSystem.Directory.GetDirectories(rootFolder)) {
                if (visitedFolders.Contains(directory)) {
                    console.WriteLine($"Folder {directory} has been visited before!", IConsole.ContentType.Negative);
                } else {
                    visitedFolders.Add(directory);
                }

                ScanSubfolders(console, fileSystem, directory);
            }
        }

    }

}
