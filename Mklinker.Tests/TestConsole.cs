using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Mklinker.Abstractions;
using NUnit.Framework;

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
			TestContext.Write(text);
		}

		void IConsole.WriteLine(string text) {
			history.Append(text + "\n");
			TestContext.WriteLine(text);
		}

		void IConsole.WriteLine(string formattedText, params object[] args) {
			string text = String.Format(formattedText, args);

			history.Append(text);
			TestContext.Write(text);
		}

		public string GetHistory () {
			return history.ToString();
		}

	}

}
