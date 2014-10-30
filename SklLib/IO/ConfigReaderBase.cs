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

        protected SklLib.IO.ConfigFileReader cfgreader;
        protected string filename;
        protected ConfigSectionReaderBase[] sections;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the file name backed by this instance.
        /// </summary>
        public string FileName { get { return filename; } }

        /// <summary>
        /// Gets a list of sections name that is mandatory.
        /// </summary>
        protected virtual string[] MandatorySections
        {
            get { return new string[0]; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a section reader instance from its name.
        /// </summary>
        /// <param name="section">The sectio name.</param>
        /// <returns>A ConfigSectionReaderBase instance.</returns>
        protected ConfigSectionReaderBase GetSectionByName(string section)
        {
            foreach (ConfigSectionReaderBase item in sections) {
                if (item.SectionName == section)
                    return item;
            }

            return null;
        }

        /// <summary>
        /// Gets a new instance ConfigSectionReaderBase class based on section name.
        /// </summary>
        /// <param name="section">The section name.</param>
        /// <returns>A new instance of ConfigSectionReaderBase class.</returns>
        protected abstract ConfigSectionReaderBase GetSectionReader(string section);

        /// <summary>
        /// Determines whether specified section name exists into current configuration.
        /// </summary>
        /// <param name="section">The section name.</param>
        /// <returns>True whether section is found; otherwise false.</returns>
        protected bool HasSection(string section)
        {
            return GetSectionByName(section) == null ? false : true;
        }

        /// <summary>
        /// Determines whether current instance is valid.
        /// </summary>
        /// <returns>True whether is valid; otherwise false.</returns>
        public virtual bool IsValid()
        {
            if (MandatorySections != null
                && MandatorySections.Length > 0) {
                foreach (string item in MandatorySections) {
                    if (!HasSection(item))
                        return false;
                }
            }

            foreach (ConfigSectionReaderBase item in sections) {
                if (!item.IsValid())
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Reads configuration file and populates current instance.
        /// </summary>
        public virtual void LoadFile()
        {
            if (cfgreader == null)
                cfgreader = new SklLib.IO.ConfigFileReader(filename);

            cfgreader.ReloadFile();
            string[] sectionsName = cfgreader.ReadSectionsName();
            sections = new ConfigSectionReaderBase[sectionsName.Length];

            for (int i = 0; i < sectionsName.Length; i++) {
                sections[i] = GetSectionReader(sectionsName[i]);
            }
        }

        #endregion
    }
}
