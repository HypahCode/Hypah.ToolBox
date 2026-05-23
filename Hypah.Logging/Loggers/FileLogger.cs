namespace Hypah.Logging.Loggers;

internal class FileLogger : ILogReceiver, IDisposable
{
    private readonly object _lock = new();
    private StreamWriter _writer;
    private readonly string _directory;
    private string _fileName;
    private readonly bool _autoRotate;
    private DateTime _currentDate;

    public LogLevel MinLevel { get; set; } = LogLevel.Trace;

    private FileLogger(string fileName, string? directory = null, bool autoRotate = false)
    {
        _fileName = fileName;
        _directory = directory ?? Path.GetDirectoryName(fileName)!;
        _autoRotate = autoRotate;
        _currentDate = DateTime.UtcNow.Date;

        Directory.CreateDirectory(_directory);
        _writer = new StreamWriter(new FileStream(_fileName, FileMode.Append, FileAccess.Write, FileShare.Read))
        {
            AutoFlush = true
        };
    }

    public static FileLogger CreateOnEmptyFile(string fileName)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(fileName)!);
        File.WriteAllText(fileName, string.Empty);
        return new FileLogger(fileName);
    }

    public static FileLogger AppendToFile(string fileName)
    {
        return new FileLogger(fileName);
    }

    public static FileLogger AutoCreate(string directory)
    {
        var fileName = CreateNewFileName(directory);
        return new FileLogger(fileName, directory, autoRotate: true);
    }

    public void LogMessage(LogMessage message)
    {
        try
        {
            lock (_lock)
            {
                if (_autoRotate && message.TimeStamp.Date != _currentDate)
                {
                    Rotate(message.TimeStamp.Date);
                }

                _writer.WriteLine(
                    $"{message.TimeStamp:O}\t{message.Level}\t{message.Component}\t{message.Method}\t{message.Message}");
            }
        }
        catch
        {
            // swallow — logging must never throw
        }
    }

    private void Rotate(DateTime newDate)
    {
        _writer.Dispose();
        _fileName = CreateNewFileName(_directory);
        _writer = new StreamWriter(new FileStream(_fileName, FileMode.Append, FileAccess.Write, FileShare.Read))
        {
            AutoFlush = true
        };
        _currentDate = newDate;
    }

    private static string CreateNewFileName(string directory)
    {
        Directory.CreateDirectory(directory);
        return Path.Combine(directory, $"{DateTime.UtcNow:yyyy-MM-dd}.txt");
    }

    public void Dispose()
    {
        lock (_lock)
        {
            _writer.Dispose();
        }
    }
}