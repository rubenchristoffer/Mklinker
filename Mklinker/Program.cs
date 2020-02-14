using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Mklinker.Commands;

namespace Mklinker {

	public class Program {

		public const string configFile = "linker.config";
		public static string[] config { get; private set; }

		private static List<CommandTask> commandTasks = new List<CommandTask>();

		private static ICommand[] availableCommands = new ICommand[] {
			new BuildCommand(),
			new AddLinkCommand()
		};

		public static void Main(string[] args) {
			if (!File.Exists(configFile)) {
				Console.WriteLine("Creating config file 'linker.config' since it does not exist");
				File.Create(configFile);
			} else if (args.Length == 0) {
				Console.WriteLine("No arguments are provided and config file already exists");
			}

			config = File.ReadAllLines(configFile);
			ParseCommands(args);
			ExecuteCommands();
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

	}

}
