using System;
using CmdrX.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CmdrXTests
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

		[TestMethod]
		public void HelperShouldRemoveNewLineCharsCorrectly()
		{
			//var s = "How now brown cow.\n The Cow jumped \nover the moon.\n\n";
			var s = "How \n\n\n";
			var s2 = StringHelper.RemoveCarriageReturns(s);

			var result = s2.IndexOf("\n", StringComparison.Ordinal);

			Assert.IsTrue(result == -1);
			Assert.IsTrue(s2 == "How ");
		}

		[TestMethod]
		public void HelperShouldRemoveNewLineChars2Correctly()
		{
			var s = "\nHow \nnow \nbrown \ncow.\n\n\n";
			var expected = "How now brown cow.";
			
			var s2 = StringHelper.RemoveCarriageReturns(s);

			var result = s2.IndexOf("\n", StringComparison.Ordinal);

			
			Assert.IsTrue(result == -1);
			Assert.IsTrue(s2 == expected);
		}

		[TestMethod]
		public void HelperShouldRemoveNewLineChars3Correctly()
		{
			var s = "\n\n\n\nHow \nnow \nbrown \ncow.\n\n\n";
			var expected = "How now brown cow.";
			
			var s2 = StringHelper.RemoveCarriageReturns(s);

			var result = s2.IndexOf("\n", StringComparison.Ordinal);

			
			Assert.IsTrue(result == -1);
			Assert.IsTrue(s2 == expected);
		}


		[TestMethod]
		public void HelperShouldApplyLineBreaksCorrectly90Charline()
		{
			var testText = TestStringsSetup.IpsumString90Chars;

			var strArray = StringHelper.BreakLineAtIndex(testText, 80);

			Assert.IsNotNull(strArray);
			Assert.IsTrue(strArray.Length == 2);
			Assert.IsTrue(strArray[0].Length <= 80);
		}

		[TestMethod]
		public void HelperShouldApplyLineBreaksCorrectly2013TextChars()
		{
			var testText = StringHelper.RemoveCarriageReturns(TestStringsSetup.IpsumString01);

			var strArray = StringHelper.BreakLineAtIndex(testText, 80);

			Assert.IsNotNull(strArray);
			Assert.IsTrue(strArray.Length == 27);

			foreach (var s in strArray)
			{
				Assert.IsTrue(s.Length <= 80);
			}
		}

		[TestMethod]
		public void HelperShouldApplyLineBreaksCorrectlyForSmallText()
		{
			var testText = "How now brown cow";
			var lBreakLength = 5;

			var strArray = StringHelper.BreakLineAtIndex(testText, lBreakLength);

			Assert.IsNotNull(strArray);
			Assert.IsTrue(strArray.Length == 4);

		}

		[TestMethod]
		public void T101_HelperShouldApplyRemoveLeadingCharsCorrectly()
		{
			var testText = "_How now brown cow";

			var result = StringHelper.RemoveLeadingChar(testText, '_');

			Assert.IsTrue(!string.IsNullOrWhiteSpace(result));
			Assert.IsTrue(result[0] != '_');

		}

		[TestMethod]
		public void T102_HelperShouldApplyRemoveTwoLeadingCharsCorrectly()
		{
			var testText = "__How now brown cow";

			var result = StringHelper.RemoveLeadingChar(testText, '_');

			Assert.IsTrue(!string.IsNullOrWhiteSpace(result));
			Assert.IsTrue(result == "How now brown cow");
		}


		[TestMethod]
		public void T103_HelperShouldApplyRemoveTwoLeadingCharsCorrectly()
		{
			var testText = "___H";

			var result = StringHelper.RemoveLeadingChar(testText, '_');

			Assert.IsTrue(!string.IsNullOrWhiteSpace(result));
			Assert.IsTrue(result == "H");
		}

		[TestMethod]
		public void T104_HelperShouldApplyRemoveAllLeadingCharsCorrectly()
		{
			var testText = "___";

			var result = StringHelper.RemoveLeadingChar(testText, '_');

			Assert.IsTrue(result == string.Empty);
		}


	}
}