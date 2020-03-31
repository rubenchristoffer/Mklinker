using System;
using System.Collections.Generic;
using System.Text;

namespace Mklinker.Abstractions {

	interface IConsole {

		void WriteLine(string line);
		string ReadLine();

	}

}
