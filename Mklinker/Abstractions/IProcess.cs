using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace Mklinker.Abstractions {

	interface IProcess {

		IProcess Start(ProcessStartInfo processStartInfo);

		StreamReader StandardOutput { get; }
		StreamReader StandardError { get; }
		StreamWriter StandardInput { get; }

	}

}
