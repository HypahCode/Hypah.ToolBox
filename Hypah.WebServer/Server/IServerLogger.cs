
namespace Hypah.WebServer.Server
{
    public enum LogLevel { Trace, Debug, Info, Warn, Error }

    public interface IServerLogger
    {
        public void Log(LogLevel level, string component, string message);
    }
}
