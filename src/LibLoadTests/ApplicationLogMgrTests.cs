using System.Configuration;
using LibLoader.Managers;
using LibLoader.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LibLoadTests
{
	[TestClass]
	public class ApplicationLogMgrTests
	{
		[TestMethod]
		public void InitializeLogMgrShouldFunctionCorrectly()
		{
			var appLogDirStr = ConfigurationManager.AppSettings["ApplicationLogDirectory"];
            var dir1 = new DirectoryDto(appLogDirStr);
			if (dir1.DirInfo.Exists)
			{
				dir1.DirInfo.Delete(true);
			}

			var logMgr = new AppicationLogMgr();

			logMgr.CreateApplicaitonLogDirectory();

			var dir2 = new DirectoryDto(appLogDirStr);

			Assert.IsTrue(dir2.DirInfo.Exists);

		}
		
	}
}