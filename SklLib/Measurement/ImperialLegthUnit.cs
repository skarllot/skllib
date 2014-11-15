// SklLib.cs
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
