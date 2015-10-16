using System;
using System.IO;

namespace LibLoader.Models
{
	public class FileDto
	{
		public string FileExtension { get; set; }
		public string FileNameAndExtension { get; set; }
		public string FilePath { get; set; }
		public string FullPathAndFileName { get; set; }
		public DirectoryDto DirDto { get; set; }
		public FileInfo FileXinfo { get; set; }

		public FileDto()
		{
			SetDtoToEmpty();
		}

		public FileDto(string fileName)
		{

			ConfigureDto(fileName);

		}

		public FileDto(DirectoryDto dirDto, string fileNameAndExtension)
		{
			if (dirDto?.DirInfo == null || string.IsNullOrWhiteSpace(fileNameAndExtension))
			{
				SetDtoToEmpty();
				return;
			}

			ConfigureDto(Path.Combine(dirDto.DirInfo.FullName, fileNameAndExtension));
		}

		public override int GetHashCode()
		{
			return (FileXinfo != null ? FileXinfo.GetHashCode() : 0);
		}


		public override bool Equals(System.Object obj)
		{
			// If parameter is null return false.
			if (obj == null)
			{
				return false;
			}

			// If parameter cannot be cast to Point return false.
			FileDto p = obj as FileDto;

			if ((System.Object)p == null)
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
			if (Object.ReferenceEquals(a, b))
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
			if (!Object.ReferenceEquals(a, b))
			{
				return true;
			} // this handles a==null && b==null

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
			if (string.IsNullOrWhiteSpace(fileName))
			{
				SetDtoToEmpty();
				return;
			}

			FileXinfo = new FileInfo(fileName);

			DirDto = new DirectoryDto(FileXinfo.DirectoryName);

			if (FileXinfo != null)
			{
				FileExtension = FileXinfo.Extension;
				FileNameAndExtension = FileXinfo.Name;
				FilePath = FileXinfo.DirectoryName;
				FullPathAndFileName = FileXinfo.FullName;
			}

		}

		private void SetDtoToEmpty()
		{
			FileExtension = string.Empty;
			FileNameAndExtension = string.Empty;
			FilePath = string.Empty;
			FullPathAndFileName = string.Empty;
			DirDto = null;
			FileXinfo = null;
		}
	}
}