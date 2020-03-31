using System;
using System.Collections.Generic;
using System.Text;

namespace Mklinker {

	interface IConfigHandler {

		IConfig LoadConfig(string pathToConfigFile);
		void SaveConfig(IConfig config, string pathToConfigFile);

	}

}
