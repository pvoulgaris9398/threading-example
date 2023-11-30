namespace ThreadingWithDocker
{
    internal class ThreadManager
    {
        private ILogWriter LogWriter { get;}
        public int NumberOfThreads { get; }
        public int NumberOfIterationsPerThread { get; }
        protected Action<string> LogMessageCallback { get; }
        protected Action<string> LogErrorCallback { get; }

        private static Action<ManualResetEvent, EventWaitHandle, int, ILogWriter, Action<string>> ThreadFunction
            = (threadStartEvent, threadCompletedEvent, numberOfIterationsPerThread, logWriter, logErrorCallback)
            =>
        {
            try
            {
                threadStartEvent.WaitOne();
                foreach (var k in Enumerable.Range(0, numberOfIterationsPerThread))
                {
                    logWriter.Write(Thread.CurrentThread.ManagedThreadId, DateTime.Now);
                }
            }
            catch (Exception ex)
            {
                logErrorCallback(ex.ToString());
            }
            finally
            {
                threadCompletedEvent.Set();
            }
        };

        public ThreadManager(
            int numberOfThreads,
            int numberOfIterationsPerThread,
            ILogWriter writer,
            Action<string> logMessageCallback,
            Action<string> logErrorCallback)
        {
            NumberOfThreads = numberOfThreads;
            NumberOfIterationsPerThread = numberOfIterationsPerThread;
            LogWriter = writer;
            LogMessageCallback = logMessageCallback;
            LogErrorCallback = logErrorCallback;
        }

        public void Run()
        {
            var waitHandles = new List<EventWaitHandle>();
            var threadStartEvent = new ManualResetEvent(false);

            foreach (var index in Enumerable.Range(0, NumberOfThreads))
            {
                var threadCompletedEvent = new EventWaitHandle(false, EventResetMode.ManualReset);
                waitHandles.Add(threadCompletedEvent);

                var thread = new Thread(
                    () => ThreadFunction(
                        threadStartEvent,
                        threadCompletedEvent,
                        NumberOfIterationsPerThread,
                        LogWriter,
                        LogErrorCallback));

                thread.Name = string.Format("Thread{0}", index);
                thread.Start();
            }
          
            threadStartEvent.Set();
            WaitHandle.WaitAll(waitHandles.ToArray());
        }
    }
}
