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
