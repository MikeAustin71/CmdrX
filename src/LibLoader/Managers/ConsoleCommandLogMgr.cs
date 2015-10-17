using System;
using System.Collections.Generic;
using System.IO;
using LibLoader.Constants;
using LibLoader.GlobalConstants;
using LibLoader.Helpers;
using LibLoader.Models;

namespace LibLoader.Managers
{
	public class ConsoleCommandLogMgr
	{
		public ErrorLogger ErrorMgr = new
			ErrorLogger(7388000,
						"ConsoleCommandLogMgr",
						AppConstants.LoggingStatus,
						AppConstants.LoggingMode);

		private string _defaultCmdConsoleLogFileBaseName;

		private string _logfileTimeStamp;

		private string _currentCmdConsoleLogFileBaseName = string.Empty;

		private string _currentCmdConsoleLogFileNameAndExt = string.Empty;

		private FileDto _currentLogfileDto;

		private StreamWriterDto _swDto;

		private Dictionary<string, int> _logFilesHistory = new Dictionary<string, int>();

		public ConsoleCommandLogMgr(string defaultCmdConsoleLogFileBaseName, string logFileTimeStamp)
		{
			_defaultCmdConsoleLogFileBaseName = defaultCmdConsoleLogFileBaseName;
			_logfileTimeStamp = logFileTimeStamp;

			DeleteDefaultConsoleCommandLogFile();

		}

		private bool SetConsoleLog(string commandLogFileBaseName)
		{
			_currentCmdConsoleLogFileBaseName = commandLogFileBaseName;

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

			if (_currentLogfileDto?.FileXinfo == null)
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

			int count;

			if (!_logFilesHistory.TryGetValue(_currentLogfileDto.FileXinfo.FullName, out count))
			{
				_logFilesHistory.Add(_currentLogfileDto.FileXinfo.FullName, 1);
			}
			else
			{
				count++;
				_logFilesHistory[_currentLogfileDto.FileXinfo.FullName] = count;
			}

			return true;
		}

		public bool InitializeCmdConsoleLog(string commandLogFileBaseName)
		{

			if (!SetConsoleLog(commandLogFileBaseName))
			{
				return false;
			}

			if (_currentLogfileDto?.FileXinfo == null)
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