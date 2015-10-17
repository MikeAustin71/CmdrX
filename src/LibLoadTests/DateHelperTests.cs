using System;
using LibLoader.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LibLoadTests
{
	[TestClass]
	public class DateHelperTests
	{
		[TestMethod]
		public void DateTimeNowShouldFormatCorrectly()
		{
			var result = DateHelper.NowYearMthDayHrsSecs();
			var expectedResult = DateTime.Now.ToString("yyyMMddHHmmss");
			Assert.IsTrue(result==expectedResult);
		}
	}
}