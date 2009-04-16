// IWriteProtected.cs
//
//  Copyright (C) 2008 Fabrício Godoy
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

namespace Root
{
    /// <summary>
    /// Defines methods that supports read-only instances.
    /// </summary>
    /// <typeparam name="T">Type of current instance.</typeparam>
    public interface IWriteProtected<T>
    {
        #region Property
        
        /// <summary>
        /// Gets a value indicating whether this instance is read-only.
        /// </summary>
        bool IsReadOnly
        { get;}

        #endregion
        
        #region Methods
        
        /// <summary>
        /// Returns a read-only type-specific wrapper.
        /// </summary>
        /// <returns>A read-only type-specific wrapper around this instance.</returns>
        T ReadOnly();

        /// <summary>
        /// Creates a shallow copy of the type-specific.
        /// </summary>
        /// <returns>A new type-specific, not write protected, copied around this instance.</returns>
        T Clone();
        
        #endregion
    }
}
