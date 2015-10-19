using System;
using LibLoader.Commands;
using LibLoader.Constants;
using LibLoader.GlobalConstants;
using LibLoader.Helpers;
using LibLoader.Models;

namespace LibLoader.Managers
{
	public class CommandExectutionMgr
	{

		public ErrorLogger ErrorMgr = new
			ErrorLogger(1677000,
						"CommandExectutionMgr",
						AppConstants.LoggingStatus,
						AppConstants.LoggingMode);

		private JobsGroupDto _commandJobs;

		private ConsoleCommandLogMgr _cmdLogMgr;

		private ConsoleCommandLogMgr _errLogMgr;

		private WorkingDirectoryMgr _wrkDirMgr;

		private readonly ConsoleExecutorDto _consoleExecutor;

		public CommandExectutionMgr(JobsGroupDto jobsGroup,
								ConsoleExecutorDto consoleCmdDto)
		{
			_commandJobs = jobsGroup;

			_cmdLogMgr = new ConsoleCommandLogMgr(consoleCmdDto.DefaultCmdConsoleLogFilePathName,
													string.Empty, 
														consoleCmdDto.CmdConsoleLogFileTimeStamp);

			_errLogMgr = new ConsoleCommandLogMgr(consoleCmdDto.DefaultCmdConsoleLogFilePathName,
                                                        consoleCmdDto.CmdConsoleLogFileErrorSuffix,
															consoleCmdDto.CmdConsoleLogFileTimeStamp);
			_wrkDirMgr = new WorkingDirectoryMgr();

			_consoleExecutor = consoleCmdDto;
		}

		public bool ExecuteCommands()
		{
			bool result;

			try
			{
				foreach (var job in _commandJobs.Jobs)
				{
					var exeCmd = new ExecuteConsoleCommand(job,_consoleExecutor, _cmdLogMgr, _errLogMgr, _wrkDirMgr);
					var exitCode = exeCmd.Execute();

					if (exitCode != 0)
					{
						var msg = "Command Returned Failed Exit Code: " 
							+ exitCode + " Job Name: " + job.CommandDisplayName;

                        Environment.ExitCode = exitCode;
						var err = new FileOpsErrorMessageDto
						{
							DirectoryPath = string.Empty,
							ErrId = 10,
							ErrorMessage = msg,
							ErrSourceMethod = "ExecuteCommands()",
							FileName = string.Empty,
							LoggerLevel = LogLevel.ERROR
						};

						ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
						ErrorMgr.WriteErrorMsg(err);
						Console.WriteLine(msg);
						Environment.ExitCode = exitCode;
					}
				}

				Environment.ExitCode = 0;
				result = true;
			}
			catch (Exception ex)
			{
				var err = new FileOpsErrorMessageDto
				{
					DirectoryPath = string.Empty,
					ErrId = 20,
					ErrorMessage = "Exception thrown while executing commands! " + ex.Message,
					ErrSourceMethod = "ExecuteCommands()",
					ErrException = ex,
					FileName = string.Empty,
					LoggerLevel = LogLevel.ERROR
				};

				ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
				ErrorMgr.WriteErrorMsg(err);
				Environment.ExitCode = -8;
				result = false;
			}
			finally
			{
				_wrkDirMgr.ChangeBackToOriginalWorkingDirectory();
				_wrkDirMgr.Dispose();
				_wrkDirMgr = null;

				_commandJobs.Dispose();
				_commandJobs = null;

				_cmdLogMgr.Dispose();
				_cmdLogMgr = null;

				_errLogMgr.Dispose();
				_errLogMgr = null;

			}

			return result;
		} 
	}
}