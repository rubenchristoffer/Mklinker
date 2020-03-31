using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using System.IO.Abstractions;

namespace Mklinker.Commands {

    [Verb ("validate", HelpText = "This will validate the config file to see if it is valid")]
    public class ValidateCommand : GlobalOptions, IDefaultAction {

        [Option ("all", HelpText = "This will list all entries in config and not just the ones that didn't pass validation")]
        public bool displayAll { get; private set; }

        void IDefaultAction.Execute(IConfigHandler configHandler, IFileSystem fileSystem) {
            IConfig config = configHandler.LoadConfig(path);

            foreach (ConfigLink configLink in config.LinkList) {
                bool validation1 = ValidateExistence(fileSystem, configLink);
                bool validation2 = ValidateLinkType(fileSystem, configLink);

                if (displayAll || !validation1 || !validation2)
                    Console.WriteLine("\n{0}\n\t# Source path exists: {1}\n\t# Link type acceptable: {2}", configLink.ToString(), validation1 ? "Yes" : "No", validation2 ? "Yes" : "No");
            }
        }

        private bool ValidateExistence (IFileSystem fileSystem, ConfigLink configLink) {
            return fileSystem.File.Exists(configLink.sourcePath) || fileSystem.Directory.Exists(configLink.sourcePath);
        }

        private bool ValidateLinkType (IFileSystem fileSystem, ConfigLink configLink) {
            if (fileSystem.File.Exists(configLink.sourcePath)) {
                return configLink.linkType == ConfigLink.LinkType.Symbolic || configLink.linkType == ConfigLink.LinkType.Hard;
            }

            if (fileSystem.Directory.Exists(configLink.sourcePath)) {
                return configLink.linkType == ConfigLink.LinkType.Symbolic || configLink.linkType == ConfigLink.LinkType.Junction;
            }

            return false;
        }

    }

}
