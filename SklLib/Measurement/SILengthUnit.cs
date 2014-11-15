// SILengthUnit.cs
//
//  Copyright (C) 2008, 2009, 2013-2014 Fabrício Godoy
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
}
