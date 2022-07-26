using System;
using System.Collections.Generic;
using System.IO;

namespace PageManagerApp
{
    public class PageManager
    {
        private const int Terminator = -999;
        private readonly Dictionary<int, List<Page>> _jobLookup;
        private readonly Queue<Job> _jobQueue = new Queue<Job>();
        private readonly DoublyLinkedList<Page> _physicalMemory;
        private readonly DoublyLinkedList<Page> _swapMemory;
        private int _lastTimeStamp;
       private readonly List<int> _abortedJobs = new List<int>();


        public PageManager(int memoryCapacity, int swapCapacity)
        {
            _physicalMemory = new DoublyLinkedList<Page>(memoryCapacity);
            _swapMemory = new DoublyLinkedList<Page>(swapCapacity);
            _jobLookup = new Dictionary<int, List<Page>>(memoryCapacity + swapCapacity);
            Statistics = new PageStatistics();
            _lastTimeStamp = 0;
        }

        public PageStatistics Statistics { get; }

        public bool IsMemoryFull => _physicalMemory.IsFull && _swapMemory.IsFull;
        public bool IsPhysicalMemoryFull => _physicalMemory.IsFull;
        public bool IsSwapMemoryFull => _swapMemory.IsFull;

        public int JobCount { get; private set; }
        public int TransactionCount { get; private set; }

        public void DisplayMemory()
        {
            _physicalMemory.Print("Physical Memory");
            _swapMemory.Print("Swap Memory");
        }

        /// <summary>
        /// Loads the job queue from a file
        /// </summary>
        /// <param name="fileName">file name including path</param>
        public void LoadFromFile(string fileName)
        {
            try
            {
                using (var reader = new StreamReader(fileName))
                {
                    TransactionCount = 0;
                    JobCount = 0;
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (line == null) continue;
                        var values = line.Split(',');
                        var job = new Job(int.Parse(values[0]), int.Parse(values[1]));
                        QueueJob(job);
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("The file could not be found. Please check the path.");
                throw e;
            }
        }

        /// <summary>
        /// Loads a page to memory
        /// </summary>
        /// <param name="page"></param>
        public void LoadPage(Page page)
        {
            if (IsMemoryFull)
            {
                Statistics.IncrementAbortedJobs();
                _abortedJobs.Add(page.Job.JobNumber);
                UnloadMemory(page.Job.JobNumber);

                Console.WriteLine($"Out of Memory. Job: {page.Job.JobNumber} Page: {page.Job.PageNumber}");
                return;
            }

            if (!IsInMemory(page))
            {
                Statistics.IncrementFirstLoad();
                if (_physicalMemory.IsFull)
                    MoveToSwap();

                _physicalMemory.Append(page);
            }
            else
            {
                if (IsInPhysicalMemory(page))
                {

                    // Page exists in physical memory
                    Statistics.IncrementPageHit();
                    _physicalMemory.MoveToBack(page);
                }
                else
                {
                    Statistics.IncrementPageFault();
                    if (!IsPhysicalMemoryFull)
                    {
                        // Page is in swap memory and
                        //  there is room in physical memory
                        MoveFromSwap(page);
                    }
                    else
                    {
                        // Page exists in swap memory
                        // and there is no room in physical memory
                        // so make room by copying data to swap
                        // move swap to physical

                        MoveToSwap();
                        MoveFromSwap(page);
                    }
                }
            }
        }

        /// <summary>
        ///     Processes all queued jobs
        /// </summary>
        public void Process()
        {
            // Process all jobs
            foreach (var job in _jobQueue)
            {
                //Skip aborted jobs
                if (_abortedJobs.Contains(job.JobNumber))
                    continue;

                if (job.PageNumber == Terminator)
                {                    
                    UnloadMemory(job.JobNumber);
                    continue;
                }
  

                var page = new Page(job, _lastTimeStamp++);
                _jobLookup[job.JobNumber].Add(page);
                LoadPage(page);
            }
        }

        /// <summary>
        /// Adds jobs to the job queue.
        /// </summary>
        /// <param name="job"></param>
        public void QueueJob(Job job)
        {
            _jobQueue.Enqueue(job);
            TransactionCount++;
            // Populate job lookup with available jobs
            if (job.PageNumber == Terminator)
                return;

            if (_jobLookup.ContainsKey(job.JobNumber)) return;

            _jobLookup.Add(job.JobNumber, new List<Page>());
            JobCount++;
        }

        /// <summary>
        /// Adds a list of jobs to be queued
        /// </summary>
        /// <param name="jobs"></param>
        public void QueueJob(List<Job> jobs)
        {
            foreach (var job in jobs) QueueJob(job);
        }

   
        /// <summary>
        /// Resets the Page Manager 
        /// </summary>
        public void Reset()
        {
            _swapMemory.Clear();
            _physicalMemory.Clear();
            _jobLookup.Clear();
            _abortedJobs.Clear();
            Statistics.Reset();
            _lastTimeStamp = 0;
        }
        /// <summary>
        /// Finds a node in memory or swap
        /// </summary>
        /// <param name="page">Data to search for</param>
        /// <returns></returns>
        private ListNode<Page> Find(Page page)
        {
            if (_physicalMemory.Contains(page))
                return _physicalMemory.Find(page);
            return _swapMemory.Contains(page) ? _swapMemory.Find(page) : null;
        }

        /// <summary>
        /// Check if a page is in memory
        /// </summary>
        /// <param name="page">Data to search for</param>
        /// <returns>True, if in memory</returns>
        private bool IsInMemory(Page page)
        {
            return _physicalMemory.Contains(page) || _swapMemory.Contains(page);
        }


        /// <summary>
        /// Indicates if the page is in physical memory
        /// </summary>
        /// <param name="page">Data to search for</param>
        /// <returns>True, if in physical memory</returns>
        private bool IsInPhysicalMemory(Page page)
        {
            return _physicalMemory.Contains(page);
        }

        /// <summary>
        /// Indicates if the page is in swap memory
        /// </summary>
        /// <param name="page">True, if in Swap memory</param>
        /// <returns></returns>
        private bool IsInSwapMemory(Page page)
        {
            return _swapMemory.Contains(page);
        }

        /// <summary>
        /// Moves a page from Swap to Physical memory
        /// </summary>
        /// <param name="page">Page to be moved</param>
        private void MoveFromSwap(Page page)
        {
            _physicalMemory.Append(page);
            _swapMemory.RemoveByValue(page);
        }

        /// <summary>
        /// Moves a page from Physical memory to Swap memory
        /// </summary>
        private void MoveToSwap()
        {
            var node = _physicalMemory.FirstNode;
            _swapMemory.Append(node.Data);
            _physicalMemory.Remove(node);
        }

        /// <summary>
        /// Removes all pages for the associated job
        /// </summary>
        /// <param name="jobNumber">Job number for the pages to be removed</param>
        private void UnloadMemory(int jobNumber)
        {
            var pages = _jobLookup[jobNumber];
            foreach (var page in pages)
            {
                _physicalMemory.RemoveByValue(page);
                _swapMemory.RemoveByValue(page);
            }
        }
    }
}