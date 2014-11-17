// Numbers.cs
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
using System.Globalization;
using System.Linq;

// Namespace where are located formatting classes.
namespace SklLib.Formatting
{
    /// <summary>
    /// Provides methods to format numbers.
    /// </summary>
    public static class Numbers
    {
        // All numeric characters who are not zero.
        private static readonly char[] NON_ZERO_CHARS = "123456789".ToCharArray();

        /// <summary>
        /// Writes a number in a spelling mode (example: "fifty-two").
        /// </summary>
        /// <param name="number">A number.</param>
        /// <returns>A String that represents a spelt number.</returns>
        public static string SpellNumber(decimal number)
        {
            return SpellNumber(number, false);
        }

        /// <summary>
        /// Writes a number in a spelling mode (example: "fifty-two").
        /// </summary>
        /// <param name="number">A number.</param>
        /// <param name="currency">Indicates whether is a currency number.</param>
        /// <returns>A String that represents a spelt number.</returns>
        public static string SpellNumber(decimal number, bool currency)
        {
            Globalization.NumberWriteInfo numWInfo = Globalization.NumberWriteInfo.CurrentInfo;
            NumberFormatInfo numFInfo = NumberFormatInfo.CurrentInfo;
            return SpellNumber(number, currency, numWInfo, numFInfo);
        }

        /// <summary>
        /// Writes a number in a spelling mode (example: "fifty-two").
        /// </summary>
        /// <param name="number">A number.</param>
        /// <param name="currency">Indicates whether is a currency number.</param>
        /// <param name="numWInfo">An NumberWriteInfo that supplies culture-specific writting
        /// information.</param>
        /// <param name="numFInfo">An NumberFormatInfo that supplies culture-specific formatting
        /// information.</param>
        /// <returns>A String that represents a spelt number.</returns>
        /// <exception cref="ArgumentNullException"><c>numWInfo</c> or <c>numFInfo</c> is a null reference.</exception>
        public static string SpellNumber(decimal number, bool currency, Globalization.NumberWriteInfo numWInfo, NumberFormatInfo numFInfo)
        {
            if (numWInfo == null)
                throw new ArgumentNullException(resExceptions.ArgumentNull.Replace("%var", "numWInfo"));
            if (numFInfo == null)
                throw new ArgumentNullException(resExceptions.ArgumentNull.Replace("%var", "numFInfo"));

            string sNumber;
            if (currency)
                sNumber = number.ToString("c", numFInfo);
            else
                sNumber = number.ToString(numFInfo);
            return SpellNumber(sNumber, currency, numWInfo, numFInfo);
        }

        private static string SpellNumber(string number, bool currency, Globalization.NumberWriteInfo numWInfo, NumberFormatInfo numFInfo)
        {
            string GroupSeparator;
            string DecimalSeparator;
            int[] GroupSizes;

            if (currency)
            {
                GroupSeparator = numFInfo.CurrencyGroupSeparator;
                DecimalSeparator = numFInfo.CurrencyDecimalSeparator;
                GroupSizes = numFInfo.CurrencyGroupSizes;
                number = number.Replace(numFInfo.CurrencySymbol, string.Empty);
                number = number.Trim();
            }
            else
            {
                GroupSeparator = numFInfo.NumberGroupSeparator;
                DecimalSeparator = numFInfo.NumberDecimalSeparator;
                GroupSizes = numFInfo.NumberGroupSizes;
            }

            if (GroupSizes.Length != 1)
                throw new ArgumentException(resExceptions.Unsupported_GroupSize, "numFInfo");
            if (GroupSizes[0] != 3)
                throw new ArgumentException(resExceptions.Unsupported_GroupSize, "numFInfo");

            decimal valNumber = Convert.ToDecimal(number);
            string[] intGroups;
            string decPart;

            number = number.Replace(GroupSeparator, string.Empty);

            SpellNumber_divide(number, out intGroups, out decPart, DecimalSeparator);

            if (intGroups.Length > numWInfo.ThousandValues.Length)
                throw new OverflowException(resExceptions.UnsupportedNumber_byNumberWriteInfo);

            string fullWrite;
            string intWrited = string.Empty;
            if (!(Math.Floor(valNumber) == 0 && valNumber > 0))
                intWrited = SpellNumber_int(intGroups, numWInfo);

            string decWrited;
            if (currency)
            {
                long value = (long)Math.Floor(valNumber);
                string cur = (value > 1 ?
                    numWInfo.CurrencyIntegerName.Plural :
                    numWInfo.CurrencyIntegerName.Singular
                    );
                intWrited += " " + cur;

                value = Convert.ToInt64(decPart);
                if (value == 0)
                    decWrited = "";
                else
                {
                    decWrited = numWInfo.IntegerDecimalSeparator;
                    decWrited += SpellNumber_int(SpellNumber_divideGroups(decPart), numWInfo);


                    cur = (value > 1 ?
                        numWInfo.CurrencyDecimalName.Plural :
                        numWInfo.CurrencyDecimalName.Singular
                        );
                    decWrited += " " + cur;
                }
            }
            else
            {
                decWrited = SpellNumber_dec(decPart, numWInfo);
                if (decWrited.Length > 0
                    && intWrited.Length > 0) {
                    decWrited = numWInfo.IntegerDecimalSeparator + decWrited;
                }
            }

            fullWrite = intWrited + decWrited;
            return fullWrite;
        }

