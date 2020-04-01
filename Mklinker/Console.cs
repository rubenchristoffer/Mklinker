using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Mklinker.Abstractions;

namespace Mklinker {

	class Console : IConsole {

		TextWriter IConsole.Writer => System.Console.Out;

		string IConsole.ReadLine() {
			return System.Console.ReadLine();
		}

		void IConsole.Write(string text) {
			System.Console.Write(text);
		}

		void IConsole.WriteLine(string line) {
			System.Console.WriteLine(line);
		}

		void IConsole.WriteLine(string formattedLine, params object[] args) {
			System.Console.WriteLine(formattedLine, args);
		}
		
	}

}
