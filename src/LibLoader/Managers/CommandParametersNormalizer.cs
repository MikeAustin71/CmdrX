using System;

namespace LibLoader.Managers
{
	public class CommandParametersNormalizer
	{
		public Decimal CommandMinTimeOutInMinutes { get; set; }
		public Decimal CommandMaxTimeOutInMinutes { get; set; }
		public string DefaultCommandOutputLogFileName { get; set; }

	}
}