using System.IO.Abstractions;
using Mklinker.Abstractions;

namespace Mklinker.Abstractions {

	interface IDefaultCommandHandler {

		void Execute(IConsole console, IConfigHandler configHandler, IFileSystem fileSystem);

	}

}
