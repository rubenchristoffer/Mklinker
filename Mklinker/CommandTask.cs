using System;

namespace Mklinker {

	public struct CommandTask {

		public ICommand command { get; private set; }
		public string[] args { get; private set; }

		public CommandTask(ICommand command, string[] args) {
			this.command = command;
			this.args = args;
		}

		public void ExecuteTask () {
			command.ExecuteCommand(args);
		}

	}

}