using System;
using System.IO.Abstractions;
using CommandLine;
using Mklinker.Abstractions;
using System.Collections.Generic;

namespace Mklinker.Commands {

    [Verb ("validate", HelpText = "This will validate the config file to see if it is valid")]
    class ValidateCommand : GlobalOptions {

        [Option ("all", HelpText = "This will list all entries in config and not just the ones that didn't pass validation")]
        public bool displayAll { get; private set; }

        public ValidateCommand() : base() {}

        public ValidateCommand(bool displayAll, string path) : base(path) {
            this.displayAll = displayAll;
        }

        internal void Execute(IConsole console, IConfigHandler configHandler, IFileSystem fileSystem, IPathResolver pathResolver) {
            IConfig config = configHandler.LoadConfig(path);
            bool isValid = true;

            foreach (ConfigLink configLink in config.LinkList) {
                string resolvedSourcePath = pathResolver.GetAbsoluteResolvedPath(configLink.sourcePath, config.Variables);

                bool validation1 = ValidateExistence(fileSystem, resolvedSourcePath);
                bool validation2 = ValidateLinkType(fileSystem, resolvedSourcePath, configLink);

                if (displayAll || !validation1 || !validation2) {
                    console.WriteLine("\n{0}\n\t# Source path exists: {1}\n\t# Link type acceptable: {2}", configLink.ToString(), validation1 ? "Yes" : "No", validation2 ? "Yes" : "No");
                    isValid = false;
                }
            }

            if (config.LinkList.Count == 0)
                console.WriteLine("Config is empty");
            else if (isValid)
                console.WriteLine("Config is 100% valid");
        }   

        internal bool ValidateExistence (IFileSystem fileSystem, string resolvedSourcePath) {
            return fileSystem.File.Exists(resolvedSourcePath) || fileSystem.Directory.Exists(resolvedSourcePath);
        }

        internal bool ValidateLinkType (IFileSystem fileSystem, string resolvedSourcePath, ConfigLink configLink) {
            if (fileSystem.File.Exists(resolvedSourcePath)) {
                return configLink.linkType == ConfigLink.LinkType.Symbolic || configLink.linkType == ConfigLink.LinkType.Hard;
            }

            if (fileSystem.Directory.Exists(resolvedSourcePath)) {
                return configLink.linkType == ConfigLink.LinkType.Symbolic || configLink.linkType == ConfigLink.LinkType.Junction;
            }

            return false;
        }

    }

}
