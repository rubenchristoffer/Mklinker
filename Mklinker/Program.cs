using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Mklinker.Commands;
using CommandLine;
using CommandLine.Text;
using System.Reflection;
using System.Diagnostics;

namespace Mklinker {

	public class Program {

		public static Config config { get; private set; }

		public static void Main(string[] args) {
			ParseAndExecute(args);
		}

		public static void ParseAndExecute (string[] args) {
			// Parse commands
			var parser = new Parser(with => with.HelpWriter = Console.Out);
			var parserResult = parser.ParseArguments<AddLinkCommand, LinkAllCommand, ListCommand, RemoveLinkCommand, ValidateCommand, InteractiveCommand, ConfigCommand>(args);

			parserResult.WithParsed<IDefaultAction>(flag => flag.Execute());
		}

		public static void CreateNewConfig () {
			config = new Config();
			config.version = GetVersion();
		}

		public static void LoadConfig (string pathToConfigFile) {
			config = Config.Deserialize(File.ReadAllText(pathToConfigFile));
		}

		public static void SaveConfig () {
			config.version = GetVersion();
			File.WriteAllText(Config.DEFAULT_CONFIG_FILE, config.Serialize());
		}

		public static string GetVersion() {
			return FileVersionInfo.GetVersionInfo(Assembly.GetAssembly(typeof (Program)).Location).ProductVersion;
		}

	}

}
