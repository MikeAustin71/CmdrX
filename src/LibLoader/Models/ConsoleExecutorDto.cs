using System;
using LibLoader.Constants;
using LibLoader.GlobalConstants;
using LibLoader.Helpers;
using LibLoader.Managers;

namespace LibLoader.Models
{
	public class ConsoleExecutorDto
	{
		private bool _disposed;

		private string _defaultConsoleCommandExecutor;
		private string _defaultConsoleCommandExeArgs;
		private string _defaultCommandOutputLogFilePathName;
		private string _cmdConsoleLogFileErrorSuffix;
		private string _cmdConsoleLogFileTimeStamp;

		public ErrorLogger ErrorMgr = new
			ErrorLogger(4281000,
				"ConsoleExecutorDto",
				ErrorLoggingStatus.On,
				ErrorLoggingMode.Verbose,
				false);


		public DirectoryDto DefaultCommandExeDirectoryDto { get; set; }

		public string DefaultConsoleCommandExecutor
		{
			get { return _defaultConsoleCommandExecutor; }
			set { _defaultConsoleCommandExecutor = StringHelper.TrimStringEnds(value); }
		}

		public string DefaultConsoleCommandExeArgs
		{
			get { return _defaultConsoleCommandExeArgs; }
			set { _defaultConsoleCommandExeArgs = StringHelper.TrimStringEnds(value); }
		}

		public decimal CommandMinTimeOutInMinutes { get; set; }

		public decimal CommandMaxTimeOutInMinutes { get; set; }

		public decimal CommandDefaultTimeOutInMinutes { get; set; }


		public string DefaultCommandOutputLogFilePathName
		{
			get { return _defaultCommandOutputLogFilePathName; }
			set
			{
				_defaultCommandOutputLogFilePathName = StringHelper.TrimStringEnds(value);
			}
		}

		public int AppLogRetentionInDays { get; set; }

		public string AppLogDirectory { get; set; }

		public string AppLogFileBaseNameOnly { get; set; }

		public string AppLogFileExtensionWithoutLeadingDot { get; set; }

		public string AppLogFileTimeStamp { get; set; }

		public string CmdConsoleLogFileErrorSuffix
		{
			get { return _cmdConsoleLogFileErrorSuffix; }
			set { _cmdConsoleLogFileErrorSuffix = StringHelper.TrimStringEnds(value); }
		}

		public string CmdConsoleLogFileTimeStamp
		{
			get { return _cmdConsoleLogFileTimeStamp; }
			set { _cmdConsoleLogFileTimeStamp = StringHelper.TrimStringEnds(value); }
		}

		public FileDto XmlCmdFileDto { get; set; }

		public ConsoleCommandType DefaultConsoleCommandType { get; set; }

		public ApplicationLogMgr AppLogMgr { get; private set; }


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
					if (XmlCmdFileDto != null)
					{
						XmlCmdFileDto.Dispose();
						XmlCmdFileDto = null;
					}

					if (DefaultCommandExeDirectoryDto!=null)
					{
						DefaultCommandExeDirectoryDto.Dispose();
						DefaultCommandExeDirectoryDto = null;
					}
				}


				// Note disposing has been done.
				_disposed = true;

			}
		}

		public void ConfigureParameters()
		{
			if (string.IsNullOrWhiteSpace(DefaultCommandOutputLogFilePathName))
			{
				var ex = new Exception("DefaultCommandOutputLogFilePathName = Empty!");

				var err = new FileOpsErrorMessageDto
				{
					DirectoryPath = string.Empty,
					ErrId = 1,
					ErrorMessage = ex.Message,
					ErrSourceMethod = "ConfigureParameters()",
					ErrException = ex,
					FileName = string.Empty,
					LoggerLevel = LogLevel.FATAL
				};

				ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
				ErrorMgr.WriteErrorMsg(err);

				throw ex;

			}

			AppLogDirectory = PathHelper.ExtractDirectoryComponent(DefaultCommandOutputLogFilePathName);



			AppLogMgr = new ApplicationLogMgr(AppLogDirectory, 
												AppLogFileBaseNameOnly, 
												AppLogFileExtensionWithoutLeadingDot, 
												AppLogFileTimeStamp);
		}


		public void SetDefaultCommandOutputLogFilePathName(string filePath)
		{
			var dirDto = new DirectoryDto(filePath);

			if (!DirectoryHelper.IsDirectoryDtoValid(dirDto))
			{
				dirDto.Dispose();
				return;
			}

			DefaultCommandOutputLogFilePathName = filePath;

			AppLogDirectory = PathHelper.ExtractDirectoryComponent(DefaultCommandOutputLogFilePathName);
		}

		public void SetDefaultCommandExeDirectory(string dir)
		{
			if (string.IsNullOrWhiteSpace(dir))
			{
				dir = ".";
			}


			DefaultCommandExeDirectoryDto?.Dispose();

			DefaultCommandExeDirectoryDto = new DirectoryDto(dir);

			if (!DirectoryHelper.IsDirectoryDtoValid(DefaultCommandExeDirectoryDto))
			{
				DefaultCommandExeDirectoryDto = DirectoryHelper.GetCurrentDirectory();

				if (!DirectoryHelper.IsDirectoryDtoValid(DefaultCommandExeDirectoryDto))
				{
					var ex = new Exception("Default Command Execution Directory Dto is INVALID!");

					var err = new FileOpsErrorMessageDto
					{
						DirectoryPath = string.Empty,
						ErrId = 1,
						ErrorMessage = ex.Message,
						ErrSourceMethod = "SetDefaultCommandExeDirectory()",
						ErrException = ex,
						FileName = string.Empty,
						LoggerLevel = LogLevel.FATAL
					};

					ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
					ErrorMgr.WriteErrorMsg(err);

					throw ex;
				}
			}

		}


	}
}