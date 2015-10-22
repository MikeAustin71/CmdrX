using System;
using System.IO;
using System.Text;
using LibLoader.Constants;
using LibLoader.GlobalConstants;
using LibLoader.Helpers;
using LibLoader.Models;

namespace LibLoader.Managers
{
	public class ConsoleCommandLogMgr
	{
		private bool _disposed;

		private readonly string _logfileTimeStamp;

		private readonly string _cmdConsoleFileErrorSuffix;

		private string _currentConsoleLogPathFileBaseName;

		private FileDto _currentLogfileDto;

		private readonly string _defaultConsoleLogPathFileBaseName;

		private FileDto _defautlLogFileDto;

		public string DefaultConsoleLogPathFileBaseName => _defaultConsoleLogPathFileBaseName;

		public FileDto DefaultLogFileDto => _defautlLogFileDto;

		public string CurrentConsoleLogPathFileBaseName => _currentConsoleLogPathFileBaseName;

		public FileDto CurrentLogFileDto => _currentLogfileDto;

		private StreamWriterDto _swDto;

		public ErrorLogger ErrorMgr = new
			ErrorLogger(7388000,
						"ConsoleCommandLogMgr",
						AppConstants.LoggingStatus,
						AppConstants.LoggingMode);


		public int NumberOfLogLinesWritten { get; set; }

		public ConsoleCommandLogMgr(
				string defaultCmdConsoleLogPathFileName, 
						string logFileTimeStamp,
						string cmdConsoleFileErrorSuffix)
		{

			_logfileTimeStamp = StringHelper.TrimStringEnds(logFileTimeStamp);

			_cmdConsoleFileErrorSuffix = StringHelper.TrimStringEnds(cmdConsoleFileErrorSuffix);

			_defautlLogFileDto = ExtractLogFileDto(defaultCmdConsoleLogPathFileName,
														_cmdConsoleFileErrorSuffix,
															_logfileTimeStamp);

			_defaultConsoleLogPathFileBaseName = defaultCmdConsoleLogPathFileName;

			if (!FileHelper.IsFileDtoValid(_defautlLogFileDto))
			{
				var msg = "Default Command Console Log File is INVALID!";
				var err = new FileOpsErrorMessageDto
				{
					DirectoryPath = string.Empty,
					ErrId = 1,
					ErrorMessage = msg,
					ErrSourceMethod = "Constructor()",
					FileName = defaultCmdConsoleLogPathFileName,
					LoggerLevel = LogLevel.FATAL
				};

				ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
				ErrorMgr.WriteErrorMsg(err);

				throw new Exception(msg);

			}

			try
			{
				_logfileTimeStamp = logFileTimeStamp;

				FileHelper.DeleteAFile(_defautlLogFileDto);

				_currentConsoleLogPathFileBaseName = defaultCmdConsoleLogPathFileName;
				_currentLogfileDto = new FileDto(_defautlLogFileDto.FileXinfo.FullName);

				CreateNewStreamWriter(_currentLogfileDto);

			}
			catch (Exception ex)
			{
				var msg = "Consold Command Log Setup Failed! " + ex.Message;
				var err = new FileOpsErrorMessageDto
				{
					DirectoryPath = string.Empty,
					ErrId = 3,
					ErrorMessage = msg,
					ErrSourceMethod = "Constructor()",
					FileName = defaultCmdConsoleLogPathFileName,
					LoggerLevel = LogLevel.FATAL
				};

				ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
				ErrorMgr.WriteErrorMsg(err);

				throw;
			}


		}

		public void CreateNewStreamWriter(FileDto fileDto)
		{
			_swDto = new StreamWriterDto(_currentLogfileDto);

		}

		public void CloseStreamWriter()
		{
			_swDto.Close();
			_swDto = null;
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

					if (_defautlLogFileDto != null)
					{
						_defautlLogFileDto.Dispose();
						_defautlLogFileDto = null;
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

		public bool InitializeCmdConsoleLog(string commandLogFilePathName)
		{
			if (_disposed)
			{
				return false;
			}

			if (string.IsNullOrWhiteSpace(commandLogFilePathName))
			{
				if (!IsStreamWriterValid())
				{
					CreateNewStreamWriter(_currentLogfileDto);						
				}

				return true;
			}

			if (commandLogFilePathName == _currentConsoleLogPathFileBaseName)
			{
				return true;
			}

			_currentLogfileDto.Dispose();

			_currentLogfileDto = ExtractLogFileDto(commandLogFilePathName,
														_cmdConsoleFileErrorSuffix,
															_logfileTimeStamp);

			if (!FileHelper.IsFileDtoValid(_currentLogfileDto))
			{
				_currentLogfileDto.Dispose();
				
				_currentLogfileDto = new FileDto(_defautlLogFileDto.FileXinfo.FullName);
			}

			if (!DirectoryHelper.CreateDirectoryIfNecessary(_currentLogfileDto.DirDto) )
			{
				var ex = new Exception("Failure Creating Command Console Log File Directory: " +_currentLogfileDto.DirDto.DirInfo.FullName);
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


			CloseStreamWriter();

			CreateNewStreamWriter(_currentLogfileDto);

			_currentConsoleLogPathFileBaseName = commandLogFilePathName;

			NumberOfLogLinesWritten = 0;

			return true;
		}

		public void LogWriteLine(string outputLine)
		{

			if (!IsStreamWriterValid())
			{
				var msg = "Stream Writer Dto Invalid!";
				var err = new FileOpsErrorMessageDto
				{
					DirectoryPath = string.Empty,
					ErrId = 30,
					ErrorMessage = msg,
					ErrSourceMethod = "LogWriteLine()",
					FileName = _currentLogfileDto.FileXinfo.FullName,
					LoggerLevel = LogLevel.FATAL
				};

				ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
				ErrorMgr.WriteErrorMsg(err);

				throw new Exception(msg);

			}

			if (string.IsNullOrWhiteSpace(outputLine))
			{
				return;
			}

			NumberOfLogLinesWritten++;


			_swDto.GetStreamWriter().WriteLine(outputLine);
		}


		public void LogFlushStreamWriter()
		{
			_swDto.GetStreamWriter().Flush();
		}

		private bool IsStreamWriterValid()
		{
			if (_swDto?.GetStreamWriter() == null)
			{
				return false;
			}


			return true;
		}

		public bool ResetToDefaultConsoleLogPathFileName()
		{
			return InitializeCmdConsoleLog(_defaultConsoleLogPathFileBaseName);
		}

		public FileDto ExtractLogFileDto(string cmdConsoleLogPathFileName, 
													string errorSuffix,
														string logFileTimeStamp)
		{
			var filePath = new FilePathDto(cmdConsoleLogPathFileName);

			var sb = new StringBuilder();

			if (filePath.HasDirectoryPath)
			{
				sb.Append(Path.Combine(filePath.DirectoryPath, filePath.FileNameOnly));
			}
			else
			{
				sb.Append(filePath.FileNameOnly);
			}

			if (!string.IsNullOrWhiteSpace(errorSuffix))
			{
				sb.Append("_" + errorSuffix);
			}

			sb.Append("_" + logFileTimeStamp);


			if (filePath.HasExtension)
			{
				if (filePath.Extension.Contains("."))
				{
					sb.Append(filePath.Extension);
				}
				else
				{
					sb.Append(".");
					sb.Append(filePath.Extension);
				}
			}


			return new FileDto(sb.ToString());
		}


	}
}