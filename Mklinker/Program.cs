using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Mklinker.Commands;

namespace Mklinker {

	public class Program {

		public const string VERSION = "v1.2.0";

		public static Config config { get; private set; }

		public static void Main(string[] args) {
			if (!File.Exists(Config.configFile)) {
				Console.WriteLine("\nCreating config file 'linker.config' since it does not exist");
				File.Create(Config.configFile).Close();
			} else if (args.Length == 0) {
				Console.WriteLine("\nNo valid arguments are provided and config file already exists");
			}

			config = Config.Deserialize(File.ReadAllText(Config.configFile));

			SaveConfig();
		}

		public static void SaveConfig () {
			config.version = VERSION;
			File.WriteAllText(Config.configFile, config.Serialize());
		}

	}

}
