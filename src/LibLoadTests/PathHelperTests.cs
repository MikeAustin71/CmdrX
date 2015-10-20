using LibLoader.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LibLoadTests
{
	[TestClass]
	public class PathHelperTests
	{
		[TestMethod]
		public void T1HelperShouldCorrectlyExtractDirComponent()
		{
			var testStr = @"D:\Level1\Level2\SomefileName.txt";
			var expectedResult = @"D:\Level1\Level2";
            var dirComponent = PathHelper.ExtractDirectoryComponent(testStr);
			Assert.IsTrue(dirComponent==expectedResult);
		}

		[TestMethod]
		public void T2HelperShouldCorrectlyExtractDirComponent()
		{
			var testStr = @".\Level2\SomefileName.txt";
			var expectedResult = @".\Level2";
            var dirComponent = PathHelper.ExtractDirectoryComponent(testStr);
			Assert.IsTrue(dirComponent==expectedResult);
		}

		[TestMethod]
		public void T3HelperShouldCorrectlyExtractEmptyDirComponent()
		{
			var testStr = @"SomefileName.txt";
			var expectedResult = string.Empty;
            var dirComponent = PathHelper.ExtractDirectoryComponent(testStr);
			Assert.IsTrue(dirComponent==expectedResult);
		}

		[TestMethod]
		public void T4HelperShouldCorrectlyExtractAmbiguousDirComponent()
		{
			var testStr = @"D:\Level1\Level2";
			var expectedResult = @"D:\Level1";
            var dirComponent = PathHelper.ExtractDirectoryComponent(testStr);
			Assert.IsTrue(dirComponent==expectedResult);
			var fileComponent = PathHelper.ExtractFileNameOnlyComponent(testStr);
			expectedResult = "Level2";
			Assert.IsTrue(fileComponent==expectedResult);
		}

		[TestMethod]
		public void T11HelperShouldCorrectlyExtractFileNameOnlyComponent()
		{
			var testStr = @".\Level2\SomefileName.txt";
			var expectedResult = @"SomefileName";
            var fileComponent = PathHelper.ExtractFileNameOnlyComponent(testStr);
			Assert.IsTrue(fileComponent==expectedResult);
		}

		[TestMethod]
		public void T12HelperShouldCorrectlyExtractFileNameAndExtensionComponent()
		{
			var testStr = @"SomefileName.txt";
			var expectedResult = @"SomefileName.txt";
            var fileComponent = PathHelper.ExtractFileNameAndExtension(testStr);
			Assert.IsTrue(fileComponent==expectedResult);
		}

		[TestMethod]
		public void T14HelperShouldCorrectlyExtractFileNameOnlyComponentFromFullPath()
		{
			var testStr = @"D:\Level1\Level2\SomefileName.txt";
			var expectedResult = @"SomefileName";
			var fileComponent = PathHelper.ExtractFileNameOnlyComponent(testStr);
			Assert.IsTrue(fileComponent == expectedResult);
		}

		[TestMethod]
		public void T15HelperShouldCorrectlyExtractNonStandardFileComponentFromFullPath()
		{
			var testStr = @"D:\Level1\Level2\.gitignore";
			var expectedResult = ".gitignore";
			var fileComponent = PathHelper.ExtractFileNameOnlyComponent(testStr);
			Assert.IsFalse(fileComponent == expectedResult);
			fileComponent = PathHelper.ExtractFileNameAndExtension(testStr);
			Assert.IsTrue(fileComponent == expectedResult);
		}



		[TestMethod]
		public void T100HelperShouldCorrectlyExtractFileExtComponentFromFullPath()
		{
			var testStr = @"D:\Level1\Level2\SomefileName.txt";
			var expectedResult = @".txt";
			var extComponent = PathHelper.ExtractFileExtensionComponent(testStr);
			Assert.IsTrue(extComponent == expectedResult);
		}

		[TestMethod]
		public void T101HelperShouldCorrectlyExtractFileExtComponentFromFullPath()
		{
			var testStr = @"D:\Level1\Level2\.gitignore";
			var expectedResult = @".gitignore";
			var extComponent = PathHelper.ExtractFileExtensionComponent(testStr);
			Assert.IsTrue(extComponent == expectedResult);
		}

		[TestMethod]
		public void T201HelperShouldRemoveLeadingDotsFromDirectoryPath()
		{
			var testStr = "..\\SomDir\\SomFile.txt";
			var expected = "\\SomDir\\SomFile.txt";
			var result = PathHelper.RemovePrefixDots(testStr);

			Assert.IsTrue(result == expected);
		}

		[TestMethod]
		public void T202HelperShouldRemoveLeadingSingleDotFromDirectoryPath()
		{
			var testStr = ".\\SomDir\\SomFile.txt";
			var expected = "\\SomDir\\SomFile.txt";
			var result = PathHelper.RemovePrefixDots(testStr);

			Assert.IsTrue(result == expected);
		}

		[TestMethod]
		public void T203HelperShouldRemovePrefixDelimiterFromDirectoryPath()
		{
			var testStr = "..\\SomDir\\SomFile.txt";
			var expected = "SomDir\\SomFile.txt";
			var result = PathHelper.RemovePrefixDelimiter(testStr);

			Assert.IsTrue(result == expected);
		}

		[TestMethod]
		public void T204HelperShouldRemovePrefixSingleDotDelimiterFromDirectoryPath()
		{
			var testStr = ".\\SomDir\\SomFile.txt";
			var expected = "SomDir\\SomFile.txt";
			var result = PathHelper.RemovePrefixDelimiter(testStr);

			Assert.IsTrue(result == expected);
		}

	}
}