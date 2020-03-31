using System.IO.Abstractions;
using Mklinker.Abstractions;

namespace Mklinker {

	interface IDefaultAction {

		void Execute(IConfigHandler configHandler, IFileSystem fileSystem);

	}

}
