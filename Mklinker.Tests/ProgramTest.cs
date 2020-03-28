using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NUnit.Framework;

namespace Mklinker.Tests {

	[TestFixture]
	class ProgramTest : TestBase {

		private const string TEST_CONFIG_FILE = "test.linker";

		[SetUp]
		[TearDown]
		public void Reset () {
			File.Delete(TEST_CONFIG_FILE);
			Assert.IsTrue(!File.Exists(TEST_CONFIG_FILE));
		}

		[Test]
		public void ParseStringToArguments () {
			Assert.AreEqual(new string[] { "cmd", "arg" }, Program.ParseStringToArguments("cmd arg"));
			Assert.AreEqual(new string[] { "cmd", "arg" }, Program.ParseStringToArguments("cmd \"arg\""));

			Assert.AreEqual(new string[] { "cmd", "arg1", "arg2" }, Program.ParseStringToArguments("cmd arg1 arg2"));
			Assert.AreEqual(new string[] { "cmd", "arg1", "arg2" }, Program.ParseStringToArguments("cmd \"arg1\" arg2"));
			Assert.AreEqual(new string[] { "cmd", "arg1", "arg2" }, Program.ParseStringToArguments("cmd arg1 \"arg2\""));
			Assert.AreEqual(new string[] { "cmd", "arg1", "arg2" }, Program.ParseStringToArguments("cmd \"arg1\" \"arg2\""));

			Assert.AreEqual(new string[] { "cmd", "arg 1" }, Program.ParseStringToArguments("cmd \"arg 1\""));
			Assert.AreEqual(new string[] { "cmd", "arg 1", "arg 2" }, Program.ParseStringToArguments("cmd \"arg 1\" \"arg 2\""));
			Assert.AreEqual(new string[] { "cmd", "this is just one argument" }, Program.ParseStringToArguments("cmd \"this is just one argument\""));

			Assert.AreEqual(new string[] { "cmd", "arg" }, Program.ParseStringToArguments("\"cmd\" arg"));
		}

		[Test]
		public void CreateNewConfig() {
			Assert.DoesNotThrow(() => Program.CreateNewConfig());

			Assert.IsNotNull(Program.config);
		}

		[Test]
		public void SaveConfig() {
			CreateNewConfig();

			Assert.DoesNotThrow(() => Program.SaveConfig(TEST_CONFIG_FILE));
			Assert.IsTrue(File.Exists(TEST_CONFIG_FILE));
		}

		[Test]
		public void LoadConfig() {
			SaveConfig();

			Assert.DoesNotThrow(() => Program.LoadConfig(TEST_CONFIG_FILE));
			Assert.AreEqual(Program.GetVersion(), Program.config.version);
		}

	}

}
