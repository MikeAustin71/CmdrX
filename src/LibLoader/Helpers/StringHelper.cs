using System;
using System.Collections.Generic;
using System.Text;

namespace LibLoader.Helpers
{
    public static class StringHelper
    {

		public static string MakeSingleCharString(char c, int len)
		{
			if (c == 0)
			{
				return string.Empty;
			}

			var sb = new StringBuilder();

			for (int i = 0; i < len; i++)
			{
				sb.Append(c);
			}

			return sb.ToString();
		}

		public static string RightJustifyString(string textIn, int totalFieldLen)
		{
			if (string.IsNullOrEmpty(textIn))
			{
				return string.Empty;
			}


			if (textIn.Length >= totalFieldLen)
			{
				return textIn;
			}

			var sFmt = "{0," + totalFieldLen + "}";

			string result;

			try
			{
				result = string.Format(sFmt, textIn);

			}
			catch
			{
				result = string.Empty;
			}


			return result;
		}

		public static string LeftJustifyString(string textIn, int totalFieldLen)
		{

			if (string.IsNullOrEmpty(textIn))
			{
				return string.Empty;
			}

			if (textIn.Length >= totalFieldLen)
			{
				return textIn;
			}

			var sFmt = "{0," + (totalFieldLen * -1) + "}";

			string result;

			try
			{
				result = string.Format(sFmt, textIn);

			}
			catch
			{
				result = string.Empty;
			}


			return result;
		}

		public static string CenterString(string textIn, int totalFieldLen)
		{
			if (string.IsNullOrEmpty(textIn))
			{
				return string.Empty;
			}

			if ((textIn.Length - 1) >= totalFieldLen)
			{
				return textIn;
			}

			var sFmt = "{0," + (totalFieldLen * -1) + "}";

			string result;

			try
			{
				// ReSharper disable once FormatStringProblem
				result = string.Format(sFmt, string.Format("{0," + ((totalFieldLen + textIn.Length) / 2) + "}", textIn));

			}
			catch
			{
				result = string.Empty;
			}


			return result;
		}


		public static int GetLastCharIndex(string str, char c)
	    {
		    if (str == null || c == 0)
		    {
			    return -1;
		    }

		    for (int i = str.Length - 1; i > -1; i--)
		    {
			    if (str[i] == c)
			    {
				    return i;
			    }
		    }

		    return -1;
	    }

	    public static int GetLastCharIndex(string str,  int startIdx, char c)
	    {
		    if (str == null || c == 0)
		    {
			    return -1;
		    }

		    for (int i = startIdx; i > -1; i--)
		    {
			    if (str[i] == c)
			    {
				    return i;
			    }
		    }

		    return -1;
	    }

	    public static string TrimStringEnds(string str)
	    {
		    if (string.IsNullOrWhiteSpace(str))
		    {
			    return string.Empty;
		    }

		    return str.TrimStart().TrimEnd();

	    }

	    public static string[] BreakLineAtIndex(string rawStr, int lineLengthMax)
	    {
		    if (lineLengthMax < 2 || string.IsNullOrWhiteSpace(rawStr))
		    {
			    return null;
		    }

		    var sChar = ' ';
			Queue<string> lines = new Queue<string>();

		    var maxIdx = lineLengthMax - 1;

			while (rawStr.Length > lineLengthMax)
			{
				var breakIdx = GetLastCharIndex(rawStr, maxIdx, sChar);

			    breakIdx = (breakIdx == -1) ? maxIdx : breakIdx;

				while (breakIdx > -1 && rawStr[breakIdx] == sChar)
				{
					breakIdx--;
				}

				lines.Enqueue(rawStr.Substring(0, ++breakIdx));

				breakIdx++;

				while(breakIdx < rawStr.Length && rawStr[breakIdx] == sChar)
				{
					breakIdx++;
				}
			
			    rawStr = rawStr.Substring(breakIdx);
		    }

			lines.Enqueue(rawStr);

		    return lines.ToArray();
	    }

		public static string RemoveCarriageReturns(string rawStr)
		{
			var chars = new Queue<char>();
			
			for (var i = 0; i < rawStr.Length; i++)
			{
				for (int x = 0; x < Environment.NewLine.Length; x++)
				{
					while ( i < rawStr.Length && rawStr[i] == Environment.NewLine[x])
					{
						i++;
					}

					if (i >= rawStr.Length)
					{
						return new string(chars.ToArray());
					}
				}

				chars.Enqueue(rawStr[i]);
			}

			return new string(chars.ToArray());
		}

	}
}