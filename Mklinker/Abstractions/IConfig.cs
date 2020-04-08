using System.Collections.Generic;

namespace Mklinker.Abstractions {

	public interface IConfig {

		string Version { get; }

		List<ConfigLink> LinkList { get; }
		List<Variable> Variables { get; }

	}

}