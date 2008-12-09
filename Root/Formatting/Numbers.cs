// Numbers.cs
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

/// <summary>
/// Namespace where are located formatting classes.
/// </summary>
namespace Root.Formatting
{
    /// <summary>
    /// Provides methods to format numbers.
    /// </summary>
    public static class Numbers
    {
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

        private static string GetStringInSignedStrings(string str, int pos)
        {
            int signIdx = str.IndexOf('&');
            if (signIdx != -1)
            {
                if (pos == 1)
                    str = str.Substring(signIdx + 1);
                else
                    str = str.Substring(0, signIdx);
            }

            return str;
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
                number = number.Replace(numFInfo.CurrencySymbol, "");
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

            string[] intGroups;
            string decPart;

            number = number.Replace(GroupSeparator, "");

            SpellNumber_divide(number, out intGroups, out decPart, DecimalSeparator);

            if (intGroups.Length > numWInfo.ThousandValues.Length)
                throw new OverflowException(resExceptions.UnsupportedNumber_byNumberWriteInfo);

            string fullWrite;
            string intWrited = SpellNumber_int(intGroups, numWInfo);

            string decWrited;
            if (currency)
            {
                long value = (long)Math.Floor(Convert.ToDecimal(number));
                string cur = numWInfo.CurrencyIntegerName;
                if (value > 1)
                    cur = GetStringInSignedStrings(cur, 1);
                else
                    cur = GetStringInSignedStrings(cur, 0);
                intWrited += " " + cur;

                value = Convert.ToInt64(decPart);
                if (value == 0)
                    decWrited = "";
                else
                {
                    decWrited = numWInfo.IntegerDecimalSeparator;
                    decWrited += SpellNumber_int(SpellNumber_divideGroups(decPart), numWInfo);


                    cur = numWInfo.CurrencyDecimalName;
                    if (value > 1)
                        cur = GetStringInSignedStrings(cur, 1);
                    else
                        cur = GetStringInSignedStrings(cur, 0);
                    decWrited += " " + cur;
                }
            }
            else
            {
                decWrited = SpellNumber_dec(decPart, numWInfo);
                if (decWrited.Length != 0)
                    decWrited = numWInfo.IntegerDecimalSeparator + decWrited;
            }

            fullWrite = intWrited + decWrited;
            return fullWrite;
        }

        private static string SpellNumber_base(string part, Globalization.NumberWriteInfo numWInfo)
        {
            for (int i = 0; i < numWInfo.SpecialCases.Length; i++)
            {
                if (Convert.ToInt64(part) == numWInfo.SpecialCases[i].value)
                    return numWInfo.SpecialCases[i].write;
            }

            if (part.Length < 3)
                part = new string('0', 3 - part.Length) + part;

            string wnumber = "";
            byte hvalue = Convert.ToByte(part.Substring(0, 1));
            byte dvalue = Convert.ToByte(part.Substring(1, 1));
            byte uvalue = Convert.ToByte(part.Substring(2, 1));

            string hd = numWInfo.HundredDozenSeparator;
            if (hvalue == 0 || (dvalue == 0 && uvalue == 0))
                hd = "";

            string du = numWInfo.DozenUnitSeparator;
            if (dvalue == 0 || uvalue == 0)
                du = "";

            // hundred
            if ((hvalue * 10) + dvalue < numWInfo.DozenValues.Length && hvalue != 0)
            {
                dvalue += 10;
                goto dozen;
            }
            wnumber += numWInfo.HundredValues[hvalue] + hd;

            // dozen
        dozen:
            for (int i = 0; i < numWInfo.SpecialCases.Length; i++)
            {
                if (numWInfo.SpecialCases[i].value.ToString().Length == 2)
                    if (dvalue * 10 + uvalue == numWInfo.SpecialCases[i].value)
                        return wnumber + numWInfo.SpecialCases[i].write;
            }

            if ((dvalue * 10) + uvalue < numWInfo.UnityValues.Length && dvalue != 0)
            {
                uvalue += 10;
                goto unity;
            }
            wnumber += numWInfo.DozenValues[dvalue] + du;

            // unity
        unity:
            for (int i = 0; i < numWInfo.SpecialCases.Length; i++)
            {
                if (numWInfo.SpecialCases[i].value.ToString().Length == 1)
                    if (uvalue == numWInfo.SpecialCases[i].value)
                        return wnumber + numWInfo.SpecialCases[i].write;
            }
            if (uvalue != 0)
                wnumber += numWInfo.UnityValues[uvalue];

            return wnumber;
        }

        private static string SpellNumber_dec(string part, Globalization.NumberWriteInfo numWInfo)
        {
            for (int i = part.Length - 1; i >= 0; i--)
            {
                if (part.Substring(part.Length - 1) == "0")
                    part = part.Substring(0, part.Length - 1);
                else
                    break;
            }

            int len = part.Length;
            if (len == 0)
                return "";

            long partValue = Convert.ToInt64(part);
            if (partValue == 0)
                return "";

            string numb = SpellNumber_int(SpellNumber_divideGroups(part), numWInfo);
            if (len >= numWInfo.DecimalValues.Length)
                throw new OverflowException(resExceptions.UnsupportedNumber_byNumberWriteInfo);

            string decValue = numWInfo.DecimalValues[len];
            if (partValue > 1)
                decValue = GetStringInSignedStrings(decValue, 1);
            else
                decValue = GetStringInSignedStrings(decValue, 0);

            numb += " " + decValue;
            return numb;
        }

        private static void SpellNumber_divide(string number, out string[] intPart, out string decPart, string decSeparator)
        {
            decPart = "";
            string[] intDec = number.Split(new string[] { decSeparator },
                StringSplitOptions.RemoveEmptyEntries);
            if (intDec.Length == 2)
                decPart = intDec[1];

            intPart = SpellNumber_divideGroups(intDec[0]);
        }

        private static string[] SpellNumber_divideGroups(string integer)
        {
            integer = Convert.ToInt64(integer).ToString();
            string[] intPart = new string[(int)Math.Ceiling(integer.Length / 3M)];
            for (int i = 0; i < intPart.Length; i++)
            {
                int len = integer.Length;
                if (len <= 3)
                {
                    intPart[i] = integer;
                    break;
                }
                intPart[i] = integer.Substring(len - 3);
                integer = integer.Substring(0, len - 3);
            }
            return intPart;
        }

        private static string SpellNumber_int(string[] intGroups, Globalization.NumberWriteInfo numWInfo)
        {
            string intWrite = "";

            if (intGroups.Length == 1)
                if (Convert.ToInt64(intGroups[0]) == 0)
                    return numWInfo.UnityValues[0];

            for (int i = intGroups.Length - 1; i >= 0; i--)
            {
                intWrite += SpellNumber_base(intGroups[i], numWInfo);

                string thVal = numWInfo.ThousandValues[i];

                int currValue = Convert.ToInt32(intGroups[i]);
                if (currValue > 1)
                    thVal = GetStringInSignedStrings(thVal, 1);
                else
                    thVal = GetStringInSignedStrings(thVal, 0);

                // verifica se nas próximas casas é "00...00" ou não.
                string bValues = "";
                for (int v = i - 1; v >= 0; v--)
                    bValues += intGroups[v];

                string th_Sep = "";
                if (bValues.Length > 0)
                    if (Convert.ToInt64(bValues) > 0)
                        th_Sep = numWInfo.ThousandsSeparator;
                // -------------------------------------------------

                if (currValue > 0)
                    intWrite += " " + thVal + th_Sep;
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
