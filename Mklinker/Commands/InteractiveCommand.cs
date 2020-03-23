using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace Mklinker.Commands {

	[Verb("interactive", HelpText = "Starts an interactive session where you can run multiple commands without 'Mklinker' in front. Use 'exit' to exit from interactive session.")]
	public class InteractiveCommand : IDefaultAction {

		public void Execute() {
			bool finished = false;

			while (!finished) {
				Console.Write("> ");
				string input = Console.ReadLine().Trim();

				if (input.ToLower().Equals("exit")) {
					finished = true;
				} else {
					Program.ParseAndExecute(input.Split(' '));
				}
			}
		}

	}

}
