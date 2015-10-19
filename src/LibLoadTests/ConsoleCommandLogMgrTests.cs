using LibLoader.Helpers;
using LibLoader.Managers;
using LibLoader.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LibLoadTests
{
	[TestClass]
	public class ConsoleCommandLogMgrTests
	{
		[TestMethod]
		public void WriteTextLinesToLogMgr()
		{
			var dir1Dto = new DirectoryDto(".");

			var fileStamp = DateHelper.NowYearMthDayHrsSecs();

			var mgr = new ConsoleCommandLogMgr(".\\logdir\\installLog.log",
												string.Empty,
														fileStamp);

			mgr.InitializeCmdConsoleLog(string.Empty);

			mgr.LogWriteLine("Hello World");

			mgr.LogFlushStreamWriter();

			mgr.Dispose();
		}
		 
	}
}