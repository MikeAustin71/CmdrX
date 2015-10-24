using System;

namespace CmdrX.Helpers
{
	public static class DateHelper
	{
		public static string NowYearMthDayHrsSecs()
		{
			//"yyyy'-'MM'-'dd'T'HH':'mm':'ss"
			return DateTime.Now.ToString("yyyMMddHHmmss");
		}
	}
}