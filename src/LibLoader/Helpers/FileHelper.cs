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



        public static Stack<string> GetAllRelativeSubDirectoriesInTree(string parentDirectory)
        {
            var dirStack = new Stack<string>();
            var stack = new Stack<string>();
            var dirs = Directory.GetDirectories(parentDirectory);
            string pDir = PathHelper.RemoveTrailingDelimiter(parentDirectory);
            string relDir;

            foreach (var dir in dirs)
            {

                relDir = dir.Substring(pDir.Length);

                stack.Push(relDir);
            }

            while (stack.Count > 0)
            {
                var dir = stack.Pop();

                dirStack.Push(dir);

                dirs = Directory.GetDirectories(pDir + dir);

                foreach (var d in dirs)
                {
                    relDir = d.Substring(pDir.Length);

                    stack.Push(relDir);
                }

            }

            return dirStack;
        }

        public static Stack<string> GetAllSubdirectoriesInTree(string parentDirectory)
        {
            var dirStack = new Stack<string>();
            var stack = new Stack<string>();
            var dirs = Directory.GetDirectories(parentDirectory);

            foreach (var dir in dirs)
            {
                stack.Push(dir);
            }

            while(stack.Count > 0)
            {
                var dir = stack.Pop();

                dirStack.Push(dir);

                dirs = Directory.GetDirectories(dir);

                foreach (var d in dirs)
                {
                    stack.Push(d);
                }

            }

            return dirStack;
        }


        public static bool DeleteDirectoryFromFilePath(string filePathNameExt)
        {
            if(string.IsNullOrEmpty(filePathNameExt))
            {
                return true;
            }


            var dir = Path.GetDirectoryName(filePathNameExt);

            return DeleteADirectory(dir);


        }

        public static bool DeleteADirectory(string dir)
        {
            if(string.IsNullOrEmpty(dir))
            {
                return true;
            }

            try
            {
                if (!Directory.Exists(dir))
                {
                    return true;
                }

                var files = Directory.GetFiles(dir);

                if (files.Length < 1)
                {
                    Directory.Delete(dir);
                }

                if (Directory.Exists(dir))
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
    }
}