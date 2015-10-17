using System;
using System.Text;
using LibLoader.Constants;
using LibLoader.GlobalConstants;
using LibLoader.Helpers;

namespace LibLoader.Models
{
	public class ConsoleCommandDto
	{
		private bool _disposed;

		public FileDto OutputCmdLogFileDto { get; private set; }

		private string _commandDisplayName = string.Empty;

		public string CommandDisplayName
		{
			get { return _commandDisplayName; }

			set
			{
				_commandDisplayName = StringHelper.TrimStringEnds(value);
			}

		}

		public ConsoleCommandType ComandType { get; set; } = ConsoleCommandType.Console;

		private string _executeInDir = string.Empty;

		public string ExecuteInDir
		{
			get { return _executeInDir; }

			set
			{
				_executeInDir = StringHelper.TrimStringEnds(value);
			}
		}

		private string _executableTarget = string.Empty;

		public string ExecutableTarget
		{
			get { return _executableTarget; }

			set
			{
				_executableTarget = StringHelper.TrimStringEnds(value);
			}

		}

		private string _commandToExecute = string.Empty;

		public string CommandToExecute
		{
			get { return _commandToExecute; }

			set
			{
				_commandToExecute = StringHelper.TrimStringEnds(value);
			}

		}

		private string _commandModifier = string.Empty;

		public string CommandModifier
		{
			get { return _commandModifier; }

			set
			{
				_commandModifier = StringHelper.TrimStringEnds(value);
			}
		}

		private string _commandArguments = string.Empty;

		public string CommandArguments
		{
			get { return _commandArguments; }

			set
			{
				_commandArguments = StringHelper.TrimStringEnds(value);
			}
		}


		public string CommandLineExecutionSyntax { get; set; } = string.Empty;

		private string _outputCmdLogFile = string.Empty;

		public string OutputCmdLogFile
		{
			get { return _outputCmdLogFile; }

			set
			{
				_outputCmdLogFile = StringHelper.TrimStringEnds(value);

			}
		}

		public void Dispose()
		{
			Dispose(true);
			// This object will be cleaned up by the Dispose method.
			// Therefore, you should call GC.SupressFinalize to
			// take this object off the finalization queue
			// and prevent finalization code for this object
			// from executing a second time.
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			// Check to see if Dispose has already been called.
			if (!_disposed)
			{
				// If disposing equals true, dispose all managed
				// and unmanaged resources.
				if (disposing)
				{
					// Dispose managed resources.
					if (OutputCmdLogFileDto != null)
					{
						OutputCmdLogFileDto.Dispose();
						OutputCmdLogFileDto = null;
					}

				}

				// Note disposing has been done.
				_disposed = true;

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

			OutputCmdLogFileDto = new FileDto(outputDirCmdFileName);

			return true;
		}
	}
}