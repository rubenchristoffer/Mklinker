using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Text;
using System.Linq;
using CommandLine;
using Mklinker.Abstractions;

namespace Mklinker.Commands {

	[Verb("addvar", HelpText = "Adds a new variable to config file")]
	class AddVariableCommand : GlobalOptions, IDefaultCommandHandler {

		[Value(0, HelpText = "The name of the variable", Required = true)]
		public string name { get; private set; }

		[Value(1, HelpText = "The value of the variable", Required = true)]
		public string value { get; private set; }

		[Option('f', "force", Default = false, HelpText = "If this flag is set it will override existing variable if name already exists", Required = false)]
		public bool force { get; private set; }

		public AddVariableCommand() : base() {}

		public AddVariableCommand (string name, string value, bool force, string path) : base(path) {
			this.name = name;
			this.value = value;
			this.force = force;
		}

		public void Execute(IConsole console, IConfigHandler configHandler, IFileSystem fileSystem) {
			if (!configHandler.DoesConfigExist(path)) {
				console.WriteLine($"Config '{ path }' does not exist. Type 'help config' in order to see how you create a config file.");
				return;
			}

			IConfig config = configHandler.LoadConfig(path);
			Variable existingVariable = config.Variables.FirstOrDefault(variable => variable.name.Equals(name, StringComparison.OrdinalIgnoreCase));

			if (existingVariable == null) {
				config.Variables.Add (new Variable (name, value));
				console.WriteLine($"Added new variable '{ name }' with value '{ value }'");

				configHandler.SaveConfig(config, path);
			} else {
				if (!force) {
					console.WriteLine($"A variable with the name '{ name }' already exists");
				} else {
					existingVariable.value = value;
					console.WriteLine($"Replaced existing variable value for '{ name }' with '{ value }'");

					configHandler.SaveConfig(config, path);
				}
			}
		}

	}

}
