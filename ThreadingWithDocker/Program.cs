
namespace ThreadingWithDocker
{
    /// <summary>
    /// The internal Program wrapper/console app
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Synchronizes access to <see cref="LogError"/>
        /// </summary>
        private static object _logErrorCallbackLockObject = new object();

        /// <summary>
        /// Writes an error message to the Console, with color-coding
        /// </summary>
        private static Action<string> LogError = error =>
        {
            lock (_logErrorCallbackLockObject)
            {
                var original = Console.ForegroundColor;
                try
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(error);
                }
                finally
                {
                    Console.ForegroundColor = original;
                }
            }
        };

        /// <summary>
        /// Synchronizes access to <see cref="LogMessage"/>
        /// </summary>
        private static object _logMessageCallbackLockObject = new object();

        /// <summary>
        /// Writes a log message to the Console, without any color-coding
        /// </summary>
        private static Action<string> LogMessage = msg =>
        {
            lock (_logMessageCallbackLockObject)
            {
                Console.WriteLine(msg);
            }
        };

        /// <summary>
        /// This program's main function
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            // TODO: Use structure logging framework here instead of the log callback implementation
            try
            {
                var app = new App(LogMessage, LogError);
                app.Run();
            }
            catch (Exception ex)
            {
                LogError($"Unhandled Exception\n{ex.Message}");
            }

            Console.WriteLine("Press any key to continue...");
            Console.Read();
        }      
    }
}