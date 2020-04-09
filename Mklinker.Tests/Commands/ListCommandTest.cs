using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Mklinker.Commands;
using Mklinker.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using Moq;

namespace Mklinker.Tests.Commands {

	[TestFixture]
	class ListCommandTest {

		TestConsole testConsole;
		Mock<IConfigHandler> testConfigHandler;
		Mock<IConfig> testConfig;
		MockFileSystem testFileSystem;
		List<ConfigLink> testLinks;
		List<Variable> testVariables;

		[SetUp]
		public void Setup () {
			testConsole = new TestConsole();
			testConfigHandler = new Mock<IConfigHandler>();
			testFileSystem = new MockFileSystem();

			testLinks = new List<ConfigLink>();
			testVariables = new List<Variable>();

			testConfig = new Mock<IConfig>();
			testConfig.Setup(m => m.LinkList).Returns(testLinks);
			testConfig.Setup(m => m.Variables).Returns(testVariables);
		}

		[Test]
		public void Execute_WithEmptyConfig_ShouldPrintConfigEmpty () {
			// Arrange
			IDefaultCommandHandler command = new ListCommand(false, "testpath");
			testConfigHandler.Setup(m => m.LoadConfig("testpath")).Returns(testConfig.Object);

			// Act
			command.Execute(testConsole, testConfigHandler.Object, testFileSystem);

			// Assert
			Assert.IsTrue(testConsole.GetHistory().Contains("empty", StringComparison.OrdinalIgnoreCase));
		}

		[Test]
		public void Execute_WithConfigLinks_ShouldPrintAllCases() {
			// Arrange
			IDefaultCommandHandler command = new ListCommand(false, "testpath");

			testLinks.Add(new ConfigLink("source", "target", ConfigLink.LinkType.Default));
			testLinks.Add(new ConfigLink("testing is fun", "not really", ConfigLink.LinkType.Hard));

			testConfigHandler.Setup(m => m.LoadConfig("testpath")).Returns(testConfig.Object);

			// Act
			command.Execute(testConsole, testConfigHandler.Object, testFileSystem);

			// Assert
			testLinks.ForEach(link => Assert.IsTrue(testConsole.GetHistory().Contains(link.ToString(), StringComparison.OrdinalIgnoreCase)));
		}

		[Test]
		public void Execute_WithVariables_ShouldPrintAllCases() {
			// Arrange
			IDefaultCommandHandler command = new ListCommand(true, "testpath");

			testVariables.Add(new Variable("var", "value"));
			testVariables.Add(new Variable("othervar", "othervalue"));

			testConfigHandler.Setup(m => m.LoadConfig("testpath")).Returns(testConfig.Object);

			// Act
			command.Execute(testConsole, testConfigHandler.Object, testFileSystem);

			// Assert
			testVariables.ForEach(variable => Assert.IsTrue(testConsole.GetHistory().Contains(variable.ToString(), StringComparison.OrdinalIgnoreCase)));
		}

	}

}
