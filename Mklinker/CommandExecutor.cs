using System;
using System.IO.Abstractions;
using CommandLine;
using Mklinker.Commands;

namespace Mklinker {

	class CommandExecutor : ICommandExecutor {

		readonly IConfigHandler configHandler;
		readonly IFileSystem fileSystem;
		readonly IConfig defaultConfig;
		readonly IArgumentHandler argumentHandler;

		public CommandExecutor (IConfigHandler configHandler, IFileSystem fileSystem, IConfig defaultConfig, IArgumentHandler argumentHandler) {
			this.configHandler = configHandler;
			this.fileSystem = fileSystem;
			this.defaultConfig = defaultConfig;
			this.argumentHandler = argumentHandler;
		}

		void ICommandExecutor.Execute(string[] args) {
			// Parse commands
			var parser = new Parser(with => with.HelpWriter = Console.Out);
			var parserResult = parser.ParseArguments<AddLinkCommand, LinkAllCommand, ListCommand, RemoveLinkCommand, ValidateCommand, InteractiveCommand, ConfigCommand>(args);

			parserResult
				.WithParsed<IDefaultAction>(flag => flag.Execute(configHandler, fileSystem))
				.WithParsed<ConfigCommand>(cmd => cmd.Execute(configHandler, fileSystem, defaultConfig))
				.WithParsed<InteractiveCommand>(cmd => cmd.Execute (configHandler, fileSystem, defaultConfig, argumentHandler, this));
		}

	}

}
