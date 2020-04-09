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
	class WindowsLinkerTest {

		TestConsole testConsole;
		MockFileSystem testFileSystem;
		Mock<IProcess> testProcess;
		TestPathResolver testPathFormatter;

		[SetUp]
		public void Setup() {
			testConsole = new TestConsole();
			testProcess = new Mock<IProcess>();
			testPathFormatter = new TestPathResolver();

			testFileSystem = new MockFileSystem(new Dictionary<string, MockFileData> {
				{ @"c:\config.linker", new MockFileData("<?xml version=\"1.0\" encoding=\"utf-16\"?><Config xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" Version=\"v1.1.1\"></Config>") },
				{ @"c:\invalidconfig.linker", new MockFileData("This content is invalid for a linker file!") },
				{ @"c:\demo\jQuery.js", new MockFileData("some js") },
				{ @"c:\demo\image.gif", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) },
				{ @"c:\demo\", new MockDirectoryData() }
			});
		}

		[Test]
		public void GetLinkTypeArgument_WithDefaultFile_WillReturnEmptyString() {
			// Arrange
			const string testSourcePath = @"c:\config.linker";
			WindowsLinker windowsLinker = new WindowsLinker(testConsole, testFileSystem, testProcess.Object, testPathFormatter);

			// Act
			string result = windowsLinker.GetLinkTypeArgument(testFileSystem, ConfigLink.LinkType.Default, testSourcePath);

			// Assert
			Assert.IsEmpty(result);
		}

		[Test]
		public void GetLinkTypeArgument_WithHardFile_WillReturnHardString() {
			// Arrange
			const string testSourcePath = @"c:\config.linker";
			WindowsLinker windowsLinker = new WindowsLinker(testConsole, testFileSystem, testProcess.Object, testPathFormatter);

			// Act
			string result = windowsLinker.GetLinkTypeArgument(testFileSystem, ConfigLink.LinkType.Hard, testSourcePath);

			// Assert
			Assert.AreEqual("/H", result);
		}

		[Test]
		public void GetLinkTypeArgument_WithDefaultDirectory_WillReturnJunctionString() {
			// Arrange
			const string testSourcePath = @"c:\demo\";
			WindowsLinker windowsLinker = new WindowsLinker(testConsole, testFileSystem, testProcess.Object, testPathFormatter);

			// Act
			string result = windowsLinker.GetLinkTypeArgument(testFileSystem, ConfigLink.LinkType.Default, testSourcePath);

			// Assert
			Assert.AreEqual("/J", result);
		}

		[Test]
		public void GetLinkTypeArgument_WithSymbolicDirectory_WillReturnSymbolicString() {
			// Arrange
			const string testSourcePath = @"c:\demo\";
			WindowsLinker windowsLinker = new WindowsLinker(testConsole, testFileSystem, testProcess.Object, testPathFormatter);

			// Act
			string result = windowsLinker.GetLinkTypeArgument(testFileSystem, ConfigLink.LinkType.Symbolic, testSourcePath);

			// Assert
			Assert.AreEqual("/D", result);
		}

		[Test]
		public void GetLinkTypeArgument_PathDoesNotExist_WillReturnEmptyString() {
			// Arrange
			const string testSourcePath = @"some random path that does not exist";
			WindowsLinker windowsLinker = new WindowsLinker(testConsole, testFileSystem, testProcess.Object, testPathFormatter);

			// Act
			string result = windowsLinker.GetLinkTypeArgument(testFileSystem, ConfigLink.LinkType.Symbolic, testSourcePath);

			// Assert
			Assert.IsEmpty(result);
		}

	}

}