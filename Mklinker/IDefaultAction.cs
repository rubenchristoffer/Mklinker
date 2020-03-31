using System.IO.Abstractions;

namespace Mklinker {

	interface IDefaultAction {

		void Execute(IConfigHandler configHandler, IFileSystem fileSystem);

	}

}
