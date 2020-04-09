using System;
using System.Linq;
using System.Collections.Generic;
using System.IO.Abstractions;
using NUnit.Framework;
using Mklinker.Commands;
using Mklinker.Abstractions;
using Moq;

namespace Mklinker.Tests {

	[TestFixture]
	class PathResolverTest {

		Mock<IConfig> testConfig;
		Mock<IFileSystem> testFileSystem;

		[SetUp]
		public void Setup() {
			testConfig = new Mock<IConfig>();
			testConfig.Setup(m => m.Variables).Returns(new List<Variable>());

			testFileSystem = new Mock<IFileSystem>();
		}

		[Test]
		public void GetAbsoluteResolvedPath_WithNormalPath_ShouldReturnNormalPath() {
			// Arrange
			const string testPath = @"C:/MyPath";
			PathResolver pathFormatter = new PathResolver(testFileSystem.Object);
			testFileSystem.Setup(m => m.Path.GetFullPath(It.IsAny<string>())).Returns<string>(val => val);

			// Act
			string result = pathFormatter.GetAbsoluteResolvedPath(testPath, new List<Variable>());

			// Assert
			Assert.AreEqual(testPath, result);
		}

		[Test]
		public void GetAbsoluteResolvedPath_WithVariablePath_ShouldReturnResolvedPath() {
			// Arrange
			string testPath = $@"C:/Users/{ PathResolver.delimiter }User{ PathResolver.delimiter }/Desktop";
			PathResolver pathFormatter = new PathResolver(testFileSystem.Object);
			var testVariables = new List<Variable>(new Variable[] { new Variable("User", "Frans") });
			testFileSystem.Setup(m => m.Path.GetFullPath(It.IsAny<string>())).Returns<string>(val => val);

			// Act
			string result = pathFormatter.GetAbsoluteResolvedPath(testPath, testVariables);

			// Assert
			Assert.AreEqual(@"C:/Users/Frans/Desktop", result);
		}

		[Test]
		public void GetAbsoluteResolvedPath_WithNestedVariablePath_ShouldReturnResolvedPath() {
			// Arrange
			string testPath = $@"{ PathResolver.delimiter }UserPath{ PathResolver.delimiter }";
			PathResolver pathFormatter = new PathResolver(testFileSystem.Object);
			testFileSystem.Setup(m => m.Path.GetFullPath(It.IsAny<string>())).Returns<string>(val => val);

			var testVariables = new List<Variable>(new Variable[] { 
				new Variable("User", "Frans"),
				new Variable("UserPath", $@"C:/Users/{ PathResolver.delimiter }User{ PathResolver.delimiter }/Desktop")
			});

			// Act
			string result = pathFormatter.GetAbsoluteResolvedPath(testPath, testVariables);

			// Assert
			Assert.AreEqual(@"C:/Users/Frans/Desktop", result);
		}

		[Test]
		public void GetAbsoluteResolvedPath_WithInvalidNestedVariablePath_ShouldReturnUnresolvedPath() {
			// Arrange
			string testPath = $@"{ PathResolver.delimiter }UserPath{ PathResolver.delimiter }";
			PathResolver pathFormatter = new PathResolver(testFileSystem.Object);
			testFileSystem.Setup(m => m.Path.GetFullPath(It.IsAny<string>())).Returns<string>(val => val);

			var testVariables = new List<Variable>(new Variable[] {
				new Variable("UserPath", $@"C:/Users/{ PathResolver.delimiter }User{ PathResolver.delimiter }/Desktop")
			});

			// Act
			string result = pathFormatter.GetAbsoluteResolvedPath(testPath, testVariables);

			// Assert
			Assert.AreEqual($@"C:/Users/{ PathResolver.delimiter }User{ PathResolver.delimiter }/Desktop", result);
		}

		[Test]
		public void GetAbsoluteResolvedPath_WithVariablePathButWithoutVariable_ShouldReturnUnresolvedPath() {
			// Arrange
			string testPath = $@"C:/Users/{ PathResolver.delimiter }User{ PathResolver.delimiter }/Desktop";
			PathResolver pathFormatter = new PathResolver(testFileSystem.Object);
			var testVariables = new List<Variable>();
			testFileSystem.Setup(m => m.Path.GetFullPath(It.IsAny<string>())).Returns<string>(val => val);

			// Act
			string result = pathFormatter.GetAbsoluteResolvedPath(testPath, testVariables);

			// Assert
			Assert.AreEqual(testPath, result);
		}

	}

}