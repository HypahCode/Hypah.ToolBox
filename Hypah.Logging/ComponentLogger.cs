using System.Runtime.CompilerServices;

namespace Hypah.Logging
{
    public class ComponentLogger : IComponentLogger
    {
        private readonly string _component;
        private readonly LogLevel _defaultLevel;

        public ComponentLogger(string component, LogLevel defaultLevel = LogLevel.Debug) 
        {
            _component = component;
            _defaultLevel = defaultLevel;
        }
        
        public ComponentLogger(object component, LogLevel defaultLevel = LogLevel.Debug) 
            : this(component.ToString()!, defaultLevel)
        {
        }

        public void Trace(string msg, [CallerMemberName] string? methodName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            Log.Trace(_component, msg, methodName ?? "", filePath ?? "", lineNumber);
        }

        public void Debug(string msg, [CallerMemberName] string? methodName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            Log.Debug(_component, msg, methodName ?? "", filePath ?? "", lineNumber);
        }

        public void Info(string msg, [CallerMemberName] string? methodName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            Log.Info(_component, msg, methodName ?? "", filePath ?? "", lineNumber);
        }

        public void Warning(string msg, [CallerMemberName] string? methodName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            Log.Warning(_component, msg, methodName ?? "", filePath ?? "", lineNumber);
        }

        public void Error(string msg, [CallerMemberName] string? methodName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            Log.Error(_component, msg, methodName ?? "", filePath ?? "", lineNumber);
        }

        public void Error(string msg, Exception exception, [CallerMemberName] string? methodName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            Log.Error(_component, msg + " " + exception.ToString(), methodName ?? "", filePath ?? "", lineNumber);
        }

        public void Error(Exception exception, [CallerMemberName] string? methodName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            Log.Error(_component, exception, methodName ?? "", filePath ?? "", lineNumber);
        }
        public void Fatal(string msg, [CallerMemberName] string? methodName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            Log.Fatal(_component, msg, methodName ?? "", filePath ?? "", lineNumber);
        }

        public void Write(LogLevel level, string msg, [CallerMemberName] string? methodName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            Log.Write(level, _component, msg, methodName ?? "", filePath ?? "", lineNumber);
        }

        public void Write(string msg, [CallerMemberName] string? methodName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
        {
            Log.Write(_defaultLevel, _component, msg, methodName ?? "", filePath ?? "", lineNumber);
        }
    }
}
