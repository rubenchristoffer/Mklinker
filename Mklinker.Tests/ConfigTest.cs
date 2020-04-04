using System;
using System.Linq;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using NUnit.Framework;
using Mklinker.Commands;
using Mklinker.Abstractions;
using Moq;

namespace Mklinker.Tests {

	[TestFixture]
	class ConfigTest {

		[Test]
		public void Constructor_NoArguments () {
			// Arrange
			Config config = new Config();

			// Act

			// Assert
			Assert.IsNotNull(config.LinkList);
			Assert.IsNotNull(config.Variables);
			Assert.AreEqual(config.Version, "Undefined");
		}

		[Test]
		public void Constructor_WithVersion() {
			// Arrange
			const string testVersion = "my custom awesome version";
			Config config = new Config(testVersion);

			// Act

			// Assert
			Assert.IsNotNull(config.LinkList);
			Assert.IsNotNull(config.Variables);
			Assert.AreEqual(config.Version, testVersion);
		}

	}

}
