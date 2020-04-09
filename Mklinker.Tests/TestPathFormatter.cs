using System;
using System.Collections.Generic;
using System.Text;
using Mklinker.Abstractions;

namespace Mklinker.Tests {

	class TestPathFormatter : IPathFormatter {

		public string GetFormattedPath(string unformattedPath) {
			return unformattedPath;
		}

	}

}
