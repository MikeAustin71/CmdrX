using CmdrX.Helpers;
using CmdrX.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CmdrXTests
{
	[TestClass]
	public class WorkingDirectoryMgrTests
	{
		[TestMethod]
		public void ShouldLocateCorrectCurrentWorkingDirector()
		{
			var appExeDir = TestDirectories.GetTestExeDir();

			var target = TestDirectories.GetMainLibLoadTestDir();

            var dirMgr = new WorkingDirectoryMgr();
			dirMgr.SetTargetDirectory(target);

			var currDir = dirMgr.SetCurrentWorkingDirectory();

			Assert.IsTrue(dirMgr.OriginalCurrentWorkingDirectory == appExeDir);
			Assert.IsTrue(currDir==appExeDir);
		}

		[TestMethod]
		public void ShouldCorrectlyChangeDirectories()
		{
			var appExeDir = TestDirectories.GetTestExeDir();

			var target = TestDirectories.GetMainLibLoadTestDir();

			var dirMgr = new WorkingDirectoryMgr();
			dirMgr.SetTargetDirectory(target);

			var originalDir = dirMgr.SetCurrentWorkingDirectory();

			dirMgr.ChangeToTargetWorkingDirectory();

			var dir1 = DirectoryHelper.GetCurrentEnvironmentDirectory();

			var dir2 = DirectoryHelper.GetCurrentDirectory();

			Assert.IsTrue(target==dir1);
			Assert.IsTrue(target==dir2);
			Assert.IsTrue(dir1!=originalDir);
			Assert.IsTrue(appExeDir==originalDir);

			DirectoryHelper.ChangeToNewCurrentDirectory(appExeDir);

			dir1 = DirectoryHelper.GetCurrentEnvironmentDirectory();
			dir2 = DirectoryHelper.GetCurrentDirectory();

			Assert.IsTrue(dir1==appExeDir);
			Assert.IsTrue(dir2==appExeDir);
		}

		[TestMethod]
		public void DirectoryMgrShouldChangeDirectoriesAndReturn()
		{

			var appExeDir = TestDirectories.GetTestExeDir();

			var target = TestDirectories.GetMainLibLoadTestDir();

			var dirMgr = new WorkingDirectoryMgr();
			dirMgr.SetTargetDirectory(target);

			dirMgr.ChangeToTargetWorkingDirectory();

			Assert.IsTrue(appExeDir==dirMgr.OriginalCurrentWorkingDirectory);

			var dir1 = DirectoryHelper.GetCurrentEnvironmentDirectory();

			var dir2 = DirectoryHelper.GetCurrentDirectory();

			Assert.IsTrue(dir1==target);
			Assert.IsTrue(dir2==target);

			var result = dirMgr.ChangeBackToOriginalWorkingDirectory();

			Assert.IsTrue(result);

			dir1 = DirectoryHelper.GetCurrentEnvironmentDirectory();

			dir2 = DirectoryHelper.GetCurrentDirectory();

			Assert.IsTrue(dir1==appExeDir);
			Assert.IsTrue(dir2==appExeDir);

		}

	}
}