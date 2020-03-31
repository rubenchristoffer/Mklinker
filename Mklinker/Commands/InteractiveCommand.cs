using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommandLine;

namespace Mklinker.Commands {

	[Verb("interactive", HelpText = "Starts an interactive session where you can run multiple commands without 'Mklinker' in front. Use 'exit' to exit from interactive session")]
	class InteractiveCommand {

		internal void Execute(IConfigHandler configHandler, IFileSystem fileSystem, IConfig defaultConfig, IArgumentHandler argumentHandler, ICommandExecutor commandExecutor) {
			bool finished = false;

			while (!finished) {
				Console.Write("> ");
				string input = Console.ReadLine().Trim();

				if (input.ToLower().Equals("exit")) {
					finished = true;
				} else {
					commandExecutor.Execute(argumentHandler.ParseStringToArguments(input), configHandler, fileSystem, defaultConfig, argumentHandler);
				}
			}
		}

	}

}
