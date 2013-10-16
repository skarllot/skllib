// IMaskeable.cs
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

namespace SklLib
{
    /// <summary>
    /// Defines methods to masking support.
    /// </summary>
    /// <remarks>
    /// The IMaskeable interface contains three members, which are intented to support
    /// masking.
    /// </remarks>
    public interface IMaskeable
    {
        #region Properties
        
        /// <summary>
        /// Provides a <see cref="String"/> to use as mask.
        /// </summary>
        /// <remarks>
        /// <para>The masker classes uses a mask to filter user-typed <see cref="Char"/> and to
        /// verify a automatic-typed <see cref="Char"/>.</para>
        /// <para>To indicate a automatic-typed <see cref="Char"/>, uses "!" before of a
        /// <see cref="Char"/>.</para>
        /// <para>The Mask <see cref="String"/> need return true when used in <see cref="IsMatch"/> method,
        /// masker classes ignores "!" to match the Mask <see cref="String"/>.</para>
        /// </remarks>
        string Mask
        { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is read-only.
        /// </summary>
        /// <remarks>
        /// <para>Masker classes uses this property to verify whether this instance is read-only.</para>
        /// <para>No read-only instances can generate problems to masker classes, thus, the masker
        /// class only accept read-only instances.</para>
        /// </remarks>
        bool IsReadOnly
        { get; }
        
        #endregion

        #region Method
        
        /// <summary>
        /// Indicates whether this instance finds a match in the input string.
        /// </summary>
        /// <param name="input">The string to search for a match.</param>
        /// <returns>true if this instance finds a match; otherwise, false.</returns>
        /// <remarks>
        /// <para>Masker classes uses this method to verify whether a user-typed <see cref="String"/> is
        /// match with Mask.</para>
        /// </remarks>
        bool IsMatch(string input);
        
        #endregion
    }
}
