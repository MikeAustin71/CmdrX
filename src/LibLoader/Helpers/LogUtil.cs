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
        public const string CrRet = "\n";

        public static bool IsFirstJobGroupLogMsg = true;

        public static bool IsFirstJobLogMsg;
        public static int ExpectedJobCount =1;

        public static DateTime JobGroupStartTime = DateTime.Now;
        public static DateTime JobStartTime = DateTime.Now;
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
            XmlConfigurator.Configure();
        }

        public static void WriteLogJobGroupStartUpMessage()
        {
            if (!IsFirstJobGroupLogMsg || ExpectedJobCount < 2)
            {
                return;
            }

            IsFirstJobGroupLogMsg = false;

            var banner = StringHelper.MakeSingleCharString('#', MaxBannerLen);

            var now = JobGroupStartTime;
            var sb = new StringBuilder();
            sb.Append("\n");
            sb.Append(banner);
            sb.Append("\n");

            var s = "NeCollegeImageTrans Assembly Version " + ExeAssemblyVersionNo;
            var cStr = StringHelper.CenterString(s, banner.Length);
            sb.Append(cStr);
            sb.Append("\n");


            sb.Append(banner);
            sb.Append("\n");
            if(!string.IsNullOrEmpty(JobGroupName))
            {
                sb.Append(StringHelper.CenterString(JobGroupName, banner.Length));
                sb.Append("\n");
            }

            if(ExpectedJobCount == 1)
            {
                s = "Starting Execution of One Job!";
            }
            else
            {
                s = "Starting Execution of " + ExpectedJobCount + " Jobs!";
            }

            cStr = StringHelper.CenterString(s, banner.Length);
            sb.Append(cStr);
            sb.Append("\n");

            s= $"Started Multiple Job Run: {now.ToLongDateString()} {now.ToLongTimeString()}\n";

            cStr = StringHelper.CenterString(s, banner.Length);
            sb.Append(cStr);
            sb.Append("\n");

            sb.Append(banner);
            sb.Append("\n");
            sb.Append(banner);
            sb.Append("\n");

            Logger.Info(sb.ToString());


        }



        public static void WriteLog(FileOpsErrorMessageDto err)
        {

            var sb = new StringBuilder();
            int errId;

            if(err.LoggerLevel == LogLevel.FATAL 
                ||err.LoggerLevel == LogLevel.ERROR)
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

            if(err.ErrException != null)
            {
                sb.Append($"-- Exception: {err.ErrException.Message} \n");

                if(err.ErrException.InnerException!=null)
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

            if (IsFirstJobLogMsg)
            {
                WriteLogJobStartUpMessage();
                WriteLogCmdArgsMessage();
            }

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

        public static void WriteLogJobStartUpMessage()
        {
            WriteLogJobGroupStartUpMessage();

            if (CmdLineArguments == null)
            {
                CmdLineArguments = new Dictionary<string, string>();
            }

            var jobName = string.Empty;

            if (CmdLineArguments.Count > 0)
            {
                if (!CmdLineArguments.TryGetValue("Job Name", out jobName))
                {
                    jobName = "Copy Job";
                }
                else
                {
                    CmdLineArguments.Remove("Job Name");
                }
            }

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

            var now = JobStartTime;
            var sb = new StringBuilder();
            sb.Append("\n");
            sb.Append(banner);
            sb.Append("\n");

            var s = "LibLoader Assembly Version " + ExeAssemblyVersionNo;
            var cStr = StringHelper.CenterString(s, banner.Length);
            sb.Append(cStr);
            sb.Append("\n");


            cStr = StringHelper.CenterString(CurrentJobNo, banner.Length);
            sb.Append(cStr);
            sb.Append("\n");

            if(!string.IsNullOrWhiteSpace(CurrentJobName))
            {
                cStr = StringHelper.CenterString(CurrentJobName, banner.Length);
                sb.Append(cStr);
                sb.Append("\n");
                
            }

            sb.Append($"Started Run: {now.ToLongDateString()} {now.ToLongTimeString()}\n");

            sb.Append(banner);
            sb.Append("\n");
            WriteLog(LogLevel.INFO, sb.ToString());
        }

        public static void WriteLogCmdArgsMessage()
        {
            var banner = StringHelper.MakeSingleCharString('-', MediumBannerLen);

            var sb = new StringBuilder();
            sb.Append("\n");
            sb.Append("\n");
            sb.Append(banner);
            sb.Append("\n");
            var s = "Command Line Arguments " + CurrentJobNo;
            var cStr = StringHelper.CenterString(s, banner.Length);
            sb.Append(cStr);
            sb.Append("\n");

            if(!string.IsNullOrEmpty(CurrentJobName))
            {
                cStr = StringHelper.CenterString(CurrentJobName, banner.Length);
                sb.Append(cStr);
                sb.Append("\n");
            }

            sb.Append(banner);
            sb.Append("\n");
            
            
            foreach (var cmdLineArgument in CmdLineArguments)
            {
                sb.Append("-" + cmdLineArgument.Key + " " + cmdLineArgument.Value);
                sb.Append("\n");
            }
            
            sb.Append("\n");
            sb.Append(banner);
            sb.Append("\n");
            sb.Append("\n");
            WriteLog(LogLevel.INFO, sb.ToString());


        }
 
    }
}