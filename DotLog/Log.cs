using System.Collections.Generic;

namespace DotLog
{
    public static class Log
    {
        private static DotLogger logger = null;
        
        /// <summary>
        /// Log to a file.
        /// </summary>
        /// <param name="path">The path of the file that will be logged to.</param>
        /// <param name="message">The message that will be logged to file.</param>
        public static void FileLog(string path, string message)
        {
            logger = new FileLogger(path);
            logger.Log(message);
        }

        /// <summary>
        /// Log to a Windows event log. Applications and services should write to the Application log or a custom log. Device drivers should write to the System log. Before writing an entry to an event log, you must register the event source with the event log as a valid source of events. If you are reading the event log, you can either specify the Source, or a Log and MachineName.
        /// </summary>
        /// <param name="logName">The name of the event log to log to.</param>
        /// <param name="logSource">Indicates what logs the event. It is often by convention the name of the application or the name of a subcomponent of the application if the application is large. When you write a log entry the system uses this parameter to find the appropriate log in which to place your entry.</param>
        /// <param name="message">The message that will be logged to the event log.</param>
        public static void EventLog(string logName, string logSource, string message)
        {
            logger = new EventLogger(logName, logSource);
            logger.Log(message);
        }

        /// <summary>
        /// Log object states to an SQL Server database. Each object should have its own table the columns of each table should be the names of that object's properties.
        /// </summary>
        /// <param name="connectionString">The database connection string.</param>
        /// <param name="tableName">The name of the table to log to.</param>
        /// <param name="propertyDictonary">A dictionary that has an object's property names as dictionary keys and the values of those properties as corresponding dictoinary values.</param>
        public static void DatabaseLog(string connectionString, string tableName, Dictionary<string, string> propertyDictonary)
        {
            logger = new DatabaseLogger(connectionString, tableName);
            logger.Log(propertyDictonary);
        }
    }
}
