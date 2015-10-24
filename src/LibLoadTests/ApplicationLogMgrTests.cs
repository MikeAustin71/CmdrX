using System.Configuration;
using LibLoader.Helpers;
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
			var appFileNameExt = ConfigurationManager.AppSettings["ApplicationLogFileNameExtension"];
			var appLogFileNameOnly = PathHelper.ExtractFileNameOnlyComponent(appFileNameExt);
			var appLogFileExtWithNoLeadingDot = 
				PathHelper.ExtractFileExtensionComponentWithoutLeadingDot(appFileNameExt);
			var appLogFileTimeStamp = DateHelper.NowYearMthDayHrsSecs();
			var cmdOutputLogFilePath = ConfigurationManager.AppSettings["DefaultCommandOutputLogFilePathName"];
            var dir1 = new DirectoryDto(cmdOutputLogFilePath);
			if (dir1.DirInfo.Exists)
			{
				dir1.DirInfo.Delete(true);
			}

			var logMgr = new ApplicationLogMgr(dir1.DirInfo.FullName, 
				appLogFileNameOnly,
				appLogFileExtWithNoLeadingDot,
				appLogFileTimeStamp);

			logMgr.CreateApplicaitonLogDirectory();

			Assert.IsTrue(logMgr.LogDirectoryDto.DirInfo.Exists);

			DirectoryHelper.DeleteADirectory(logMgr.LogDirectoryDto);

			logMgr.Dispose();

		}
		
	}
}