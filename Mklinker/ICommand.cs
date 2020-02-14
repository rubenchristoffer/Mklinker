using System;
using System.Collections.Generic;
using System.Text;

namespace Mklinker {

	public interface ICommand {

		string GetName();
		void ExecuteCommand(string[] args);

	}

}
