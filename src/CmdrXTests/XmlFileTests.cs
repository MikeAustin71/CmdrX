using CmdrX.Builders;
using CmdrX.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CmdrXTests
{
	[TestClass]
	public class XmlFileTests
	{
		[TestMethod]
		public void T001_VerifyTestXmlFileExists()
		{
			var cmdExeDto = TestDirectories.GetConsoleExecutorDto();

			var fileDto = cmdExeDto.XmlCmdFileDto;
			Assert.IsNotNull(fileDto);
			Assert.IsTrue(fileDto.DirDto.DirInfo.Exists);
			Assert.IsTrue(fileDto.FileXinfo.Exists);
			Assert.IsTrue(fileDto.FileNameAndExtension == "CmdrXCmds.xml");
			Assert.IsTrue(fileDto.FullPathAndFileName == fileDto.FileXinfo.FullName);
			Assert.IsTrue(fileDto.FileExtension == fileDto.FileXinfo.Extension);
			Assert.IsTrue(fileDto.FilePath == fileDto.FileXinfo.DirectoryName);

			fileDto.Dispose();

		}

		[TestMethod]
		public void T002_XmlCmdFileShouldParseSuccessfully()
		{
			var cmdExeDto = TestDirectories.GetConsoleExecutorDto();

			var builder = new XmlParameterBuilder(cmdExeDto);

			var jobGroup = builder.BuildParmsFromXml();

			Assert.IsNotNull(jobGroup);

			Assert.IsTrue(jobGroup.Jobs.Count == 3);

			Assert.IsTrue(jobGroup.Jobs[0].CommandDisplayName== "jshint and jscs");

			Assert.IsTrue(jobGroup.Jobs[2].CommandDisplayName == "gulp plumber");
		}

		[TestMethod]
		public void T003_XmlParserShouldParseExitCodeLimitsCorrectly()
		{
			var cmdExeDto = TestDirectories.GetConsoleExecutorDto();
			var defaultUpperLimit = 99999;
			var defaultLowerLimit = -99999;

			Assert.IsTrue(cmdExeDto.DefaultKillJobsRunOnExitCodeGreaterThan == defaultUpperLimit);
			Assert.IsTrue(cmdExeDto.DefaultKillJobsRunOnExitCodeLessThan == defaultLowerLimit);

			var builder = new XmlParameterBuilder(cmdExeDto);

			var jobGroup = builder.BuildParmsFromXml();

			Assert.IsNotNull(jobGroup);

			Assert.IsTrue(jobGroup.Jobs.Count == 3);
			Assert.IsTrue(jobGroup.Jobs[1].KillJobsRunOnExitCodeGreaterThan == 50);
			Assert.IsTrue(jobGroup.Jobs[1].KillJobsRunOnExitCodeLessThan == -50);

			Assert.IsTrue(jobGroup.Jobs[2].KillJobsRunOnExitCodeGreaterThan == defaultUpperLimit);
			Assert.IsTrue(jobGroup.Jobs[2].KillJobsRunOnExitCodeLessThan == defaultLowerLimit);
		}

		[TestMethod]
		public void T004_XmlCmdFileShouldParseHeaderSuccessfully()
		{
			var cmdExeDto = TestDirectories.GetConsoleExecutorDto();

			var builder = new XmlParameterBuilder(cmdExeDto);

			builder.BuildParmsFromXml();
			var expectedDirDto = new DirectoryDto(TestDirectories.GetTestExeDir().DirInfo.FullName + "\\installLog");
			var dirDto = new DirectoryDto(cmdExeDto.DefaultCommandOutputLogFilePathName);
            Assert.IsTrue(cmdExeDto.AppLogRetentionInDays == 0);
			Assert.IsTrue(dirDto.DirInfo.FullName == expectedDirDto.DirInfo.FullName);
		}


	}
}