using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace Mklinker {

	public class GlobalOptions {

		[Option('p', "path", HelpText = "Specifies path to config file", Default = Config.DEFAULT_CONFIG_FILE)]
		public string path { get; protected set; }

	}

}
