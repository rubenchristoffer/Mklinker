using System.Linq;
using System.Collections.Generic;
using Mklinker.Commands;
using NUnit.Framework;
using System.IO.Abstractions.TestingHelpers;
using Moq;

namespace Mklinker.Tests.Commands {

	[TestFixture]
	class ConfigCommandTest {

		MockFileSystem testFileSystem;
		TestConsole testConsole;
		Config testDefaultConfig;
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
			testDefaultConfig = new Config("TestVersion");
			testConfigHandler = new ConfigHandler(testFileSystem, testDefaultConfig);
		}

		[Test]
		public void Execute_WithOnlyPathFlag_FileDoesNotExist() {
			// Arrange
			string testPath = testFileSystem.AllFiles.First();
			ConfigCommand configCommand = new ConfigCommand(false, false, testPath);

			Config testConfig = new Config("1.1.1");
			testConfig.LinkList.Add(new ConfigLink("source", "target", ConfigLink.LinkType.Default));

			var mockHandler = new Mock<Abstractions.IConfigHandler>();
			mockHandler.Setup(m => m.LoadConfig(testPath)).Returns(testConfig);
			mockHandler.Setup(m => m.DoesConfigExist(testPath)).Returns(true);

			// Act
			configCommand.Execute(testConsole, mockHandler.Object, testDefaultConfig);

			// Assert
			Assert.IsTrue(testConsole.GetHistory().Contains("Total links: " + testConfig.LinkList.Count));
		}

		[Test]
		public void Execute_WithConfigFlag_ShouldCreateFile() {
			// Arrange
			const string testPath = "config.linker";
			ConfigCommand configCommand = new ConfigCommand(true, false, testPath);

			// Act
			configCommand.Execute(testConsole, testConfigHandler, testDefaultConfig);
			
			// Assert
			Assert.IsTrue(testFileSystem.File.Exists(testPath));
		}

		[Test]
		public void Execute_WithConfigFlag_FileAlreadyExists() {
			// Arrange
			string testPath = testFileSystem.AllFiles.First();
			ConfigCommand configCommand = new ConfigCommand(true, false, testPath);

			// Act
			configCommand.Execute(testConsole, testConfigHandler, testDefaultConfig);

			// Assert
			Assert.IsTrue(testConsole.GetHistory().Contains("already exists", System.StringComparison.OrdinalIgnoreCase));
		}

		[Test]
		public void Execute_WithDeleteFlag_ShouldDeleteFile() {
			// Arrange
			string testPath = testFileSystem.AllFiles.First();
			ConfigCommand configCommand = new ConfigCommand(false, true, testPath);

			// Act
			configCommand.Execute(testConsole, testConfigHandler, testDefaultConfig);

			// Assert
			Assert.IsFalse(testFileSystem.File.Exists(testPath));
		}

		[Test]
		public void Execute_WithDeleteFlag_FileDoesNotExist() {
			// Arrange
			string testPath = "doesnotexist";
			ConfigCommand configCommand = new ConfigCommand(false, true, testPath);

			// Act
			configCommand.Execute(testConsole, testConfigHandler, testDefaultConfig);

			// Assert
			Assert.IsTrue(testConsole.GetHistory().Contains("does not exist", System.StringComparison.OrdinalIgnoreCase));
		}

	}

}
