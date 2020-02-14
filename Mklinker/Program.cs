using System;
using System.IO;

namespace Mklinker {

	public class Program {

		public static void Main(string[] args) {
			if (!File.Exists("linker.config")) {
				Console.WriteLine("Creating config file 'linker.config' since it does not exist");
				File.Create("linker.config");
			}

			ExecuteCommands(args);
		}

		public static void ExecuteCommands (string[] args) {

		}

	}

}
