using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Mklinker.Tests {

	[TestFixture]
	class ProgramTest {

		private const string CONFIG_FILE = "test.linker";

		[SetUp]
		public void Reset() {
			typeof(Program).GetProperty("config", BindingFlags.Static | BindingFlags.Public).SetValue(null, null);
			Assert.IsNull(Program.config);

			File.Delete(CONFIG_FILE);
			Assert.IsTrue(!File.Exists(CONFIG_FILE));
		}

		[Test]
		public void CreateNewConfig() {
			Assert.DoesNotThrow(() => Program.CreateNewConfig());

			Assert.IsNotNull(Program.config);
		}

		[Test]
		public void SaveConfig() {
			CreateNewConfig();

			Assert.DoesNotThrow(() => Program.SaveConfig(CONFIG_FILE));
			Assert.IsTrue(File.Exists(CONFIG_FILE));
		}

		[Test]
		public void LoadConfig() {
			SaveConfig();

			Assert.DoesNotThrow(() => Program.LoadConfig(CONFIG_FILE));
			Assert.AreEqual(Program.GetVersion(), Program.config.version);
		}

	}

}
