using System;
using System.IO;
using LibLoader.Constants;
using LibLoader.GlobalConstants;
using LibLoader.Helpers;
using LibLoader.Managers;
using LibLoader.Models;

namespace LibLoader.Commands
{
	public class ExecuteConsoleCommand
	{

		public static ErrorLogger ErrorMgr = new
			ErrorLogger(1456000,
						"ExecuteConsoleCommand",
						AppConstants.LoggingStatus,
						AppConstants.LoggingMode);

		private readonly ConsoleCommandDto _executeCommand;

		private readonly ConsoleCommandLogMgr _logMgr;

		private readonly ConsoleCommandLogMgr _errLogMgr;

		private readonly WorkingDirectoryMgr _wrkDirectoryMgr;


		public ExecuteConsoleCommand(ConsoleCommandDto cmdDto, 
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

			if (cmdDto.AssembleCommandLineSyntax()==string.Empty )
			{
				var msg = "Console Command Execution Syntax Is Empty! Command Display Name: " + _executeCommand.CommandDisplayName;
				var err = new FileOpsErrorMessageDto
				{
					DirectoryPath = string.Empty,
					ErrId = 2,
					ErrorMessage = msg,
					ErrSourceMethod = "Constructor",
					CommandName = _executeCommand.CommandDisplayName,
					LoggerLevel = LogLevel.FATAL
				};

				ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
				ErrorMgr.WriteErrorMsg(err);

				throw new ArgumentException(msg);
			}

			_executeCommand = cmdDto;
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
			_logMgr.InitializeCmdConsoleLog(_executeCommand.OutputCmdLogFileBaseName);
			_errLogMgr.InitializeCmdConsoleLog(_executeCommand.OutputCmdLogFileBaseName
			                                   + AppConstants.ConsoleErrorLogFileNameSuffix);

			_wrkDirectoryMgr.SetTargetDirectory(_executeCommand.ExecuteInDir);

			_wrkDirectoryMgr.ChangeToTargetWorkingDirectory();

            var result = MikeExecuteCommandSync(_executeCommand);

			_wrkDirectoryMgr.ChangeBackToOriginalWorkingDirectory();

			return 0;
		}

		private int MikeExecuteCommandSync(ConsoleCommandDto cmdDto)
		{
			var thisMethod = "MikeExecuteCommandSync()";
			cmdDto.CommandStartTime = DateTime.Now;
			System.Diagnostics.Process proc = new System.Diagnostics.Process();
			StreamReader outputReader;

			try
			{
				System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
				startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
				startInfo.FileName = "cmd.exe";
				startInfo.Arguments = "/c " + _executeCommand.CommandLineExecutionSyntax;
				// The following commands are needed to redirect the standard output.
				// This means that it will be redirected to the Process.StandardOutput StreamReader.
				startInfo.RedirectStandardOutput = true;
				startInfo.UseShellExecute = false;
				// Do not create the black window.
				startInfo.CreateNoWindow = true;

				/* startInfo.WorkingDirectory
				 When the UseShellExecute property is false, gets or sets the working directory
				 for the process to be started.When UseShellExecute is true, gets or sets the
				 directory that contains the process to be started. 
				*/
				

				// Now we create a process, assign its ProcessStartInfo and start it
				
				proc.StartInfo = startInfo;
				proc.Start();

				outputReader = proc.StandardOutput;
				
				// To avoid deadlocks, always read the output stream first and then wait.
				string output = outputReader.ReadToEnd();

				proc.WaitForExit();
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

				proc.Close();
				proc.Dispose();



			}
			return -1;
		}

		private int ExecuteCommandSync(ConsoleCommandDto cmdDto)
		{
			try
			{
				// create the ProcessStartInfo using "cmd" as the program to be run,
				// and "/c " as the parameters.
				// Incidentally, /c tells cmd that we want it to execute the command that follows,
				// and then exit.
				System.Diagnostics.ProcessStartInfo procStartInfo =
					new System.Diagnostics.ProcessStartInfo("cmd", "/c " + cmdDto.CommandLineExecutionSyntax);

				// The following commands are needed to redirect the standard output.
				// This means that it will be redirected to the Process.StandardOutput StreamReader.
				procStartInfo.RedirectStandardOutput = true;
				procStartInfo.UseShellExecute = false;
				// Do not create the black window.
				procStartInfo.CreateNoWindow = true;
				// Now we create a process, assign its ProcessStartInfo and start it
				System.Diagnostics.Process proc = new System.Diagnostics.Process();
				proc.StartInfo = procStartInfo;
				proc.Start();
				// Get the output into a string
				string result = proc.StandardOutput.ReadToEnd();
				// Display the command output.

				return proc.ExitCode;
			}
			catch (Exception e)
			{
				var msg = "Exception Thrown during Command Execution. Command Name: " + _executeCommand.CommandDisplayName;
				var err = new FileOpsErrorMessageDto
				{
					DirectoryPath = string.Empty,
					ErrId = 3,
					ErrorMessage = msg,
					ErrSourceMethod = "Constructor",
					CommandName = _executeCommand.CommandDisplayName,
					ErrException = e,
					LoggerLevel = LogLevel.FATAL
				};

				ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
				ErrorMgr.WriteErrorMsg(err);

				throw new ArgumentException(msg);
			}
		}

	}
}