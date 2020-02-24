using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Mklinker.Commands;

namespace Mklinker {

	public class Program {

		public const string VERSION = "v1.1.1";

		public static Config config { get; private set; }

		private static List<CommandTask> commandTasks = new List<CommandTask>();

		private static ICommand[] availableCommands = new ICommand[] {
			new LinkAllCommand(),
			new AddLinkCommand(),
			new RemoveLinkCommand(),
			new ListCommand(),
			new ValidateCommand()
		};

		public static void Main(string[] args) {
			if (!File.Exists(Config.configFile)) {
				Console.WriteLine("\nCreating config file 'linker.config' since it does not exist");
				File.Create(Config.configFile).Close();
			} else if (args.Length == 0) {
				Console.WriteLine("\nNo valid arguments are provided and config file already exists");
			}

			config = Config.Deserialize(File.ReadAllText(Config.configFile));
			ParseCommands(args);
			ExecuteCommands();

			if (commandTasks.Count == 0) {
				Console.WriteLine("\nHere are the available commands:\n");

				foreach (ICommand command in availableCommands) {
					Console.WriteLine(command.GetName());
				}
			}

			SaveConfig();
		}

		public static bool IsCommand (string str) {
			return availableCommands.Any(c => c.GetName().ToLower().Equals(str));
		}

		public static ICommand GetCommand(string str) {
			return availableCommands.FirstOrDefault(c => c.GetName().ToLower().Equals(str));
		}

		public static void ParseCommands(string[] args) {
			List<int> commandIndexes = new List<int>();

			for (int i = 0; i < args.Length; i++) {
				if (IsCommand(args[i])) {
					commandIndexes.Add(i);
				}
			}

			for (int x = 0; x < commandIndexes.Count; x++) {
				int index = commandIndexes[x];
				int argumentAmount = 0;

				if (x == commandIndexes.Count - 1)
					argumentAmount = args.Length - index - 1;
				else
					argumentAmount = commandIndexes[x + 1] - index - 1;

				string[] commandArguments = new string[argumentAmount];
				Array.Copy(args, index + 1, commandArguments, 0, argumentAmount);

				commandTasks.Add(new CommandTask(GetCommand(args[index]), commandArguments));
			}
		}

		public static void ExecuteCommands() {
			commandTasks.ForEach(task => task.ExecuteTask());
		}

		public static void SaveConfig () {
			config.version = VERSION;
			File.WriteAllText(Config.configFile, config.Serialize());
		}

	}

}
