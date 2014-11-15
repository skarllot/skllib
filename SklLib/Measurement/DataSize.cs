// InformationSize.cs
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
using System.Collections.Generic;
using System.Globalization;
using Serialization = System.Runtime.Serialization;

namespace SklLib.Measurement
{
    /// <summary>
    /// Represents a information size.
    /// </summary>
    [Serializable]
    public struct InformationSize : IComparable, IComparable<InformationSize>, IEquatable<InformationSize>, Serialization.ISerializable
    {
        #region Fields

        private static readonly Dictionary<ByteIEC, Formatting.GrammarNumberWriteInfo> fullWriteList;
        private static readonly Dictionary<ByteIEC, Formatting.GrammarNumberWriteInfo> contractedWriteList;
        private ulong _bValue;

        #endregion

        #region Constructors

        static InformationSize()
        {
            fullWriteList = new Dictionary<ByteIEC, Formatting.GrammarNumberWriteInfo>();
            fullWriteList.Add(ByteIEC.Byte, new Formatting.GrammarNumberWriteInfo("Byte", "Bytes"));
        }

        /// <summary>
        /// Initializes a new instance of the InformationSize structure to specified value in bytes.
        /// </summary>
        /// <param name="bytes">A value in bytes.</param>
        public InformationSize(ulong bytes)
        {
            this._bValue = bytes;
        }

        /// <summary>
        /// Initializes a new instance of the LinearSize structure to specified value.
        /// </summary>
        /// <param name="value">A value.</param>
        /// <param name="mult">Greatness of value.</param>
        /// <exception cref="OverflowException">The value is greater than 8 Exbibytes.</exception>
        public InformationSize(ulong value, ByteIEC mult)
        {
            this._bValue = value * (ulong)mult;
        }

