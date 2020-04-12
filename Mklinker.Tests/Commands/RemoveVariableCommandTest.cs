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
	class RemoveVariableCommandTest {

		TestConsole testConsole;
		Mock<IConfigHandler> testConfigHandler;
		MockFileSystem testFileSystem;
		Mock<IConfig> testConfig;
		List<Variable> testVariables;

		[SetUp]
		public void Setup() {
			testConsole = new TestConsole();

			testFileSystem = new MockFileSystem(new Dictionary<string, MockFileData> {
				{ @"c:\config.linker", new MockFileData("<?xml version=\"1.0\" encoding=\"utf-16\"?><Config xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" Version=\"v1.1.1\"></Config>") },
				{ @"c:\invalidconfig.linker", new MockFileData("This content is invalid for a linker file!") },
				{ @"c:\demo\jQuery.js", new MockFileData("some js") },
				{ @"c:\demo\", new MockDirectoryData () },
				{ @"c:\demo\image.gif", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) }
			});

			testConfig = new Mock<IConfig>();
			testVariables = new List<Variable>();
			testConfig.Setup(m => m.Variables).Returns(testVariables);

			testConfigHandler = new Mock<IConfigHandler>();
			testConfigHandler.Setup(m => m.DoesConfigExist("config")).Returns(true);
			testConfigHandler.Setup(m => m.LoadConfig("config")).Returns(testConfig.Object);
		}

		[Test]
		public void Execute_WithExistingVariable_WillRemoveVariable() {
			// Arrange
			testVariables.Add(new Variable("var", "value"));
			RemoveVariableCommand command = new RemoveVariableCommand("var", "config");

			// Act
			command.Execute(testConsole, testConfigHandler.Object, testFileSystem);

			// Assert
			Assert.IsFalse(testConfigHandler.Object.LoadConfig("config").Variables
				.Any(variable => variable.name.Equals("var")));
		}

		[Test]
		public void Execute_WithNoExistingVariable_WillShowError () {
			// Arrange
			RemoveVariableCommand command = new RemoveVariableCommand("var", "config");

			// Act
			command.Execute(testConsole, testConfigHandler.Object, testFileSystem);

			// Assert
			Assert.IsTrue(testConsole.GetHistory().Contains("does not exist", StringComparison.OrdinalIgnoreCase));
		}

	}

}