// Strings.cs
//
//  Copyright (C) 2008, 2014 Fabrício Godoy
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

/// <summary>
/// Namespace where are located classes to high performance,
/// but can be dangerous.
/// </summary>
namespace SklLib.Performance
{
    /// <summary>
    /// <para>Perform fatest operations with Strings.</para>
    /// <para>Dangerous unsafe class.</para>
    /// </summary>
    public static class Strings
    {
        /// <summary>
        /// Converts the value of each Unicode character to its uppercase equivalent, using the casing
        /// rules of the current culture.
        /// </summary>
        /// <param name="str">A Unicode String.</param>
        public static unsafe void UnsafeToUpper(this string str)
        {
            fixed (char* pFixed = str)
            {
                for (char* p = pFixed; *p != 0; p++)
                    *p = char.ToUpper(*p);
            }
        }

        /// <summary>
        /// Gets the maximum number of characters that can be contained in the
        /// memory allocated by the current instance.
        /// </summary>
        /// <param name="str">A Unicode String.</param>
        /// <returns>
        /// The maximum number of characters that can be contained in the memory allocated
        /// by the current instance.
        /// </returns>
        public static unsafe int GetCapacity(this string str)
        {
            fixed (char* p = str)
            {
                int* pcapacity = (int*)p - 2;
                return *pcapacity;
            }
        }

        /// <summary>
        /// Gets the number of characters in the specified string.
        /// </summary>
        /// <param name="str">A Unicode String.</param>
        /// <returns>The number of characters in this instance.</returns>
        /// <remarks>
        /// This function is redundant, because it accomplishes
        /// the same role as String.Length.
        /// </remarks>
        public static unsafe int GetLength(this string str)
        {
            fixed (char* p = str)
            {
                int* plength = (int*)p - 1;
                int length = *plength & 0x3fffffff;
                System.Diagnostics.Debug.Assert(length == str.Length);
                return length;
            }
        }

        /// <summary>
        /// Sets the number of characters in the specified string.
        /// </summary>
        /// <param name="str">A Unicode String.</param>
        /// <param name="length">The length of this instance.</param>
        public static unsafe void SetLength(string str, int length)
        {
            fixed (char* p = str)
            {
                int* pi = (int*)p;
                if (length < 0 || length > pi[-2])
                    throw (new ArgumentOutOfRangeException("length"));

                pi[-1] = length;
                p[length] = '\0';
            }
        }

        /// <summary>
        /// Creates a new String with specified capacity.
        /// </summary>
        /// <param name="capacity">The String capacity.</param>
        /// <returns>A new String.</returns>
        public static string NewString(int capacity)
        {
            return new string('\0', capacity);
        }

        /// <summary>
        /// Sets a char into a String.
        /// </summary>
        /// <param name="str">A Unicode String.</param>
        /// <param name="index">Index into String to sets.</param>
        /// <param name="ch">Char to replacement.</param>
        public static unsafe void SetChar(string str, int index, char ch)
        {
            if (index < 0 || index >= str.Length)
                throw new ArgumentOutOfRangeException("index");

            fixed (char* p = str)
                p[index] = ch;
        }
    }
}
