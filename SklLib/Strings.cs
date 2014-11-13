// Strings.cs
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
using stringb = System.Text.StringBuilder;

namespace SklLib
{
    /// <summary>
    /// Provides methods for manipulating, searching and validating <see cref="String"/>.
    /// </summary>
    /// <remarks>
    /// <para>The Strings class is the additional class to assist in the following operations with <see cref="String"/>:</para>
    /// <para>- Manipulating, <see cref="EliminateAccents"/> and <see cref="RepeatString"/> methods;</para>
    /// <para>- Searching, <see cref="AdditionalChars"/> method;</para>
    /// <para>- Validating, <see cref="IsAlphabetic"/>, <see cref="IsLetterOnly"/> and <see cref="IsNumeric(string)"/> methods.</para>
    /// <para>Also provides the <see cref="IndexedChar"/> struct, that represents <see cref="Char"/> of a
    /// <see cref="String"/> and a index (<see cref="Char"/> position in a <see cref="String"/>).</para>
    /// </remarks>
    public static class Strings
    {
        /// <summary>
        /// Reports a Array with indexes of the all occurrences of the specified Unicode character
        /// in a string.
        /// </summary>
        /// <param name="str">String where will be made the search.</param>
        /// <param name="value">A Unicode character to seek.</param>
        /// <returns>A Array with indexes positions of value if that character is found, or empty array
        /// if it is not.</returns>
        /// <exception cref="ArgumentNullException"><c>str</c> is a null reference.</exception>
        public static int[] AllIndexOf(this string str, char value)
        {
            int[] result = new int[CountOf(str, value)];

            int idx = 0, count = 0;

        loop:
            idx = str.IndexOf(value, idx);
            if (idx != -1)
            {
                result[count] = idx;
                count++;
                idx++;
                goto loop;
            }

            if (result.Length != count)
                return new int[0];
            return result;
        }

        /// <summary>
        /// Reports a Array with indexes of the all occurrences of the specified string in
        /// a string.
        /// </summary>
        /// <param name="str">String where will be made the search.</param>
        /// <param name="value">The string to seek.</param>
        /// <returns>A Array with indexes positions of value if that string is found, or empty array
        /// if it is not.</returns>
        /// <exception cref="ArgumentNullException"><c>str</c> or <c>value</c> is a null reference.</exception>
        public static int[] AllIndexOf(this string str, string value)
        {
            int[] result = new int[CountOf(str, value)];

            int idx = 0, count = 0;

        loop:
            idx = str.IndexOf(value, idx);
            if (idx != -1)
            {
                result[count] = idx;
                count++;
                idx++;
                goto loop;
            }

            if (result.Length != count)
                return new int[0];
            return result;
        }

