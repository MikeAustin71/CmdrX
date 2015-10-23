using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using log4net.Config;
using LibLoader.Models;

namespace LibLoader.Helpers
{
    public static class LogUtil
    {
        public static string JobGroupName = string.Empty;
        public static string CrRet = Environment.NewLine;

        public static bool IsFirstJobLogMsg;
        public static int ExpectedJobCount =1;

        public static DateTime JobGroupStartTime = DateTime.Now;
		public static DateTime JobGroupEndTime = DateTime.Now;
	    public static TimeSpan JobGroupElapsedTime;
	    public static int JobGroupMessageCnt = 0;
        public static bool IsAnyLoggingActive;

        public static Dictionary<string, string> CmdLineArguments;

        public static string ExeAssemblyVersionNo;

        public const int MaxBannerLen = 79;

        public const int MediumBannerLen = 59;

        private static readonly ILog Logger = LogManager.GetLogger(typeof(LogUtil));

        public static int JobNumber = 1;

        public static string CurrentJobNo;
        public static string CurrentJobName = string.Empty;

        static LogUtil()
        {
            //XmlConfigurator.Configure();
        }
	
        public static void WriteLogJobGroupStartUpMessage(JobsGroupDto jobsGroup)
        {
            var banner = StringHelper.MakeSingleCharString('#', MaxBannerLen);
	        if (!string.IsNullOrWhiteSpace(jobsGroup.JobGroupName))
	        {
		        JobGroupName = "Execution Job Group: " + jobsGroup.JobGroupName;
	        }
	        else
	        {
		        JobGroupName = "Execution Job Group: Job Group # 1";
	        }

			JobGroupStartTime = DateTime.Now;
	        ExpectedJobCount = jobsGroup.Jobs.Count;
			var now = JobGroupStartTime;
            var sb = new StringBuilder();
            sb.Append(CrRet);
            sb.Append(banner + CrRet);
            sb.Append(banner + CrRet);

            var s = "LibLoader.exe Assembly Version " + ExeAssemblyVersionNo;
            var cStr = StringHelper.CenterString(s, banner.Length);
            sb.Append(cStr + CrRet);


            sb.Append(banner + CrRet);
                sb.Append(StringHelper.CenterString(JobGroupName, banner.Length));
                sb.Append(CrRet);

            if(ExpectedJobCount == 1)
            {
                s = "Starting Execution of One Job.";
            }
            else
            {
                s = $"Starting Execution of {ExpectedJobCount} Jobs.";
            }

            cStr = StringHelper.CenterString(s, banner.Length);
            sb.Append(cStr);
            sb.Append(CrRet);

            s= $"Started Job Run: {now.ToLongDateString()} {now.ToLongTimeString()}" + CrRet;

            cStr = StringHelper.CenterString(s, banner.Length);
            sb.Append(cStr + CrRet);

            sb.Append(banner + CrRet);
            sb.Append(banner + CrRet);
            sb.Append(CrRet);

            Logger.Info(sb.ToString());
        }

	    public static void WriteLogJobGroupCompletionMessage(JobsGroupDto jobsGroup)
	    {
			var banner = StringHelper.MakeSingleCharString('#', MaxBannerLen);
			var subbanner = StringHelper.MakeSingleCharString('*', MaxBannerLen);
			JobGroupEndTime = DateTime.Now;
		    JobGroupElapsedTime = JobGroupEndTime - JobGroupStartTime;
			var sb = new StringBuilder();
			sb.Append(CrRet);
			sb.Append(banner + CrRet);
			sb.Append(banner + CrRet);

			var s = "LibLoader.exe Assembly Version " + ExeAssemblyVersionNo;
			var cStr = StringHelper.CenterString(s, banner.Length);
			sb.Append(cStr + CrRet);

			sb.Append(banner + CrRet);
			sb.Append(StringHelper.CenterString(JobGroupName, banner.Length));
			sb.Append(CrRet);
			sb.Append(banner + CrRet);

		    JobNumber--;
		    var ts = JobGroupElapsedTime;
			s = $"Completed Execution of {JobNumber} Jobs.";
			sb.Append(s + CrRet);
		    s = $"Number of messages logged: {JobGroupMessageCnt} ";
			sb.Append(s + CrRet);
			sb.Append(subbanner + CrRet);
			sb.Append($"JobGroup   Start Time: {JobGroupStartTime.ToLongDateString()} {JobGroupStartTime.ToLongTimeString()}" + CrRet);
			sb.Append($"JobGroup     End Time: {JobGroupEndTime.ToLongDateString()} {JobGroupEndTime.ToLongTimeString()}" + CrRet);
			sb.Append($"JobGroup Elapsed Time: {ts.Hours:00}:{JobGroupElapsedTime.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds:00}" + CrRet);
			sb.Append(subbanner + CrRet);

		}


