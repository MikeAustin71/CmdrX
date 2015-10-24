using System;
using System.Collections.Generic;

namespace CmdrX.Models
{
	public class JobsGroupDto
	{
		private bool _disposed;

		public string JobGroupName { get; private set; }

		public int NumberOfJobs { get; set; } = 0;

		public List<ConsoleCommandDto> Jobs { get; set; } = new List<ConsoleCommandDto>();

		public JobsGroupDto(string jobGroupName)
		{
			JobGroupName = jobGroupName;
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
					if (Jobs != null)
					{
						foreach (var job in Jobs)
						{
							job.Dispose();
						}

						Jobs = null;
						NumberOfJobs = 0;
					}

				}


				// Note disposing has been done.
				_disposed = true;

			}
		}



	}
}