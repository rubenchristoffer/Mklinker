using System;
using System.Linq;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using NUnit.Framework;
using Mklinker.Commands;
using Mklinker.Abstractions;
using Moq;

namespace Mklinker.Tests.Commands {

	[TestFixture]
	class ScanCommandTest {

		TestConsole testConsole;
		MockFileSystem testFileSystem;
		TestPathResolver testPathResolver;

		[SetUp]
		public void Setup () {
			testConsole = new TestConsole ();

			testFileSystem = new MockFileSystem (new Dictionary<string, MockFileData> {
				{ @"c:/config.linker", new MockFileData("<?xml version=\"1.0\" encoding=\"utf-16\"?><Config xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" Version=\"v1.1.1\"></Config>") },
				{ @"c:/invalidconfig.linker", new MockFileData("This content is invalid for a linker file!") },
				{ @"c:/demo/jQuery.js", new MockFileData("some js") },
				{ @"c:/demo/image.gif", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) },
				{ @"c:/somedirectory/", new MockDirectoryData() },

				// Simulate circular junction / symbolic directory
				// The loop folder in this case is a symlink to c:\
				// causing this kind of pattern when enumerating directories
				{ @"c:/somedirectory/loop/somedirectory/loop/somedirectory/loop/", new MockDirectoryData() }
			});

			testPathResolver = new TestPathResolver ();
		}

		[Test]
		public void Execute_WithNoLoops_WillFindNoLoops () {
			// Arrange
			ScanCommand command = new ScanCommand (@"c:/demo", 5, false, false);

			// Act
			command.Execute (testConsole, testFileSystem, testPathResolver);

			// Assert
			Assert.IsTrue (testConsole.GetHistory().Contains("no loops found", StringComparison.OrdinalIgnoreCase));
		}

		[Test]
		public void Execute_WithLoop_WillFindLoop () {
			// Arrange
			ScanCommand command = new ScanCommand (@"c:/", 5, false, false);

			// Act
			command.Execute (testConsole, testFileSystem, testPathResolver);

			// Assert
			Assert.IsTrue (testConsole.GetHistory ().Contains ("possible loops found", StringComparison.OrdinalIgnoreCase));
			Assert.IsTrue (testConsole.GetHistory ().Contains ("loop (3)", StringComparison.OrdinalIgnoreCase));
		}

		[Test]
		/**
		 * This may seem counter-intuitive because it should work the OPPOSITE in production,
		 * but since this loop is simulated it is actually a 'real' folder and should not be detected
		 * as a loop with a high recursion limit
		 */
		public void Execute_WithLoopAndHighRecursionLimit_WillNotFindLoop () {
			// Arrange
			ScanCommand command = new ScanCommand (@"c:/", 10, false, false);

			// Act
			command.Execute (testConsole, testFileSystem, testPathResolver);

			// Assert
			Assert.IsTrue (testConsole.GetHistory ().Contains ("no loops found", StringComparison.OrdinalIgnoreCase));
		}

		[Test]
		public void Execute_WithNonExistingRootFolder_WillPrintError () {
			// Arrange
			ScanCommand command = new ScanCommand (@"c:/doesnotexist", 5, false, false);

			// Act
			command.Execute (testConsole, testFileSystem, testPathResolver);

			// Assert
			Assert.IsTrue (testConsole.GetHistory ().Contains ("does not exist", StringComparison.OrdinalIgnoreCase));
		}

	}

}