        private InformationSize(Serialization.SerializationInfo info, Serialization.StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException("info", resExceptions.ArgumentNull.Replace("%var", "info"));

            _bValue = info.GetUInt64("_bValue");
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value represented by this instance in bytes.
        /// </summary>
        public ulong Bytes
        {
            get { return _bValue; }
            set { _bValue = value; }
        }

        /// <summary>
        /// Gets or sets the value represented by this instance in kibibytes.
        /// </summary>
        /// <exception cref="OverflowException">The value is greater than 8 Exbibytes.</exception>
        public decimal Kibibytes
        {
            get { return (decimal)_bValue / (ulong)ByteIEC.Kibibyte; }
            set { _bValue = (ulong)(value * (ulong)ByteIEC.Kibibyte); }
        }

        /// <summary>
        /// Gets or sets the value represented by this instance in mebibytes.
        /// </summary>
        /// <exception cref="OverflowException">The value is greater than 8 Exbibytes.</exception>
        public decimal Mebibytes
        {
            get { return (decimal)_bValue / (ulong)ByteIEC.Mebibyte; }
            set { _bValue = (ulong)(value * (ulong)ByteIEC.Mebibyte); }
        }

        /// <summary>
        /// Gets or sets the value represented by this instance in gibibytes.
        /// </summary>
        /// <exception cref="OverflowException">The value is greater than 8 Exbibytes.</exception>
        public decimal Gibibytes
        {
            get { return (decimal)_bValue / (ulong)ByteIEC.Gibibyte; }
            set { _bValue = (ulong)(value * (ulong)ByteIEC.Gibibyte); }
        }

        /// <summary>
        /// Gets or sets the value represented by this instance in tebibytes.
        /// </summary>
        /// <exception cref="OverflowException">The value is greater than 8 Exbibytes.</exception>
        public decimal Tebibytes
        {
            get { return (decimal)_bValue / (ulong)ByteIEC.Tebibyte; }
            set { _bValue = (ulong)(value * (ulong)ByteIEC.Tebibyte); }
        }

        /// <summary>
        /// Gets or sets the value represented by this instance in pebibytes.
        /// </summary>
        /// <exception cref="OverflowException">The value is greater than 8 Exbibytes.</exception>
        public decimal Pebibytes
        {
            get { return (decimal)_bValue / (ulong)ByteIEC.Pebibyte; }
            set { _bValue = (ulong)(value * (ulong)ByteIEC.Pebibyte); }
        }

        /// <summary>
        /// Gets or sets the value represented by this instance in exbibytes.
        /// </summary>
        /// <exception cref="OverflowException">The value is greater than 8 Exbibytes.</exception>
        public decimal Exbibytes
        {
            get { return (decimal)_bValue / (ulong)ByteIEC.Exbibyte; }
            set { _bValue = (ulong)(value * (ulong)ByteIEC.Exbibyte); }
        }

        #endregion

        #region Operators

        /// <summary>
        /// Adds two specified InformationSize values.
        /// </summary>
        /// <param name="op1">A InformationSize.</param>
        /// <param name="op2">A InformationSize.</param>
        /// <returns>The InformationSize result of adding op1 and op2.</returns>
        /// <exception cref="OverflowException">The return value is greater than <see cref="F:System.UInt64.MaxValue"/>.</exception>
        public static InformationSize operator +(InformationSize op1, InformationSize op2)
        {
            return new InformationSize(op1._bValue + op2._bValue);
        }

        /// <summary>
        /// Subtracts two specified InformationSize values.
        /// </summary>
        /// <param name="op1">A InformationSize.</param>
        /// <param name="op2">A InformationSize.</param>
        /// <returns>The InformationSize result of subtracting op1 from op2.</returns>
        public static InformationSize operator -(InformationSize op1, InformationSize op2)
        {
            return new InformationSize(op1._bValue - op2._bValue);
        }

        /// <summary>
        /// Multiplies two specified InformationSize values.
        /// </summary>
        /// <param name="op1">A InformationSize.</param>
        /// <param name="op2">A InformationSize.</param>
        /// <returns>The InformationSize result of multiplying op1 by op2.</returns>
        /// <exception cref="OverflowException">The return value is greater than <see cref="F:System.UInt64.MaxValue"/>.</exception>
        public static InformationSize operator *(InformationSize op1, InformationSize op2)
        {
            return new InformationSize(op1._bValue * op2._bValue);
        }

        /// <summary>
        /// Divides two specified InformationSize values.
        /// </summary>
        /// <param name="op1">A InformationSize (the dividend).</param>
        /// <param name="op2">A InformationSize (the divisor).</param>
        /// <returns>The InformationSize result from dividing of op1 by op2.</returns>
        public static InformationSize operator /(InformationSize op1, InformationSize op2)
        {
            return new InformationSize(op1._bValue / op2._bValue);
        }

        /// <summary>
        /// Returns the remainder resulting from dividing two specified InformationSize values.
        /// </summary>
        /// <param name="op1">A InformationSize.</param>
        /// <param name="op2">A InformationSize.</param>
        /// <returns>The InformationSize remainder resulting from dividing d1 by d2.</returns>
        public static InformationSize operator %(InformationSize op1, InformationSize op2)
        {
            return new InformationSize(op1._bValue % op2._bValue);
        }

        /// <summary>
        /// Determines whether two specified instances of InformationSize are equal.
        /// </summary>
        /// <param name="b">A InformationSize.</param>
        /// <param name="m">A InformationSize.</param>
        /// <returns>true if b and m represent the same binary measure value; otherwise, false.</returns>
        public static bool operator ==(InformationSize b, InformationSize m)
        {
            return b._bValue == m._bValue;
        }

        /// <summary>
        /// Determines whether two specified instances of InformationSize are not equal.
        /// </summary>
        /// <param name="b">A InformationSize.</param>
        /// <param name="m">A InformationSize.</param>
        /// <returns>true if b and m do not represent the same binary measure value; otherwise, false</returns>
        public static bool operator !=(InformationSize b, InformationSize m)
        {
            return b._bValue != m._bValue;
        }

        /// <summary>
        /// Determines whether one specified InformationSize is greater than another specified InformationSize.
        /// </summary>
        /// <param name="b">A InformationSize.</param>
        /// <param name="m">A InformationSize.</param>
        /// <returns>true if b is greater than m; otherwise, false.</returns>
        public static bool operator >(InformationSize b, InformationSize m)
        {
            return b._bValue > m._bValue;
        }

        /// <summary>
        /// Determines whether one specified InformationSize is greater than or equal to another specified InformationSize.
        /// </summary>
        /// <param name="b">A InformationSize.</param>
        /// <param name="m">A InformationSize.</param>
        /// <returns>true if b is greater than or equal to m; otherwise, false.</returns>
        public static bool operator >=(InformationSize b, InformationSize m)
        {
            return b._bValue >= m._bValue;
        }

        /// <summary>
        /// Determines whether one specified InformationSize is less than another specified InformationSize.
        /// </summary>
        /// <param name="b">A InformationSize.</param>
        /// <param name="m">A InformationSize.</param>
        /// <returns>true if b is less than m; otherwise, false.</returns>
        public static bool operator <(InformationSize b, InformationSize m)
        {
            return b._bValue < m._bValue;
        }

        /// <summary>
        /// Determines whether one specified InformationSize is less than or equal to another specified InformationSize.
        /// </summary>
        /// <param name="b">A InformationSize.</param>
        /// <param name="m">A InformationSize.</param>
        /// <returns>true if b is less than or equal to m; otherwise, false.</returns>
        public static bool operator <=(InformationSize b, InformationSize m)
        {
            return b._bValue <= m._bValue;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return _bValue.GetHashCode();
        }

        /// <summary>
        /// Returns a value indicating whether this instance and a specified <see cref="T:System.Object"/>
        /// represent the same type and value.
        /// </summary>
        /// <param name="obj">An object to compare to this instance.</param>
        /// <returns>true if value is a InformationSize and equal to this instance; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is InformationSize)
                return (InformationSize)obj == this;
            return false;
        }

