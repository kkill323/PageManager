namespace PageManagerApp
{
    public class PageStatistics
    {
        public int FirstLoad { get; private set; }
        public int PageHits { get; private set; }
        public int PageFaults { get; private set; }
        public int AbortedJobs { get; private set; }
        public int TotalRequests { get; private set; }

        public void IncrementAbortedJobs()
        {
            ++AbortedJobs;
            ++TotalRequests;
        }

        public void IncrementFirstLoad()
        {
            ++FirstLoad;
            ++TotalRequests;
        }

        public void IncrementPageFault()
        {
            ++PageFaults;
            ++TotalRequests;
        }

        public void IncrementPageHit()
        {
            ++PageHits;
            ++TotalRequests;
        }

        public void Reset()
        {
            FirstLoad = 0;
            PageHits = 0;
            PageFaults = 0;
            AbortedJobs = 0;
        }

        public override string ToString()
        {
            return
                $"First Load: {FirstLoad} Page Hits: {PageHits} PageFaults: {PageFaults} Aborted Jobs: {AbortedJobs}";
        }
    }
}