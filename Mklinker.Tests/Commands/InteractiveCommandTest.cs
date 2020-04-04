	using NUnit.Framework;
using Mklinker.Commands;
using Mklinker.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using Moq;

namespace Mklinker.Tests.Commands {

	[TestFixture]
	class InteractiveCommandTest {

		TestConsole testConsole;
		TestArgumentParser testArgumentParser;
		Mock<ICommandExecutor> testCommandExecutor;

		[SetUp]
		public void Setup() {
			testConsole = new TestConsole();
			testArgumentParser = new TestArgumentParser();
			testCommandExecutor = new Mock<ICommandExecutor>();
		}

		[Test]
		public void Execute_WithExitString_ShouldExit () {
			// Arrange
			InteractiveCommand command = new InteractiveCommand();
			testConsole.ReadLineText = new string[] { "exit" };

			// Act
			command.Execute(testConsole, testArgumentParser, testCommandExecutor.Object);

			// Assert
			Assert.Pass();
		}

		[Test]
		public void Execute_WithCommandString_ShouldExecuteCommand() {
			// Arrange
			InteractiveCommand command = new InteractiveCommand();
			testConsole.ReadLineText = new string[] { "command arg1 arg2", "exit" };

			// Act
			command.Execute(testConsole, testArgumentParser, testCommandExecutor.Object);

			// Assert
			testCommandExecutor.Verify(m => m.Execute(new string[] { "command", "arg1", "arg2" }));
		}

	}

}
