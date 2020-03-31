using System;
using System.Collections.Generic;
using System.Text;

namespace Mklinker.Abstractions {

	interface IConfigHandler {

		IConfig LoadConfig(string pathToConfigFile);
		void SaveConfig(IConfig config, string pathToConfigFile);
		void DeleteConfig(string pathToConfigFile);
		bool DoesConfigExist(string pathToConfigFile);

	}

}
