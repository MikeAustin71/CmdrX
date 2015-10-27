using System;
using CmdrX.Models;

namespace CmdrX.Helpers
{
	public class XmlParameterConverter
	{
		private readonly string[,] _xmlParms = new string[,]
		{
			{"%(CURDATESTR)%", " "}
		};

		public XmlParameterConverter(ConsoleExecutorDto cmDto)
		{
			_xmlParms[0, 1] = cmDto.CmdConsoleLogFileTimeStamp;
		}

		public string RunConversion(string xml)
		{
			if (string.IsNullOrWhiteSpace(xml))
			{
				return string.Empty;
			}

			var boundary = _xmlParms.GetUpperBound(0);

			for(var i= 0; i <= boundary; i++)
			{
				if (xml.Contains(_xmlParms[i, 0]))
				{
					xml = xml.Replace(_xmlParms[i, 0], _xmlParms[i, 1]);
				}
			}

			return xml;
		}

	}
}