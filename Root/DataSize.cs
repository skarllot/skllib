// DataSize.cs
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

namespace SklLib
{
    /// <summary>
    /// Represents a data size, based on bytes multiples.
    /// </summary>
    [Serializable]
    public struct DataSize : IComparable, IComparable<DataSize>, IEquatable<DataSize>, Serialization.ISerializable
    {
        #region Fields

        private long _bValue;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the DataSize structure to specified bytes value.
        /// </summary>
        /// <param name="bytes">A value in bytes.</param>
        public DataSize(long bytes)
        {
            this._bValue = bytes;
        }

        /// <summary>
        /// Initializes a new instance of the LinearSize structure to specified value.
        /// </summary>
        /// <param name="value">A value.</param>
        /// <param name="mult">Greatness of value.</param>
        /// <exception cref="OverflowException">The value is greater than 8 Exabytes.</exception>
        public DataSize(long value, ByteMeasure mult)
        {
            this._bValue = value * (long)mult;
        }

        private DataSize(Serialization.SerializationInfo info, Serialization.StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException("info", resExceptions.ArgumentNull.Replace("%var", "info"));

            _bValue = info.GetInt64("_bValue");
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value represented by this instance in bytes.
        /// </summary>
        public long Bytes
        {
            get { return _bValue; }
            set { _bValue = value; }
        }

        /// <summary>
        /// Gets or sets the value represented by this instance in kilobytes.
        /// </summary>
        /// <exception cref="OverflowException">The value is greater than 8 Exabytes.</exception>
        public decimal Kilobytes
        {
            get { return (decimal)_bValue / (long)ByteMeasure.Kilobytes; }
            set { _bValue = (long)(value * (long)ByteMeasure.Kilobytes); }
        }

        /// <summary>
        /// Gets or sets the value represented by this instance in megabytes.
        /// </summary>
        /// <exception cref="OverflowException">The value is greater than 8 Exabytes.</exception>
        public decimal Megabytes
        {
            get { return (decimal)_bValue / (long)ByteMeasure.Megabytes; }
            set { _bValue = (long)(value * (long)ByteMeasure.Megabytes); }
        }

        /// <summary>
        /// Gets or sets the value represented by this instance in gigabytes.
        /// </summary>
        /// <exception cref="OverflowException">The value is greater than 8 Exabytes.</exception>
        public decimal Gigabytes
        {
            get { return (decimal)_bValue / (long)ByteMeasure.Gigabytes; }
            set { _bValue = (long)(value * (long)ByteMeasure.Gigabytes); }
        }

        /// <summary>
        /// Gets or sets the value represented by this instance in terabytes.
        /// </summary>
        /// <exception cref="OverflowException">The value is greater than 8 Exabytes.</exception>
        public decimal Terabytes
        {
            get { return (decimal)_bValue / (long)ByteMeasure.Terabytes; }
            set { _bValue = (long)(value * (long)ByteMeasure.Terabytes); }
        }

        /// <summary>
        /// Gets or sets the value represented by this instance in petabytes.
        /// </summary>
        /// <exception cref="OverflowException">The value is greater than 8 Exabytes.</exception>
        public decimal Petabytes
        {
            get { return (decimal)_bValue / (long)ByteMeasure.Petabytes; }
            set { _bValue = (long)(value * (long)ByteMeasure.Petabytes); }
        }

        /// <summary>
        /// Gets or sets the value represented by this instance in exabytes.
        /// </summary>
        /// <exception cref="OverflowException">The value is greater than 8 Exabytes.</exception>
        public decimal Exabytes
        {
            get { return (decimal)_bValue / (long)ByteMeasure.Exabytes; }
            set { _bValue = (long)(value * (long)ByteMeasure.Exabytes); }
        }

        #endregion

        #region Operators

        /// <summary>
        /// Adds two specified DataSize values.
        /// </summary>
        /// <param name="op1">A DataSize.</param>
        /// <param name="op2">A DataSize.</param>
        /// <returns>The DataSize result of adding op1 and op2.</returns>
        /// <exception cref="OverflowException">The return value is greater than <see cref="F:System.UInt64.MaxValue"/>.</exception>
        public static DataSize operator +(DataSize op1, DataSize op2)
        {
            return new DataSize(op1._bValue + op2._bValue);
        }

        /// <summary>
        /// Subtracts two specified DataSize values.
        /// </summary>
        /// <param name="op1">A DataSize.</param>
        /// <param name="op2">A DataSize.</param>
        /// <returns>The DataSize result of subtracting op1 from op2.</returns>
        public static DataSize operator -(DataSize op1, DataSize op2)
        {
            return new DataSize(op1._bValue - op2._bValue);
        }

        /// <summary>
        /// Multiplies two specified DataSize values.
        /// </summary>
        /// <param name="op1">A DataSize.</param>
        /// <param name="op2">A DataSize.</param>
        /// <returns>The DataSize result of multiplying op1 by op2.</returns>
        public static DataSize operator *(DataSize op1, DataSize op2)
        {
            return new DataSize(op1._bValue * op2._bValue);
        }

        /// <summary>
        /// Divides two specified DataSize values.
        /// </summary>
        /// <param name="op1">A DataSize (the dividend).</param>
        /// <param name="op2">A DataSize (the divisor).</param>
        /// <returns>The DataSize result from dividing of op1 by op2.</returns>
        public static DataSize operator /(DataSize op1, DataSize op2)
        {
            return new DataSize(op1._bValue / op2._bValue);
        }

        /// <summary>
        /// Returns the remainder resulting from dividing two specified DataSize values.
        /// </summary>
        /// <param name="op1">A DataSize.</param>
        /// <param name="op2">A DataSize.</param>
        /// <returns>The DataSize remainder resulting from dividing d1 by d2.</returns>
        public static DataSize operator %(DataSize op1, DataSize op2)
        {
            return new DataSize(op1._bValue % op2._bValue);
        }

        /// <summary>
        /// Negates the value of the specified DataSize operand.
        /// </summary>
        /// <param name="op">The DataSize operand.</param>
        /// <returns>The result of op multiplied by negative one (-1).</returns>
        public static DataSize operator -(DataSize op)
        {
            return new DataSize(op._bValue * -1);
        }

        /// <summary>
        /// Returns the value of the DataSize operand (the sign of the operand is unchanged).
        /// </summary>
        /// <param name="op">The DataSize operand.</param>
        /// <returns>The value of the operand, op.</returns>
        public static DataSize operator +(DataSize op)
        {
            return new DataSize(op._bValue);
        }

        /// <summary>
        /// Determines whether two specified instances of DataSize are equal.
        /// </summary>
        /// <param name="b">A DataSize.</param>
        /// <param name="m">A DataSize.</param>
        /// <returns>true if b and m represent the same binary measure value; otherwise, false.</returns>
        public static bool operator ==(DataSize b, DataSize m)
        {
            return b._bValue == m._bValue;
        }

        /// <summary>
        /// Determines whether two specified instances of DataSize are not equal.
        /// </summary>
        /// <param name="b">A DataSize.</param>
        /// <param name="m">A DataSize.</param>
        /// <returns>true if b and m do not represent the same binary measure value; otherwise, false</returns>
        public static bool operator !=(DataSize b, DataSize m)
        {
            return b._bValue != m._bValue;
        }

        /// <summary>
        /// Determines whether one specified DataSize is greater than another specified DataSize.
        /// </summary>
        /// <param name="b">A DataSize.</param>
        /// <param name="m">A DataSize.</param>
        /// <returns>true if b is greater than m; otherwise, false.</returns>
        public static bool operator >(DataSize b, DataSize m)
        {
            return b._bValue > m._bValue;
        }

        /// <summary>
        /// Determines whether one specified DataSize is greater than or equal to another specified DataSize.
        /// </summary>
        /// <param name="b">A DataSize.</param>
        /// <param name="m">A DataSize.</param>
        /// <returns>true if b is greater than or equal to m; otherwise, false.</returns>
        public static bool operator >=(DataSize b, DataSize m)
        {
            return b._bValue >= m._bValue;
        }

        /// <summary>
        /// Determines whether one specified DataSize is less than another specified DataSize.
        /// </summary>
        /// <param name="b">A DataSize.</param>
        /// <param name="m">A DataSize.</param>
        /// <returns>true if b is less than m; otherwise, false.</returns>
        public static bool operator <(DataSize b, DataSize m)
        {
            return b._bValue < m._bValue;
        }

        /// <summary>
        /// Determines whether one specified DataSize is less than or equal to another specified DataSize.
        /// </summary>
        /// <param name="b">A DataSize.</param>
        /// <param name="m">A DataSize.</param>
        /// <returns>true if b is less than or equal to m; otherwise, false.</returns>
        public static bool operator <=(DataSize b, DataSize m)
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
            return ((int)_bValue ^ (int)(_bValue >> 32));
        }

