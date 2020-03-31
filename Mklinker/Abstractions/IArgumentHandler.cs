using System;
using System.Collections.Generic;
using System.Text;

namespace Mklinker.Abstractions {

	interface IArgumentParser {

		string[] ParseStringToArguments(string input);

	}

}
