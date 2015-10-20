using System;
using System.IO;
using System.Net.NetworkInformation;
using LibLoader.Constants;
using LibLoader.Helpers;
using LibLoader.Models;

namespace LibLoadTests
{
	public static class TestDirectories
	{
		public static DirectoryDto GetTestExeDir()
		{
			return new DirectoryDto(DirectoryHelper.GetCurrentApplicationDirectoryLocation().DirInfo.FullName);
		}


		public static DirectoryDto GetMainLibLoadTestDir()
		{
			const string testLibDir = "LibLoadTests";

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
			var cmdDto = new ConsoleCommandDto
			{
				CommandDisplayName = "Copy1",
				ComandType = ConsoleCommandType.Console,
				ExecuteInDir = string.Empty,
				ExecutableTarget = "Copy",
				CommandToExecute = string.Empty,
				CommandModifier = string.Empty
			};
			var arg1 = GetTestDirectory001().DirInfo.FullName + "\\*.*";
			var arg2 = GetTestDirectory003().DirInfo.FullName + "\\";
			cmdDto.CommandArguments = arg1 + " " + arg2;
			return cmdDto;
		}

		public static ConsoleCommandDto GetCopy2Job()
		{
			var cmdDto = new ConsoleCommandDto
			{
				CommandDisplayName = "Copy2",
				ComandType = ConsoleCommandType.Console,
				ExecuteInDir = string.Empty,
				ExecutableTarget = "Copy",
				CommandToExecute = string.Empty,
				CommandModifier = string.Empty
			};
			var arg1 = GetTestDirectory002().DirInfo.FullName + "\\*.*";
			var arg2 = GetTestDirectory003().DirInfo.FullName + "\\";
			cmdDto.CommandArguments = arg1 + " " + arg2;
			return cmdDto;
		}



	}
}