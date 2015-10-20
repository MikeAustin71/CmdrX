using System;
using LibLoader.Constants;
using LibLoader.GlobalConstants;
using LibLoader.Helpers;

namespace LibLoader.Models
{
	public class ConsoleExecutorDto
	{

		private string _defaultConsoleCommandExecutor;
		private string _defaultConsoleCommandExeArgs;
		private string _defaultCmdConsoleLogFilePathName;
		private string _cmdConsoleLogFileErrorSuffix;
		private string _cmdConsoleLogFileTimeStamp;

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


		public string DefaultCmdConsoleLogFilePathName
		{
			get { return _defaultCmdConsoleLogFilePathName; }
			set { _defaultCmdConsoleLogFilePathName = StringHelper.TrimStringEnds(value); }
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

		public FileDto XmlCmdFileDto { get; set; }

	}
}