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
		Mock<IConfig> testConfig;
		List<ConfigLink> testLinks;
		ConfigLink[] testLinkElements;
		TestPathResolver testPathResolver;

		[SetUp]
		public void Setup() {
			testConsole = new TestConsole();

			testConfigHandler = new Mock<IConfigHandler>();
			testConfigHandler.Setup(m => m.DoesConfigExist(It.IsAny<string>())).Returns(true);

			testFileSystem = new MockFileSystem(new Dictionary<string, MockFileData> {
				{ @"c:\config.linker", new MockFileData("<?xml version=\"1.0\" encoding=\"utf-16\"?><Config xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" Version=\"v1.1.1\"></Config>") },
				{ @"c:\invalidconfig.linker", new MockFileData("This content is invalid for a linker file!") },
				{ @"c:\demo\jQuery.js", new MockFileData("some js") },
				{ @"c:\demo\", new MockDirectoryData () },
				{ @"c:\demo\image.gif", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) }
			});

			testLinks = new List<ConfigLink>();

			testLinkElements = new ConfigLink[] {
				new ConfigLink("source", "target", ConfigLink.LinkType.Default),
				new ConfigLink("testing is fun", "not really", ConfigLink.LinkType.Hard)
			};

			testConfig = new Mock<IConfig>();
			testConfig.Setup(m => m.LinkList).Returns(testLinks);

			testPathResolver = new TestPathResolver();
		}

		[Test]
		public void Execute_WithTargetPath_WillRemoveFromConfig () {
			// Arrange
			RemoveLinkCommand command = new RemoveLinkCommand("target", "testpath");

			testLinks.Add(testLinkElements[0]);
			testLinks.Add(testLinkElements[1]);

			testConfigHandler.Setup(m => m.LoadConfig("testpath")).Returns(testConfig.Object);

			// Act
			command.Execute(testConsole, testConfigHandler.Object, testFileSystem, testPathResolver);

			// Assert
			Assert.IsFalse(testConfigHandler.Object.LoadConfig("testpath").LinkList.Contains(testLinkElements[0]));
			Assert.IsTrue(testConfigHandler.Object.LoadConfig("testpath").LinkList.Contains(testLinkElements[1]));
		}

		[Test]
		public void Execute_WithInvalidTargetPath_WillPrintError() {
			// Arrange
			const string testTargetPath = "some random target here";
			RemoveLinkCommand command = new RemoveLinkCommand(testTargetPath, "testpath");

			testLinks.Add(testLinkElements[0]);
			testLinks.Add(testLinkElements[1]);
			testConfigHandler.Setup(m => m.LoadConfig("testpath")).Returns(testConfig.Object);

			// Act
			command.Execute(testConsole, testConfigHandler.Object, testFileSystem, testPathResolver);

			// Assert
			Assert.IsTrue(testConsole.GetHistory().Contains("does not exist", StringComparison.OrdinalIgnoreCase));
			Assert.IsTrue(testConsole.GetHistory().Contains(testTargetPath));
		}

	}

}
