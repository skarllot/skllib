// PostalCodes.cs
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

using SklLib.Performance;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace SklLib.Formatting
{
    /// <summary>
    /// Provides methods to format Postal Codes.
    /// </summary>
    /// <remarks>
    /// <para>This class contains information as a Postal Code is formatted and stores several Postal Code
    /// format information of specific cultures.</para>
    /// <para>To create a PostalCode for a specific culture, use <see cref="GetCultureBasedInfo"/>
    /// method. To create a PostalCode for the culture of the current thread, use the
    /// <see cref="CurrentInfo"/> property. Use the PostalCode constructor for a writable version.</para>
    /// <para>This class implements the ICloneable interface to enable duplication of PostalCode objects.
    /// It also implements IMaskeable indicating that this class can be used by SklLib.Windows.Forms.Mask
    /// class.</para>
    /// </remarks>
    /// <example>
    /// The following console application example show how to use PostalCode class.
    /// <code>
    /// using System;
    /// using SklLib.Formatting;
    /// 
    /// class Program
    /// {
    ///     static void Main()
    ///     {
    ///         PostalCode pCode = PostalCode.GetCultureBasedInfo("en-US");
    ///            string userPCode = "12345-6789";
    ///         Console.WriteLine("Compacted Postal Code: {0}", pCode.GetCompactPostalCode(userPCode));
    ///         string internalPCode = "987654321";
    ///         Console.WriteLine("Standard Postal Code: {0}", pCode.GetStandardPostalCode(internalPCode));
    ///         Console.ReadKey();
    ///     }
    /// }
    /// </code>
    /// This example returns the following output.
    /// <code>
    /// Compacted Postal Code: 123456789
    /// Standard Postal Code: 98765-4321
    /// </code>
    /// </example>
    public sealed class PostalCode : FormatStringBase
    {
        #region Field

        /// <summary>
        /// Stores a <see cref="Dictionary&lt;TKey, TValue&gt;"/>, where key is a culture name and value
        /// is a lazy-loaded instance of <see cref="PostalCode"/>.
        /// </summary>
        private static Dictionary<string, LazyLoaded<PostalCode>> storedCultureInfo;

        #endregion

        #region Constructor

        static PostalCode()
        {
            storedCultureInfo = new Dictionary<string, LazyLoaded<PostalCode>>(7);
            storedCultureInfo.Add("de-DE", new LazyLoaded<PostalCode>(Get_deDE));
            storedCultureInfo.Add("en-CA", new LazyLoaded<PostalCode>(Get_enCA));
            storedCultureInfo.Add("en-US", new LazyLoaded<PostalCode>(Get_enUS));
            storedCultureInfo.Add("fr-FR", new LazyLoaded<PostalCode>(Get_frFR));
            storedCultureInfo.Add("nl-NL", new LazyLoaded<PostalCode>(Get_nlNL));
            storedCultureInfo.Add("pt-BR", new LazyLoaded<PostalCode>(Get_ptBR));
            storedCultureInfo.Add("pt-PT", new LazyLoaded<PostalCode>(Get_ptPT));
        }

        /// <summary>
        /// Initializes a new writable instance of the <see cref="PostalCode"/> class.
        /// </summary>
        /// <remarks>
        /// The properties of the new instance can be modified if you want user-defined formatting.
        /// </remarks>
        public PostalCode() : base() { }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a read-only <see cref="PostalCode"/> with informations based on the current culture.
        /// </summary>
        /// <value>A read-only <see cref="PostalCode"/> based on the
        /// <see cref="System.Globalization.CultureInfo"/> of the current thread.</value>
        public static PostalCode CurrentInfo
        {
            get
            {
                string name = CultureInfo.CurrentCulture.Name;
                return GetCultureBasedInfo(name);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Retrieves a cached, instance of a PostalCode using the specified culture name.
        /// </summary>
        /// <param name="name">The name of a culture.</param>
        /// <returns>A PostalCode object.</returns>
        /// <exception cref="ArgumentException">name specifies a culture that is not supported.</exception>
        public static PostalCode GetCultureBasedInfo(string name)
        {
            if (!storedCultureInfo.ContainsKey(name))
                throw new ArgumentException(resExceptions.UnsupportedCulture);

            return storedCultureInfo[name];
        }

        /// <summary>
        /// Gets the list of names of supported cultures by <see cref="PostalCode"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerable&lt;T&gt;"/> of type <see cref="String"/> that contains the cultures names.
        /// The list of cultures is sorted.</returns>
        public static IEnumerable<string> GetDisponibleCultures()
        {
            return storedCultureInfo.Keys;
        }

        private static PostalCode Get_deDE()
        {
            PostalCode pc = new PostalCode();
            pc.Expression = @"^\d{5}$";
            pc.CompactExpression = @"^\d{5}$";
            pc.Mask = "00000";
            Strings.IndexedChar[] ic = new Strings.IndexedChar[0];
            pc.RemovableChars = ic;
            pc.isReadOnly = true;
            return pc;
        }

        private static PostalCode Get_enCA()
        {
            PostalCode pc = new PostalCode();
            pc.Expression = @"^[A-Y]\d[A-Z] \d[A-Z]\d$";
            pc.CompactExpression = @"^[A-Y]\d[A-Z]\d[A-Z]\d$";
            pc.Mask = "A0A! 0A0";
            Strings.IndexedChar[] ic = new Strings.IndexedChar[1];
            ic[0] = new Strings.IndexedChar(' ', 3);
            pc.RemovableChars = ic;
            pc.isReadOnly = true;
            return pc;
        }

        private static PostalCode Get_enUS()
        {
            PostalCode pc = new PostalCode();
            pc.Expression = @"^\d{5}-\d{4}$";
            pc.CompactExpression = @"^\d{9}$";
            pc.Mask = "00000!-0000";
            Strings.IndexedChar[] ic = new Strings.IndexedChar[1];
            ic[0] = new Strings.IndexedChar('-', 5);
            pc.RemovableChars = ic;
            pc.isReadOnly = true;
            return pc;
        }

        private static PostalCode Get_frFR()
        {
            return Get_deDE();
        }

        private static PostalCode Get_nlNL()
        {
            PostalCode pc = new PostalCode();
            pc.Expression = @"^\d{4} [A-EGHJ-NPR-TV-XZ]{2}$";
            pc.CompactExpression = @"^\d{4}[A-EGHJ-NPR-TV-XZ]{2}$";
            pc.Mask = "0000! AA";
            Strings.IndexedChar[] ic = new Strings.IndexedChar[1];
            ic[0] = new Strings.IndexedChar(' ', 4);
            pc.RemovableChars = ic;
            pc.isReadOnly = true;
            return pc;
        }

        private static PostalCode Get_ptBR()
        {
            PostalCode pc = new PostalCode();
            pc.Expression = @"^\d{5}-\d{3}$";
            pc.CompactExpression = @"^\d{8}$";
            pc.Mask = "00000!-000";
            Strings.IndexedChar[] ic = new Strings.IndexedChar[1];
            ic[0] = new Strings.IndexedChar('-', 5);
            pc.RemovableChars = ic;
            pc.isReadOnly = true;
            return pc;
        }

        private static PostalCode Get_ptPT()
        {
            PostalCode pc = new PostalCode();
            pc.Expression = @"^\d{4}-\d{3} [A-Z]{1,25}$";
            pc.CompactExpression = @"^\d{7}[A-Z]{1,25}$";
            pc.Mask = "0000!-000! A";
            Strings.IndexedChar[] ic = new Strings.IndexedChar[2];
            ic[0] = new Strings.IndexedChar('-', 4);
            ic[1] = new Strings.IndexedChar(' ', 8);
            pc.RemovableChars = ic;
            pc.isReadOnly = true;
            return pc;
        }

        #endregion
    }
}
