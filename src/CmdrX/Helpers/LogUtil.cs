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
			var now = JobGroupStartTime;
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

            s= $"Started Job Run: {now.ToLongDateString()} {now.ToLongTimeString()}";

            cStr = StringHelper.CenterString(s, banner.Length);
            sb.Append(cStr + NewLine);

            sb.Append(banner + NewLine);
            sb.Append(banner + NewLine);
            sb.Append(NewLine);

			WriteLog(LogLevel.MESSAGE, sb.ToString());

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

            sb.Append($"Started Job: {now.ToLongDateString()} {now.ToLongTimeString()}\n");
			sb.Append(subbanner);
			sb.Append(NewLine);

	        sb.Append("Process StartInfo FileName: " + job.ProcFileNameCommand);
			sb.Append(NewLine);
			var args = StringHelper.BreakLineAtIndex(StringHelper.RemoveCarriageReturns(job.ProcFileArguments), 70);
	        sb.Append("Process StartInfo Arguments: " );
			if (args == null || args.Length == 0)
			{
				sb.Append("<NO ARGUMENTS>" + NewLine);
			}
			else if (args.Length == 1)
			{
				sb.Append(args[0] + NewLine);
			}
			else
			{
				sb.Append(args[0] + NewLine);
				for (int i = 1; i < args.Length; i++)
				{
					sb.Append("        " + args[i] + NewLine);
				}
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
		    TimeSpan ts = endTime.Subtract(startTime);
			var sb = new StringBuilder();
			sb.Append(NewLine);
			sb.Append(NewLine);
			sb.Append(subbanner + NewLine);
			var cStr = StringHelper.CenterString("Job Completed: " + job.CommandDisplayName, banner.Length);
			sb.Append(cStr);
			sb.Append(NewLine);

			sb.Append(subbanner + NewLine);
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
			int errId;

			if (err.LoggerLevel == LogLevel.FATAL
				|| err.LoggerLevel == LogLevel.ERROR)
			{
				errId = (err.ErrId * -1);

				sb.Append($"Class: {err.ErrSourceClass}  Method: {err.ErrSourceMethod} Error Id:{errId} MsgType: {err.LoggerLevel} \n");

			}
			else
			{

				errId = err.ErrId;

				sb.Append($"Class: {err.ErrSourceClass}  Method: {err.ErrSourceMethod}   Msg Id:{errId} MsgType: {err.LoggerLevel} \n");

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