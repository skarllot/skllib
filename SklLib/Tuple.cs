// Tuple.cs
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
    /// Represents a pair of values.
    /// </summary>
    /// <typeparam name="T1">The type of first item.</typeparam>
    /// <typeparam name="T2">The type of second item.</typeparam>
    public struct Tuple<T1, T2> : IComparable<Tuple<T1, T2>>
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of <see cref="Tuple"/> and sets the
        /// first and second item.
        /// </summary>
        /// <param name="item1">The value to first item.</param>
        /// <param name="item2">The value to second item.</param>
        public Tuple(T1 item1, T2 item2)
            : this()
        {
            this.Item1 = item1;
            this.Item2 = item2;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value of first item.
        /// </summary>
        public T1 Item1 { get; set; }

        /// <summary>
        /// Gets or sets the value of second item.
        /// </summary>
        public T2 Item2 { get; set; }

        #endregion

        #region IComparable

        public int CompareTo(Tuple<T1, T2> other)
        {
            IComparable<T1> this1 = this.Item1 as IComparable<T1>;
            IComparable<T2> this2 = this.Item2 as IComparable<T2>;
            if (this1 == null) {
                throw new InvalidOperationException(string.Format(
                    resExceptions.TypeNotImplementInterface,
                    typeof(T1).Name, typeof(IComparable<T1>).Name));
            }
            if (this2 == null) {
                throw new InvalidOperationException(string.Format(
                    resExceptions.TypeNotImplementInterface,
                    typeof(T2).Name, typeof(IComparable<T2>).Name));
            }

            int result = this1.CompareTo(other.Item1);
            if (result != 0)
                return result;

            return this2.CompareTo(other.Item2);
        }

        #endregion
    }
}
