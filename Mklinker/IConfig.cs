using System.Collections.Generic;

namespace Mklinker {

	interface IConfig {

		string Version { get; }

		List<ConfigLink> LinkList { get; }
		List<Variable> Variables { get; }

		IConfig Deserialize(string xml);
		string Serialize();

	}

}