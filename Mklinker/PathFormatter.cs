using Mklinker.Abstractions;
using System.IO.Abstractions;

namespace Mklinker {

	class PathFormatter : IPathFormatter {

		readonly IConfig config;

		public PathFormatter (IConfig config) {
			this.config = config;
		}

		public string GetFormattedPath(string unformattedPath) {
			return unformattedPath;
		}

	}

}
