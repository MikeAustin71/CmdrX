﻿using System;
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

		[TestMethod]
		public void T001_HelperShouldFormatTimespanHoursToMillisecondsCorrectly()
		{
			var dt1 = new DateTime(2015,10,24,14,08,57,200);

			var dt2 = new DateTime(2015,10,24,14,09,59, 345);

			var ts = dt2.Subtract(dt1);

			var result = DateHelper.TimeSpanInMiliseconds(ts);

			var expected = "00:01:02.145";

			Assert.IsTrue(result == expected);
		}

		[TestMethod]
		public void T002_HelperShouldFormatTimespanHoursToMillisecondsCorrectly()
		{
			var dt1 = new DateTime(2015,10,24,14,08,58,200);

			var dt2 = new DateTime(2015,10,24,14,08,58, 345);

			var ts = dt2.Subtract(dt1);

			var result = DateHelper.TimeSpanInMiliseconds(ts);

			var expected = "00:00:00.145";

			Assert.IsTrue(result == expected);
		}



		[TestMethod]
		public void T003_HelperShouldFormatTimespanDaysToMillisecondsCorrectly()
		{
			var dt1 = new DateTime(2015,10,23,14,08,57,200);

			var dt2 = new DateTime(2015,10,24,14,09,59, 345);

			var ts = dt2.Subtract(dt1);

			var result = DateHelper.TimeSpanInMiliseconds(ts);

			var expected = "01.00:01:02.145";

			Assert.IsTrue(result == expected);
		}

		[TestMethod]
		public void T004_HelperShouldFormatDatetimeToMillisecondsCorrectly()
		{
			var dt1 = new DateTime(2015,10,23,14,08,57,200);

			var result = DateHelper.DateTimeToMillisconds(dt1);

			var expected = "2015-10-23 14:08:57.200";

			Assert.IsTrue(result == expected);
		}

		[TestMethod]
		public void T005_HelperShouldFormatDatetimeToDayMillisecondsCorrectly()
		{
			var dt1 = new DateTime(2015,10,24,14,08,57,200);

			var result = DateHelper.DateTimeToDayMilliseconds(dt1);

			var expected = "Saturday, 2015-10-24 14:08:57.200";

			Assert.IsTrue(result == expected);
		}

		[TestMethod]
		public void T006_HelperShouldFormatTimespanVeboseDisplayCorrectly()
		{
			var dt1 = new DateTime(2015, 10, 24, 14, 08, 58, 200);

			var dt2 = new DateTime(2015, 10, 24, 14, 08, 58, 345);

			var ts = dt2.Subtract(dt1);

			var result = DateHelper.TimeSpanDetailToMiliseconds(ts);

			var expected = "00 Hours  00 Minutes  00 Seconds  145 Milliseconds";

			Assert.IsTrue(result == expected);

		}

		[TestMethod]
		public void T007_HelperShouldFormatTimespanVeboseDaysToDisplayCorrectly()
		{
			var dt1 = new DateTime(2015, 10, 23, 13, 07, 52, 200);

			var dt2 = new DateTime(2015, 10, 24, 14, 08, 58, 345);

			var ts = dt2.Subtract(dt1);

			var result = DateHelper.TimeSpanDetailToMiliseconds(ts);

			var expected = "01 Days  01 Hours  01 Minutes  06 Seconds  145 Milliseconds";

			Assert.IsTrue(result == expected);

		}


	}
}