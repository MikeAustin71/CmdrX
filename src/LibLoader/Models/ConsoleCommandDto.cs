using System;
using System.IO;
using System.Text;
using LibLoader.Constants;
using LibLoader.GlobalConstants;
using LibLoader.Helpers;

namespace LibLoader.Models
{
	public class ConsoleCommandDto
	{
		private string _commandDisplayName = string.Empty;

		public DirectoryDto OutputCmdLogDirectoryDto { get; private set; }
		public FileDto OutputCmdLogFileDto { get; private set; }

		public string CommandDisplayName
		{
			get { return _commandDisplayName; }

			set
			{
				if (string.IsNullOrWhiteSpace(value))
				{
					_commandDisplayName = string.Empty;
					return;
				}

				_commandDisplayName = value.TrimStart().TrimEnd();
			}

		}

		public ConsoleCommandType ComandType { get; set; } = ConsoleCommandType.Console;

		private string _executeInDir = string.Empty;

		public string ExecuteInDir
		{
			get { return _executeInDir; }

			set
			{
				if (string.IsNullOrWhiteSpace(value))
				{
					_executeInDir = string.Empty;
					return;
				}

				_executeInDir = value.TrimStart().TrimEnd();
			}
		}

		private string _executableTarget = string.Empty;

		public string ExecutableTarget
		{
			get { return _executableTarget; }

			set
			{
				if (string.IsNullOrWhiteSpace(value))
				{
					_executableTarget = string.Empty;
					return;
				}

				_executableTarget = value.TrimStart().TrimEnd();
			}

		}

		private string _commandToExecute = string.Empty;

		public string CommandToExecute
		{
			get { return _commandToExecute; }

			set
			{
				if (string.IsNullOrWhiteSpace(value))
				{
					_commandToExecute = string.Empty;
					return;
				}

				_commandToExecute = value.TrimStart().TrimEnd();
			}

		}

		private string _commandModifier = string.Empty;

		public string CommandModifier
		{
			get { return _commandModifier; }

			set
			{
				if (string.IsNullOrWhiteSpace(value))
				{
					_commandModifier = string.Empty;
					return;
				}

				_commandModifier = value.TrimStart().TrimEnd();
			}
		}

		private string _commandArguments = string.Empty;

		public string CommandArguments
		{
			get { return _commandArguments; }

			set
			{
				if (string.IsNullOrWhiteSpace(value))
				{
					_commandArguments = string.Empty;
					return;
				}

				_commandArguments = value.TrimStart().TrimEnd();
			}
		}


		public string CommandLineExecutionSyntax { get; set; } = string.Empty;

		private string _outputCmdLogFile = string.Empty;
		public string OutputCmdLogFile
		{
			get { return _outputCmdLogFile; }

			set
			{
				if (string.IsNullOrWhiteSpace(value))
				{
					_outputCmdLogFile = string.Empty;
					return;
				}

				_outputCmdLogFile = value.TrimStart().TrimEnd();
			}
		}

		public string AssembleCommandLineSyntax()
		{
			var sb = new StringBuilder();

			if (!string.IsNullOrWhiteSpace(ExecutableTarget))
			{
				sb.Append(ExecutableTarget);
			}

			if (!string.IsNullOrWhiteSpace(CommandToExecute))
			{
				sb.Append(" " + CommandToExecute);
			}

			if (!string.IsNullOrWhiteSpace(CommandModifier))
			{
				sb.Append(" " + CommandModifier);
			}

			if (!string.IsNullOrWhiteSpace(CommandArguments))
			{
				sb.Append(" " + CommandArguments);
			}

			CommandLineExecutionSyntax = sb.ToString();

			return CommandLineExecutionSyntax;
		}

		public bool SetOutputCmdLogFile(string outputDirCmdFileName)
		{
			if (string.IsNullOrWhiteSpace(outputDirCmdFileName))
			{
				outputDirCmdFileName = AppConstants.CommandOutputLogFileName;
			}

			var dirName = Path.GetDirectoryName(outputDirCmdFileName);

			if (string.IsNullOrWhiteSpace(dirName))
			{
				OutputCmdLogDirectoryDto =
					DirectoryHelper.GetCurrentApplicationDirectoryLocation();

				OutputCmdLogFileDto = new FileDto(OutputCmdLogDirectoryDto, outputDirCmdFileName);
			}
			else
			{
				OutputCmdLogDirectoryDto = new DirectoryDto();
				OutputCmdLogFileDto = new FileDto(outputDirCmdFileName);

			}

			return true;
		}
	}
}