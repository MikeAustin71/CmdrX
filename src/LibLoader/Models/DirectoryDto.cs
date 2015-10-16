using System;
using System.IO;
using LibLoader.Helpers;

namespace LibLoader.Models
{
	/// <summary>
	/// Note Directories are stored WITHOUT Trailing Delimiters
	/// </summary>
    public class DirectoryDto
    {
        public DirectoryInfo DirInfo { get; set; }
        public bool DirInfoIsValid { get; set; }
        public string DirectoryName { get; set; }

	    public DirectoryDto()
	    {
		    SetDirectoryEmpty();
	    }
			
	    public DirectoryDto(string directoryPath)
	    {
			SetDirectory(directoryPath);
	    }

	    public DirectoryDto(FileInfo fInfo)
	    {
		    SetDirectory(fInfo.DirectoryName);
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
			var formattedDir = PathHelper.RemoveTrailingDelimiter(directoryPath);
			DirInfo = new DirectoryInfo(formattedDir);
			DirInfoIsValid = DirInfo.Exists;
		    DirectoryName = GetBaseDirectoryName(directoryPath);

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

	}
}