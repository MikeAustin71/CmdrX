using System;
using System.Collections.Generic;
using System.Net.Configuration;
using System.Security.Permissions;
using LibLoader.Constants;
using LibLoader.Models;

namespace LibLoader.Helpers
{
	public class ErrorLogger
	{
		private readonly int _errBaseCode ;
		private readonly string _thisClass;
		public ErrorLoggingStatus LoggingStatus { get; set; }
		public ErrorLoggingMode LoggingMode { get; set; }
		public bool IsLoggingConfigured { get; set; } = false;

		public List<FileOpsErrorMessageDto> ErrorList = new List<FileOpsErrorMessageDto>();


		public ErrorLogger(int errBaseCode, 
							string thisClass, 
							ErrorLoggingStatus loggingStatus = ErrorLoggingStatus.On,
							ErrorLoggingMode loggingMode = ErrorLoggingMode.Verbose,
							bool isLoggingConfigured = true)
		{
			_errBaseCode = errBaseCode;
			_thisClass = thisClass;
			LoggingStatus = loggingStatus;
			LoggingMode = loggingMode;
			IsLoggingConfigured = isLoggingConfigured;
		}

		public void WriteErrorMsg(FileOpsErrorMessageDto err)
		{
			ErrorList.Add(err);

			if (!IsLoggingConfigured || LoggingStatus == ErrorLoggingStatus.Off)
			{
				return;
			}

			err.ErrSourceClass = _thisClass;

			err.ErrId = _errBaseCode + err.ErrId;

			LogUtil.WriteLog(err);
		}

		public void WriteErrorMsgsToConsole(FileOpsErrorMessageDto err)
		{
			ErrorList.Add(err);

			WriteErrorMsgsToConsole();
		}
		

		public void WriteErrorMsgsToConsole()
		{
			foreach (var msg in ErrorList)
			{
				Console.WriteLine(msg.ErrSourceClass + ":" + msg.ErrSourceMethod + " ErrId " + msg.ErrId);
				Console.WriteLine("    " + msg.ErrorMessage);
				if (msg.ErrException != null)
				{
					Console.WriteLine(msg.ErrException.Message);
				}
				Console.WriteLine("");
			}
		}

	}
}