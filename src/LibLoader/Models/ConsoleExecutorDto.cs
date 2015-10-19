using System;
using LibLoader.Constants;
using LibLoader.GlobalConstants;
using LibLoader.Helpers;

namespace LibLoader.Models
{
	public class ConsoleExecutorDto
	{

		private string _exeCommand;
		private string _exeCmdArguments;
		private string _defaultCmdConsoleLogFilePathName;
		private string _cmdConsoleLogFileErrorSuffix;
		private string _cmdConsoleLogFileTimeStamp;

		public string ExeCommand
		{
			get { return _exeCommand; }
			private set { _exeCommand = StringHelper.TrimStringEnds(value); }
		}

		public string ExeCmdArguments
		{
			get { return _exeCmdArguments; }
			private set { _exeCmdArguments = StringHelper.TrimStringEnds(value); }
		}

		public int NumberOfMiliSecondsToWaitForExecution { get; private set; }


		public string DefaultCmdConsoleLogFilePathName
		{
			get { return _defaultCmdConsoleLogFilePathName; }
			private set { _defaultCmdConsoleLogFilePathName = StringHelper.TrimStringEnds(value); }
		}

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

		public ErrorLogger ErrorMgr = new
			ErrorLogger(9148000,
						"ConsoleExecutorDto",
						AppConstants.LoggingStatus,
						AppConstants.LoggingMode);


		public ConsoleExecutorDto(
					string exeCommand, 
						string exeCmdArguments, 
							int numberOfMinutesToWaitForExecution,
								string defaultCmdConsoleLogFilePathName,
									string cmdConsoleLogFileErrorSuffix,
										string cmdConsoleLogFileTimeStamp)
		{

			if (string.IsNullOrWhiteSpace(exeCommand))
			{
				var ex = new Exception("Error: Console Exe Command is Empty!");

				var err = new FileOpsErrorMessageDto
				{
					DirectoryPath = string.Empty,
					ErrId = 10,
					ErrorMessage = ex.Message,
					ErrSourceMethod = "Constructor()",
					ErrException = ex,
					FileName = string.Empty,
					LoggerLevel = LogLevel.FATAL
				};

				ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
				ErrorMgr.WriteErrorMsg(err);

			}


			ExeCommand = exeCommand;

			ExeCmdArguments = exeCmdArguments;

			NumberOfMiliSecondsToWaitForExecution = numberOfMinutesToWaitForExecution*60000;

			_defaultCmdConsoleLogFilePathName = defaultCmdConsoleLogFilePathName;

			_cmdConsoleLogFileErrorSuffix = cmdConsoleLogFileErrorSuffix;

			_cmdConsoleLogFileTimeStamp = cmdConsoleLogFileTimeStamp;

		}

	}
}