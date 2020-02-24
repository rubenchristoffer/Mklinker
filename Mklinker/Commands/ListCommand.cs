using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mklinker.Commands {

	public class ListCommand : ICommand {

		public void ExecuteCommand (string[] args) {
			Program.config.linkList.ForEach(link => Console.WriteLine("\n" + link.ToString()));
		}

		public string GetName() {
			return "List";
		}

	}

}
