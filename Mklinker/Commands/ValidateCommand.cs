using System;
using System.Linq;
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
            if (!configHandler.DoesConfigExist(path)) {
                console.WriteLine($"Config '{ path }' does not exist. Type 'help config' in order to see how you create a config file.");
                return;
            }

            IConfig config = configHandler.LoadConfig(path);
            bool isValid = true;

            foreach (ConfigLink configLink in config.LinkList) {
                string resolvedSourcePath = pathResolver.GetAbsoluteResolvedPath(configLink.sourcePath, config.Variables);
                string resolvedTargetPath = pathResolver.GetAbsoluteResolvedPath(configLink.targetPath, config.Variables);

                bool validation1 = ValidateExistence(fileSystem, resolvedSourcePath);
                bool validation2 = ValidateLinkType(fileSystem, resolvedSourcePath, configLink.linkType);
                bool validation3 = ValidateDuplicate(pathResolver, config, resolvedTargetPath);

                if (displayAll || !validation1 || !validation2 || !validation3) {
                    console.WriteLine($"\n{ configLink.ToString() }");

                    isValid = false;
                }

                if (!validation1 || displayAll)
                    console.WriteLine($"\t# Source path exists: { (validation1 ? "Yes" : "No") }");

                if (!validation2 || displayAll)
                    console.WriteLine($"\t# Link type acceptable: { (validation2 ? "Yes" : "No") }");

                if (!validation3 || displayAll)
                    console.WriteLine($"\t# Duplicate target path exists: { (validation3 ? "False" : "True") }");
            }

            if (config.LinkList.Count == 0)
                console.WriteLine("Config is empty");
            else if (isValid)
                console.WriteLine("Config is 100% valid");
        }   

        internal bool ValidateExistence (IFileSystem fileSystem, string resolvedSourcePath) {
            return fileSystem.File.Exists(resolvedSourcePath) || fileSystem.Directory.Exists(resolvedSourcePath);
        }

        internal bool ValidateLinkType (IFileSystem fileSystem, string resolvedSourcePath, ConfigLink.LinkType linkType) {
            if (linkType == ConfigLink.LinkType.Default)
                return true;

            if (fileSystem.File.Exists(resolvedSourcePath)) {
                return linkType == ConfigLink.LinkType.Symbolic || linkType == ConfigLink.LinkType.Hard;
            }

            if (fileSystem.Directory.Exists(resolvedSourcePath)) {
                return linkType == ConfigLink.LinkType.Symbolic || linkType == ConfigLink.LinkType.Junction;
            }

            return false;
        }

        internal bool ValidateDuplicate (IPathResolver pathResolver, IConfig config, string resolvedTargetPath) {
            return config.LinkList
                .Where(link => pathResolver.GetAbsoluteResolvedPath(link.targetPath, config.Variables)
                    .Equals(resolvedTargetPath, StringComparison.OrdinalIgnoreCase))
                .Count() <= 1;
        }

    }

}
