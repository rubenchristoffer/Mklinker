using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using NUnit.Framework;

namespace Mklinker.Tests.Commands {

	[TestFixture]
	class InteractiveCommandTest {

		StringWriter testOut;

		[SetUp]
		public void Setup () {
			StringReader testIn = new StringReader(
				"help\n" +
				"exit\n"
			);

			testOut = new StringWriter();

			Console.SetIn(testIn);
			Console.SetOut(testOut);
		}

		[Test]
		public void TestInteractiveMode () {
			Program.ParseAndExecute(Program.ParseStringToArguments("interactive"));

			string output = testOut.ToString();

			Assert.IsTrue(output.Contains("Mklinker"));
			Assert.IsTrue(output.Contains("help"));
			Assert.IsTrue(output.Contains("version"));
		}
	}

}
