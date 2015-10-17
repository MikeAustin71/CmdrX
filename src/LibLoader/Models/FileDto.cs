using System;
using System.IO;
using LibLoader.Constants;
using LibLoader.GlobalConstants;
using LibLoader.Helpers;

namespace LibLoader.Models
{
	public class FileDto
	{
		private bool _disposed;

		private FilePathDto _filePathDto;

		public ErrorLogger ErrorMgr = new
			ErrorLogger(3267000,
						"FilePathDto",
						AppConstants.LoggingStatus,
						AppConstants.LoggingMode);

		public string FileExtension { get; set; }
		public string FileNameAndExtension { get; set; }
		public string FilePath { get; set; }
		public string FullPathAndFileName { get; set; }
		public DirectoryDto DirDto { get; set; }
		public FileInfo FileXinfo { get; set; }
		public bool IsFileDtoEmpty { get; private set; }

		public FileDto()
		{
			SetDtoToEmpty();
		}

		public FileDto(string fileName)
		{

			ConfigureDto(StringHelper.TrimStringEnds(fileName));

		}

		public FileDto(DirectoryDto dirDto, string fileNameAndExtension)
		{
			if (dirDto?.DirInfo == null)
			{
				SetDtoToEmpty();
				return;
			}

			ConfigureDto(Path.Combine(dirDto.DirInfo.FullName, StringHelper.TrimStringEnds(fileNameAndExtension)));
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
					if (FileXinfo != null)
					{
						FileXinfo = null;
						IsFileDtoEmpty = true;
					}

					if (DirDto != null)
					{
						DirDto.Dispose();
						DirDto = null;
					}

				}


				// Note disposing has been done.
				_disposed = true;

			}
		}


		public override int GetHashCode()
		{
			return (FileXinfo != null ? FileXinfo.GetHashCode() : 0);
		}


		public override bool Equals(object obj)
		{
			// If parameter is null return false.
			if (obj == null)
			{
				return false;
			}

			// If parameter cannot be cast to Point return false.
			FileDto p = obj as FileDto;

			if ((object)p == null)
			{
				return false;
			}

			if (p.FileXinfo == null && p.FileXinfo == null)
			{
				return true;
			}

			if (FileXinfo == null || p.FileXinfo == null)
			{
				return false;
			}

			return FileXinfo.FullName == p.FileXinfo.FullName;
		}

		public static bool operator ==(FileDto a, FileDto b)
		{
			// ReSharper disable once BuiltInTypeReferenceStyle
			if (ReferenceEquals(a, b))
			{
				return true;
			} // this handles a==null && b==null

			if (a == null || b == null)
			{
				return false;
			}

			if (a.FileXinfo == null && b.FileXinfo == null)
			{
				return true;
			}

			if (a.FileXinfo == null || b.FileXinfo == null)
			{
				return false;
			}

			return a.FileXinfo.FullName == b.FileXinfo.FullName;
		}

		public static bool operator !=(FileDto a, FileDto b)
		{
			// ReSharper disable once BuiltInTypeReferenceStyle
			if (!ReferenceEquals(a, b))
			{
				return true;
			} // this handles a==null && b==null

			// ReSharper disable once ConditionIsAlwaysTrueOrFalse
			if (a == null && b == null)
			{
				return false;
			}

			// ReSharper disable once ConditionIsAlwaysTrueOrFalse
			if (a == null || b == null)
			{
				return false;
			}

			if (a.FileXinfo == null && b.FileXinfo == null)
			{
				return false;
			}

			if (a.FileXinfo == null || b.FileXinfo == null)
			{
				return true;
			}


			return a.FileXinfo.FullName != b.FileXinfo.FullName;

		}

		private void ConfigureDto(string fileName)
		{
			var fName = StringHelper.TrimStringEnds(fileName);

			if (!ValidateFileInputString(fName))
			{
				var err = new FileOpsErrorMessageDto
				{
					DirectoryPath = string.Empty,
					ErrId = 10,
					ErrorMessage = "Validation failed on FileDto input file path!",
					ErrSourceMethod = "ValidateFileInputString()",
					FileName = fileName,
					LoggerLevel = LogLevel.ERROR
				};

				ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
				ErrorMgr.WriteErrorMsg(err);

				SetDtoToEmpty();
				return;
			}

			try
			{
				FileXinfo = new FileInfo(fName);

				DirDto = new DirectoryDto(FileXinfo.DirectoryName);

			}
			catch(Exception ex) 
			{
				var err = new FileOpsErrorMessageDto
				{
					DirectoryPath = string.Empty,
					ErrId = 30,
					ErrorMessage = "Failue creating FileInfo and DirectoryInfo objects!",
					ErrSourceMethod = "ConfigureDto()",
					ErrException = ex,
					FileName = fileName,
					LoggerLevel = LogLevel.ERROR
				};

				ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
				ErrorMgr.WriteErrorMsg(err);

				SetDtoToEmpty();
				return;

			}

			FileExtension = FileXinfo.Extension;
			FileNameAndExtension = FileXinfo.Name;
			FilePath = FileXinfo.DirectoryName;
			FullPathAndFileName = FileXinfo.FullName;
			IsFileDtoEmpty = false;
		}

		private bool ValidateFileInputString(string fileStr)
		{
			if (string.IsNullOrWhiteSpace(fileStr))
			{
				return false;
			}

			try
			{
				_filePathDto = new FilePathDto(fileStr);

				if (_filePathDto.FileNameAndExtension == string.Empty)
				{
					return false;
				}

			}
			catch
			{
				return false;
			}

			return true;
		}

		private void SetDtoToEmpty()
		{
			FileExtension = string.Empty;
			FileNameAndExtension = string.Empty;
			FilePath = string.Empty;
			FullPathAndFileName = string.Empty;
			DirDto = null;
			FileXinfo = null;
			IsFileDtoEmpty = true;
		}
	}
}