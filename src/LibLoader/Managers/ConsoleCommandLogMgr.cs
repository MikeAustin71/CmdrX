using System;
using LibLoader.Constants;
using LibLoader.GlobalConstants;
using LibLoader.Helpers;
using LibLoader.Models;

namespace LibLoader.Managers
{
	public class ConsoleCommandLogMgr
	{
		private bool _disposed;

		public ErrorLogger ErrorMgr = new
			ErrorLogger(7388000,
						"ConsoleCommandLogMgr",
						AppConstants.LoggingStatus,
						AppConstants.LoggingMode);

		private readonly string _defaultCmdConsoleLogFileBaseName;

		private readonly string _logfileTimeStamp;

		private string _currentCmdConsoleLogFileNameAndExt = string.Empty;

		private FileDto _currentLogfileDto;

		private StreamWriterDto _swDto;


		public ConsoleCommandLogMgr(string defaultCmdConsoleLogFileBaseName, string logFileTimeStamp)
		{
			_defaultCmdConsoleLogFileBaseName = defaultCmdConsoleLogFileBaseName;
			_logfileTimeStamp = logFileTimeStamp;

			DeleteDefaultConsoleCommandLogFile();

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
					if (_swDto != null)
					{
						_swDto.GetStreamWriter().Flush();
						_swDto.Close();
						_swDto = null;
					}

					if (_currentLogfileDto != null)
					{
						_currentLogfileDto.Dispose();
						_currentLogfileDto = null;
					}

				}


				// Note disposing has been done.
				_disposed = true;

			}
		}



		private bool SetConsoleLog(string commandLogFileBaseName)
		{
			if (string.IsNullOrWhiteSpace(commandLogFileBaseName))
			{
				_currentCmdConsoleLogFileNameAndExt = _defaultCmdConsoleLogFileBaseName
				                                      + "_" + _logfileTimeStamp + ".log";
			}
			else
			{
				_currentCmdConsoleLogFileNameAndExt = commandLogFileBaseName
													  + "_" + _logfileTimeStamp + ".log";
			}

			_currentLogfileDto = new FileDto(_currentCmdConsoleLogFileNameAndExt);

			if (!FileHelper.IsFileDtoValid(_currentLogfileDto))
			{
				var msg = "Command Console Log File INVALID!";
				var err = new FileOpsErrorMessageDto
				{
					DirectoryPath = string.Empty,
					ErrId = 10,
					ErrorMessage = msg,
					ErrSourceMethod = "SetConsoleLog()",
					FileName = _currentCmdConsoleLogFileNameAndExt,
                    LoggerLevel = LogLevel.FATAL
				};

				ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
				ErrorMgr.WriteErrorMsg(err);

				throw new Exception(msg);

			}

			return true;
		}

		public bool InitializeCmdConsoleLog(string commandLogFileBaseName)
		{

			if (!SetConsoleLog(commandLogFileBaseName))
			{
				return false;
			}

			if (!FileHelper.IsFileDtoValid(_currentLogfileDto))
			{
				var msg = "Command Console Log File INVALID!";
				var err = new FileOpsErrorMessageDto
				{
					DirectoryPath = string.Empty,
					ErrId = 10,
					ErrorMessage = msg,
					ErrSourceMethod = "InitializeCmdConsoleLog()",
					FileName = _currentCmdConsoleLogFileNameAndExt,
					LoggerLevel = LogLevel.FATAL
				};

				ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
				ErrorMgr.WriteErrorMsg(err);

				throw new Exception(msg);

			}

			if (!FileHelper.CreateAFile(_currentLogfileDto) )
			{
				var ex = new Exception("Failure Creating Command Console Log File: " +_currentLogfileDto.FileXinfo.Name);
				var err = new FileOpsErrorMessageDto
				{
					DirectoryPath = _currentLogfileDto.FileXinfo.DirectoryName,
					ErrId = 25,
					ErrorMessage =ex.Message,
					ErrSourceMethod = "InitializeCmdConsoleLog()",
					FileName = _currentLogfileDto.FileXinfo.FullName,
					LoggerLevel = LogLevel.FATAL
				};

				ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
				ErrorMgr.WriteErrorMsg(err);

				throw ex;
			}


			if (_swDto == null)
			{
				_swDto = new StreamWriterDto(_currentLogfileDto);
			}
			else
			{
				_swDto.SetStreamWriter(_currentLogfileDto);
			}

			return true;
		}

		public bool DeleteDefaultConsoleCommandLogFile()
		{
			var fileDto =  new FileDto(_defaultCmdConsoleLogFileBaseName
													  + "_" + _logfileTimeStamp + ".log");

			return FileHelper.DeleteAFile(fileDto);
		}

	}
}