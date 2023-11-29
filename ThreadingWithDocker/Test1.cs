namespace ThreadingWithDocker
{
    internal class Test1
    {
        internal static void Run()
        {
            Task.Run(() => MyAsyncFunction());
        }
        static async Task MyAsyncFunction()
        {
            foreach (var i in Enumerable.Range(0, 10))
            {
                await Task.Run(() =>
                {
                    foreach (var j in Enumerable.Range(0, 10))
                    {
                        Write();
                    }
                });
            }
        }
        static int Counter;

        private static object _synchLock = new object();
        static void Write()
        {
            lock (_synchLock)
            {
                Counter++;
                var threadId = Thread.CurrentThread.ManagedThreadId;
                var line = string.Format("{0},{1},{2}", Counter, threadId, DateTime.Now.ToString("HH:mm:ss:fff"));
                Console.WriteLine(line);
            }
        }
    }
}
