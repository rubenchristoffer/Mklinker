using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Mklinker.Abstractions;

namespace Mklinker {

	class Process : IProcess {

		readonly System.Diagnostics.Process process;

		StreamWriter IProcess.StandardInput => process.StandardInput;
		StreamReader IProcess.StandardOutput => process.StandardOutput;
		StreamReader IProcess.StandardError => process.StandardError;

		public Process () {}

		Process (System.Diagnostics.Process process) {
			this.process = process;
		}

		IProcess IProcess.Start(ProcessStartInfo processStartInfo) {
			return new Process(System.Diagnostics.Process.Start(processStartInfo));
		}

	}

}
