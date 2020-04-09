using System.Collections.Generic;

namespace Mklinker.Abstractions {

	interface IPathResolver {

		string GetAbsoluteResolvedPath(string unresolvedPath, IEnumerable<Variable> variables);

	}

}
