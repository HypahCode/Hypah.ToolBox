using Hypah.Logging.Loggers;
using System.Runtime.CompilerServices;
﻿
﻿namespace Hypah.Logging
﻿{
﻿    public static class Log
﻿    {
﻿        private static LogDispatcher? _instance;
﻿        public static ILogDispatcher Dispatcher
        {
﻿            get
﻿            {
﻿                if (_instance == null)
﻿                {
﻿                    _instance = new LogDispatcher();
﻿                    Write(LogLevel.Debug, nameof(Log), MessageOfTheDay.GetMessage());
﻿                }
﻿                return _instance;
﻿            }
﻿        }

        public static int MaxHistorySize 
        { 
            get => Dispatcher.MaxHistorySize; 
            set => Dispatcher.MaxHistorySize = value; 
        }

        public static void Trace(string component, string msg, [CallerMemberName] string? methodName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
﻿        {
            Dispatcher.LogMessage(new LogMessage(LogLevel.Trace, component, methodName ?? "", filePath ?? "", lineNumber, msg, DateTime.Now));
﻿        }
﻿
﻿        public static void Debug(string component, string msg, [CallerMemberName] string? methodName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
﻿        {
            Dispatcher.LogMessage(new LogMessage(LogLevel.Debug, component, methodName ?? "", filePath ?? "", lineNumber, msg, DateTime.Now));
﻿        }
﻿
﻿        public static void Info(string component, string msg, [CallerMemberName] string? methodName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
﻿        {
            Dispatcher.LogMessage(new LogMessage(LogLevel.Info, component, methodName ?? "", filePath ?? "", lineNumber, msg, DateTime.Now));
﻿        }
﻿
﻿        public static void Warning(string component, string msg, [CallerMemberName] string? methodName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
﻿        {
            Dispatcher.LogMessage(new LogMessage(LogLevel.Warn, component, methodName ?? "", filePath ?? "", lineNumber, msg, DateTime.Now));
﻿        }
﻿
﻿        public static void Error(string component, string msg, [CallerMemberName] string? methodName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
﻿        {
            Dispatcher.LogMessage(new LogMessage(LogLevel.Error, component, methodName ?? "", filePath ?? "", lineNumber, msg, DateTime.Now));
﻿        }
﻿
﻿        public static void Error(string component, Exception exception, [CallerMemberName] string? methodName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
﻿        {
            Dispatcher.LogMessage(new LogMessage(LogLevel.Error, component, methodName ?? "", filePath ?? "", lineNumber, exception.ToString(), DateTime.Now));
﻿        }
﻿
﻿        public static void Fatal(string component, string msg, [CallerMemberName] string? methodName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
﻿        {
            Dispatcher.LogMessage(new LogMessage(LogLevel.Fatal, component, methodName ?? "", filePath ?? "", lineNumber, msg, DateTime.Now));
﻿        }
﻿
﻿        public static void Write(LogLevel level, string component, string msg, [CallerMemberName] string? methodName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
﻿        {
            Dispatcher.LogMessage(new LogMessage(level, component, methodName ?? "", filePath ?? "", lineNumber, msg, DateTime.Now));
﻿        }
﻿
﻿        public static void Write(LogLevel level, object? component, string msg, [CallerMemberName] string? methodName = null, [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
﻿        {
            Dispatcher.LogMessage(new LogMessage(level, component?.GetType().Name ?.ToString() ?? "", methodName ?? "", filePath ?? "", lineNumber, msg, DateTime.Now));
﻿        }
﻿
﻿        public static void RegisterLogReceiver(ILogReceiver logReceiver, bool recieveHistory)
﻿        {
            Dispatcher.RegisterReceiver(logReceiver, recieveHistory);
﻿        }
﻿
﻿        public static void RemoveLogReceiver(ILogReceiver logReceiver)
﻿        {
            Dispatcher.RemoveReceiver(logReceiver);
﻿        }
﻿
﻿        public static void AddFileLogger(string filename, bool createNew = false)
﻿        {
﻿            if (createNew)
﻿            {
                Dispatcher.RegisterReceiver(FileLogger.CreateOnEmptyFile(filename), true);
﻿            }
﻿            else
﻿            {
                Dispatcher.RegisterReceiver(FileLogger.AppendToFile(filename), true);
﻿            }
﻿        }
﻿
﻿        public static void AddLogFiles(string directory)
﻿        {
            Dispatcher.RegisterReceiver(FileLogger.AutoCreate(directory), true);
﻿        }
﻿
﻿        public static void AddConsoleLogger(LogLevel initialMinLevel = LogLevel.Info)
﻿        {
            Dispatcher.RegisterReceiver(new ConsoleLogger(initialMinLevel), true);
﻿        }
﻿    }
﻿}
﻿
