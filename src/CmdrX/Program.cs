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
				DefaultKillJobsRunOnExitCodeGreaterThan = AppConstants.DefaultKillJobsRunOnExitCodeGreaterThan,
				DefaultKillJobsRunOnExitCodeLessThan = AppConstants.DefaultKillJobsRunOnExitCodeLessThan,
                DefaultCommandOutputLogFilePathName = AppConstants.DefaultCommandOutputLogFilePathName,
				XmlCmdFileDto = AppInfoHelper.GetDefaultXmlCommandFile(),
				DefaultConsoleCommandType = AppConstants.DefaultConsoleCommandType
			};


			try
			{
				if (!ProcessCmdArgs(cmdExeDto, args))
				{
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

			}
			catch(Exception ex)
			{
				if (Environment.ExitCode == 0)
				{
					Environment.ExitCode = -50;
				}
				var err = new FileOpsErrorMessageDto
				{
					DirectoryPath = string.Empty,
					ErrId = 10,
					ErrorMessage = "Exception Thrown On Setup",
					ErrSourceMethod = "Main()",
					ErrException = ex,
					FileName = string.Empty,
					LoggerLevel = LogLevel.FATAL
				};

				cmdExeDto.ApplicationExitStatus.IsExceptionThrown = true;
				cmdExeDto.ApplicationExitStatus.OpsError = _errorMgr.FormatErrorDto(err);
				cmdExeDto.ApplicationExitStatus.WriteExitConsoleMessage();
				return;
			}

			try
			{
				ExecuteConsoleCommands(cmdJobs, cmdExeDto);

			}
			catch (Exception ex)
			{
				if (Environment.ExitCode == 0)
				{
					Environment.ExitCode = -50;
				}

				var err = new FileOpsErrorMessageDto
				{
					DirectoryPath = string.Empty,
					ErrId = 150,
					ErrorMessage = "Command Execution Exception Thrown: ",
					ErrSourceMethod = "SetUpLogging()",
					ErrException = ex,
					FileName = string.Empty,
					LoggerLevel = LogLevel.ERROR
				};

				cmdExeDto.ApplicationExitStatus.IsExceptionThrown = true;
				cmdExeDto.ApplicationExitStatus.OpsError = _errorMgr.FormatErrorDto(err);

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
				cmdExeDto?.ApplicationExitStatus.WriteExitConsoleMessage();
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

				Environment.ExitCode = -2;
				var err = new FileOpsErrorMessageDto
				{
					DirectoryPath = string.Empty,
					ErrId = 10,
					ErrorMessage = "Application Log Setup Failure!",
					ErrSourceMethod = "SetUpLogging()",
					ErrException = ex,
					FileName = string.Empty,
					LoggerLevel = LogLevel.ERROR
				};

				cmdExeDto.ApplicationExitStatus.IsExceptionThrown = true;
				cmdExeDto.ApplicationExitStatus.OpsError = _errorMgr.FormatErrorDto(err);
				cmdExeDto.ApplicationExitStatus.WriteExitConsoleMessage();

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

					cmdExeDto.ApplicationExitStatus.OpsError = _errorMgr.FormatErrorDto(err);
					cmdExeDto.ApplicationExitStatus.IsTerminateApp = true;
					cmdExeDto.ApplicationExitStatus.WriteExitConsoleMessage();
					Environment.ExitCode = -3;
					return false;
				}

			}
			catch(Exception ex) 
			{
				var err = new FileOpsErrorMessageDto
				{
					DirectoryPath = string.Empty,
					ErrId = 137,
					ErrorMessage = ex.Message,
					ErrSourceMethod = "ParseCommandJobsFromXml()",
					ErrException = ex,
					FileName = string.Empty,
					LoggerLevel = LogLevel.FATAL
				};

				_errorMgr.LoggingStatus = ErrorLoggingStatus.On;
				_errorMgr.WriteErrorMsg(err);
				Environment.ExitCode = -3;
				cmdExeDto.ApplicationExitStatus.OpsError = _errorMgr.FormatErrorDto(err);
				cmdExeDto.ApplicationExitStatus.IsExceptionThrown = true;
				cmdExeDto.ApplicationExitStatus.WriteExitConsoleMessage();

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
					ErrId = 345,
					ErrorMessage = "Command Job Execution Failed!",
					ErrSourceMethod = "ExecuteConsoleCommands()",
					ErrException = ex,
					FileName = string.Empty,
					LoggerLevel = LogLevel.FATAL
				};

				Environment.ExitCode = -4;
				_errorMgr.LoggingStatus = ErrorLoggingStatus.On;
				_errorMgr.WriteErrorMsg(err);
				cmdExeDto.ApplicationExitStatus.OpsError = _errorMgr.FormatErrorDto(err);
				cmdExeDto.ApplicationExitStatus.IsExceptionThrown = true;
				cmdExeDto.ApplicationExitStatus.WriteExitConsoleMessage();
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
					ErrSourceMethod = "ValidateXmlCommandFile()",
					FileName = string.Empty,
					LoggerLevel = LogLevel.FATAL
				};

				cmdExeDto.ApplicationExitStatus.IsFatalError = true;
				cmdExeDto.ApplicationExitStatus.OpsError = _errorMgr.FormatErrorDto(err);
				Environment.ExitCode = -5;
				cmdExeDto.ApplicationExitStatus.WriteExitConsoleMessage();
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
				return false;
			}

			return true;
		}
	}
}
