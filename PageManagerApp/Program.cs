using System;

namespace PageManagerApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var manager = new PageManager(10, 15);
            manager.LoadFromFile(@"/labdata/job_data_2.csv");
            manager.Process();


            var header = "Page Replacement Simulation [LRU]".BuildFramedHeader();
            var statistics = "Statistics".BuildFramedHeader(15, "*");


            Console.WriteLine(header);
            Console.WriteLine($"Jobs: {manager.JobCount}  Transactions: {manager.TransactionCount}");

            Console.WriteLine(statistics);
            Console.WriteLine($"{"First Load".PadRight(15)}: {manager.Statistics.FirstLoad}");
            Console.WriteLine($"{"Page Hits".PadRight(15)}: {manager.Statistics.PageHits}");
            Console.WriteLine($"{"Page Faults".PadRight(15)}: {manager.Statistics.PageFaults}");
            Console.WriteLine($"{"Aborted Jobs".PadRight(15)}: {manager.Statistics.AbortedJobs}");

            manager.DisplayMemory();



            Console.ReadKey();
        }
    }
}