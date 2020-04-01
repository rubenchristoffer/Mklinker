using CommandLine;

namespace Mklinker {

	class GlobalOptions {

		[Option('p', "path", HelpText = "Specifies path to config file", Default = Program.DEFAULT_LINKER_PATH)]
		public string path { get; internal set; }

	}

}
