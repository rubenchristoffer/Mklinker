using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using NUnit.Framework;
using Mklinker;
using Mklinker.Commands;
using Mklinker.Abstractions;
using Moq;

namespace Mklinker.Tests.Commands {

	[TestFixture]
	class AddLinkCommandTest {

		TestConsole testConsole;
		MockFileSystem testFileSystem;
		ConfigHandler testConfigHandler;

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
			testConfigHandler = new ConfigHandler(testFileSystem, new Config ());
		}

		[Test]
		public void Execute_WithOnlyRequiredArguments_WillCreateDefaultFileLink () {
			// Arrange
			const string testPath = @"c:\config.linker";
			const string testSourcePath = @"c:\config.linker";
			const string testTargetPath = @"c:\demo\image.gif"; // Should be able to add target link that already exists

			AddLinkCommand command = new AddLinkCommand(testTargetPath, testSourcePath, ConfigLink.LinkType.Default, testPath);

			// Act
			((IDefaultCommandHandler) command).Execute(testConsole, testConfigHandler, testFileSystem);

			// Assert
			Assert.IsTrue(testConfigHandler.LoadConfig (testPath).LinkList.Any (link => 
				link.sourcePath.Equals(testSourcePath) && link.targetPath.Equals(testTargetPath) && link.linkType == ConfigLink.LinkType.Symbolic));
		}

		// NOTE: This will become platform-dependent later if cross-platform is implemented
		[Test]
		public void Execute_WithOnlyRequiredArguments_WillCreateDefaultDirectoryLink() {
			// Arrange
			const string testPath = @"c:\config.linker";
			const string testSourcePath = @"c:\demo\";
			const string testTargetPath = "testTargetDirectory";

			AddLinkCommand command = new AddLinkCommand(testTargetPath, testSourcePath, ConfigLink.LinkType.Default, testPath);

			// Act
			((IDefaultCommandHandler)command).Execute(testConsole, testConfigHandler, testFileSystem);

			// Assert
			Assert.IsTrue(testConfigHandler.LoadConfig(testPath).LinkList.Any(link =>
			  link.sourcePath.Equals(testSourcePath) && link.targetPath.Equals(testTargetPath) && link.linkType == ConfigLink.LinkType.Junction));
		}

		[Test]
		public void Execute_WithOnlyRequiredArgumentsRunTwice_ShouldShowDuplicatePathErrorMessage() {
			// Arrange
			const string testPath = @"c:\config.linker";
			const string testSourcePath = @"c:\demo\";
			const string testTargetPath = "testTargetDirectory";

			AddLinkCommand command = new AddLinkCommand(testTargetPath, testSourcePath, ConfigLink.LinkType.Default, testPath);

			// Act
			testConsole.ShouldRecordHistory = false;
			((IDefaultCommandHandler)command).Execute(testConsole, testConfigHandler, testFileSystem);

			testConsole.ShouldRecordHistory = true;
			((IDefaultCommandHandler)command).Execute(testConsole, testConfigHandler, testFileSystem);

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

			// Act
			((IDefaultCommandHandler)command).Execute(testConsole, testConfigHandler, testFileSystem);

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

			// Act
			((IDefaultCommandHandler)command).Execute(testConsole, testConfigHandler, testFileSystem);

			// Assert
			Assert.IsTrue(testConfigHandler.LoadConfig(testPath).LinkList.Any(link =>
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

			// Act
			((IDefaultCommandHandler)command).Execute(testConsole, testConfigHandler, testFileSystem);

			// Assert
			Assert.IsTrue(testConfigHandler.LoadConfig(testPath).LinkList.Any(link =>
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

			// Act
			((IDefaultCommandHandler)command).Execute(testConsole, testConfigHandler, testFileSystem);

			// Assert
			Assert.IsTrue(testConfigHandler.LoadConfig(testPath).LinkList.Any(link =>
			  link.sourcePath.Equals(testSourcePath) && link.targetPath.Equals(testTargetPath) && link.linkType == testLinkType));
		}

	}

}
