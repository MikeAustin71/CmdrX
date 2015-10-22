using System;
using System.Xml;
using LibLoader.Constants;
using LibLoader.GlobalConstants;
using LibLoader.Helpers;
using LibLoader.Models;

namespace LibLoader.Builders
{
	public class XmlParameterBuilder
	{
		public static ErrorLogger ErrorMgr = new
			ErrorLogger(925000,
				"XmlParameterBuilder",
				AppConstants.LoggingStatus,
				AppConstants.LoggingMode,
				false);


		private readonly XmlTextReader _xmlReader;

		private JobsGroupDto _jobsGroupDto;

		private ConsoleCommandDto _currentConsoleCommand;

		private XmlValueExtractor _xmlHlpr = new XmlValueExtractor();

		private readonly ConsoleExecutorDto _cmdExeDto;

		public XmlParameterBuilder(ConsoleExecutorDto cmdExeDto)
		{

			if (cmdExeDto?.XmlCmdFileDto?.FileXinfo == null || !cmdExeDto.XmlCmdFileDto.FileXinfo.Exists)
			{
				var err = new FileOpsErrorMessageDto
				{
					DirectoryPath = String.Empty,
					ErrId = 1,
					ErrorMessage = "Xml Command File is Invalid!",
					ErrSourceMethod = "Constructor()",
					FileName = string.Empty,
					LoggerLevel = LogLevel.FATAL
				};

				ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
				ErrorMgr.WriteErrorMsg(err);

				throw new ArgumentException("Invalid Xml Command File!");

			}

			try
			{
				_xmlReader = new XmlTextReader(cmdExeDto.XmlCmdFileDto.FileXinfo.FullName);
            }
			catch (Exception e)
			{
				var err = new FileOpsErrorMessageDto
				{
					DirectoryPath = String.Empty,
					ErrId = 1,
					ErrorMessage = "Xml Command File threw exception while opening!",
					ErrSourceMethod = "Constructor()",
					ErrException = e,
					FileName = string.Empty,
					LoggerLevel = LogLevel.FATAL
				};

				ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
				ErrorMgr.WriteErrorMsg(err);

				throw new ArgumentException("Xml Command File Failed To Open!");

			}

			_jobsGroupDto = new JobsGroupDto(cmdExeDto.XmlCmdFileDto.FileXinfo.Name + " Commands");

			_cmdExeDto = cmdExeDto;
		}

		public JobsGroupDto BuildParmsFromXml()
		{
			

			while (_xmlReader.Read())
			{
				switch (_xmlReader.NodeType)
				{
					case XmlNodeType.Element:

						ProcessElement(_xmlReader);

						break;

					case XmlNodeType.EndElement:

						ProcessEndElement(_xmlReader);

						break;

				}
			}

			_xmlReader.Close();
			_xmlReader.Dispose();

			return _jobsGroupDto;
		}

		private void ProcessElement(XmlTextReader reader)
		{
			if (reader.Name == "Commands")
			{
				return;
			}

			if (reader.Name == "ExectuteCommand")
			{
				_currentConsoleCommand = new ConsoleCommandDto(_cmdExeDto);

				return;
			}

			if (reader.Name == "CommandDisplayName")
			{
				_currentConsoleCommand.CommandDisplayName = _xmlHlpr.ExtractStringValue(reader);

				return;
			}

			if (reader.Name == "ConsoleCommandType")
			{
				_currentConsoleCommand.CommandType = _xmlHlpr.ExtractConsoleCommandType(reader);

				return;
			}

			if (reader.Name == "CommandOutputLogFilePathBaseName")
			{
				_currentConsoleCommand.CommandOutputLogFilePathBaseName = _xmlHlpr.ExtractStringValue(reader);

				return;
			}

			if (reader.Name == "CommandTimeOutInMinutes")
			{
				_currentConsoleCommand.CommandTimeOutInMinutes = _xmlHlpr.ExtractDecimalValue(reader, 0.0M);

				return;
			}

			if (reader.Name == "DefaultConsoleCommandExecutor")
			{
				_currentConsoleCommand.ConsoleCommandExecutor = _xmlHlpr.ExtractStringValue(reader);

				return;
			}
			
			if (reader.Name == "ConsoleCommandExeArguments")
			{
				_currentConsoleCommand.ConsoleCommandExeArguments = _xmlHlpr.ExtractStringValue(reader);

				return;
			}
			
			if (reader.Name == "ExecuteInDir")
			{
				_currentConsoleCommand.ExecuteInDir = _xmlHlpr.ExtractStringValue(reader);

				return;
			}

			if (reader.Name == "ExecutableTarget")
			{
				_currentConsoleCommand.ExecutableTarget = _xmlHlpr.ExtractStringValue(reader);

				return;
			}

			if (reader.Name == "CommandToExecute")
			{
				_currentConsoleCommand.CommandToExecute = _xmlHlpr.ExtractStringValue(reader);

				return;
			}

			if (reader.Name == "CommandModifier")
			{
				_currentConsoleCommand.CommandModifier = _xmlHlpr.ExtractStringValue(reader);

				return;
			}

			if (reader.Name == "CommandArguments")
			{
				_currentConsoleCommand.CommandArguments = _xmlHlpr.ExtractStringValue(reader);

				return;
			}



		}

		private void ProcessEndElement(XmlTextReader reader)
		{
			// Command element closing
			if (reader.Name == "ExectuteCommand")
			{
				_currentConsoleCommand.NormalizeCommandParameters();
				_jobsGroupDto.Jobs.Add(_currentConsoleCommand);				
				return;
			}

			// Last element closing
			if (reader.Name == "Commands")
			{
				_jobsGroupDto.NumberOfJobs = _jobsGroupDto.Jobs.Count;
			}

		}

	}
}