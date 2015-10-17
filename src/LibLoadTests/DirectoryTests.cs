using System;
using System.IO;
using LibLoader.Helpers;
using LibLoader.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LibLoadTests
{
	[TestClass]
	public class DirectoryTests
	{
		[TestMethod]
		public void GetCorrectApplicaitonDirectory()
		{
			var curDir = AppInfoHelper.GetCurrentApplicationDirectory();
			
			Assert.IsNotNull(curDir);

		}

		[TestMethod]
		public void GetCorrectTestDirectory()
		{
			var testDir = AppInfoHelper.GetCurrentTestDirectory();

			Assert.IsNotNull(testDir.DirInfo.FullName.Contains("LibLoadTests"));
		}

		[TestMethod]
		public void DifferentDirectoryDtosAreNotEqual()
		{
			var dir1 = TestDirectories.GetTestExeDir();
			var dir2 = TestDirectories.GetTest1Dir();
			Assert.IsFalse(dir1==dir2);
		}

		[TestMethod]
		public void SameDirectoriesShouldCompareEqualDespiteTraililngSlash()
		{
			var dir1 = TestDirectories.GetTestExeDir();
			var dir2 = TestDirectories.GetAppDirWithTrailingSlash();
			var result = (dir1 == dir2);
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void DotDirectoryShouldTranslateIntoFullPath()
		{
			var dir1 = new DirectoryDto(".");

			var dir2 = TestDirectories.GetTestExeDir();

			var result = (dir1 == dir2);

			Assert.IsTrue(result);

		}

		[TestMethod]
		public void TwoDotDirectoryShouldProcessCorrectly()
		{
			var dir1 = new DirectoryDto("..\\..");
			var targetDir = "LibLoadTests";
			var dir2 = TestDirectories.GetTestExeDir();
			var idx = dir2.DirInfo.FullName.IndexOf(targetDir, StringComparison.Ordinal);
			var expectedResult = dir2.DirInfo.FullName.Substring(0, idx + targetDir.Length);
			Assert.IsTrue(dir1.DirInfo.FullName==expectedResult);

		}

		[TestMethod]
		public void PathSeparatorsShouldMatchOpSystem()
		{
			var pathSep1 = PathHelper.PrimaryPathDelimiter;
			var pathSep2 = PathHelper.AlternatePathDelimiter;

			Assert.IsTrue(pathSep1 == '\\');
			Assert.IsTrue(pathSep2 == '/');
		}

		[TestMethod]
		public void Test001IncompletePathForDtoCreation()
		{
			var testStr = @"D:\Level1\Level2\SomefileName.txt";
			var expectedResult = @"D:\Level1\Level2";

			var dir1 = new DirectoryDto(testStr);

			Assert.IsTrue(dir1.DirInfo.FullName == expectedResult);
		}

		[TestMethod]
		public void Test002IncompletePathForDtoCreation()
		{
			var testStr = @"D:\Level1\Level2\";
			var expectedResult = @"D:\Level1\Level2";

			var dir1 = new DirectoryDto(testStr);

			Assert.IsTrue(dir1.DirInfo.FullName == expectedResult);
		}

		[TestMethod]
		public void Test003IncompletePathForDtoCreation()
		{
			var testStr = @"D:\Level1\Level2";
			var expectedResult = @"D:\Level1\Level2";

			var dir1 = new DirectoryDto(testStr);

			Assert.IsTrue(dir1.DirInfo.FullName == expectedResult);
		}

		[TestMethod]
		public void Test004RelativePathForDtoCreation()
		{
			var testStr = @".\Level2";
			var expectedResult = TestDirectories.GetTestExeDir().DirInfo.FullName + "\\Level2";

			var dir1 = new DirectoryDto(testStr);

			Assert.IsTrue(dir1.DirInfo.FullName == expectedResult);
		}

		[TestMethod]
		public void Test005CorrectlyCombineTwoDirectories()
		{
			var dirStr1 = @"D:\Level1\Level2\";
			var dirStr2 = @"D:\Level1\Level2\Level3";
			var result = Path.Combine(dirStr1, dirStr2);
			Assert.IsTrue(result==dirStr2);
		}

		[TestMethod]
		public void Test006CorrectlyCombinePathAndFileName()
		{
			var dirStr1 = @"D:\Level1\Level2\"; // Directory has trailing slash
			var dirStr2 = @"SomeFile.txt";
			var result = Path.Combine(dirStr1, dirStr2);
			var expectedResult = @"D:\Level1\Level2\SomeFile.txt";
            Assert.IsTrue(result==expectedResult);
		}

		[TestMethod]
		public void Test007CombinePathAndFileNameVariant()
		{
			var dirStr1 = @"D:\Level1\Level2"; // No trailing slash
			var dirStr2 = @"SomeFile.txt";
			var result = Path.Combine(dirStr1, dirStr2);
			var expectedResult = @"D:\Level1\Level2\SomeFile.txt";
            Assert.IsTrue(result==expectedResult);
		}



	}
}
