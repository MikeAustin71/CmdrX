using System;
using System.Security.Permissions;

namespace CmdrX.Helpers
{
	public static class DateHelper
	{
		public static string NowYearMthDayHrsSecs()
		{
			//"yyyy'-'MM'-'dd'T'HH':'mm':'ss"
			return DateTime.Now.ToString("yyyMMddHHmmss");
		}

		public static string TimeSpanInMiliseconds(TimeSpan ts)
		{
			var fmt = ts.Days > 0 ? @"dd\.hh\:mm\:ss\.fff" : @"hh\:mm\:ss\.fff";

			try
			{
				return ts.ToString(fmt);
			}
			catch
			{
				return string.Empty;
			}
		}

		public static string DateTimeToMillisconds(DateTime dt)
		{
			try
			{
				return dt.ToString(@"yyyy-MM-dd HH:mm:ss.fff");
			}
			catch
			{
				return string.Empty;
			}
			
		}
	}
}