        // Process hundred chunk from number (e.g: '45' and '433' from '45,433.77')
        private static string SpellNumber_base(string part, Globalization.NumberWriteInfo numWInfo)
        {
            string special = (
                from a in numWInfo.SpecialCases
                let valPart = Convert.ToInt64(part)
                where valPart == a.Value
                select a.Spell)
                .FirstOrDefault();
            if (special != null)
                return special;

            if (part.Length < 3)
                part = part.PadLeft(3, '0');

            string wnumber = string.Empty;
            byte hvalue = (byte)(part[0] - '0');
            byte dvalue = (byte)(part[1] - '0');
            byte uvalue = (byte)(part[2] - '0');

            if (hvalue == 0 && dvalue == 0 && uvalue == 0)
                return string.Empty;

            // Spelling to hundred, dozen and unit
            string wHValue = null, wDValue = null, wUValue = null;

            // === hundred ===
            // (e.g: for '123' if has 12th element in DozenValues)
            if (hvalue > 0 && dvalue > 0)
            wDValue = numWInfo.DozenValues
                .ElementAtOrDefault(hvalue * 10 + dvalue);

            if (wDValue == null)
                wHValue = numWInfo.HundredValues[hvalue];

            // === dozen ===
            // (e.g: for '123' if has the value 23 in SpecialCases)
            wUValue = (
                from a in numWInfo.SpecialCases
                let valDU = dvalue * 10 + uvalue
                where valDU == a.Value
                select a.Spell)
                .FirstOrDefault();
            // (e.g: for '123' if has 23th element in UnityValues)
            if (dvalue > 0 && uvalue > 0) {
                wUValue = numWInfo.UnityValues
                    .ElementAtOrDefault(dvalue * 10 + uvalue);
            }

            if (wUValue == null)
                wDValue = numWInfo.DozenValues[dvalue];

            // === unity ===
            if (uvalue != 0 && wUValue == null)
                wUValue = numWInfo.UnityValues[uvalue];

            // === Joining all together ===
            string hd = numWInfo.HundredDozenSeparator;
            if (hvalue == 0 || (dvalue == 0 && uvalue == 0))
                hd = string.Empty;

            string du = numWInfo.DozenUnitSeparator;
            if (dvalue == 0 || uvalue == 0)
                du = string.Empty;

            if (wHValue != null)
                wnumber += wHValue + hd;
            if (wDValue != null)
                wnumber += wDValue + du;
            if (wUValue != null)
                wnumber += wUValue;

            return wnumber;
        }

        // Process decimal part from number (e.g: '768' from '7,843.768')
        private static string SpellNumber_dec(string part, Globalization.NumberWriteInfo numWInfo)
        {
            part = part.TrimEnd('0');

            int len = part.Length;
            if (len == 0)
                return string.Empty;
            if (len >= numWInfo.DecimalValues.Length)
                throw new OverflowException(resExceptions.UnsupportedNumber_byNumberWriteInfo);

            ulong partValue = Convert.ToUInt64(part);
            if (partValue == 0)
                return string.Empty;

            string numb = SpellNumber_int(SpellNumber_divideGroups(part), numWInfo);

            numb += " " + (partValue > 1 ?
                numWInfo.DecimalValues[len].Plural :
                numWInfo.DecimalValues[len].Singular
                );

            return numb;
        }

        // Splits a number in parts (e.g: ('75', '433') and '93' from '75,433.93')
        private static void SpellNumber_divide(string number, out string[] intPart, out string decPart, string decSeparator)
        {
            decPart = string.Empty;
            string[] intDec = number.Split(new string[] { decSeparator },
                StringSplitOptions.RemoveEmptyEntries);
            if (intDec.Length == 2)
                decPart = intDec[1];

            intPart = SpellNumber_divideGroups(intDec[0]);
        }

