using System;
using System.Collections.Generic;
using System.Text;

namespace Mklinker {

	interface IArgumentHandler {

		string[] ParseStringToArguments(string input);

	}

}