        /// <summary>
        /// Returns a value indicating whether this instance and a specified <see cref="T:System.Object"/>
        /// represent the same type and value.
        /// </summary>
        /// <param name="obj">An object to compare to this instance.</param>
        /// <returns>true if value is a DataSize and equal to this instance; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is DataSize)
                return (DataSize)obj == this;
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
        public decimal GetValue(ByteMeasure mult)
        {
            return (decimal)_bValue / (long)mult;
        }

        /// <summary>
        /// Converts the value of this instance to its equivalent string representation using the specified format.
        /// </summary>
        /// <param name="format">A format string.</param>
        /// <returns>A string representation of value of this instance as specified by <c>format</c>.</returns>
        /// <exception cref="FormatException">format does not contain a valid custom format pattern.</exception>
        /// <example>
        /// The following console application example show how to use DataSize.ToString method.
        /// <code>
        /// using System;
        /// using SklLib;
        /// 
        /// class Program
        /// {
        ///     static void Main()
        ///     {
        ///            DataSize binMea = new DataSize();
        ///            binMea.Megabytes = 15.317M;
        /// 
        ///            Console.WriteLine(binMea.ToString("G|M-"));
        ///                // output: "15.3169994354248046875 MB"
        /// 
        ///            Console.WriteLine(binMea.ToString("G|M+"));
        ///                // output: "15.3169994354248046875 Megabytes"
        /// 
        ///            Console.WriteLine(binMea.ToString("N2|M-"));
        ///                // output: "15.32 MB"
        /// 
        ///            Console.WriteLine(binMea.ToString("G|K-"));
        ///                // output: "15684.607421875 KB"
        /// 
        ///         Console.WriteLine(binMea.ToString("N1|K-"));
        ///                // output: "15,684.6 KB"
        /// 
        ///            binMea.Gigabytes = 4000;
        /// 
        ///            Console.WriteLine(binMea.ToString("G|>-"));
        ///                // output: "3.90625 TB"
        /// 
        ///         Console.WriteLine(binMea.ToString("N0|>-"));
        ///                // output: "4 TB"
        /// 
        ///         Console.WriteLine(binMea.ToString("G|0-"));
        ///                // output: "4000 GB"
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
        /// The following console application example show how to use DataSize.ToString method.
        /// <code>
        /// using System;
        /// using System.Globalization;
        /// using SklLib;
        /// 
        /// class Program
        /// {
        ///     static void Main()
        ///     {
        ///            DataSize binMea = new DataSize();
        ///            binMea.Megabytes = 15.317M;
        /// 
        ///            Console.WriteLine(binMea.ToString("G|M-", NumberFormatInfo.CurrentInfo));
        ///                // output: "15.3169994354248046875 MB"
        /// 
        ///            Console.WriteLine(binMea.ToString("G|M+", NumberFormatInfo.CurrentInfo));
        ///                // output: "15.3169994354248046875 Megabytes"
        /// 
        ///            Console.WriteLine(binMea.ToString("N2|M-", NumberFormatInfo.CurrentInfo));
        ///                // output: "15.32 MB"
        /// 
        ///            Console.WriteLine(binMea.ToString("G|K-", NumberFormatInfo.CurrentInfo));
        ///                // output: "15684.607421875 KB"
        /// 
        ///         Console.WriteLine(binMea.ToString("N1|K-", NumberFormatInfo.CurrentInfo));
        ///                // output: "15,684.6 KB"
        /// 
        ///            binMea.Gigabytes = 4000;
        /// 
        ///            Console.WriteLine(binMea.ToString("G|>-", NumberFormatInfo.CurrentInfo));
        ///                // output: "3.90625 TB"
        /// 
        ///         Console.WriteLine(binMea.ToString("N0|>-", NumberFormatInfo.CurrentInfo));
        ///                // output: "4 TB"
        /// 
        ///         Console.WriteLine(binMea.ToString("G|0-", NumberFormatInfo.CurrentInfo));
        ///                // output: "4000 GB"
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

