using System.Collections.Generic;
using NUnit.Framework;
using Moq;
using Mklinker.Abstractions;
using Mklinker.Commands;
using System.IO.Abstractions.TestingHelpers;

namespace Mklinker.Tests.Commands {

	[TestFixture]
	class LinkAllCommandTest {

		TestConsole testConsole;
		Mock<IConfigHandler> testConfigHandler;
		Mock<IConfig> testConfig;
		MockFileSystem testFileSystem; 
		Mock<ILinker> testLinker;
		List<ConfigLink> testLinks;

		[SetUp]
		public void Setup () {
			testConsole = new TestConsole();
			testConfigHandler = new Mock<IConfigHandler>();
			testLinker = new Mock<ILinker>();

			testFileSystem = new MockFileSystem(new Dictionary<string, MockFileData> {
				{ @"c:\config.linker", new MockFileData("<?xml version=\"1.0\" encoding=\"utf-16\"?><Config xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" Version=\"v1.1.1\"></Config>") },
				{ @"c:\invalidconfig.linker", new MockFileData("This content is invalid for a linker file!") },
				{ @"c:\demo\jQuery.js", new MockFileData("some js") },
				{ @"c:\demo\image.gif", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) }
			});

			testLinks = new List<ConfigLink>();
			testConfig = new Mock<IConfig>();
			testConfig.Setup(m => m.LinkList).Returns(testLinks);
		}

		[Test]
		public void Execute_WithConfigLinks_WillCreateLinkForAll () {
			// Arrange
			testLinks.Add(new ConfigLink(@"c:\config.linker", "targetfile.linker", ConfigLink.LinkType.Default));
			testLinks.Add(new ConfigLink(@"c:\invalidconfig.linker", "somerandomlink.linker", ConfigLink.LinkType.Default));

			testLinks.ForEach(link => testLinker.Setup(m => m.CreateLink(link)).Returns(true));
			testConfigHandler.Setup(m => m.LoadConfig("testpath")).Returns(testConfig.Object);

			LinkAllCommand command = new LinkAllCommand("testpath");

			// Act
			command.Execute(testConsole, testConfigHandler.Object, testFileSystem, testLinker.Object);

			// Assert
			testLinks.ForEach(link => testLinker.Verify(m => m.CreateLink(link)));
		}

		[Test]
		public void Execute_WithNonexistingSource_WillShowError() {
			// Arrange
			const string testSourcePath = @"source that does not exist";
			testLinks.Add(new ConfigLink(testSourcePath, "targetfile.linker", ConfigLink.LinkType.Default));

			testLinks.ForEach(link => testLinker.Setup(m => m.CreateLink(link)));
			testConfigHandler.Setup(m => m.LoadConfig("testpath")).Returns(testConfig.Object);

			LinkAllCommand command = new LinkAllCommand("testpath");

			// Act
			command.Execute(testConsole, testConfigHandler.Object, testFileSystem, testLinker.Object);

			// Assert
			Assert.IsTrue(testConsole.GetHistory().Contains("does not exist", System.StringComparison.OrdinalIgnoreCase));
			Assert.IsTrue(testConsole.GetHistory().Contains(testSourcePath, System.StringComparison.OrdinalIgnoreCase));
		}

	}

}
