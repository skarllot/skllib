// IniSectionReaderBase.cs
//
// Copyright (C) 2014 Fabrício Godoy
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using SklLib.IO;
using System;
using System.Text.RegularExpressions;

namespace SklLib.Configuration
{
    /// <summary>
    /// Provides base methods to implement a class to represent a section reader from INI file.
    /// </summary>
    public abstract class IniSectionReaderBase : IValidatable
    {
        #region Fields

        /// <summary>
        /// Default character for comma-separated values.
        /// </summary>
        public const char DEFAULT_CSV_SEPARATOR = ';';

        protected IniFileReader cfgreader;
        protected string section;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of IniSectionReaderBase class.
        /// </summary>
        /// <param name="reader">The configuration file reader.</param>
        /// <param name="section">The section name for this instance.</param>
        public IniSectionReaderBase(IniFileReader reader, string section)
        {
            this.cfgreader = reader;
            this.section = section;
        }

        #endregion

        #region Properties
        
        /// <summary>
        /// Gets the section name.
        /// </summary>
        public string SectionName { get { return section; } }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the specified key converted to boolean.
        /// </summary>
        /// <param name="key">The configuration file key.</param>
        /// <returns>The key value as boolean.</returns>
        protected bool? GetBoolean(string key)
        {
            string val;
            if (!cfgreader.TryReadValue(section, key, out val))
                return null;

            bool result;
            if (!bool.TryParse(val, out result))
                return null;
            return result;
        }

        /// <summary>
        /// Gets the specified key converted to integer.
        /// </summary>
        /// <param name="key">The configuration file key.</param>
        /// <returns>The key value as integer.</returns>
        protected int? GetInteger(string key)
        {
            string val;
            if (!cfgreader.TryReadValue(section, key, out val))
                return null;

            int result;
            if (!int.TryParse(val, out result))
                return null;
            return result;
        }

        /// <summary>
        /// Gets the specified key as string.
        /// </summary>
        /// <param name="key">The configuration file key.</param>
        /// <returns>The key value as string.</returns>
        protected string GetString(string key)
        {
            string val;
            cfgreader.TryReadValue(section, key, out val);
            return val;
        }

        /// <summary>
        /// Gets the specified key converted to string array.
        /// </summary>
        /// <param name="key">The configuration file key.</param>
        /// <returns>The key value as string array.</returns>
        protected string[] GetCsvString(string key)
        {
            return GetCsvString(key, DEFAULT_CSV_SEPARATOR);
        }

        /// <summary>
        /// Gets the specified key converted to string array.
        /// </summary>
        /// <param name="key">The configuration file key.</param>
        /// <param name="separator">The character used as separator between array items.</param>
        /// <returns>The key value as string array.</returns>
        protected string[] GetCsvString(string key, char separator)
        {
            string val;
            cfgreader.TryReadValue(section, key, out val);
            string[] list = new string[0];
            if (!string.IsNullOrEmpty(val) && !string.IsNullOrEmpty(val.Trim()))
                list = val.Split(new char[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            return list;
        }

        /// <summary>
        /// Gets the specified key converted to Regex class.
        /// </summary>
        /// <param name="key">The configuration file key.</param>
        /// <returns>The key value as Regex class.</returns>
        protected Regex GetRegex(string key)
        {
            string val;
            if (!cfgreader.TryReadValue(section, key, out val))
                return null;
            if (string.IsNullOrEmpty(val)
                || string.IsNullOrEmpty(val.Trim()))
                return null;

            Regex result;
            try { result = new Regex(val); }
            catch { return null; }

            return result;
        }

        /// <summary>
        /// Gets the specified key converted to TimeSpan.
        /// </summary>
        /// <param name="key">The configuration file key.</param>
        /// <returns>The key value as TimeSpan.</returns>
        protected TimeSpan? GetTimeSpan(string key)
        {
            string val;
            if (!cfgreader.TryReadValue(section, key, out val))
                return null;

            TimeSpan result;
            if (!TimeSpan.TryParse(val, out result))
                return null;
            return result;
        }

        #endregion

        #region IValidatable

        /// <summary>
        /// Validates each property from current instance and executes a action.
        /// </summary>
        /// <param name="action">Action to execute after each validation.</param>
        /// <returns>True whether all properties are valid; otherwise, false.</returns>
        public abstract bool Validate(Action<ValidationEventArgs> action);

        #endregion
    }
}
