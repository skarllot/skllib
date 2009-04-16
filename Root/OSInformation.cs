// OSInformation.cs
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

namespace Root
{
    /// <summary>
    /// Provides basic information from current operating system.
    /// </summary>
    public static class OSInformation
    {
        #region Fields

        private static bool _isAspNetServer;
        private static bool _isPostWin2k;
        private static bool _isWin2k;
        private static bool _isWin2k3;
        private static bool _isWin9x;
        private static bool _isWinHttp51;
        private static bool _isWinNt;
        private static OSVersion _os;

        #endregion

        #region Properties

        /// <summary>
        /// Gets whether this machine is ASP.NET Server.
        /// </summary>
        public static bool IsAspNetServer
        {
            get { return _isAspNetServer; }
        }

        /// <summary>
        /// Gets whether current operating system is newest than Windows 2000.
        /// </summary>
        public static bool IsPostWindows2000
        {
            get { return _isPostWin2k; }
        }

        /// <summary>
        /// Gets whether current operating system is Windows 2000.
        /// </summary>
        public static bool IsWindows2000
        {
            get { return _isWin2k; }
        }

        /// <summary>
        /// Gets whether current operating system is Windows 2003.
        /// </summary>
        public static bool IsWindows2003
        {
            get { return _isWin2k3; }
        }

        /// <summary>
        /// Gets whether current operating system is Windows 9x.
        /// </summary>
        public static bool IsWindows9x
        {
            get { return _isWin9x; }
        }

        /// <summary>
        /// Gets whether current machine have installed HTTP 5.1.
        /// </summary>
        public static bool IsHttp51
        {
            get { return _isWinHttp51; }
        }

        /// <summary>
        /// Gets whether current operating system is Windows NT Family.
        /// </summary>
        public static bool IsWindowsNT
        {
            get { return _isWinNt; }
        }

        /// <summary>
        /// Gets current operating system version.
        /// </summary>
        public static OSVersion GetOSVersion
        {
            get { return _os; }
        }

        #endregion

        #region Constructor

        static OSInformation()
        {
            OperatingSystem os = Environment.OSVersion;
            int major = os.Version.Major;
            int minor = os.Version.Minor;

            if (os.Platform == PlatformID.Win32Windows)
            {
                if (major == 4)
                {
                    if (minor == 0)
                        _os = OSVersion.Windows95;
                    else if (minor == 1)
                        _os = OSVersion.Windows98;
                    else if (minor == 2)
                        _os = OSVersion.Windows98SE;
                    else if (minor == 9)
                        _os = OSVersion.WindowsME;
                    else
                        _os = OSVersion.Unidentifield;
                }
                _isWin9x = true;
            }
            else if (os.Platform == PlatformID.Win32NT)
            {
                try
                {
                    _isAspNetServer =
                        System.Threading.Thread.GetDomain().GetData(".appDomain") != null;
                }
                catch { }

                _isWinNt = true;
                //_isWin2k = true;
                if (major == 4)
                    _os = OSVersion.WindowsNT4;
                else if (major == 5)
                {
                    if (minor == 0)
                    {
                        _os = OSVersion.Windows2000;
                        _isWin2k = true;
                        _isWinHttp51 = os.Version.MajorRevision >= 3;
                    }
                    else
                    {
                        _isPostWin2k = true;
                        if (minor == 1)
                        {
                            _os = OSVersion.WindowsXP;
                            _isWinHttp51 = os.Version.MajorRevision >= 1;
                        }
                        else
                        {
                            _isWinHttp51 = true;
                            _isWin2k3 = true;
                            if (minor == 2)
                                _os = OSVersion.WindowsServer2003;
                        }
                    }
                }
                else if (major == 6)
                {
                    _os = OSVersion.WindowsVista;
                    _isPostWin2k = true;
                    _isWinHttp51 = true;    // probably newest
                }
                else if (major > 6)
                {
                    _os = OSVersion.WindowsNew;
                    _isPostWin2k = true;
                    _isWinHttp51 = true;    // propably newest
                }
                else
                    _os = OSVersion.Unidentifield;
            }
            else if (os.Platform == PlatformID.WinCE)
            {
                if (major < 3)
                    _os = OSVersion.WindowsCEOld;
                else if (major == 3)
                    _os = OSVersion.WindowsCE3;
                else if (major == 4)
                    _os = OSVersion.WindowsCE4;
                else if (major == 5)
                    _os = OSVersion.WindowsCE5;
                else if (major == 6)
                    _os = OSVersion.WindowsCE6;
                else
                    _os = OSVersion.WindowsCENew;
            }
            else
                _os = OSVersion.Unidentifield;
        }

        #endregion
    }
}
