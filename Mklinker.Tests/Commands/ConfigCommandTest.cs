using System.Linq;
using System.Collections.Generic;
using Mklinker.Commands;
using Mklinker.Abstractions;
using NUnit.Framework;
using System.IO.Abstractions.TestingHelpers;
using Moq;

namespace Mklinker.Tests.Commands {

	[TestFixture]
	class ConfigCommandTest {

		MockFileSystem testFileSystem;
		TestConsole testConsole;
		Mock<IConfigHandler> testConfigHandler;
		Mock<IConfig> testConfig;
		List<ConfigLink> testLinks;
		ConfigLink[] testLinkElements;

		[SetUp]
		public void Setup () {
			testFileSystem = new MockFileSystem(new Dictionary<string, MockFileData> {
				{ @"c:\config.linker", new MockFileData("<?xml version=\"1.0\" encoding=\"utf-16\"?><Config xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" Version=\"v1.1.1\"></Config>") },
				{ @"c:\invalidconfig.linker", new MockFileData("This content is invalid for a linker file!") },
				{ @"c:\demo\jQuery.js", new MockFileData("some js") },
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
		}

		[Test]
		public void Execute_WithOnlyPathFlag_PrintsTotalLinks() {
			// Arrange
			string testPath = testFileSystem.AllFiles.First();
			ConfigCommand command = new ConfigCommand(false, false, testPath);

			testLinks.Add(testLinkElements[0]);

			testConfigHandler.Setup(m => m.LoadConfig(testPath)).Returns(testConfig.Object);
			testConfigHandler.Setup(m => m.DoesConfigExist(testPath)).Returns(true);

			// Act
			command.Execute(testConsole, testConfigHandler.Object, testConfig.Object);

			// Assert
			Assert.IsTrue(testConsole.GetHistory().Contains("Total links: " + testLinks.Count));
		}

		[Test]
		public void Execute_WithOnlyInvalidPathFlag_FileDoesNotExist() {
			// Arrange
			string testPath = "some config that does not exist";
			ConfigCommand command = new ConfigCommand(false, false, testPath);
			testConfigHandler.Setup(m => m.DoesConfigExist(testPath)).Returns(false);

			// Act
			command.Execute(testConsole, testConfigHandler.Object, testConfig.Object);

			// Assert
			Assert.IsTrue(testConsole.GetHistory().Contains("does not exist"));
		}

		[Test]
		public void Execute_WithConfigFlag_ShouldCreateFile() {
			// Arrange
			const string testPath = "config.linker";
			ConfigCommand configCommand = new ConfigCommand(true, false, testPath);
			testConfigHandler.Setup(m => m.DoesConfigExist(testPath)).Returns(false);

			// Act
			configCommand.Execute(testConsole, testConfigHandler.Object, testConfig.Object);

			// Assert
			testConfigHandler.Verify(m => m.SaveConfig(testConfig.Object, testPath));
		}

		[Test]
		public void Execute_WithConfigFlag_FileAlreadyExists() {
			// Arrange
			string testPath = testFileSystem.AllFiles.First();
			ConfigCommand configCommand = new ConfigCommand(true, false, testPath);
			testConfigHandler.Setup(m => m.DoesConfigExist(testPath)).Returns(true);

			// Act
			configCommand.Execute(testConsole, testConfigHandler.Object, testConfig.Object);

			// Assert
			Assert.IsTrue(testConsole.GetHistory().Contains("already exists", System.StringComparison.OrdinalIgnoreCase));
		}

		[Test]
		public void Execute_WithDeleteFlag_ShouldDeleteFile() {
			// Arrange
			string testPath = testFileSystem.AllFiles.First();
			ConfigCommand configCommand = new ConfigCommand(false, true, testPath);
			testConfigHandler.Setup(m => m.DoesConfigExist(testPath)).Returns(true);

			// Act
			configCommand.Execute(testConsole, testConfigHandler.Object, testConfig.Object);

			// Assert
			testConfigHandler.Verify(m => m.DeleteConfig(testPath));
		}

		[Test]
		public void Execute_WithDeleteFlag_FileDoesNotExist() {
			// Arrange
			string testPath = "doesnotexist";
			ConfigCommand configCommand = new ConfigCommand(false, true, testPath);
			testConfigHandler.Setup(m => m.DoesConfigExist(testPath)).Returns(false);

			// Act
			configCommand.Execute(testConsole, testConfigHandler.Object, testConfig.Object);

			// Assert
			Assert.IsTrue(testConsole.GetHistory().Contains("does not exist", System.StringComparison.OrdinalIgnoreCase));
		}

	}

}
