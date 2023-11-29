namespace ThreadingWithDocker
{
    internal class Test2
    {
        internal static void Run()
        {

            //1. Open the file, handle errors as appropriate

            //2. Write the first entry
            Write(0, DateTime.Now);

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
