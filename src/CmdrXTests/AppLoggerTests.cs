using System;
using System.IO;
using System.Text;
using CmdrX.Helpers;
using CmdrX.Managers;
using CmdrX.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CmdrXTests
{
	[TestClass]
	public class AppLoggerTests
	{
		[TestMethod]
		public void LoggerShouldInitializeCorrectly()
		{
			var appLogDir = ".//installDir";
			var logDirDto = new DirectoryDto(appLogDir);

			if (logDirDto.DirInfo.Exists)
			{
				DirectoryHelper.DeleteADirectory(logDirDto);
			}

			var appLogFileNameOnly = "LibLoadTests";
			var appLogFileExensionWithoutLeadingDot = "log";
			var appLogFileTimeStamp = DateHelper.NowYearMthDayHrsSecs();

			var appLogMgr = new ApplicationLogMgr(appLogDir, 
													appLogFileNameOnly,
														appLogFileExensionWithoutLeadingDot,
															appLogFileTimeStamp);

			

			var logger = appLogMgr.ConfigureLogger();

			Assert.IsTrue(appLogMgr.LogPathFileNameDto.FileXinfo.Exists);

			var logFileName = appLogMgr.LogPathFileNameDto.FileXinfo.FullName;

			LogUtil.ConfigureLogger(logger);

			LogUtil.WriteLog(LogLevel.INFO, "Test9758: Originated from Test Method: LoggerShouldInitializeCorrectly()");

			LogUtil.Dispose();

			appLogMgr.Dispose();
			
			FileStream fs = new FileStream(logFileName,FileMode.Open,FileAccess.Read,FileShare.Read);

			StreamReader fr = new StreamReader(fs);

			var sb = new StringBuilder();

			sb.Append(fr.ReadToEnd());

			fs.Flush();
			fs.Close();
			fr.Close();
			fs.Dispose();
			fr.Dispose();
			var s = sb.ToString();
			var result = s.IndexOf("Test9758", StringComparison.Ordinal);

			var logFileDto = new FileDto(logFileName);

			DirectoryHelper.DeleteADirectory(logDirDto);

			logFileDto.FileXinfo.Refresh();
			Assert.IsFalse(logFileDto.FileXinfo.Exists);

			logFileDto.Dispose();
			logDirDto.Dispose();

			Assert.IsTrue(result > -1);
		}

	}
}