            ByteMeasure toBMult;        // Verify selected option
            if (pIdx < 7)
                toBMult = GetMultByDefined(primaryChars[pIdx]);
            else if (pIdx == 7)
                toBMult = GetMultByGreat();
            else
                toBMult = GetMultByZeroable();

            value = (decimal)this._bValue / (long)toBMult;    // stores the value in specified multiple

            // Define the multiple spell
            strMult = Enum.GetName(typeof(ByteMeasure), toBMult);
            if (sIdx == 0)
                strMult = strMult[0] == 'B' ? "B" : strMult[0] + "B";
            // -------------------------------------------

            return value.ToString(fmtNumber, nfi) + " " + strMult;
        }

        private ByteMeasure GetMultByDefined(char greatness)
        {
            string multiples = "BKMGTPE";

            int idx = multiples.IndexOf(greatness);

            if (idx == -1)
                return ByteMeasure.Bytes;
            else
            {
                long[] val = (long[])Enum.GetValues(typeof(ByteMeasure));
                return (ByteMeasure)val[idx];
            }
        }

        private ByteMeasure GetMultByGreat()
        {
            long[] values = (long[])Enum.GetValues(typeof(ByteMeasure));
            long bytes = Math.Abs(this._bValue);

            for (int i = values.Length - 1; i >= 0; i--)
            {
                if (bytes >= values[i])
                    return (ByteMeasure)values[i];
            }

            return ByteMeasure.Bytes;
        }