		public static void WriteLogJobStartUpMessage(ConsoleCommandDto job, ConsoleExecutorDto consoleExecutor)
        {

	        var jobName = job.CommandDisplayName;

			CurrentJobNo = "Job No: " + JobNumber++;

            if(!string.IsNullOrEmpty(jobName))
            {
                CurrentJobName = "Job Name: " + jobName;
            }
            else
            {
                CurrentJobName = string.Empty;    
            }
            

            IsFirstJobLogMsg = false;
            IsAnyLoggingActive = true;
            
            var banner = StringHelper.MakeSingleCharString('=', MaxBannerLen);
			var subbanner = StringHelper.MakeSingleCharString('-', MaxBannerLen);

            var now = job.CommandStartTime;
            var sb = new StringBuilder();
            sb.Append(CrRet);
            sb.Append(banner + CrRet);
            sb.Append(banner + CrRet);

            var s = "LibLoader Assembly Version " + ExeAssemblyVersionNo;
            var cStr = StringHelper.CenterString(s, banner.Length);
            sb.Append(cStr);
            sb.Append(CrRet);


            cStr = StringHelper.CenterString(CurrentJobNo, banner.Length);
            sb.Append(cStr);
            sb.Append(CrRet);

			sb.Append(subbanner);
            sb.Append(CrRet);
            if(!string.IsNullOrWhiteSpace(CurrentJobName))
            {
                cStr = StringHelper.CenterString(CurrentJobName, banner.Length);
                sb.Append(cStr);
                sb.Append(CrRet);
                
            }

            sb.Append($"Started Job: {now.ToLongDateString()} {now.ToLongTimeString()}\n");
			sb.Append(subbanner);
			sb.Append(CrRet);

	        sb.Append("Process StartInfo FileName: " + job.ProcFileNameCommand);
			sb.Append(CrRet);

	        sb.Append("Process StartInfo Arguments: " + job.ProcFileArguments);
			sb.Append(CrRet);
			sb.Append(CrRet);

			sb.Append(banner);
            sb.Append(CrRet);

            WriteLog(LogLevel.INFO, sb.ToString());
        }

	    public static void WriteLogJobEndMessage(ConsoleCommandDto job, ConsoleExecutorDto consoleExecutor)
	    {
			var banner = StringHelper.MakeSingleCharString('=', MaxBannerLen);
			var subbanner = StringHelper.MakeSingleCharString('-', MaxBannerLen);
		    var startTime = job.CommandStartTime;
		    var endTime = job.CommandExitTime;
		    TimeSpan ts = endTime - startTime;
			var sb = new StringBuilder();
			sb.Append(banner);
			sb.Append(CrRet);
			var cStr = StringHelper.CenterString("Job Completed: " + job.CommandDisplayName, banner.Length);
			sb.Append(cStr);
			sb.Append(CrRet);

			sb.Append(subbanner + CrRet);
			sb.Append($"Start Time: {startTime.ToLongDateString()} {startTime.ToLongTimeString()}" + CrRet);
			sb.Append($"End Time: {endTime.ToLongDateString()} {endTime.ToLongTimeString()}" + CrRet);
		    sb.Append($"Elapsed Time: {ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds:00}" + CrRet);
		    sb.Append("Job Exit Code: " + job.CommandExitCode);

			sb.Append(banner);
			sb.Append(CrRet);
			sb.Append(banner);
			sb.Append(CrRet);
		}


