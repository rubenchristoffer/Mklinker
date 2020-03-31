using System;
using System.IO.Abstractions;
using CommandLine;
using Mklinker.Abstractions;

namespace Mklinker.Commands {

	[Verb("interactive", HelpText = "Starts an interactive session where you can run multiple commands without 'Mklinker' in front. Use 'exit' to exit from interactive session")]
	class InteractiveCommand {

		internal void Execute(IConfigHandler configHandler, IFileSystem fileSystem, IConfig defaultConfig, IArgumentParser argumentHandler, ICommandExecutor commandExecutor) {
			bool finished = false;

			while (!finished) {
				Console.Write("> ");
				string input = Console.ReadLine().Trim();

				if (input.ToLower().Equals("exit")) {
					finished = true;
				} else {
					commandExecutor.Execute(argumentHandler.ParseStringToArguments(input));
				}
			}
		}

	}

}
