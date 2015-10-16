using System;
using System.IO;
using System.Reflection;
using LibLoader.Constants;
using LibLoader.GlobalConstants;
using LibLoader.Models;

namespace LibLoader.Helpers
{
	public static class DirectoryHelper
	{
		public static ErrorLogger ErrorMgr = new
			ErrorLogger(832000,
						"FileHelper",
						AppConstants.LoggingStatus,
						AppConstants.LoggingMode);

		public static DirectoryDto GetCurrentEnvironmentDirectory()
		{
			return new DirectoryDto(Environment.CurrentDirectory);
		}

		public static bool SetCurrentEnvironmentDirectory(DirectoryDto targetDto)
		{
			if (targetDto == null || targetDto.DirInfo == null)
			{
				return false;
			}

			Environment.CurrentDirectory = targetDto.DirInfo.FullName;

			return GetCurrentEnvironmentDirectory().DirInfo.FullName == targetDto.DirInfo.FullName;
		}

		public static DirectoryDto GetCurrentApplicationDirectoryLocation()
		{
			// (new Uri(Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath;
			var fileInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);
			return new DirectoryDto(fileInfo);
		}

		public static DirectoryDto GetCurrentApplicationDirectoryAbsolutePath()
		{
			// (new Uri(Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath;
			var fileInfo = new FileInfo((new Uri(Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath);
			return new DirectoryDto(fileInfo);
		}

		public static DirectoryDto GetApplicationDirectoryCodeBase()
		{
			var exeDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
			return new DirectoryDto(exeDir);
		}

		/// <summary>
		/// Call this with a type. Example typeof(PurgeLogCommand)
		/// </summary>
		/// <param name="targetAssembly">Type of the target assembly</param>
		/// <returns></returns>
		public static DirectoryDto GetPathForThisAssembly(Type targetAssembly)
		{
			var fullPath = Assembly.GetAssembly(targetAssembly).Location;

			return new DirectoryDto(fullPath);
		}
		//var exeDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
		//get the full location of the assembly with DaoTests in it
		//string fullPath = System.Reflection.Assembly.GetAssembly(typeof(PurgeLogCommand)).Location;


		public static DirectoryDto GetCurrentDirectory()
		{
			// Gets The Current Working Directory of this application!
			return new DirectoryDto(Directory.GetCurrentDirectory());
		}

		public static bool DeleteADirectory(string dir)
		{
			if (String.IsNullOrEmpty(dir))
			{
				return true;
			}

			try
			{
				if (!Directory.Exists(dir))
				{
					return true;
				}

				var files = Directory.GetFiles(dir);

				if (files.Length < 1)
				{
					Directory.Delete(dir);
				}

				if (Directory.Exists(dir))
				{
					return false;
				}
			}
			catch(Exception ex)
			{
				var err = new FileOpsErrorMessageDto
				{
					DirectoryPath = String.Empty,
					ErrId = 2,
					ErrorMessage = "Directory Deletion Failed!",
					ErrSourceMethod = "DeleteADirectory",
					ErrException = ex,
					FileName = "Target Dir: " + dir,
					LoggerLevel = LogLevel.FATAL
				};

				ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
				ErrorMgr.WriteErrorMsg(err);

				return false;
			}

			return true;
		}


		public static bool CreateDirectoryIfNecessary(DirectoryDto dirDto)
		{
			return CreateADirectory(dirDto.DirInfo.FullName);
		}

		public static bool CreateDirectoryIfNecessary(string targetFile)
		{
			bool result;

			try
			{
				var dir = Path.GetDirectoryName(targetFile);

				result = CreateADirectory(dir);
			}
			catch
			{

				result = false;
			}


			return result;
		}

		public static bool CreateADirectory(string dir)
		{
			try
			{
				if (String.IsNullOrEmpty(dir))
				{
					return false;
				}

				if (Directory.Exists(dir))
				{
					return true;
				}


				var di = Directory.CreateDirectory(dir);

				if (!di.Exists)
				{
					return false;
				}

			}
			catch
			{
				return false;
			}

			return true;
		}

		public static bool ChangeToNewCurrentDirectory(DirectoryDto targetDirDto)
		{
			if (targetDirDto?.DirInfo == null)
			{
				return false;
			}

			try
			{
				Directory.SetCurrentDirectory(targetDirDto.DirInfo.FullName);
			}
			catch (Exception ex)
			{
				var err = new FileOpsErrorMessageDto
				{
					DirectoryPath = String.Empty,
					ErrId = 4,
					ErrorMessage = "Directory Deletion Failed!",
					ErrSourceMethod = "ChangeToNewCurrentDirectory()",
					ErrException = ex,
					FileName = "Target Dir: " + targetDirDto.DirInfo.FullName,
					LoggerLevel = LogLevel.FATAL
				};

				ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
				ErrorMgr.WriteErrorMsg(err);

				return false;
			}

			var newCurrentDirectory = new DirectoryDto(Directory.GetCurrentDirectory());

			return targetDirDto == newCurrentDirectory;

		}
	}
}