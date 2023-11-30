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
            WaitHandle[] waitHandles = new WaitHandle[NumberOfThreads];
            var threadStartEvent = new ManualResetEvent(false);
            
            for (int i = 0; i < NumberOfThreads; i++)
            {
                var j = i;

                var threadCompletedEvent = new EventWaitHandle(false, EventResetMode.ManualReset);

                var thread = new Thread(
                    () => ThreadFunction(
                        threadStartEvent,
                        threadCompletedEvent,
                        NumberOfIterationsPerThread,
                        LogWriter,
                        LogErrorCallback));
                    /*
                    () =>
                {
                    try
                    {
                        threadStartEvent.WaitOne();
                        foreach (var k in Enumerable.Range(0, NumberOfIterationsPerThread))
                        {
                            LogWriter.Write(Thread.CurrentThread.ManagedThreadId, DateTime.Now);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogErrorCallback(ex.ToString());
                    }
                    finally
                    {
                        threadCompletedEvent.Set();
                    }
                }
                */
                
                thread.Name = string.Format("Thread{0}", i);
                waitHandles[j] = threadCompletedEvent;
                thread.Start();

            }
            threadStartEvent.Set();
            WaitHandle.WaitAll(waitHandles);
        }
    }
}
