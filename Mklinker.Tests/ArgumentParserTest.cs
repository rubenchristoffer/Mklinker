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
	class ArgumentParserTest {

		[Test]
		public void ParseStringToArguments_WithOneArgument_WillReturnLengthOneArray() {
			// Arrange
			const string testInput = "arg";
			ArgumentParser parser = new ArgumentParser();

			// Act
			string[] result = parser.ParseStringToArguments(testInput);

			// Assert
			Assert.AreEqual(result, new string[] { "arg" });
		}

		[Test]
		public void ParseStringToArguments_WithTwoArguments_WillReturnLengthTwoArray() {
			// Arrange
			const string testInput = "arg1 arg2";
			ArgumentParser parser = new ArgumentParser();

			// Act
			string[] result = parser.ParseStringToArguments(testInput);

			// Assert
			Assert.AreEqual(result, new string[] { "arg1", "arg2" });
		}

		[Test]
		public void ParseStringToArguments_WithOneQuotedArgument_WillReturnLengthOneArray() {
			// Arrange
			const string testInput = "\"this is one single arg\"";
			ArgumentParser parser = new ArgumentParser();

			// Act
			string[] result = parser.ParseStringToArguments(testInput);

			// Assert
			Assert.AreEqual(result, new string[] { "this is one single arg" });
		}

		[Test]
		public void ParseStringToArguments_WithTwoQuotedArguments_WillReturnLengthTwoArray() {
			// Arrange
			const string testInput = "\"this is one single arg\" \"this is yet another arg\"";
			ArgumentParser parser = new ArgumentParser();

			// Act
			string[] result = parser.ParseStringToArguments(testInput);

			// Assert
			Assert.AreEqual(result, new string[] { "this is one single arg", "this is yet another arg" });
		}

	}

}
