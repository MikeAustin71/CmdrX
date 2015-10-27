using System.Collections.Generic;
using System.Configuration;
using CmdrX.Constants;

namespace CmdrX.GlobalConstants
{
	public static class AppConstants
	{
		public static ErrorLoggingStatus LoggingStatus { get; set; } = ErrorLoggingStatus.On;

		public static ErrorLoggingMode LoggingMode { get; set; } = ErrorLoggingMode.Verbose;

		public static string AppLogFileNameExtension { get; set; } = ConfigurationManager.AppSettings["ApplicationLogFileNameExtension"];

		public static int DefaultKillJobsRunOnExitCodeGreaterThan { get; set; } = int.Parse(ConfigurationManager.AppSettings["DefaultKillJobsRunOnExitCodeGreaterThan"]);

		public static int DefaultKillJobsRunOnExitCodeLessThan { get; set; } = int.Parse(ConfigurationManager.AppSettings["DefaultKillJobsRunOnExitCodeLessThan"]);

		public static string DefaultCommandOutputLogFilePathName { get; set; } = ConfigurationManager.AppSettings["DefaultCommandOutputLogFilePathName"];

		public static Dictionary<string, string> CommandLineArguments = new Dictionary<string, string>();

		public static string DefaultConsoleCommandExecutor = ConfigurationManager.AppSettings["DefaultConsoleCommandExecutor"];

		public static string DefaultConsoleCommandExeArgs = ConfigurationManager.AppSettings["DefaultConsoleCommandExeArguments"];

		public static decimal CommandMinTimeOutInMinutes = decimal.Parse(ConfigurationManager.AppSettings["CommandMinTimeOutInMinutes"]);

		public static decimal CommandMaxTimeOutInMinutes = decimal.Parse(ConfigurationManager.AppSettings["CommandMaxTimeOutInMinutes"]);

		public static decimal CommandDefaultTimeOutInMinutes = decimal.Parse(ConfigurationManager.AppSettings["CommandDefaultTimeOutInMinutes"]);

		public static string ConsoleErrorLogFileNameSuffix = "_Error";

		public static ConsoleCommandType DefaultConsoleCommandType = ConsoleCommandType.Console;

	}
}