using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Mklinker.Commands;
using Mklinker.Abstractions;
using Moq;
using System.IO.Abstractions.TestingHelpers;

namespace Mklinker.Tests.Commands {

	[TestFixture]
	class RemoveLinkCommandTest {

		TestConsole testConsole;
		Mock<IConfigHandler> testConfigHandler;
		MockFileSystem testFileSystem;

		[SetUp]
		public void Setup() {
			testConsole = new TestConsole();

			testConfigHandler = new Mock<IConfigHandler>();

			testFileSystem = new MockFileSystem(new Dictionary<string, MockFileData> {
				{ @"c:\config.linker", new MockFileData("<?xml version=\"1.0\" encoding=\"utf-16\"?><Config xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" Version=\"v1.1.1\"></Config>") },
				{ @"c:\invalidconfig.linker", new MockFileData("This content is invalid for a linker file!") },
				{ @"c:\demo\jQuery.js", new MockFileData("some js") },
				{ @"c:\demo\", new MockDirectoryData () },
				{ @"c:\demo\image.gif", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) }
			});
		}

		[Test]
		public void Execute_WithTargetPath_WillRemoveFromConfig () {
			// Arrange
			IDefaultCommandHandler command = new RemoveLinkCommand("target", "testpath");

			List<ConfigLink> links = new List<ConfigLink>();

			var element1 = new ConfigLink("source", "target", ConfigLink.LinkType.Default);
			var element2 = new ConfigLink("testing is fun", "not really", ConfigLink.LinkType.Hard);

			links.Add(element1);
			links.Add(element2);

			var mockConfig = new Mock<IConfig>();
			mockConfig.Setup(m => m.LinkList).Returns(links);
			testConfigHandler.Setup(m => m.LoadConfig("testpath")).Returns(mockConfig.Object);

			// Act
			command.Execute(testConsole, testConfigHandler.Object, testFileSystem);

			// Assert
			Assert.IsFalse(links.Contains(element1));
			Assert.IsTrue(links.Contains(element2));
		}

		[Test]
		public void Execute_WithInvalidTargetPath_WillPrintError() {
			// Arrange
			const string testTargetPath = "some random target here";
			IDefaultCommandHandler command = new RemoveLinkCommand(testTargetPath, "testpath");

			List<ConfigLink> links = new List<ConfigLink>();

			var element1 = new ConfigLink("source", "target", ConfigLink.LinkType.Default);
			var element2 = new ConfigLink("testing is fun", "not really", ConfigLink.LinkType.Hard);

			links.Add(element1);
			links.Add(element2);

			var mockConfig = new Mock<IConfig>();
			mockConfig.Setup(m => m.LinkList).Returns(links);
			testConfigHandler.Setup(m => m.LoadConfig("testpath")).Returns(mockConfig.Object);

			// Act
			command.Execute(testConsole, testConfigHandler.Object, testFileSystem);

			// Assert
			Assert.IsTrue(testConsole.GetHistory().Contains("does not exist", StringComparison.OrdinalIgnoreCase));
			Assert.IsTrue(testConsole.GetHistory().Contains(testTargetPath));
		}

	}

}
