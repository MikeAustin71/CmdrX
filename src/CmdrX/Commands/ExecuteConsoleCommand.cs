using System;
using System.Diagnostics;
using CmdrX.Constants;
using CmdrX.GlobalConstants;
using CmdrX.Helpers;
using CmdrX.Managers;
using CmdrX.Models;

namespace CmdrX.Commands
{
	public class ExecuteConsoleCommand
	{

		public ErrorLogger ErrorMgr = new
			ErrorLogger(1456000,
						"ExecuteConsoleCommand",
						AppConstants.LoggingStatus,
						AppConstants.LoggingMode);

		private readonly ConsoleCommandDto _executeCommand;

		private readonly ConsoleExecutorDto _consoleExecutor;

		private readonly ConsoleCommandLogMgr _logMgr;

		private readonly ConsoleCommandLogMgr _errLogMgr;

		private readonly WorkingDirectoryMgr _wrkDirectoryMgr;


		public ExecuteConsoleCommand(ConsoleCommandDto cmdDto,
										ConsoleExecutorDto consoleExecutor,
											ConsoleCommandLogMgr logMgr,
												ConsoleCommandLogMgr errLogMgr,
													WorkingDirectoryMgr wrkDirectoryMgr)
		{

			if (cmdDto == null)
			{
				var msg = "Console Command Dto is NULL!";
				var err = new FileOpsErrorMessageDto
				{
					DirectoryPath = string.Empty,
					ErrId = 1,
					ErrorMessage = msg,
					ErrSourceMethod = "Constructor",
					LoggerLevel = LogLevel.FATAL
				};

				ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
				ErrorMgr.WriteErrorMsg(err);

				throw new ArgumentException(msg);

			}


			_executeCommand = cmdDto;
			_consoleExecutor = consoleExecutor;
			_logMgr = logMgr;
			_errLogMgr = errLogMgr;
			_wrkDirectoryMgr = wrkDirectoryMgr;

		}

		//System.Diagnostics.Process process = new System.Diagnostics.Process();
		//System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
		//startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
		//startInfo.FileName = "cmd.exe";
		//startInfo.Arguments = _executeCommand.CommandLineExecutionSyntax;
		//process.StartInfo = startInfo;
		//process.Start();

		// http://www.codeproject.com/Articles/25983/How-to-Execute-a-Command-in-C

		public int Execute()
		{
			_logMgr.InitializeCmdConsoleLog(_executeCommand.CommandOutputLogFilePathBaseName);
			_errLogMgr.InitializeCmdConsoleLog(_executeCommand.CommandOutputLogFilePathBaseName);

			if (_executeCommand.NumberOfCommandElements == 0)
			{
				var msg = "Console Command Execution Syntax Is Empty! Command Display Name: " + _executeCommand.CommandDisplayName + " - Skipping this command.";
				var err = new FileOpsErrorMessageDto
				{
					DirectoryPath = string.Empty,
					ErrId = 21,
					ErrorMessage = msg,
					ErrSourceMethod = "Execute()",
					CommandName = _executeCommand.CommandDisplayName,
					LoggerLevel = LogLevel.WARN
				};

				ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
				ErrorMgr.WriteErrorMsg(err);

				return 0;
			}

			_wrkDirectoryMgr.SetTargetDirectory(_executeCommand.ExecuteInDir);
			
			return ExecuteCommand(_executeCommand);
		}

		private int ExecuteCommand(ConsoleCommandDto cmdDto)
		{
			var thisMethod = "ExecuteCommand()";
			bool procStatus = false;
			var proc = new Process();

			// ReSharper disable once RedundantAssignment
			var exitCode = -1;

			try
			{
				// No window will be displayed to the user
				proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

				// Commands are set on the following two lines
				proc.StartInfo.FileName = _executeCommand.ProcFileNameCommand;
				proc.StartInfo.Arguments =  _executeCommand.ProcFileArguments;

				// Shell Execute must be false in order to 
				// set the Working directory below
				proc.StartInfo.UseShellExecute = false;
				
				// Do not create the black window.
				proc.StartInfo.CreateNoWindow = true;

				/* startInfo.WorkingDirectory
				 When the UseShellExecute property is false, gets or sets the working directory
				 for the process to be started. When UseShellExecute is true, gets or sets the
				 directory that contains the process to be started. 
				*/
				proc.StartInfo.WorkingDirectory = _wrkDirectoryMgr.TargetWorkingDirectory.DirInfo.FullName;


				// The following commands are needed to redirect the standard output.
				// This means that it will be redirected to the Process.StandardOutput StreamReader.
				proc.StartInfo.RedirectStandardOutput = true;
				proc.OutputDataReceived += CmdOutputDataHandler;

				// The following commands are needed to redirect standard error output.
				proc.StartInfo.RedirectStandardError = true;
				proc.ErrorDataReceived += CmdErrorDataHandler;


				// Start Process
				proc.Start();


				// Start the asynchronous read of the standard output stream.
				proc.BeginOutputReadLine();

				// Start the asynchronous read of the standard
				// error stream.
				proc.BeginErrorReadLine();

				procStatus = proc.WaitForExit(_executeCommand.CommandTimeOutInMiliseconds);

				exitCode = proc.ExitCode;

				_executeCommand.CommandExitTime = DateTime.Now;

			}
			catch (Exception ex)
			{
				var msg = "Console Command Execution Failed!";
				var err = new FileOpsErrorMessageDto
				{
					DirectoryPath = string.Empty,
					ErrId = 10,
					ErrorMessage = msg,
					ErrSourceMethod = thisMethod,
					ErrException = ex,
					CommandName = cmdDto.CommandDisplayName,
					LoggerLevel = LogLevel.FATAL
				};

				ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
				ErrorMgr.WriteErrorMsg(err);

				throw new ArgumentException(msg);

			}
			finally
			{
				try
				{
					if (!procStatus)
					{
						proc.Kill();
					}
					
				}
				catch
				{
					;
				}
				
				proc.Close();
				proc.Dispose();
				// ReSharper disable once RedundantAssignment
				proc = null;

			}

			return exitCode;
		}

		private void CmdOutputDataHandler(object sendingProcess,
			DataReceivedEventArgs outLine)
		{
				// Write the text to the collected output.
				_logMgr.LogWriteLine(outLine.Data);
		}


		private void CmdErrorDataHandler(object sendingProcess,
			DataReceivedEventArgs errLine)
		{
			_errLogMgr.LogWriteLine(errLine.Data);
			
		}

	}
}