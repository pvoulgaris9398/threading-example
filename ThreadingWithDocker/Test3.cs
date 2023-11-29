namespace ThreadingWithDocker
{
    internal class Test3
    {
        internal void Initialize()
        {
            /* SOME NOTES
             * 1. Create class to manage an arbitrary number of threads
             * 2. Class should accept the function to be called by each thread
             *    (Refactor current implementation to wrap calls to Console.WriteLine
             *    perhaps called ConsoleLogger? and then also implement FileLogger).
             * 3. Implement thread management, then call this function (or class implementation)
             * 4. Write class to implement writing to the file
             * 5. It will first attempt to open the file, create if not exists
             * 6. Initialize with the first log entry
             * 7. Then allow each thread to write it's message
             */
            
        }
        internal static void Run()
        {

            //1. Open the file, handle errors as appropriate

            //2. Write the first entry
            Write(0, DateTime.Now);

            //3. Create threads, handles, etc. and kick them off
            int numOfThreads = 10;
            WaitHandle[] waitHandles = new WaitHandle[numOfThreads];
            var threadStartEvent = new ManualResetEvent(false);

            for (int i = 0; i < numOfThreads; i++)
            {
                var j = i;

                var threadCompletedEvent = new EventWaitHandle(false, EventResetMode.ManualReset);

                var thread = new Thread(() =>
                {
                    threadStartEvent.WaitOne();
                    foreach (var k in Enumerable.Range(0, 10))
                    {
                        Write(Thread.CurrentThread.ManagedThreadId, DateTime.Now);
                    }
                    threadCompletedEvent.Set();
                });
                thread.Name = string.Format("Thread{0}", i);
                waitHandles[j] = threadCompletedEvent;
                thread.Start();

            }
            threadStartEvent.Set();
            WaitHandle.WaitAll(waitHandles);
        }

        private static int Counter = 0;

        private static object _synchLock = new object();
        private static void Write(int threadId, DateTime timeStamp)
        {
            lock (_synchLock)
            {
                var line = string.Format("{0},{1},{2}", Counter, threadId, timeStamp.ToString("HH:mm:ss:fff"));
                Console.WriteLine(line);
                Counter++;
            }
        }
    }
}
