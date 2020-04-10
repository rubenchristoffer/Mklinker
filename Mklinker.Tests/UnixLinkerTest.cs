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
	class UnixLinkerTest {

		TestConsole testConsole;
		MockFileSystem testFileSystem;
		Mock<IProcess> testProcess;

		[SetUp]
		public void Setup() {
			testConsole = new TestConsole();
			testProcess = new Mock<IProcess>();

			testFileSystem = new MockFileSystem(new Dictionary<string, MockFileData> {
				{ @"c:\config.linker", new MockFileData("<?xml version=\"1.0\" encoding=\"utf-16\"?><Config xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" Version=\"v1.1.1\"></Config>") },
				{ @"c:\invalidconfig.linker", new MockFileData("This content is invalid for a linker file!") },
				{ @"c:\demo\jQuery.js", new MockFileData("some js") },
				{ @"c:\demo\image.gif", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) },
				{ @"c:\demo\", new MockDirectoryData() }
			});
		}

		[Test]
		public void GetLinkTypeArgument_WithDefaultFile_WillReturnSymbolicString() {
			// Arrange
			const string testSourcePath = @"c:\config.linker";
			UnixLinker unixLinker = new UnixLinker(testConsole, testFileSystem, testProcess.Object);

			// Act
			string result = unixLinker.GetLinkTypeArgument(testFileSystem, ConfigLink.LinkType.Default, testSourcePath);

			// Assert
			Assert.AreEqual("-s", result);
		}

		[Test]
		public void GetLinkTypeArgument_WithHardFile_WillReturnEmptyString() {
			// Arrange
			const string testSourcePath = @"c:\config.linker";
			UnixLinker unixLinker = new UnixLinker(testConsole, testFileSystem, testProcess.Object);

			// Act
			string result = unixLinker.GetLinkTypeArgument(testFileSystem, ConfigLink.LinkType.Hard, testSourcePath);

			// Assert
			Assert.IsEmpty(result);
		}

		[Test]
		public void GetLinkTypeArgument_WithDefaultDirectory_WillReturnSymbolicString() {
			// Arrange
			const string testSourcePath = @"c:\demo\";
			UnixLinker unixLinker = new UnixLinker(testConsole, testFileSystem, testProcess.Object);

			// Act
			string result = unixLinker.GetLinkTypeArgument(testFileSystem, ConfigLink.LinkType.Default, testSourcePath);

			// Assert
			Assert.AreEqual("-s", result);
		}

		[Test]
		public void GetLinkTypeArgument_WithSymbolicDirectory_WillReturnSymbolicString() {
			// Arrange
			const string testSourcePath = @"c:\demo\";
			UnixLinker unixLinker = new UnixLinker(testConsole, testFileSystem, testProcess.Object);

			// Act
			string result = unixLinker.GetLinkTypeArgument(testFileSystem, ConfigLink.LinkType.Symbolic, testSourcePath);

			// Assert
			Assert.AreEqual("-s", result);
		}

		[Test]
		public void GetLinkTypeArgument_PathDoesNotExist_WillReturnEmptyString() {
			// Arrange
			const string testSourcePath = @"some random path that does not exist";
			UnixLinker unixLinker = new UnixLinker(testConsole, testFileSystem, testProcess.Object);

			// Act
			string result = unixLinker.GetLinkTypeArgument(testFileSystem, ConfigLink.LinkType.Symbolic, testSourcePath);

			// Assert
			Assert.IsEmpty(result);
		}

	}

}