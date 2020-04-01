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

		IConsole testConsole;

		[SetUp]
		public void Setup () {
			testConsole = new TestConsole("");
		}

		[Test]
		public void Execute_CreateConfig_WillSucceed() {
			const string testPath = "linker.config";

			var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData> {
				{ @"c:\myfile.txt", new MockFileData("Testing is meh.") },
				{ @"c:\demo\jQuery.js", new MockFileData("some js") },
				{ @"c:\demo\image.gif", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) }
			});

			IConfig defaultConfig = new Config("TestVersion");
			IConfigHandler configHandler = new ConfigHandler(fileSystem, defaultConfig);

			ConfigCommand configCommand = new ConfigCommand();
			configCommand.path = testPath;
			configCommand.create = true;

			configCommand.Execute(testConsole, configHandler, defaultConfig);
			Assert.IsTrue(fileSystem.File.Exists(testPath));
		}

	}

}
