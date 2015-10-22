using LibLoader.Helpers;

namespace LibLoader.Models
{
	public class FilePathDto
	{
		public string DirectoryPath { get; private set; } = string.Empty;
		public string FileNameOnly { get; private set; } = string.Empty;
		public string Extension { get; private set; } = string.Empty;
		public string ExtensionWithoutLeadingDot { get; private set; } = string.Empty;
		public string FileNameAndExtension { get; private set; } = string.Empty;
		public string FullPathAndFileName { get; private set; } = string.Empty;
		public bool HasExtension { get; private set; } = false;
		public bool HasFileNameAndExtension { get; private set; } = false;
		public bool HasDirectoryPath { get; private set; } = false;
		public bool HasFileNameOnly { get; private set; } = false;

		public FilePathDto(string filePath)
		{

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
			ExtensionWithoutLeadingDot = PathHelper.ExtractFileExtensionComponentWithoutLeadingDot(filePath);
			FileNameAndExtension = PathHelper.ExtractFileNameAndExtension(filePath);
			FullPathAndFileName = PathHelper.ExtractFullPathAndFileName(filePath);
			
			HasExtension = PathHelper.FilePathHasExtension(filePath);

			if (!string.IsNullOrWhiteSpace(FileNameAndExtension)
			    && HasExtension)
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