using CmdrX.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CmdrXTests
{
	[TestClass]
	public class XmlParameterConverterTests
	{
		[TestMethod]
		public void T001_HelperShouldConvertToCorrectDateString()
		{
			var cmdDto = TestDirectories.GetConsoleExecutorDto();
			cmdDto.CmdConsoleLogFileTimeStamp = "20151026195929";

			var hlpr = new XmlParameterConverter(cmdDto);
			var testXml = "%(CURDATESTR)%_SomeDirectoryName";
			var result = hlpr.RunConversion(testXml);
			var expected = "20151026195929_SomeDirectoryName";
			Assert.IsTrue(result == expected);
		}
	}
}