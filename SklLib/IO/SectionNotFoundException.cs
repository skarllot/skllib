// SectionNotFoundException.cs
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
    /// Exception that is thrown when the section specified for accessing a
    /// configuration file does not match any section in the file.
    /// </summary>
    class SectionNotFoundException : Exception
    {
        #region Constructors

        /// <summary>
        /// Initializes a new SectionNotFoundException object using default values.
        /// </summary>
        public SectionNotFoundException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new SectionNotFoundException object using specified error
        /// message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public SectionNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new SectionNotFoundException object using specified error
        /// message and an inner exception that caused this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that caused this exception.</param>
        public SectionNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new SectionNotFoundException object using serialized data.
        /// </summary>
        /// <param name="info">Serialization data.</param>
        /// <param name="context">Serialization context.</param>
        public SectionNotFoundException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}
