namespace Hypah.Logging
{
    public interface ILogDispatcher
    {
        int MaxHistorySize { get; set; }
        bool IsEnabled { get; set; }
        void LogMessage(LogMessage message);
        void RegisterReceiver(ILogReceiver logReceiver, bool recieveHistory);
        void RemoveReceiver(ILogReceiver logReceiver);
    }
}
