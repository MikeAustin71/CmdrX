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

			Assert.IsTrue(fileDto.FileXinfo.FullName == testStr);
		}
		 
		[TestMethod]
		public void Test002CreateFileDtoUsingFileNameOnly()
		{
			var testStr = @"SomefileName.txt";
			var expectedDir = TestDirectories.GetTestExeDir();
			var expectedResult = Path.Combine(expectedDir.DirInfo.FullName, testStr);
			var fileDto = new FileDto(testStr);
			Assert.IsTrue(fileDto.FileXinfo.FullName == expectedResult);
			
		}
		 
		[TestMethod]
		public void Test003CreateFileDtoWithNoExt()
		{
			var testStr = @"D:\Level1\Level2";
			var expectedResult = "Level2";
			var expectedDir = @"D:\Level1";
            var fileDto = new FileDto(testStr);
			Assert.IsTrue(fileDto.FileXinfo.Name == expectedResult);
			Assert.IsTrue(fileDto.DirDto.DirInfo.FullName == expectedDir);
		}
		 
	}
}