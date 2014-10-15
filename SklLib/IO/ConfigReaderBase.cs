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
    abstract class ConfigReaderBase : IValidatable
    {
        #region Fields

        private const char DEFAULT_CSV_SEPARATOR = ';';

        protected SklLib.IO.ConfigFileReader cfgreader;
        protected string filename;
        protected string[] sections;

        #endregion

        #region Properties

        public string FileName { get { return filename; } }

        #endregion

        #region Methods

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

        protected string GetString(string section, string key)
        {
            string val;
            cfgreader.TryReadValue(section, key, out val);
            return val;
        }

        protected string[] GetCsvString(string section, string key)
        {
            return GetCsvString(section, key, DEFAULT_CSV_SEPARATOR);
        }

        protected string[] GetCsvString(string section, string key, char separator)
        {
            string val;
            cfgreader.TryReadValue(section, key, out val);
            string[] list = new string[0];
            if (!string.IsNullOrEmpty(val) && !string.IsNullOrEmpty(val.Trim()))
                list = val.Split(new char[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            return list;
        }

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

        public abstract bool IsValid();

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
