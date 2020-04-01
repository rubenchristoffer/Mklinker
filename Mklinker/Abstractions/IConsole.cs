using System.IO;

namespace Mklinker.Abstractions {

	interface IConsole {

		TextWriter Writer { get; }

		void Write(string text = "");
		void WriteLine(string text = "");
		void WriteLine(string formattedText, params object[] args);

		string ReadLine();

	}

}
