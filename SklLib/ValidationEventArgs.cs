// ValidationEventArgs.cs
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
    /// Provides event data to item validation.
    /// </summary>
    public class ValidationEventArgs : EventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of ValidationEventArgs.
        /// </summary>
        /// <param name="isValid">Defines validation status of validated item.</param>
        /// <param name="message">Defines a error message, if any.</param>
        /// <param name="itemName">Defines the name of validated item.</param>
        /// <param name="itemValue">Defines the value of validated item.</param>
        public ValidationEventArgs(
            bool isValid, string message,
            string itemName, object itemValue)
        {
            IsValid = isValid;
            Message = message;
            ItemName = itemName;
            ItemValue = itemValue;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets current property validity status.
        /// </summary>
        public bool IsValid { get; private set; }

        /// <summary>
        /// Gets the error message, if any.
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
