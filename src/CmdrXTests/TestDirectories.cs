using System;
using System.Configuration;
using CmdrX.Constants;
using CmdrX.Helpers;
using CmdrX.Models;

namespace CmdrXTests
{
	public static class TestDirectories
	{
		public static DirectoryDto GetTestExeDir()
		{
			return new DirectoryDto(DirectoryHelper.GetCurrentApplicationDirectoryLocation().DirInfo.FullName);
		}


		public static DirectoryDto GetMainLibLoadTestDir()
		{
			const string testLibDir = "CmdrXTests";

			var appDir = DirectoryHelper.GetCurrentApplicationDirectoryLocation().DirInfo.FullName;

			var idx = appDir.IndexOf(testLibDir, StringComparison.Ordinal);

			if (idx < 0)
			{
				return null;
			}

			var testDir = PathHelper.AddDefaultTrailingDelimiter(appDir.Substring(0, idx + testLibDir.Length));

			var dirDto = new DirectoryDto(testDir);

			return dirDto;
		}

		public static FileDto GetXmlCmdTest002FileDto()
		{
			var fileName = "\\XmlCmdFiles\\Test002.xml";
			var testDir = GetMainLibLoadTestDir();
			var result = new FileDto(testDir.DirInfo.FullName + fileName);
			if (!FileHelper.IsFileDtoValid(result) || !result.FileXinfo.Exists)
			{
				throw new Exception("Invalid FileDto created for XmlCmdFile: " + fileName);
			}

			return result;


		}

		public static FileDto GetXmlCmdFileDto()
		{
			var fileName = ConfigurationManager.AppSettings["DefaultXmlCmdFile"];
			var testDir = GetMainLibLoadTestDir();
			var result = new FileDto(testDir.DirInfo.FullName + "\\" + fileName);
			if (!FileHelper.IsFileDtoValid(result))
			{
				throw new Exception("Invalid FileDto created for XmlCmdFile: " + fileName);
			}

			return result;
		}

		public static DirectoryDto GetTestDirectory001()
		{
			var mainLoadTestDir = GetMainLibLoadTestDir();
			return new DirectoryDto(mainLoadTestDir.DirInfo.FullName + "\\TestFiles001");
		}

		public static DirectoryDto GetTestDirectory002()
		{
			var mainLoadTestDir = GetMainLibLoadTestDir();
			return new DirectoryDto(mainLoadTestDir.DirInfo.FullName + "\\TestFiles002");
		}

		public static DirectoryDto GetTestDirectory003()
		{
			var mainLoadTestDir = GetMainLibLoadTestDir();
			return new DirectoryDto(mainLoadTestDir.DirInfo.FullName + "\\TestFiles003");
		}


		public static DirectoryDto GetAppDirWithTrailingSlash()
		{
			var appDir = DirectoryHelper.GetCurrentApplicationDirectoryLocation().DirInfo.FullName;


			var dirDto = new DirectoryDto(PathHelper.AddDefaultTrailingDelimiter(appDir));

			return dirDto;
		}

		public static ConsoleExecutorDto GetConsoleExecutorDto()
		{
			return new ConsoleExecutorDto
			{
				DefaultConsoleCommandExecutor = GetConsoleCommandExecutor(),
				DefaultConsoleCommandExeArgs = GetConsoleCommandExeArguments(),
				CmdConsoleLogFileErrorSuffix = "Error",
				CmdConsoleLogFileTimeStamp = DateHelper.NowYearMthDayHrsSecs(),
				CommandDefaultTimeOutInMinutes = 5.0M,
				CommandMaxTimeOutInMinutes = 45.0M,
				CommandMinTimeOutInMinutes = 1.0M,
				DefaultCommandOutputLogFilePathName = 
					ConfigurationManager.AppSettings["DefaultCommandOutputLogFilePathName"],
				XmlCmdFileDto = GetXmlCmdFileDto(),
				DefaultConsoleCommandType = ConsoleCommandType.Console

			};

		}

		public static string GetConsoleCommandExecutor()
		{
			return "cmd.exe";
		}

		public static string GetConsoleCommandExeArguments()
		{
			return "/c";
		}

		public static JobsGroupDto GetTestJobGroup()
		{
			var jobs = new JobsGroupDto("TestJobs1");

			jobs.Jobs.Add(GetCopy1Job());
			jobs.Jobs.Add(GetCopy2Job());

			return jobs;
		}

		public static ConsoleCommandDto GetCopy1Job()
		{
			var cmdExeDto = GetConsoleExecutorDto();

            var cmdDto = new ConsoleCommandDto(cmdExeDto)
			{
				CommandDisplayName = "Copy1",
				CommandType = ConsoleCommandType.Console,
				CommandOutputLogFilePathBaseName = cmdExeDto.DefaultCommandOutputLogFilePathName,
				ConsoleCommandExecutor = cmdExeDto.DefaultConsoleCommandExecutor,
				ConsoleCommandExeArguments = cmdExeDto.DefaultConsoleCommandExeArgs,
				CommandTimeOutInMinutes = 5.0M,
                ExecuteInDir = string.Empty,
				ExecutableTarget = "Copy",
				CommandToExecute = string.Empty,
				CommandModifier = string.Empty
			};
			var arg1 = GetTestDirectory001().DirInfo.FullName + "\\*.*";
			var arg2 = GetTestDirectory003().DirInfo.FullName + "\\";
			cmdDto.CommandArguments = arg1 + " " + arg2;
			cmdDto.NormalizeCommandParameters();
			return cmdDto;
		}

		public static ConsoleCommandDto GetCopy2Job()
		{
			var cmdExeDto = GetConsoleExecutorDto();
            var cmdDto = new ConsoleCommandDto(cmdExeDto)
			{
				CommandDisplayName = "Copy2",
				CommandType = ConsoleCommandType.Console,
				CommandOutputLogFilePathBaseName = cmdExeDto.DefaultCommandOutputLogFilePathName,
				ConsoleCommandExecutor = cmdExeDto.DefaultConsoleCommandExecutor,
				ConsoleCommandExeArguments = cmdExeDto.DefaultConsoleCommandExeArgs,
				CommandTimeOutInMinutes = 5.0M,
				ExecuteInDir = string.Empty,
				ExecutableTarget = "Copy",
				CommandToExecute = string.Empty,
				CommandModifier = string.Empty
			};
			var arg1 = GetTestDirectory002().DirInfo.FullName + "\\*.*";
			var arg2 = GetTestDirectory003().DirInfo.FullName + "\\";
			cmdDto.CommandArguments = arg1 + " " + arg2;
			cmdDto.NormalizeCommandParameters();
			return cmdDto;
		}

	}
}