// KeyNotFoundException.cs
//
//  Copyright (C) 2013 Fabrício Godoy
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

namespace SklLib.IO
{
    /// <summary>
    /// Exception that is thrown when the key specified for accessing a
    /// configuration file does not match any key in the section.
    /// </summary>
    public class KeyNotFoundException : Exception
    {
        #region Constructors

        /// <summary>
        /// Initializes a new KeyNotFoundException object using default values.
        /// </summary>
        public KeyNotFoundException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new KeyNotFoundException object using specified error
        /// message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public KeyNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new KeyNotFoundException object using specified error
        /// message and an inner exception that caused this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that caused this exception.</param>
        public KeyNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new KeyNotFoundException object using serialized data.
        /// </summary>
        /// <param name="info">Serialization data.</param>
        /// <param name="context">Serialization context.</param>
        public KeyNotFoundException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}
