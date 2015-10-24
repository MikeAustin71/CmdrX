using System;
using CmdrX.Builders;
using CmdrX.Constants;
using CmdrX.GlobalConstants;
using CmdrX.Helpers;
using CmdrX.Managers;
using CmdrX.Models;

namespace CmdrX
{
	class Program
	{
		private static ErrorLogger _errorMgr;

		static void Main(string[] args)
		{
			_errorMgr = new
					ErrorLogger(1000,
						"Program",
						AppConstants.LoggingStatus,
						AppConstants.LoggingMode, 
						false);

			AppConstants.LoggingStatus = ErrorLoggingStatus.On;
			AppConstants.LoggingMode = ErrorLoggingMode.Verbose;
			JobsGroupDto cmdJobs = null;

			var nowTimeStamp = DateHelper.NowYearMthDayHrsSecs();

			var cmdExeDto = new ConsoleExecutorDto()
			{   AppLogFileBaseNameOnly =
					PathHelper.ExtractFileNameOnlyComponent(AppConstants.AppLogFileNameExtension),
				AppLogFileExtensionWithoutLeadingDot = 
					PathHelper.ExtractFileExtensionComponentWithoutLeadingDot(AppConstants.AppLogFileNameExtension),
				AppLogDirectory = PathHelper.ExtractDirectoryComponent(AppConstants.DefaultCommandOutputLogFilePathName),
				AppLogFileTimeStamp = nowTimeStamp,
                DefaultConsoleCommandExecutor = AppConstants.DefaultConsoleCommandExecutor,
				DefaultConsoleCommandExeArgs = AppConstants.DefaultConsoleCommandExeArgs,
				CmdConsoleLogFileErrorSuffix = AppConstants.ConsoleErrorLogFileNameSuffix,
				CmdConsoleLogFileTimeStamp = nowTimeStamp,
				CommandDefaultTimeOutInMinutes = AppConstants.CommandDefaultTimeOutInMinutes,
				CommandMaxTimeOutInMinutes = AppConstants.CommandMaxTimeOutInMinutes,
				CommandMinTimeOutInMinutes = AppConstants.CommandMinTimeOutInMinutes,
				DefaultCommandOutputLogFilePathName = AppConstants.DefaultCommandOutputLogFilePathName,
				XmlCmdFileDto = AppInfoHelper.GetDefaultXmlCommandFile(),
				DefaultConsoleCommandType = AppConstants.DefaultConsoleCommandType
			};

			try
			{
				if (!ProcessCmdArgs(cmdExeDto, args))
				{
					return;
				}

				if (!ValidateXmlCommandFile(cmdExeDto))
				{
					return;
				}

				if (!ParseCommandJobsFromXml(cmdExeDto, out cmdJobs))
				{
					return;
				}

				if (!SetUpLogging(cmdExeDto, cmdJobs))
				{
					return;
				}

				
				ExecuteConsoleCommands(cmdJobs, cmdExeDto);

			}
			catch (Exception ex)
			{
				if (Environment.ExitCode == 0)
				{
					Environment.ExitCode = -20;
				}

				var err = new FileOpsErrorMessageDto
				{
					DirectoryPath = string.Empty,
					ErrId = 10,
					ErrorMessage = "Setup Failure: " + ex.Message,
					ErrSourceMethod = "SetUpLogging()",
					ErrException = ex,
					FileName = string.Empty,
					LoggerLevel = LogLevel.ERROR
				};

				Console.WriteLine(ex.Message);
				_errorMgr.LoggingStatus = ErrorLoggingStatus.On;
				_errorMgr.WriteErrorMsg(err);

			}
			finally
			{
				AppShutdownAndCleanUp(cmdJobs, cmdExeDto);
			}
			
		}

		private static void AppShutdownAndCleanUp(JobsGroupDto jobs, ConsoleExecutorDto cmdExeDto)
		{
			try
			{
				LogUtil.WriteLogJobGroupCompletionMessage(jobs);

			}
				// ReSharper disable once EmptyGeneralCatchClause
			catch
			{
			}
			finally
			{
				jobs?.Dispose();
				cmdExeDto?.Dispose();

			}
		}

		private static bool SetUpLogging(ConsoleExecutorDto cmdExeDto, JobsGroupDto jobsGroup)
		{

			try
			{
				// Setup Application Logging
				cmdExeDto.AppLogMgr.PurgeOldLogFiles();

				LogUtil.ConfigureLogger(cmdExeDto.AppLogMgr.ConfigureLogger());

				_errorMgr.IsLoggingConfigured = true;

				LogUtil.ExeAssemblyVersionNo = AppInfoHelper.GetThisAssemblyVersion();

				LogUtil.WriteLogJobGroupStartUpMessage(jobsGroup);

				cmdExeDto.ErrorMgr.IsLoggingConfigured = true;

			}
			catch (Exception ex)
			{

				Console.WriteLine("Application Log Setup Failure!");
				Environment.ExitCode = -2;
				var err = new FileOpsErrorMessageDto
				{
					DirectoryPath = string.Empty,
					ErrId = 10,
					ErrorMessage = "Application Log Setup Failure!" + ex.Message,
					ErrSourceMethod = "SetUpLogging()",
					ErrException = ex,
					FileName = string.Empty,
					LoggerLevel = LogLevel.ERROR
				};

				_errorMgr.LoggingStatus = ErrorLoggingStatus.On;
				_errorMgr.WriteErrorMsg(err);

				Console.WriteLine("SetupLogging() " + ex.Message);
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
					ErrorMessage = ex.Message,
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

				_errorMgr.WriteErrorMsgsToConsole(err);

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

			var cmdLineParser = new CommandLineParameterBuilder(cmdExeDto);

            if (!cmdLineParser.BuildFileInfoParamters(args))
			{
				cmdLineParser.ErrorMgr.WriteErrorMsgsToConsole();

				AppInfoHelper.DisplayCmdLineParms();

				Environment.ExitCode = 0;

				return false;
			}

			return true;
		}
	}
}
