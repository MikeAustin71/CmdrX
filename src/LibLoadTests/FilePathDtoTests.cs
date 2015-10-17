using System.IO;
using LibLoader.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LibLoadTests
{
	[TestClass]
	public class FilePathDtoTests
	{
		[TestMethod]
		public void Test001BreakoutWithFileAndExtensionOnly()
		{
			var testStr = @"SomefileName.txt";
			var expectedResult = @"SomefileName.txt";
			var filePathDto = new FilePathDto(testStr);
			Assert.IsTrue(filePathDto.FileNameAndExtension == expectedResult);

		}

		[TestMethod]
		public void Test002ParseValidFilePathCorrectly()
		{
			var testStr = @"D:\Level1\Level2\SomefileName.txt";
			var expectedFileNameAndExt = @"SomefileName.txt";
			var expectedExt = @".txt";
			var expectedFileNameOnly = @"SomefileName";
			var expectedDirPath = @"D:\Level1\Level2";

            var filePathDto = new FilePathDto(testStr);
			Assert.IsTrue(filePathDto.FileNameAndExtension == expectedFileNameAndExt);
			Assert.IsTrue(filePathDto.DirectoryPath == expectedDirPath);
			Assert.IsTrue(filePathDto.FileNameOnly == expectedFileNameOnly);
			Assert.IsTrue(filePathDto.Extension ==expectedExt);
		}

		[TestMethod]
		public void Test003ParseDirOnlyTrailingSlash()
		{
			var testStr = @"D:\Level1\Level2\";
			var expectedDirPath = @"D:\Level1\Level2";

            var filePathDto = new FilePathDto(testStr);
			Assert.IsTrue(filePathDto.DirectoryPath == expectedDirPath);
		}

		[TestMethod]
		public void Test004ParseDirOnlyNoTrailingSlash()
		{
			var testStr = @"D:\Level1\Level2";
			var expectedDirPath = @"D:\Level1";

            var filePathDto = new FilePathDto(testStr);
			Assert.IsTrue(filePathDto.DirectoryPath == expectedDirPath);
		}

		[TestMethod]
		public void Test005ParseFullPath()
		{
			var testStr = @"D:\Level1\Level2\SomefileName.txt";
			var expectedDirPath = @"D:\Level1\Level2\SomefileName.txt";

            var filePathDto = new FilePathDto(testStr);
			Assert.IsTrue(filePathDto.FullPathAndFileName == expectedDirPath);
		}

		[TestMethod]
		public void Test006ParseFullPathOnRelativePath()
		{
			var fInfo = new FileInfo("SomefileName.txt");
			var testStr = @".\SomefileName.txt";
			var expectedDirPath = fInfo.FullName;

            var filePathDto = new FilePathDto(testStr);
			Assert.IsTrue(filePathDto.FullPathAndFileName == expectedDirPath);
		}

		

	}
}