namespace ThreadingWithDocker
{
    /// <summary>
    /// Used for initial debugging and writing output to the console, as needed
    /// Note: Manages multi-threaded access so that records are inserted
    /// with ascending row numbers, regardless of which thread is calling
    /// </summary>
    internal class ConsoleLogWriter : ILogWriter
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
        /// Writes to the console
        /// </summary>
        /// <param name="threadId">The currently executing thread's identifier</param>
        /// <param name="timeStamp">The current date and time</param>
        public void Write(int threadId, DateTime timeStamp)
        {
            lock (_synchLock)
            { 
                RowNumber++;
                var line = string.Format("{0},{1},{2}", RowNumber, threadId, timeStamp.ToString("HH:mm:ss:fff"));
                Console.WriteLine(line);
            }
        }
    }
}
