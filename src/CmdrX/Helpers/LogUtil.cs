using System;
using System.Collections.Generic;
using System.Text;
using CmdrX.Managers;
using CmdrX.Models;

namespace CmdrX.Helpers
{
    public static class LogUtil
    {
	    public static bool IsLoggerConfigured { get; private set; }
        public static string JobGroupName = string.Empty;
        public static string NewLine = Environment.NewLine;

        public static bool IsFirstJobLogMsg;
        public static int ExpectedJobCount =1;

        public static DateTime JobGroupStartTime = DateTime.Now;
		public static DateTime JobGroupEndTime = DateTime.Now;
	    public static TimeSpan JobGroupElapsedTime;
	    public static int JobGroupMessageCnt;
        public static bool IsAnyLoggingActive;

        public static Dictionary<string, string> CmdLineArguments;

        public static string ExeAssemblyVersionNo;

        public const int MaxBannerLen = 79;

        public const int MediumBannerLen = 59;

        private static ApplicationLogger _logger;

        public static int JobNumber = 1;

        public static string CurrentJobNo;
        public static string CurrentJobName = string.Empty;

        static LogUtil()
        {
	        IsLoggerConfigured = false;
        }

	    public static void Dispose()
	    {
		    if (_logger != null)
		    {
			    IsLoggerConfigured = false;
				_logger.Dispose();
			    _logger = null;
		    }
	    }
	    public static void ConfigureLogger(ApplicationLogger logger)
	    {
		    IsLoggerConfigured = true;
		    _logger = logger;
	    }

        public static void WriteLogJobGroupStartUpMessage(JobsGroupDto jobsGroup)
        {
	        if (!IsLoggerConfigured)
	        {
		        return;
	        }

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
            var sb = new StringBuilder();
            sb.Append(NewLine);
            sb.Append(banner + NewLine);
            sb.Append(banner + NewLine);

            var s = "CmdrX.exe Assembly Version " + ExeAssemblyVersionNo;
            var cStr = StringHelper.CenterString(s, banner.Length);
            sb.Append(cStr + NewLine);


            sb.Append(banner + NewLine);
                sb.Append(StringHelper.CenterString(JobGroupName, banner.Length));
                sb.Append(NewLine);

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
            sb.Append(NewLine);

            s= $"Started Job Run: " + DateHelper.DateTimeToDayMilliseconds(JobGroupStartTime);

            cStr = StringHelper.CenterString(s, banner.Length);
            sb.Append(cStr + NewLine);

            sb.Append(banner + NewLine);
            sb.Append(banner + NewLine);
            sb.Append(NewLine);

			WriteLog(LogLevel.MESSAGE, sb.ToString());
			_logger.LogFlushStreamWriter();

		}

		public static void WriteLogJobGroupCompletionMessage(JobsGroupDto jobsGroup)
	    {
			if (!IsLoggerConfigured)
			{
				return;
			}

			var banner = StringHelper.MakeSingleCharString('#', MaxBannerLen);
			var subbanner = StringHelper.MakeSingleCharString('*', MaxBannerLen);
			JobGroupEndTime = DateTime.Now;
		    JobGroupElapsedTime = JobGroupEndTime.Subtract(JobGroupStartTime);
			var sb = new StringBuilder();

			sb.Append(NewLine);
			sb.Append(NewLine);
			sb.Append(banner + NewLine);
			sb.Append(banner + NewLine);

			var s = "CmdrX.exe Assembly Version " + ExeAssemblyVersionNo;
			var cStr = StringHelper.CenterString(s, banner.Length);
			sb.Append(cStr + NewLine);

			sb.Append(banner + NewLine);
			sb.Append(StringHelper.CenterString(JobGroupName, banner.Length));
			sb.Append(NewLine);
			sb.Append(banner + NewLine);

		    JobNumber--;
		    
			s = $"Completed Execution of {JobNumber} Jobs.";
			sb.Append(s + NewLine);
		    s = $"Number of messages logged: {JobGroupMessageCnt} ";
			sb.Append(s + NewLine);
			sb.Append(subbanner + NewLine);
			sb.Append("JobGroup   Start Time: " + DateHelper.DateTimeToMillisconds(JobGroupStartTime) + NewLine);
			sb.Append("JobGroup     End Time: " + DateHelper.DateTimeToMillisconds(JobGroupEndTime) + NewLine);
			sb.Append("JobGroup Elapsed Time: " + DateHelper.TimeSpanDetailToMiliseconds(JobGroupElapsedTime) + NewLine);
			sb.Append(subbanner + NewLine);

			WriteLog(LogLevel.MESSAGE, sb.ToString());
			_logger.LogFlushStreamWriter();
		}


		public static void WriteLogJobStartUpMessage(ConsoleCommandDto job, ConsoleExecutorDto consoleExecutor)
        {

			if (!IsLoggerConfigured)
			{
				return;
			}

			var jobName = job.CommandDisplayName;

			JobNumber = job.JobNumber;

			CurrentJobNo = "Starting Job No: " + JobNumber;

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
            sb.Append(NewLine);
            sb.Append(banner + NewLine);
            sb.Append(banner + NewLine);

            var s = "CmdrX Assembly Version " + ExeAssemblyVersionNo;
            var cStr = StringHelper.CenterString(s, banner.Length);
            sb.Append(cStr);
            sb.Append(NewLine);


            cStr = StringHelper.CenterString(CurrentJobNo, banner.Length);
            sb.Append(cStr);
            sb.Append(NewLine);

			sb.Append(subbanner);
            sb.Append(NewLine);
            if(!string.IsNullOrWhiteSpace(CurrentJobName))
            {
                cStr = StringHelper.CenterString(CurrentJobName, banner.Length);
                sb.Append(cStr);
                sb.Append(NewLine);
                
            }

            sb.Append($"Started Job: {now.ToLongDateString()} {now.ToLongTimeString()}" + NewLine);
			sb.Append(subbanner);
			sb.Append(NewLine);

	        sb.Append("Process StartInfo FileName: " + job.ProcFileNameCommand);
			sb.Append(NewLine);
			var args = StringHelper.BreakLineAtIndex(StringHelper.RemoveCarriageReturns(job.ProcFileArguments), 65);
	        sb.Append("Process StartInfo Arguments: ");
			if (args.Length > 0)
			{
				sb.Append(NewLine);

				for (int i = 0; i < args.Length; i++)
				{
					sb.Append("     " + args[i] + NewLine);
				}
			}
			else
			{
				sb.Append("<NO ARGUMENTS>" + NewLine);
			}

			sb.Append(subbanner + NewLine);

            WriteLog(LogLevel.MESSAGE, sb.ToString());
        }

