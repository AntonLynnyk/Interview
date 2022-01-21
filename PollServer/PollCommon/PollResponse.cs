using System.Xml.Serialization;
using System.Collections.Generic;

namespace PollCommon
{
	
	public class PollResponse
	{	
		// ELEMENTS
		[XmlElement("PollId")]
        public string PollId { get; set; }
		
		[XmlElement("AuthenticationToken")]
		public AuthenticationToken AuthenticationToken { get; set; }

        [XmlElement("AnswersToQuestionnaire")]
        public List<Answer> AnswersToQuestionnaire { get; set; }
		
	}
}
