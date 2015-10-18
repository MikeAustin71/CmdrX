using System.Collections.Generic;
using System.Configuration;
using LibLoader.Constants;
using LibLoader.Helpers;
using LibLoader.Managers;
using LibLoader.Models;

namespace LibLoader.GlobalConstants
{
	public static class AppConstants
	{
		public static ErrorLoggingStatus LoggingStatus { get; set; } = ErrorLoggingStatus.On;

		public static ErrorLoggingMode LoggingMode { get; set; } = ErrorLoggingMode.Verbose;

		public static FileDto XmlCmdFileDto { get; set; } = AppInfoHelper.GetDefaultXmlCommandFile();

		public static AppicationLogMgr AppLogMgr { get; set; } = new AppicationLogMgr();

		public static string CommandOutputLogFileBaseName { get; set; } = ConfigurationManager.AppSettings["CommandOutputLogFile"];

		public static Dictionary<string, string> CommandLineArguments = new Dictionary<string, string>();

		public static string ConsoleCommandExecutor = ConfigurationManager.AppSettings["ConsoleCommandExecutor"];

		public static string ConsoleErrorLogFileNameSuffix = "_Error";

	}
}