using System;
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
		public void PathSeparatorsShouldMatchOpSystem()
		{
			var pathSep1 = PathHelper.PrimaryPathDelimiter;
			var pathSep2 = PathHelper.AlternatePathDelimiter;

			Assert.IsTrue(pathSep1 == '\\');
			Assert.IsTrue(pathSep2 == '/');
		}
	}
}
