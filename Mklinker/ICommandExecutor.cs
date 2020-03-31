using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Abstractions;

namespace Mklinker {

	interface ICommandExecutor {

		void Execute(string[] args, IConfigHandler configHandler, IFileSystem fileSystem, IConfig defaultConfig, IArgumentHandler argumentHandler);

	}

}
