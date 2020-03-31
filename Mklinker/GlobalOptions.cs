using CommandLine;

namespace Mklinker {

	class GlobalOptions {

		[Option('p', "path", HelpText = "Specifies path to config file", Default = "linker.config")]
		public string path { get; protected set; }

	}

}
