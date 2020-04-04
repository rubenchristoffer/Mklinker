using NUnit.Framework;
using Mklinker.Commands;
using Mklinker.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using Moq;

namespace Mklinker.Tests.Commands {

	[TestFixture]
	class InteractiveCommandTest {

		TestConsole testConsole;
		IConfig testDefaultConfig;
		IConfigHandler testConfigHandler;
		MockFileSystem testFileSystem;
		TestArgumentParser testArgumentParser;
		ICommandExecutor testCommandExecutor;

		[SetUp]
		public void Setup () {
			testConsole = new TestConsole();
			testDefaultConfig = new Mock<IConfig>().Object;
			testConfigHandler = new Mock<IConfigHandler>().Object;
			testFileSystem = new MockFileSystem();
			testArgumentParser = new TestArgumentParser();

			var testLinker = new Mock<ILinker>();

			testCommandExecutor = new CommandExecutor(testConsole, testConfigHandler, testFileSystem, testDefaultConfig, testArgumentParser, testLinker.Object);
		}

		[Test]
		public void Execute () {
			// Arrange
			InteractiveCommand command = new InteractiveCommand();
			testConsole.ReadLineText = "exit ";

			// Act
			command.Execute(testConsole, testConfigHandler, testFileSystem, testDefaultConfig, testArgumentParser, testCommandExecutor);

			// Assert
			Assert.Pass();
		}

	}

}
