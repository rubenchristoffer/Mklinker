using System;
using System.Linq;
using Mklinker.Abstractions;
using System.IO.Abstractions;

namespace Mklinker {

	class PathFormatter : IPathFormatter {

		public const string delimiter = "?";

		readonly IConfig config;

		public PathFormatter (IConfig config) {
			this.config = config;
		}

		public string GetFormattedPath(string unformattedPath) {
			string formattedPath = unformattedPath;

			config.Variables.ForEach(variable => formattedPath = formattedPath.Replace($"{ delimiter }{ variable.name }{ delimiter }", variable.value));

			return formattedPath;
		}

	}

}
