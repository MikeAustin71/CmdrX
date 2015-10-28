using System;
using System.Collections.Generic;
using System.Linq;
using CmdrX.Constants;
using CmdrX.GlobalConstants;
using CmdrX.Helpers;
using CmdrX.Models;

namespace CmdrX.Builders
{
    public class CommandLineParameterBuilder
    {
	    private readonly ConsoleExecutorDto _cmdExeDto;

		public ErrorLogger ErrorMgr = new ErrorLogger(4000,
			"CommandLineParameterBuilder",
			AppConstants.LoggingStatus,
			AppConstants.LoggingMode,
			false);

		public Dictionary<string,string> CommandLineArguments { get; set; } = new Dictionary<string, string>();

	    public CommandLineParameterBuilder(ConsoleExecutorDto cmdExeDto)
	    {
		    _cmdExeDto = cmdExeDto;
	    }

        public bool BuildFileInfoParamters(string[] commandArgs)
        {
            if(commandArgs==null || commandArgs.Length < 1)
            {
                return true;
            }

            return ProcessCommandArgs(commandArgs);
        }

	    private bool ProcessCommandArgs(string[] commandArgs)
	    {

		    var parser = new ParseArgs(commandArgs);

		    if (parser.Arguments.Count == 0)
		    {
				return true;
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

				    if (!FileHelper.IsFileDtoValid(fileDto) || !fileDto.FileXinfo.Exists)
				    {
						var err = new FileOpsErrorMessageDto
						{
							DirectoryPath = String.Empty,
							ErrId = 3,
							ErrorMessage = "Command Line Xml File spec does NOT exist! Option -xml INVALID!",
							ErrSourceMethod = "ProcessCommandArgs",
							FileName = string.Empty,
							LoggerLevel = LogLevel.FATAL
						};

						_cmdExeDto.ApplicationExitStatus.OpsError = ErrorMgr.FormatErrorDto(err);
					    _cmdExeDto.ApplicationExitStatus.IsFatalError = true;
						_cmdExeDto.ApplicationExitStatus.WriteExitConsoleMessage();

						return false;
					}

				    _cmdExeDto.XmlCmdFileDto = fileDto;

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
						_cmdExeDto.AppLogMgr.LogRetentionInDays = logRetentionDays;
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

			var result = dictArgs
				.Any(pair => pair.Key.ToLower().Contains("help") 
				|| pair.Key.ToLower().Contains("?"));

			if (result)
			{
				var err = new FileOpsErrorMessageDto
				{
					DirectoryPath = string.Empty,
					ErrId = 57,
					ErrorMessage = "User Requested Command Line Options",
					ErrSourceMethod = "ResquestForHelp()",
					FileName = string.Empty,
					LoggerLevel = LogLevel.INFO
				};

				_cmdExeDto.ApplicationExitStatus.IsDisplayCmdLineArgs = true;
				_cmdExeDto.ApplicationExitStatus.OpsError = ErrorMgr.FormatErrorDto(err);
				Environment.ExitCode = 0;
				_cmdExeDto.ApplicationExitStatus.WriteExitConsoleMessage();

			}

			return result;
		}


	}
}