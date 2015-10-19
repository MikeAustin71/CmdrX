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

		public int NumberOfCommandElements { get; set; }

		private string _outputCmdLogFileBaseName = string.Empty;

		public string OutputCmdLogFileBaseName
		{
			get { return _outputCmdLogFileBaseName; }

			set
			{
				_outputCmdLogFileBaseName = StringHelper.TrimStringEnds(value);

			}
		}

		public DateTime CommandStartTime { get; set; }

		public DateTime CommandExitTime { get; set; }

		public int CommandExitCode { get; set; }

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


		public string GetCommandExecutionSyntax(string cmdExeArguments)
		{
			var sb = new StringBuilder();
			bool hasFirstElement = false;
			NumberOfCommandElements = 0;

            if (!string.IsNullOrWhiteSpace(cmdExeArguments))
			{
				sb.Append(StringHelper.TrimStringEnds(cmdExeArguments));

				hasFirstElement = true;
			}


			if (!string.IsNullOrWhiteSpace(ExecutableTarget))
			{
				if (hasFirstElement)
				{
					sb.Append(" ");
				}
				else
				{
					hasFirstElement = true;
				}

				sb.Append(ExecutableTarget);

				NumberOfCommandElements++;
			}

			if (!string.IsNullOrWhiteSpace(CommandToExecute))
			{
				if (hasFirstElement)
				{
					sb.Append(" ");
				}
				else
				{
					hasFirstElement = true;
				}

				sb.Append(CommandToExecute);

				NumberOfCommandElements++;
			}

			if (!string.IsNullOrWhiteSpace(CommandModifier))
			{
				if (hasFirstElement)
				{
					sb.Append(" ");
				}
				else
				{
					hasFirstElement = true;
				}

				sb.Append(CommandModifier);

				NumberOfCommandElements++;
			}

			if (!string.IsNullOrWhiteSpace(CommandArguments))
			{
				if (hasFirstElement)
				{
					sb.Append(" ");
				}

				sb.Append(CommandArguments);

				NumberOfCommandElements++;
			}

			CommandLineExecutionSyntax = StringHelper.TrimStringEnds(sb.ToString());

            return (CommandLineExecutionSyntax);
		}

		public bool SetOutputCmdLogFile(string outputDirCmdFileName)
		{
			if (string.IsNullOrWhiteSpace(outputDirCmdFileName))
			{
				outputDirCmdFileName = AppConstants.CommandOutputLogFileBaseName;
			}

			OutputCmdLogFileDto = new FileDto(outputDirCmdFileName);

			return true;
		}
	}
}