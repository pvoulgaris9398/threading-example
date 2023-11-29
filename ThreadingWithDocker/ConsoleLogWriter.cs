namespace ThreadingWithDocker
{
    internal class ConsoleLogWriter : ILogWriter
    {
        private int RowNumber { get; set;} = -1;
        private static object _synchLock = new object();
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
