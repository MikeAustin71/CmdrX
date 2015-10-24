using System;
using CmdrX.Constants;
using CmdrX.GlobalConstants;
using CmdrX.Helpers;
using CmdrX.Models;

namespace CmdrX.Managers
{
	public class WorkingDirectoryMgr
	{
		private bool _disposed;

		public ErrorLogger ErrorMgr = new
			ErrorLogger(6492000,
						"WorkingDirectoryMgr",
						AppConstants.LoggingStatus,
						AppConstants.LoggingMode);


		public bool IsSetToTargetDirectory { get; private set; }
		public bool IsCurrentAndTargetSameDirectory { get; private set; }
		public DirectoryDto OriginalCurrentWorkingDirectory { get; private set; } 
		public DirectoryDto TargetWorkingDirectory { get; set; }

		public WorkingDirectoryMgr()
		{
			SetCurrentWorkingDirectory();

			TargetWorkingDirectory = new DirectoryDto(OriginalCurrentWorkingDirectory.DirInfo.FullName);

			if (!DirectoryHelper.IsDirectoryDtoValid(TargetWorkingDirectory))
			{
				var ex = new Exception("TargetWorkingDirectory Dto Invalid!");

				var err = new FileOpsErrorMessageDto
				{
					DirectoryPath = OriginalCurrentWorkingDirectory.DirInfo.FullName,
					ErrId = 1,
					ErrorMessage = "Directory Deletion Failed!",
					ErrSourceMethod = "Constructor()",
					ErrException = ex,
					FileName = string.Empty,
					LoggerLevel = LogLevel.FATAL
				};

				ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
				ErrorMgr.WriteErrorMsg(err);

				throw ex;

			}


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
					if (OriginalCurrentWorkingDirectory != null)
					{
						OriginalCurrentWorkingDirectory.Dispose();
						OriginalCurrentWorkingDirectory = null;
					}

					if (TargetWorkingDirectory != null)
					{
						TargetWorkingDirectory.Dispose();
						TargetWorkingDirectory = null;
					}

				}


				// Note disposing has been done.
				_disposed = true;

			}
		}


		public bool ChangeToTargetWorkingDirectory()
		{

			SetCurrentWorkingDirectory();

			if (OriginalCurrentWorkingDirectory == TargetWorkingDirectory)
			{
				IsSetToTargetDirectory = true;
				IsCurrentAndTargetSameDirectory = true;

				return true;
			}

			DirectoryHelper.CreateDirectoryIfNecessary(TargetWorkingDirectory);

			DirectoryHelper.ChangeToNewCurrentDirectory(TargetWorkingDirectory);

			var latestCurrWorkingDirectory = DirectoryHelper.GetCurrentDirectory();

			IsSetToTargetDirectory = TargetWorkingDirectory == latestCurrWorkingDirectory;

			return IsSetToTargetDirectory;
		}

		public bool ChangeBackToOriginalWorkingDirectory()
		{
			var latestCurrWorkingDirectory = DirectoryHelper.GetCurrentDirectory();

			if (latestCurrWorkingDirectory == OriginalCurrentWorkingDirectory)
			{
				return true;
			}

			DirectoryHelper.ChangeToNewCurrentDirectory(OriginalCurrentWorkingDirectory);

			latestCurrWorkingDirectory = DirectoryHelper.GetCurrentDirectory();

			return TargetWorkingDirectory != latestCurrWorkingDirectory;
		}

		public DirectoryDto SetCurrentWorkingDirectory()
		{
			OriginalCurrentWorkingDirectory = DirectoryHelper.GetCurrentDirectory();

			if (!DirectoryHelper.IsDirectoryDtoValid(OriginalCurrentWorkingDirectory))
			{
				var ex = new Exception("OriginalCurrentWorkingDirectory Dto Invalid!");

				var err = new FileOpsErrorMessageDto
				{
					DirectoryPath = string.Empty,
					ErrId = 50,
					ErrorMessage = ex.Message,
					ErrSourceMethod = "SetCurrentWorkingDirectory()",
					ErrException = ex,
					FileName = string.Empty,
					LoggerLevel = LogLevel.FATAL
				};

				ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
				ErrorMgr.WriteErrorMsg(err);

				throw ex;

			}

			return OriginalCurrentWorkingDirectory;
			
		}

		public void SetTargetDirectory(string targetDir)
		{
			if (string.IsNullOrWhiteSpace(targetDir))
			{
				return;
			}

			TargetWorkingDirectory.Dispose();
			TargetWorkingDirectory = new DirectoryDto(targetDir);
		}

		public void SetTargetDirectory(DirectoryDto targetDto)
		{
			if (!DirectoryHelper.IsDirectoryDtoValid(targetDto))
			{
				return;
			}

			TargetWorkingDirectory.Dispose();
			TargetWorkingDirectory = new DirectoryDto(targetDto.DirInfo.FullName);
		}

	}
}