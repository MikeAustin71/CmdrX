using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using LibLoader.Constants;

namespace LibLoader.Helpers
{
	public class XmlValueExtractor
	{
		public int ExtractIntValue(XmlTextReader reader, int defaultInt)
		{
			int iVal;

			if (!Int32.TryParse(reader.ReadString(), out iVal))
			{
				iVal = defaultInt;
			}


			return iVal;
		}

		public decimal ExtractDecimalValue(XmlTextReader reader, decimal defaultDecimal)
		{
			decimal dVal;

			return !decimal.TryParse(reader.ReadString(), out dVal) ? defaultDecimal : dVal;
		}

		public string ExtractStringValue(XmlTextReader reader)
		{
			var result = reader.ReadString();

			return result ?? string.Empty;
		}

		public bool ExtractBooleanValue(XmlTextReader reader)
		{
			bool result;

			if (ExtractBoolFromOffOnString(reader.ReadString(), out result))
			{
				return result;
			}

			if (!Boolean.TryParse(reader.ReadString(), out result))
			{
				return false;
			}

			return result;

		}

		public bool ExtractBoolFromOffOnString(string strInVal, out bool result)
		{
			if (string.IsNullOrEmpty(strInVal))
			{
				result = false;

				return false;
			}


			if (strInVal.ToLower().Contains("on"))
			{
				result = true;

				return true;
			}

			if (strInVal.ToLower().Contains("off"))
			{
				result = false;

				return true;
			}


			result = false;

			return false;

		}

		public List<string> ExtractCommaDelimitedParms(XmlTextReader reader, out string outParms)
		{

			outParms = reader.ReadString();

			var parmList = new List<string>();

			if (string.IsNullOrEmpty(outParms))
			{
				outParms = string.Empty;

				return parmList;
			}

			parmList.AddRange(outParms.Split(',').Select(parm => parm.TrimStart().TrimEnd()));

			return parmList;

		}

		public ConsoleCommandType ExtractConsoleCommandType(XmlTextReader reader)
		{
			var strVal = ExtractStringValue(reader);

			if (string.IsNullOrWhiteSpace(strVal))
			{
				return ConsoleCommandType.Console;
			}

			strVal = strVal.ToLower();


			if (strVal.Contains("console"))
			{
				return ConsoleCommandType.Console;
			}


			if (strVal.Contains("copy"))
			{
				return ConsoleCommandType.Copy;
			}

			if (strVal.Contains("delete"))
			{
				return ConsoleCommandType.Delete;
			}

			return ConsoleCommandType.None;
		}
	}
}
