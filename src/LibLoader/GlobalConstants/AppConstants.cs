﻿using System.Collections.Generic;
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

		public static AppicationLogMgr AppLogMgr { get; set; } = new AppicationLogMgr();

		public static string DefaultCommandOutputLogFileName { get; set; } = ConfigurationManager.AppSettings["CommandOutputLogFilePathBaseName"];

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