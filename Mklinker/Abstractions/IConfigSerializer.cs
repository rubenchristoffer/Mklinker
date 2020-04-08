using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Mklinker.Abstractions {

	interface IConfigSerializer {

		string Serialize(IConfig config);
		IConfig Deserialize(string serializedString);

	}

}