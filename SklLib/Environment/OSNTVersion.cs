// OSNTVersion.cs
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
    /// Identifies a operating system as Windows NT version.
    /// </summary>
    public enum OSNTVersion
    {
        /// <summary>
        /// Windows not NT version
        /// </summary>
        NotNT,
        /// <summary>
        /// Windows older than Windows 2000
        /// </summary>
        WindowsNTOld,
        /// <summary>
        /// Windows 2000
        /// </summary>
        Windows2000,
        /// <summary>
        /// Windows XP
        /// </summary>
        WindowsXP,
        /// <summary>
        /// Windows Server 2003 (or rare Windows XP Pro x64 Edition)
        /// </summary>
        Windows2003,
        /// <summary>
        /// Windows Vista or Windows Server 2008
        /// </summary>
        WindowsVistaOr2008,
        /// <summary>
        /// Windows 7 or Windows Server 2008 R2
        /// </summary>
        Windows7Or2008R2,
        /// <summary>
        /// Windows 8 or Windows Server 2012
        /// </summary>
        Windows8Or2012,
        /// <summary>
        /// Windows 8.1 or Windows Server 2012 R2
        /// </summary>
        Windows81Or2012R2,
        /// <summary>
        /// Windows newer than 8.1 or 2012 R2
        /// </summary>
        WindowsNew,
        /// <summary>
        /// Invalid Windows edition
        /// </summary>
        Invalid
    }
}
