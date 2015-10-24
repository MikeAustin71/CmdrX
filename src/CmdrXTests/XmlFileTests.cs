using CmdrX.Builders;
using CmdrX.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CmdrXTests
{
	[TestClass]
	public class XmlFileTests
	{
		[TestMethod]
		public void VerifyTestXmlFileExists()
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
		public void XmlCmdFileShouldParseSuccessfully()
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
		public void XmlCmdFileShouldParseHeaderSuccessfully()
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