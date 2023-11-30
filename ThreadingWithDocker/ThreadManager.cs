namespace ThreadingWithDocker
{
    /// <summary>
    /// A class that manages an arbitrary number of threads and performs
    /// the action indicated by the <see cref="ThreadFunction"</see> delegate
    /// </summary>
    internal class ThreadManager
    {
        /// <summary>
        /// An reference to the <see cref="ILogWriter"</see> interface
        /// </summary>
        private ILogWriter LogWriter { get;}

        /// <summary>
        /// Number of threads to be managed by this class
        /// </summary>
        public int NumberOfThreads { get; }

        /// <summary>
        /// Number of iterations to perform per thread
        /// Note: This is really implementation dependent
        /// </summary>
        public int NumberOfIterationsPerThread { get; }

        /// <summary>
        /// A callback method used to log messages
        /// Note: Implemented by the calling function
        /// </summary>
        protected Action<string> LogMessageCallback { get; }

        /// <summary>
        /// A callback method used to log error messages
        /// Note: Implemented by the calling function
        /// </summary>
        protected Action<string> LogErrorCallback { get; }

        /// <summary>
        /// The specific thread function implementation
        /// </summary>
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

        /// <summary>
        /// The constructor for this class
        /// </summary>
        /// <param name="numberOfThreads">Number of threads to be managed</param>
        /// <param name="numberOfIterationsPerThread">Number of iterations per thread</param>
        /// <param name="writer">The ILogWriter implementation to be used</param>
        /// <param name="logMessageCallback">Used to log messages back to the caller</param>
        /// <param name="logErrorCallback">Used to log errors back to the caller</param>
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

        /// <summary>
        /// The method that kicks off the processing by the managed threads
        /// </summary>
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
