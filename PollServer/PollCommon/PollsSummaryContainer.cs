using System;
using System.Collections.Generic;
using System.Diagnostics;
using VisualSummaryGenerator;


namespace PollCommon
{
    public class PollsSummaryContainer
    {
        private Dictionary<string, List<PollResponse>> m_UnauthenticatedPollResponses = new Dictionary<string, List<PollResponse>>();
        private Dictionary<string, List<PollResponse>> m_AuthenticatedPollResponses = new Dictionary<string, List<PollResponse>>();

        public void AddAuthenticatedResponse(PollResponse rd)
        {
            if (m_AuthenticatedPollResponses.ContainsKey(rd.PollId))
            {
                m_AuthenticatedPollResponses[rd.PollId].Add(rd);
            }
            else
            {
                var newList = new List<PollResponse> {rd};
                m_AuthenticatedPollResponses.Add(rd.PollId, newList);   
            }
            
            //Console.WriteLine("PollsSummaryContainer::AddAuthenticatedResponse | Poll" + rd.PollId + " has "+ m_AuthenticatedPollResponses.Count + " items.");
        }

        public void AddNonAuthenticatedResponse(PollResponse rd)
        {
            if (m_UnauthenticatedPollResponses.ContainsKey(rd.PollId))
            {
                m_UnauthenticatedPollResponses[rd.PollId].Add(rd);
            }
            else
            {
                var newList = new List<PollResponse> { rd };
                m_UnauthenticatedPollResponses.Add(rd.PollId, newList);
            }

            //Console.WriteLine("PollsSummaryContainer::AddNonAuthenticatedResponse | "+ m_UnauthenticatedPollResponses.Count + " PollResponses were not authenticated and are not included in Poll " + rd.PollId);
        }

        public IVisualSummaryResult GetRequestResults(string p)
        {
            IVisualSummaryResult result = null;

            var watch = new Stopwatch();
            watch.Start();

            try
            {
                //Page generation takes time (about 200 ms in average) due to processing that is required in order to render the result.
                result = (IVisualSummaryResult)SummaryGenerator.GeneratePage(m_AuthenticatedPollResponses[p]);
            }
            catch (Exception)
            {
                
            }
            
            watch.Stop();

            Console.WriteLine("PollsSummaryContainer::GetRequestResults | Page generation took " + watch.Elapsed.TotalMilliseconds + " ms.");
            
            return result;
        }
    }
}
