using System;
using System.Collections.Concurrent;
using System.Threading;
using AuthenticateUser;
using PollCommon;


namespace PollServer
{
    public class PollsSubmitionsHandler
    {
        private PollsSummaryContainer m_PollsDataConatainer;
        private ConcurrentQueue<object> m_ReceivedRequests;
        
        public PollsSubmitionsHandler(PollsSummaryContainer pollsDataConatainer, ConcurrentQueue<object> receivedRequests)
        {
            m_PollsDataConatainer = pollsDataConatainer;
            m_ReceivedRequests = receivedRequests;
        }

        public void PollsHandlerThread()
        {
            IPollAuthenticationModule autheticator = new AuthenticationModuleMock();
            while (true)
            {
                if (m_ReceivedRequests.Count == 0)
                {
                    Console.WriteLine("PollsSubmitionsHandler::PollsHandlerThread | Received requests queue is empty");
                    Thread.Sleep(500);
                }
                else
                {
                    object obj;
                    m_ReceivedRequests.TryPeek(out obj);
                    if (obj.GetType() != typeof (PollResponse))
                    {
                        //Not the request type that we need - wait until a PollResponse apears in queue
                        Thread.Sleep(50);
                    }
                    else
                    {
                        Console.WriteLine("PollsSubmitionsHandler::PollsHandlerThread | Received requests queue contains " +
                                          m_ReceivedRequests.Count + " Elements.");
                        object rdObj;
                        m_ReceivedRequests.TryDequeue(out rdObj);
                        PollResponse rd = (PollResponse) rdObj;
                        
                        if (autheticator.Authenticate(rd.AuthenticationToken.AuthenticationData))
                        {
                            Console.WriteLine(
                                "PollsSubmitionsHandler::PollsHandlerThread | New response data was authenticated");

                            m_PollsDataConatainer.AddAuthenticatedResponse(rd);
                        }
                        else
                        {
                            Console.WriteLine(
                                "PollsSubmitionsHandler::PollsHandlerThread | New response data was failed authentication");
                            m_PollsDataConatainer.AddNonAuthenticatedResponse(rd);
                        }
                    }
                }
            } //while
        }
    }
}
