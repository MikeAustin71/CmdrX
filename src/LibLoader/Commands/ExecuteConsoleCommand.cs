using System;
using System.IO;
using LibLoader.Constants;
using LibLoader.GlobalConstants;
using LibLoader.Helpers;
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

		public ConsoleCommandDto ExecuteCommand { get; private set; }


		public ExecuteConsoleCommand(ConsoleCommandDto cmdDto)
		{

			if (string.IsNullOrWhiteSpace(cmdDto?.CommandLineExecutionSyntax))
			{
				var msg = "Console Command Execution Syntax Is Empty! Command Display Name: " + ExecuteCommand.CommandDisplayName;
				var err = new FileOpsErrorMessageDto
				{
					DirectoryPath = string.Empty,
					ErrId = 1,
					ErrorMessage = msg,
					ErrSourceMethod = "Constructor",
					CommandName = ExecuteCommand.CommandDisplayName,
					LoggerLevel = LogLevel.FATAL
				};

				ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
				ErrorMgr.WriteErrorMsg(err);

				throw new ArgumentException(msg);

			}

			ExecuteCommand = cmdDto;

		}

		public int Execute()
		{
			//System.Diagnostics.Process process = new System.Diagnostics.Process();
			//System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
			//startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
			//startInfo.FileName = "cmd.exe";
			//startInfo.Arguments = ExecuteCommand.CommandLineExecutionSyntax;
			//process.StartInfo = startInfo;
			//process.Start();

			// http://www.codeproject.com/Articles/25983/How-to-Execute-a-Command-in-C

			return ExecuteCommandSync(ExecuteCommand);

		}

		private int MikeExecuteCommandSync(ConsoleCommandDto cmdDto)
		{
			var thisMethod = "MikeExecuteCommandSync()";
            var originalWorkingDirectory = DirectoryHelper.GetCurrentEnvironmentDirectory();
			var changedCurrentWorkingDirectory = false;
			System.Diagnostics.Process proc = new System.Diagnostics.Process();
			StreamReader outputReader;
			//StreamWriter cmdLogFile = new StreamWriter();

			try
			{
				System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
				startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
				startInfo.FileName = "cmd.exe";
				startInfo.Arguments = "/c " + ExecuteCommand.CommandLineExecutionSyntax;
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
				
                if (!string.IsNullOrWhiteSpace(cmdDto.ExecuteInDir))
                {
	                changedCurrentWorkingDirectory = true;
					Directory.SetCurrentDirectory(cmdDto.ExecuteInDir.TrimStart().TrimEnd());
				}

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

				if (changedCurrentWorkingDirectory)
				{
					Directory.SetCurrentDirectory(originalWorkingDirectory.DirInfo.FullName);
				}


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
				var msg = "Exception Thrown during Command Execution. Command Name: " + ExecuteCommand.CommandDisplayName;
				var err = new FileOpsErrorMessageDto
				{
					DirectoryPath = string.Empty,
					ErrId = 3,
					ErrorMessage = msg,
					ErrSourceMethod = "Constructor",
					CommandName = ExecuteCommand.CommandDisplayName,
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