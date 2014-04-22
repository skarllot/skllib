// OSInformation.cs
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
    /// Provides basic information from current operating system.
    /// </summary>
    public static class OSInformation
    {
        #region Fields

        private static bool _isAspNetServer;
        private static bool _isPostWin2k;
        private static bool? _isServerEdition;
        private static bool _isWin2k;
        private static bool _isWin2k3;
        private static bool _isWin9x;
        private static bool _isWinCE;
        private static bool _leastWinHttp51;
        private static bool _isWinNt;
        private static OSNTVersion _os;
        private static OSOldVersion? _osOld;
        private static OSNTClientVersion? _osClient;
        private static OSServerVersion? _osServer;

        #endregion

        #region Properties

        /// <summary>
        /// Gets whether current machine have installed at least version 5.1 from HTTP.
        /// </summary>
        public static bool HasLeastHttp51
        {
            get { return _leastWinHttp51; }
        }

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
        /// Gets whether current operating system is Windows CE.
        /// </summary>
        public static bool IsWindowsCE
        {
            get { return _isWinCE; }
        }

        /// <summary>
        /// Gets whether current operating system is Windows NT Family.
        /// </summary>
        public static bool IsWindowsNT
        {
            get { return _isWinNt; }
        }

        /// <summary>
        /// Gets whether current operating system is Server Edition when possible.
        /// </summary>
        public static bool? IsWindowsServer
        {
            get { return _isServerEdition; }
        }

        /// <summary>
        /// Gets current operating system NT verstion.
        /// </summary>
        public static OSNTVersion GetNTVersion
        {
            get { return _os; }
        }

        /// <summary>
        /// Gets current operating system version when it is Windows NT workstation.
        /// </summary>
        public static OSNTClientVersion? GetOSNTVersion
        {
            get { return _osClient; }
        }

        /// <summary>
        /// Gets current operating system version when it is Windows 9x or Windows CE.
        /// </summary>
        public static OSOldVersion? GetOSOldVersion
        {
            get { return _osOld; }
        }

        /// <summary>
        /// Gets current operating system version when it is Windows Server edition.
        /// </summary>
        public static OSServerVersion? GetOsServerVersion
        {
            get { return _osServer; }
        }

        #endregion

        #region Constructor

        static OSInformation()
        {
            OperatingSystem os = System.Environment.OSVersion;
            int major = os.Version.Major;
            int minor = os.Version.Minor;

            if (os.Platform == PlatformID.Win32Windows)
            {
                if (major == 4)
                {
                    if (minor == 0)
                        _osOld = OSOldVersion.Windows95;
                    else if (minor == 1)
                        _osOld = OSOldVersion.Windows98;
                    else if (minor == 2)
                        _osOld = OSOldVersion.Windows98SE;
                    else if (minor == 9)
                        _osOld = OSOldVersion.WindowsME;
                    else
                        _osOld = OSOldVersion.Unidentifield;
                }
                _isWin9x = true;
                _os = OSNTVersion.NotNT;
            }
            else if (os.Platform == PlatformID.WinCE) {
                if (major < 3)
                    _osOld = OSOldVersion.WindowsCEOld;
                else if (major == 3)
                    _osOld = OSOldVersion.WindowsCE3;
                else if (major == 4)
                    _osOld = OSOldVersion.WindowsCE4;
                else if (major == 5)
                    _osOld = OSOldVersion.WindowsCE5;
                else if (major == 6)
                    _osOld = OSOldVersion.WindowsCE6;
                else
                    _osOld = OSOldVersion.WindowsCENew;
                _isWinCE = true;
                _os = OSNTVersion.NotNT;
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
                if (major < 5) {
                    _os = OSNTVersion.WindowsNTOld;
                    _osClient = OSNTClientVersion.WindowsNTOld;
                    _osServer = OSServerVersion.WindowsNTOld;
                }
            }
            else
                _osOld = OSOldVersion.Unidentifield;

            if (_isWinNt && major >= 5) {
                OSVERSIONINFOEX osinfoex = new OSVERSIONINFOEX();
                osinfoex.dwOSVersionInfoSize =
                    System.Runtime.InteropServices.Marshal.SizeOf(typeof(OSVERSIONINFOEX));
                GetVersionEx(ref osinfoex);
                bool isServer = (osinfoex.wProductType == ProductType.VER_NT_SERVER
                    || osinfoex.wProductType == ProductType.VER_NT_DOMAIN_CONTROLLER);
                _isServerEdition = isServer;

                if (major == 5 && minor == 0)
                    _isWin2k = true;
                else
                    _isPostWin2k = true;
                if (isServer && major == 5 && minor == 2)
                    _isWin2k3 = true;
                if ((major == 5 && minor == 0 && os.Version.MajorRevision >= 3)     // Windows 2000 SP3
                    || (major == 5 && minor == 1 && os.Version.MajorRevision >= 1)  // Windows XP SP1
                    || (major == 5 && minor > 1)                                    // Windows XP Pro x64, 2003 and 2003 R2
                    || (major > 5))                                                 // greater than 2003
                    _leastWinHttp51 = true;

                switch (major) {
                    case 5:
                        switch (minor) {
                            case 0:
                                _os = OSNTVersion.Windows2000;
                                if (isServer) _osServer = OSServerVersion.Windows2000;
                                else _osClient = OSNTClientVersion.Windows2000;
                                break;
                            case 1:
                                _os = OSNTVersion.WindowsXP;
                                if (isServer) _osServer = OSServerVersion.Invalid;
                                else _osClient = OSNTClientVersion.WindowsXP;
                                break;
                            case 2:
                                _os = OSNTVersion.Windows2003;
                                if (isServer) _osServer = OSServerVersion.Windows2003;
                                else _osClient = OSNTClientVersion.WindowsXPPro64;
                                break;
                            default:
                                _os = OSNTVersion.Invalid;
                                if (isServer) _osServer = OSServerVersion.Invalid;
                                else _osClient = OSNTClientVersion.Invalid;
                                break;
                        }
                        break;
                    case 6:
                        switch (minor) {
                            case 0:
                                _os = OSNTVersion.WindowsVistaOr2008;
                                if (isServer) _osServer = OSServerVersion.Windows2008;
                                else _osClient = OSNTClientVersion.WindowsVista;
                                break;
                            case 1:
                                _os = OSNTVersion.Windows7Or2008R2;
                                if (isServer) _osServer = OSServerVersion.Windows2008R2;
                                else _osClient = OSNTClientVersion.Windows7;
                                break;
                            case 2:
                                _os = OSNTVersion.Windows8Or2012;
                                if (isServer) _osServer = OSServerVersion.Windows2012;
                                else _osClient = OSNTClientVersion.Windows8;
                                break;
                            case 3:
                                _os = OSNTVersion.Windows81Or2012R2;
                                if (isServer) _osServer = OSServerVersion.Windows2012R2;
                                else _osClient = OSNTClientVersion.Windows8_1;
                                break;
                            default:
                                _os = OSNTVersion.WindowsNew;
                                if (isServer) _osServer = OSServerVersion.WindowsNew;
                                else _osClient = OSNTClientVersion.WindowsNew;
                                break;
                        }
                        break;
                    default:
                        _os = OSNTVersion.WindowsNew;
                        if (isServer) _osServer = OSServerVersion.WindowsNew;
                        else _osClient = OSNTClientVersion.WindowsNew;
                        break;
                }
            }
        }

        #endregion

        #region PInvoke

        private enum ProductType : byte
        {
            VER_NT_WORKSTATION = 0x01,
            VER_NT_SERVER = 0x03,
            VER_NT_DOMAIN_CONTROLLER = 0x02
        }

        // Reference: http://msdn.microsoft.com/en-us/library/ms724833%28v=vs.85%29.aspx
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        private struct OSVERSIONINFOEX
        {
            public int dwOSVersionInfoSize;
            public int dwMajorVersion;
            public int dwMinorVersion;
            public int dwBuildNumber;
            public int dwPlatformId;
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szCSDVersion;
            public short wServicePackMajor;
            public short wServicePackMinor;
            public short wSuiteMask;
            public ProductType wProductType;
            public byte wReserved;
        }

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool GetVersionEx(ref OSVERSIONINFOEX osVersionInfo);

        #endregion
    }
}