        // Splits the integer part of a number (e.g: ('3', '943') from '3943')
        private static string[] SpellNumber_divideGroups(string integer)
        {
            integer = Convert.ToInt64(integer).ToString();
            return integer.SplitReverse(3);
        }

        // Gets spelling of integer number
        private static string SpellNumber_int(string[] intGroups, Globalization.NumberWriteInfo numWInfo)
        {
            string intWrite = string.Empty;

            if (intGroups.Length == 1) {
                if (Convert.ToInt64(intGroups[0]) == 0)
                    return numWInfo.UnityValues[0];
            }

            if (intGroups.Length > numWInfo.ThousandValues.Length)
                throw new OverflowException(resExceptions.UnsupportedNumber_byNumberWriteInfo);

            var va = intGroups
                .Select((a, i) => {
                    int aValue = Convert.ToInt32(a);

                    // Is next groups zeroes?
                    bool isAllNextZero = intGroups
                        .Take(i)
                        .Where(s => s.IndexOfAny(NON_ZERO_CHARS) != -1)
                        .Count() == 0;

                    return new {
                        NumberValue = aValue,
                        NumberWrite = SpellNumber_base(a, numWInfo),
                        ThousandValue = (aValue > 1 ?
                        numWInfo.ThousandValues[i].Plural :
                        numWInfo.ThousandValues[i].Singular),
                        IsAllNextZero = isAllNextZero
                    };
                })
                .Reverse();

            foreach (var item in va) {
                if (item.NumberValue == 0)
                    continue;

                intWrite += item.NumberWrite + " " + item.ThousandValue;
                if (!item.IsAllNextZero)
                    intWrite += numWInfo.ThousandsSeparator;
            }

            return intWrite.Trim();
        }


        /// <summary>
        /// Converts the indicated numeric value to its equivalent string representation and
        /// keeps the number of indicated characters adding zero characters.
        /// </summary>
        /// <param name="number">Numeric value.</param>
        /// <param name="count">Number of characters to keeps.</param>
        /// <returns>The string representation of the value of this instance, with length equal or
        /// greater than indicated value.</returns>
        public static string ToStringKeepLength(short number, int count)
        {
            string result = number.ToString();
            int len = count - result.Length;

            if (len <= 0)
                return result;

            return new string('0', len) + result;
        }

        /// <summary>
        /// Converts the indicated numeric value to its equivalent string representation and
        /// keeps the number of indicated characters adding zero characters.
        /// </summary>
        /// <param name="number">Numeric value.</param>
        /// <param name="count">Number of characters to keeps.</param>
        /// <returns>The string representation of the value of this instance, with length equal or
        /// greater than indicated value.</returns>
        public static string ToStringKeepLength(short number, short count)
        {
            string result = number.ToString();
            int len = count - result.Length;

            if (len <= 0)
                return result;

            return new string('0', len) + result;
        }

        /// <summary>
        /// Converts the indicated numeric value to its equivalent string representation and
        /// keeps the number of indicated characters adding zero characters.
        /// </summary>
        /// <param name="number">Numeric value.</param>
        /// <param name="count">Number of characters to keeps.</param>
        /// <returns>The string representation of the value of this instance, with length equal or
        /// greater than indicated value.</returns>
        public static string ToStringKeepLength(int number, int count)
        {
            string result = number.ToString();
            int len = count - result.Length;

            if (len <= 0)
                return result;

            return new string('0', len) + result;
        }

        /// <summary>
        /// Converts the indicated numeric value to its equivalent string representation and
        /// keeps the number of indicated characters adding zero characters.
        /// </summary>
        /// <param name="number">Numeric value.</param>
        /// <param name="count">Number of characters to keeps.</param>
        /// <returns>The string representation of the value of this instance, with length equal or
        /// greater than indicated value.</returns>
        public static string ToStringKeepLength(int number, short count)
        {
            string result = number.ToString();
            int len = count - result.Length;

            if (len <= 0)
                return result;

            return new string('0', len) + result;
        }

        /// <summary>
        /// Converts the indicated numeric value to its equivalent string representation and
        /// keeps the number of indicated characters adding zero characters.
        /// </summary>
        /// <param name="number">Numeric value.</param>
        /// <param name="count">Number of characters to keeps.</param>
        /// <returns>The string representation of the value of this instance, with length equal or
        /// greater than indicated value.</returns>
        public static string ToStringKeepLength(long number, int count)
        {
            string result = number.ToString();
            int len = count - result.Length;

            if (len <= 0)
                return result;

            return new string('0', len) + result;
        }

