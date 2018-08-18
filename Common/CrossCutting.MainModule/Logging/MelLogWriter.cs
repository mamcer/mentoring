namespace CrossCutting.MainModule.Logging
{
    using System;
    using System.Diagnostics;
    using System.Text;
    using Core.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Logging;
    
    public class MelLogWriter : ILogWriter
    {
        public MelLogWriter(TraceEventType severity)
        {
            var logWriterFactory = new LogWriterFactory();
            Writer = logWriterFactory.Create();
            LogSeverity = severity;
        }

        private LogWriter Writer
        {
            get;
            set;
        }

        private TraceEventType LogSeverity
        {
            get;
            set;
        }

        public void Write(string message)
        {
            Writer.Write(CreateLogEntry(Category.General, message, Priority.Normal));
        }

        public void Write(string message, Exception ex)
        {
            var completeMessage = new StringBuilder();

            completeMessage.Append(message);
            if (ex != null)
            {
                completeMessage.AppendLine("Message:");
                completeMessage.AppendLine(ex.Message);
                completeMessage.AppendLine("Stack Trace:");
                completeMessage.AppendLine(ex.StackTrace);

                if (ex.InnerException != null)
                {
                    completeMessage.AppendLine("Inner Exception Message:");
                    completeMessage.AppendLine(ex.InnerException.Message);
                    completeMessage.AppendLine("Inner Exception Stack Trace:");
                    completeMessage.AppendLine(ex.InnerException.StackTrace);
                }
            }

            Writer.Write(CreateLogEntry(Category.General, completeMessage.ToString(), Priority.Normal));
        }

        private LogEntry CreateLogEntry(string category, string message, int priority)
        {
            var logEntry = new LogEntry();
            logEntry.Categories.Add(category);
            logEntry.Message = message;
            logEntry.Priority = priority;
            logEntry.Severity = LogSeverity;
            
            return logEntry;
        }
    }
}