using System;
using System.Linq;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using NUnit.Framework;
using Mklinker.Commands;
using Mklinker.Abstractions;
using Moq;

namespace Mklinker.Tests {

	[TestFixture]
	class CommandExecutorTest {

		TestConsole testConsole;
		MockFileSystem testFileSystem;
		Mock<IConfigHandler> testConfigHandler;
		Mock<IConfig> testConfig;
		Mock<IArgumentParser> testArgumentParser;
		Mock<ILinker> testLinker;

		[SetUp]
		public void Setup() {
			testConsole = new TestConsole();
			testFileSystem = new MockFileSystem();
			testConfigHandler = new Mock<IConfigHandler>();
			testConfig = new Mock<IConfig>();
			testArgumentParser = new Mock<IArgumentParser>();
			testLinker = new Mock<ILinker>();
		}

		[Test]
		public void Execute_WithInvalidCommand_WillPrintHelp() {
			// Arrange
			ICommandExecutor commandExecutor = new CommandExecutor(testConsole, testConfigHandler.Object, testFileSystem, testConfig.Object, testArgumentParser.Object, testLinker.Object);

			// Act
			commandExecutor.Execute("invalidcommand");

			// Assert
			Assert.IsTrue(testConsole.GetHistory().Contains("help", StringComparison.OrdinalIgnoreCase));
		}

		[Test]
		public void Execute_WithHelpCommand_WillPrintHelp() {
			// Arrange
			ICommandExecutor commandExecutor = new CommandExecutor(testConsole, testConfigHandler.Object, testFileSystem, testConfig.Object, testArgumentParser.Object, testLinker.Object);

			// Act
			commandExecutor.Execute("help");

			// Assert
			Assert.IsTrue(testConsole.GetHistory().Contains("help", StringComparison.OrdinalIgnoreCase));
		}

		[Test]
		public void Execute_WithVersionCommand_WillPrintVersion() {
			// Arrange
			ICommandExecutor commandExecutor = new CommandExecutor(testConsole, testConfigHandler.Object, testFileSystem, testConfig.Object, testArgumentParser.Object, testLinker.Object);

			// Act
			commandExecutor.Execute("version");

			// Assert
			Assert.IsTrue(testConsole.GetHistory().Contains("testhost", StringComparison.OrdinalIgnoreCase));
		}

	}

}
