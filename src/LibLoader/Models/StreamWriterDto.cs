using System;
using System.IO;
using LibLoader.Constants;
using LibLoader.GlobalConstants;
using LibLoader.Helpers;

namespace LibLoader.Models
{
	public class StreamWriterDto
	{
		public ErrorLogger ErrorMgr = new
			ErrorLogger(2964000,
						"StreamWriterDto",
						AppConstants.LoggingStatus,
						AppConstants.LoggingMode);

		private FileStream _fStream;

		private StreamWriter _swWriter;

		public FileDto StreamWriterFile { get; private set; }

		public StreamWriterDto(FileDto swFile)
		{
			if (!FileHelper.IsFileDtoValid(swFile))
			{
				var ex = new Exception("Invalid FileDto Passed to StreamWriter!");
				var err = new FileOpsErrorMessageDto
				{
					DirectoryPath = string.Empty,
					ErrId = 2,
					ErrorMessage = ex.Message,
					ErrSourceMethod = "SetStreamWriter()",
					ErrException = ex,
					FileName = string.Empty,
					LoggerLevel = LogLevel.FATAL
				};

				ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
				ErrorMgr.WriteErrorMsg(err);
				throw ex;

			}

			CreateStreamWriter(swFile.FileXinfo.FullName);
		}

		public void SetStreamWriter(FileDto swFile)
		{
			if (!FileHelper.IsFileDtoValid(swFile))
			{
				var ex = new Exception("Invalid FileDto Passed to StreamWriter!");
				var err = new FileOpsErrorMessageDto
				{
					DirectoryPath = string.Empty,
					ErrId = 20,
					ErrorMessage = ex.Message,
					ErrSourceMethod = "SetStreamWriter()",
					ErrException = ex,
					FileName = string.Empty,
					LoggerLevel = LogLevel.FATAL
				};

				ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
				ErrorMgr.WriteErrorMsg(err);
				throw ex;
			}


			if (swFile.FileXinfo.FullName == StreamWriterFile.FileXinfo.FullName)
			{
				return;
			}

			CreateStreamWriter(swFile.FileXinfo.FullName);
		}

		public bool CreateStreamWriter(string swFileName)
		{
			Close();

			StreamWriterFile = new FileDto(swFileName);

			if (!FileHelper.IsFileDtoValid(StreamWriterFile))
			{
				var ex = new Exception("Invalid FileDto Passed to StreamWriter!");
				var err = new FileOpsErrorMessageDto
				{
					DirectoryPath = string.Empty,
					ErrId = 30,
					ErrorMessage = ex.Message,
					ErrSourceMethod = "CreateStreamWriter()",
					ErrException = ex,
					FileName = string.Empty,
					LoggerLevel = LogLevel.FATAL
				};

				ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
				ErrorMgr.WriteErrorMsg(err);
				throw ex;
			}


			try
			{
				StreamWriterFile.DirDto.DirInfo.Refresh();

				if (!StreamWriterFile.DirDto.DirInfo.Exists)
				{
					if (!DirectoryHelper.CreateDirectoryIfNecessary(StreamWriterFile))
					{
						var ex = new Exception("Failed to create directory for StreamWriter!");
						var err = new FileOpsErrorMessageDto
						{
							DirectoryPath = string.Empty,
							ErrId = 30,
							ErrorMessage = ex.Message,
							ErrSourceMethod = "CreateStreamWriter()",
							ErrException = ex,
							FileName = string.Empty,
							LoggerLevel = LogLevel.FATAL
						};

						ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
						ErrorMgr.WriteErrorMsg(err);
						throw ex;

					}
				}

				_fStream = new FileStream(StreamWriterFile.FileXinfo.FullName, 
											FileMode.Append,
												FileAccess.Write,
													FileShare.Read);

				_swWriter = new StreamWriter(_fStream);
			}
			catch(Exception ex)
			{
				var err = new FileOpsErrorMessageDto
				{
					DirectoryPath = string.Empty,
					ErrId = 32,
					ErrorMessage = "Failed To Create StreamWriter! " + ex.Message,
					ErrSourceMethod = "SetStreamWriter()",
					ErrException = ex,
					FileName = string.Empty,
					LoggerLevel = LogLevel.FATAL
				};

				ErrorMgr.LoggingStatus = ErrorLoggingStatus.On;
				ErrorMgr.WriteErrorMsg(err);

				throw;
			}


			return true;
		}

		public StreamWriter GetStreamWriter()
		{
			return _swWriter;
		}

		public FileStream GetFileStream()
		{
			return _fStream;
		}

		public void Close()
		{
			if (_swWriter != null)
			{
				_swWriter.Flush();
				_swWriter.Close();
				_swWriter.Dispose();
				_swWriter = null;
			}

			if (_fStream != null)
			{
				_fStream.Close();
				_fStream.Dispose();
				_fStream = null;
			}

			if (StreamWriterFile != null)
			{
				StreamWriterFile.Dispose();
				StreamWriterFile = null;

			}

		}
	}
}