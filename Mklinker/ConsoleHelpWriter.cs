using System.IO;
using System.Linq;

namespace Mklinker {

	class ConsoleHelpWriter : StringWriter {

		public override void Write (string value) {
			string[] lines = value.Split (System.Console.Out.NewLine);

			for (int i = 0; i < lines.Length - 1; i++) {
				WriteLine (lines[i]);
			}
		}

		public override void WriteLine (string value) {
			// Use prefixes to add color coding to certain lines
			string[] yellowPrefixes = { "Mklinker", "Copyright" };
			string[] whitePrefixes = { "help", "--help", "version", "--version" };
			string[] redPrefixes = { "ERROR", "Verb", "Option", "A required" };

			if (yellowPrefixes.Any (p => value.Trim ().StartsWith (p))) {
				System.Console.ForegroundColor = System.ConsoleColor.Yellow;
			}

			if (whitePrefixes.Any (p => value.Trim ().StartsWith (p))) {
				System.Console.ForegroundColor = System.ConsoleColor.White;
			}

			if (redPrefixes.Any (p => value.Trim ().StartsWith (p))) {
				System.Console.ForegroundColor = System.ConsoleColor.Red;
			}

			System.Console.Out.WriteLine (value);
			System.Console.ResetColor ();
		}

	}

}
