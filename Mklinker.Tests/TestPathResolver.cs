using System;
using System.Collections.Generic;
using System.Text;
using Mklinker.Abstractions;

namespace Mklinker.Tests {

	class TestPathResolver : IPathResolver {

		public string GetAbsoluteResolvedPath(string unresolvedPath, IEnumerable<Variable> variables) {
			return unresolvedPath;
		}

	}

}
