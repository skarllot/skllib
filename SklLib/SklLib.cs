// SklLib.cs
//
//  Copyright (C) 2008-2014 Fabrício Godoy
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
using stringb = System.Text.StringBuilder;

/// <summary>
/// Namespace where are located basic code.
/// </summary>
namespace SklLib
{
    /// <summary>
    /// Represents a generic function that returns a type-specific.
    /// </summary>
    /// <typeparam name="T">Any type-specific.</typeparam>
    /// <returns>Specified type-specific.</returns>
    public delegate T GetType<T>();

    /// <summary>
    /// Represents a generic function that returns a type-specific and receives
    /// another type-specific.
    /// </summary>
    /// <typeparam name="Ret">Any type-specific to return.</typeparam>
    /// <typeparam name="P1">Any type-specific to receive.</typeparam>
    /// <param name="param1">First parameter.</param>
    /// <returns>Specified type-specific.</returns>
    public delegate Ret GetType<Ret, P1>(P1 param1);

    /// <summary>
    /// Byte multiples.
    /// </summary>
    public enum ByteMeasure : long
    {
        /// <summary>
        /// Represents a byte.
        /// </summary>
        Bytes = 1L,
        /// <summary>
        /// Represents a kilobyte (1024 B).
        /// </summary>
        Kilobytes = 1024L,
        /// <summary>
        /// Represents a megabyte (1024 KB).
        /// </summary>
        Megabytes = 1048576L,
        /// <summary>
        /// Represents a gigabyte (1024 MB).
        /// </summary>
        Gigabytes = 1073741824L,
        /// <summary>
        /// Represents a terabyte (1024 GB).
        /// </summary>
        Terabytes = 1099511627776L,
        /// <summary>
        /// Represents a petabyte (1024 TB).
        /// </summary>
        Petabytes = 1125899906842624L,
        /// <summary>
        /// Represents a exabyte (1024 PB).
        /// </summary>
        Exabytes = 1152921504606846976L
    }

    /// <summary>
    /// Linear units from International System of Units.
    /// </summary>
    public enum SILengthUnit : int
    {
        /// <summary>
        /// Millimeter length unit.
        /// </summary>
        Millimeter = 1,
        /// <summary>
        /// Centimeter length unit.
        /// </summary>
        Centimeter = 10,
        /// <summary>
        /// Decimeter length unit.
        /// </summary>
        Decimeter = 100,
        /// <summary>
        /// Meter length unit.
        /// </summary>
        Meter = 1000,
        /// <summary>
        /// Decameter length unit.
        /// </summary>
        Decameter = 10000,
        /// <summary>
        /// Hectometer length unit.
        /// </summary>
        Hectometer = 100000,
        /// <summary>
        /// Kilometer length unit.
        /// </summary>
        Kilometer = 1000000
    };

    /// <summary>
    /// Imperial linear units.
    /// </summary>
    public enum ImperialLengthUnit : int
    {
        /// <summary>
        /// Inch length unit.
        /// </summary>
        Inch = 1,
        /// <summary>
        /// Foot length unit.
        /// </summary>
        Foot = 12,
        /// <summary>
        /// Yard length unit.
        /// </summary>
        Yard = 36,
        /// <summary>
        /// Furlon length unit.
        /// </summary>
        Furlong = 7920,
        /// <summary>
        /// Mile length unit.
        /// </summary>
        Mile = 63360,
        /// <summary>
        /// League length unit.
        /// </summary>
        League = 190080
    }

}
