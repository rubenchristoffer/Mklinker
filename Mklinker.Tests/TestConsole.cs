﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Mklinker.Abstractions;
using NUnit.Framework;
using System.Linq;

namespace Mklinker.Tests {

	class TestConsole : IConsole {

		StringBuilder history;

		public TextWriter Writer { get; private set; }

		public int CurrentReadLineIndex { get; set; } = -1;
		public string[] ReadLineText { get; set; }
		public bool ShouldRecordHistory { get; set; } = true;

		public TestConsole(string[] readLineText = null) {
			this.history = new StringBuilder();
			Writer = new StringWriter(history);
			ReadLineText = readLineText;
		}

		string IConsole.ReadLine() {
			CurrentReadLineIndex++;

			if (ReadLineText == null || CurrentReadLineIndex >= ReadLineText.Length)
				return "";

			return ReadLineText[CurrentReadLineIndex];
		}

		void IConsole.Write(string text, IConsole.ContentType contentType) {
			if (ShouldRecordHistory) {
				history.Append(text);
				TestContext.Write(text);
			}
		}

		void IConsole.WriteLine(string text, IConsole.ContentType contentType) {
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
