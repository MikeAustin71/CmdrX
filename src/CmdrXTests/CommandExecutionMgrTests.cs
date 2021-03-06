﻿using CmdrX.Constants;
using CmdrX.Helpers;
using CmdrX.Managers;
using CmdrX.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CmdrXTests
{
	[TestClass]
	public class CommandExecutionMgrTests
	{
		[TestMethod]
		public void ShouldExecuteCommandsCorrectly()
		{
			var exeDir = TestDirectories.GetTestExeDir();

			var logDir = new DirectoryDto(exeDir.DirInfo.FullName + "\\logdir");
			DirectoryHelper.DeleteADirectory(logDir);
			var dirT3 = TestDirectories.GetTestDirectory003();
            FileHelper.DeleteAllFilesInDirectoryTree(dirT3);

			var consoleExeDto = new ConsoleExecutorDto
			{
				DefaultConsoleCommandExecutor = TestDirectories.GetConsoleCommandExecutor(),
				DefaultConsoleCommandExeArgs = TestDirectories.GetConsoleCommandExeArguments(),
				CmdConsoleLogFileErrorSuffix = "Error",
				CmdConsoleLogFileTimeStamp = DateHelper.NowYearMthDayHrsSecs(),
				CommandDefaultTimeOutInMinutes = 5.0M,
				CommandMaxTimeOutInMinutes = 45.0M,
				CommandMinTimeOutInMinutes = 1.0M,
				DefaultCommandOutputLogFilePathName = ".\\logdir\\installLog.log",
				XmlCmdFileDto = TestDirectories.GetXmlCmdTest002FileDto(),
				DefaultConsoleCommandType = ConsoleCommandType.Console
			};

			var jobs = TestDirectories.GetTestJobGroup();

			var cmdExeMgr = new CommandExecutionMgr(jobs,consoleExeDto);


			var result = cmdExeMgr.ExecuteCommands();

			Assert.IsTrue(result);

			var files = dirT3.DirInfo.GetFiles();

			FileHelper.DeleteAllFilesInDirectoryTree(dirT3);

			Assert.IsTrue(files.Length == 4);
		}
	}
}