using System;
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


		public ErrorLogger ErrorMgr = new
			ErrorLogger(708000,
						"AppicationLogMgr",
						AppConstants.LoggingStatus,
						AppConstants.LoggingMode,
						false);

		public string LogFileNameOnly { get; set; }

		public string LogFileExtensionWithNoLeadingDot { get; set; }

		public int LogRetentionInDays { get; set; } 

		public string LogFileTimeStamp { get; set; }

		public DirectoryDto LogDirectoryDto { get; private set; }

		public FileDto LogPathFileNameDto { get; private set; }

		public AppicationLogMgr(string logDirPath, 
									string logFileNameOnly, 
										string logFileExtensionWithoutLeadingDot, 
											string logFileTimeStamp)
		{
			LogFileNameOnly = logFileNameOnly;
			LogFileExtensionWithNoLeadingDot = logFileExtensionWithoutLeadingDot;
			LogFileTimeStamp = logFileTimeStamp;
			SetNewLogFileDirectory(logDirPath);
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

					if (LogPathFileNameDto != null)
					{
						LogPathFileNameDto.Dispose();
						LogPathFileNameDto = null;
					}
				}


				// Note disposing has been done.
				_disposed = true;

			}
		}

		public void SetNewLogFileDirectory(string dirPath)
		{
			LogDirectoryDto?.Dispose();
			LogDirectoryDto = new DirectoryDto(dirPath);

			if (!DirectoryHelper.IsDirectoryDtoValid(LogDirectoryDto))
			{
				var msg = "Application Log Directory Dto Invalid!";
				var ex = new ArgumentException(msg);

				var err = new FileOpsErrorMessageDto
				{
					ErrId = 35,
					ErrorMessage = "Application Log Directory Dto Invalid!",
					ErrSourceMethod = "CreateApplicaitonLogDirectory()",
					ErrException = ex,
					DirectoryPath = dirPath,
					LoggerLevel = LogLevel.FATAL
				};

				ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
				ErrorMgr.WriteErrorMsg(err);

				throw ex;
			}

			SetLogFileDto();
			
		}

		private void SetLogFileDto()
		{
			LogPathFileNameDto?.Dispose();

			var fName = LogFileNameOnly + "_" + LogFileTimeStamp + "." + LogFileExtensionWithNoLeadingDot;
            var fileDto = new FileDto(LogDirectoryDto, fName);

			if (!FileHelper.IsFileDtoValid(fileDto))
			{
				var msg = "Application Log Directory Dto Invalid!";
				var ex = new ArgumentException(msg);

				var err = new FileOpsErrorMessageDto
				{
					ErrId = 35,
					ErrorMessage = "Application Log Directory Dto Invalid!",
					ErrSourceMethod = "CreateApplicaitonLogDirectory()",
					ErrException = ex,
					FileName = fName,
					DirectoryPath = LogDirectoryDto?.DirInfo?.FullName,
					LoggerLevel = LogLevel.FATAL
				};

				ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
				ErrorMgr.WriteErrorMsg(err);

				throw ex;

			}

			LogPathFileNameDto = fileDto;
		}


		public bool PurgeOldLogFiles()
		{
			var  purgeLogCmd = new PurgeLogCommand(LogRetentionInDays, LogDirectoryDto);

			return purgeLogCmd.Execute();

		}

		public bool CreateApplicaitonLogDirectory()
		{
			if (!DirectoryHelper.IsDirectoryDtoValid(LogDirectoryDto))
			{
				var msg = "Application Log Directory Dto Invalid!";
				var ex = new ArgumentException(msg);

				var err = new FileOpsErrorMessageDto
				{
					ErrId = 5,
					ErrorMessage = "Application Log Directory Dto Invalid!",
					ErrSourceMethod = "CreateApplicaitonLogDirectory()",
					ErrException = ex,
					DirectoryPath = string.Empty,
					LoggerLevel = LogLevel.FATAL
				};

				ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
				ErrorMgr.WriteErrorMsg(err);
				throw  ex;
			}

			if (LogDirectoryDto.DirInfo.Exists)
			{
				return true;
			}

			DirectoryHelper.CreateDirectoryIfNecessary(LogDirectoryDto);

			LogDirectoryDto.DirInfo.Refresh();

			if (!LogDirectoryDto.DirInfo.Exists)
			{
				var msg = "Application Log Directory Creation Failed!";
				var ex = new Exception(msg);

				var err = new FileOpsErrorMessageDto
				{
					ErrId = 8,
					ErrorMessage = msg,
					ErrSourceMethod = "CreateApplicaitonLogDirectory()",
					ErrException = ex,
					DirectoryPath = LogDirectoryDto.DirInfo.FullName,
					LoggerLevel = LogLevel.FATAL
				};

				ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
				ErrorMgr.WriteErrorMsg(err);
				throw ex;
			}

			return true;
		}
	

	}
}