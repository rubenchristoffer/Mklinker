using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Mklinker.Abstractions;

namespace Mklinker {

	class Console : IConsole {

		TextWriter IConsole.Writer => new ConsoleHelpWriter();

		string IConsole.ReadLine() {
			return System.Console.ReadLine();
		}

		void IConsole.Write(string text, IConsole.ContentType contentType) {
			SetColor (contentType);

			System.Console.Write(text);

			System.Console.ResetColor ();
		}

		void IConsole.WriteLine(string line, IConsole.ContentType contentType) {
			SetColor (contentType);

			System.Console.WriteLine(line);

			System.Console.ResetColor ();
		}

		void IConsole.WriteLine(string formattedLine, params object[] args) {
			System.Console.WriteLine(formattedLine, args);
		}

		void SetColor(IConsole.ContentType contentType) {
			switch (contentType) {
				case IConsole.ContentType.Positive:
					System.Console.ForegroundColor = ConsoleColor.Green;
					break;

				case IConsole.ContentType.Negative:
					System.Console.ForegroundColor = ConsoleColor.Red;
					break;

				case IConsole.ContentType.Header:
					System.Console.ForegroundColor = ConsoleColor.Yellow;
					break;
			}
		}

	}

}