        /// <summary>
        /// Seeks and stores all of additional characters found in a <see cref="String"/>,
        /// that were not found in another <see cref="String"/>.
        /// </summary>
        /// <param name="strA">The first <see cref="String"/>.</param>
        /// <param name="strB">The second <see cref="String"/>.</param>
        /// <returns>A <see cref="Array"/> of <see cref="Strings.IndexedChar"/></returns>
        /// <remarks>
        /// This method seeks which characters that a <see cref="String"/> has, and another
        /// <see cref="String"/> doesn't have.
        /// </remarks>
        /// <example>
        /// The following console application example show how to use AdditionalChars method.
        /// <code>
        /// using System;
        /// using SklLib;
        /// 
        /// class Program
        /// {
        ///     static void Main()
        ///     {
        ///         Strings.IndexedChar[] indexChr = "123#example*".AdditionalChars("123example");
        ///         for (int i = 0; i &lt; indexChr.Length; i++)
        ///             Console.WriteLine("{0}) Char = '{1}' at Index = {2}",
        ///                 i + 1, indexChr[i].character, indexChr[i].index);
        ///         Console.ReadKey();
        ///     }
        /// }
        /// </code>
        /// 
        /// <br></br>This example returns the following output.
        /// <code>
        /// 1) Char = '#' at Index = 3
        /// 2) Char = '*' at Index = 11
        /// </code>
        /// <para>Look as knowing index of each character:</para>
        /// <pre>
        /// 00|01|02|03|04|05|06|07|08|09|10|11<br/>
        /// 1 |2 |3 |# |e |x |a |m |p |l |e |* <br/>
        /// 1 |2 |3 |e |x |a |m |p |l |e |* <br/>
        /// 1 |2 |3 |e |x |a |m |p |l |e </pre>
        /// <br></br>
        /// <para>The first character is '#', because in another <see cref="String"/>
        /// doesn't have '#' at index 3.</para>
        /// <para>After, the character at index 3 of current String will removed (123#example* => 123example*).</para>
        /// <para>Next character not found, in another <see cref="String"/>, is '*' at index 11.</para>
        /// </example>
        /// <exception cref="ArgumentException"><c>strB</c> length is major that <c>strA</c> length.</exception>
        /// <exception cref="ArgumentNullException"><c>strA</c> or <c>strB</c> is null reference.</exception>
        public static IndexedChar[] AdditionalChars(this string strA, string strB)
        {
            if (strA == null)
                throw new ArgumentNullException(resExceptions.ArgumentNull.Replace("%var", "strA"));
            if (strB == null)
                throw new ArgumentNullException(resExceptions.ArgumentNull.Replace("%var", "strB"));
            if (strA.Length < strB.Length)
                throw new ArgumentException(resExceptions.StringMustMajorString.Replace("%var1", "strA").Replace("%var2", "strB"));
                
            IndexedChar[] indChr = new IndexedChar[strA.Length];
            int count = 0, i = 0;
            while (i < strA.Length)
            {
                if (i >= strB.Length)
                {
                    indChr[count] = new IndexedChar(strA[i], i + count);
                    strA = strA.Remove(i, 1);
                    count++;
                }
                else if (strA[i] != strB[i])
                {
                    indChr[count] = new IndexedChar(strA[i], i + count);
                    strA = strA.Remove(i, 1);
                    count++;
                }
                else
                    i++;
            }
            IndexedChar[] indChr2 = new IndexedChar[count];
            Array.Copy(indChr, 0, indChr2, 0, count);
            return indChr2;
        }

        /// <summary>
        /// Counts in a string how many occurrences is found of number character.
        /// </summary>
        /// <param name="str">String where will be made the search.</param>
        /// <returns>Result of number of found occurrences.</returns>
        /// <exception cref="ArgumentNullException"><c>str</c> is a null reference.</exception>
        public static int CountNumericChars(this string str)
        {
            if (str == null)
                throw new ArgumentNullException(resExceptions.ArgumentNull.Replace("%var", "str"));

            if (str.Length == 0)
                return 0;

            int count = 0;
            foreach (char c in str)
                if (char.IsNumber(c))
                    count++;
            return count;
        }

        /// <summary>
        /// Counts in a string how many occurrences is found of the specified Unicode character.
        /// </summary>
        /// <param name="str">String where will be made the search.</param>
        /// <param name="value">A Unicode character to count.</param>
        /// <returns>Result of number of found occurrences.</returns>
        /// <exception cref="ArgumentNullException"><c>str</c> is a null reference.</exception>
        public static int CountOf(this string str, char value)
        {
            if (str == null)
                throw new ArgumentNullException(resExceptions.ArgumentNull.Replace("%var", "str"));

            if (str.Length == 0)
                return 0;

            int count = 0;
            foreach (char c in str)
                if (c == value)
                    count++;
            return count;
        }

        /// <summary>
        /// Counts in a string how many occurrence is found of the specified string.
        /// </summary>
        /// <param name="str">String where will be made the search.</param>
        /// <param name="value">The string to count.</param>
        /// <returns>Result of number of found occurrences.</returns>
        /// <exception cref="ArgumentNullException"><c>str</c> or <c>value</c> is a null reference.</exception>
        public static int CountOf(this string str, string value)
        {
            if (str == null)
                throw new ArgumentNullException(resExceptions.ArgumentNull.Replace("%var", "str"));
            if (value == null)
                throw new ArgumentNullException(resExceptions.ArgumentNull.Replace("%var", "value"));

            if (value.Length > str.Length || value.Length == 0)
                return 0;

            int count = 0;
            int idx = 0;
            int len = str.Length;

            loop:
                idx = str.IndexOf(value, idx);
                if (idx != -1)
                {
                    count++;
                    idx++;
                    goto loop;
                }

            return count;
        }

