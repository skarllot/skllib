// LogTransaction.cs
//
//  Copyright (C) 2014 Fabrício Godoy
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 3 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA 
//
//

using System;
using System.Diagnostics;
using stringb = System.Text.StringBuilder;

namespace SklLib.Diagnostics
{
    /// <summary>
    /// Provides methods to handle multiple messages to single log entry.
    /// </summary>
    public class LogTransaction : ITransaction
    {
        #region Fields

        /// <summary>
        /// Default EventLog type to new <see cref="LogTransaction"/> instances.
        /// </summary>
        public const EventLogEntryType DEFAULT_LOG_TYPE = EventLogEntryType.Error;

        /// <summary>
        /// Default EventLog event ID to new <see cref="LogTransaction"/> instances.
        /// </summary>
        public static readonly EventId DEFAULT_LOG_ID = EventId.UnexpectedError;

        /// <summary>
        /// Default header for each message.
        /// </summary>
        public const string DEFAULT_LINE_HEADER = "[{0}] ";

        /// <summary>
        /// Default timestamp format for line header.
        /// </summary>
        public const string DEFAULT_TIMESTAMP_FORMAT = "yyyy-MM-dd HH:mm:ss";

        stringb log = new stringb();
        Logger logger = null;
        string fmtLineHeader = DEFAULT_LINE_HEADER;
        string fmtTimestamp = DEFAULT_TIMESTAMP_FORMAT;
        bool headerHasTimestamp = true;
        EventLogEntryType type;
        EventId eventId;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of LogTransaction linked to Logger class.
        /// </summary>
        /// <param name="logger">The class that handles message logging.</param>
        public LogTransaction(Logger logger)
        {
            this.logger = logger;
            this.type = DEFAULT_LOG_TYPE;
            this.eventId = DEFAULT_LOG_ID;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the entry type of current log transaction.
        /// </summary>
        public EventLogEntryType EntryType
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// Gets or sets the event ID of current log transaction.
        /// </summary>
        public EventId EventLogId
        {
            get { return eventId; }
            set { eventId = value; }
        }

        /// <summary>
        /// Gets or sets whether the line header has a timestamp placeholder.
        /// </summary>
        public bool HeaderHasTimestamp
        {
            get { return headerHasTimestamp; }
            set { headerHasTimestamp = value; }
        }

        /// <summary>
        /// Gets or sets the line header.
        /// </summary>
        public string LineHeader
        {
            get { return fmtLineHeader; }
            set { fmtLineHeader = value; }
        }

        /// <summary>
        /// Gets or sets the formatting for log timestamp.
        /// </summary>
        public string TimestampFormat
        {
            get { return fmtTimestamp; }
            set { fmtTimestamp = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Appends a line to log message.
        /// </summary>
        /// <param name="message">The line to append.</param>
        /// <exception cref="ObjectDisposedException">Current instance is already disposed.</exception>
        public void AppendLine(string message)
        {
            if (logger == null)
                throw new ObjectDisposedException("LogTransaction");

            if (headerHasTimestamp)
                log.Append(string.Format("[{0}] ", GetTimestamp()));
            log.AppendLine(message);
        }

        /// <summary>
        /// Appends a line to log message.
        /// </summary>
        /// <param name="message">The line to append.</param>
        /// <param name="args">Parameters to replace placeholders from line header.</param>
        /// <exception cref="ObjectDisposedException">Current instance is already disposed.</exception>
        public void AppendLine(string message, params object[] args)
        {
            if (logger == null)
                throw new ObjectDisposedException("LogTransaction");

            object[] list;
            if (headerHasTimestamp) {
                list = new object[args.Length + 1];
                list[0] = GetTimestamp();
                Array.Copy(args, 0, list, 1, args.Length);
            }
            else {
                list = args;
            }

            log.Append(string.Format(fmtLineHeader, list));
            log.AppendLine(message);
        }

        /// <summary>
        /// Commits this transaction to a new log entry.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Current instance is already disposed.</exception>
        /// <exception cref="InvalidOperationException">Cannot commit empty log message.</exception>
        public void Commit()
        {
            if (logger == null)
                throw new ObjectDisposedException("LogTransaction");
            if (log.Length == 0)
                throw new InvalidOperationException(resExceptions.EventLogMessageEmpty);

            logger.WriteEntry(log.ToString(), this.type, this.eventId);
            this.Dispose();
        }

        /// <summary>
        /// Frees unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            log.Remove(0, log.Length);
            logger = null;
        }

        private string GetTimestamp()
        {
            return DateTime.Now.ToString(fmtTimestamp);
        }

        /// <summary>
        /// Discards this transaction to not add any entry to event log.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Current instance is already disposed.</exception>
        public void Rollback()
        {
            if (logger == null)
                throw new ObjectDisposedException("LogTransaction");

            this.Dispose();
        }

        #endregion
    }
}
