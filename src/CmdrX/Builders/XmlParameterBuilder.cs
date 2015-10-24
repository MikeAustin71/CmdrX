using System;
using System.Xml;
using CmdrX.Constants;
using CmdrX.GlobalConstants;
using CmdrX.Helpers;
using CmdrX.Managers.XmlElementParsers;
using CmdrX.Models;

namespace CmdrX.Builders
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

		private IXmlElementParser _elementParser;

		private ConsoleExecutorDto _cmdExeDto;

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

			if (reader.Name == "CommandFileHeader")
			{
				_elementParser = new ParseCmdFileHeaderElements(XmlElementType.CommandsHeader);
			}


			if (reader.Name == "ExectuteCommand")
			{
				_currentConsoleCommand = new ConsoleCommandDto(_cmdExeDto);
				_elementParser = new ParseExecuteElements(XmlElementType.ExecuteCommand);

				return;
			}


			_elementParser?.ExtractElementInfo(reader,  ref _currentConsoleCommand, ref _cmdExeDto);
		}

		private void ProcessEndElement(XmlTextReader reader)
		{
			if (reader.Name == "CommandFileHeader")
			{
				_elementParser = null;
			}

			// Command element closing
			if (reader.Name == "ExectuteCommand")
			{
				_currentConsoleCommand.NormalizeCommandParameters();
				_jobsGroupDto.Jobs.Add(_currentConsoleCommand);
				_cmdExeDto.ConfigureParameters();
				_elementParser = null;			
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