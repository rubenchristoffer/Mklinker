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

		[SetUp]
		public void Setup () {
			testConsole = new TestConsole();
			testConfigHandler = new Mock<IConfigHandler>();

			testFileSystem = new MockFileSystem(new Dictionary<string, MockFileData> {
				{ @"c:\config.linker", new MockFileData("<?xml version=\"1.0\" encoding=\"utf-16\"?><Config xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" Version=\"v1.1.1\"></Config>") },
				{ @"c:\invalidconfig.linker", new MockFileData("This content is invalid for a linker file!") },
				{ @"c:\demo\jQuery.js", new MockFileData("some js") },
				{ @"c:\demo\image.gif", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) }
			});
		}

		[Test]
		public void Execute_WithValidConfig_WillPrintValidConfig () {
			// Arrange
			ValidateCommand command = new ValidateCommand(false, "testpath");

			List<ConfigLink> links = new List<ConfigLink>();

			var element1 = new ConfigLink(@"c:\config.linker", "target", ConfigLink.LinkType.Symbolic);

			links.Add(element1);

			var mockConfig = new Mock<IConfig>();
			mockConfig.Setup(m => m.LinkList).Returns(links);
			testConfigHandler.Setup(m => m.LoadConfig("testpath")).Returns(mockConfig.Object);

			// Act
			((IDefaultCommandHandler)command).Execute(testConsole, testConfigHandler.Object, testFileSystem);

			// Assert
			Assert.IsTrue(testConsole.GetHistory().Contains("100% valid", System.StringComparison.OrdinalIgnoreCase));
		}

		[Test]
		public void Execute_WithEmptyConfig_WillPrintEmptyConfig() {
			// Arrange
			ValidateCommand command = new ValidateCommand(false, "testpath");

			List<ConfigLink> links = new List<ConfigLink>();

			var mockConfig = new Mock<IConfig>();
			mockConfig.Setup(m => m.LinkList).Returns(links);
			testConfigHandler.Setup(m => m.LoadConfig("testpath")).Returns(mockConfig.Object);

			// Act
			((IDefaultCommandHandler)command).Execute(testConsole, testConfigHandler.Object, testFileSystem);

			// Assert
			Assert.IsTrue(testConsole.GetHistory().Contains("empty", System.StringComparison.OrdinalIgnoreCase));
		}

		[Test]
		public void Execute_WithInvalidConfig_WillPrintInvalidConfig() {
			// Arrange
			ValidateCommand command = new ValidateCommand(false, "testpath");

			List<ConfigLink> links = new List<ConfigLink>();

			var element1 = new ConfigLink(@"c:\config_that_does_not_exist.linker", "target", ConfigLink.LinkType.Symbolic);

			links.Add(element1);

			var mockConfig = new Mock<IConfig>();
			mockConfig.Setup(m => m.LinkList).Returns(links);
			testConfigHandler.Setup(m => m.LoadConfig("testpath")).Returns(mockConfig.Object);

			// Act
			((IDefaultCommandHandler)command).Execute(testConsole, testConfigHandler.Object, testFileSystem);

			// Assert
			Assert.IsTrue(testConsole.GetHistory().Contains(element1.ToString(), System.StringComparison.OrdinalIgnoreCase));
		}

	}

}
