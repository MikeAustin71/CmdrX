using System;
using System.Collections.Generic;
using System.Linq;
using LibLoader.Constants;
using LibLoader.GlobalConstants;
using LibLoader.Helpers;
using LibLoader.Models;

namespace LibLoader.Builders
{
    public class CommandLineParameterBuilder
    {
		public ErrorLogger ErrorMgr = new ErrorLogger(4000,
			"CommandLineParameterBuilder",
			AppConstants.LoggingStatus,
			AppConstants.LoggingMode);

		public Dictionary<string,string> CommandLineArguments { get; set; } = new Dictionary<string, string>();

        public bool BuildFileInfoParamters(string[] commandArgs)
        {
            if(commandArgs==null || commandArgs.Length < 1)
            {
                return false;
            }

            return ProcessCommandArgs(commandArgs);
        }

	    private bool ProcessCommandArgs(string[] commandArgs)
	    {

		    var parser = new ParseArgs(commandArgs);

		    if (parser.Arguments.Count == 0)
		    {

			    var err = new FileOpsErrorMessageDto
			    {
				    DirectoryPath = String.Empty,
				    ErrId = 2,
				    ErrorMessage = "Command Line Arguments Contained No Valid Commands!",
				    ErrSourceMethod = "ProcessCommandArgs",
				    FileName = string.Empty,
				    LoggerLevel = LogLevel.FATAL
			    };

				ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
				ErrorMgr.WriteErrorMsg(err);

				return false;
		    }

		    CommandLineArguments = parser.Arguments;

		    AppConstants.CommandLineArguments = parser.Arguments;

		    if (ResquestForHelp(parser.Arguments))
		    {
			    return false;
		    }

		    var result = false;

		    foreach (var pair in parser.Arguments)
		    {
			    if (pair.Key.ToLower() == "xml")
			    {
				    var fileDto = new FileDto(pair.Value);

				    if (!fileDto.FileXinfo.Exists)
				    {
						var err = new FileOpsErrorMessageDto
						{
							DirectoryPath = String.Empty,
							ErrId = 3,
							ErrorMessage = "Command Line Xml File spec does NOT exist!",
							ErrSourceMethod = "ProcessCommandArgs",
							FileName = string.Empty,
							LoggerLevel = LogLevel.FATAL
						};

						ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
						ErrorMgr.WriteErrorMsg(err);

						return false;
					}

				    AppConstants.XmlCmdFileDto = fileDto;
				    result = true;
			    }

			    if (pair.Key.ToLower() == "l"
					 && pair.Value.ToLower().Contains("off"))
			    {
					AppConstants.LoggingStatus = ErrorLoggingStatus.Off;
				    result = true;
			    }

			    if (pair.Key.ToLower() == "lr")
			    {
				    int logRetentionDays;
				    if (int.TryParse(pair.Value, out logRetentionDays))
				    {
					    AppConstants.AppLogMgr.LogRetentionInDays = logRetentionDays;
					    result = true;
				    }
			    }


			}

			return result;
	    }

		private bool ResquestForHelp(Dictionary<string, string> dictArgs)
		{
			if (dictArgs == null || dictArgs.Count < 1)
			{
				return false;
			}

			return dictArgs
				.Any(pair => pair.Key.ToLower().Contains("help") 
				|| pair.Key.ToLower().Contains("?"));
		}


	}
}