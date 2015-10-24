using System;
using System.IO;
using System.Linq;
using CmdrX.Constants;
using CmdrX.GlobalConstants;
using CmdrX.Helpers;
using CmdrX.Models;

namespace CmdrX.Commands
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
		        return true;
	        }

			return PurgeOldLogFiles(_logFileDirectoryDto.DirInfo.FullName);
        }


	    private bool PurgeOldLogFiles(string logDir)
        {

			if (_logRetentionInDays > 365)
			{
				return true;
			}

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

	        return _logRetentionInDays < 1 ? DeleteAllLogFiles(logFiles) : DeleteOldLogFiles(logFiles);
        }

	    private bool DeleteAllLogFiles(string[] logFiles)
	    {
			if (logFiles == null || logFiles.Length < 1)
			{
				return true;
			}


		    foreach (var fileDto in logFiles.Select(logFile => new FileDto(logFile)))
		    {
			    FileHelper.DeleteAFile(fileDto);
		    }

		    return true;
	    }

		private bool DeleteOldLogFiles(string[] logFiles)
        {
            if (logFiles == null || logFiles.Length < 1)
            {
                return true;
            }

            TimeSpan dif = new TimeSpan(_logRetentionInDays, 0, 0, 0);

            DateTime threshold = DateTime.Now.Subtract(dif);

			foreach (var logFile in logFiles)
            {
                try
                {
	                var fileDto = new FileDto(logFile);

	                if (fileDto.FileXinfo.CreationTime < threshold)
                    {
	                    FileHelper.DeleteAFile(fileDto);
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