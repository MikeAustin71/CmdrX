using LibLoader.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LibLoadTests
{
	[TestClass]
	public class StringHelperFormatTests
	{
		[TestMethod]
		public void T001_HelperShouldCenterStringCorrectly()
		{
			var s = "How now brown cow!";
			var result = StringHelper.CenterString(s, 30);
			Assert.IsTrue(result.Length == 30);
		}

		[TestMethod]
		public void T002_RightJustifyShouldFormatCorrectly()
		{
			var s = "How now brown cow!";
			var result = StringHelper.RightJustifyString(s, 30);
			Assert.IsTrue(result.Length == 30);
			Assert.IsTrue(result[0] == ' ');
			Assert.IsTrue(result[29] == '!');
		}

		[TestMethod]
		public void T003_LeftJustifyShouldFormatCorrectly()
		{
			var s = "How now brown cow!";
			var result = StringHelper.LeftJustifyString(s, 30);
			Assert.IsTrue(result.Length == 30);
			Assert.IsTrue(result[0] == 'H');
			Assert.IsTrue(result[29] == ' ');
		}

		[TestMethod]
		public void T004_MakeSingleCharStringShouldFormatCorrectly()
		{
			var result = StringHelper.MakeSingleCharString('#', 30);
			Assert.IsTrue(result.Length == 30);
			Assert.IsTrue(result[0] == '#');
			Assert.IsTrue(result[29] == '#');

		}


	}
}