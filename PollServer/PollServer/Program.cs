using System.Collections.Concurrent;
using System.Threading;
using PollCommon;
using PollCommunicationUtility;

namespace PollServer
{
    class Program
    {
        private static void Main(string[] args)
        {
            //Contains all the requests recieved from the communication layer (both request types - PollResponse and PollSummaryRequest).
            var receivedRequests = new ConcurrentQueue<object>();

            //Handles for all incoming and outgoing requests
            var communicator = new Communicator(receivedRequests);
            communicator.Initialize();


            //This container holds ALL the server polls data.
            var pollsDataConatainer = new PollsSummaryContainer();

            // running the polls submission thread
            var submissionsHandler = new PollsSubmitionsHandler(pollsDataConatainer, receivedRequests);
            var submissionsHandlerThreadDelegate = new ThreadStart(submissionsHandler.PollsHandlerThread);
            var submitionsHandlerThread = new Thread(submissionsHandlerThreadDelegate) { Name = "submissionsHandlerThread" };
            submitionsHandlerThread.Start();

            // running the summary results thread
            var summaryResultsModuleModule = new SummaryResultsHandler(pollsDataConatainer, receivedRequests);
            var summaryResultsThreadDelegate = new ThreadStart(summaryResultsModuleModule.SummaryThread);
            var summaryResultsThread = new Thread(summaryResultsThreadDelegate) {Name = "summaryResultsThread"};
            summaryResultsThread.Start();
        }
    }

}

