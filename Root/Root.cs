// Root.cs
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
using stringb = System.Text.StringBuilder;

/// <summary>
/// Namespace where are located basic code.
/// </summary>
namespace Root
{
    /// <summary>
    /// Represents a generic function that returns a type-specific.
    /// </summary>
    /// <typeparam name="T">Any type-specific.</typeparam>
    /// <returns>Specified type-specific.</returns>
    public delegate T GetType<T>();

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
    /// Identifies a operating System version.
    /// </summary>
    public enum OSVersion
    {
        /// <summary>
        /// Unidentifield Windows Version
        /// </summary>
        Unidentifield,
        /// <summary>
        /// Windows Compact Edition oldest than 3.x
        /// </summary>
        WindowsCEOld,
        /// <summary>
        /// Windows Compact Edition 3.x
        /// </summary>
        WindowsCE3,
        /// <summary>
        /// Windows Compact Edition 4.x
        /// </summary>
        WindowsCE4,
        /// <summary>
        /// Windows Compact Edition 5.x
        /// </summary>
        WindowsCE5,
        /// <summary>
        /// Windows Compact Edition 6.x
        /// </summary>
        WindowsCE6,
        /// <summary>
        /// Windows Compact Edition newest than 6.x
        /// </summary>
        WindowsCENew,
        /// <summary>
        /// Windows 95
        /// </summary>
        Windows95,
        /// <summary>
        /// Windows 98
        /// </summary>
        Windows98,
        /// <summary>
        /// Windows 98 Second Edition
        /// </summary>
        Windows98SE,
        /// <summary>
        /// Windows Millenium Edition
        /// </summary>
        WindowsME,
        /// <summary>
        /// Windows NT 4.x
        /// </summary>
        WindowsNT4,
        /// <summary>
        /// Windows 2000
        /// </summary>
        Windows2000,
        /// <summary>
        /// Windows XP
        /// </summary>
        WindowsXP,
        /// <summary>
        /// Windows Server 2003
        /// </summary>
        WindowsServer2003,
        /// <summary>
        /// Windows Vista
        /// </summary>
        WindowsVista,
        /// <summary>
        /// Windows newest than Vista
        /// </summary>
        WindowsNew
    }

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
