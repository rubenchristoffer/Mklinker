using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace Mklinker.Tests.Commands {

	[TestFixture]
	class ListCommandTest : TestBase {

		private const string TEST_CONFIG_FILE = "listcommandtest.linker";

		private StringWriter testOut;

		protected override void AfterBaseSetup() {
			Program.CreateNewConfig();
			Program.SaveConfig(TEST_CONFIG_FILE);
			Assert.IsTrue(File.Exists(TEST_CONFIG_FILE));

			testOut = new StringWriter();
			Console.SetOut(testOut);
		}

		[Test]
		public void ListConfig_Empty () {
			Program.ParseAndExecute(Program.ParseStringToArguments("list --path " + TEST_CONFIG_FILE));
			string output = testOut.ToString();
			
			Assert.IsTrue(output.Contains("empty", StringComparison.OrdinalIgnoreCase));
		}

		[Test]
		public void ListConfig_OneElement() {
			Program.config.linkList.Add(new ConfigLink("testSource", "testTarget", ConfigLink.LinkType.Default));
			Program.SaveConfig(TEST_CONFIG_FILE);

			Program.ParseAndExecute(Program.ParseStringToArguments("list --path " + TEST_CONFIG_FILE));
			string output = testOut.ToString();

			Assert.IsTrue(output.Contains("testSource", StringComparison.OrdinalIgnoreCase));
			Assert.IsTrue(output.Contains("testTarget", StringComparison.OrdinalIgnoreCase));
			Assert.IsTrue(output.Contains("default", StringComparison.OrdinalIgnoreCase));
		}

	}

}
