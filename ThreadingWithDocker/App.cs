namespace ThreadingWithDocker
{
    internal class App
    {
        public string FilePath { get; }
        public int NumberOfThreads { get; }
        public int NumberOfIterationsPerThread { get; }
        public ILogWriter LogWriter { get; private set; }
        public ThreadManager ThreadManager { get; }
        protected Action<string> LogMessageCallback { get; }
        protected Action<string> LogErrorCallback { get; }

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
