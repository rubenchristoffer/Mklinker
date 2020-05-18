using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Text;
using System.Linq;
using CommandLine;
using Mklinker.Abstractions;

namespace Mklinker.Commands {

	[Verb("removevar", HelpText = "Removes a variable from config file")]
	class RemoveVariableCommand : GlobalOptions, IDefaultCommandHandler {

		[Value(0, HelpText = "The name of the variable", Required = true)]
		public string name { get; private set; }

		public RemoveVariableCommand() : base() {}

		public RemoveVariableCommand(string name, string path) : base(path) {
			this.name = name;
		}

		public void Execute(IConsole console, IConfigHandler configHandler, IFileSystem fileSystem) {
			if (!configHandler.DoesConfigExist(path)) {
				console.WriteLine($"Config '{ path }' does not exist. Type 'help config' in order to see how you create a config file.", IConsole.ContentType.Negative);
				return;
			}

			IConfig config = configHandler.LoadConfig(path);
			Variable existingVariable = config.Variables.FirstOrDefault(variable => variable.name.Equals(name, StringComparison.OrdinalIgnoreCase));

			if (existingVariable == null) {
				console.WriteLine($"A variable with name '{ name }' does not exist", IConsole.ContentType.Negative);
			} else {
				config.Variables.Remove(existingVariable);
				configHandler.SaveConfig(config, path);

				console.WriteLine($"Variable with name '{ name }' has been removed");
			}
		}

	}

}
