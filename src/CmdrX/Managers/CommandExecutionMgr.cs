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
					jobNo++;
					job.JobNumber = jobNo;
					job.CommandStartTime = DateTime.Now;
					LogUtil.WriteLogJobStartUpMessage(job, _consoleExecutor);
					_cmdLogMgr.LogWriteStartJobHeader(job);
					_errLogMgr.LogWriteStartJobHeader(job);
					var exeCmd = new ExecuteConsoleCommand(job, _cmdLogMgr, _errLogMgr, _wrkDirMgr);
					exeCmd.Execute();

					_cmdLogMgr.LogWriteEndJobFooter(job);
					_errLogMgr.LogWriteEndJobFooter(job);
					Console.WriteLine($"Completed Job No. {jobNo,4:###0} Exit Code: {job.CommandExitCode} Job Name: {job.CommandDisplayName}" );

					if (job.CommandExitCode > job.KillJobsRunOnExitCodeGreaterThan
						|| job.CommandExitCode < job.KillJobsRunOnExitCodeLessThan)
					{
						var msg = $"Job No. {job.JobNumber} Job Name: {job.CommandDisplayName}"
							+ Environment.NewLine
							+ $"Command Exit Code Is Out-Of-Bounds! Job Exit Code = {job.CommandExitCode}  " 
							+ Environment.NewLine
							+ $"Maximum Exit Code = {job.KillJobsRunOnExitCodeGreaterThan}  " 
							+ $"Mininimum Exit Code = {job.KillJobsRunOnExitCodeLessThan}  "
							+ Environment.NewLine
							+ "Terminating Job Group Command Execution!";

                        Environment.ExitCode = job.CommandExitCode;
						var err = new FileOpsErrorMessageDto
						{
							JobName = job.CommandDisplayName,
                            DirectoryPath = string.Empty,
							ErrId = 10,
							ErrorMessage = msg,
							ErrSourceMethod = "ExecuteCommands()",
							FileName = string.Empty,
							LoggerLevel = LogLevel.FATAL
						};

						_consoleExecutor.ApplicationExitStatus.OpsError =
							ErrorMgr.FormatErrorDto(err);
						_consoleExecutor.ApplicationExitStatus.IsFatalError = true;
						ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
						ErrorMgr.WriteErrorMsg(err);
						LogUtil.WriteLogJobEndMessage(job, _consoleExecutor);
						return false;
					}

					LogUtil.WriteLogJobEndMessage(job, _consoleExecutor);

					_wrkDirMgr.ChangeBackToOriginalWorkingDirectory();
				}

				Environment.ExitCode = 0;
				_consoleExecutor.ApplicationExitStatus.IsSuccessfulCompletion = true;
				result = true;
			}
			catch (Exception ex)
			{
				var err = new FileOpsErrorMessageDto
				{
					DirectoryPath = string.Empty,
					ErrId = 20,
					ErrorMessage = "Exception thrown while executing commands! ",
					ErrSourceMethod = "ExecuteCommands()",
					ErrException = ex,
					FileName = string.Empty,
					LoggerLevel = LogLevel.FATAL
				};

				ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
				ErrorMgr.WriteErrorMsg(err);
				_consoleExecutor.ApplicationExitStatus.IsExceptionThrown = true;
				_consoleExecutor.ApplicationExitStatus.OpsError = ErrorMgr.FormatErrorDto(err);

				if (Environment.ExitCode == 0)
				{
					Environment.ExitCode = -8;
				}

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