        /// <summary>
        /// Converts the numeric value of this instance to its equivalent <see cref="T:System.String"/> representation
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> representing the value of this instance.</returns>
        public override string ToString()
        {
            return this.ToString(null, NumberFormatInfo.CurrentInfo);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the value represented by this instance in the specified measure.
        /// </summary>
        /// <param name="mult">Specifies a measure.</param>
        /// <returns>The value represented by this instance in the specified measure.</returns>
        public decimal GetValue(ByteIEC mult)
        {
            return (decimal)_bValue / (ulong)mult;
        }

        /// <summary>
        /// Converts the value of this instance to its equivalent string representation using the specified format.
        /// </summary>
        /// <param name="format">A format string.</param>
        /// <returns>A string representation of value of this instance as specified by <c>format</c>.</returns>
        /// <exception cref="FormatException">format does not contain a valid custom format pattern.</exception>
        /// <example>
        /// The following console application example show how to use InformationSize.ToString method.
        /// <code>
        /// using System;
        /// using SklLib;
        /// 
        /// class Program
        /// {
        ///     static void Main()
        ///     {
        ///            InformationSize binMea = new InformationSize();
        ///            binMea.Mebibytes = 15.317M;
        /// 
        ///            Console.WriteLine(binMea.ToString("G|M-"));
        ///                // output: "15.3169994354248046875 MiB"
        /// 
        ///            Console.WriteLine(binMea.ToString("G|M+"));
        ///                // output: "15.3169994354248046875 Mebibytes"
        /// 
        ///            Console.WriteLine(binMea.ToString("N2|M-"));
        ///                // output: "15.32 MiB"
        /// 
        ///            Console.WriteLine(binMea.ToString("G|K-"));
        ///                // output: "15684.607421875 KiB"
        /// 
        ///         Console.WriteLine(binMea.ToString("N1|K-"));
        ///                // output: "15,684.6 KiB"
        /// 
        ///            binMea.Gibibytes = 4000;
        /// 
        ///            Console.WriteLine(binMea.ToString("G|>-"));
        ///                // output: "3.90625 TiB"
        /// 
        ///         Console.WriteLine(binMea.ToString("N0|>-"));
        ///                // output: "4 TiB"
        /// 
        ///         Console.WriteLine(binMea.ToString("G|0-"));
        ///                // output: "4000 GiB"
        /// 
        ///         Console.ReadKey();
        ///     }
        /// }
        /// </code>
        /// </example>
        public string ToString(string format)
        {
            return this.ToString(format, NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Converts the value of this instance to its equivalent string representation using the specified format
        /// and culture-specific format information.
        /// </summary>
        /// <param name="format">A format string.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <returns>A string representation of value of this instance as specified by <c>format</c> and <c>provider</c>.</returns>
        /// <exception cref="FormatException">format does not contain a valid custom format pattern.</exception>
        /// <example>
        /// The following console application example show how to use InformationSize.ToString method.
        /// <code>
        /// using System;
        /// using System.Globalization;
        /// using SklLib;
        /// 
        /// class Program
        /// {
        ///     static void Main()
        ///     {
        ///            InformationSize binMea = new InformationSize();
        ///            binMea.Mebibytes = 15.317M;
        /// 
        ///            Console.WriteLine(binMea.ToString("G|M-", NumberFormatInfo.CurrentInfo));
        ///                // output: "15.3169994354248046875 MiB"
        /// 
        ///            Console.WriteLine(binMea.ToString("G|M+", NumberFormatInfo.CurrentInfo));
        ///                // output: "15.3169994354248046875 Mebibytes"
        /// 
        ///            Console.WriteLine(binMea.ToString("N2|M-", NumberFormatInfo.CurrentInfo));
        ///                // output: "15.32 MiB"
        /// 
        ///            Console.WriteLine(binMea.ToString("G|K-", NumberFormatInfo.CurrentInfo));
        ///                // output: "15684.607421875 KiB"
        /// 
        ///         Console.WriteLine(binMea.ToString("N1|K-", NumberFormatInfo.CurrentInfo));
        ///                // output: "15,684.6 KiB"
        /// 
        ///            binMea.Gibibytes = 4000;
        /// 
        ///            Console.WriteLine(binMea.ToString("G|>-", NumberFormatInfo.CurrentInfo));
        ///                // output: "3.90625 TiB"
        /// 
        ///         Console.WriteLine(binMea.ToString("N0|>-", NumberFormatInfo.CurrentInfo));
        ///                // output: "4 TiB"
        /// 
        ///         Console.WriteLine(binMea.ToString("G|0-", NumberFormatInfo.CurrentInfo));
        ///                // output: "4000 GiB"
        /// 
        ///         Console.ReadKey();
        ///     }
        /// }
        /// </code>
        /// </example>
        public string ToString(string format, IFormatProvider provider)
        {
            if (string.IsNullOrEmpty(format))
                format = "G";
            NumberFormatInfo nfi = NumberFormatInfo.GetInstance(provider);

            // Verify whether has the secondary format
            if (format.IndexOf('|') == -1)
                format = format + "|>-"; // if no, creates a standart format

            // ----- Split format-string, getting 2 formats -----
            string[] str = format.Split('|');

            if (str.Length != 2)
                throw new FormatException(resExceptions.Format_InvalidString);

            string fmtNumber = str[0];
            string fmtBinM = str[1].ToUpper();
            str = null;
            // --------------------------------------------------

            decimal value = 0.0M;    // stores a returned value
            string strMult = "";    // stores multiple spell (by select option)

            // ----- Process secondary format string -----
            if (fmtBinM.Length != 2)
                throw new FormatException(resExceptions.Format_InvalidString);

            string primaryChars = "BKMGTPE>0";    // allowed chars on first index
            string secondaryChars = "-+";        // allowed chars on second index

            int pIdx = primaryChars.IndexOf(fmtBinM[0]);    // stores index of first format-char on primaryChars
            int sIdx = secondaryChars.IndexOf(fmtBinM[1]);    // stores index of second format-char on secondaryChars

            if (pIdx == -1 || sIdx == -1)        // Verify whether has a invalid chars
                throw new FormatException(resExceptions.Format_InvalidString);

            ByteIEC toBMult;        // Verify selected option
            if (pIdx < 7)
                toBMult = GetMultByDefined(primaryChars[pIdx]);
            else if (pIdx == 7)
                toBMult = GetMultByGreat();
            else
                toBMult = GetMultByZeroable();

            value = (decimal)this._bValue / (ulong)toBMult;    // stores the value in specified multiple

            // Define the multiple spell
            strMult = Enum.GetName(typeof(ByteIEC), toBMult);
            if (sIdx == 0)
                strMult = strMult[0] == 'B' ? "B" : strMult[0] + "B";
            // -------------------------------------------

            return value.ToString(fmtNumber, nfi) + " " + strMult;
        }

        private ByteIEC GetMultByDefined(char greatness)
        {
            string multiples = "BKMGTPE";

            int idx = multiples.IndexOf(greatness);

            if (idx == -1)
                return ByteIEC.Byte;
            else
            {
                ulong[] val = (ulong[])Enum.GetValues(typeof(ByteIEC));
                return (ByteIEC)val[idx];
            }
        }

        private ByteIEC GetMultByGreat()
        {
            ulong[] values = (ulong[])Enum.GetValues(typeof(ByteIEC));
            ulong bytes = this._bValue;

            for (int i = values.Length - 1; i >= 0; i--)
            {
                if (bytes >= values[i])
                    return (ByteIEC)values[i];
            }

            return ByteIEC.Byte;
        }

        private ByteIEC GetMultByZeroable()
        {
            if (this._bValue == 0)
                return ByteIEC.Byte;

            ulong[] values = (ulong[])Enum.GetValues(typeof(ByteIEC));
            ulong bytes = this._bValue;

            for (int i = values.Length - 1; i > 0; i--)
            {
                decimal val = (decimal)this._bValue / values[i];
                if ((long)val == val)
                    return (ByteIEC)values[i];
            }

            return GetMultByGreat();
        }

        #endregion

        #region IComparable Members

        /// <summary>
        /// Compares this instance to a specified object and returns an indication of their relative values.
        /// </summary>
        /// <param name="obj">A boxed InformationSize object to compare, or null.</param>
        /// <returns>
        /// <para>A signed number indicating the relative values of this instance and value.</para>
        /// <para>Value Description:</para>
        /// <para>- Less than zero: This instance is less than value.</para>
        /// <para>- Zero: This instance is equal to value.</para>
        /// <para>- Greater than zero: This instance is greater than value, or value is null.</para></returns>
        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            if (!(obj is InformationSize))
                throw new ArgumentException(resExceptions.Obj_MustBeType.Replace("%var", "InformationSize"));

            InformationSize other = (InformationSize)obj;
            if (this > other)
                return 1;
            if (this < other)
                return -1;
            return 0;
        }

        #endregion

        #region IComparable<InformationSize> Members

        /// <summary>
        /// Compares this instance to a specified InformationSize object and returns an indication of their relative values.
        /// </summary>
        /// <param name="other">A InformationSize object to compare.</param>
        /// <returns>
        /// <para>A signed number indicating the relative values of this instance and the value parameter.</para>
        /// <para>Value Description:</para>
        /// <para>- Less than zero: This instance is less than value.</para>
        /// <para>- Zero: This instance is equal to value.</para>
        /// <para>- Greater than zero: This instance is greater than value.</para></returns>
        public int CompareTo(InformationSize other)
        {
            if (this > other)
                return 1;
            if (this < other)
                return -1;
            return 0;
        }

        #endregion

        #region IEquatable<InformationSize> Members

        /// <summary>
        /// Returns a value indicating whether this instance is equal to the specified InformationSize instance.
        /// </summary>
        /// <param name="other">A InformationSize instance to compare to this instance.</param>
        /// <returns>true if the value parameter equals the value of this instance; otherwise, false.</returns>
        public bool Equals(InformationSize other)
        {
            return this == other;
        }

        #endregion

        #region ISerializable Members

        void Serialization.ISerializable.GetObjectData(Serialization.SerializationInfo info, Serialization.StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException("info", resExceptions.ArgumentNull.Replace("%var", "info"));

            info.AddValue("_bValue", _bValue);
        }

        #endregion
    }
}
