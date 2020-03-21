using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Mklinker.Commands;
using CommandLine;
using CommandLine.Text;

namespace Mklinker {

	public class Program {

		public const string VERSION = "v1.2.0";

		public static Config config { get; private set; }

		public class GlobalOptions {

			public static bool InteractiveFlag { get; set; }

			[Option(Default = false, Hidden = true)]
			public bool Interactive {
				get => InteractiveFlag;
				set { if (value) InteractiveFlag = true; }
			}

			[Verb("--interactive", HelpText = "Interactive mode means that you can run more than one command")]
			public class InteractiveVerb : IDefaultAction {
				void IDefaultAction.Execute() {
					InteractiveFlag = true;
				}
			}

		}

		public static void Main(string[] args) {
			if (!File.Exists(Config.configFile)) {
				Console.WriteLine("\nCreating config file 'linker.config' since it does not exist");
				File.Create(Config.configFile).Close();
			} else if (args.Length == 0) {
				Console.WriteLine("\nNo valid arguments are provided and config file already exists");
			}

			config = Config.Deserialize(File.ReadAllText(Config.configFile));

			// Parse commands
			var parser = Parser.Default;//new Parser(with => { });
			var parserResult = parser.ParseArguments<GlobalOptions.InteractiveVerb, AddLinkCommand>(args);

			parserResult
				.WithParsed<IDefaultAction>(flag => flag.Execute());
				//.WithNotParsed(errors => DisplayHelp(parserResult, errors));

			SaveConfig();

			Console.WriteLine("Is interactive: " + GlobalOptions.InteractiveFlag);
			Console.ReadLine();
		}

		private static void DisplayHelp<T>(ParserResult<T> result, IEnumerable<Error> errs) {
			var helpText = HelpText.AutoBuild(result, h => {
				h.AdditionalNewLineAfterOption = false;
				h.Heading = String.Format("Mklinker {0}", VERSION);
				h.Copyright = "Copyright (c) 2020";
				return HelpText.DefaultParsingErrorsHandler(result, h);
			}, e => e);

			Console.WriteLine(helpText);
		}

		public static void SaveConfig () {
			config.version = VERSION;
			File.WriteAllText(Config.configFile, config.Serialize());
		}

	}

}
