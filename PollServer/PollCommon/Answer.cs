using System.Xml.Serialization;

namespace PollCommon
{
	
	public class Answer
	{
		
		// ELEMENTS
		[XmlElement("QuestionId")]
		public int QuestionId { get; set; }
		
		[XmlElement("Response")]
		public bool Response { get; set; }
		
	}
}
