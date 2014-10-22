// Logger.cs
//
// Copyright (C) 2014 Fabrício Godoy
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.Diagnostics;

namespace SklLib.Diagnostics
{
    /// <summary>
    /// Provides methods to handle Windows native logging.
    /// </summary>
    public class Logger
    {
        #region Fields

        /// <summary>
        /// Defines how much characters can be handled by EventLog.
        /// </summary>
        const int EVENT_LOG_MAX_LENGTH = 15000;
        EventLog eventLog;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of Logger class based on specified source and log name.
        /// </summary>
        /// <param name="source">The source of event log entries.</param>
        /// <param name="logName">The name of the log.</param>
        /// <exception cref="Exception">Throws when failed to create event log.</exception>
        public Logger(string source, string logName)
        {
            eventLog = CreateEventlog(source, logName);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Starts a transaction to write multiple messages to a single log entry.
        /// </summary>
        /// <returns>An object to handle a log transaction.</returns>
        public LogTransaction BeginWriteEntry()
        {
            return new LogTransaction(this);
        }

        /// <summary>
        /// Writes a new log entry with specified message.
        /// </summary>
        /// <param name="message">The message to write to log entry.</param>
        /// <param name="eventArgs">The event log information data.</param>
        public void WriteEntry(string message, LogEventArgs eventArgs)
        {
            string[] msgArr;
            if (message.Length > EVENT_LOG_MAX_LENGTH) {
                msgArr = message.Split(EVENT_LOG_MAX_LENGTH);
            }
            else
                msgArr = new string[] { message };

            foreach (string item in msgArr)
                eventLog.WriteEntry(item, eventArgs.EntryType, eventArgs.EventId);
        }

        /// <summary>
        /// Writes a new log entry with specified message.
        /// </summary>
        /// <param name="message">The message to write to log entry.</param>
        /// <param name="type">The type of event log.</param>
        /// <param name="eventId">The application-unique identifier to log type.</param>
        public void WriteEntry(string message, EventLogEntryType type, EventId eventId)
        {
            WriteEntry(message, new LogEventArgs(type, eventId));
        }

        private static EventLog CreateEventlog(string source, string logName)
        {
            EventLog result = null;

            try {
                // PS> Remove-EventLog <logname>
                if (EventLog.SourceExists(source)) {
                    result = new EventLog { Source = source };
                    if (result.Log != logName) {
                        EventLog.DeleteEventSource(source);
                        result.Dispose();
                        result = null;
                    }
                }

                if (!EventLog.SourceExists(source)) {
                    EventLog.CreateEventSource(source, logName);
                    result = new EventLog { Source = source, Log = logName };
                    result.WriteEntry("Event Log created",
                        EventLogEntryType.Information, EventId.EventLogCreated);
                }
            }
            catch (Exception e) {
                throw new Exception(string.Format(
                    "Error creating EventLog (Source: {0} and Log: {1})", source, logName), e);
            }

            return result;
        }

        #endregion
    }
}
