using System;
using System.Linq;
using Mklinker.Abstractions;
using System.IO.Abstractions;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Mklinker {

	class PathResolver : IPathResolver {

		public const string delimiter = "?";
		public static readonly string regex = $@"{ Regex.Escape(delimiter) }[\s\S]*{ Regex.Escape(delimiter) }";

		readonly IFileSystem fileSystem;

		public PathResolver (IFileSystem fileSystem) {
			this.fileSystem = fileSystem;
		} 

		public string GetAbsoluteResolvedPath(string unresolvedPath, IEnumerable<Variable> variables) {
			string resolvedPath = unresolvedPath;

			Match match;

			while ((match = Regex.Match(resolvedPath, regex)).Success) {
				string name = match.Value;
				Variable matchedVariables = variables.FirstOrDefault(variable => (delimiter + variable.name + delimiter).Equals(name, StringComparison.OrdinalIgnoreCase));

				if (matchedVariables == null)
					break;

				resolvedPath = resolvedPath.Replace(name, matchedVariables.value, StringComparison.OrdinalIgnoreCase);
			}

			return fileSystem.Path.GetFullPath(resolvedPath.Replace(@"\", "/")).Replace(@"\", "/");
		}

	}

}
