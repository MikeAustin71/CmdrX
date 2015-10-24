using System;
using System.Security.Permissions;
using System.Text;

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

		public static string TimeSpanDetailToMiliseconds(TimeSpan ts)
		{

			var sb = new StringBuilder();

			try
			{
				var fmt = ts.Days > 0 ? @"dd\.hh\:mm\:ss\.fff" : @"hh\:mm\:ss\.fff";

				var test = ts.ToString(fmt);

				if (ts.Days > 0)
				{
					sb.Append($"{ts.Days:00} Days  ");
				}

				sb.Append($"{ts.Hours:00} Hours  ");
				sb.Append($"{ts.Minutes:00} Minutes  ");
				sb.Append($"{ts.Seconds:00} Seconds  ");
				sb.Append($"{ts.Milliseconds:000} Milliseconds");

				return sb.ToString();
			}
			catch
			{
				return string.Empty;
			}
		}

		public static string DateTimeToDayMilliseconds(DateTime dt)
		{
			try
			{
				return dt.ToString(@"dddd, yyyy-MM-dd HH:mm:ss.fff");
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