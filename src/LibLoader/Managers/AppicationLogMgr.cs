using System;
using System.Configuration;
using LibLoader.Commands;
using LibLoader.Constants;
using LibLoader.GlobalConstants;
using LibLoader.Helpers;
using LibLoader.Models;

namespace LibLoader.Managers
{
	public class AppicationLogMgr
	{
		private readonly string _appLogDir;

		public static ErrorLogger ErrorMgr = new
			ErrorLogger(708000,
						"AppicationLogMgr",
						AppConstants.LoggingStatus,
						AppConstants.LoggingMode);

		public int LogRetentionInDays { get; set; } = int.Parse(ConfigurationManager.AppSettings["LogFileRetentionInDays"]);
		public DirectoryDto LogDirectoryDto { get; private set; }
		public PurgeLogCommand PurgeLogCmd { get; private set; }



		public AppicationLogMgr()
		{
			 _appLogDir = ConfigurationManager.AppSettings["ApplicationLogDirectory"];
			LogDirectoryDto  = new DirectoryDto(_appLogDir);
			PurgeLogCmd = new PurgeLogCommand(LogRetentionInDays, LogDirectoryDto);
		}

		public bool CreateApplicaitonLogDirectory()
		{
			if (LogDirectoryDto?.DirInfo == null)
			{
				var msg = "Application Log Directory Dto Invalid!";
				var ex = new ArgumentException(msg);

				var err = new FileOpsErrorMessageDto
				{
					ErrId = 5,
					ErrorMessage = "Application Log Directory Dto Invalid!",
					ErrSourceMethod = "CreateApplicaitonLogDirectory()",
					ErrException = ex,
					DirectoryPath = _appLogDir,
					LoggerLevel = LogLevel.FATAL
				};

				ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
				ErrorMgr.WriteErrorMsg(err);
				return false;
			}

			if (!LogDirectoryDto.DirInfo.Exists)
			{
				LogDirectoryDto.DirInfo.Create();
			}

			return LogDirectoryDto.DirInfo.Exists;
		}
	

	}
}