using System;
using System.IO;
using System.Net.Configuration;
using System.Reflection;
using LibLoader.Constants;
using LibLoader.GlobalConstants;
using LibLoader.Helpers;
using LibLoader.Models;

namespace LibLoader.Commands
{
    public class PurgeLogCommand
    {
		public ErrorLogger ErrorMgr = new ErrorLogger(120000, 
			"PurgeLogCommand",
			AppConstants.LoggingStatus,
			AppConstants.LoggingMode);

	    private readonly int _logRetentionInDays;

	    private readonly DirectoryDto _logFileDirectoryDto;

	    public PurgeLogCommand(int logRetentionInDays, DirectoryDto logFileDirectoryDto)
	    {
		    _logRetentionInDays = logRetentionInDays;
		    _logFileDirectoryDto = logFileDirectoryDto;
	    }

        public bool Execute()
        {
	        if (_logFileDirectoryDto?.DirInfo == null || ! _logFileDirectoryDto.DirInfo.Exists)
	        {
		        var msg = "Log File Directory Dto is INVALID!";

				var ex = new ArgumentException(msg);
				var err = new FileOpsErrorMessageDto
				{
					ErrId = 37,
					ErrorMessage = "Application Log Directory Dto Invalid!",
					ErrSourceMethod = "CreateApplicaitonLogDirectory()",
					ErrException = ex,
					DirectoryPath = string.Empty,
					LoggerLevel = LogLevel.FATAL
				};

				ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
				ErrorMgr.WriteErrorMsg(err);

				throw ex;


			}

			return PurgeOldLogFiles(_logFileDirectoryDto.DirInfo.FullName);
        }


        private bool GetValidLogFilesPath(out string logFilesPath)
        {
	        //var exeDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            //get the full location of the assembly with DaoTests in it
            //string fullPath = System.Reflection.Assembly.GetAssembly(typeof(PurgeLogCommand)).Location;


            //get the folder that's in
            //string exeDir = Path.GetDirectoryName(fullPath);

            var fullPath = (new Uri(Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath;

            var exeDir = Path.GetDirectoryName(fullPath);

            if (string.IsNullOrEmpty(exeDir) || !Directory.Exists(exeDir))
            {

                if(SetAlternativeLogPath(out logFilesPath))
                {
                    return true;
                }


                var err = new FileOpsErrorMessageDto
                {
                    DirectoryPath = string.Empty,
                    ErrId =  1,
                    ErrorMessage = "Could not locate executing assembly directory",
                    ErrSourceMethod = "PurgeOldLogs",
                    FileName = string.Empty,
                    LoggerLevel = LogLevel.FATAL
                };

				ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
				ErrorMgr.WriteErrorMsg(err);

				return false;

            }

            
            exeDir = PathHelper.AddDefaultTrailingDelimiter(exeDir);

            var logDir = exeDir + "log";

            if (!Directory.Exists(logDir))
            {
                if (SetAlternativeLogPath(out logFilesPath))
                {
                    return true;
                }
                
                var err = new FileOpsErrorMessageDto
                {
                    DirectoryPath = string.Empty,
                    ErrId = 902,
                    ErrorMessage = "Log Directory Does NOT Exist",
                    ErrSourceMethod = "PurgeOldLogs",
                    FileName = string.Empty,
                    LoggerLevel = LogLevel.FATAL
                };

				ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
				ErrorMgr.WriteErrorMsg(err);

				return false;
            }

            logFilesPath = logDir;

            return true;
        }

        private bool SetAlternativeLogPath(out string altLogPath)
        {
            altLogPath = @".\log";

            if(!Directory.Exists(altLogPath))
            {
                return false;
            }

            return true;

        }

        private bool PurgeOldLogFiles(string logDir)
        {

            string[] logFiles;

            try
            {

                logFiles = Directory.GetFiles(logDir, "*.log");

                if (logFiles.Length < 1)
                {
                    return true;
                }

            }
            catch (Exception ex)
            {
                var err = new FileOpsErrorMessageDto
                {
                    DirectoryPath = string.Empty,
                    ErrId = 99,
                    ErrorMessage = "Exception Thrown While Acquiring Log Files!",
                    ErrSourceMethod = "PurgeOldLogs",
                    ErrException = ex,
                    FileName = string.Empty,
                    LoggerLevel = LogLevel.INFO
                };

				ErrorMgr.WriteErrorMsg(err);

				return false;

            }

            return DeleteOldLogFiles(logFiles);

        }

        private bool DeleteOldLogFiles(string[] logFiles)
        {
            if (logFiles == null || logFiles.Length < 1)
            {
                return true;
            }

            TimeSpan dif = new TimeSpan(_logRetentionInDays, 0, 0, 0);

            DateTime threshold = DateTime.Now.Subtract(dif);

            FileInfo fi;

            foreach (var logFile in logFiles)
            {

                try
                {
                    fi = new FileInfo(logFile);

                    if (fi.CreationTime < threshold)
                    {
                        fi.Delete();
                    }


                }
                catch (Exception ex)
                {
                    var err = new FileOpsErrorMessageDto
                    {
                        DirectoryPath = string.Empty,
                        ErrId = 79,
                        ErrorMessage = "Exception Thrown While Deleting Log File!",
                        ErrSourceMethod = "PurgeOldLogs",
                        ErrException = ex,
                        FileName = logFile,
                        LoggerLevel = LogLevel.INFO
                    };

					ErrorMgr.WriteErrorMsg(err);

				}
			}

            return true;
        }

    }
}