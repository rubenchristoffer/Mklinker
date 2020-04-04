using CommandLine;
using Mklinker.Abstractions;

namespace Mklinker.Commands {

	[Verb("interactive", HelpText = "Starts an interactive session where you can run multiple commands without 'Mklinker' in front. Use 'exit' to exit from interactive session")]
	class InteractiveCommand {

		internal void Execute(IConsole console, IArgumentParser argumentHandler, ICommandExecutor commandExecutor) {
			bool finished = false;

			while (!finished) {
				console.Write("> ");
				string input = console.ReadLine().Trim();

				if (input.ToLower().Equals("exit")) {
					finished = true;
				} else {
					commandExecutor.Execute(argumentHandler.ParseStringToArguments(input));
				}
			}
		}

	}

}
