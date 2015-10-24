using System;
using System.Xml;
using CmdrX.Constants;
using CmdrX.GlobalConstants;
using CmdrX.Helpers;
using CmdrX.Models;

namespace CmdrX.Managers.XmlElementParsers
{
	public class ParseExecuteElements : IXmlElementParser
	{
		private XmlValueExtractor _xmlHlpr = new XmlValueExtractor();

		public ErrorLogger ErrorMgr = new
			ErrorLogger(1043000,
						"ParseExecuteElements",
						AppConstants.LoggingStatus,
						AppConstants.LoggingMode);

		public XmlElementType ElementType { get; private set; }

		public ParseExecuteElements(XmlElementType elementType) 
		{
			if (elementType != XmlElementType.ExecuteCommand)
			{
				var ex = new ArgumentException("Invalid XmlElement Type For this Xml Parser!");

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
												ref	ConsoleExecutorDto cmdExeDto)
		{
			if (reader.Name == "CommandDisplayName")
			{
				consoleCommand.CommandDisplayName = _xmlHlpr.ExtractStringValue(reader);

				return;
			}

			if (reader.Name == "ConsoleCommandType")
			{
				consoleCommand.CommandType = _xmlHlpr.ExtractConsoleCommandType(reader);

				return;
			}

			if (reader.Name == "CommandOutputLogFilePathBaseName")
			{
				consoleCommand.CommandOutputLogFilePathBaseName = _xmlHlpr.ExtractStringValue(reader);

				return;
			}

			if (reader.Name == "CommandTimeOutInMinutes")
			{
				consoleCommand.CommandTimeOutInMinutes = _xmlHlpr.ExtractDecimalValue(reader, 0.0M);

				return;
			}

			if (reader.Name == "DefaultConsoleCommandExecutor")
			{
				consoleCommand.ConsoleCommandExecutor = _xmlHlpr.ExtractStringValue(reader);

				return;
			}

			if (reader.Name == "ConsoleCommandExeArguments")
			{
				consoleCommand.ConsoleCommandExeArguments = _xmlHlpr.ExtractStringValue(reader);

				return;
			}

			if (reader.Name == "ExecuteInDir")
			{
				consoleCommand.ExecuteInDir = _xmlHlpr.ExtractStringValue(reader);

				return;
			}

			if (reader.Name == "ExecutableTarget")
			{
				consoleCommand.ExecutableTarget = _xmlHlpr.ExtractStringValue(reader);

				return;
			}

			if (reader.Name == "CommandToExecute")
			{
				consoleCommand.CommandToExecute = _xmlHlpr.ExtractStringValue(reader);

				return;
			}

			if (reader.Name == "CommandModifier")
			{
				consoleCommand.CommandModifier = _xmlHlpr.ExtractStringValue(reader);

				return;
			}

			if (reader.Name == "CommandArguments")
			{
				consoleCommand.CommandArguments = _xmlHlpr.ExtractStringValue(reader);
			}


		}

	}
}