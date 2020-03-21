using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mklinker.Commands {

	public class AddVariableCommand {

		public void ExecuteCommand(string[] args) {
			if (args.Length != 2) {
				Console.WriteLine("");
				return;
			}
		}

		public string GetName() {
			return "AddVariable";
		}

	}

}
