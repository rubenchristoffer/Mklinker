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
				{ @"c:\demo\image.gif", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) }
			});

			testConsole = new TestConsole("");
			testConfigHandler = new ConfigHandler(testFileSystem, new Config ());
		}

		[Test]
		public void Execute_WithOnlyRequiredArguments_WillCreateDefaultFileLink () {
			// Arrange
			const string testPath = @"c:\config.linker";
			string testSourcePath = testFileSystem.AllFiles.First();
			string testTargetPath = testFileSystem.AllFiles.Last();

			AddLinkCommand command = new AddLinkCommand(testTargetPath, testSourcePath, ConfigLink.LinkType.Default, testPath);

			// Act
			((IDefaultCommandHandler) command).Execute(testConsole, testConfigHandler, testFileSystem);

			// Assert
			Assert.IsTrue(testConfigHandler.LoadConfig (testPath).LinkList.Any (link => 
				link.sourcePath.Equals(testSourcePath) && link.targetPath.Equals(testTargetPath) && link.linkType == ConfigLink.LinkType.Symbolic));
		}

	}

}
