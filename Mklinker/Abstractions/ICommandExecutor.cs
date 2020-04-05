using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Abstractions;

namespace Mklinker.Abstractions {

	interface ICommandExecutor {

		void Execute(params string[] args);

	}

}
