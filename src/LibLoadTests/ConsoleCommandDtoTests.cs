using LibLoader.Builders;
using LibLoader.Helpers;
using LibLoader.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LibLoadTests
{
	[TestClass]
	public class ConsoleCommandDtoTests
	{
		[TestMethod]
		public void ConsoleCommandShouldInitializeCorrectly()
		{
			var cmdExeDto = TestDirectories.GetConsoleExecutorDto();

			var builder = new XmlParameterBuilder(cmdExeDto);

			var jobGroup = builder.BuildParmsFromXml();

			var job = jobGroup.Jobs[0];

			Assert.IsNotNull(job);

			var expectedProcFile = "cmd.exe";
			var expectedProcArgs = "/c npm install --save-dev gulp-jshint gulp-jscs jshint-stylish";

            Assert.IsTrue(job.ProcFileNameCommand == expectedProcFile);

			Assert.IsTrue(job.ProcFileArguments == expectedProcArgs);
		}

		[TestMethod]
		public void ConsoleCommandShouldCorrectlyProcessWithNoCommandExecutor()
		{
			var mainTestDir = TestDirectories.GetMainLibLoadTestDir();
			var xmlFileDto = new FileDto(mainTestDir, ".\\XmlCmdFiles\\CmdFile002.xml");

			if (!FileHelper.IsFileDtoValid(xmlFileDto) || !xmlFileDto.FileXinfo.Exists)
			{
				Assert.IsTrue(false);
			}


			var cmdExeDto = TestDirectories.GetConsoleExecutorDto();
			cmdExeDto.XmlCmdFileDto = xmlFileDto;

			var builder = new XmlParameterBuilder(cmdExeDto);

			var jobGroup = builder.BuildParmsFromXml();

			var job = jobGroup.Jobs[0];

			Assert.IsNotNull(job);

			var expectedProcFile = "npm";
			var expectedProcArgs = "install --save-dev gulp-jshint gulp-jscs jshint-stylish";

            Assert.IsTrue(job.ProcFileNameCommand == expectedProcFile);

			Assert.IsTrue(job.ProcFileArguments == expectedProcArgs);
		}


	}
}