// WriteNumber.cs
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

namespace SklLib.Globalization
{
    /// <summary>
    /// Stores how writes a number.
    /// </summary>
    public struct WriteNumber
    {
        /// <summary>
        /// A value.
        /// </summary>
        public long value;

        /// <summary>
        /// How <c>value</c> is written.
        /// </summary>
        public string write;

        /// <summary>
        /// Initializes the WriteNumber struct with loaded values.
        /// </summary>
        /// <param name="v">A value.</param>
        /// <param name="w">How <c>v</c> is written.</param>
        public WriteNumber(long v, string w)
        {
            value = v;
            write = w;
        }
    }
}
