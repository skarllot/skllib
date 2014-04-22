// OSNTClientVersion.cs
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

namespace SklLib.Environment
{
    /// <summary>
    /// Identifies a operating system as Windows NT workstation version.
    /// </summary>
    public enum OSNTClientVersion
    {
        /// <summary>
        /// Windows older than Windows 2000
        /// </summary>
        WindowsNTOld,
        /// <summary>
        /// Windows 2000 Workstation
        /// </summary>
        Windows2000,
        /// <summary>
        /// Windows XP
        /// </summary>
        WindowsXP,
        /// <summary>
        /// Windows XP Professional x64 Edition
        /// </summary>
        WindowsXPPro64,
        /// <summary>
        /// Windows Vista
        /// </summary>
        WindowsVista,
        /// <summary>
        /// Windows 7
        /// </summary>
        Windows7,
        /// <summary>
        /// Windows 8
        /// </summary>
        Windows8,
        /// <summary>
        /// Windows 8.1
        /// </summary>
        Windows8_1,
        /// <summary>
        /// Windows newest than 8.1
        /// </summary>
        WindowsNew,
        /// <summary>
        /// Invalid Windows edition
        /// </summary>
        Invalid
    }
}
