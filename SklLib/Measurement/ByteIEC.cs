// ByteIEC.cs
//
//  Copyright (C) 2008-2009, 2013-2014 Fabrício Godoy
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


namespace SklLib.Measurement
{
    /// <summary>
    /// Byte IEC multiples.
    /// </summary>
    public enum ByteIEC : ulong
    {
        /// <summary>
        /// Represents a byte.
        /// </summary>
        Byte = 1UL,
        /// <summary>
        /// Represents a kibibyte (1024 B).
        /// </summary>
        Kibibyte = 1024UL,
        /// <summary>
        /// Represents a mebibyte (1024 KiB).
        /// </summary>
        Mebibyte = 1048576UL,
        /// <summary>
        /// Represents a gibibyte (1024 MiB).
        /// </summary>
        Gibibyte = 1073741824UL,
        /// <summary>
        /// Represents a tebibyte (1024 GiB).
        /// </summary>
        Tebibyte = 1099511627776UL,
        /// <summary>
        /// Represents a pebibyte (1024 TiB).
        /// </summary>
        Pebibyte = 1125899906842624UL,
        /// <summary>
        /// Represents a exbibyte (1024 PiB).
        /// </summary>
        Exbibyte = 1152921504606846976UL

        //Zebibyte = 1180591620717411303424
    }
}
