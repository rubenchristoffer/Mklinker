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

    [Verb("detect", HelpText = "Detect if circular paths exist for a given root folder")]
    class DetectCommand {

        private List<string> cases = new List<string>();

        [Option ('p', "path", Default = ".", HelpText = "The root folder that should be scanned. Default will scan current working directory", Required = false)]
        public string rootFolder { get; private set; }

        [Option('l', "limit", Default = 20, HelpText = "Maximum amount of subfolders used for detecting loop (recursion limit)", Required = false)]
        public int recursionLimit { get; private set; }

        [Option('v', "verbose", Default = true, HelpText = "Will display detailed output including every path that has reached the recursion limit")]
        public bool verbose { get; private set; }

        internal void Execute (IConsole console, IFileSystem fileSystem, IPathResolver pathResolver) {
            if (!fileSystem.Directory.Exists(rootFolder)) {
                console.WriteLine ($"Root folder '{rootFolder}' does not exist", IConsole.ContentType.Negative);
                return;
            }

            console.WriteLine ($"### Recursion limit test (limit = {recursionLimit}) ###", IConsole.ContentType.Header);
            ScanRecursive (console, fileSystem, pathResolver, rootFolder, rootFolder, 0);

            console.WriteLine ($"### Try to detect repeating directory names ###", IConsole.ContentType.Header);
            DetectRepeatingPath (console);

            if (cases.Count == 0) {
                console.WriteLine ("No loops found!", IConsole.ContentType.Positive);
            }
        }

        internal void ScanRecursive (IConsole console, IFileSystem fileSystem, IPathResolver pathResolver, string rootFolder, string currentFolder, int recursionLevel) {
            // Try to find loops by using a recursion limit
            foreach (string directory in fileSystem.Directory.GetDirectories (currentFolder)) {
                if (recursionLevel >= recursionLimit) {
                    console.WriteLine (directory);
                    console.WriteLine ();
                    cases.Add (directory.Replace('\\', '/'));

                    continue;
                }

                ScanRecursive (console, fileSystem, pathResolver, rootFolder, directory, recursionLevel + 1);
            }
        }

        internal void DetectRepeatingPath (IConsole console) {
            if (cases.Count == 0) {
                return;
            }

            Dictionary<string, int> wordCount = new Dictionary<string, int> ();
            Dictionary<string, List<string>> wordAndPaths = new Dictionary<string, List<string>> ();

            // Try to find directory that creates loop here
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
