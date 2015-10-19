using LibLoader.Builders;
using LibLoader.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LibLoadTests
{
	[TestClass]
	public class XmlFileTests
	{
		[TestMethod]
		public void VerifyTestXmlFileExists()
		{
			var fileDto = AppInfoHelper.GetDefaultXmlCommandFile();
			Assert.IsNotNull(fileDto);
			Assert.IsTrue(fileDto.DirDto.DirInfo.Exists);
			Assert.IsTrue(fileDto.FileXinfo.Exists);
			Assert.IsTrue(fileDto.FileNameAndExtension == "LibLoadCmds.xml");
			Assert.IsTrue(fileDto.FullPathAndFileName == fileDto.FileXinfo.FullName);
			Assert.IsTrue(fileDto.FileExtension == fileDto.FileXinfo.Extension);
			Assert.IsTrue(fileDto.FilePath == fileDto.FileXinfo.DirectoryName);

		}

		[TestMethod]
		public void XmlCmdFileShouldParseSuccessfully()
		{
			var fileDto = AppInfoHelper.GetDefaultXmlCommandFile();
			Assert.IsNotNull(fileDto);
			Assert.IsNotNull(fileDto.FileXinfo);
			Assert.IsTrue(fileDto.FileXinfo.Exists);

			var builder = new XmlParameterBuilder(fileDto);

			var jobGroup = builder.BuildParmsFromXml();

			Assert.IsNotNull(jobGroup);

			Assert.IsTrue(jobGroup.Jobs.Count == 3);

			Assert.IsTrue(jobGroup.Jobs[0].CommandDisplayName== "jshint and jscs");

			Assert.IsTrue(jobGroup.Jobs[2].CommandDisplayName == "gulp plumber");

		}

	}
}