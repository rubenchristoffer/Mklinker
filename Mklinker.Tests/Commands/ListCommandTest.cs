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
		MockFileSystem testFileSystem;

		[SetUp]
		public void Setup () {
			testConsole = new TestConsole();
			testConfigHandler = new Mock<IConfigHandler>();
			testFileSystem = new MockFileSystem();
		}

		[Test]
		public void Execute_WithEmptyConfig_ShouldPrintConfigEmpty () {
			// Arrange
			IDefaultCommandHandler command = new ListCommand("testpath");

			List<ConfigLink> links = new List<ConfigLink>();

			var mockConfig = new Mock<IConfig>();
			mockConfig.Setup(m => m.LinkList).Returns(links);
			testConfigHandler.Setup(m => m.LoadConfig("testpath")).Returns(mockConfig.Object);

			// Act
			command.Execute(testConsole, testConfigHandler.Object, testFileSystem);

			// Assert
			Assert.IsTrue(testConsole.GetHistory().Contains("empty", StringComparison.OrdinalIgnoreCase));
		}

		[Test]
		public void Execute_WithConfigLinks_ShouldAllCases() {
			// Arrange
			IDefaultCommandHandler command = new ListCommand("testpath");

			List<ConfigLink> links = new List<ConfigLink>();
			links.Add(new ConfigLink("source", "target", ConfigLink.LinkType.Default));
			links.Add(new ConfigLink("testing is fun", "not really", ConfigLink.LinkType.Hard));

			var mockConfig = new Mock<IConfig>();
			mockConfig.Setup(m => m.LinkList).Returns(links);
			testConfigHandler.Setup(m => m.LoadConfig("testpath")).Returns(mockConfig.Object);

			// Act
			command.Execute(testConsole, testConfigHandler.Object, testFileSystem);

			// Assert
			links.ForEach(link => Assert.IsTrue(testConsole.GetHistory().Contains(link.ToString(), StringComparison.OrdinalIgnoreCase)));
		}

	}

}
