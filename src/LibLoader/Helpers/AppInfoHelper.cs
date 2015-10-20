using System;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using LibLoader.GlobalConstants;
using LibLoader.Models;

namespace LibLoader.Helpers
{
	public static class AppInfoHelper
	{
		public const string TestDir = "LibLoadTests";

		public static string GetThisAssemblyVersion()
		{
			string version;

			try
			{
				Assembly assembly = Assembly.GetExecutingAssembly();
				FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
				version = fvi.ProductVersion;

			}
			catch
			{
				version = "Unknown Version";
			}

			return version;
		}

		public static DirectoryDto GetCurrentApplicationDirectory()
		{
			// (new Uri(Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath;
			var fileInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);
			return new DirectoryDto(fileInfo);
		}

		public static DirectoryDto GetCurrentTestDirectory()
		{
			
			var appDir = Assembly.GetExecutingAssembly().Location;
			var idx = appDir.LastIndexOf(TestDir, StringComparison.Ordinal);
			idx = idx + TestDir.Length;
			var baseAppDir = appDir.Substring(0, idx);
			return new DirectoryDto(baseAppDir);

		}

		public static FileDto GetDefaultXmlCommandFile()
		{
			var defaultCmdFileName = ConfigurationManager.AppSettings["DefaultXmlCmdFile"];

			if (defaultCmdFileName.Contains("\\") || defaultCmdFileName.Contains("/"))
			{
				return new FileDto(defaultCmdFileName);
			}

			var currDir = GetCurrentApplicationDirectory();
			return new FileDto(currDir.DirInfo.FullName 
				+ "\\" 
				+ defaultCmdFileName);
		}

		public static FileDto GetCommandOutputLogFile(DirectoryDto targetDir)
		{
			var filePrefix = DateTime.Now.ToString(CultureInfo.InvariantCulture);
			return new FileDto(targetDir.DirInfo.FullName 
				+ "\\" 
				+ filePrefix 
				+ "_" 
				+ AppConstants.DefaultCommandOutputLogFileName);
		}

		public static void DisplayCmdLineParms()
		{
			Console.WriteLine("-xml=Source Directory");
			Console.WriteLine("-l=logging On|Off (Default On)");
			Console.WriteLine("-lr=Log Retention in Days (Example -lr=90) (Default 5)");

		}

	}
}