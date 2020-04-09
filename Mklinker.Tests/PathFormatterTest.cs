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
	class PathFormatterTests {

		Mock<IConfig> testConfig;

		[SetUp]
		public void Setup() {
			testConfig = new Mock<IConfig>();
			testConfig.Setup(m => m.Variables).Returns(new List<Variable>());
		}

		[Test]
		public void GetFormattedPath_WithNormalPath_ShouldReturnNormalPath() {
			// Arrange
			const string testPath = @"C:\MyPath";
			PathFormatter pathFormatter = new PathFormatter(testConfig.Object);

			// Act
			string result = pathFormatter.GetFormattedPath(testPath);

			// Assert
			Assert.AreEqual(result, testPath);
		}

		[Test]
		public void GetFormattedPath_WithVariablePath_ShouldReturnFormattedPath() {
			// Arrange
			string testPath = $@"C:\Users\{ PathFormatter.delimiter }User{ PathFormatter.delimiter }\Desktop";
			PathFormatter pathFormatter = new PathFormatter(testConfig.Object);
			testConfig.Setup(m => m.Variables).Returns(new List<Variable>(new Variable[] { new Variable("User", "Frans") }));

			// Act
			string result = pathFormatter.GetFormattedPath(testPath);

			// Assert
			Assert.AreEqual(result, @"C:\Users\Frans\Desktop");
		}

		[Test]
		public void GetFormattedPath_WithVariablePathButWithoutVariable_ShouldReturnUnformattedPath() {
			// Arrange
			string testPath = $@"C:\Users\{ PathFormatter.delimiter }User{ PathFormatter.delimiter }\Desktop";
			PathFormatter pathFormatter = new PathFormatter(testConfig.Object);

			// Act
			string result = pathFormatter.GetFormattedPath(testPath);

			// Assert
			Assert.AreEqual(result, testPath);
		}

	}

}