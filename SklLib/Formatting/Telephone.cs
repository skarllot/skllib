// Telephone.cs
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

namespace SklLib.Formatting
{
    /// <summary>
    /// Stores a complete telephone number; includes country code, area code and
    /// subscriber number.
    /// </summary>
    [Obsolete("The type Telephone was not implemented yet", true)]
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

        /// <summary>
        /// Initializes a new instance of <see cref="Telephone"/> and defines country code,
        /// area code and subscriber number.
        /// </summary>
        /// <param name="countryCode">The country code.</param>
        /// <param name="areaCode">The area code.</param>
        /// <param name="subscriberNumber">The subscriber number.</param>
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
        /// <param name="tel1">A Telephone.</param>
        /// <param name="tel2">A Telephone.</param>
        /// <returns>true if tel1 and tel2 represent the same telephone value; otherwise, false.</returns>
        public static bool operator ==(Telephone tel1, Telephone tel2)
        {
            return (tel1._area == tel2._area) && (tel1._country == tel2._country) && (tel1._number == tel2._number);
        }

        /// <summary>
        /// Determines whether two specified instances of Telephone are not equal.
        /// </summary>
        /// <param name="tel1">A Telephone.</param>
        /// <param name="tel2">A Telephone.</param>
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

        /// <summary>
        /// Converts the numeric value of this instance to its equivalent <see cref="T:System.String"/> representation.
        /// </summary>
        /// <param name="format">The formatting string.</param>
        /// <returns>This <see cref="Telephone"/> instance formatted as indicated into format string.</returns>
        public string ToString(string format)
        {
            if (string.IsNullOrEmpty(format))
                format = "+cc (ac) sn";
            format = format.Replace("cc", Formatting.Numbers.ToStringKeepLength(_country, _countryLen));
            format = format.Replace("ac", Formatting.Numbers.ToStringKeepLength(_area, _areaLen));
            format = format.Replace("sn", Formatting.Numbers.ToStringKeepLength(_number, _numberLen));
            return format;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ToBasicTelephoneString()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ToLocalTelephoneString()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ToCompleteTelephoneString()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tel"></param>
        /// <param name="mask"></param>
        /// <returns></returns>
        public static Telephone FromString(string tel, string mask)
        {
            throw new NotImplementedException();
        }

        #endregion

        /// <summary>
        /// Alphabetic mnemonic system used to telephone numbers.
        /// </summary>
        /// <remarks>
        /// An oddity of NANP telephone numbering is the popularity of
        /// alphabetic dialing. On most US and Canadian telephones, three
        /// letters appear on each number button from 2 through 9. This
        /// accommodates 24 letters. Historically, the letters Q and Z were
        /// omitted, though on some modern telephones, they are added.
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
}
