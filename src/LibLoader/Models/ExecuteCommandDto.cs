using System.Text;

namespace LibLoader.Models
{
	public class ExecuteCommandDto
	{
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

		public string AssembleComletedCommand()
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

			return sb.ToString();
		}
	}
}