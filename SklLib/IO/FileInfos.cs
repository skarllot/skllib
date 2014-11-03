// FileInfos.cs
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
    public static class FileInfos
    {
        /// <summary>
        /// Establishes a hard link between an existing file and a new file.
        /// This function is only supported on the NTFS file system, and only
        /// for files, not directories.
        /// </summary>
        /// <param name="fInfo">The original file.</param>
        /// <param name="hlPath">The path where to create the hard link.</param>
        /// <exception cref="System.ComponentModel.Win32Exception">The hard link was not created.</exception>
        public static void CreateHardLink(this FileInfo fInfo, string hlPath)
        {
            if (!TryCreateHardLink(fInfo, hlPath)) {
                throw new System.ComponentModel.Win32Exception(
                        System.Runtime.InteropServices.Marshal.GetLastWin32Error());
            }
        }

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
        /// <returns>True whether the required access permission is granted; otherwise, false.</returns>
        public static bool HasPermission(string path, FileIOPermissionAccess perm)
        {
            return HasPermission(new FileInfo(path), perm);
        }

        /// <summary>
        /// Establishes a hard link between an existing file and a new file.
        /// This function is only supported on the NTFS file system, and only
        /// for files, not directories.
        /// </summary>
        /// <param name="fInfo">The original file.</param>
        /// <param name="hlPath">The path where to create the hard link.</param>
        /// <returns>True whether the hard link was created; otherwise, false.</returns>
        public static bool TryCreateHardLink(this FileInfo fInfo, string hlPath)
        {
            return CreateHardLink(hlPath, fInfo.FullName, IntPtr.Zero);
        }


        [System.Runtime.InteropServices.DllImport("Kernel32.dll",
            CharSet = System.Runtime.InteropServices.CharSet.Unicode, SetLastError = true)]
        static extern bool CreateHardLink(string lpFileName, string lpExistingFileName, IntPtr lpSecurityAttributes);
    }
}
