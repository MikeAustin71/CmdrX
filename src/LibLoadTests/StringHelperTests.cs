using LibLoader.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LibLoadTests
{
	[TestClass]
	public class StringHelperTests
	{

		[TestMethod]
		public void GetLastCharIndexShouldReturnCorrectIndex()
		{
			var testDir = @"D:\Level1\SomeFile.txt";

			var idx = StringHelper.GetLastCharIndex(testDir, PathHelper.PrimaryPathDelimiter);

			Assert.IsTrue(testDir[idx]==PathHelper.PrimaryPathDelimiter);
			Assert.IsTrue(idx==9);

		}

		[TestMethod]
		public void UnsuccessfulGetLastCharIndexShoudReturnMinusOne()
		{
			var testDir = @"D:\Level1\SomeFile.txt";

			var idx = StringHelper.GetLastCharIndex(testDir, PathHelper.AlternatePathDelimiter);

			Assert.IsTrue(idx==-1);
		}

	}
}