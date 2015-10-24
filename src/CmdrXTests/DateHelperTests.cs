using System;
using CmdrX.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CmdrXTests
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