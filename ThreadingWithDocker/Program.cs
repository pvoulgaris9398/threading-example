
namespace ThreadingWithDocker
{
    internal class Program
    {
        private static object _logErrorCallbackLockObject = new object();
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

        private static object _logMessageCallbackLockObject = new object();

        private static Action<string> LogMessage = msg =>
        {
            lock (_logMessageCallbackLockObject)
            {
                Console.WriteLine(msg);
            }
        };

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