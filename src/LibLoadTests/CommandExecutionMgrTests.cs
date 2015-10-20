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

			var consoleExeDto = new ConsoleExecutorDto(
				TestDirectories.GetConsoleCommandExecutor(),
				TestDirectories.GetConsoleCommandExeArguments(),
				5,
				".\\logdir\\installLog.log",
				"Error",
				DateHelper.NowYearMthDayHrsSecs()
				);

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