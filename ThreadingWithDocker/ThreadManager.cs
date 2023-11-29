
namespace ThreadingWithDocker
{
    internal class ThreadManager
    {
        private ILogWriter LogWriter { get;}
        public ThreadManager(ILogWriter writer)
        {
            LogWriter = writer;
        }

        //TODO: Get from Config, abstract the thread function, etc.
        public void Run()
        {
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
                        LogWriter.Write(Thread.CurrentThread.ManagedThreadId, DateTime.Now);
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
    }
}
