using System;
using System.Collections.Concurrent;
using System.Threading;
using PollCommon;
using PollCommunicationUtility;


namespace PollServer
{
    public class SummaryResultsHandler
    {
        private PollsSummaryContainer m_PollsDataConatainer;
        private ConcurrentQueue<object> m_ReceivedRequests;

        public SummaryResultsHandler(PollsSummaryContainer pollsDataConatainer, ConcurrentQueue<object> receivedRequests)
        {
            m_PollsDataConatainer = pollsDataConatainer;
            m_ReceivedRequests = receivedRequests;
        }

        public void SummaryThread()
        {
            while (true)
            {
                if (m_ReceivedRequests.Count == 0)
                {
                    Console.WriteLine("SummaryResultsHandler::SummaryResultsHandler | Received requests queue is empty.");
                    Thread.Sleep(100);
                }
                else
                {
                    object obj;
                    m_ReceivedRequests.TryPeek(out obj);
                    if (obj.GetType() != typeof(PollSummaryRequest))
                    {
                        //Not the request type that we need - wait until a PollSummaryRequest appears in queue
                        Thread.Sleep(50);
                    }
                    else
                    {
                        Console.WriteLine("SummaryResultsHandler::SummaryResultsHandler | Received requests queue has " +
                                          m_ReceivedRequests.Count + " Elements.");
                        object reqObj;
                        m_ReceivedRequests.TryDequeue(out reqObj);
                        PollSummaryRequest req = (PollSummaryRequest) reqObj;
                        IVisualSummaryResult result = m_PollsDataConatainer.GetRequestResults(req.pollId);

                        Communicator.SubmitResults(req.caller, result);
                    }
                }
            } //while   
        }
    }
}
