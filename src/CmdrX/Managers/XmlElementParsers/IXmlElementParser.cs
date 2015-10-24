using System.Xml;
using CmdrX.Constants;
using CmdrX.Models;

namespace CmdrX.Managers.XmlElementParsers
{
	public interface IXmlElementParser
	{
		XmlElementType ElementType { get; }
		void ExtractElementInfo(XmlTextReader reader, 
									ref ConsoleCommandDto consoleCommand,
										ref ConsoleExecutorDto cmdExeDto);
	}
}