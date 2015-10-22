using System;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using LibLoader.Constants;
using LibLoader.Helpers;
using LibLoader.Managers;

namespace LibLoader.Models
{
	public class ConsoleExecutorDto
	{
		private bool _disposed;

		private string _defaultConsoleCommandExecutor;
		private string _defaultConsoleCommandExeArgs;
		private string _defaultCmdConsoleLogFilePathName;
		private string _cmdConsoleLogFileErrorSuffix;
		private string _cmdConsoleLogFileTimeStamp;

		public string DefaultConsoleCommandExecutor
		{
			get { return _defaultConsoleCommandExecutor; }
			set { _defaultConsoleCommandExecutor = StringHelper.TrimStringEnds(value); }
		}

		public string DefaultConsoleCommandExeArgs
		{
			get { return _defaultConsoleCommandExeArgs; }
			set { _defaultConsoleCommandExeArgs = StringHelper.TrimStringEnds(value); }
		}

		public decimal CommandMinTimeOutInMinutes { get; set; }

		public decimal CommandMaxTimeOutInMinutes { get; set; }

		public decimal CommandDefaultTimeOutInMinutes { get; set; }


		public string DefaultCmdConsoleLogFilePathName
		{
			get { return _defaultCmdConsoleLogFilePathName; }
			set
			{
				_defaultCmdConsoleLogFilePathName = StringHelper.TrimStringEnds(value);
			}
		}

		public int AppLogRetentionInDays { get; set; }

		public string AppLogFileBaseNameOnly { get; set; }

		public string AppLogFileExtensionWithoutLeadingDot { get; set; }

		public string AppLogFileTimeStamp { get; set; }

		public string CmdConsoleLogFileErrorSuffix
		{
			get { return _cmdConsoleLogFileErrorSuffix; }
			set { _cmdConsoleLogFileErrorSuffix = StringHelper.TrimStringEnds(value); }
		}

		public string CmdConsoleLogFileTimeStamp
		{
			get { return _cmdConsoleLogFileTimeStamp; }
			set { _cmdConsoleLogFileTimeStamp = StringHelper.TrimStringEnds(value); }
		}

		public FileDto XmlCmdFileDto { get; set; }

		public ConsoleCommandType DefaultConsoleCommandType { get; set; }

		public AppicationLogMgr AppLogMgr { get; private set; }

		public void ConfigureParameters()
		{
			
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
					if (XmlCmdFileDto != null)
					{
						XmlCmdFileDto.Dispose();
						XmlCmdFileDto = null;
					}

				}


				// Note disposing has been done.
				_disposed = true;

			}
		}

		private void SetLogFilePathName(string defaultCmdConsoleLogFilePathName)
		{
			
		}



	}
}