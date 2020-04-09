﻿using System;
using System.IO.Abstractions;
using CommandLine;
using Mklinker.Abstractions;

namespace Mklinker.Commands {

    [Verb ("validate", HelpText = "This will validate the config file to see if it is valid")]
    class ValidateCommand : GlobalOptions {

        [Option ("all", HelpText = "This will list all entries in config and not just the ones that didn't pass validation")]
        public bool displayAll { get; private set; }

        public ValidateCommand() : base() {}

        public ValidateCommand(bool displayAll, string path) : base(path) {
            this.displayAll = displayAll;
        }

        internal void Execute(IConsole console, IConfigHandler configHandler, IFileSystem fileSystem, IPathFormatter pathFormatter) {
            IConfig config = configHandler.LoadConfig(path);
            bool isValid = true;

            foreach (ConfigLink configLink in config.LinkList) {
                bool validation1 = ValidateExistence(fileSystem, configLink, pathFormatter);
                bool validation2 = ValidateLinkType(fileSystem, configLink, pathFormatter);

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

        internal bool ValidateExistence (IFileSystem fileSystem, ConfigLink configLink, IPathFormatter pathFormatter) {
            return fileSystem.File.Exists(configLink.sourcePath) || fileSystem.Directory.Exists(configLink.sourcePath);
        }

        internal bool ValidateLinkType (IFileSystem fileSystem, ConfigLink configLink, IPathFormatter pathFormatter) {
            string formattedSourcePath = pathFormatter.GetFormattedPath(configLink.sourcePath);

            if (fileSystem.File.Exists(formattedSourcePath)) {
                return configLink.linkType == ConfigLink.LinkType.Symbolic || configLink.linkType == ConfigLink.LinkType.Hard;
            }

            if (fileSystem.Directory.Exists(formattedSourcePath)) {
                return configLink.linkType == ConfigLink.LinkType.Symbolic || configLink.linkType == ConfigLink.LinkType.Junction;
            }

            return false;
        }

    }

}
