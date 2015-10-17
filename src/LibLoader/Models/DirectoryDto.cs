using System;
using System.IO;
using LibLoader.Constants;
using LibLoader.GlobalConstants;
using LibLoader.Helpers;

namespace LibLoader.Models
{
	/// <summary>
	/// Note Directories are stored WITHOUT Trailing Delimiters
	/// </summary>
    public class DirectoryDto
    {
		private bool _disposed;

		public ErrorLogger ErrorMgr = new
			ErrorLogger(2143000,
						"DirectoryDto",
						AppConstants.LoggingStatus,
						AppConstants.LoggingMode);



		public DirectoryInfo DirInfo { get; set; }
        public bool DirInfoIsValid { get; set; }
        public string DirectoryName { get; set; }

	    public DirectoryDto()
	    {
		    SetDirectoryEmpty();
	    }
			
	    public DirectoryDto(string directoryPath)
	    {
		    if (!ValidateInputDirString(directoryPath))
		    {
				var err = new FileOpsErrorMessageDto
				{
					DirectoryPath = directoryPath,
					ErrId = 10,
					ErrorMessage = "Directory Path Invalid! Dto set to empty.",
					ErrSourceMethod = "ValidateInputDirString()",
					FileName = string.Empty,
					LoggerLevel = LogLevel.ERROR
				};

				ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
				ErrorMgr.WriteErrorMsg(err);
				SetDirectoryEmpty();
			    return;
		    }

			SetDirectory(AnalyzeRawDirectoryPath(directoryPath));
	    }


		public DirectoryDto(FileInfo fInfo)
	    {
		    SetDirectory(fInfo.DirectoryName);
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
					if (DirInfo != null)
					{
						DirInfo = null;
						DirInfoIsValid = false;
					}

				}

				// Note disposing has been done.
				_disposed = true;

			}
		}

		public string GetDirectoryFullNameWithTrailingDelimiter()
		{
			if (DirInfo == null)
			{
				return string.Empty;
			}

			return PathHelper.AddDefaultTrailingDelimiter(DirInfo.FullName);
		}

		public override int GetHashCode()
		{
			return (DirInfo != null ? DirInfo.GetHashCode() : 0);
		}

		public override bool Equals(System.Object obj)
	    {
			// If parameter is null return false.
			if (obj == null)
			{
				return false;
			}

			// If parameter cannot be cast to Point return false.
			DirectoryDto p = obj as DirectoryDto;

			if ((System.Object)p == null)
			{
				return false;
			}

			if (this.DirInfo == null && p.DirInfo == null)
			{
				return true;
			}

		    if (this.DirInfo == null || p.DirInfo == null)
		    {
			    return false;
		    }

		    return this.DirInfo.FullName == p.DirInfo.FullName;
	    }

	    public static bool operator ==(DirectoryDto a, DirectoryDto b)
	    {

			if (object.ReferenceEquals(a, null))
			{
				return object.ReferenceEquals(b, null);
			}

		    return a.Equals(b);
	    }

		public static bool operator !=(DirectoryDto a, DirectoryDto b)
	    {
			if (object.ReferenceEquals(a, null))
			{
				return !object.ReferenceEquals(b, null);
			}

			return !a.Equals(b);
	    }

		private void SetDirectory(string directoryPath)
		{

			try
			{
				var formattedDir = PathHelper.RemoveTrailingDelimiter(directoryPath);
				DirInfo = new DirectoryInfo(formattedDir);
				DirInfoIsValid = DirInfo.Exists;
				DirectoryName = GetBaseDirectoryName(directoryPath);

			}
			catch (Exception ex)
			{
				var err = new FileOpsErrorMessageDto
				{
					DirectoryPath = directoryPath,
					ErrId = 20,
					ErrorMessage = "Exception thrown while setting Directory Path",
					ErrSourceMethod = "SetDirectory()",
					ErrException = ex,
					FileName = string.Empty,
					LoggerLevel = LogLevel.ERROR
				};

				ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
				ErrorMgr.WriteErrorMsg(err);

				SetDirectoryEmpty();
			}
	    }

		private void SetDirectoryEmpty()
	    {
		    DirInfo = null;
		    DirInfoIsValid = false;
		    DirectoryName = string.Empty;
	    }

	    public static string GetBaseDirectoryName(string dir)
	    {
		    var wrkDir = PathHelper.RemoveTrailingDelimiter(dir);

		    if (string.IsNullOrWhiteSpace(wrkDir))
		    {
			    return string.Empty;
		    }

		    var idx = wrkDir.LastIndexOf("\\", StringComparison.Ordinal);

		    if (idx < 0)
		    {
			    idx = wrkDir.LastIndexOf("/", StringComparison.Ordinal);
			}

		    if (idx < 0)
		    {
			    return string.Empty;
		    }

		    return wrkDir.Substring(++idx, wrkDir.Length - idx);

	    }

		private bool ValidateInputDirString(string directoryPath)
		{
			if (string.IsNullOrWhiteSpace(directoryPath))
			{
				return false;
			}

			return true;
		}

		private string AnalyzeRawDirectoryPath(string directoryPath)
		{
			var dirComponent = PathHelper.ExtractDirectoryComponent(directoryPath);
			var extComponent = PathHelper.ExtractFileExtensionComponent(directoryPath);

			if (extComponent == string.Empty)
			{
				return directoryPath;
			}

			return dirComponent;

		}

	}
}