        private ByteMeasure GetMultByZeroable()
        {
            if (this._bValue == 0)
                return ByteMeasure.Bytes;

            long[] values = (long[])Enum.GetValues(typeof(ByteMeasure));
            long bytes = Math.Abs(this._bValue);

            for (int i = values.Length - 1; i > 0; i--)
            {
                decimal val = (decimal)this._bValue / values[i];
                if ((long)val == val)
                    return (ByteMeasure)values[i];
            }

            return GetMultByGreat();
        }

        #endregion

        #region IComparable Members

        /// <summary>
        /// Compares this instance to a specified object and returns an indication of their relative values.
        /// </summary>
        /// <param name="obj">A boxed DataSize object to compare, or null.</param>
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

            if (!(obj is DataSize))
                throw new ArgumentException(resExceptions.Obj_MustBeType.Replace("%var", "DataSize"));

            DataSize other = (DataSize)obj;
            if (this > other)
                return 1;
            if (this < other)
                return -1;
            return 0;
        }

        #endregion

        #region IComparable<DataSize> Members

        /// <summary>
        /// Compares this instance to a specified DataSize object and returns an indication of their relative values.
        /// </summary>
        /// <param name="other">A DataSize object to compare.</param>
        /// <returns>
        /// <para>A signed number indicating the relative values of this instance and the value parameter.</para>
        /// <para>Value Description:</para>
        /// <para>- Less than zero: This instance is less than value.</para>
        /// <para>- Zero: This instance is equal to value.</para>
        /// <para>- Greater than zero: This instance is greater than value.</para></returns>
        public int CompareTo(DataSize other)
        {
            if (this > other)
                return 1;
            if (this < other)
                return -1;
            return 0;
        }

        #endregion

        #region IEquatable<DataSize> Members

        /// <summary>
        /// Returns a value indicating whether this instance is equal to the specified DataSize instance.
        /// </summary>
        /// <param name="other">A DataSize instance to compare to this instance.</param>
        /// <returns>true if the value parameter equals the value of this instance; otherwise, false.</returns>
        public bool Equals(DataSize other)
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
