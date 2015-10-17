using System;
using System.Text;

namespace LibLoader.Helpers
{
    public static class StringHelper
    {
        public static string MakeSingleCharString(char c, int len)
        {
            if(c==0)
            {
                return String.Empty;
            }

            var sb = new StringBuilder();

            for(int i=0; i < len; i++)
            {
                sb.Append(c);
            }

            return sb.ToString();
        }

        public static string RightJustifyString(string textIn, int totalFieldLen)
        {
            if (String.IsNullOrEmpty(textIn))
            {
                return String.Empty;
            }


            if (textIn.Length >= totalFieldLen)
            {
                return textIn;
            }

            var sFmt = "{0," + totalFieldLen + "}";

            string result;

            try
            {
                result = String.Format(sFmt, textIn);

            }
            catch
            {
                result = String.Empty;
            }


            return result;
        }

        public static string LeftJustifyString(string textIn, int totalFieldLen)
        {

            if (String.IsNullOrEmpty(textIn))
            {
                return String.Empty;
            }

            if (textIn.Length >= totalFieldLen)
            {
                return textIn;
            }

            var sFmt = "{0," + (totalFieldLen * -1) + "}";

            string result;

            try
            {
                result = String.Format(sFmt, textIn);

            }
            catch
            {
                result = String.Empty;
            }


            return result;
        }

        public static string CenterString(string textIn, int totalFieldLen)
        {
            if(String.IsNullOrEmpty(textIn))
            {
                return String.Empty;
            }

            if ((textIn.Length - 1) >= totalFieldLen)
            {
                return textIn;
            }

            var sFmt = "{0," + (totalFieldLen * -1) + "}";

            string result;

            try
            {
                result = String.Format(sFmt, String.Format("{{0," + ((totalFieldLen + textIn.Length) / 2) + "}}", textIn));

            }
            catch
            {
                result = String.Empty;
            }


            return result;
        }

	    public static int GetLastCharIndex(string str, char c)
	    {
		    if (str == null || c == null)
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

	    public static string TrimStringEnds(string str)
	    {
		    if (string.IsNullOrWhiteSpace(str))
		    {
			    return string.Empty;
		    }

		    return str.TrimStart().TrimEnd();

	    }
    }
}