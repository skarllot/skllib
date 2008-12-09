// Telephones.cs
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
using System.Collections.Generic;
using System.Globalization;

namespace Root.Formatting
{
    /// <summary>
    /// Provides methods to format Telephones.
    /// </summary>
    public sealed class Telephones : FormatStringBase
    {
        #region Fields

        /// <summary>
        /// Stores a <see cref="Dictionary"/>, where key is a culture name and value
        /// is a delegate to get type- and culture-specific.
        /// </summary>
        private static Dictionary<string, GetType<Telephones>> storedCultureInfo;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new writable instance of the <see cref="Telephones"/> class.
        /// </summary>
        /// <remarks>
        /// The properties of the new instance can be modified if you want user-defined formatting.
        /// </remarks>
        public Telephones() : base() { }

        static Telephones()
        {
            storedCultureInfo = new Dictionary<string, GetType<Telephones>>(2);
            storedCultureInfo.Add("pt-BR", Get_ptBR);
            storedCultureInfo.Add("en-US", Get_enUS);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a Telephones with values based on the current culture.
        /// </summary>
        /// <value>A Telephones based on the <see cref="CultureInfo"/> of the current thread.</value>
        public static Telephones CurrentInfo
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
        /// Retrieves a cached, instance of a Telephones using the specified culture name.
        /// </summary>
        /// <param name="name">The name of a culture.</param>
        /// <returns>A Telephones object.</returns>
        /// <exception cref="ArgumentException">name specifies a culture that is not supported.</exception>
        public static Telephones GetCultureBasedInfo(string name)
        {
            if (name.IndexOf('-') == -1)
                name = name.Insert(2, "-");
            name = name.Remove(3) + name.Substring(3).ToUpper();
            if (!storedCultureInfo.ContainsKey(name))
                throw new ArgumentException(resExceptions.UnsupportedCulture);
            return storedCultureInfo[name]();
        }

        /// <summary>
        /// Gets the list of names of supported cultures by Telephones.
        /// </summary>
        /// <returns>An <see cref="Array"/> of type <see cref="String"/> that contains the cultures names.
        /// The <see cref="Array"/> of cultures is sorted.</returns>
        public static string[] GetDisponibleCultures()
        {
            string[] cultureNames = new string[storedCultureInfo.Keys.Count];
            storedCultureInfo.Keys.CopyTo(cultureNames, 0);
            return cultureNames;
        }

        private static Telephones Get_enUS()
        {
            Telephones tel = new Telephones();
            tel.Expression = @"^\([2-9][0-8]\d\) [2-9]\d{2}-\d{4}$";
            tel.CompactExpression = @"^[2-9][0-8]\d[2-9]\d{6}$";
            tel.Mask = "!(200!)! 200!-0000";
            tel.RemovableChars = Strings.AdditionalChars("(123) 456-7890", "1234567890");
            tel.isReadOnly = true;
            return tel;
        }

        private static Telephones Get_ptBR()
        {
            Telephones tel = new Telephones();
            tel.Expression = @"^\(\d{2}\) \d{4}-\d{4}$";
            tel.CompactExpression = @"^\d{10}$";
            tel.Mask = "!(00!)! 0000!-0000";
            tel.RemovableChars = Strings.AdditionalChars("(12) 3456-7890", "1234567890");
            tel.isReadOnly = true;
            return tel;
        }

        #endregion
    }
}
