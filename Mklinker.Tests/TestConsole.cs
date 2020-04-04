using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Mklinker.Abstractions;
using NUnit.Framework;
using System.Linq;

namespace Mklinker.Tests {

	class TestConsole : IConsole {

		StringBuilder history;

		TextWriter IConsole.Writer => null;

		public int CurrentReadLineIndex { get; set; }
		public string[] ReadLineText { get; set; }
		public bool ShouldRecordHistory { get; set; } = true;

		public TestConsole(string[] readLineText = null) {
			this.history = new StringBuilder();

			ReadLineText = readLineText;
			CurrentReadLineIndex = -1;
		}

		string IConsole.ReadLine() {
			CurrentReadLineIndex++;

			if (ReadLineText == null || CurrentReadLineIndex >= ReadLineText.Length)
				return "";

			return ReadLineText[CurrentReadLineIndex];
		}

		void IConsole.Write(string text) {
			if (ShouldRecordHistory) {
				history.Append(text);
				TestContext.Write(text);
			}
		}

		void IConsole.WriteLine(string text) {
			if (ShouldRecordHistory) {
				history.Append(text + "\n");
				TestContext.WriteLine(text);
			}
		}

		void IConsole.WriteLine(string formattedText, params object[] args) {
			if (ShouldRecordHistory) {
				string text = String.Format(formattedText, args);

				history.Append(text);
				TestContext.Write(text);
			}
		}

		public string GetHistory () {
			return history.ToString();
		}

	}

}
