using System;
using System.Linq;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using NUnit.Framework;
using Mklinker.Commands;
using Mklinker.Abstractions;
using Moq;

namespace Mklinker.Tests {

	[TestFixture]
	class ConfigHandlerTest {

		MockFileSystem testFileSystem;
		Mock<IConfigSerializer> testConfigSerializer;
		Mock<IConfig> testConfig;

		[SetUp]
		public void Setup() {
			testFileSystem = new MockFileSystem(new Dictionary<string, MockFileData> {
				{ @"c:\config.linker", new MockFileData("<?xml version=\"1.0\" encoding=\"utf-16\"?><Config xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" Version=\"v1.1.1\"></Config>") },
				{ @"c:\invalidconfig.linker", new MockFileData("This content is invalid for a linker file!") },
				{ @"c:\demo\jQuery.js", new MockFileData("some js") },
				{ @"c:\demo\image.gif", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) }
			});

			testConfigSerializer = new Mock<IConfigSerializer>();
			testConfig = new Mock<IConfig>();
		}

		[Test]
		public void DoesConfigExist_WhereFileExists_WillReturnTrue() {
			// Arrange
			const string testPath = @"c:\config.linker";
			ConfigHandler configHandler = new ConfigHandler(testFileSystem, testConfigSerializer.Object);

			// Act
			bool result = configHandler.DoesConfigExist(testPath);

			// Assert
			Assert.IsTrue(result);
		}

		[Test]
		public void DoesConfigExist_WhereFileDoesNotExist_WillReturnFalse() {
			// Arrange
			const string testPath = @"c:\config_not_existing.linker";
			ConfigHandler configHandler = new ConfigHandler(testFileSystem, testConfigSerializer.Object);

			// Act
			bool result = configHandler.DoesConfigExist(testPath);

			// Assert
			Assert.IsFalse(result);
		}

		[Test]
		public void DeleteConfig_WhereFileExists_WillDeleteFile() {
			// Arrange
			const string testPath = @"c:\config.linker";
			ConfigHandler configHandler = new ConfigHandler(testFileSystem, testConfigSerializer.Object);

			// Act
			configHandler.DeleteConfig(testPath);

			// Assert
			Assert.IsFalse(testFileSystem.File.Exists(testPath));
		}

		[Test]
		public void SaveConfig_WhereFileDoesNotExist_WillCreateConfig() {
			// Arrange
			const string testPath = @"c:\config_not_existing.linker";
			ConfigHandler configHandler = new ConfigHandler(testFileSystem, testConfigSerializer.Object);
			testConfigSerializer.Setup(m => m.Serialize(testConfig.Object)).Returns("serialized string");

			// Act
			configHandler.SaveConfig(testConfig.Object, testPath);

			// Assert
			Assert.IsTrue(testFileSystem.File.ReadAllText(testPath).Equals("serialized string"));
		}

		[Test]
		public void SaveConfig_WhereFileExists_WillCreateConfig() {
			// Arrange
			const string testPath = @"c:\config.linker";
			ConfigHandler configHandler = new ConfigHandler(testFileSystem, testConfigSerializer.Object);
			configHandler.SaveConfig(testConfig.Object, testPath);
			testConfigSerializer.Setup(m => m.Serialize(testConfig.Object)).Returns("serialized string");

			// Act
			configHandler.SaveConfig(testConfig.Object, testPath);

			// Assert
			Assert.IsTrue(testFileSystem.File.ReadAllText(testPath).Equals("serialized string"));
		}

		[Test]
		public void LoadConfig_WhereFileExists_WillLoadConfig() {
			// Arrange
			const string testPath = @"c:\demo\jQuery.js";
			ConfigHandler configHandler = new ConfigHandler(testFileSystem, testConfigSerializer.Object);
			testConfigSerializer.Setup(m => m.Deserialize("some js")).Returns(testConfig.Object);

			// Act
			IConfig result = configHandler.LoadConfig(testPath);

			// Assert
			Assert.AreEqual(result, testConfig.Object);
		}

	}

}
