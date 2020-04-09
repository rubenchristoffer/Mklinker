using System;
using System.Linq;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using NUnit.Framework;
using Mklinker.Commands;
using Mklinker.Abstractions;
using Moq;

namespace Mklinker.Tests.Commands {

	[TestFixture]
	class AddLinkCommandTest {

		TestConsole testConsole;
		MockFileSystem testFileSystem;
		Mock<IConfigHandler> testConfigHandler;
		Mock<IConfig> testConfig;
		List<ConfigLink> testLinks;
		ConfigLink[] testLinkElements;
		TestPathFormatter testPathFormatter;

		[SetUp]
		public void Setup () {
			testFileSystem = new MockFileSystem(new Dictionary<string, MockFileData> {
				{ @"c:\config.linker", new MockFileData("<?xml version=\"1.0\" encoding=\"utf-16\"?><Config xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" Version=\"v1.1.1\"></Config>") },
				{ @"c:\invalidconfig.linker", new MockFileData("This content is invalid for a linker file!") },
				{ @"c:\demo\jQuery.js", new MockFileData("some js") },
				{ @"c:\demo\", new MockDirectoryData () },
				{ @"c:\demo\image.gif", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) }
			});

			testConsole = new TestConsole();
			testConfigHandler = new Mock<IConfigHandler>();
			testLinks = new List<ConfigLink>();

			testLinkElements = new ConfigLink[] {
				new ConfigLink("source", "target", ConfigLink.LinkType.Default),
				new ConfigLink("testing is fun", "not really", ConfigLink.LinkType.Hard)
			};

			testConfig = new Mock<IConfig>();
			testConfig.Setup(m => m.LinkList).Returns(testLinks);
			testPathFormatter = new TestPathFormatter();
		}

		[Test]
		public void Execute_WithOnlyRequiredArguments_WillCreateDefaultFileLink () {
			// Arrange
			const string testPath = @"c:\config.linker";
			const string testSourcePath = @"c:\config.linker";
			const string testTargetPath = @"c:\demo\image.gif"; // Should be able to add target link that already exists

			AddLinkCommand command = new AddLinkCommand(testTargetPath, testSourcePath, ConfigLink.LinkType.Default, testPath);

			testLinks.Add(testLinkElements[0]);
			testLinks.Add(testLinkElements[1]);
			testConfigHandler.Setup(m => m.LoadConfig(testPath)).Returns(testConfig.Object);

			// Act
			command.Execute(testConsole, testConfigHandler.Object, testFileSystem, testPathFormatter);

			// Assert
			Assert.IsTrue(testConfigHandler.Object.LoadConfig (testPath).LinkList.Any (link => 
				link.sourcePath.Equals(testSourcePath) && link.targetPath.Equals(testTargetPath) && link.linkType == ConfigLink.LinkType.Default));
		}

		[Test]
		public void Execute_WithOnlyRequiredArguments_WillCreateDefaultDirectoryLink() {
			// Arrange
			const string testPath = @"c:\config.linker";
			const string testSourcePath = @"c:\demo\";
			const string testTargetPath = "testTargetDirectory";

			AddLinkCommand command = new AddLinkCommand(testTargetPath, testSourcePath, ConfigLink.LinkType.Default, testPath);

			testLinks.Add(testLinkElements[0]);
			testLinks.Add(testLinkElements[1]);
			testConfigHandler.Setup(m => m.LoadConfig(testPath)).Returns(testConfig.Object);

			// Act
			command.Execute(testConsole, testConfigHandler.Object, testFileSystem, testPathFormatter);

			// Assert
			Assert.IsTrue(testConfigHandler.Object.LoadConfig(testPath).LinkList.Any(link =>
			  link.sourcePath.Equals(testSourcePath) && link.targetPath.Equals(testTargetPath) && link.linkType == ConfigLink.LinkType.Default));
		}

		[Test]
		public void Execute_WithOnlyRequiredArgumentsRunTwice_ShouldShowDuplicatePathErrorMessage() {
			// Arrange
			const string testPath = @"c:\config.linker";
			const string testSourcePath = @"c:\demo\";
			const string testTargetPath = "testTargetDirectory";

			AddLinkCommand command = new AddLinkCommand(testTargetPath, testSourcePath, ConfigLink.LinkType.Default, testPath);
			testConfigHandler.Setup(m => m.LoadConfig(testPath)).Returns(testConfig.Object);

			// Act
			testConsole.ShouldRecordHistory = false;
			command.Execute(testConsole, testConfigHandler.Object, testFileSystem, testPathFormatter);

			testConsole.ShouldRecordHistory = true;
			command.Execute(testConsole, testConfigHandler.Object, testFileSystem, testPathFormatter);

			// Assert
			Assert.IsTrue(testConsole.GetHistory().Contains("already exists", StringComparison.OrdinalIgnoreCase));
			Assert.IsTrue(testConsole.GetHistory().Contains(testTargetPath, StringComparison.OrdinalIgnoreCase));
		}

