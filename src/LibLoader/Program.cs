using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibLoader.Builders;
using LibLoader.Commands;
using LibLoader.Constants;
using LibLoader.GlobalConstants;
using LibLoader.Helpers;
using LibLoader.Models;

namespace LibLoader
{
	class Program
	{
		private static ErrorLogger ErrorMgr;

		static void Main(string[] args)
		{

			// Setup Application Logging
			AppConstants.LoggingStatus = ErrorLoggingStatus.On;
			AppConstants.LoggingMode = ErrorLoggingMode.Verbose;
			ErrorMgr = new
				ErrorLogger(1000,
							"Program",
							AppConstants.LoggingStatus,
							AppConstants.LoggingMode);

			if (!AppConstants.AppLogMgr.CreateApplicaitonLogDirectory())
			{
				Console.WriteLine("Application Log Directory Invalid!");
				Environment.ExitCode = -1;
				return;
			}


			AppConstants.AppLogMgr.PurgeLogCmd.Execute();
			LogUtil.ExeAssemblyVersionNo = AppInfoHelper.GetThisAssemblyVersion();

			JobsGroupDto cmdJobs;

			if (!ProcessCmdArgs(args)
				|| !ValidateXmlCommandFile()
				|| !ParseCommandJobsFromXml(out cmdJobs)
				|| !ExecuteConsoleCommands(cmdJobs))
			{
				return;
			}



		}

		private static bool ExecuteConsoleCommands(JobsGroupDto cmdJobs)
		{
			try
			{

			}
			catch
			{
				Environment.ExitCode = -1;
				return false;
			}

			return true;
		}

		private static bool ParseCommandJobsFromXml(out JobsGroupDto jobsGroupDto)
		{
			var xmlParser = new XmlParameterBuilder(AppConstants.XmlCmdFileDto);
			jobsGroupDto = xmlParser.BuildParmsFromXml();

			if (jobsGroupDto == null || jobsGroupDto.NumberOfJobs < 1)
			{
				var err = new FileOpsErrorMessageDto
				{
					DirectoryPath = string.Empty,
					ErrId = 35,
					ErrorMessage = "Zero jobs were extracted from the XmlCommands file!",
					ErrSourceMethod = "Main()",
					FileName = string.Empty,
					LoggerLevel = LogLevel.FATAL
				};

				ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
				ErrorMgr.WriteErrorMsg(err);
				Environment.ExitCode = -1;
				return false;
			}

			return true;
		}

		private static bool ValidateXmlCommandFile()
		{
			if (!FileHelper.DoesFileExist(AppConstants.XmlCmdFileDto))
			{
				var err = new FileOpsErrorMessageDto
				{
					DirectoryPath = string.Empty,
					ErrId = 3,
					ErrorMessage = "Could Not Locate a valid Xml Commands file!",
					ErrSourceMethod = "Main()",
					FileName = string.Empty,
					LoggerLevel = LogLevel.FATAL
				};

				ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
				ErrorMgr.WriteErrorMsg(err);

				AppInfoHelper.DisplayCmdLineParms();
				Environment.ExitCode = -1;
				return false;
			}

			return true;
		}

		private static bool ProcessCmdArgs(string[] args)
		{
			if (args == null || args.Length < 1)
			{
				return true;
			}

			if (!new CommandLineParameterBuilder().BuildFileInfoParamters(args))
			{
				var err = new FileOpsErrorMessageDto
				{
					DirectoryPath = string.Empty,
					ErrId = 2,
					ErrorMessage = "Arg Parser Returned False. Args Invalid or Help Requested",
					ErrSourceMethod = "Main()",
					FileName = string.Empty,
					LoggerLevel = LogLevel.INFO
				};

				ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
				ErrorMgr.WriteErrorMsg(err);

				AppInfoHelper.DisplayCmdLineParms();
				Environment.ExitCode = 0;
				return false;
			}

			return true;
		}
	}
}
