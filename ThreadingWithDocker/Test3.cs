using System.IO;
namespace ThreadingWithDocker
{
    internal class Test3
    {
        internal void Initialize()
        {
            /* SOME NOTES
             * 1. Create class to manage an arbitrary number of threads
             * 2. Class should accept the function to be called by each thread
             *    (Refactor current implementation to wrap calls to Console.WriteLine
             *    perhaps called ConsoleLogger? and then also implement FileLogger).
             * 3. Implement thread management, then call this function (or class implementation)
             * 4. Write class to implement writing to the file
             * 5. It will first attempt to open the file, create if not exists
             * 6. Initialize with the first log entry
             * 7. Then allow each thread to write it's message
             */
            
        }
        internal static void Run()
        {
            // TODO: Get from config/DI
            var filePath = "/log/out.txt";
            //var filePath = "out.txt";

            ILogWriter writer = new FileLogWriter(filePath);
            //ILogWriter writer = new ConsoleLogWriter();

            writer.Write(0, DateTime.Now);

            ThreadManager manager = new ThreadManager(10, 10, writer, s => { }, e => { });

            manager.Run();         
        }
    }
}
