// Validatable.cs
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


namespace SklLib
{
    /// <summary>
    /// Provides methods to <see cref="IValidatable"/> interface.
    /// </summary>
    public static class Validatable
    {
        /// <summary>
        /// Returns whether current instance is valid.
        /// </summary>
        /// <param name="obj">The IValidatable instance.</param>
        /// <returns>True whether is valid; otherwise false.</returns>
        public static bool IsValid(this IValidatable obj)
        {
            return obj.Validate(delegate(InvalidEventArgs vea) { });
        }
    }
}
