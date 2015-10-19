using System.IO;
using System.Security.Permissions;
using LibLoader.Helpers;

namespace LibLoader.Models
{
	public class FilePathDto
	{
		public string DirectoryPath { get; private set; }
		public string FileNameOnly { get; private set; }
		public string Extension { get; private set; }
		public string FileNameAndExtension { get; private set; }
		public string FullPathAndFileName { get; private set; }
		public bool HasExtensinon { get; private set; }
		public bool HasFileNameAndExtension { get; private set; }
		public bool HasDirectoryPath { get; private set; }
		public bool HasFileNameOnly { get; private set; }

		public FilePathDto(string filePath)
		{
			DirectoryPath = string.Empty;
			FileNameOnly = string.Empty;
			Extension = string.Empty;
			FileNameAndExtension = string.Empty;
			FullPathAndFileName = string.Empty;
			HasExtensinon = false;

			AnalyzeRawFilePath(StringHelper.TrimStringEnds(filePath));
		}

		private void AnalyzeRawFilePath(string filePath)
		{
			if (string.IsNullOrWhiteSpace(filePath))
			{
				return;
			}

			DirectoryPath = PathHelper.ExtractDirectoryComponent(filePath);
			FileNameOnly = PathHelper.ExtractFileNameOnlyComponent(filePath);
			Extension = PathHelper.ExtractFileExtensionComponent(filePath);
			FileNameAndExtension = PathHelper.ExtractFileNameAndExtension(filePath);
			FullPathAndFileName = PathHelper.ExtractFullPathAndFileName(filePath);
			HasExtensinon = PathHelper.FilePathHasExtension(filePath);

			if (!string.IsNullOrWhiteSpace(FileNameAndExtension)
			    && HasExtensinon)
			{
				HasFileNameAndExtension = true;
			}

			if (!string.IsNullOrWhiteSpace(DirectoryPath))
			{
				var dirDto = new DirectoryDto(DirectoryPath);
				if (DirectoryHelper.IsDirectoryDtoValid(dirDto))
				{
					HasDirectoryPath = true;
				}

				dirDto.Dispose();
				
			}

			if (!string.IsNullOrWhiteSpace(FileNameOnly))
			{
				HasFileNameOnly = true;
			}

		}

	}
}