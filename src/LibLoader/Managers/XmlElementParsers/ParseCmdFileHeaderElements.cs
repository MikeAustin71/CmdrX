using System;
using System.Xml;
using LibLoader.Constants;
using LibLoader.GlobalConstants;
using LibLoader.Helpers;
using LibLoader.Models;

namespace LibLoader.Managers.XmlElementParsers
{
	public class ParseCmdFileHeaderElements : IXmlElementParser
	{

		private XmlValueExtractor _xmlHlpr = new XmlValueExtractor();

		public ErrorLogger ErrorMgr = new
			ErrorLogger(2153000,
				"ParseCmdFileHeaderElements",
				AppConstants.LoggingStatus,
				AppConstants.LoggingMode);

		public XmlElementType ElementType { get; private set; }

		public ParseCmdFileHeaderElements(XmlElementType elementType)
		{
			if (elementType != XmlElementType.CommandsHeader)
			{
				var ex = new ArgumentException("Invalid XmlElement Type For this Xml Cmd Header Parser!");

				var err = new FileOpsErrorMessageDto
				{
					DirectoryPath = string.Empty,
					ErrId = 1,
					ErrorMessage = ex.Message,
					ErrSourceMethod = "Constructor()",
					ErrException = ex,
					FileName = string.Empty,
					LoggerLevel = LogLevel.FATAL
				};

				ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
				ErrorMgr.WriteErrorMsg(err);

				throw ex;
			}


			ElementType = elementType;
		}

		public void ExtractElementInfo(XmlTextReader reader, 
						ref ConsoleCommandDto consoleCommand, 
							ref ConsoleExecutorDto cmdExeDto)
		{

			if (reader.Name == "DefaultLogFileRetentionInDays")
			{
				cmdExeDto.AppLogRetentionInDays = _xmlHlpr.ExtractIntValue(reader, -1);

				return;
			}

			if (reader.Name == "DefaultCommandExeDirectory")
			{
				var dir = _xmlHlpr.ExtractStringValue(reader);

				cmdExeDto.SetDefaultCommandExeDirectory(dir);

				return;
			}

			if (reader.Name == "DefaultCommandOutputLogFilePathName")
			{
				var filePath = _xmlHlpr.ExtractStringValue(reader);

				cmdExeDto.SetDefaultCommandExeDirectory(filePath);
			}

		}

	}
}