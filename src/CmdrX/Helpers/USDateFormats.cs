namespace CmdrX.Helpers
{
	// ReSharper disable InconsistentNaming
	public static class USDateFormats
	// ReSharper restore InconsistentNaming
	{

		/*
					d - Numeric day of the month without a leading zero.
					dd - Numeric day of the month with a leading zero.
					ddd - Abbreviated name of the day of the week.
					dddd - Full name of the day of the week.

					f,ff,fff,ffff,fffff,ffffff,fffffff - 
						Fraction of a second. The more Fs the higher the precision.

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

		       z - Time zone off set 
					zz - Time zone off set with +/-.		 
		     zzz - Time zone offset

		*/

		public static readonly string[] FormatStrings = new[]
		{
			"M/d/yyyy",									// 9/9/2012 Short Date String
			"M/d/yy",									// 9/9/12
			"MM/dd/yyyy",								// 08/22/2006
			"M/d/yyyy",								    // 8/3/2006
			"M/d/yy",								    // 8/3/06
			"MM/dd/yy",								    // 08/03/06
			"dddd, dd MMMM yyyy",						// Tuesday, 22 August 2006
			"dddd, dd MMMM yyyy HH:mm",					// Tuesday, 22 August 2006 06:30
			"dddd, dd MMMM yyyy hh:mm tt",				// Tuesday, 22 August 2006 06:30 AM
			"dddd, dd MMMM yyyy H:mm",					// Tuesday, 22 August 2006 6:30
			"dddd, dd MMMM yyyy h:mm tt",				// Tuesday, 22 August 2006 6:30 AM
			"dddd, dd MMMM yyyy HH:mm:ss",				// Tuesday, 22 August 2006 06:30:07
			"yyyy-MM-dd HH:mm:ss tt",
			"yyyy-MM-dd HH:mm:ss",
			"yyyy-M-dd H:mm:ss tt",
			"yyyy-M-dd H:mm:ss",
			"yyyy-M-d HH:mm:ss",
			"M-d-yy HH:mm tt",
			"yyyy-MM-dd HH:mm:ss",
			"yyyy-MM-ddTHH:mm:ssZ",
			"yyyy-MM-dd HH:mm",							//  
			"yyyy-MM-dd HH:mm:ss zzz",
			"yyyy-MM-dd HH:mm:ss zz",
			"yyyy-MM-dd HH:mm:ss z",
			"yyyy-MM-dd hh:mm tt",						//  
			"yyyy-MM-dd H:mm",							//  
			"yyyy-MM-dd HH:mm:ss.fff tt",						// 
 			"yyyy-MM-dd HH:mm:ss.fff",
 			"yyyy-MM-dd HH:mm:ss fff",
			"MM-dd-yyyy",								// 09/01/2012
			"M-d-yyyy",									// 9-9-2012 Short Date String
			"M-d-yy",									// 9-9-12 
			"MM-dd-yy",									// 09/09/12
			"MM-dd-yyyy HH:mm:ss tt",
			"MM-dd-yyyy HH:mm:ss",
			"M-dd-yyyy H:mm:ss tt",
			"M-dd-yyyy H:mm:ss",
			"MM-dd-yyyy HH:mm",							//  08-22-2006 06:30
			"MM-dd-yyyy hh:mm tt",						//  08-22-2006 06:30 AM
			"MM-dd-yyyy H:mm",							//  08-22-2006 6:30
			"M-d-yyyy H:mm",							//  8-2-2006 6:30
			"M-d-yy H:mm",								//  8-2-06 6:30
			"MM-dd-yyyy HH:mm:ss",						//  08-22-2006 06:30:07
			"M-d-yyyy HH:mm:ss",						//  8-2-2006 06:30:07
			"M-d-yy HH:mm:ss",							//  8-2-06 06:30:07
			"MM/dd/yyyy HH:mm:ss tt",
			"MM/dd/yyyy HH:mm:ss",
			"M/dd/yyyy H:mm:ss tt",
			"M/dd/yyyy H:mm:ss",
			"MM/dd/yyyy HH:mm",							//  08/22/2006 06:30
			"MM/dd/yyyy hh:mm tt",						//  08/22/2006 06:30 AM
			"M/dd/yyyy h:mm:ss tt",						// 12/11/2012 8:23:35 PM
			"M/d/yyyy h:mm:ss tt",                      // 9/9/2012 7:08:05 AM
			"MM/dd/yyyy H:mm",							//  8/22/2006 6:30
			"M/d/yyyy H:mm",							//  8/2/2006 6:30
			"M/d/yy H:mm",								//  8/2/06 6:30
			"MM/dd/yyyy HH:mm:ss",						//  08/22/2006 06:30:07
			"M/d/yyyy HH:mm:ss",						//  8/2/2006 06:30:07
			"M/d/yy HH:mm:ss",							//  8/2/06 06:30:07
			"MMMM dd",									//	August 22
			"yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffK",	//  2006-08-22T06:30:07.7199222-04:00
			"ddd, dd MMM yyyy HH':'mm':'ss 'GMT'",		//  Tue, 22 Aug 2006 06:30:07 GMT
			"yyyy'-'MM'-'dd'T'HH':'mm':'ss",			//  2006-08-22T06:30:07
			"yyyy'-'MM'-'dd HH':'mm':'ss'Z'",			//  2006-08-22 06:30:07Z
			"dddd, dd MMMM yyyy HH:mm:ss",				//	Tuesday, 22 August 2006 06:30:07
			"dddd, MMMM dd, yyyy"						//  Sunday, September 09, 2012


		};


	}
}