namespace Hypah.Logging.Loggers;

internal class ConsoleLogger : ILogReceiver
{
    public LogLevel MinLevel { get; set; } = LogLevel.Trace;

    public ConsoleLogger(LogLevel minLevel = LogLevel.Info)
    {
        MinLevel = minLevel;
    }

    public void LogMessage(LogMessage message)
    {
        string time = message.TimeStamp.ToString("HH:mm:ss.fff");
        string levelText = message.Level.ToString();
        char levelChar = levelText[0];

        bool isError = message.Level >= LogLevel.Error;

        string methodPart = isError ? $"{message.Method}\t" : "";
        string filePart = isError ? $"{message.Filename}\t{message.LineNumber}\t" : "";

        Console.WriteLine(
            $"{time}\t{levelChar}\t{message.Component}\t{methodPart}{filePart}{message.Message}"
        );
    }
}
