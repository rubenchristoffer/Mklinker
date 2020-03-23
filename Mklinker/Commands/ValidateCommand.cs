using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CommandLine;

namespace Mklinker.Commands {

    [Verb ("validate", HelpText = "This will validate the config file to see if it is valid")]
    public class ValidateCommand : GlobalOptions, IDefaultAction {

        [Option ("all", HelpText = "This will list all entries in config and not just the ones that didn't pass validation")]
        public bool displayAll { get; private set; }

        public void Execute() {
            Program.LoadConfig(path);

            foreach (ConfigLink configLink in Program.config.linkList) {
                bool validation1 = ValidateExistence (configLink);
                bool validation2 = ValidateLinkType(configLink);

                if (displayAll || !validation1 || !validation2)
                    Console.WriteLine("\n{0}\n\t# Source path exists: {1}\n\t# Link type acceptable: {2}", configLink.ToString(), validation1 ? "Yes" : "No", validation2 ? "Yes" : "No");
            }
        }

        private bool ValidateExistence (ConfigLink configLink) {
            return File.Exists(configLink.sourcePath) || Directory.Exists(configLink.sourcePath);
        }

        private bool ValidateLinkType (ConfigLink configLink) {
            if (File.Exists(configLink.sourcePath)) {
                return configLink.linkType == ConfigLink.LinkType.Symbolic || configLink.linkType == ConfigLink.LinkType.Hard;
            }

            if (Directory.Exists(configLink.sourcePath)) {
                return configLink.linkType == ConfigLink.LinkType.Symbolic || configLink.linkType == ConfigLink.LinkType.Junction;
            }

            return false;
        }

    }

}
