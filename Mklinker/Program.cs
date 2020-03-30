using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Mklinker.Commands;
using CommandLine;
using CommandLine.Text;
using System.Reflection;
using System.Diagnostics;
using Autofac;
using System.IO.Abstractions;

namespace Mklinker {

	public static class Program {

		private static IContainer Container { get; set; }

		public static Config config { get; private set; }

		public static void Main(string[] args) {
			var builder = new ContainerBuilder();
			builder.RegisterType<FileSystem>().As<IFileSystem>();
			Container = builder.Build();

			ParseAndExecute(args);
		}

		public static void ParseAndExecute (string[] args) {
			// Parse commands
			var parser = new Parser(with => with.HelpWriter = Console.Out);
			var parserResult = parser.ParseArguments<AddLinkCommand, LinkAllCommand, ListCommand, RemoveLinkCommand, ValidateCommand, InteractiveCommand, ConfigCommand>(args);

			using (var scope = Container.BeginLifetimeScope ()) {
				parserResult.WithParsed<IDefaultAction>(flag => flag.Execute());
			}
		}

		public static string[] ParseStringToArguments(string input) {
			return input.Split('"')
				.Select((element, index) => index % 2 == 0 ? element.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries) : new string[] { element })
				.SelectMany(element => element).ToArray();
		}

		public static void CreateNewConfig () {
			config = new Config();
			config.version = GetVersion();
		}

		public static void LoadConfig (string pathToConfigFile) {
			config = Config.Deserialize(File.ReadAllText(pathToConfigFile));
		}

		public static void SaveConfig (string pathToConfigFile) {
			config.version = GetVersion();
			File.WriteAllText(pathToConfigFile, config.Serialize());
		}

		public static string GetVersion() {
			return FileVersionInfo.GetVersionInfo(Assembly.GetAssembly(typeof (Program)).Location).ProductVersion;
		}

	}

}
