// OSOldVersion.cs
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

namespace SklLib.Environment
{
    /// <summary>
    /// Identifies a operating System from old versions.
    /// </summary>
    public enum OSOldVersion
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
        WindowsME
    }
}
