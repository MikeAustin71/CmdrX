using System;
using System.Text;
using LibLoader.Constants;
using LibLoader.GlobalConstants;
using LibLoader.Helpers;
using LibLoader.Models;

namespace LibLoader.Builders
{
	public class ExecuteCommandBuilder
	{
		public static ErrorLogger ErrorMgr = new
			ErrorLogger(1345000,
						"XmlParameterBuilder",
						AppConstants.LoggingStatus,
						AppConstants.LoggingMode);

		public ConsoleCommandDto ExecuteCommand { get; private set; }

		public ExecuteCommandBuilder(ConsoleCommandDto cmdDto )
		{
			if (cmdDto == null
			    || cmdDto.ExecutableTarget == string.Empty)
			{
				var err = new FileOpsErrorMessageDto
				{
					DirectoryPath = string.Empty,
					ErrId = 1,
					ErrorMessage = "Console Command Dto is Invalid!",
					ErrSourceMethod = "Constructor()",
					FileName = string.Empty,
					LoggerLevel = LogLevel.FATAL
				};

				ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
				ErrorMgr.WriteErrorMsg(err);

				throw new ArgumentException("Invalid Xml Command File!");

			}

			ExecuteCommand = cmdDto;
		}

		public ConsoleCommandDto Build()
		{
			if (string.IsNullOrWhiteSpace(ExecuteCommand.ExecutableTarget))
			{
				var msg = "Console Command Executable Target Is Empty! Command Display Name: " + ExecuteCommand.CommandDisplayName;
                var err = new FileOpsErrorMessageDto
				{
					DirectoryPath = string.Empty,
					ErrId = 2,
					ErrorMessage = msg,
					ErrSourceMethod = "Build()",
					CommandName = ExecuteCommand.CommandDisplayName,
					LoggerLevel = LogLevel.FATAL
				};

				ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
				ErrorMgr.WriteErrorMsg(err);

				throw new ArgumentException(msg);

			}

			return AssembleExeCommand();
		}

		private ConsoleCommandDto AssembleExeCommand()
		{
			var sb = new StringBuilder(1024);

			if (!string.IsNullOrWhiteSpace(ExecuteCommand.ExecutableTarget))
			{
				sb.Append(ExecuteCommand.ExecutableTarget.TrimStart().TrimEnd());
				
			}

			if (!string.IsNullOrWhiteSpace(ExecuteCommand.CommandToExecute))
			{
				sb.Append(" " + ExecuteCommand.CommandToExecute.TrimStart().TrimEnd());

			}

			if (!string.IsNullOrWhiteSpace(ExecuteCommand.CommandModifier))
			{
				sb.Append(" " + ExecuteCommand.CommandModifier.TrimStart().TrimEnd());

			}

			if (!string.IsNullOrWhiteSpace(ExecuteCommand.CommandArguments))
			{
				sb.Append(" " + ExecuteCommand.CommandArguments.TrimStart().TrimEnd());

			}

			ExecuteCommand.CommandLineExecutionSyntax = sb.ToString();

			return ExecuteCommand;
		}
	}
}