// SpeltNumber.cs
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
    /// Stores how to write a specific number.
    /// </summary>
    public struct SpeltNumber
    {
        #region Constructor

        /// <summary>
        /// Initializes the SpeltNumber struct with loaded values.
        /// </summary>
        /// <param name="v">A value.</param>
        /// <param name="w">How <c>v</c> is written.</param>
        public SpeltNumber(long v, string w)
            : this()
        {
            Value = v;
            Spell = w;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The number that this instance represents.
        /// </summary>
        public long Value { get; private set; }

        /// <summary>
        /// How the number represented by this instance is spelt.
        /// </summary>
        public string Spell { get; private set; }

        #endregion
    }
}
