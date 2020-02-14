using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Mklinker {

	public class Program {

		public const string configFile = "linker.config";

		private static string[] config;
		private static readonly Type commandType = typeof(Command);

		private static Dictionary<Command, string[]> parsedCommands = new Dictionary<Command, string[]>();

		public enum Command {
			Build,
			AddLink
		}

		public static void Main(string[] args) {
			if (!File.Exists(configFile)) {
				Console.WriteLine("Creating config file 'linker.config' since it does not exist");
				File.Create(configFile);
			} else if (args.Length == 0) {
				Console.WriteLine("No arguments are provided and config file already exists");
			}

			config = File.ReadAllLines(configFile);
			ParseCommands(args);
			//ExecuteCommands(args);
		}

		public static bool IsCommand (string str) {
			return Enum.GetNames(commandType).Any(c => c.ToLower().Equals(str.ToLower()));
		}

		public static Command GetCommand(string str) {
			return (Command)Enum.Parse(commandType, str, true);
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

				parsedCommands.Add(GetCommand(args[index]), commandArguments);
			}
		}

		public static void ExecuteCommands () {
			/*foreach (string arg in args) {
				
			}*/
		}

	}

}
