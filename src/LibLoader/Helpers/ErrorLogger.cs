using System.Collections.Generic;
using LibLoader.Constants;
using LibLoader.GlobalConstants;
using LibLoader.Models;

namespace LibLoader.Helpers
{
	public class ErrorLogger
	{
		private readonly int _errBaseCode ;
		private readonly string _thisClass;
		public ErrorLoggingStatus LoggingStatus { get; set; }
		public ErrorLoggingMode LoggingMode { get; set; }
		public List<FileOpsErrorMessageDto> ErrorList = new List<FileOpsErrorMessageDto>();


		public ErrorLogger(int errBaseCode, 
							string thisClass, 
							ErrorLoggingStatus loggingStatus = ErrorLoggingStatus.On,
							ErrorLoggingMode loggingMode = ErrorLoggingMode.Verbose)
		{
			_errBaseCode = errBaseCode;
			_thisClass = thisClass;
			LoggingStatus = loggingStatus;
			LoggingMode = loggingMode;
		}

		public void WriteErrorMsg(FileOpsErrorMessageDto err)
		{
			if (LoggingStatus == ErrorLoggingStatus.Off)
			{
				return;
			}

			err.ErrSourceClass = _thisClass;

			err.ErrId = _errBaseCode + err.ErrId;

			LogUtil.WriteLog(err);
		}



	}
}