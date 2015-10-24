using System;
using System.Text;
using CmdrX.Constants;
using CmdrX.GlobalConstants;
using CmdrX.Helpers;

namespace CmdrX.Models
{
	public class ConsoleCommandDto
	{
		private bool _disposed;
		private ConsoleExecutorDto _cmdExeDto;

		#region CommandExeControl
		private string _commandDisplayName = string.Empty;

		public string CommandDisplayName
		{
			get { return _commandDisplayName; }

			set
			{
				_commandDisplayName = StringHelper.TrimStringEnds(value);
			}

		}

		public ConsoleCommandType CommandType { get; set; } = ConsoleCommandType.None;

		private string _commandOutputLogFilePathBaseName = string.Empty;

		public string CommandOutputLogFilePathBaseName
		{
			get { return _commandOutputLogFilePathBaseName; }

			set { _commandOutputLogFilePathBaseName = StringHelper.TrimStringEnds(value); }
		}


		public decimal CommandTimeOutInMinutes { get; set; }

		public int CommandTimeOutInMiliseconds => (int) (CommandTimeOutInMinutes*60000.0M);

		#endregion CommandExeControl

		#region CommandElements

		private string _consoleCommandExecutor = string.Empty;

		public string ConsoleCommandExecutor
		{
			get { return _consoleCommandExecutor;}

			set
			{
				_consoleCommandExecutor = StringHelper.TrimStringEnds(value);
			}
		}

		private string _consoleCommandExeArguments = string.Empty;
        public string ConsoleCommandExeArguments {

			get { return _consoleCommandExeArguments; }

	        set
	        {
		        _consoleCommandExeArguments = StringHelper.TrimStringEnds(value);
			}
		}

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

		#endregion CommandElements

#region CommandExeParameters

		public string ProcFileNameCommand { get; set; } = string.Empty;

		public string ProcFileArguments { get; set; } = string.Empty;

		public int NumberOfCommandElements { get; set; }

		public DateTime CommandStartTime { get; set; }

		public DateTime CommandExitTime { get; set; }

		public int CommandExitCode { get; set; }

#endregion CommandExeParameters

		public ErrorLogger ErrorMgr = new
			ErrorLogger(9923000,
						"ConsoleCommandDto",
						AppConstants.LoggingStatus,
						AppConstants.LoggingMode);

		public ConsoleCommandDto(ConsoleExecutorDto cmdExeDto)
		{
			_cmdExeDto = cmdExeDto;
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
					_cmdExeDto = null;
				}

				// Note disposing has been done.
				_disposed = true;

			}
		}

		public void NormalizeCommandParameters()
		{
			if (string.IsNullOrWhiteSpace(CommandOutputLogFilePathBaseName))
			{
				CommandOutputLogFilePathBaseName = _cmdExeDto.DefaultCommandOutputLogFilePathName;
			}

			if (CommandType == ConsoleCommandType.None)
			{
				CommandType = _cmdExeDto.DefaultConsoleCommandType;
			}

			if (string.IsNullOrWhiteSpace(CommandOutputLogFilePathBaseName))
			{
				CommandOutputLogFilePathBaseName = _cmdExeDto.DefaultCommandOutputLogFilePathName;
			}

			if (CommandTimeOutInMinutes < _cmdExeDto.CommandMinTimeOutInMinutes
				 || CommandTimeOutInMinutes > _cmdExeDto.CommandMaxTimeOutInMinutes)
			{
				CommandTimeOutInMinutes = _cmdExeDto.CommandDefaultTimeOutInMinutes;
			}

			ConfigureCommandExecutionSyntax();
		}

		public void ConfigureCommandExecutionSyntax()
		{
			var sb = new StringBuilder();
			bool hasCmdExecutor = false;
			bool hasFirstElement = false;
			NumberOfCommandElements = 0;

            if (!string.IsNullOrWhiteSpace(ConsoleCommandExecutor))
            {
	            ProcFileNameCommand = ConsoleCommandExecutor;
				hasCmdExecutor = true;
			}

			if (!string.IsNullOrWhiteSpace(ConsoleCommandExeArguments)
				 && hasCmdExecutor)
			{
				hasFirstElement = true;
				sb.Append(ConsoleCommandExeArguments);
			}

			if (!string.IsNullOrWhiteSpace(ExecutableTarget))
			{
				if (!hasCmdExecutor)
				{
					ProcFileNameCommand = ExecutableTarget;
				}
				else
				{
					if (hasFirstElement)
					{
						sb.Append(" ");
					}

					hasFirstElement = true;
					NumberOfCommandElements++;

					sb.Append(ExecutableTarget);
				}
			}

			if (!string.IsNullOrWhiteSpace(CommandToExecute))
			{
				if (hasFirstElement)
				{
					sb.Append(" ");
				}

				hasFirstElement = true;

				sb.Append(CommandToExecute);

				NumberOfCommandElements++;
			}

			if (!string.IsNullOrWhiteSpace(CommandModifier))
			{
				if (hasFirstElement)
				{
					sb.Append(" ");
				}

				hasFirstElement = true;

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

			ProcFileArguments = sb.ToString();
		}


	}
}