        /// <summary>
        /// Counts in a string how many occurrences is found of each character in a specified array of Unicode characters.
        /// </summary>
        /// <param name="str">String where will be made the search.</param>
        /// <param name="allOf">A Unicode character array containing one or more characters to count.</param>
        /// <returns>Result of number of found occurrences.</returns>
        /// <exception cref="ArgumentNullException"><c>str</c> or <c>allOf</c> is a null reference.</exception>
        public static int CountOfAll(this string str, char[] allOf)
        {
            if (str == null)
                throw new ArgumentNullException(resExceptions.ArgumentNull.Replace("%var", "str"));
            if (allOf == null)
                throw new ArgumentNullException(resExceptions.ArgumentNull.Replace("%var", "allOf"));

            int len = allOf.Length;

            if (len == 0 || str.Length == 0)
                return 0;
            
            int count = 0;
            for (int i = 0; i < len; i++)
                count += CountOf(str, allOf[i]);
            return count;
        }

        /// <summary>
        /// Counts in a string how many occurrences is found of each string in a specified array of string.
        /// </summary>
        /// <param name="str">String where will be made the search.</param>
        /// <param name="allOf">The string array containing one or more strings to count.</param>
        /// <returns>Result of number of found occurrences.</returns>
        /// <exception cref="ArgumentNullException"><c>str</c> or <c>allOf</c> is a null reference.</exception>
        /// <exception cref="ArgumentException"><c>str</c> length is major that any <c>allOf element</c> length.</exception>
        public static int CountOfAll(this string str, string[] allOf)
        {
            if (str == null)
                throw new ArgumentNullException(resExceptions.ArgumentNull.Replace("%var", "str"));
            if (allOf == null)
                throw new ArgumentNullException(resExceptions.ArgumentNull.Replace("%var", "allOf"));

            int len = allOf.Length;
            int count = 0;

            for (int i = 0; i < len; i++)
                count += CountOf(str, allOf[i]);
            return count;
        }

        /// <summary>
        /// Eliminates accents of a <see cref="String"/>.
        /// </summary>
        /// <param name="s">A <see cref="String"/>.</param>
        /// <returns>The <c>s</c> <see cref="String"/> without accents.</returns>
        /// <remarks>
        /// This method eliminates accents of accented <see cref="String"/>, but this
        /// method is valid only to Latin-1 (ISO-8859-1) characters.
        /// </remarks>
        /// <example>
        /// The following console application example show how to use EliminateAccents method.
        /// <code>
        /// using System;
        /// using SklLib;
        ///
        /// class Program
        /// {
        ///        static void Main()
        ///        {
        ///            string text1 = "Você, faça estes testes.";
        ///            string text2 = "¿Hará usted estas pruebas?";
        ///            Console.WriteLine("First example: {0}", text1.EliminateAccents());
        ///            Console.WriteLine("Second example: {0}", text2.EliminateAccents());
        ///            Console.ReadKey();
        ///        }
        /// }
        /// </code>
        /// <br></br>This example returns the following output.
        /// <code>
        /// First example: Voce, faca estes testes.
        /// Second example: ¿Hara usted estas pruebas?
        /// </code>
        /// </example>
        public static string EliminateAccents(this string s)
        {
            if (null == s)
                return null;
            byte[] bytes = System.Text.UnicodeEncoding.Unicode.GetBytes(s);

            for (int idx = 0; idx < bytes.Length; idx += 2)
            {
                if (bytes[idx] > 0xBF && bytes[idx] < 0xFD && bytes[idx + 1] == 0x0)
                    ChangeByte(ref bytes[idx]);
            }

            return System.Text.UnicodeEncoding.Unicode.GetString(bytes);
        }

