using System;
using System.IO;
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

		public static DirectoryDto GetTest1Dir()
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
		 
		public static DirectoryDto GetAppDirWithTrailingSlash()
		{
			var appDir = DirectoryHelper.GetCurrentApplicationDirectoryLocation().DirInfo.FullName;


			var dirDto = new DirectoryDto(PathHelper.AddDefaultTrailingDelimiter(appDir));

			return dirDto;
		}
		 
	}
}