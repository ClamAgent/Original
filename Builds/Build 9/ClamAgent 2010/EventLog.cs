using System;
using System.Collections.Generic;
using System.Text;

using System.Diagnostics;
using System.IO;

namespace ClamAgent
{
    class Log
    {
        public static string GlobalSource;
        public static void Register(string FileName, string Source)
        {
            Log.Register(FileName, Source, "Application");
        }
        public static void Register(string FileName, string Source, string LogName)
        {
            EventSourceCreationData EventSource;
            Log.GlobalSource = Source;
            if (!EventLog.SourceExists(Source))
            {
                EventSource = new EventSourceCreationData(Source, LogName);
                if (File.Exists(FileName))
                {
                    EventSource.MessageResourceFile = FileName;
                }
                EventLog.CreateEventSource(EventSource);
            }
        }
        public static void UnRegister(string Source)
        {
            if (EventLog.SourceExists(Source))
            {
                EventLog.DeleteEventSource(Source);
            }
        }
        public static void Write(EventLogMessages id, string[] InsertionStrings)
        {
            Log.Write(Log.GlobalSource, id, InsertionStrings);
        }
        public static void Write(string Source, EventLogMessages id, string[] InsertionStrings)
        {
            if (EventLog.SourceExists(Source))
            {
                EventLog.WriteEvent(Source, new EventInstance((long)((ulong)id & 0x0000FFFF), 0, (EventLogEntryType)((uint)id >> 16)), InsertionStrings);
            }
        }
    }
}
