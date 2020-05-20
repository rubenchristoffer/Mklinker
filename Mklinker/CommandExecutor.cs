using System;
using System.IO.Abstractions;
using CommandLine;
using Mklinker.Abstractions;
using Mklinker.Commands;

namespace Mklinker {

	class CommandExecutor : ICommandExecutor {

		readonly IConsole console;
		readonly IConfigHandler configHandler;
		readonly IFileSystem fileSystem;
		readonly IConfig defaultConfig;
		readonly IArgumentParser argumentHandler;
		readonly ILinker linker;
		readonly IPathResolver pathResolver;

		public CommandExecutor (IConsole console, IConfigHandler configHandler, IFileSystem fileSystem, IConfig defaultConfig, IArgumentParser argumentHandler, ILinker linker, IPathResolver pathResolver) {
			this.console = console;
			this.configHandler = configHandler;
			this.fileSystem = fileSystem;
			this.defaultConfig = defaultConfig;
			this.argumentHandler = argumentHandler;
			this.linker = linker;
			this.pathResolver = pathResolver;
		}

		void ICommandExecutor.Execute(params string[] args) {
			// Parse commands
			var parser = new Parser(with => with.HelpWriter = console.Writer);
			var parserResult = parser.ParseArguments<AddLinkCommand, AddLinksCommand, AddVariableCommand, RemoveLinkCommand, RemoveVariableCommand, LinkAllCommand, ListCommand, ConfigCommand, ValidateCommand, InteractiveCommand, DetectCommand>(args);

			parserResult
				.WithParsed<IDefaultCommandHandler>(flag => flag.Execute(console, configHandler, fileSystem))
				.WithParsed<ConfigCommand>(cmd => cmd.Execute(console, configHandler, defaultConfig, pathResolver))
				.WithParsed<InteractiveCommand>(cmd => cmd.Execute (console, argumentHandler, this))
				.WithParsed<LinkAllCommand>(cmd => cmd.Execute (console, configHandler, fileSystem, linker, pathResolver))
				.WithParsed<AddLinkCommand>(cmd => cmd.Execute (console, configHandler, fileSystem, pathResolver))
				.WithParsed<ValidateCommand>(cmd => cmd.Execute(console, configHandler, fileSystem, pathResolver))
				.WithParsed<ListCommand>(cmd => cmd.Execute (console, configHandler, fileSystem, pathResolver))
				.WithParsed<AddLinksCommand>(cmd => cmd.Execute(console, configHandler, fileSystem, pathResolver))
				.WithParsed<DetectCommand>(cmd => cmd.Execute (console, fileSystem, pathResolver));
		}

	}

}
