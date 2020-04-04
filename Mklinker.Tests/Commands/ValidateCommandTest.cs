using System.Collections.Generic;
using Moq;
using Mklinker.Commands;
using Mklinker.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using NUnit.Framework;

namespace Mklinker.Tests.Commands {

	[TestFixture]
	class ValidateCommandTest {

		TestConsole testConsole;
		Mock<IConfigHandler> testConfigHandler;
		MockFileSystem testFileSystem;
		Mock<IConfig> testConfig;
		List<ConfigLink> testLinks;
		ConfigLink[] testLinkElements;

		[SetUp]
		public void Setup () {
			testConsole = new TestConsole();
			testConfigHandler = new Mock<IConfigHandler>();

			testFileSystem = new MockFileSystem(new Dictionary<string, MockFileData> {
				{ @"c:\config.linker", new MockFileData("<?xml version=\"1.0\" encoding=\"utf-16\"?><Config xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" Version=\"v1.1.1\"></Config>") },
				{ @"c:\invalidconfig.linker", new MockFileData("This content is invalid for a linker file!") },
				{ @"c:\demo\jQuery.js", new MockFileData("some js") },
				{ @"c:\demo\image.gif", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) },
				{ @"somedirectory", new MockDirectoryData() }
			});

			testLinks = new List<ConfigLink>();
			testConfig = new Mock<IConfig>();
			testConfig.Setup(m => m.LinkList).Returns(testLinks);

			testLinkElements = new ConfigLink[] {
				new ConfigLink(@"c:\config_that_does_not_exist.linker", "target", ConfigLink.LinkType.Symbolic),
				new ConfigLink(@"c:\config.linker", "target", ConfigLink.LinkType.Symbolic),
				new ConfigLink(@"somedirectory", "target", ConfigLink.LinkType.Hard),
			};
		}

		[Test]
		public void Execute_WithValidConfig_WillPrintValidConfig () {
			// Arrange
			ValidateCommand command = new ValidateCommand(false, "testpath");

			testLinks.Add(testLinkElements[1]);
			testConfigHandler.Setup(m => m.LoadConfig("testpath")).Returns(testConfig.Object);

			// Act
			((IDefaultCommandHandler)command).Execute(testConsole, testConfigHandler.Object, testFileSystem);

			// Assert
			Assert.IsTrue(testConsole.GetHistory().Contains("100% valid", System.StringComparison.OrdinalIgnoreCase));
		}

		[Test]
		public void Execute_WithEmptyConfig_WillPrintEmptyConfig() {
			// Arrange
			ValidateCommand command = new ValidateCommand(false, "testpath");

			testConfigHandler.Setup(m => m.LoadConfig("testpath")).Returns(testConfig.Object);

			// Act
			((IDefaultCommandHandler)command).Execute(testConsole, testConfigHandler.Object, testFileSystem);

			// Assert
			Assert.IsTrue(testConsole.GetHistory().Contains("empty", System.StringComparison.OrdinalIgnoreCase));
		}

		[Test]
		public void Execute_WithInvalidFileLink_WillPrintInvalidConfig() {
			// Arrange
			ValidateCommand command = new ValidateCommand(false, "testpath");

			testLinks.Add(testLinkElements[0]);
			testConfigHandler.Setup(m => m.LoadConfig("testpath")).Returns(testConfig.Object);

			// Act
			((IDefaultCommandHandler)command).Execute(testConsole, testConfigHandler.Object, testFileSystem);

			// Assert
			Assert.IsTrue(testConsole.GetHistory().Contains(testLinkElements[0].ToString(), System.StringComparison.OrdinalIgnoreCase));
		}

		[Test]
		public void Execute_WithInvalidDirectoryLink_WillPrintInvalidConfig() {
			// Arrange
			ValidateCommand command = new ValidateCommand(false, "testpath");

			testLinks.Add(testLinkElements[2]);
			testConfigHandler.Setup(m => m.LoadConfig("testpath")).Returns(testConfig.Object);

			// Act
			((IDefaultCommandHandler)command).Execute(testConsole, testConfigHandler.Object, testFileSystem);

			// Assert
			Assert.IsTrue(testConsole.GetHistory().Contains(testLinkElements[2].ToString(), System.StringComparison.OrdinalIgnoreCase));
		}

	}

}
