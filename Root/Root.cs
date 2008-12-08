// Root.cs
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
using System.Globalization;
using Serialization = System.Runtime.Serialization;
using stringb = System.Text.StringBuilder;

/// <summary>
/// Namespace where are located basic code.
/// </summary>
namespace Root
{
	/// <summary>
	/// Represents a generic function that returns a type-specific.
	/// </summary>
	/// <typeparam name="T">Any type-specific.</typeparam>
	/// <returns>Specified type-specific.</returns>
	public delegate T GetType<T>();

	/// <summary>
	/// Byte multiples.
	/// </summary>
	public enum ByteMeasure : long
	{
		/// <summary>
		/// Represents a byte.
		/// </summary>
		Bytes = 1L,
		/// <summary>
		/// Represents a kilobyte (1024 B).
		/// </summary>
		Kilobytes = 1024L,
		/// <summary>
		/// Represents a megabyte (1024 KB).
		/// </summary>
		Megabytes = 1048576L,
		/// <summary>
		/// Represents a gigabyte (1024 MB).
		/// </summary>
		Gigabytes = 1073741824L,
		/// <summary>
		/// Represents a terabyte (1024 GB).
		/// </summary>
		Terabytes = 1099511627776L,
		/// <summary>
		/// Represents a petabyte (1024 TB).
		/// </summary>
		Petabytes = 1125899906842624L,
		/// <summary>
		/// Represents a exabyte (1024 PB).
		/// </summary>
		Exabytes = 1152921504606846976L
	}

	/// <summary>
	/// Linear units from International System of Units.
	/// </summary>
	public enum SILengthUnit : int
	{
		/// <summary>
		/// Millimeter length unit.
		/// </summary>
		Millimeter = 1,
		/// <summary>
		/// Centimeter length unit.
		/// </summary>
		Centimeter = 10,
		/// <summary>
		/// Decimeter length unit.
		/// </summary>
		Decimeter = 100,
		/// <summary>
		/// Meter length unit.
		/// </summary>
		Meter = 1000,
		/// <summary>
		/// Decameter length unit.
		/// </summary>
		Decameter = 10000,
		/// <summary>
		/// Hectometer length unit.
		/// </summary>
		Hectometer = 100000,
		/// <summary>
		/// Kilometer length unit.
		/// </summary>
		Kilometer = 1000000
	};

