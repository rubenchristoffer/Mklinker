using System;
using System.Collections.Generic;
using System.Text;
using Mklinker.Abstractions;
using Mklinker.Commands;
using NUnit.Framework;
using System.IO.Abstractions.TestingHelpers;
using Moq;

namespace Mklinker.Tests.Commands {

	[TestFixture]
	class ConfigCommandTest {

		const string testPath = Program.DEFAULT_LINKER_PATH;

		MockFileSystem testFileSystem;
		TestConsole testConsole;
		Config testDefaultConfig;
		ConfigHandler testConfigHandler;

		[SetUp]
		public void Setup () {
			testFileSystem = new MockFileSystem(new Dictionary<string, MockFileData> {
				{ @"c:\myfile.txt", new MockFileData("Testing is meh.") },
				{ @"c:\demo\jQuery.js", new MockFileData("some js") },
				{ @"c:\demo\image.gif", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) }
			});

			testConsole = new TestConsole("");
			testDefaultConfig = new Config("TestVersion");
			testConfigHandler = new ConfigHandler(testFileSystem, testDefaultConfig);
		}

		[Test]
		public void Execute_CreateConfig_WillCreateFile() {
			ConfigCommand configCommand = new ConfigCommand();
			configCommand.path = testPath;
			configCommand.create = true;

			configCommand.Execute(testConsole, testConfigHandler, testDefaultConfig);
			Assert.IsTrue(testFileSystem.File.Exists(testPath));
		}

	}

}
