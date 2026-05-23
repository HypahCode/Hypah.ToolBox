
namespace Hypah.Logging
{
    public struct LogMessage
    {
        public LogMessage(LogLevel level, string component, string method, string filename, int lineNumber, string msg, DateTime timeStamp) : this()
        {
            Level = level;
            Component = component;
            Method = method;
            Filename = filename;
            LineNumber = lineNumber;
            Message = msg;
            TimeStamp = timeStamp;
        }

        public string Message { get; }
        public string Component { get; }
        public string Method { get; }
        public string Filename { get; }
        public int LineNumber { get; }
        public LogLevel Level { get; }
        public DateTime TimeStamp { get; }
    }
}
