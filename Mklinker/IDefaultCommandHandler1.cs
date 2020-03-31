using System.IO.Abstractions;
using Mklinker.Abstractions;

namespace Mklinker {

	interface IDefaultCommandHandler {

		void Execute(IConfigHandler configHandler, IFileSystem fileSystem);

	}

}
