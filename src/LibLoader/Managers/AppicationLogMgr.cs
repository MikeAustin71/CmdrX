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
		private bool _disposed;

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

		public void Dispose()
		{
			Dispose(true);
			// This object will be cleaned up by the Dispose method.
			// Therefore, you should call GC.SupressFinalize to
			// take this object off the finalization queue
			// and prevent finalization code for this object
			// from executing a second time.
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			// Check to see if Dispose has already been called.
			if (!_disposed)
			{
				// If disposing equals true, dispose all managed
				// and unmanaged resources.
				if (disposing)
				{
					// Dispose managed resources.
					if (LogDirectoryDto != null)
					{
						LogDirectoryDto.Dispose();
						LogDirectoryDto = null;
					}
				}


				// Note disposing has been done.
				_disposed = true;

			}
		}


		public bool CreateApplicaitonLogDirectory()
		{
			if (LogDirectoryDto?.DirInfo == null 
				|| !DirectoryHelper.IsDirectoryDtoValid(LogDirectoryDto))
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

			if (LogDirectoryDto.DirInfo.Exists)
			{
				return true;
			}

			DirectoryHelper.CreateDirectoryIfNecessary(LogDirectoryDto);

			LogDirectoryDto.DirInfo.Refresh();

			return LogDirectoryDto.DirInfo.Exists;
		}
	

	}
}