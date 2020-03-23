using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace Mklinker.Commands {

	[Verb("config", HelpText = "Displays information about config file")]
	public class ConfigCommand : IDefaultAction {

		[Option ('c', "create", HelpText = "Creates a new config file if it does not exist", SetName = "Create")]
		public bool create { get; private set; }

		[Option('d', "delete", HelpText = "Deleted config file if it exists", SetName = "Delete")]
		public bool delete { get; private set; }
		
		[Option('p', "path", HelpText = "Specifies path to config file")]
		public string path { get; private set; }

		public void Execute() {

		}

	}

}
