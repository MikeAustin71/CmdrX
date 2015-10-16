using System.Collections.Generic;

namespace LibLoader.Models
{
    public class FileOperationDto
    {
        private string _sourcePath;

        public string SourcePath
        {
            get
            {
                return string.IsNullOrEmpty(_sourcePath) ? string.Empty : _sourcePath;
            }

            set
            {
                if(string.IsNullOrEmpty(value))
                {
                    _sourcePath = string.Empty;
                }
                else if(!value.EndsWith("\\"))
                {
                    _sourcePath = value + "\\";
                }
                else
                {
                    _sourcePath = value;
                }

            }
        }

        private string _archviePath;

        public string ArchivePath
        {
            get
            {
                return string.IsNullOrEmpty(_archviePath) ? string.Empty : _archviePath;
            }

            set
            {
                if(string.IsNullOrEmpty(value))
                {
                    _archviePath = string.Empty;
                }
                else if(!value.EndsWith("\\"))
                {
                    _archviePath = value + "\\";
                }
                else
                {
                    _archviePath = value;
                }

            }
        }

        public bool CreateArchivePath { get; set; }

        private List<string> _destinationPaths;

        public List<string> DestinationPaths
        {
            get
            {
                if(_destinationPaths == null)
                {
                    return new List<string>();
                }

                return _destinationPaths;
            }

            set
            {
                _destinationPaths = new List<string>();

                if(value==null || value.Count < 1)
                {

                    return;
                }

                foreach (var s in value)
                {
                    if(!s.EndsWith("\\"))
                    {
                        _destinationPaths.Add(s + "\\");
                    }
                    else
                    {
                        _destinationPaths.Add(s);
                    }
                }

            }
        }

        public bool CreateDestinationPath { get; set; }

        public string FileName;

        public bool WasCopiedToAllDestinationsOk;

        public bool WasCopiedToArchiveOk;

        public bool WasDeletedFromAllDestinationsOk;

        public bool WasSourceFileDeletedOk;

    }
}