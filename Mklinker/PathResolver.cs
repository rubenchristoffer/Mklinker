using System;
using System.Linq;
using Mklinker.Abstractions;
using System.IO.Abstractions;
using System.Collections.Generic;

namespace Mklinker {

	class PathResolver : IPathResolver {

		public const string delimiter = "?";

		readonly IFileSystem fileSystem;

		public PathResolver (IFileSystem fileSystem) {
			this.fileSystem = fileSystem;
		} 

		public string GetAbsoluteResolvedPath(string unresolvedPath, IEnumerable<Variable> variables) {
			string resolvedPath = unresolvedPath;

			foreach (Variable variable in variables) {
				resolvedPath = resolvedPath.Replace($"{ delimiter }{ variable.name }{ delimiter }", variable.value);
			}

			return fileSystem.Path.GetFullPath(resolvedPath);
		}

	}

}
