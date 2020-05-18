using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Enumeration;

namespace Mklinker {

	class ConsoleHelpWriter : StringWriter {

		/**
		 * NOTE: Don't look too much into this messy function.
		 * All it does is set the color codes in the help screen
		 */
		public override void Write (string value) {
			// Colors Mklinker vX.X.X
			System.Console.ForegroundColor = ConsoleColor.Yellow;

			for (int i = 0; i < value.Length; i++) {
				if (value[i] == '\n') {
					// Want to color both error AND error message
					// Keeps color until carriage return (13) is found
					if (value.Length > i + 1) {
						if (value[i + 1] == 13) {
							System.Console.ResetColor ();
						}
					}

					if (value.Length > i + 6 && value.Substring (i, 6).Contains ("error", StringComparison.OrdinalIgnoreCase)) {
						// Error = Red
						System.Console.ForegroundColor = ConsoleColor.Red;
					} else if (value.Length > i + 10 && value.Substring (i, 10).Contains ("copyright", StringComparison.OrdinalIgnoreCase)) {
						// Header = Yellow
						System.Console.ForegroundColor = ConsoleColor.Yellow;
					} else if (value.Length > i + 9 && value.Substring (i, 9).Contains ("help", StringComparison.OrdinalIgnoreCase)) {
						// Help/Version text = White
						System.Console.ForegroundColor = ConsoleColor.White;
					} else if (value.Length > i + 12 && value.Substring (i, 12).Contains ("version", StringComparison.OrdinalIgnoreCase)) {
						// Help/Version text = White
						System.Console.ForegroundColor = ConsoleColor.White;
					}
				}

				System.Console.Out.Write (value[i]);
			}

			// Reset color back to default at the end
			System.Console.ResetColor ();
		}

	}

}
