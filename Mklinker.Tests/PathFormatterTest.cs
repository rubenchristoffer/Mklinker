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
		MockFileSystem testFileSystem;

		[SetUp]
		public void Setup() {
			testConfig = new Mock<IConfig>();
			testConfig.Setup(m => m.Variables).Returns(new List<Variable>());

			testFileSystem = new MockFileSystem();
		}

		[Test]
		public void GetAbsoluteResolvedPath_WithNormalPath_ShouldReturnNormalPath() {
			// Arrange
			const string testPath = @"C:\MyPath";
			PathResolver pathFormatter = new PathResolver(testFileSystem);

			// Act
			string result = pathFormatter.GetAbsoluteResolvedPath(testPath, new List<Variable>());

			// Assert
			Assert.AreEqual(result, testPath);
		}

		[Test]
		public void GetAbsoluteResolvedPath_WithVariablePath_ShouldReturnResolvedPath() {
			// Arrange
			string testPath = $@"C:\Users\{ PathResolver.delimiter }User{ PathResolver.delimiter }\Desktop";
			PathResolver pathFormatter = new PathResolver(testFileSystem);
			var testVariables = new List<Variable>(new Variable[] { new Variable("User", "Frans") });

			// Act
			string result = pathFormatter.GetAbsoluteResolvedPath(testPath, testVariables);

			// Assert
			Assert.AreEqual(result, @"C:\Users\Frans\Desktop");
		}

		[Test]
		public void GetAbsoluteResolvedPath_WithVariablePathButWithoutVariable_ShouldReturnUnresolvedPath() {
			// Arrange
			string testPath = $@"C:\Users\{ PathResolver.delimiter }User{ PathResolver.delimiter }\Desktop";
			PathResolver pathFormatter = new PathResolver(testFileSystem);
			var testVariables = new List<Variable>();

			// Act
			string result = pathFormatter.GetAbsoluteResolvedPath(testPath, testVariables);

			// Assert
			Assert.AreEqual(result, testPath);
		}

	}

}