	    public static void WriteLogJobEndMessage(ConsoleCommandDto job, ConsoleExecutorDto consoleExecutor)
	    {
			if (!IsLoggerConfigured)
			{
				return;
			}

			var banner = StringHelper.MakeSingleCharString('=', MaxBannerLen);
			var subbanner = StringHelper.MakeSingleCharString('-', MaxBannerLen);
		    var startTime = job.CommandStartTime;
		    var endTime = job.CommandExitTime;
		    var ts = job.CommandElapsedTime;
			var sb = new StringBuilder();
			sb.Append(NewLine);
			sb.Append(NewLine);
			sb.Append(subbanner + NewLine);
			var cStr = StringHelper.CenterString("Job Completed: " + job.CommandDisplayName, banner.Length);
			sb.Append(cStr);
			sb.Append(NewLine);

			sb.Append(subbanner + NewLine);
			sb.Append("Job       Number: " + job.JobNumber + NewLine);
			sb.Append("Job   Start Time: " + DateHelper.DateTimeToDayMilliseconds(startTime) + NewLine);
			sb.Append("Job     End Time: " + DateHelper.DateTimeToDayMilliseconds(endTime) + NewLine);
		    sb.Append("Job Elapsed Time: " + DateHelper.TimeSpanDetailToMiliseconds(ts) + NewLine);
		    sb.Append("Job    Exit Code: " + job.CommandExitCode + NewLine);

			sb.Append(banner);
			sb.Append(NewLine);
			sb.Append(banner);
			sb.Append(NewLine);

			WriteLog(LogLevel.MESSAGE, sb.ToString());
			_logger.LogFlushStreamWriter();
		}

		public static void WriteLog(FileOpsErrorMessageDto err)
		{
			if (!IsLoggerConfigured)
			{
				return;
			}

			JobGroupMessageCnt++;

			var sb = new StringBuilder();

			if (err.LoggerLevel == LogLevel.FATAL
				|| err.LoggerLevel == LogLevel.ERROR)
			{
				err.ErrId = (err.ErrId * -1);
			}

			sb.Append($"Msg Id: {err.ErrId}  MsgType: {err.LoggerLevel}  ");
			if (!string.IsNullOrWhiteSpace(err.JobName))
			{
				sb.Append("Job Name: " + err.JobName);
			}

			sb.Append(NewLine);

			sb.Append($"Class: {err.ErrSourceClass}  Method: {err.ErrSourceMethod}" + NewLine);

			if (!string.IsNullOrWhiteSpace(err.ErrorMessage))
			{
				sb = StringHelper.AddBreakLinesAtIndex("Error Message: " + err.ErrorMessage, MaxBannerLen - 1, sb, true);
			}
			


			if (err.ErrException != null)
			{
				sb = StringHelper.AddBreakLinesAtIndex("Exception: " + err.ErrException.Message, MaxBannerLen - 1, sb, true);

				if (err.ErrException.InnerException != null)
				{
					sb = StringHelper.AddBreakLinesAtIndex("Inner Exception: " + err.ErrException.InnerException.Message, MaxBannerLen - 1, sb, true);
				}

			}

			if (!string.IsNullOrWhiteSpace(err.DirectoryPath))
			{
				sb.Append($"-- Directory: {err.DirectoryPath}" + NewLine);
			}

			if (!string.IsNullOrWhiteSpace(err.DirectoryPath2))
			{
				sb.Append($"-- Directory2: {err.DirectoryPath2}" + NewLine);
			}

			if (!string.IsNullOrWhiteSpace(err.FileName))
			{
				sb.Append($"-- File Name: {err.FileName}" + NewLine);
			}

			if (!string.IsNullOrWhiteSpace(err.CommandName))
			{
				sb.Append($"-- Command Name: {err.FileName}" + NewLine);
			}


			WriteLog(err.LoggerLevel, sb.ToString());

		}



		public static void WriteLog(LogLevel logLevel, string log)
		{
			if (!IsLoggerConfigured)
			{
				return;
			}


			if (logLevel.Equals(LogLevel.DEBUG))
			{
				_logger.LogWriteLine("DEBUG- " + log);
			}
			else if (logLevel.Equals(LogLevel.ERROR))
			{
				_logger.LogWriteLine("ERROR- " + log);
			}
			else if (logLevel.Equals(LogLevel.FATAL))
			{
				_logger.LogWriteLine("FATAL- " + log);

			}
			else if (logLevel.Equals(LogLevel.INFO))
			{
				_logger.LogWriteLine("INFO- " + log);
			}
			else if (logLevel.Equals(LogLevel.WARN))
			{
				_logger.LogWriteLine("WARN- " + log);
			}
			else if (logLevel.Equals(LogLevel.MESSAGE))
			{
				_logger.LogWriteLine(log);
			}

		}

	}
}