		public static void WriteLogCmdArgsMessage()
        {
            var banner = StringHelper.MakeSingleCharString('-', MediumBannerLen);

            var sb = new StringBuilder();
            sb.Append(CrRet);
            sb.Append(CrRet);
            sb.Append(banner);
            sb.Append(CrRet);
            var s = "Command Line Arguments " + CurrentJobNo;
            var cStr = StringHelper.CenterString(s, banner.Length);
            sb.Append(cStr);
            sb.Append(CrRet);

            if(!string.IsNullOrEmpty(CurrentJobName))
            {
                cStr = StringHelper.CenterString(CurrentJobName, banner.Length);
                sb.Append(cStr);
                sb.Append(CrRet);
            }

            sb.Append(banner);
            sb.Append(CrRet);
            
            
            foreach (var cmdLineArgument in CmdLineArguments)
            {
                sb.Append("-" + cmdLineArgument.Key + " " + cmdLineArgument.Value);
                sb.Append(CrRet);
            }
            
            sb.Append(CrRet);
            sb.Append(banner);
            sb.Append(CrRet);
            sb.Append(CrRet);
            WriteLog(LogLevel.INFO, sb.ToString());

        }

		public static void WriteLog(FileOpsErrorMessageDto err)
		{
			JobGroupMessageCnt++;

			var sb = new StringBuilder();
			int errId;

			if (err.LoggerLevel == LogLevel.FATAL
				|| err.LoggerLevel == LogLevel.ERROR)
			{
				errId = (err.ErrId * -1);

				sb.Append($"Class: {err.ErrSourceClass}  Method: {err.ErrSourceMethod} Error Id:{errId} \n");

			}
			else
			{

				errId = err.ErrId;

				sb.Append($"Class: {err.ErrSourceClass}  Method: {err.ErrSourceMethod}   Msg Id:{errId} \n");

			}



			sb.Append($"-- Message: {err.ErrorMessage} \n");

			if (err.ErrException != null)
			{
				sb.Append($"-- Exception: {err.ErrException.Message} \n");

				if (err.ErrException.InnerException != null)
				{
					sb.Append($"-- Inner Exception: {err.ErrException.InnerException.Message} \n");
				}

			}

			if (!string.IsNullOrWhiteSpace(err.DirectoryPath))
			{
				sb.Append($"-- Directory: {err.DirectoryPath} \n");
			}

			if (!string.IsNullOrWhiteSpace(err.DirectoryPath2))
			{
				sb.Append($"-- Directory2: {err.DirectoryPath2} \n");
			}

			if (!string.IsNullOrWhiteSpace(err.FileName))
			{
				sb.Append($"-- File Name: {err.FileName} \n");
			}

			if (!string.IsNullOrWhiteSpace(err.CommandName))
			{
				sb.Append($"-- Command Name: {err.FileName} \n");
			}


			WriteLog(err.LoggerLevel, sb.ToString());

		}



		public static void WriteLog(LogLevel logLevel, string log)
		{

			if (logLevel.Equals(LogLevel.DEBUG))
			{
				Logger.Debug(log);
			}
			else if (logLevel.Equals(LogLevel.ERROR))
			{
				Logger.Error(log);
			}
			else if (logLevel.Equals(LogLevel.FATAL))
			{
				Logger.Fatal(log);
			}
			else if (logLevel.Equals(LogLevel.INFO))
			{
				Logger.Info(log);
			}
			else if (logLevel.Equals(LogLevel.WARN))
			{
				Logger.Warn(log);
			}

		}

	}
}