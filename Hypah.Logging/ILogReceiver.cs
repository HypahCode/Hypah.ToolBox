namespace Hypah.Logging
{
    public interface ILogReceiver
    {
        LogLevel MinLevel { get; set; }
        void LogMessage(LogMessage message);
    }
}