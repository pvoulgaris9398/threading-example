namespace ThreadingWithDocker
{
    /// <summary>
    /// Formats and writes a specific message to a file
    /// Note: Manages multi-threaded access so that records are inserted
    /// with ascending row numbers, regardless of which thread is calling
    /// </summary>
    internal class FileLogWriter : ILogWriter
    {
        /// <summary>
        /// Running counter of rows written
        /// </summary>
        private int RowNumber { get; set; } = -1;

        /// <summary>
        /// Used to synchronize write access
        /// </summary>
        private object _synchLock = new object();

        /// <summary>
        /// The location of the log file
        /// </summary>
        private string LogFilePath { get; }
        
        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="logFilePath">Log file name and path</param>
        public FileLogWriter(string logFilePath)
        {
            LogFilePath = logFilePath;
        }

        /// <summary>
        /// Writes to the file, allowing only one thread to write at a time
        /// </summary>
        /// <param name="threadId">The currently executing thread's identifier</param>
        /// <param name="timeStamp">The current date and time</param>
        public void Write(int threadId, DateTime timeStamp)
        {
            lock (_synchLock)
            {
                RowNumber++;
                var line = string.Format("{0},{1},{2}\n", RowNumber, threadId, timeStamp.ToString("HH:mm:ss:fff"));
                File.AppendAllText(LogFilePath, line);
            }
        }
    }
}
