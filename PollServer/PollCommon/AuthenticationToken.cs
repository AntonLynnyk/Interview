using System.Xml.Serialization;

namespace PollCommon
{
	
	public class AuthenticationToken
	{
		
		// ELEMENTS
		[XmlElement("AuthenticationType")]
		public AuthenticationType AuthenticationType { get; set; }
		
		[XmlElement("AuthenticationData")]
		public string AuthenticationData { get; set; }
		
	}
}
