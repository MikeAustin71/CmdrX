using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibLoader.Builders;
using LibLoader.Commands;
using LibLoader.Constants;
using LibLoader.GlobalConstants;
using LibLoader.Helpers;

namespace LibLoader
{
	class Program
	{
		static void Main(string[] args)
		{
			// Setup Application Logging
			AppConstants.AppLogMgr.CreateApplicaitonLogDirectory();
			AppConstants.AppLogMgr.PurgeLogCmd.Execute();
			LogUtil.ExeAssemblyVersionNo = AppInfoHelper.GetThisAssemblyVersion();
			AppConstants.LoggingStatus = ErrorLoggingStatus.On;
			AppConstants.LoggingMode = ErrorLoggingMode.Verbose;

			if (args.Length > 0)
			{
				if (!new CommandLineParameterBuilder().BuildFileInfoParamters(args))
				{
					AppInfoHelper.DisplayCmdLineParms();
					Environment.ExitCode = 0;
					return;
				}
			}

			if (!FileHelper.DoesFileExist(AppConstants.XmlCmdFileDto))
			{
				AppInfoHelper.DisplayCmdLineParms();
				Environment.ExitCode = -1;
				return;

			}

			//var xmlBuilder 



			

		}
	}
}
