using System.Security.Cryptography.X509Certificates;
using System.Xml;
using LibLoader.Constants;
using LibLoader.Models;

namespace LibLoader.Managers.XmlElementParsers
{
	public interface IXmlElementParser
	{
		XmlElementType ElementType { get; }
		void ExtractElementInfo(XmlTextReader reader, 
									ref ConsoleCommandDto consoleCommand,
										ref ConsoleExecutorDto cmdExeDto);
	}
}