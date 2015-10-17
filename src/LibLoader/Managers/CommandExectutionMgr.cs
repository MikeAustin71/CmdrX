using LibLoader.GlobalConstants;
using LibLoader.Helpers;
using LibLoader.Models;

namespace LibLoader.Managers
{
	public class CommandExectutionMgr
	{
		public ErrorLogger ErrorMgr = new
			ErrorLogger(1677000,
						"CommandExectutionMgr",
						AppConstants.LoggingStatus,
						AppConstants.LoggingMode);

		private JobsGroupDto _commandJobs;

		private ConsoleCommandLogMgr _cmdLogMgr;

		public CommandExectutionMgr(JobsGroupDto jobsGroup, 
					string defaultCmdConsoleLogFileBaseName, 
						string logFileTimeStamp)
		{
			_commandJobs = jobsGroup;
			_cmdLogMgr = new ConsoleCommandLogMgr(defaultCmdConsoleLogFileBaseName, logFileTimeStamp);

		} 
	}
}