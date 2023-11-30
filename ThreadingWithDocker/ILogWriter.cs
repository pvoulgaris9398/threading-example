namespace ThreadingWithDocker
{
    /// <summary>
    /// The interface that will be implemented for writing
    /// messages containing thread identifiers and their timestamps
    /// </summary>
    internal interface ILogWriter
    {
        /// <summary>
        /// Write a message
        /// </summary>
        /// <param name="threadId">Identifier of the currently executing thread</param>
        /// <param name="timeStamp">Current date and time</param>
        void Write(int threadId, DateTime timeStamp);
    }
}
