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
		TestPathResolver testPathResolver;

		[SetUp]
		public void Setup () {
			testConsole = new TestConsole();
			testConfigHandler = new Mock<IConfigHandler>();
			testConfigHandler.Setup(m => m.DoesConfigExist(It.IsAny<string>())).Returns(true);
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

			testPathResolver = new TestPathResolver();
		}

		[Test]
		public void Execute_WithConfigLinks_WillCreateLinkForAll () {
			// Arrange
			testLinks.Add(new ConfigLink(@"c:\config.linker", "targetfile.linker", ConfigLink.LinkType.Default));
			testLinks.Add(new ConfigLink(@"c:\invalidconfig.linker", "somerandomlink.linker", ConfigLink.LinkType.Default));

			testLinks.ForEach(link => testLinker.Setup(m => m.CreateLink(link.sourcePath, link.targetPath, link.linkType)).Returns(true));
			testConfigHandler.Setup(m => m.LoadConfig("testpath")).Returns(testConfig.Object);

			LinkAllCommand command = new LinkAllCommand("testpath");

			// Act
			command.Execute(testConsole, testConfigHandler.Object, testFileSystem, testLinker.Object, testPathResolver);

			// Assert
			testLinks.ForEach(link => testLinker.Verify(m => m.CreateLink(link.sourcePath, link.targetPath, link.linkType)));
		}

		[Test]
		public void Execute_WithMissingSubdirectories_WillCreateLinkForAll () {
			// Arrange
			testLinks.Add (new ConfigLink (@"c:\config.linker", @"./sub dir/targetfile.linker", ConfigLink.LinkType.Default));
			testLinks.Add (new ConfigLink (@"c:\invalidconfig.linker", @"./subdir1/subdir2/somerandomlink.linker", ConfigLink.LinkType.Default));

			testLinks.ForEach (link => testLinker.Setup (m => m.CreateLink (link.sourcePath, link.targetPath, link.linkType)).Returns (true));
			testConfigHandler.Setup (m => m.LoadConfig ("testpath")).Returns (testConfig.Object);

			LinkAllCommand command = new LinkAllCommand ("testpath");

			// Act
			command.Execute (testConsole, testConfigHandler.Object, testFileSystem, testLinker.Object, testPathResolver);

			// Assert
			Assert.IsTrue (testFileSystem.Directory.Exists (@"./sub dir/"));
			Assert.IsTrue (testFileSystem.Directory.Exists(@"subdir1/subdir2/"));

			testLinks.ForEach (link => testLinker.Verify (m => m.CreateLink (link.sourcePath, link.targetPath, link.linkType)));
		}

	}

}
