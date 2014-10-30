// FileInfoExtension.cs
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
using System.IO;
using System.Security;
using System.Security.Permissions;

namespace SklLib.IO
{
    /// <summary>
    /// Provides methods to <see cref="System.IO.FileInfo"/> class.
    /// </summary>
    public static class FileInfoExtension
    {
        /// <summary>
        /// Determines whether specified permission is granted to access the file.
        /// </summary>
        /// <param name="fInfo">The target file to test permission access.</param>
        /// <param name="perm">The type of file access required.</param>
        /// <returns>True whether the required access permission is granted; otherwise, False.</returns>
        public static bool HasPermission(this FileInfo fInfo, FileIOPermissionAccess perm)
        {
            FileIOPermission filePerm = new FileIOPermission(perm, fInfo.FullName);

            /* 
             * .NET Framework 4 or greater
             * 
            PermissionSet mgrPerm = new PermissionSet(PermissionState.None);
            mgrPerm.AddPermission(filePerm);
            return mgrPerm.IsSubsetOf(AppDomain.CurrentDomain.PermissionSet);
             */

            return SecurityManager.IsGranted(filePerm);
        }

        /// <summary>
        /// Determines whether specified permission is granted to access the file.
        /// </summary>
        /// <param name="path">The path to target file to test permission access.</param>
        /// <param name="perm">The type of file access required.</param>
        /// <returns>True whether the required access permission is granted; otherwise, False.</returns>
        public static bool HasPermission(string path, FileIOPermissionAccess perm)
        {
            return HasPermission(new FileInfo(path), perm);
        }
    }
}
