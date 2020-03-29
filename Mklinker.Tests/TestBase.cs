using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Mklinker.Tests {

	public abstract class TestBase {

		[SetUp]
		public void TestBase_Setup () {
			typeof(Program).GetProperty("config", BindingFlags.Static | BindingFlags.Public).SetValue(null, null);
			Assert.IsNull(Program.config);

			TestBase_Cleanup();

			Assert.IsTrue(!File.Exists(Config.DEFAULT_CONFIG_FILE));

			AfterBaseSetup();
		}

		[TearDown]
		public void TestBase_Cleanup() {
			if (File.Exists(Config.DEFAULT_CONFIG_FILE))
				File.Delete(Config.DEFAULT_CONFIG_FILE);
		}

		// To ensure that it doesn't conflict with TestBase_Setup
		protected virtual void AfterBaseSetup() {}

	}

}
