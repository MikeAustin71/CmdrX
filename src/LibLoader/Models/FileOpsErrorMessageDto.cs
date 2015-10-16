using System;
using LibLoader.Helpers;

namespace LibLoader.Models
{
    public class FileOpsErrorMessageDto
    {
	    public string ErrorMessage { get; set; } = string.Empty;
        public string ErrSourceClass { get; set; } = string.Empty;
		public string ErrSourceMethod { get; set; } = string.Empty;
		public string FileName { get; set; } = string.Empty;
	    public string CommandName { get; set; } = string.Empty;
		public string DirectoryPath { get; set; } = string.Empty;
		public string DirectoryPath2 { get; set; } = string.Empty;
		public string TargetPath { get; set; } = string.Empty;
		public int ErrId { get; set; }
		public Exception ErrException { get; set; }
		public LogLevel LoggerLevel { get; set; }

        public FileOpsErrorMessageDto()
        {
            ErrId = -1;
            LoggerLevel = LogLevel.INFO;
        }
    }
}