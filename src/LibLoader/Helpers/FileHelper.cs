using System.Collections.Generic;
using System.IO;
using LibLoader.GlobalConstants;
using LibLoader.Models;

namespace LibLoader.Helpers
{
    public static class FileHelper
    {
		public static ErrorLogger ErrorMgr = new 
			ErrorLogger(831000,
						"FileHelper",
						AppConstants.LoggingStatus,
						AppConstants.LoggingMode);

	    public static bool CreateAFile(FileDto fileDto)
	    {
		    if (fileDto?.FileXinfo == null)
		    {
			    return false;
		    }

			fileDto.FileXinfo.Refresh();

		    if (fileDto.FileXinfo.Exists)
		    {
			    return true;
		    }

		    try
		    {
			    fileDto.FileXinfo.Create();
		    }
		    catch
		    {
			    return false;
		    }

		    return true;
	    }

		public static List<string> GetAllFilesInDirectoryStructure(string parentDirectory)
        {
            var fileList = new List<string>();
            var stack = new Stack<string>();

            stack.Push(parentDirectory);

            while (stack.Count > 0)
            {
                var dir = stack.Pop();

                try
                {
                    var files = Directory.GetFiles(dir, "*.*");

                    if (files.Length > 0)
                    {
                        fileList.AddRange(files);
                    }

                    foreach (var subDir in Directory.GetDirectories(dir))
                    {
                        stack.Push(subDir);
                    }
                }
                catch
                {
	                // ReSharper disable once RedundantJumpStatement
                    continue;
                }
            }

            return fileList;
            
        }


        public static List<string> GetAllFilesInDirectoryStructure(string parentDirectory, string criteria)
        {
            var fileList = new List<string>();
            var stack = new Stack<string>();

            stack.Push(parentDirectory);

            while (stack.Count > 0)
            {
                var dir = stack.Pop();

                try
                {
                    var files = Directory.GetFiles(dir, criteria);

                    if (files.Length > 0)
                    {
                        fileList.AddRange(files);
                    }

                    foreach (var subDir in Directory.GetDirectories(dir))
                    {
                        stack.Push(subDir);
                    }
                }
                catch
                {
	                // ReSharper disable once RedundantJumpStatement
	                continue;
                }
            }

            return fileList;
        }


	    public static bool DoesFileExist(FileDto targetFileDto)
	    {
		    if (targetFileDto?.FileXinfo == null)
		    {
			    return false;
		    }

		    return targetFileDto.FileXinfo.Exists;
	    }

        public static bool DoesFileExist(string targetFile)
        {
            if(string.IsNullOrEmpty(targetFile))
            {
                return false;
            }

            var result = false;

            try
            {
                if(File.Exists(targetFile))
                {
                    result = true;
                }
            }
            catch
            {
                result = false;
            }

            return result;
        }

	    public static bool DeleteAFile(FileDto fileDto)
	    {
		    if (fileDto?.FileXinfo == null)
		    {
			    return false;
		    }

		    if (!fileDto.FileXinfo.Exists)
		    {
			    return true;
		    }

		    try
		    {
				fileDto.FileXinfo.Delete();
		    }
		    catch
		    {
			    return false;
		    }

		    return true;
	    }

        public static bool DeleteAFile(string targetFile)
        {

            if(string.IsNullOrEmpty(targetFile))
            {
                return false;
            }

            var result = true;

            try
            {
                if(!File.Exists(targetFile))
                {
                    return true;
                }

                File.Delete(targetFile);

                if(File.Exists(targetFile))
                {
                    result = false;
                }
            }
            catch
            {
                result = false;
            }

            return result;
        }

        public static bool IsSourceNewerThanDestination(FileInfo fi, string targetFile)
        {
            if(fi==null || !fi.Exists || string.IsNullOrEmpty(targetFile))
            {
                return false;
            }

            try
            {
                if(!File.Exists(targetFile))
                {
                    return true;
                }

                var destLastWriteTime = File.GetLastWriteTimeUtc(targetFile);

                var sourceLastWriteTime = fi.LastWriteTimeUtc;
                
                if(sourceLastWriteTime > destLastWriteTime)
                {
                    return true;
                }

            }
            catch
            {
                return true;
            }


            return false;

        }

	    public static bool IsFileDtoValid(FileDto fileDto)
	    {
		    if (fileDto?.FileXinfo == null || fileDto.DirDto == null)
		    {
			    return false;
		    }

		    if (string.IsNullOrWhiteSpace(fileDto.FileXinfo.FullName) 
				|| fileDto.FileXinfo.FullName.Length < 4)
		    {
			    return false;
		    }

		    if (DirectoryHelper.IsDirectoryDtoValid(fileDto.DirDto))
		    {
			    return false;
		    }

		    return true;
	    }
    }
}