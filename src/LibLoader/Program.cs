﻿using System;
using LibLoader.Builders;
using LibLoader.Constants;
using LibLoader.GlobalConstants;
using LibLoader.Helpers;
using LibLoader.Managers;
using LibLoader.Models;

namespace LibLoader
{
	class Program
	{
		private static ErrorLogger _errorMgr;

		static void Main(string[] args)
		{

			JobsGroupDto cmdJobs;

			var cmdExeDto = new ConsoleExecutorDto()
			{
				DefaultConsoleCommandExecutor = AppConstants.DefaultConsoleCommandExecutor,
				DefaultConsoleCommandExeArgs = AppConstants.DefaultConsoleCommandExeArgs,
				CmdConsoleLogFileErrorSuffix = AppConstants.ConsoleErrorLogFileNameSuffix,
				CmdConsoleLogFileTimeStamp = DateHelper.NowYearMthDayHrsSecs(),
				CommandDefaultTimeOutInMinutes = AppConstants.CommandDefaultTimeOutInMinutes,
				CommandMaxTimeOutInMinutes = AppConstants.CommandMaxTimeOutInMinutes,
				CommandMinTimeOutInMinutes = AppConstants.CommandMinTimeOutInMinutes,
				DefaultCmdConsoleLogFilePathName = AppConstants.DefaultCommandOutputLogFileName,
				XmlCmdFileDto = AppInfoHelper.GetDefaultXmlCommandFile(),
				DefaultConsoleCommandType = AppConstants.DefaultConsoleCommandType
			};


			if (!SetUpLogging()
				|| !ProcessCmdArgs(cmdExeDto, args)
				|| !ValidateXmlCommandFile(cmdExeDto)
				|| !ParseCommandJobsFromXml(cmdExeDto, out cmdJobs))
			{
				return;
			}


			ExecuteConsoleCommands(cmdJobs, cmdExeDto);

		}

		private static bool SetUpLogging()
		{
			try
			{
				// Setup Application Logging
				AppConstants.LoggingStatus = ErrorLoggingStatus.On;
				AppConstants.LoggingMode = ErrorLoggingMode.Verbose;
				_errorMgr = new
					ErrorLogger(1000,
						"Program",
						AppConstants.LoggingStatus,
						AppConstants.LoggingMode);

				if (!AppConstants.AppLogMgr.CreateApplicaitonLogDirectory())
				{
					Console.WriteLine("Application Log Directory Invalid!");
					Environment.ExitCode = -2;
					return false;
				}

				LogUtil.ExeAssemblyVersionNo = AppInfoHelper.GetThisAssemblyVersion();

				AppConstants.AppLogMgr.PurgeLogCmd.Execute();

			}
			catch (Exception)
			{

				Console.WriteLine("Application Log Setup Failure!");
				Environment.ExitCode = -2;
				return false;
			}

			return true;
		}

		private static bool ParseCommandJobsFromXml(ConsoleExecutorDto cmdExeDto, out JobsGroupDto jobsGroupDto)
		{
			jobsGroupDto = null;

			try
			{
				var xmlParser = new XmlParameterBuilder(cmdExeDto);
				jobsGroupDto = xmlParser.BuildParmsFromXml();

				if (jobsGroupDto == null || jobsGroupDto.NumberOfJobs < 1)
				{
					var err = new FileOpsErrorMessageDto
					{
						DirectoryPath = string.Empty,
						ErrId = 35,
						ErrorMessage = "Zero jobs were extracted from the XmlCommands file!",
						ErrSourceMethod = "ParseCommandJobsFromXml()",
						FileName = string.Empty,
						LoggerLevel = LogLevel.FATAL
					};

					_errorMgr.LoggingStatus = ErrorLoggingStatus.On;
					_errorMgr.WriteErrorMsg(err);
					Environment.ExitCode = -3;
					Console.WriteLine(err.ErrorMessage);
					return false;
				}

			}
			catch(Exception ex) 
			{
				var err = new FileOpsErrorMessageDto
				{
					DirectoryPath = string.Empty,
					ErrId = 37,
					ErrorMessage = "Zero jobs were extracted from the XmlCommands file!",
					ErrSourceMethod = "ParseCommandJobsFromXml()",
					ErrException = ex,
					FileName = string.Empty,
					LoggerLevel = LogLevel.FATAL
				};

				_errorMgr.LoggingStatus = ErrorLoggingStatus.On;
				_errorMgr.WriteErrorMsg(err);

				Console.WriteLine(err.ErrorMessage);
				Environment.ExitCode = -3;
				return false;
			}

			return true;
		}



		private static void ExecuteConsoleCommands(JobsGroupDto cmdJobs, ConsoleExecutorDto cmdExeDto)
		{
			try
			{

				var mgr = new CommandExecutionMgr(cmdJobs,cmdExeDto);

				mgr.ExecuteCommands();
			}
			catch(Exception ex)
			{
				var err = new FileOpsErrorMessageDto
				{
					DirectoryPath = string.Empty,
					ErrId = 45,
					ErrorMessage = "Command Job Execution Failed!",
					ErrSourceMethod = "Main()",
					ErrException = ex,
					FileName = string.Empty,
					LoggerLevel = LogLevel.FATAL
				};

				_errorMgr.LoggingStatus = ErrorLoggingStatus.On;
				_errorMgr.WriteErrorMsg(err);
				Environment.ExitCode = -4;
				Console.WriteLine(err.ErrorMessage);
			}
		}

		private static bool ValidateXmlCommandFile(ConsoleExecutorDto cmdExeDto)
		{
			if (!FileHelper.DoesFileExist(cmdExeDto.XmlCmdFileDto))
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

				_errorMgr.LoggingStatus = ErrorLoggingStatus.On;
				_errorMgr.WriteErrorMsg(err);

				AppInfoHelper.DisplayCmdLineParms();
				Environment.ExitCode = -5;
				return false;
			}

			return true;
		}

		private static bool ProcessCmdArgs(ConsoleExecutorDto cmdExeDto, string[] args)
		{
			if (args == null || args.Length < 1)
			{
				return true;
			}

			if (!new CommandLineParameterBuilder(cmdExeDto).BuildFileInfoParamters(args))
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

				_errorMgr.LoggingStatus = ErrorLoggingStatus.On;
				_errorMgr.WriteErrorMsg(err);

				AppInfoHelper.DisplayCmdLineParms();
				Environment.ExitCode = 0;
				return false;
			}

			return true;
		}
	}
}