        /// <summary>
        /// Converts the indicated numeric value to its equivalent string representation and
        /// keeps the number of indicated characters adding zero characters.
        /// </summary>
        /// <param name="number">Numeric value.</param>
        /// <param name="count">Number of characters to keeps.</param>
        /// <returns>The string representation of the value of this instance, with length equal or
        /// greater than indicated value.</returns>
        public static string ToStringKeepLength(long number, short count)
        {
            string result = number.ToString();
            int len = count - result.Length;

            if (len <= 0)
                return result;

            return new string('0', len) + result;
        }

        /// <summary>
        /// Converts the numeric value of this instance to its equivalent String representation,
        /// using the specified format and remove excedent decimal.
        /// </summary>
        /// <param name="number">A number.</param>
        /// <param name="format">A format specification.</param>
        /// <returns></returns>
        public static string ToStringTrimExcess(float number, string format)
        {
            string strNumber = number.ToString(format);
            return ToStringTrimExcess(strNumber, NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Converts the numeric value of this instance to its equivalent String representation
        /// using the specified format and culture-specific format information and remove excedent
        /// decimal.
        /// </summary>
        /// <param name="number">A number.</param>
        /// <param name="format">A format specification.</param>
        /// <param name="numInfo">An NumberFormatInfo that supplies culture-specific formatting
        /// information.</param>
        /// <returns>The String representation of the value of this instance as specified by format and
        /// provider and cutted excedent decimal.</returns>
        public static string ToStringTrimExcess(float number, string format, NumberFormatInfo numInfo)
        {
            string strNumber = number.ToString(format, numInfo);
            return ToStringTrimExcess(strNumber, numInfo);
        }

        /// <summary>
        /// Converts the numeric value of this instance to its equivalent String representation,
        /// using the specified format and remove excedent decimal.
        /// </summary>
        /// <param name="number">A number.</param>
        /// <param name="format">A format specification.</param>
        /// <returns></returns>
        public static string ToStringTrimExcess(double number, string format)
        {
            string strNumber = number.ToString(format);
            return ToStringTrimExcess(strNumber, NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Converts the numeric value of this instance to its equivalent String representation
        /// using the specified format and culture-specific format information and remove excedent
        /// decimal.
        /// </summary>
        /// <param name="number">A number.</param>
        /// <param name="format">A format specification.</param>
        /// <param name="numInfo">An NumberFormatInfo that supplies culture-specific formatting
        /// information.</param>
        /// <returns>The String representation of the value of this instance as specified by format and
        /// provider and cutted excedent decimal.</returns>
        public static string ToStringTrimExcess(double number, string format, NumberFormatInfo numInfo)
        {
            string strNumber = number.ToString(format, numInfo);
            return ToStringTrimExcess(strNumber, numInfo);
        }

        /// <summary>
        /// Converts the numeric value of this instance to its equivalent String representation,
        /// using the specified format and remove excedent decimal.
        /// </summary>
        /// <param name="number">A number.</param>
        /// <param name="format">A format specification.</param>
        /// <returns></returns>
        public static string ToStringTrimExcess(decimal number, string format)
        {
            string strNumber = number.ToString(format);
            return ToStringTrimExcess(strNumber, NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Converts the numeric value of this instance to its equivalent String representation
        /// using the specified format and culture-specific format information and remove excedent
        /// decimal.
        /// </summary>
        /// <param name="number">A number.</param>
        /// <param name="format">A format specification.</param>
        /// <param name="numInfo">An NumberFormatInfo that supplies culture-specific formatting
        /// information.</param>
        /// <returns>The String representation of the value of this instance as specified by format and
        /// provider and cutted excedent decimal.</returns>
        public static string ToStringTrimExcess(decimal number, string format, NumberFormatInfo numInfo)
        {
            string strNumber = number.ToString(format, numInfo);
            return ToStringTrimExcess(strNumber, numInfo);
        }

        private static string ToStringTrimExcess(string number, NumberFormatInfo numInfo)
        {
            for (int i = number.Length - 1; i > 0; i--)
            {
                if (!char.IsNumber(number[i]))
                {
                    number = number.Remove(i);
                    break;
                }
                if (number[i] == numInfo.NativeDigits[0][0])
                    number = number.Remove(i);
                else
                    break;
            }
            return number;
        }
    }
    
}
