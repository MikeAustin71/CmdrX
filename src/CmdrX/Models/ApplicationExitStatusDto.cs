using System;
using System.Text;
using CmdrX.Helpers;

namespace CmdrX.Models
{
	public class ApplicationExitStatusDto
	{
		private const int Banner1Width = 60;
		private readonly string _newLine = Environment.NewLine;
		private readonly string _banner1 = StringHelper.MakeSingleCharString('*', Banner1Width);

		public bool IsDisplayCmdLineArgs { get; set; }

		private bool _isExceptionThrown;

		public bool IsExceptionThrown
		{
			get
			{
				return _isExceptionThrown;
			}
			set
			{
				_isExceptionThrown = value;

				if (_isExceptionThrown)
				{
					IsSuccessfulCompletion = false;
				}
			}
		}

		private bool _isFatalError;

		public bool IsFatalError
		{
			get
			{
				return _isFatalError; 
				
			}
			set
			{
				_isFatalError = value;

				if (_isFatalError)
				{
					IsTerminateApp = true;
					IsSuccessfulCompletion = false;
				}

			}
		}

		public bool IsTerminateApp { get; set; }


		private bool _isSuccessfulCompletion;

		public bool IsSuccessfulCompletion
		{
			get
			{
				return _isSuccessfulCompletion;
			}

			set
			{
				_isSuccessfulCompletion = value;

				if (_isSuccessfulCompletion)
				{
					_isFatalError = false;
					IsTerminateApp = false;
				}
			}
		}

		public FileOpsErrorMessageDto OpsError { get; set; }

		public bool IsExceptionThrown1
		{
			get
			{
				return _isExceptionThrown;
			}

			set
			{
				_isExceptionThrown = value;
			}
		}

		public bool IsExceptionThrown2
		{
			get
			{
				return _isExceptionThrown;
			}

			set
			{
				_isExceptionThrown = value;
			}
		}

		public StringBuilder WriteSuccessfulExitConsoleMessage(StringBuilder sb)
		{
			sb.Append(_newLine);
			sb.Append(_banner1 + _newLine);
			var s =$"CmdrX.exe job run successfully completed! Exit Code = " + Environment.ExitCode;
			sb.Append(StringHelper.CenterString(s, Banner1Width) + _newLine);
			sb.Append(_banner1 + _newLine);
			sb.Append(_newLine);
			sb.Append(_newLine);
			return sb;
		}

		public void WriteExitConsoleMessage()
		{
			var sb = new StringBuilder();

			if (IsExceptionThrown)
			{
				sb = WriteExceptionExitMsg(sb);
			}
			else if (IsFatalError)
			{
				sb = WriteFatalErrorExitMsg(sb);
			}
			else if (IsTerminateApp)
			{
				sb = WriteTerminateAppMsg(sb);
			}
			else
			{
				sb = WriteSuccessfulExitConsoleMessage(sb);
			}

			if (IsDisplayCmdLineArgs)
			{
				sb = GetDisplayCmdLineParms(sb);
			}

			Console.Write(sb);
		}

		private StringBuilder WriteExceptionExitMsg(StringBuilder sb)
		{
			sb.Append(_newLine);

			if (OpsError == null)
			{
				sb.Append("Missing Ops Error - ApplicationExitStatusDto:WriteFatalErrorExitMsg()" + _newLine);
				return sb;
			}

			sb.Append(_newLine);
			sb.Append(_banner1 + _newLine);
			var s = "CmdrX Terminated - Fatal Exception Thrown";
			sb.Append(StringHelper.CenterString(s, Banner1Width) + _newLine);
			sb.Append(_banner1 + _newLine);
			sb.Append($" ErrId: {OpsError.ErrId} {OpsError.ErrSourceClass}.{OpsError.ErrSourceMethod} Err Level: {OpsError.LoggerLevel}" + _newLine);
			if (!string.IsNullOrWhiteSpace(OpsError.JobName))
			{
				sb.Append($"Job Name: {OpsError.JobName} " + _newLine);
			}

			if (!string.IsNullOrWhiteSpace(OpsError.DirectoryPath))
			{
				sb.Append($"Directory: {OpsError.DirectoryPath} " + _newLine);

			}

			if (!string.IsNullOrWhiteSpace(OpsError.FileName))
			{
				sb.Append($"File Name: {OpsError.FileName} " + _newLine);
			}

			sb.Append(OpsError.ErrorMessage + _newLine);

			if (OpsError.ErrException != null)
			{
				
				sb = StringHelper.AddBreakLinesAtIndex("Exception: " + OpsError.ErrException.Message, Banner1Width, sb, true);

				if (OpsError.ErrException.InnerException != null)
				{
					sb = StringHelper.AddBreakLinesAtIndex("Inner Exception: " + OpsError.ErrException.InnerException.Message, Banner1Width, sb, true);

				}
			}

			sb.Append(_newLine);
			sb.Append(_banner1 + _newLine);

			return sb;
		}

		private StringBuilder WriteTerminateAppMsg(StringBuilder sb)
		{
			sb.Append(_newLine);
			if (OpsError == null)
			{
				sb.Append("Missing Ops Error - ApplicationExitStatusDto:WriteFatalErrorExitMsg()" + _newLine);
				return sb;
			}


			sb.Append(_newLine);
			var s = "CmdrX Application Termination";
			sb.Append(StringHelper.CenterString(s, Banner1Width) + _newLine);
			sb.Append(_banner1 + _newLine);
			sb.Append(OpsError.ErrorMessage + _newLine);
			sb.Append(_banner1 + _newLine);
			sb.Append(_newLine);

			return sb;

		}

		private StringBuilder WriteFatalErrorExitMsg(StringBuilder sb)
		{
			sb.Append(_newLine);
			if (OpsError == null)
			{
				sb.Append("Missing Ops Error - ApplicationExitStatusDto:WriteFatalErrorExitMsg()" + _newLine);
				return sb;
			}

			sb.Append(_newLine);
			var s = "CmdrX FATAL Error Termination";
			sb.Append(StringHelper.CenterString(s, Banner1Width) + _newLine);
			sb.Append(_banner1 + _newLine);
			sb.Append(OpsError.ErrorMessage + _newLine);
			sb.Append(_banner1 + _newLine);
			sb.Append(_newLine);

			return sb;
		}

		public StringBuilder GetDisplayCmdLineParms(StringBuilder sb)
		{
			sb.Append(_newLine);
			sb.Append(_newLine);
			sb.Append(StringHelper.CenterString("CmdrX Command Line Options", Banner1Width) + _newLine);
			sb.Append("CmdrX.exe [options] <ENTER>");
			sb.Append(_banner1 + _newLine);
			sb.Append("-xml=Source Directory" + _newLine);
			sb.Append("-l=logging On|Off (Default On)" + _newLine);
			sb.Append("-lr=Log Retention in Days (Example -lr=90) (Default 5)" + _newLine);
			sb.Append(_banner1 + _newLine);
			sb.Append(_newLine);

			return sb;
		}

	}
}