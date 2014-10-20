// LogEventArgs.cs
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
    /// Provides data to add a new log entry.
    /// </summary>
    public class LogEventArgs : EventArgs
    {
        #region Fields

        EventLogEntryType type;
        EventId eventId;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of LogEventArgs with specified log type and ID.
        /// </summary>
        /// <param name="type">The type of event log.</param>
        /// <param name="eventId">The application-unique identifier to log type.</param>
        public LogEventArgs(EventLogEntryType type, EventId eventId)
        {
            this.type = type;
            this.eventId = eventId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the type of event log.
        /// </summary>
        public EventLogEntryType EntryType
        {
            get { return type; }
        }

        /// <summary>
        /// Gets the application-unique identifier to log type.
        /// </summary>
        public EventId EventId
        {
            get { return eventId; }
        }

        #endregion
    }
}
