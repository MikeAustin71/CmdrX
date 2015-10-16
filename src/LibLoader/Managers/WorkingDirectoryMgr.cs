using System.Runtime.Serialization.Formatters;
using LibLoader.Helpers;
using LibLoader.Models;

namespace LibLoader.Managers
{
	public class WorkingDirectoryMgr
	{
		public bool IsSetToTargetDirectory { get; private set; } = false;
		public bool IsCurrentAndTargetSameDirectory { get; private set; } = false;
		public DirectoryDto OriginalCurrentWorkingDirectory { get; private set; } 
		public DirectoryDto TargetWorkingDirectory { get; set; }

		public WorkingDirectoryMgr(DirectoryDto targetWorkingDirectory)
		{
			TargetWorkingDirectory = targetWorkingDirectory;
			SetCurrentWorkingDirectory();
		}

		public bool ChangeToTargetWorkingDirectory()
		{

			SetCurrentWorkingDirectory();

			if (OriginalCurrentWorkingDirectory == TargetWorkingDirectory)
			{
				IsSetToTargetDirectory = true;
				IsCurrentAndTargetSameDirectory = true;

				return true;
			}

			DirectoryHelper.CreateDirectoryIfNecessary(TargetWorkingDirectory);

			DirectoryHelper.ChangeToNewCurrentDirectory(TargetWorkingDirectory);

			var latestCurrWorkingDirectory = DirectoryHelper.GetCurrentDirectory();

			IsSetToTargetDirectory = TargetWorkingDirectory == latestCurrWorkingDirectory;

			return IsSetToTargetDirectory;
		}

		public bool ChangeBackToOriginalWorkingDirectory()
		{
			var latestCurrWorkingDirectory = DirectoryHelper.GetCurrentDirectory();

			if (latestCurrWorkingDirectory == OriginalCurrentWorkingDirectory)
			{
				return true;
			}

			DirectoryHelper.ChangeToNewCurrentDirectory(OriginalCurrentWorkingDirectory);

			latestCurrWorkingDirectory = DirectoryHelper.GetCurrentDirectory();

			return TargetWorkingDirectory != latestCurrWorkingDirectory;
		}

		public DirectoryDto SetCurrentWorkingDirectory()
		{
			OriginalCurrentWorkingDirectory = DirectoryHelper.GetCurrentDirectory();

			return OriginalCurrentWorkingDirectory;
			
		}

	}
}