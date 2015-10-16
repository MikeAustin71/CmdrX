using System.Collections.Generic;

namespace LibLoader.Models
{
	public class JobsGroupDto
	{
		public string JobGroupName { get; private set; }

		public int NumberOfJobs { get; set; } = 0;

		public List<ConsoleCommandDto> Jobs { get; set; } = new List<ConsoleCommandDto>();

		public JobsGroupDto(string jobGroupName)
		{
			JobGroupName = jobGroupName;
		}
	}
}