using System;
using System.Diagnostics;
using System.Threading;

namespace ThreadOverheadExample
{
    class Program
    {
        static ManualResetEvent wakeThreads = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            ThreadOverhead();

            Console.Read();
        }

        private static void ThreadOverhead()
        {
            const Int32 OneMB = 1024 * 1024;
            int threadNum = 0;
            try
            {
                while (true)
                {
                    Thread t = new Thread(WaitOnEvent);
                    t.Start(wakeThreads);
                    Console.WriteLine("{0}: {1}MB", ++threadNum,
                        Process.GetCurrentProcess().PrivateMemorySize64 / OneMB);
                }
            }
            catch (OutOfMemoryException)
            {
                Console.WriteLine("Out of memory after {0} threads", threadNum);
                Debugger.Break();
                wakeThreads.Set(); // Release all the threads
            }
        }

        private static void WaitOnEvent(object obj)
        {
            wakeThreads.WaitOne();
        }
    }
}