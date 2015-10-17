using System.IO;

namespace LibLoader.Models
{
	public class StreamWriterDto
	{
		public StreamWriter swWriter;

		public FileDto StreamWriterFile { get; private set; }

		public StreamWriterDto(FileDto swFile)
		{
			StreamWriterFile = swFile;
		}

		public void SetStreamWriter(FileDto swFile)
		{
			if (swFile.FileXinfo.FullName == StreamWriterFile.FileXinfo.FullName)
			{
				return;
			}

			Close();

			StreamWriterFile = swFile;

			CreateStreamWriter();
		}

		public bool CreateStreamWriter()
		{
			swWriter = new StreamWriter(StreamWriterFile.FileXinfo.FullName);

			return true;
		}

		public StreamWriter GetStreamWriter()
		{
			return swWriter;
		}

		public void Close()
		{
			if (swWriter != null)
			{
				swWriter.Flush();
				swWriter.Close();
				swWriter.Dispose();
				swWriter = null;
			}

			if (StreamWriterFile != null)
			{
				StreamWriterFile.Dispose();
				StreamWriterFile = null;

			}

		}
	}
}