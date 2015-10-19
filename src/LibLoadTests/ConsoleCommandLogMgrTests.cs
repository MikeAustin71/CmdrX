using System;
using System.IO;
using System.Text;
using LibLoader.Helpers;
using LibLoader.Managers;
using LibLoader.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LibLoadTests
{
	[TestClass]
	public class ConsoleCommandLogMgrTests
	{

		[TestMethod]
		public void LogMgrShouldInitializeCorrectly()
		{

			var fileStamp = DateHelper.NowYearMthDayHrsSecs();

			var mgr = new ConsoleCommandLogMgr(".\\logdir\\installLog.log",
													fileStamp,
														string.Empty);

			var currWrkDir = TestDirectories.GetTestExeDir();
			var subDir = "\\logdir\\installLog" + "_" + fileStamp + ".log";
			var expectedResult = FileHelper.CombineDirSubDirWithFile(currWrkDir , subDir);
			var result = mgr.DefaultLogFileDto.FileXinfo.FullName;
			mgr.Dispose();

			Assert.IsTrue(result == expectedResult.FileXinfo.FullName);
		}

		[TestMethod]
		public void WriteTextLinesToLogMgr()
		{
			var dir1Dto = new DirectoryDto(".\\logdir");

			DirectoryHelper.DeleteADirectory(dir1Dto);

			var fileStamp = DateHelper.NowYearMthDayHrsSecs();

			var mgr = new ConsoleCommandLogMgr(".\\logdir\\installLog.log",
												fileStamp,
													string.Empty);

			var currWrkDir = TestDirectories.GetTestExeDir();
			var subDir = "\\logdir\\installLog" + "_" + fileStamp + ".log";
			var expectedResult = FileHelper.CombineDirSubDirWithFile(currWrkDir, subDir);

			mgr.InitializeCmdConsoleLog(string.Empty);

			var textOutput = "Hello World!";
			mgr.LogWriteLine(textOutput);
			var expectedText = textOutput+ Environment.NewLine;

			mgr.LogFlushStreamWriter();

			mgr.Dispose();

			StreamReader sReader = null;
			var sb = new StringBuilder();
			
			try
			{
				sReader = new StreamReader(expectedResult.FileXinfo.FullName);
				sb.Append(sReader.ReadToEnd());
				sReader.Close();
				sReader.Dispose();
			}
			catch
			{
				if (sReader != null)
				{
					sReader.Close();
					sReader.Dispose();
				}

				Assert.IsTrue(false);
			}
			var result = sb.ToString();

			DirectoryHelper.DeleteADirectory(dir1Dto);

			Assert.IsTrue(result == expectedText);
		}

		[TestMethod]
		public void WriteLinesToTwoLoggers()
		{
			var dir1Dto = new DirectoryDto(".\\logdir");

			DirectoryHelper.DeleteADirectory(dir1Dto);

			var fileStamp = DateHelper.NowYearMthDayHrsSecs();

			var mgr = new ConsoleCommandLogMgr(".\\logdir\\installLog.log",
												fileStamp,
													string.Empty);

			var currWrkDir = TestDirectories.GetTestExeDir();
			var subDir = "\\logdir\\installLog" + "_" + fileStamp + ".log";
			var expectedResult = FileHelper.CombineDirSubDirWithFile(currWrkDir, subDir);

			mgr.InitializeCmdConsoleLog(string.Empty);

			var textOutput = "Hello World!";
			mgr.LogWriteLine(textOutput);
			var expectedText = textOutput + Environment.NewLine;

			mgr.LogFlushStreamWriter();

			StreamReader sReader = null;
			var sb = new StringBuilder();
			FileStream fStream = null;
			try
			{

				fStream = new FileStream(expectedResult.FileXinfo.FullName,
					FileMode.Open,
					FileAccess.Read,
					FileShare.ReadWrite);

				sReader = new StreamReader(fStream);
				sb.Append(sReader.ReadToEnd());
			}
			catch (Exception ex)
			{
				// ReSharper disable once UnusedVariable
				var x = ex.Message;

			}
			finally
			{
				if (sReader != null)
				{
					sReader.Close();
					sReader.Dispose();
					sReader = null;
				}

				if (fStream != null)
				{
					fStream.Close();
					fStream.Dispose();
					fStream = null;
				}
			}
			var result = sb.ToString();

			Assert.IsTrue(result == expectedText);

			subDir = ".\\logdir\\installLog2" + "_" + fileStamp + ".log";
			var baseFile = ".\\logdir\\installLog2" +".log";
			expectedResult = FileHelper.CombineDirSubDirWithFile(currWrkDir, subDir);

			mgr.InitializeCmdConsoleLog(baseFile);
			textOutput = "Hello2 World!";
			mgr.LogWriteLine(textOutput);
			expectedText = textOutput + Environment.NewLine;
			mgr.LogFlushStreamWriter();
			sb = new StringBuilder();

			try
			{
				fStream = new FileStream(expectedResult.FileXinfo.FullName,
					FileMode.Open,
					FileAccess.Read,
					FileShare.ReadWrite);

				sReader = new StreamReader(fStream);
				sb.Append(sReader.ReadToEnd());
			}
			catch(Exception ex)
			{
				// ReSharper disable once UnusedVariable
				var y = ex.Message;
			}
			finally
			{
				if (sReader != null)
				{
					sReader.Close();
					sReader.Dispose();
				}

				if (fStream != null)
				{
					fStream.Close();
					fStream.Dispose();
				}

			}

			result = sb.ToString();
			mgr.Dispose();
			DirectoryHelper.DeleteADirectory(dir1Dto);

			Assert.IsTrue(result == expectedText);
		}

	}
}