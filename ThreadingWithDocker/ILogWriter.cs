namespace ThreadingWithDocker
{
    internal interface ILogWriter
    {
        void Write(int threadId, DateTime timeStamp);
    }
}
