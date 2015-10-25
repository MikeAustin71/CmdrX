using CmdrX.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CmdrXTests
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

		[TestMethod]
		public void T005_TestRightJustifyNumericFormat()
		{
			var x1 = 1;
			var sX1 = $"This is right justify one digit {x1,3:##0}";
			var x20 = 20;
			var sX20 = $"This is right justify two digit {x20,3:##0}";
			Assert.IsTrue(sX1.Length == sX20.Length);
			Assert.IsTrue(sX1[sX1.Length-1] == '1');
			Assert.IsTrue(sX20[sX20.Length-1] == '0');
		}

	}
}