        /// <summary>
        /// Check a string, looking for control character (code less than 32).
        /// </summary>
        /// <param name="str">A string.</param>
        /// <returns>true if <c>str</c> has any control character; otherwise, false.</returns>
        public static bool HasControlChar(this string str)
        {
            if (str == null)
                throw new ArgumentNullException(resExceptions.ArgumentNull.Replace("%var", "s"));

            for (int i = 0; i < str.Length; i++)
            {
                if ((int)str[i] < 32)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Indicates whether a <see cref="String"/> contains only letters and spaces.
        /// </summary>
        /// <param name="s">A <see cref="String"/>.</param>
        /// <returns>true if <c>s</c> is an letter- and space-only String; otherwise, false.</returns>
        /// <remarks>
        /// This method verify whether a <see cref="String"/> contains only letters and spaces.
        /// </remarks>
        /// <example>
        /// The following console application example show how to use IsAlphabetic method.
        /// <code>
        /// using System;
        /// using SklLib;
        ///
        /// class Program
        /// {
        ///        static void Main()
        ///        {
        ///            string text1 = "Make these tests";
        ///            string text2 = "testing?";
        ///            string text3 = "text3";
        ///            string text4 = "example";
        ///            Console.WriteLine("First example: {0}", text1.IsAlphabetic());
        ///            Console.WriteLine("Second example: {0}", text2.IsAlphabetic());
        ///            Console.WriteLine("Third example: {0}", text3.IsAlphabetic());
        ///            Console.WriteLine("Fourth example: {0}", text4.IsAlphabetic());
        ///            Console.ReadKey();
        ///        }
        /// }
        /// </code>
        /// <br></br>This example returns the following output.
        /// <code>
        /// First example: True
        /// Second example: False
        /// Third example: False
        /// Fourth example: True
        /// </code>
        /// </example>
        /// <exception cref="ArgumentNullException"><c>s</c> is a null reference.</exception>
        public static bool IsAlphabetic(this string s)
        {
            if (s == null)
                throw new ArgumentNullException(resExceptions.ArgumentNull.Replace("%var", "s"));
            return IsLetterOnly(s.Replace(" ", ""));
        }

        /// <summary>
        /// Indicates whether a <see cref="String"/> contains only letters, spaces and numeric chars.
        /// </summary>
        /// <param name="s">A <see cref="String"/>.</param>
        /// <returns>true if <c>s</c> is an letter- and space-only and/or numeric String; otherwise, false.</returns>
        public static bool IsAlphabeticAndNumeric(this string s)
        {
            return IsAlphabeticAndNumeric(s, NumberFormatInfo.CurrentInfo, false);
        }

        /// <summary>
        /// Indicates whether a <see cref="String"/> contains only letters, spaces and numeric chars.
        /// </summary>
        /// <param name="s">A <see cref="String"/>.</param>
        /// <param name="numInfo">Culture specific <see cref="NumberFormatInfo"/>.</param>
        /// <returns>true if <c>s</c> is an letter- and space-only and/or numeric String; otherwise, false.</returns>
        public static bool IsAlphabeticAndNumeric(this string s, NumberFormatInfo numInfo)
        {
            return IsAlphabeticAndNumeric(s, numInfo, false);
        }

        /// <summary>
        /// Indicates whether a <see cref="String"/> contains only letters, spaces and numeric chars.
        /// </summary>
        /// <param name="s">A <see cref="String"/>.</param>
        /// <param name="decimalNumber">Indicates whether the number can be decimal.</param>
        /// <returns>true if <c>s</c> is an letter- and space-only and/or numeric String; otherwise, false.</returns>
        public static bool IsAlphabeticAndNumeric(this string s, bool decimalNumber)
        {
            return IsAlphabeticAndNumeric(s, NumberFormatInfo.CurrentInfo, decimalNumber);
        }

        /// <summary>
        /// Indicates whether a <see cref="String"/> contains only letters, spaces and numeric chars.
        /// </summary>
        /// <param name="s">A <see cref="String"/>.</param>
        /// <param name="numInfo">Culture specific <see cref="NumberFormatInfo"/>.</param>
        /// <param name="decimalNumber">Indicates whether the number can be decimal.</param>
        /// <returns>true if <c>s</c> is an letter- and space-only and/or numeric String; otherwise, false.</returns>
        public static bool IsAlphabeticAndNumeric(this string s, NumberFormatInfo numInfo, bool decimalNumber)
        {
            if (s == null)
                throw new ArgumentNullException(resExceptions.ArgumentNull.Replace("%var", "s"));

            s = s.Replace(" ", "");
            stringb str = new stringb(s.Length); 
            for (int i = 0; i < s.Length; i++)
            {
                if (!char.IsLetter(s, i))
                    str.Append(s[i]);
            }

            if (str.Length == 0)
                return true;

            return IsNumeric(str.ToString(), numInfo, decimalNumber);
        }

        /// <summary>
        /// Indicates whether a <see cref="String"/> contains only letters.
        /// </summary>
        /// <param name="s">A <see cref="String"/>.</param>
        /// <returns>true if <c>s</c> is an letter-only String; otherwise, false.</returns>
        /// <remarks>
        /// This method verify whether a <see cref="String"/> contains only letters.
        /// </remarks>
        /// <example>
        /// The following console application example show how to use IsLetterOnly method.
        /// <code>
        /// using System;
        /// using SklLib;
        ///
        /// class Program
        /// {
        ///        static void Main()
        ///        {
        ///            string text1 = "Make these tests";
        ///            string text2 = "testing?";
        ///            string text3 = "text3";
        ///            string text4 = "example";
        ///            Console.WriteLine("First example: {0}", text1.IsLetterOnly());
        ///            Console.WriteLine("Second example: {0}", text2.IsLetterOnly());
        ///            Console.WriteLine("Third example: {0}", text3.IsLetterOnly());
        ///            Console.WriteLine("Fourth example: {0}", text4.IsLetterOnly());
        ///            Console.ReadKey();
        ///        }
        /// }
        /// </code>
        /// <br></br>This example returns the following output.
        /// <code>
        /// First example: False
        /// Second example: False
        /// Third example: False
        /// Fourth example: True
        /// </code>
        /// </example>
        /// <exception cref="ArgumentNullException"><c>s</c> is a null reference.</exception>
        public static bool IsLetterOnly(this string s)
        {
            if (s == null)
                throw new ArgumentNullException(resExceptions.ArgumentNull.Replace("%var", "s"));
            for (int i = 0; i < s.Length; i++)
            {
                if (!char.IsLetter(s, i))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Indicates whether a <see cref="String"/> is categorized as a integer number.
        /// </summary>
        /// <param name="s">A <see cref="String"/>.</param>
        /// <returns>True if <c>s</c> is numeric; otherwise, false.</returns>
        /// <remarks>
        /// <para>This method verify whether a <see cref="String"/> contains only numeric
        /// characters, including current culture specific Number Group Separator.</para>
        /// <para>Based on the <see cref="System.Globalization.CultureInfo"/> of the current thread.</para>
        /// </remarks>
        /// <example>
        /// The following console application example show how to use IsNumeric method.
        /// <code>
        /// using System;
        /// using SklLib;
        ///
        /// class Program
        /// {
        ///        static void Main()
        ///        {
        ///            string text1 = "4,687.59";
        ///            string text2 = "8.98";
        ///            string text3 = "51687";
        ///            string text4 = "25,598";
        ///            Console.WriteLine("First example: {0}", text1.IsNumeric());
        ///            Console.WriteLine("Second example: {0}", text2.IsNumeric());
        ///            Console.WriteLine("Third example: {0}", text3.IsNumeric());
        ///            Console.WriteLine("Fourth example: {0}", text4.IsNumeric());
        ///            Console.ReadKey();
        ///        }
        ///    }
        /// </code>
        /// <br></br>This example returns the following output.
        /// <code>
        /// First example: False
        /// Second example: False
        /// Third example: True
        /// Fourth example: True
        /// </code>
        /// </example>
        /// <exception cref="ArgumentNullException"><c>s</c> is a null reference.</exception>
        public static bool IsNumeric(this string s)
        {
            return IsNumeric(s, NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Indicates whether a <see cref="String"/> is categorized as a integer number.
        /// </summary>
        /// <param name="s">A <see cref="String"/>.</param>
        /// <param name="numInfo">Culture specific <see cref="NumberFormatInfo"/>.</param>
        /// <returns>True if <c>s</c> is numeric; otherwise, false.</returns>
        /// <remarks>
        /// This method verify whether a <see cref="String"/> contains only numeric
        /// characters, including the user-defined culture specific Number Group Separator.
        /// </remarks>
        /// <example>
        /// The following console application example show how to use IsNumeric method.
        /// <code>
        /// using System;
        /// using SklLib;
        /// using System.Globalization;
        ///
        /// class Program
        /// {
        ///        static void Main()
        ///        {
        ///            NumberFormatInfo nfi = CultureInfo.GetCultureInfo("en-US").NumberFormat;
        ///            string text1 = "4,687.59";
        ///            string text2 = "8.98";
        ///            string text3 = "51687";
        ///            string text4 = "25,598";
        ///            Console.WriteLine("First example: {0}", text1.IsNumeric(nfi));
        ///            Console.WriteLine("Second example: {0}", text2.IsNumeric(nfi));
        ///            Console.WriteLine("Third example: {0}", text3.IsNumeric(nfi));
        ///            Console.WriteLine("Fourth example: {0}", text4.IsNumeric(nfi));
        ///            Console.ReadKey();
        ///        }
        ///    }
        /// </code>
        /// <br></br>This example returns the following output.
        /// <code>
        /// First example: False
        /// Second example: False
        /// Third example: True
        /// Fourth example: True
        /// </code>
        /// </example>
        /// <exception cref="ArgumentNullException"><c>s</c> or <c>numInfo</c> is a null reference.</exception>
        public static bool IsNumeric(this string s, NumberFormatInfo numInfo)
        {
            if (s == null)
                throw new ArgumentNullException(resExceptions.ArgumentNull.Replace("%var", "s"));
            if (numInfo == null)
                throw new ArgumentNullException(resExceptions.ArgumentNull.Replace("%var", "numInfo"));

            string uStr = s.Replace(numInfo.NumberGroupSeparator, "");
            for (int i = 0; i < uStr.Length; i++)
            {
                if (!(char.IsNumber(uStr[i])))
                    return false;
            }

            if (s.Contains(numInfo.NumberGroupSeparator))
                if (Formatting.Numbers.ToStringTrimExcess(Convert.ToDecimal(uStr, numInfo), "n", numInfo) != s)
                    return false;
            return true;
        }

        /// <summary>
        /// Indicates whether a String is categorized as a integer number or
        /// a decimal number.
        /// </summary>
        /// <param name="s">A <see cref="String"/>.</param>
        /// <param name="decimalNumber">Indicates whether the can be a decimal number.</param>
        /// <returns>True if <c>s</c> is a number; otherwise, false.</returns>
        /// <remarks>
        /// <para>This method verify whether a <see cref="String"/> contains only numeric
        /// characters, including current culture specific Number Group Separator.</para>
        /// <para>If decimalNumber parameter is true, this method accepts current culture
        /// specific Number Decimal Separator.</para>
        /// <para>Based on the <see cref="System.Globalization.CultureInfo"/> of the current thread.</para>
        /// </remarks>
        /// <example>
        /// The following console application example show how to use IsNumeric method.
        /// <code>
        /// using System;
        /// using SklLib;
        ///
        /// class Program
        /// {
        ///        static void Main()
        ///        {
        ///            string text1 = "46,87.59";
        ///            string text2 = "78.98";
        ///            string text3 = "51687.547";
        ///            string text4 = "25,598.9";
        ///            string text5 = "87643";
        ///            Console.WriteLine("First example: {0}", text1.IsNumeric(true));
        ///            Console.WriteLine("Second example: {0}", text2.IsNumeric(true));
        ///            Console.WriteLine("Third example: {0}", text3.IsNumeric(true));
        ///            Console.WriteLine("Fourth example: {0}", text4.IsNumeric(true));
        ///            Console.WriteLine("Fifth example: {0}", text5.IsNumeric(true));
        ///            Console.ReadKey();
        ///        }
        ///    }
        /// </code>
        /// <br></br>This example returns the following output.
        /// <code>
        /// First example: False
        /// Second example: True
        /// Third example: True
        /// Fourth example: True
        /// Fifth example: True
        /// </code>
        /// </example>
        /// <exception cref="ArgumentNullException"><c>s</c> is a null reference.</exception>
        public static bool IsNumeric(this string s, bool decimalNumber)
        {
            NumberFormatInfo numInfo = CultureInfo.CurrentCulture.NumberFormat;
            if (decimalNumber)
                return IsNumeric(s, numInfo, true);
            else
                return IsNumeric(s, numInfo);
        }

        /// <summary>
        /// Indicates whether a String is categorized as a integer number or
        /// a decimal number.
        /// </summary>
        /// <param name="s">A <see cref="String"/>.</param>
        /// <param name="numInfo">Culture specific <see cref="NumberFormatInfo"/>.</param>
        /// <param name="decimalNumber">Indicates whether the can be a decimal number.</param>
        /// <returns>True if <c>s</c> is a number; otherwise, false.</returns>
        /// <remarks>
        /// <para>This method verify whether a <see cref="String"/> contains only numeric
        /// characters, including the user-defined culture specific Number Group Separator.</para>
        /// <para>If decimalNumber parameter is true, this method accepts user-defined culture
        /// specific Number Decimal Separator.</para>
        /// </remarks>
        /// <example>
        /// The following console application example show how to use IsNumeric method.
        /// <code>
        /// using System;
        /// using SklLib;
        ///
        /// class Program
        /// {
        ///        static void Main()
        ///        {
        ///            NumberFormatInfo nfi = CultureInfo.GetCultureInfo("en-US").NumberFormat;
        ///            string text1 = "46,87.59";
        ///            string text2 = "78.98";
        ///            string text3 = "51687.547";
        ///            string text4 = "25,598.9";
        ///            string text5 = "87643";
        ///            Console.WriteLine("First example: {0}", text1.IsNumeric(nfi, true));
        ///            Console.WriteLine("Second example: {0}", text2.IsNumeric(nfi, true));
        ///            Console.WriteLine("Third example: {0}", text3.IsNumeric(nfi, true));
        ///            Console.WriteLine("Fourth example: {0}", text4.IsNumeric(nfi, true));
        ///            Console.WriteLine("Fifth example: {0}", text5.IsNumeric(nfi, true));
        ///            Console.ReadKey();
        ///        }
        ///    }
        /// </code>
        /// <br></br>This example returns the following output.
        /// <code>
        /// First example: False
        /// Second example: True
        /// Third example: True
        /// Fourth example: True
        /// Fifth example: True
        /// </code>
        /// </example>
        /// <exception cref="ArgumentNullException"><c>s</c> or <c>numInfo</c> is a null reference.</exception>
        public static bool IsNumeric(this string s, NumberFormatInfo numInfo, bool decimalNumber)
        {
            if (numInfo == null)
                throw new ArgumentNullException(resExceptions.ArgumentNull.Replace("%var", "numInfo"));

            if (!decimalNumber)
                return IsNumeric(s, numInfo);

            if (s == null)
                throw new ArgumentNullException(resExceptions.ArgumentNull.Replace("%var", "s"));

            int idxNDS = s.IndexOf(numInfo.NumberDecimalSeparator);

            if (idxNDS != s.LastIndexOf(numInfo.NumberDecimalSeparator))
                return false;

            string intStr;

            if (idxNDS != -1)
            {
                intStr = s.Substring(0, idxNDS);

                if (s.Length <= idxNDS + 1)
                    return false;

                string decStr = s.Substring(idxNDS + 1);

                if (decStr.IndexOf(numInfo.NumberGroupSeparator) != -1)
                    return false;
                if (!IsNumeric(decStr, numInfo))
                    return false;
            }
            else
                intStr = s;

            return IsNumeric(intStr, numInfo);
        }

        /// <summary>
        /// Repeats a String to the number of times specified.
        /// </summary>
        /// <param name="s">A <see cref="String"/>.</param>
        /// <param name="count">The number of times that <c>s</c> occurs.</param>
        /// <returns>The <c>s</c> repeated the number of times specified.</returns>
        /// <remarks>
        /// If <c>count</c> is zero, an <see cref="String.Empty"/> instance is returned.
        /// </remarks>
        /// <example>
        /// <para>The following simple code example demonstrates how you can use the RepeatString
        /// method.</para>
        /// <code>
        /// String str = "-=";
        /// String repeated = str.RepeatString(10);
        /// // The value of repeated String is "-=-=-=-=-=-=-=-=-=-=", "-=" repeated 10 times.
        /// </code>
        /// </example>
        /// <exception cref="ArgumentNullException"><c>s</c> is a null reference.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><c>count</c> is less than zero, or
        /// <c>s</c> repeated in the specified times, by <c>count</c>, is too big to return.</exception>
        public static string RepeatString(this string s, int count)
        {
            if (s == null)
                throw new ArgumentNullException(resExceptions.ArgumentNull.Replace("%var", "s"));
            if (count < 0)
                throw new ArgumentOutOfRangeException(resExceptions.LessThanZero.Replace("%var", "count"));
            if (s.Length * count > int.MaxValue)
                throw new ArgumentOutOfRangeException(resExceptions.TooBig_StringOrNumber.Replace("%var1", "s").Replace("%var2", "count"));

            if (count == 0)
                return string.Empty;

            int len = s.Length;
            System.Text.StringBuilder baseStr = new System.Text.StringBuilder(len * count, len * count);
            for (int i = 0; i < count; i++)
                baseStr.Append(s);
            return baseStr.ToString();
        }

        /// <summary>
        /// Splits a String into chunks of specified size.
        /// </summary>
        /// <param name="s">A <see cref="String"/>.</param>
        /// <param name="chunkSize">The chunk size.</param>
        /// <returns>An String array from specified String.</returns>
        /// <exception cref="ArgumentNullException"><c>s</c> is a null reference.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><c>chunkSize</c> is less than zero.</exception>
        public static string[] Split(this string s, int chunkSize)
        {
            if (s == null)
                throw new ArgumentNullException(resExceptions.ArgumentNull.Replace("%var", "s"));
            if (chunkSize < 0)
                throw new ArgumentOutOfRangeException(resExceptions.LessThanZero.Replace("%var", "chunkSize"));

            if (chunkSize == 0 || chunkSize >= s.Length)
                return new string[] { s };

            string[] result = new string[(int)Math.Ceiling(s.Length / (double)chunkSize)];
            for (int i = 0; i < result.Length; i++)
            {
                if (i != result.Length - 1)
                    result[i] = s.Substring(i * chunkSize, chunkSize);
                else
                    result[i] = s.Substring(i * chunkSize);
            }

            return result;
        }

        // Eliminates accent of a byte character.
        // aByte = A Byte character (Unicode).
        private static void ChangeByte(ref byte aByte)
        {
            if (aByte >= 0xC0 && aByte <= 0xC5)    //ÀÁÂÃÄÅ
                aByte = 0x41; //A
            else if (aByte == 0xC7) //Ç
                aByte = 0x43; //C
            else if (aByte >= 0xC8 && aByte <= 0xCB)    //ÈÉÊË
                aByte = 0x45; //E
            else if (aByte >= 0xCC && aByte <= 0xCF)    //ÌÍÎÏ
                aByte = 0x49; //I
            else if (aByte >= 0xD2 && aByte <= 0xD6)    //ÒÓÔÕÖ
                aByte = 0x4F; //O
            else if (aByte >= 0xD9 && aByte <= 0xDC)    //ÙÚÛÜ
                aByte = 0x55; //U 
            else if (aByte >= 0xE0 && aByte <= 0xE5) //àáâãäå
                aByte = 0x61; //a
            else if (aByte == 0xE7) //ç
                aByte = 0x63; //c
            else if (aByte >= 0xE8 && aByte <= 0xEB)    //èéêë
                aByte = 0x65; //e
            else if (aByte >= 0xEC && aByte <= 0xEF)    //ìíîï
                aByte = 0x69; //i
            else if (aByte >= 0xF2 && aByte <= 0xF6)    //òóôõö
                aByte = 0x6F; //o
            else if (aByte >= 0xF9 && aByte <= 0xFC)    //ùúûü
                aByte = 0x75; //u
        }



        /// <summary>
        /// Stores a <see cref="Char"/> and itself position in a <see cref="String"/>.
        /// </summary>
        public struct IndexedChar
        {
            /// <summary>
            /// A <see cref="Char"/>.
            /// </summary>
            public char Character;
            /// <summary>
            /// A integer position.
            /// </summary>
            public int Position;

            /// <summary>
            /// Initializes the IndexedChar structure with a <see cref="Char"/> and itself position.
            /// </summary>
            /// <param name="c">A <see cref="Char"/>.</param>
            /// <param name="index">Position of <c>c</c> in a <see cref="String"/>.</param>
            public IndexedChar(char c, int index)
            {
                Character = c;
                Position = index;
            }

            /// <summary>
            /// Initializes the IndexedChar structure with a specified <see cref="Char"/>
            /// of a <see cref="String"/>.
            /// </summary>
            /// <param name="s">A <see cref="String"/>.</param>
            /// <param name="index">Position of <see cref="Char"/> to get.</param>
            public IndexedChar(string s, int index)
            {
                Character = s[index];
                Position = index;
            }
        }
    }
}
