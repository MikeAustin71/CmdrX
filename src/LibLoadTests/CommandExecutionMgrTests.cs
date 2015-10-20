using System.Configuration;
using LibLoader.Constants;
using LibLoader.Helpers;
using LibLoader.Managers;
using LibLoader.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LibLoadTests
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
				DefaultCmdConsoleLogFilePathName = ".\\logdir\\installLog.log",
				XmlCmdFileDto = TestDirectories.GetXmlCmdFileDto(),
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