// InvalidEventArgs.cs
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

namespace SklLib
{
    /// <summary>
    /// Provides event data when a validated item is invalid.
    /// </summary>
    public class InvalidEventArgs : EventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of InvalidEventArgs.
        /// </summary>
        /// <param name="message">Defines a error message.</param>
        /// <param name="itemName">Defines the name of validated item.</param>
        /// <param name="itemValue">Defines the value of validated item.</param>
        public InvalidEventArgs(
            string message,
            string itemName,
            object itemValue)
        {
            Message = message;
            ItemName = itemName;
            ItemValue = itemValue;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the error message.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Gets the name of validated item.
        /// </summary>
        public string ItemName { get; private set; }

        /// <summary>
        /// Gets the value of validated item.
        /// </summary>
        public object ItemValue { get; private set; }

        #endregion
    }
}
