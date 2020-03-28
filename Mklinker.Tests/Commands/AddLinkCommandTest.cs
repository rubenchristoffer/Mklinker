using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace Mklinker.Tests.Commands {

	[TestFixture]
	class AddLinkCommandTest : TestBase {

		private const string testfile1 = "testfile1";
		private const string testfile2 = "testfile2.txt";

		private const string testdir1 = "testdir1";
		private const string testdir2 = "testdir2";

		[SetUp]
		public void Setup () {
			if (!File.Exists(testfile1)) File.Create(testfile1).Close();
			if (!File.Exists(testfile2)) File.Create(testfile2).Close();

			if (!Directory.Exists(testdir1)) Directory.CreateDirectory(testdir1);
			if (!Directory.Exists(testdir2)) Directory.CreateDirectory(testdir2);

			Program.CreateNewConfig();
			Program.SaveConfig(Config.DEFAULT_CONFIG_FILE);
		}

		[TearDown]
		public void Cleanup () {
			if (File.Exists(testfile1)) File.Delete(testfile1);
			if (File.Exists(testfile2)) File.Delete(testfile2);

			if (Directory.Exists(testdir1)) Directory.Delete(testdir1, true);
			if (Directory.Exists(testdir2)) Directory.Delete(testdir2, true);
		}

		[Test]
		public void AddLink_NoArguments () {
			if (File.Exists(Config.DEFAULT_CONFIG_FILE)) File.Delete(Config.DEFAULT_CONFIG_FILE);

			Program.ParseAndExecute(Program.ParseStringToArguments("addlink"));
			Assert.IsFalse(File.Exists(Config.DEFAULT_CONFIG_FILE));
		}

		[Test]
		public void AddLink_File_TargetAndSourceOnly() {
			Program.ParseAndExecute(Program.ParseStringToArguments(String.Format("addlink {0} {1}", testfile1, testfile2)));
			Assert.IsTrue(Program.config.linkList.Any (link => link.targetPath.Equals(testfile1) && link.sourcePath.Equals(testfile2)));

			Program.ParseAndExecute(Program.ParseStringToArguments(String.Format("addlink {0} {1}", testfile2, testfile1)));
			Assert.IsTrue(Program.config.linkList.Any(link => link.targetPath.Equals(testfile2) && link.sourcePath.Equals(testfile1)));
		}

		[Test]
		public void AddLink_File_Hard() {
			Program.ParseAndExecute(Program.ParseStringToArguments(String.Format("addlink {0} {1} Hard", testfile1, testfile2)));
			Assert.IsTrue(Program.config.linkList.Any(link => 
				link.targetPath.Equals(testfile1) && link.sourcePath.Equals(testfile2) && link.linkType == ConfigLink.LinkType.Hard)
			);
		}

		[Test]
		public void AddLink_File_Symbolic() {
			Program.ParseAndExecute(Program.ParseStringToArguments(String.Format("addlink {0} {1} Symbolic", testfile1, testfile2)));
			Assert.IsTrue(Program.config.linkList.Any(link =>
				link.targetPath.Equals(testfile1) && link.sourcePath.Equals(testfile2) && link.linkType == ConfigLink.LinkType.Symbolic)
			);
		}

		[Test]
		public void AddLink_Folder_TargetAndSourceOnly() {
			Program.ParseAndExecute(Program.ParseStringToArguments(String.Format("addlink {0} {1}", testdir1, testdir2)));
			Assert.IsTrue(Program.config.linkList.Any(link => link.targetPath.Equals(testdir1) && link.sourcePath.Equals(testdir2)));

			Program.ParseAndExecute(Program.ParseStringToArguments(String.Format("addlink {0} {1}", testdir2, testdir1)));
			Assert.IsTrue(Program.config.linkList.Any(link => link.targetPath.Equals(testdir2) && link.sourcePath.Equals(testdir1)));
		}

		[Test]
		public void AddLink_Folder_Symbolic() {
			Program.ParseAndExecute(Program.ParseStringToArguments(String.Format("addlink {0} {1} Symbolic", testdir1, testdir2)));
			Assert.IsTrue(Program.config.linkList.Any(link =>
				link.targetPath.Equals(testdir1) && link.sourcePath.Equals(testdir2) && link.linkType == ConfigLink.LinkType.Symbolic)
			);
		}

		[Test]
		public void AddLink_Folder_Junction() {
			Program.ParseAndExecute(Program.ParseStringToArguments(String.Format("addlink {0} {1} Junction", testdir1, testdir2)));
			Assert.IsTrue(Program.config.linkList.Any(link =>
				link.targetPath.Equals(testdir1) && link.sourcePath.Equals(testdir2) && link.linkType == ConfigLink.LinkType.Junction)
			);
		}

	}

}
