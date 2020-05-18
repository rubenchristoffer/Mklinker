using System;
using System.Linq;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using NUnit.Framework;
using Mklinker.Commands;
using Mklinker.Abstractions;
using Moq;
using NUnit.Framework.Internal;

namespace Mklinker.Tests.Commands {

	[TestFixture]
	class AddLinksCommandTest {

		TestConsole testConsole;
		MockFileSystem testFileSystem;
		Mock<IConfigHandler> testConfigHandler;
		Mock<IConfig> testConfig;
		List<ConfigLink> testLinks;
		TestPathResolver testPathFormatter;

		[SetUp]
		public void Setup () {
			testFileSystem = new MockFileSystem (new Dictionary<string, MockFileData> {
				{ @"c:/testfile1.txt", new MockFileData("random data") },
				{ @"c:/testfile2.txt", new MockFileData("random data") },
				{ @"c:/testfile3.txt", new MockFileData("random data") },

				{ @"c:/subdir1/testfile4.txt", new MockFileData("random data") },
				{ @"c:/subdir1/testfile5.txt", new MockFileData("random data") },
				{ @"c:/subdir1/testfile6.txt", new MockFileData("random data") },

				{ @"c:/subdir2/testfile7.txt", new MockFileData("random data") },
				{ @"c:/subdir2/testfile8.txt", new MockFileData("random data") },

				{ @"c:/subdir2/subdir/subdir/testfile8.txt", new MockFileData("random data") },
			});

			testLinks = new List<ConfigLink> ();

			testConsole = new TestConsole ();
			testConfigHandler = new Mock<IConfigHandler> ();
			testConfigHandler.Setup (m => m.DoesConfigExist (It.IsAny<string> ())).Returns (true);

			testConfig = new Mock<IConfig> ();
			testConfig.Setup (m => m.LinkList).Returns (testLinks);
			testPathFormatter = new TestPathResolver ();

			testConfigHandler.Setup (m => m.LoadConfig (It.IsAny<string> ())).Returns (testConfig.Object);
		}

		[Test]
		public void Execute_WithNoConfig_ShouldPrintError () {
			// Arrange
			AddLinksCommand command = new AddLinksCommand ("c:/", ".", ConfigLink.LinkType.Default, @"[\s\S]*", @"[\s\S]*", false, false, null);
			testConfigHandler.Setup (m => m.DoesConfigExist (It.IsAny<string> ())).Returns (false);

			// Act
			command.Execute (testConsole, testConfigHandler.Object, testFileSystem, testPathFormatter);

			// Assert
			Assert.IsTrue (testConsole.GetHistory().Contains("does not exist", StringComparison.OrdinalIgnoreCase));
		}

		[TestCase ("testfile1.txt", true)]
		[TestCase ("testfile2.txt", true)]
		[TestCase ("testfile3.txt", true)]
		[TestCase ("subdir1/testfile4.txt", false)]
		[TestCase ("subdir1/testfile5.txt", false)]
		[TestCase ("subdir1/testfile6.txt", false)]
		[TestCase ("subdir2/testfile7.txt", false)]
		[TestCase ("subdir2/testfile8.txt", false)]
		[TestCase ("subdir2/subdir/subdir/testfile8.txt", false)]
		public void Execute_NonRecursiveFiles (string fileName, bool shouldExist) {
			// Arrange
			AddLinksCommand command = new AddLinksCommand ("c:/", ".", ConfigLink.LinkType.Default, @"[\s\S]*", @"[\s\S]*", false, false, null);

			// Act
			command.Execute (testConsole, testConfigHandler.Object, testFileSystem, testPathFormatter);

			// Assert
			if (shouldExist) {
				Assert.IsTrue (testLinks.Any (link => link.sourcePath.EndsWith ("c:/" + fileName) && link.targetPath.EndsWith ("./" + fileName)));
			} else {
				Assert.IsFalse (testLinks.Any (link => link.sourcePath.EndsWith ("c:/" + fileName) && link.targetPath.EndsWith ("./" + fileName)));
			}
		}

		[TestCase ("testfile1.txt", true)]
		[TestCase ("testfile2.txt", true)]
		[TestCase ("testfile3.txt", true)]
		[TestCase ("subdir1/testfile4.txt", true)]
		[TestCase ("subdir1/testfile5.txt", true)]
		[TestCase ("subdir1/testfile6.txt", true)]
		[TestCase ("subdir2/testfile7.txt", true)]
		[TestCase ("subdir2/testfile8.txt", true)]
		[TestCase ("subdir2/subdir/subdir/testfile8.txt", true)]
		[TestCase ("subdir2/subdir/subdir/testfile8.txt", true, @"testfile8.txt")]
		[TestCase ("subdir2/subdir/subdir/testfile8.txt", true, @"test[f]+ile[0-9]*.txt")]
		[TestCase ("subdir2/subdir/subdir/testfile8.txt", false, @"test[f]+ile[0-7]*.txt")]
		[TestCase ("subdir2/subdir/subdir/testfile8.txt", false, @"test[f]+ile[0-9]*.txt", @"willnotmatch")]
		[TestCase ("subdir2/subdir/subdir/testfile8.txt", true, @"test[f]+ile[0-9]*.txt", @"subdir")]
		public void Execute_RecursiveFiles (string relativePath, bool shouldExist, string regex = @"[\s\S]*", string absoluteRegex = @"[\s\S]*") {
			// Arrange
			AddLinksCommand command = new AddLinksCommand ("c:/", ".", ConfigLink.LinkType.Default, regex, absoluteRegex, true, false, null);

			// Act
			command.Execute (testConsole, testConfigHandler.Object, testFileSystem, testPathFormatter);

			// Assert
			if (shouldExist) {
				Assert.IsTrue (testLinks.Any (link => link.sourcePath.EndsWith ("c:/" + relativePath) && link.targetPath.EndsWith ("./" + relativePath)));
			} else {
				Assert.IsFalse (testLinks.Any (link => link.sourcePath.EndsWith ("c:/" + relativePath) && link.targetPath.EndsWith ("./" + relativePath)));
			}
		}

		[TestCase ("subdir1", true)]
		[TestCase ("subdir2", true)]
		[TestCase ("subdir2/subdir", false)]
		public void Execute_AddDirectories (string relativePath, bool shouldExist) {
			// Arrange
			AddLinksCommand command = new AddLinksCommand ("c:/", ".", ConfigLink.LinkType.Default, @"[\s\S]*", @"[\s\S]*", false, true, null);

			// Act
			command.Execute (testConsole, testConfigHandler.Object, testFileSystem, testPathFormatter);

			// Assert
			if(shouldExist) {
				Assert.IsTrue (testLinks.Any (link => link.sourcePath.EndsWith ("c:/" + relativePath) && link.targetPath.EndsWith ("./" + relativePath)));
			} else {
				Assert.IsFalse (testLinks.Any (link => link.sourcePath.EndsWith ("c:/" + relativePath) && link.targetPath.EndsWith ("./" + relativePath)));
			}
		}

		[Test]
		public void Execute_WithInvalidRegex_ShouldPrintError () {
			// Arrange
			AddLinksCommand command = new AddLinksCommand ("c:/", "", ConfigLink.LinkType.Default, "[", "", false, false, "");

			// Act
			command.Execute (testConsole, testConfigHandler.Object, testFileSystem, testPathFormatter);

			// Assert
			Assert.IsTrue (testConsole.GetHistory().Contains("regex", StringComparison.OrdinalIgnoreCase));
			Assert.IsTrue (testConsole.GetHistory ().Contains ("invalid", StringComparison.OrdinalIgnoreCase));
		}

	}

}
