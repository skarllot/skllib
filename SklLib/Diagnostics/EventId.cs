// EventId.cs
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

namespace SklLib.Diagnostics
{
    /// <summary>
    /// Defines identification values to logging events.
    /// </summary>
    public class EventId : EnumClass<ushort>
    {
        // ---------------------------
        // Service related codes (0-9)
        // ---------------------------
        /// <summary>
        /// Evend ID used when service was started or stopped (0).
        /// </summary>
        public static readonly EventId ServiceStateChanged = new EventId(0);
        /// <summary>
        /// Event ID used when service running time is bigger than loop wait time (1).
        /// </summary>
        public static readonly EventId ServiceInsufficientWaitTime = new EventId(1);
        /// <summary>
        /// Event ID used when a new source of event log is created (2).
        /// </summary>
        public static readonly EventId EventLogCreated = new EventId(2);


        // ----------------------------------------
        // Configuration file related codes (10-29)
        // ----------------------------------------
        /// <summary>
        /// Event ID used when a configuration file is not found (10).
        /// </summary>
        public static readonly EventId ConfigFileNotFound = new EventId(10);
        /// <summary>
        /// Event ID used when has a error to load a configuration file (11).
        /// </summary>
        public static readonly EventId ConfigFileLoadError = new EventId(11);
        /// <summary>
        /// Event ID used when has a error to reload a configuration file (12).
        /// </summary>
        public static readonly EventId ConfigFileReloadError = new EventId(12);
        /// <summary>
        /// Event ID used when a configuration file was reloaded successfully (13).
        /// </summary>
        public static readonly EventId ConfigFileReloaded = new EventId(13);


        // -------------------------------
        // Unhandled related codes (65535)
        // -------------------------------
        /// <summary>
        /// Unhandled error code (65535).
        /// </summary>
        public static readonly EventId UnexpectedError = new EventId(UInt16.MaxValue);

        #region Constructors

        /// <summary>
        /// Initializes a new instance of EventId with specified value.
        /// </summary>
        /// <param name="value">Value represented by this instance.</param>
        protected EventId(ushort value) : base(value) { }

        #endregion
    }
}
