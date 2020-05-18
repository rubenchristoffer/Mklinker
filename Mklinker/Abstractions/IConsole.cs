using System.IO;

namespace Mklinker.Abstractions {

	interface IConsole {

		TextWriter Writer { get; }

		public enum ContentType {
			Ordinary,
			Negative,
			Positive,
			Header
		}

		void Write(string text = "", ContentType contentType = ContentType.Ordinary);
		void WriteLine(string text = "", ContentType contentType = ContentType.Ordinary);
		void WriteLine(string formattedText, params object[] args);

		string ReadLine();

	}

}
