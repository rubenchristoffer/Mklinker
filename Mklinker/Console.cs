using System;
using System.Collections.Generic;
using System.Text;
using Mklinker.Abstractions;

namespace Mklinker {

	class Console : IConsole {

		string IConsole.ReadLine() {
			return System.Console.ReadLine();
		}

		void IConsole.WriteLine(string line) {
			System.Console.WriteLine(line);
		}

	}

}
