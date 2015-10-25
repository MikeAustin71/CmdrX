﻿using System;
using CmdrX.Commands;
using CmdrX.Constants;
using CmdrX.GlobalConstants;
using CmdrX.Helpers;
using CmdrX.Models;

namespace CmdrX.Managers
{
	public class CommandExecutionMgr
	{

		public ErrorLogger ErrorMgr = new
			ErrorLogger(1677000,
						"CommandExecutionMgr",
						AppConstants.LoggingStatus,
						AppConstants.LoggingMode);

		private JobsGroupDto _commandJobs;

		private ConsoleCommandLogMgr _cmdLogMgr;

		private ConsoleCommandLogMgr _errLogMgr;

		private WorkingDirectoryMgr _wrkDirMgr;

		private readonly ConsoleExecutorDto _consoleExecutor;

		public CommandExecutionMgr(JobsGroupDto jobsGroup,
								ConsoleExecutorDto consoleCmdDto)
		{
			_commandJobs = jobsGroup;

			_consoleExecutor = consoleCmdDto;

			_cmdLogMgr = new ConsoleCommandLogMgr(consoleCmdDto.DefaultCommandOutputLogFilePathName,
													consoleCmdDto.CmdConsoleLogFileTimeStamp,
														string.Empty);

			_errLogMgr = new ConsoleCommandLogMgr(consoleCmdDto.DefaultCommandOutputLogFilePathName,
                                                        consoleCmdDto.CmdConsoleLogFileTimeStamp,
															consoleCmdDto.CmdConsoleLogFileErrorSuffix);
			
			_wrkDirMgr = new WorkingDirectoryMgr(_consoleExecutor.DefaultCommandExeDirectoryDto);

		}

		public bool ExecuteCommands()
		{
			bool result;
			int jobNo = 0;
			Console.WriteLine("CmdrX.exe: Starting Job Execution...");
			try
			{
				foreach (var job in _commandJobs.Jobs)
				{
					job.CommandStartTime = DateTime.Now;
					LogUtil.WriteLogJobStartUpMessage(job, _consoleExecutor);

					var exeCmd = new ExecuteConsoleCommand(job, _consoleExecutor, _cmdLogMgr, _errLogMgr, _wrkDirMgr);
					var exitCode = exeCmd.Execute();
					jobNo++;
					LogUtil.WriteLogJobEndMessage(job, _consoleExecutor);
					Console.WriteLine($"Completed Job No. {jobNo,4:###0} Exit Code: {job.CommandExitCode} Job Name: {job.CommandDisplayName}" );

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
						return false;
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