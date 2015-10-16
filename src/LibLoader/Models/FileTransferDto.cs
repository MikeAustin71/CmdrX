using System;
using System.Collections.Generic;
using LibLoader.Constants;

namespace LibLoader.Models
{
    public class FileTransferDto : IDisposable
    {
        private bool _disposed;

        public string JobName { get; set; }

        public JobParametersDeliveryMode JobParametersMode { get; set; }

        public FileTransferMode FileTransferOperationMode { get; set; }

        public Dictionary<string, string> CmdLineArguments { get; set; }

        public DateTime ProcessStartTime { get; set; }

        public DateTime ProcessEndTime { get; set; }

        public string Source { get; set; }

        public List<string> FileSearchCriterion { get; set; }

        public List<string> DirSearchCriterion { get; set; }

        public bool SelectSourceFilesWithArchiveBitOn { get; set; }

        public bool ClearSourceFileArchiveBitOffAfterCopy { get; set; }

        public List<string> Destination { get; set; }
        
        public bool CreateDestinationDirectories { get; set; }
        
        public string Archive { get; set; }

        public bool CopyToArchive { get; set; }

        public bool CreateArchiveDirectories { get; set; }

        public List<FileOperationDto> FileNames { get; set; }

        public bool IsLoggingOn { get; set; }

        public bool IsVerboseLogging { get; set; }

        public int LogRetentionInDays { get; set; }

        public int DestinationDirectoryCount { get; set; }

        public int DestinationSuccessfulFilesCopiedCount { get; set; }

        public int DestinationFailedFilesCopiedCount { get; set; }

        public int SourceFilesCopiedCount { get; set; }

        public int ArchiveFilesSuccessfulCopyCount { get; set; }

        public int ArciveFilesFailedCopyCount { get; set; }

        public bool DeleteSrcFilesAfterOkCopy { get; set; }
        public bool DeleteSrcFilesAndDirsAfterOkCopy { get; set; }

        public bool CopyEmptyDirectories { get; set; }

        // default is 'false'
        // If 'true', files will not be copied
        // from source to destination. Instead,
        // source files will be located in destination
        // directories and deleted. If the deletion is 
        // successful, then the corresponding source files
        // will also be deleted. 
        public bool DeleteSrcFilesInDestDirs { get; set; }

        public int DestinationFilesDeletedCount { get; set; }

        public int SourceFilesSuccessfulDeletionCount { get; set; }

        public int SourceFilesFailedDeletionCount { get; set; }

        // Default is false;
        public bool CopyToDestinationOnlyIfSrcIsNewer { get; set; }

        public bool DisplayFilesSkipped { get; set; }

        public List<string> ExcludeFiles { get; set; }

        public bool UseFileSearchCriteriaToDeleteDestFiles { get; set; }

        public bool UseDirSearchCriteriaToDeleteDestFiles { get; set; }

        public bool WereAllSrcFilesCopiedToAllDestinationsOk { get; set; }

        public bool WereAllSrcFilesCopiedToArchiveOk { get; set; }

        public bool WereAllSrcFilesDeletedOk { get; set; }

        public bool WereAllSrcDirsDeletedOk { get; set; }

        public bool WereAllDestFilesDeletedOk { get; set; }

        public bool WereAllDestDirsCreatedOk { get; set; }

        public bool WasArchiveDirCreateOk { get; set; }

        public bool IsDebuggingOn { get; set; }

        public FileTransferDto()
            : this(string.Empty,new List<string>(), string.Empty, true )
        {
            

        }


        public FileTransferDto(string source, List<string> destination, string archive, bool turnOnLogging)
        {
            CmdLineArguments = new Dictionary<string, string>();

            JobName = string.Empty;

            JobParametersMode = JobParametersDeliveryMode.None;

            CopyToArchive = false;

            LogRetentionInDays = 180;

            DestinationSuccessfulFilesCopiedCount = 0;

            ArchiveFilesSuccessfulCopyCount = 0;

            IsLoggingOn = turnOnLogging;

            Source = source;

            Destination = destination;

            Archive = archive;

            DisplayFilesSkipped = false;

            ExcludeFiles = new List<string>();

            SelectSourceFilesWithArchiveBitOn = false;

            ClearSourceFileArchiveBitOffAfterCopy = false;

            WereAllSrcFilesCopiedToAllDestinationsOk = true;

            WereAllSrcFilesCopiedToArchiveOk = true;

            WereAllSrcFilesDeletedOk = true;

            WereAllDestFilesDeletedOk = true;

            WereAllDestDirsCreatedOk = true;

            WasArchiveDirCreateOk = true;

            WereAllSrcDirsDeletedOk = true;

            FileTransferOperationMode = FileTransferMode.None;

            IsVerboseLogging = false;

            WereAllSrcFilesDeletedOk = true;

            WereAllSrcDirsDeletedOk = true;

            WereAllDestFilesDeletedOk = true;

            WereAllDestDirsCreatedOk = true;

            WasArchiveDirCreateOk = true;


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

                    if (CmdLineArguments != null)
                    {
                        CmdLineArguments = null;
                    }


                    if (FileSearchCriterion != null)
                    {
                        FileSearchCriterion = null;
                    }


                    if (DirSearchCriterion != null)
                    {
                        DirSearchCriterion = null;

                    }

                    if (Destination != null)
                    {
                        Destination = null;

                    }

                    if (FileNames != null)
                    {
                        FileNames = null;

                    }

                    if (ExcludeFiles != null)
                    {
                        ExcludeFiles = null;

                    }

                }


                // Note disposing has been done.
                _disposed = true;

            }
        }


    }
}