// ConfigReaderBase.cs
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

using System;

namespace SklLib.IO
{
    /// <summary>
    /// Provides base methods to implement a class to represent a configuration file reader.
    /// </summary>
    public abstract class ConfigReaderBase : IValidatable
    {
        #region Fields

        private const char DEFAULT_CSV_SEPARATOR = ';';

        protected SklLib.IO.ConfigFileReader cfgreader;
        protected string filename;
        protected string[] sections;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the file name backed by this instance.
        /// </summary>
        public string FileName { get { return filename; } }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the specified key from specified section converted to boolean.
        /// </summary>
        /// <param name="section">The configuration file section.</param>
        /// <param name="key">The configuration file key.</param>
        /// <returns>The key value as boolean.</returns>
        protected bool GetBoolean(string section, string key)
        {
            bool result;
            string val;
            if (!cfgreader.TryReadValue(section, key, out val))
                return false;
            if (!bool.TryParse(val, out result))
                return false;
            return result;
        }

        /// <summary>
        /// Gets the specified key from specified section converted to integer.
        /// </summary>
        /// <param name="section">The configuration file section.</param>
        /// <param name="key">The configuration file key.</param>
        /// <returns>The key value as integer.</returns>
        protected int GetInteger(string section, string key)
        {
            int result;
            string val;
            if (!cfgreader.TryReadValue(section, key, out val))
                return -1;
            if (!int.TryParse(val, out result))
                return -1;
            return result;
        }

        /// <summary>
        /// Gets the specified key from specified section as string.
        /// </summary>
        /// <param name="section">The configuration file section.</param>
        /// <param name="key">The configuration file key.</param>
        /// <returns>The key value as string.</returns>
        protected string GetString(string section, string key)
        {
            string val;
            cfgreader.TryReadValue(section, key, out val);
            return val;
        }

        /// <summary>
        /// Gets the specified key from specified section converted to string array.
        /// </summary>
        /// <param name="section">The configuration file section.</param>
        /// <param name="key">The configuration file key.</param>
        /// <returns>The key value as string array.</returns>
        protected string[] GetCsvString(string section, string key)
        {
            return GetCsvString(section, key, DEFAULT_CSV_SEPARATOR);
        }

        /// <summary>
        /// Gets the specified key from specified section converted to string array.
        /// </summary>
        /// <param name="section">The configuration file section.</param>
        /// <param name="key">The configuration file key.</param>
        /// <param name="separator">The character used as separator between array items.</param>
        /// <returns>The key value as string array.</returns>
        protected string[] GetCsvString(string section, string key, char separator)
        {
            string val;
            cfgreader.TryReadValue(section, key, out val);
            string[] list = new string[0];
            if (!string.IsNullOrEmpty(val) && !string.IsNullOrEmpty(val.Trim()))
                list = val.Split(new char[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            return list;
        }

        /// <summary>
        /// Gets the specified key from specified section converted to TimeSpan.
        /// </summary>
        /// <param name="section">The configuration file section.</param>
        /// <param name="key">The configuration file key.</param>
        /// <returns>The key value as TimeSpan.</returns>
        protected TimeSpan GetTimeSpan(string section, string key)
        {
            TimeSpan result;
            string val;
            if (!cfgreader.TryReadValue(section, key, out val))
                return TimeSpan.MaxValue;
            if (!TimeSpan.TryParse(val, out result))
                return TimeSpan.MaxValue;
            return result;
        }

        /// <summary>
        /// Determines whether current instance is valid.
        /// </summary>
        /// <returns>True whether is valid; otherwise false.</returns>
        public abstract bool IsValid();

        /// <summary>
        /// Reads configuration file and populates current instance.
        /// </summary>
        protected void LoadFile()
        {
            if (cfgreader == null)
                cfgreader = new SklLib.IO.ConfigFileReader(filename);

            cfgreader.ReloadFile();
            sections = cfgreader.ReadSectionsName();
        }

        #endregion
    }
}
