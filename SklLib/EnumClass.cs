// EnumClass.cs
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

namespace SklLib
{
    /// <summary>
    /// Represents a value to emulate enum-like behaviour.
    /// </summary>
    public abstract class EnumClass<T>
        where T : struct, IComparable, IConvertible, IComparable<T>, IEquatable<T>
    {
        #region Fields

        /// <summary>
        /// Value represented by this class instance.
        /// </summary>
        protected T value;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of EnumClass with specified value.
        /// </summary>
        /// <param name="value">Value represented by this instance.</param>
        protected EnumClass(T value)
        {
            this.value = value;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Value represented by this instance.
        /// </summary>
        public T Value
        {
            get { return value; }
            protected set { this.value = value; }
        }

        #endregion

        #region Operators

        /// <summary>
        /// Converts an instance of EnumClass to struct value.
        /// </summary>
        /// <param name="c">Instance of EnumClass.</param>
        /// <returns>Returns a value represented by EnumClass.</returns>
        public static implicit operator T(EnumClass<T> c)
        {
            return c.value;
        }

        #endregion
    }
}
