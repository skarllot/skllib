// Process.cs
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

namespace Root.Performance
{
    
    /// <summary>
    /// Provides performance improvements to processes.
    /// </summary>
    public static class Process
    {
        /// <summary>
        /// Trims memory usage from current process.
        /// </summary>
        /// <exception cref="PlatformNotSupportedException">
        /// Only works on Microsoft Windows 2000 or better.
        /// </exception>
        public static void TrimMemoryUsage()
        {
            TrimMemoryUsage(System.Diagnostics.Process.GetCurrentProcess());
        }
        
        /// <summary>
        /// Trims memory usage from desired process.
        /// </summary>
        /// <param name="process">
        /// The process to trim memory usage.
        /// </param>
        /// <exception cref="PlatformNotSupportedException">
        /// Only works on Microsoft Windows 2000 or better.
        /// </exception>
        public static void TrimMemoryUsage(System.Diagnostics.Process process)
        {
            if (!OSInformation.IsWindows2000 && !OSInformation.IsPostWindows2000)
                throw new PlatformNotSupportedException(resExceptions.NeedWin2000OrBetter);
            
            IntPtr hProcess = process.Handle;
            SetProcessWorkingSetSize(hProcess, (IntPtr)(-1), (IntPtr)(-1));
        }
        
        [System.Runtime.InteropServices.DllImport("Kernel32.dll")]
        static extern bool SetProcessWorkingSetSize(IntPtr hProcess,
            IntPtr dwMinimumWorkingSetSize, IntPtr dwMaximumWorkingSetSize);
    }
}
