using System.Linq;
using CommandLine;
using System.IO.Abstractions;
using Mklinker.Abstractions;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;
using System.Runtime.CompilerServices;

namespace Mklinker.Commands {

    [Verb("scan", HelpText = "Detect if circular paths (loops) exist for a given root folder by scanning all directories and collecting data. This detection is very rough and will only give an indication by displaying how often certain directory names are used (indicating loops around those directory names)")]
    class DetectCommand {

        private List<string> cases = new List<string>();
        private bool error = false;

        [Value (0, Default = ".", HelpText = "The root folder that should be scanned. Default will scan current working directory", Required = false)]
        public string rootFolder { get; private set; }

        [Option('l', "limit", Default = 30, HelpText = "Maximum amount of subfolders used for detecting loop (recursion limit). Higher values might result in more time, but will be more accurate", Required = false)]
        public int recursionLimit { get; private set; }

        [Option('v', "verbose", Default = false, HelpText = "Will display detailed output including every path that has reached the recursion limit")]
        public bool verbose { get; private set; }

        internal void Execute (IConsole console, IFileSystem fileSystem, IPathResolver pathResolver) {
            if (!fileSystem.Directory.Exists(rootFolder)) {
                console.WriteLine ($"Root folder '{rootFolder}' does not exist", IConsole.ContentType.Negative);
                return;
            }

            console.WriteLine ($"### Running recursion limit test (limit = {recursionLimit}) ###", IConsole.ContentType.Header);
            ScanRecursive (console, fileSystem, pathResolver, rootFolder, rootFolder, 0);

            if (!error) {
                if (cases.Count == 0) {
                    console.WriteLine ("No loops found!", IConsole.ContentType.Positive);
                } else {
                    console.WriteLine ("Possible loops found!", IConsole.ContentType.Negative);
                    console.WriteLine ();

                    console.WriteLine ($"### Collecting word count for directory names ###", IConsole.ContentType.Header);
                    CountDirectoryWords (console);
                }
            }
        }

        internal void ScanRecursive (IConsole console, IFileSystem fileSystem, IPathResolver pathResolver, string rootFolder, string currentFolder, int recursionLevel) {
            try {
                // Try to find loops by using a recursion limit
                foreach (string directory in fileSystem.Directory.GetDirectories (currentFolder)) {
                    if (recursionLevel >= recursionLimit) {
                        if (verbose) {
                            console.WriteLine (directory);
                            console.WriteLine ();
                        }

                        cases.Add (directory.Replace ('\\', '/'));

                        continue;
                    }

                    ScanRecursive (console, fileSystem, pathResolver, rootFolder, directory, recursionLevel + 1);
                }
            } catch(IOException e) {
                // Most common error is when recursion liimt is too high
                console.WriteLine ("An error has occured!", IConsole.ContentType.Negative);
                console.WriteLine ($"Perhaps recursion limit ({recursionLimit}) is set too high?", IConsole.ContentType.Negative);
                console.WriteLine ($"Try setting recursion limit to {recursionLevel - 1} or lower", IConsole.ContentType.Negative);
                
                if (verbose) {
                    console.WriteLine ();
                    console.WriteLine (e.ToString(), IConsole.ContentType.Negative);
                }

                cases.Clear ();
                error = true;
            }
        }

        internal void CountDirectoryWords (IConsole console) {
            Dictionary<string, int> wordCount = new Dictionary<string, int> ();
            Dictionary<string, List<string>> wordAndPaths = new Dictionary<string, List<string>> ();

            // Count how often directory words occur in the paths
            // Directory names that appear often have a higher chance
            // of being the cause of the loop or near the cause of the loop
            for (int i = 0; i < cases.Count; i++) {
                string @case = cases[i];
                string[] pathDirectories = @case.Split ('/');

                for (int ii = 0; ii < pathDirectories.Length; ii++) {
                    string word = pathDirectories[ii];

                    if (wordCount.ContainsKey (word)) {
                        wordCount[word]++;
                    } else if (!word.Equals(rootFolder)) {
                        wordCount.Add (word, 1);
                    }
                }
            }

            // Write all word counts at the end
            var wordCollection = wordCount
                .OrderByDescending(wc => wc.Value)
                .Select (wc => $"{wc.Key} ({wc.Value})");

            console.WriteLine ($"Directory name(s) ordered by word count:");
            console.Write ("| ");

            foreach (string word in wordCollection) {
                console.Write (word, IConsole.ContentType.Negative);
                console.Write (" | ");
            }

            console.WriteLine ();
        }

    }

}