	/// <summary>
	/// Identifies a operating System version.
	/// </summary>
	public enum OSVersion
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
		WindowsME,
		/// <summary>
		/// Windows NT 4.x
		/// </summary>
		WindowsNT4,
		/// <summary>
		/// Windows 2000
		/// </summary>
		Windows2000,
		/// <summary>
		/// Windows XP
		/// </summary>
		WindowsXP,
		/// <summary>
		/// Windows Server 2003
		/// </summary>
		WindowsServer2003,
		/// <summary>
		/// Windows Vista
		/// </summary>
		WindowsVista,
		/// <summary>
		/// Windows newest than Vista
		/// </summary>
		WindowsNew
	}

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

	/// <summary>
	/// Stores a complete telephone number; includes country code, area code and
	/// subscriber number.
	/// </summary>
	public struct Telephone: IEquatable<Telephone>
	{
		#region Fields

		private short _country; //Globalization.CountryCallingCodes country;
		private short _countryLen;
		private short _area;
		private short _areaLen;
		private int _number;
		private short _numberLen;

		#endregion

		#region Constructors

		public Telephone(short countryCode, short areaCode, int subscriberNumber)
		{
			if (countryCode > 999)
				throw new OverflowException(resExceptions.TooBig_Number.Replace("%var", "countryCode").Replace("%value", "999"));
			if (subscriberNumber < 100)
				throw new OverflowException(resExceptions.TooSmall_Number.Replace("%var", "subscriberNumber")
					.Replace("%value", "100"));
			_country = countryCode; //(Globalization.CountryCallingCodes)countryCode;
			_area = areaCode;
			_number = subscriberNumber;

			_countryLen = (short)_country.ToString().Length;
			_areaLen = (short)_area.ToString().Length;
			_numberLen = (short)_number.ToString().Length;
		}

		#endregion

		#region Operators

		/// <summary>
		/// Determines whether two specified instances of Telephone are equal.
		/// </summary>
		/// <param name="l">A Telephone.</param>
		/// <param name="m">A Telephone.</param>
		/// <returns>true if tel1 and tel2 represent the same telephone value; otherwise, false.</returns>
		public static bool operator ==(Telephone tel1, Telephone tel2)
		{
			return (tel1._area == tel2._area) && (tel1._country == tel2._country) && (tel1._number == tel2._number);
		}

		/// <summary>
		/// Determines whether two specified instances of Telephone are not equal.
		/// </summary>
		/// <param name="l">A Telephone.</param>
		/// <param name="m">A Telephone.</param>
		/// <returns>true if tel1 and tel2 do not represent the same telephone value; otherwise, false</returns>
		public static bool operator !=(Telephone tel1, Telephone tel2)
		{
			return (tel1._area != tel2._area) || (tel1._country != tel2._country) || (tel1._number != tel2._number);
		}

		#endregion

		#region Overrides

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public override int GetHashCode()
		{
			return _country.GetHashCode() ^ _area.GetHashCode() ^ _number.GetHashCode();
		}

		/// <summary>
		/// Returns a value indicating whether this instance and a specified <see cref="T:System.Object"/>
		/// represent the same type and value.
		/// </summary>
		/// <param name="obj">An object to compare to this instance.</param>
		/// <returns>true if value is a Telephone and equal to this instance; otherwise, false.</returns>
		public override bool Equals(object obj)
		{
			if (obj is Telephone)
				return (Telephone)obj == this;
			return false;
		}

		/// <summary>
		/// Converts the numeric value of this instance to its equivalent <see cref="T:System.String"/> representation.
		/// </summary>
		/// <returns>A <see cref="T:System.String"/> representing the value of this instance.</returns>
		public override string ToString()
		{
			return this.ToString(null);
		}

		#endregion

		#region Methods

		public string ToString(string format)
		{
			if (string.IsNullOrEmpty(format))
				format = "+cc (ac) sn";
			format = format.Replace("cc", Formatting.Numbers.ToStringKeepLength(_country, _countryLen));
			format = format.Replace("ac", Formatting.Numbers.ToStringKeepLength(_area, _areaLen));
			format = format.Replace("sn", Formatting.Numbers.ToStringKeepLength(_number, _numberLen));
			return format;
		}

		public string ToBasicTelephoneString()
		{
			return string.Empty;
		}

		public string ToLocalTelephoneString()
		{
			return string.Empty;
		}

		public string ToCompleteTelephoneString()
		{
			return string.Empty;
		}

		public static Telephone FromString(string tel, string mask)
		{
			return new Telephone();
		}

		#endregion

		/// <summary>
		/// Alphabetic mnemonic system used to telephone numbers.
		/// </summary>
		/// <remarks>
		/// Another oddity of NANP telephone numbering is the popularity of alphabetic dialing. On most US and Canadian telephones,
		/// three letters appear on each number button from 2 through 9. This accommodates 24 letters. Historically, the letters Q
		/// and Z were omitted, though on some modern telephones, they are added.
		/// </remarks>
		public enum AlphabeticMnemonicSystem
		{
			/// <summary>
			/// Letter "A" is button 2.
			/// </summary>
			A = 2,
			/// <summary>
			/// Letter "B" is button 2.
			/// </summary>
			B = 2,
			/// <summary>
			/// Letter "C" is button 2.
			/// </summary>
			C = 2,
			/// <summary>
			/// Letter "D" is button 3.
			/// </summary>
			D = 3,
			/// <summary>
			/// Letter "E" is button 3.
			/// </summary>
			E = 3,
			/// <summary>
			/// Letter "F" is button 3.
			/// </summary>
			F = 3,
			/// <summary>
			/// Letter "G" is button 4.
			/// </summary>
			G = 4,
			/// <summary>
			/// Letter "H" is button 4.
			/// </summary>
			H = 4,
			/// <summary>
			/// Letter "I" is button 4.
			/// </summary>
			I = 4,
			/// <summary>
			/// Letter "J" is button 5.
			/// </summary>
			J = 5,
			/// <summary>
			/// Letter "K" is button 5.
			/// </summary>
			K = 5,
			/// <summary>
			/// Letter "L" is button 5.
			/// </summary>
			L = 5,
			/// <summary>
			/// Letter "M" is button 6.
			/// </summary>
			M = 6,
			/// <summary>
			/// Letter "N" is button 6.
			/// </summary>
			N = 6,
			/// <summary>
			/// Letter "O" is button 6.
			/// </summary>
			O = 6,
			/// <summary>
			/// Letter "P" is button 7.
			/// </summary>
			P = 7,
			/// <summary>
			/// Letter "Q" is button 7.
			/// </summary>
			Q = 7,
			/// <summary>
			/// Letter "R" is button 7.
			/// </summary>
			R = 7,
			/// <summary>
			/// Letter "S" is button 7.
			/// </summary>
			S = 7,
			/// <summary>
			/// Letter "T" is button 8.
			/// </summary>
			T = 8,
			/// <summary>
			/// Letter "U" is button 8.
			/// </summary>
			U = 8,
			/// <summary>
			/// Letter "V" is button 8.
			/// </summary>
			V = 8,
			/// <summary>
			/// Letter "W" is button 9.
			/// </summary>
			W = 9,
			/// <summary>
			/// Letter "X" is button 9.
			/// </summary>
			X = 9,
			/// <summary>
			/// Letter "Y" is button 9.
			/// </summary>
			Y = 9,
			/// <summary>
			/// Letter "Z" is button 9.
			/// </summary>
			Z = 9
		}

		#region IEquatable<Telephone> Members

		/// <summary>
		/// Returns a value indicating whether this instance is equal to the specified Telephone instance.
		/// </summary>
		/// <param name="other">A Telephone instance to compare to this instance.</param>
		/// <returns>true if the value parameter equals the value of this instance; otherwise, false.</returns>
		public bool Equals(Telephone other)
		{
			return this == other;
		}

		#endregion
	}

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
					_isWinHttp51 = true;	// probably newest
				}
				else if (major > 6)
				{
					_os = OSVersion.WindowsNew;
					_isPostWin2k = true;
					_isWinHttp51 = true;	// propably newest
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

	/// <summary>
	/// Stores a object protected by multiaccess.
	/// </summary>
	/// <typeparam name="T">Type of object to protect.</typeparam>
	public class LockedMultiAccess<T>
	{
		#region Fields

		private T userVar;
		private object isLocked;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes LockedMultiAccess.
		/// </summary>
		public LockedMultiAccess()
		{
			isLocked = (object)false;
		}

		/// <summary>
		/// Initializes LockedMultiAccess and stores a initial value.
		/// </summary>
		/// <param name="value"></param>
		public LockedMultiAccess(T value)
		{
			userVar = value;
			isLocked = (object)false;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the object stored by this instance directly.
		/// </summary>
		public T DirectAccess
		{
			get { return userVar; }
			set { userVar = value; }
		}

		/// <summary>
		/// Gets or sets the object stored by this instance; If the object is locked then
		/// waits to unlock.
		/// </summary>
		public T SafeAccess
		{
			get
			{
				LockObject();
				T obj = userVar;
				UnLockObject();
				return obj;
			}
			set
			{
				LockObject();
				userVar = value;
				UnLockObject();
			}
		}

		/// <summary>
		/// Gets a boolean indicating whether current object is locked.
		/// </summary>
		public bool IsLocked
		{
			get
			{
				if (!System.Threading.Monitor.TryEnter(isLocked))
					return true;
				System.Threading.Monitor.Exit(isLocked);
				return false;
				//System.Threading.Thread.Sleep(1);
				//bool b1 = (bool)isLocked;
				//return b1;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Locks stored object to be accessed by <see cref="SafeAccess"/>, if the object is locked then
		/// waits to unlock.
		/// </summary>
		public void LockObject()
		{
			System.Threading.Monitor.Enter(this.isLocked);
		//inicio:
		//    while ((bool)isLocked)
		//    {
		//        System.Threading.Thread.Sleep(1);
		//    }
		//    if (!internalIsLocked(true))
		//        goto inicio;
		}

		/// <summary>
		/// Unlocks stored object to be accessed by <see cref="SafeAccess"/>.
		/// </summary>
		public void UnLockObject()
		{
			System.Threading.Monitor.Exit(this.isLocked);
			//internalIsLocked(false);
			//System.Threading.Thread.Sleep(1);
		}

		//private bool internalIsLocked(bool value)
		//{
		//    lock (isLocked)
		//    {
		//        if ((bool)isLocked && value)
		//            return false;

		//        isLocked = (object)value;
		//        return true;
		//    }
		//}

		#endregion
	}

}
