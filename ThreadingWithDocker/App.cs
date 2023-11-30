namespace ThreadingWithDocker
{
    /// <summary>
    /// This is the main driver class that would encapsulate retrieving settings
    /// from a configuration store and implementation of the program's functionality
    /// </summary>
    internal class App
    {
        /// <summary>
        /// Fully-qualified path and file name
        /// </summary>
        public string FilePath { get; }

        /// <summary>
        /// Number of required threads
        /// </summary>
        public int NumberOfThreads { get; }

        /// <summary>
        /// Number of iterations for each thread
        /// </summary>
        public int NumberOfIterationsPerThread { get; }

        /// <summary>
        /// The contract for the Log Writer functionality
        /// </summary>
        public ILogWriter LogWriter { get; private set; }

        /// <summary>
        /// The class that will manage thread lifetime and associated functionality
        /// </summary>
        public ThreadManager ThreadManager { get; }

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
        /// The constructor
        /// </summary>
        /// <param name="logMessageCallback">Used to log messages back to the caller</param>
        /// <param name="logErrorCallback">Used to log errors back to the caller</param>
        public App(Action<string> logMessageCallback, Action<string> logErrorCallback)
        {
            /*
             * TODO: Get config from appsettings.json/Environment Variable(s), etc.
             */
            LogMessageCallback = logMessageCallback;
            LogErrorCallback = logErrorCallback;
            FilePath = "/log/out.txt";
            InitializeWriter();
            NumberOfThreads = 10;
            NumberOfIterationsPerThread = 10;
            ThreadManager = new ThreadManager(NumberOfThreads,NumberOfIterationsPerThread, LogWriter, logMessageCallback, logErrorCallback);
        }

        /// <summary>
        /// Attempts to write the file output target, handles any potential errors,
        /// and gracefully falls back to writing to the console
        /// </summary>
        protected void InitializeWriter()
        {
            LogWriter = new FileLogWriter(FilePath);

            try
            {
                // Attempt to write initial record to confirm file access
                LogWriter.Write(0, DateTime.Now);
            }
            catch (Exception ex)
            {
                // If not, log message, instantiate fallback writer and write initial record
                LogErrorCallback(string.Format($"Unable to write to file: '{FilePath}'\nError: '{ex.Message}'\nFalling back to {nameof(ConsoleLogWriter)}..."));
                LogWriter = new ConsoleLogWriter();
                LogWriter.Write(0, DateTime.Now);
            }
        }

        /// <summary>
        /// The method used to call this applications specific functionality
        /// </summary>
        public void Run()
        {            
            try
            { 
                ThreadManager.Run();
            }
            catch (Exception ex)
            {
                LogErrorCallback(string.Format($"Error: {ex.Message}"));
            }
        }
    }
}
