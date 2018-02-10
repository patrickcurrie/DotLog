using System.Diagnostics;

namespace DotLog
{
    class EventLogger : DotLogger
    {
        public string LogName { get; private set; }
        public string EventSource { get; private set; }

        public EventLogger(string logName, string eventSource)
        {
            LogName = logName;
            EventSource = eventSource;
        }

        public override void Log(string message)
        {
            lock (threadLock)
            {
                EventLog eventLog = new EventLog(LogName);
                eventLog.Source = EventSource;
                eventLog.WriteEntry(message);
            }
        }
    }
}
