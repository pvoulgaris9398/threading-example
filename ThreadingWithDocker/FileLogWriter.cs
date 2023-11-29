namespace ThreadingWithDocker
{
    internal class FileLogWriter : ILogWriter
    {
        private int RowNumber { get; set; } = -1;
        private static object _synchLock = new object();
        private string LogFilePath { get; }
        public FileLogWriter(string logFilePath)
        {
            LogFilePath = logFilePath;
        }

        public void Write(int threadId, DateTime timeStamp)
        {
            /*TODO: 
             * Handle errors...
             * Perhaps try to create the directory if it doesn't exist?
             * Overwrite existing file if or delete existing file first?
             * Handle running from Windows Console App and Docker container gracefully
             * 
             */
            lock (_synchLock)
            {
                RowNumber++;
                var line = string.Format("{0},{1},{2}\n", RowNumber, threadId, timeStamp.ToString("HH:mm:ss:fff"));
                File.AppendAllText(LogFilePath, line);
            }
        }
    }
}
