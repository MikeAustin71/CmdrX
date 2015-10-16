using System;
using LibLoader.Constants;

namespace LibLoader.Models
{
	public class ConsoleCommandDto
	{
		public string CommandDisplayName { get; set; } = string.Empty;
		public ConsoleCommandType ComandType { get; set; } = ConsoleCommandType.Console;
		public string ExecuteInDir { get; set; } = string.Empty;
		public string ExecutableTarget { get; set; } = string.Empty;
		public string CommandToExecute { get; set; } = string.Empty;
		public string CommandModifier { get; set; } = string.Empty;
		public string CommandArguments { get; set; } = string.Empty;
		public string CommandLineExecutionSyntax { get; set; } = string.Empty;
		public string OutputCmdLogFile { get; set; } = string.Empty;

	}
}