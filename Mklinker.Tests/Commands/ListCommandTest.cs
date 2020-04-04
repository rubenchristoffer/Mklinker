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

		[SetUp]
		public void Setup () {
			testConsole = new TestConsole();
			testConfigHandler = new Mock<IConfigHandler>();
			testFileSystem = new MockFileSystem();

			testLinks = new List<ConfigLink>();
			testConfig = new Mock<IConfig>();
			testConfig.Setup(m => m.LinkList).Returns(testLinks);
		}

		[Test]
		public void Execute_WithEmptyConfig_ShouldPrintConfigEmpty () {
			// Arrange
			IDefaultCommandHandler command = new ListCommand("testpath");
			testConfigHandler.Setup(m => m.LoadConfig("testpath")).Returns(testConfig.Object);

			// Act
			command.Execute(testConsole, testConfigHandler.Object, testFileSystem);

			// Assert
			Assert.IsTrue(testConsole.GetHistory().Contains("empty", StringComparison.OrdinalIgnoreCase));
		}

		[Test]
		public void Execute_WithConfigLinks_ShouldAllCases() {
			// Arrange
			IDefaultCommandHandler command = new ListCommand("testpath");

			testLinks.Add(new ConfigLink("source", "target", ConfigLink.LinkType.Default));
			testLinks.Add(new ConfigLink("testing is fun", "not really", ConfigLink.LinkType.Hard));

			testConfigHandler.Setup(m => m.LoadConfig("testpath")).Returns(testConfig.Object);

			// Act
			command.Execute(testConsole, testConfigHandler.Object, testFileSystem);

			// Assert
			testLinks.ForEach(link => Assert.IsTrue(testConsole.GetHistory().Contains(link.ToString(), StringComparison.OrdinalIgnoreCase)));
		}

	}

}
