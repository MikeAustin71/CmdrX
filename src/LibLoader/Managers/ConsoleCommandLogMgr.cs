﻿using System;
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

		private FileDto _currentLogfileDto;

		private readonly FileDto _defautlLogFileDto;

		private StreamWriterDto _swDto;

		public ErrorLogger ErrorMgr ;

		public int NumberOfLogLinesWritten { get; set; }

		public ConsoleCommandLogMgr(
				string defaultCmdConsoleLogPathFileName, 
					string cmdConsoleFileErrorSuffix,
						string logFileTimeStamp)
		{
			ErrorMgr = new
			ErrorLogger(7388000,
						"ConsoleCommandLogMgr",
						AppConstants.LoggingStatus,
						AppConstants.LoggingMode);

			_logfileTimeStamp = logFileTimeStamp;

			_cmdConsoleFileErrorSuffix = cmdConsoleFileErrorSuffix;

			_defautlLogFileDto = ExtractLogFileDto(defaultCmdConsoleLogPathFileName,
														_cmdConsoleFileErrorSuffix,
															_logfileTimeStamp);


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

			_logfileTimeStamp = logFileTimeStamp;

			FileHelper.DeleteAFile(_defautlLogFileDto);

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

		public bool InitializeCmdConsoleLog(string commandLogFilePathName)
		{

			if (string.IsNullOrWhiteSpace(commandLogFilePathName))
			{
				_currentLogfileDto = new FileDto(_defautlLogFileDto.FileXinfo.FullName);

				return true;
			}

			_currentLogfileDto = ExtractLogFileDto(commandLogFilePathName,
														_cmdConsoleFileErrorSuffix,
															_logfileTimeStamp);

			if (!FileHelper.IsFileDtoValid(_currentLogfileDto))
			{
				_currentLogfileDto.Dispose();
				
				_currentLogfileDto = new FileDto(_defautlLogFileDto.FileXinfo.FullName);
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


			_swDto.GetStreamWriter().WriteLine(Environment.NewLine + outputLine);
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


		public FileDto ExtractLogFileDto(string defaultCmdConsoleLogPathFileName, 
													string errorSuffix,
														string logFileTimeStamp)
		{
			var filePath = new FilePathDto(defaultCmdConsoleLogPathFileName);

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


			if (filePath.HasExtensinon)
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