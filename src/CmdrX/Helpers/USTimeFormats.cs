using System.Security.Permissions;

namespace CmdrX.Helpers
{
// ReSharper disable InconsistentNaming
	public static class USTimeFormats
	{
		/*
					*******************************************************
					*** NOTE: These formats are used for 'DateTime' values
					https://msdn.microsoft.com/en-us/library/8kb3ddd4(v=vs.110).aspx
					*******************************************************
					d - Numeric day of the month without a leading zero.
					dd - Numeric day of the month with a leading zero.
					ddd - Abbreviated name of the day of the week.
					dddd - Full name of the day of the week.

					f,ff,fff,ffff,fffff,ffffff,fffffff - 
						Fraction of a second. The more Fs the higher the precision.

					f		- tenths of second
					ff		- hundredths of a second
					fff		- thousandths of a second = milliseconds
					ffff	- ten thousandths of a second
					fffff	- The hundred thousandths of a second in a date and time value.
					ffffff  - The millionths of a second in a date and time value.
					fffffff	- The ten millionths of a second in a date and time value.

					h - 12 Hour clock, no leading zero.
					hh - 12 Hour clock with leading zero.
					H - 24 Hour clock, no leading zero.
					HH - 24 Hour clock with leading zero.

					m - Minutes with no leading zero.
					mm - Minutes with leading zero.

					M - Numeric month with no leading zero.
					MM - Numeric month with a leading zero.
					MMM - Abbreviated name of month.
					MMMM - Full month name.

					s - Seconds with no leading zero.
					ss - Seconds with leading zero.

					t - AM/PM but only the first letter. 
					tt - AM/PM ( a.m. / p.m.)

					y - Year with out century and leading zero.
					yy - Year with out century, with leading zero.
					yyyy - Year with century.

					zz - Time zone off set with +/-.		 

		*/

		public static readonly string[] FormatStrings = new[]
			    {
				    "HH:mm", // 06:30
				    "hh:mm tt", // 06:30 AM
				    "H:mm", // 6:30
				    "h:mm tt", // 6:30 AM
				    "HH:mm:ss" // 06:30:07

			    };


		/*
			******************************************************************
			NOTE: These formats are used with Time Span Format values
			https://msdn.microsoft.com/en-us/library/ee372287(v=vs.110).aspx
			******************************************************************

		*/

		public static readonly string[] TimeSpanFormats = new[]
		{
			"HH:mm:ss.fff"
		};

		// ReSharper restore InconsistentNaming

	}
}