using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Mklinker.Commands {

    public class ValidateCommand : ICommand {

        public void ExecuteCommand(string[] args) {
            bool displayAll = args.Any(arg => arg.ToLower().Equals("all"));

            foreach (ConfigLink configLink in Program.config.linkList) {
                bool validation1 = ValidateExistence (configLink);
                bool validation2 = ValidateLinkType(configLink);

                if (displayAll || !validation1 || !validation2)
                    Console.WriteLine("\n{0}\n\t# Source path exists: {1}\n\t# Link type acceptable: {2}", configLink.ToString(), validation1 ? "Yes" : "No", validation2 ? "Yes" : "No");
            }
        }

        public bool ValidateExistence (ConfigLink configLink) {
            return File.Exists(configLink.sourcePath) || Directory.Exists(configLink.sourcePath);
        }

        public bool ValidateLinkType (ConfigLink configLink) {
            if (File.Exists(configLink.sourcePath)) {
                return configLink.linkType == ConfigLink.LinkType.Symbolic || configLink.linkType == ConfigLink.LinkType.Hard;
            }

            if (Directory.Exists(configLink.sourcePath)) {
                return configLink.linkType == ConfigLink.LinkType.Symbolic || configLink.linkType == ConfigLink.LinkType.Junction;
            }

            return false;
        }

        public string GetName() {
            return "Validate";
        }

    }

}
