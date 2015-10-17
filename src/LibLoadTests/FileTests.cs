using System.IO;
using LibLoader.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LibLoadTests
{
	[TestClass]
	public class FileTests
	{
		[TestMethod]
		public void Test001CreateFileDtoUsingFileNameString()
		{
			var testStr = @"D:\Level1\Level2\SomefileName.txt";

			var fileDto = new FileDto(testStr);
		}
		 
		[TestMethod]
		public void Test002CreateFileDtoUsingFileNameString()
		{
			var testStr = @"SomefileName.txt";

			var fileDto = new FileInfo(testStr);

			Assert.IsNotNull(fileDto);
		}
		 
	}
}