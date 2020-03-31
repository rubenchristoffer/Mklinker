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

	public class Program {

		private static IContainer Container { get; set; }

		Program (string[] args) {
			var builder = new ContainerBuilder();

			builder.RegisterType<FileSystem>().As<IFileSystem>();
			builder.RegisterType<ConfigHandler>().As<IConfigHandler>();
			builder.RegisterType<Config>().As<IConfig>();

			Container = builder.Build();

			// Parse commands
			var parser = new Parser(with => with.HelpWriter = Console.Out);
			var parserResult = parser.ParseArguments<AddLinkCommand, LinkAllCommand, ListCommand, RemoveLinkCommand, ValidateCommand, InteractiveCommand, ConfigCommand>(args);

			using (var scope = Container.BeginLifetimeScope()) {
				parserResult
					.WithParsed<IDefaultAction>(flag => flag.Execute(scope.Resolve<IConfigHandler>(), scope.Resolve<IFileSystem>()))
					.WithParsed<ConfigCommand>(cmd => cmd.Execute (scope.Resolve<IConfigHandler>(), scope.Resolve<IFileSystem>(), scope.Resolve<IConfig>()));
			}
		}

		public static string GetVersion() {
			return FileVersionInfo.GetVersionInfo(Assembly.GetAssembly(typeof (Program)).Location).ProductVersion;
		}

		public static void Main(string[] args) {
			new Program(args);
		}

	}

}
