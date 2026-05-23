using Hypah.Logging;

internal class LogDispatcher : ILogDispatcher
{
    private readonly List<ILogReceiver> _receivers = new();
    private readonly Queue<LogMessage> _history = new();
    private readonly object _lock = new();

    public int MaxHistorySize { get; set; } = 1000;
    public bool IsEnabled { get; set; } = true;

    public void LogMessage(LogMessage message)
    {
        if (!IsEnabled) return;

        List<ILogReceiver> receiversSnapshot;

        lock (_lock)
        {
            _history.Enqueue(message);
            if (_history.Count > MaxHistorySize)
                _history.Dequeue();

            // Copy receivers so we don't call them inside the lock
            receiversSnapshot = new List<ILogReceiver>(_receivers);
        }

        // Deliver messages outside the lock
        foreach (var receiver in receiversSnapshot)
        {
            if (message.Level >= receiver.MinLevel)
            {
                try
                {
                    receiver.LogMessage(message);
                }
                catch (Exception ex)
                {
                    // Optional: internal fallback logging
                    Console.WriteLine($"Log receiver error: {ex}");
                }
            }
        }
    }
    
    public void RegisterReceiver(ILogReceiver receiver, bool receiveHistory)
    {
        List<LogMessage>? historySnapshot = null;

        lock (_lock)
        {
            _receivers.Add(receiver);

            if (receiveHistory)
                historySnapshot = new List<LogMessage>(_history);
        }

        // Replay history outside the lock
        if (historySnapshot != null)
        {
            foreach (var msg in historySnapshot)
            {
                if (msg.Level >= receiver.MinLevel)
                {
                    try
                    {
                        receiver.LogMessage(msg);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Log receiver error: {ex}");
                    }
                }
            }
        }
    }

    public void RemoveReceiver(ILogReceiver receiver)
    {
        lock (_lock)
        {
            _receivers.Remove(receiver);
        }
    }
}