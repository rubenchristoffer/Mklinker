using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using NUnit.Framework;

namespace Mklinker.Tests.Commands {

	[TestFixture]
	class ConfigCommandTest : TestBase {

		private const string customFile = "custom file.config";
		private const string customFileArg = "\"" + customFile + "\"";

		[SetUp]
		[TearDown]
		public void Reset () {
			if (File.Exists(customFile))
				File.Delete(customFile);

			Assert.IsFalse(File.Exists(customFile));
		}

		[Test]
		public void CreateConfig_Default () {
			Program.ParseAndExecute(Program.ParseStringToArguments("config --create"));
			Assert.IsTrue (File.Exists(Config.DEFAULT_CONFIG_FILE));
		}

		[Test]
		public void CreateConfigShort_Default() {
			Program.ParseAndExecute(Program.ParseStringToArguments("config -c"));
			Assert.IsTrue(File.Exists(Config.DEFAULT_CONFIG_FILE));
		}

		[Test]
		public void CreateConfig_CustomFile() {
			Program.ParseAndExecute(Program.ParseStringToArguments("config --create --path " + customFileArg));
			Assert.IsTrue(File.Exists(customFile));
		}

		[Test]
		public void CreateConfigShort_CustomFile() {
			Program.ParseAndExecute(Program.ParseStringToArguments("config -c -p " + customFileArg));
			Assert.IsTrue(File.Exists(customFile));
		}

		[Test]
		public void DeleteConfig_Default () {
			if (!File.Exists(Config.DEFAULT_CONFIG_FILE))
				File.Create(Config.DEFAULT_CONFIG_FILE).Close();

			Assert.IsTrue(File.Exists(Config.DEFAULT_CONFIG_FILE));

			Program.ParseAndExecute(Program.ParseStringToArguments("config --delete"));
			Assert.IsFalse(File.Exists(Config.DEFAULT_CONFIG_FILE));
		}

		[Test]
		public void DeleteConfigShort_Default() {
			if (!File.Exists(Config.DEFAULT_CONFIG_FILE))
				File.Create(Config.DEFAULT_CONFIG_FILE).Close();

			Assert.IsTrue(File.Exists(Config.DEFAULT_CONFIG_FILE));

			Program.ParseAndExecute(Program.ParseStringToArguments("config -d"));
			Assert.IsFalse(File.Exists(Config.DEFAULT_CONFIG_FILE));
		}

		[Test]
		public void DeleteConfig_CustomFile() {
			if (!File.Exists(customFile))
				File.Create(customFile).Close();

			Assert.IsTrue(File.Exists(customFile));

			Program.ParseAndExecute(Program.ParseStringToArguments("config --delete --path " + customFileArg));
			Assert.IsFalse(File.Exists(customFile));
		}

		[Test]
		public void DeleteConfigShort_CustomFile() {
			if (!File.Exists(customFile))
				File.Create(customFile).Close();

			Assert.IsTrue(File.Exists(customFile));

			Program.ParseAndExecute(Program.ParseStringToArguments("config -d -p " + customFileArg));
			Assert.IsFalse(File.Exists(customFile));
		}

	}

}
