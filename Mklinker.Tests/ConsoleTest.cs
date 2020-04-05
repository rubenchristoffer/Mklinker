using System;
using System.Linq;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.IO;
using NUnit.Framework;
using Mklinker.Commands;
using Mklinker.Abstractions;
using Moq;

namespace Mklinker.Tests {

	[TestFixture]
	class ConsoleTest {

		Mock<TextReader> testIn;
		Mock<TextWriter> testOut;

		[SetUp]
		public void Setup() {
			testIn = new Mock<TextReader>();
			testOut = new Mock<TextWriter>();

			System.Console.SetIn(testIn.Object);
			System.Console.SetOut(testOut.Object);
		}

		[Test]
		public void WriteLine_WillWriteToOut() {
			// Arrange
			const string testText = "some line here";
			IConsole console = new Console();

			// Act
			console.WriteLine(testText);

			// Assert
			testOut.Verify(m => m.WriteLine(testText));
		}

		[Test]
		public void WriteLine_Formatted_WillWriteToOut() {
			// Arrange
			const string testText = "{0} was very {1}";
			const string arg1 = "The sun";
			const string arg2 = "hot";
			IConsole console = new Console();

			// Act
			console.WriteLine(testText, arg1, arg2);

			// Assert
			testOut.Verify(m => m.WriteLine(testText, new string[] { arg1, arg2 }));
		}

		[Test]
		public void Write_WillWriteToOut() {
			// Arrange
			const string testText = "some line here";
			IConsole console = new Console();

			// Act
			console.Write(testText);

			// Assert
			testOut.Verify(m => m.Write(testText));
		}

		[Test]
		public void ReadLine_WillWriteToOut() {
			// Arrange
			IConsole console = new Console();

			// Act
			console.ReadLine();

			// Assert
			testIn.Verify(m => m.ReadLine());
		}

	}

}
