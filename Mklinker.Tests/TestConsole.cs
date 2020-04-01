using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Mklinker.Abstractions;

namespace Mklinker.Tests {

	class TestConsole : IConsole {

		StringBuilder history;
		string readLineText;

		TextWriter IConsole.Writer => null;

		public TestConsole (string readLineText) {
			this.history = new StringBuilder();
			this.readLineText = readLineText;
		}

		string IConsole.ReadLine() {
			return readLineText;
		}

		void IConsole.Write(string text) {
			history.Append(text);
		}

		void IConsole.WriteLine(string text) {
			history.Append(text + "\n");
		}

		void IConsole.WriteLine(string formattedText, params object[] args) {
			history.Append(String.Format(formattedText, args));
		}

		public string GetHistory () {
			return history.ToString();
		}

	}

}
