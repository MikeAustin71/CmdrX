﻿using System;
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
						AppConstants.LoggingMode);


		private readonly XmlTextReader _xmlReader;

		private JobsGroupDto _jobsGroupDto;

		private ConsoleCommandDto _currentConsoleCommand = new ConsoleCommandDto();

		private XmlValueExtractor _xmlHlpr = new XmlValueExtractor();

		public XmlParameterBuilder(FileDto xmlCommandFile)
		{

			if (xmlCommandFile?.FileXinfo == null || !xmlCommandFile.FileXinfo.Exists)
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
				_xmlReader = new XmlTextReader(xmlCommandFile.FullPathAndFileName);
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

			_jobsGroupDto = new JobsGroupDto(xmlCommandFile.FileXinfo.Name + " Commands");
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
				_currentConsoleCommand = new ConsoleCommandDto();

				return;
			}

			if (reader.Name == "CommandDisplayName")
			{
				_currentConsoleCommand.CommandDisplayName = _xmlHlpr.ExtractStringValue(reader);

				return;
			}

			if (reader.Name == "ConsoleCommandType")
			{
				_currentConsoleCommand.ComandType = _xmlHlpr.ExtractConsoleCommandType(reader);

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