// OSServerVersion.cs
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
    /// Identifies a operating system as Windows server version.
    /// </summary>
    public enum OSServerVersion
    {
        /// <summary>
        /// Windows older than Windows 2000
        /// </summary>
        WindowsNTOld,
        /// <summary>
        /// Windows 2000 Server
        /// </summary>
        Windows2000,
        /// <summary>
        /// Windows Server 2003 (or 2003 R2)
        /// </summary>
        Windows2003,
        /// <summary>
        /// Windows Server 2008
        /// </summary>
        Windows2008,
        /// <summary>
        /// Windows Server 2008 R2
        /// </summary>
        Windows2008R2,
        /// <summary>
        /// Windows Server 2012
        /// </summary>
        Windows2012,
        /// <summary>
        /// Windows Server 2012 R2
        /// </summary>
        Windows2012R2,
        /// <summary>
        /// Windows Server version newer than 2012 R2
        /// </summary>
        WindowsNew,
        /// <summary>
        /// Invalid Windows Server edition
        /// </summary>
        Invalid
    }
}