		[Test]
		public void Execute_WithInvalidSourceFolder_ShouldShowInvalidPathErrorMessage() {
			// Arrange
			const string testPath = @"c:\config.linker";
			const string testSourcePath = @"c:\some path that does not exist";
			const string testTargetPath = "testTargetDirectory";

			AddLinkCommand command = new AddLinkCommand(testTargetPath, testSourcePath, ConfigLink.LinkType.Default, testPath);
			testConfigHandler.Setup(m => m.LoadConfig(testPath)).Returns(testConfig.Object);

			// Act
			command.Execute(testConsole, testConfigHandler.Object, testFileSystem, testPathFormatter);

			// Assert
			Assert.IsTrue(testConsole.GetHistory().Contains("does not exist", StringComparison.OrdinalIgnoreCase));
			Assert.IsTrue(testConsole.GetHistory().Contains(testSourcePath, StringComparison.OrdinalIgnoreCase));
		}

		[Test]
		public void Execute_WithLinkFlagHard_ShouldCreateHardLink() {
			// Arrange
			const string testPath = @"c:\config.linker";
			const string testSourcePath = @"c:\config.linker";
			const string testTargetPath = "someOtherFile";
			const ConfigLink.LinkType testLinkType = ConfigLink.LinkType.Hard;

			AddLinkCommand command = new AddLinkCommand(testTargetPath, testSourcePath, testLinkType, testPath);
			testConfigHandler.Setup(m => m.LoadConfig(testPath)).Returns(testConfig.Object);

			// Act
			command.Execute(testConsole, testConfigHandler.Object, testFileSystem, testPathFormatter);

			// Assert
			Assert.IsTrue(testConfigHandler.Object.LoadConfig(testPath).LinkList.Any(link =>
			  link.sourcePath.Equals(testSourcePath) && link.targetPath.Equals(testTargetPath) && link.linkType == testLinkType));
		}

		[Test]
		public void Execute_WithLinkFlagSymbolic_ShouldCreateSymbolicLink() {
			// Arrange
			const string testPath = @"c:\config.linker";
			const string testSourcePath = @"c:\config.linker";
			const string testTargetPath = "someOtherFile";
			const ConfigLink.LinkType testLinkType = ConfigLink.LinkType.Symbolic;

			AddLinkCommand command = new AddLinkCommand(testTargetPath, testSourcePath, testLinkType, testPath);
			testConfigHandler.Setup(m => m.LoadConfig(testPath)).Returns(testConfig.Object);

			// Act
			command.Execute(testConsole, testConfigHandler.Object, testFileSystem, testPathFormatter);

			// Assert
			Assert.IsTrue(testConfigHandler.Object.LoadConfig(testPath).LinkList.Any(link =>
			  link.sourcePath.Equals(testSourcePath) && link.targetPath.Equals(testTargetPath) && link.linkType == testLinkType));
		}

		[Test]
		public void Execute_WithLinkFlagJunction_ShouldCreateJunctionLink() {
			// Arrange
			const string testPath = @"c:\config.linker";
			const string testSourcePath = @"c:\demo\";
			const string testTargetPath = @"c:\folder\anotherfolder\yetanotherfolder";
			const ConfigLink.LinkType testLinkType = ConfigLink.LinkType.Junction;

			AddLinkCommand command = new AddLinkCommand(testTargetPath, testSourcePath, testLinkType, testPath);
			testConfigHandler.Setup(m => m.LoadConfig(testPath)).Returns(testConfig.Object);

			// Act
			command.Execute(testConsole, testConfigHandler.Object, testFileSystem, testPathFormatter);

			// Assert
			Assert.IsTrue(testConfigHandler.Object.LoadConfig(testPath).LinkList.Any(link =>
			  link.sourcePath.Equals(testSourcePath) && link.targetPath.Equals(testTargetPath) && link.linkType == testLinkType));
		}

	}

}
