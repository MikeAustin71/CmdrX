using System;
using System.IO;
using LibLoader.Constants;
using LibLoader.GlobalConstants;
using LibLoader.Models;

namespace LibLoader.Helpers
{
	public static class PathHelper
	{

		public static ErrorLogger ErrorMgr = new
			ErrorLogger(836000,
						"PathHelper",
						AppConstants.LoggingStatus,
						AppConstants.LoggingMode);


		public static char PrimaryPathDelimiter { get; } = Path.DirectorySeparatorChar;

		public static char AlternatePathDelimiter { get; } = Path.AltDirectorySeparatorChar;

		public static string RemoveTrailingDelimiter(string directory)
		{
			if (String.IsNullOrEmpty(directory))
			{
				return String.Empty;
			}
			var lastChar = directory[directory.Length - 1];

			if (lastChar == PrimaryPathDelimiter || lastChar == AlternatePathDelimiter)
			{
				return directory.Substring(0, directory.Length - 1);
			}

			return directory;
		}

		public static string RemovePrefixDelimiter(string directory)
		{

			var fChar = directory[0];

			if (fChar == PrimaryPathDelimiter || fChar == AlternatePathDelimiter)
			{
				return directory.Substring(1, directory.Length - 1);
			}

			return directory;
		}

		/// <summary>
		/// Adds Trailing forward slash or back slash as
		/// specified by input parameter
		/// </summary>
		/// <param name="dir">DirectoryDto</param>
		/// <returns></returns>
		public static string AddTrailingDelimiter(DirectoryDto dir)
		{
			if (dir?.DirInfo == null)
			{
				return string.Empty;
			}

			return AddDefaultTrailingDelimiter(dir.DirInfo.FullName);
		}

		public static string AddTrailingDelimiter(string parentDirectory, string delimiter)
		{
			var lChar = parentDirectory[parentDirectory.Length - 1];

			if (lChar == PrimaryPathDelimiter || lChar == AlternatePathDelimiter)
			{
				parentDirectory = parentDirectory.Substring(0, parentDirectory.Length - 1);
			}

			return parentDirectory + delimiter;
		}

		public static string AddDefaultTrailingDelimiter(string directoryPath)
		{
			var defaultDelimiter = CurrentDefaultDelimiter(directoryPath);

			return AddTrailingDelimiter(directoryPath,defaultDelimiter);
		}

		public static string CurrentDefaultDelimiter(string filePath)
		{
			if (string.IsNullOrWhiteSpace(filePath))
			{
				return string.Empty;
			}

			if (StringHelper.GetLastCharIndex(filePath, PrimaryPathDelimiter) > -1)
			{
				return PrimaryPathDelimiter.ToString();
			}

			return StringHelper.GetLastCharIndex(filePath, AlternatePathDelimiter) > -1 ?
				AlternatePathDelimiter.ToString() : string.Empty;
		}

		public static string ReplacePathDelimiter(string filePath, string newDelimiter)
		{
			string thisMethod = "ReplacePathDelimiter()";

			if (string.IsNullOrWhiteSpace(filePath))
			{
				var err = new FileOpsErrorMessageDto
				{
					DirectoryPath = String.Empty,
					ErrId = 1,
					ErrorMessage = "Empty File Path!",
					ErrSourceMethod = thisMethod,
					FileName = string.Empty,
					LoggerLevel = LogLevel.FATAL
				};

				ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
				ErrorMgr.WriteErrorMsg(err);

				throw new ArgumentException("ReplacePathDelimier: Empty File Path!");
			}

			if (string.IsNullOrWhiteSpace(newDelimiter))
			{
				var err = new FileOpsErrorMessageDto
				{
					DirectoryPath = String.Empty,
					ErrId = 1,
					ErrorMessage = "Empty File Path!",
					ErrSourceMethod = thisMethod,
					FileName = string.Empty,
					LoggerLevel = LogLevel.FATAL
				};

				ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
				ErrorMgr.WriteErrorMsg(err);

				throw new ArgumentException("ReplacePathDelimiter: Empty newDelimiter!");
			}


			if (newDelimiter == PrimaryPathDelimiter.ToString())
			{
				return filePath.Replace(AlternatePathDelimiter.ToString(), newDelimiter);
			}

			if (newDelimiter == AlternatePathDelimiter.ToString())
			{
				return filePath.Replace(PrimaryPathDelimiter.ToString(), newDelimiter);
			}


			var err2 = new FileOpsErrorMessageDto
			{
				DirectoryPath = String.Empty,
				ErrId = 20,
				ErrorMessage = "INVALID newDelimiter: "+ newDelimiter,
				ErrSourceMethod = thisMethod,
				FileName = string.Empty,
				LoggerLevel = LogLevel.FATAL
			};

			ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
			ErrorMgr.WriteErrorMsg(err2);

			throw new ArgumentException("ReplacePathDelimiter: INVALID newDelimiter!");
		}
	}
}