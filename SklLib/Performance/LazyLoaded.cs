// LazyLoaded.cs
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

namespace SklLib.Performance
{
    /// <summary>
    /// Defines a type that is loaded after request.
    /// </summary>
    /// <typeparam name="T">The type to store.</typeparam>
    public class LazyLoaded<T>
    {
        #region Fields

        bool _isLoaded;
        Func<T> _loadFunction;
        T _value;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="LazyLoaded"/> class and
        /// sets the function used to load the stored type.
        /// </summary>
        /// <param name="loadFunction">The function called after value request.</param>
        public LazyLoaded(Func<T> loadFunction)
        {
            _isLoaded = false;
            _loadFunction = loadFunction;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="LazyLoaded"/> class,
        /// sets the function used to load the stored type and defines the
        /// default value for stored type.
        /// </summary>
        /// <param name="loadFunction">The function called after value request.</param>
        /// <param name="defaultValue">The default value for type.</param>
        public LazyLoaded(Func<T> loadFunction, T defaultValue)
            : this(loadFunction)
        {
            _value = defaultValue;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets whether current type is already loaded.
        /// </summary>
        public bool IsLoaded { get { return _isLoaded; } }

        /// <summary>
        /// Gets the value of current type.
        /// </summary>
        public T Value
        {
            get
            {
                if (!_isLoaded) {
                    if (_loadFunction != null) {
                        _value = _loadFunction();
                        _isLoaded = true;
                    }
                }

                return _value;
            }
        }

        #endregion

        #region Operators

        /// <summary>
        /// Converts an instance of <see cref="LazyLoaded"/> to its generic type.
        /// </summary>
        /// <param name="a">Instance of <see cref="LazyLoaded"/>.</param>
        /// <returns>The value stored by <see cref="LazyLoaded"/> instance.</returns>
        public static implicit operator T(LazyLoaded<T> a)
        {
            return a.Value;
        }

        #endregion

        #region Object

        public override bool Equals(object obj)
        {
            return this.Value.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }

        #endregion
    }
}
