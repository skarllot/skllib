// IniReaderBase.cs
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

namespace SklLib.Configuration
{
    /// <summary>
    /// Provides base methods to implement a class to represent a INI file reader.
    /// </summary>
    public abstract class IniReaderBase : IValidatable
    {
        #region Fields

        private const string MESSAGE_SECTION_MANDATORY_MISSING = "The mandatory section {0} was not found";

        /// <summary>
        /// The INI file reader.
        /// </summary>
        protected IniFileReader cfgreader;

        /// <summary>
        /// The INI file name.
        /// </summary>
        protected string filename;

        /// <summary>
        /// Stores found sections into target INI file.
        /// </summary>
        protected IniSectionReaderBase[] sections;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the file name backed by this instance.
        /// </summary>
        public string FileName { get { return filename; } }

        /// <summary>
        /// Gets a list of mandatory sections.
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
        /// <param name="section">The section name.</param>
        /// <returns>A IniSectionReaderBase instance whether section was found; otherwise, null.</returns>
        protected IniSectionReaderBase GetSectionByName(string section)
        {
            foreach (IniSectionReaderBase item in sections) {
                if (item.SectionName == section)
                    return item;
            }

            return null;
        }

        /// <summary>
        /// Gets a new instance IniSectionReaderBase class based on section name.
        /// </summary>
        /// <param name="section">The section name.</param>
        /// <returns>A new instance of IniSectionReaderBase class.</returns>
        protected abstract IniSectionReaderBase GetSectionInstance(string section);

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
        /// Reads configuration file and populates current instance.
        /// </summary>
        /// <exception cref="ArgumentNullException"><c>fileName</c> is a null reference.</exception>
        /// <exception cref="System.IO.FileNotFoundException">The specifield file was not found.</exception>
        /// <exception cref="System.IO.FileLoadException">The current file is invalid.</exception>
        public virtual void LoadFile()
        {
            if (cfgreader == null)
                cfgreader = new SklLib.IO.IniFileReader(filename);

            cfgreader.ReloadFile();
            string[] sectionsName = cfgreader.ReadSectionsName();
            sections = new IniSectionReaderBase[sectionsName.Length];

            for (int i = 0; i < sectionsName.Length; i++) {
                sections[i] = GetSectionInstance(sectionsName[i]);
            }
        }

        #endregion

        #region IValidatable

        /// <summary>
        /// Validates each section from current instance and executes a action.
        /// </summary>
        /// <param name="action">Action to execute after each validation.</param>
        /// <returns>True whether all sections are valid; otherwise, false.</returns>
        public virtual bool Validate(Action<InvalidEventArgs> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            bool result = true;
            if (MandatorySections != null
                && MandatorySections.Length > 0) {
                foreach (string item in MandatorySections) {
                    if (!HasSection(item)) {
                        result = false;
                        action(new InvalidEventArgs(
                            string.Format(MESSAGE_SECTION_MANDATORY_MISSING, item),
                            "MandatorySections", item));
                    }
                }
            }

            foreach (IniSectionReaderBase item in sections) {
                if (!item.Validate(action))
                    result = false;
            }

            return result;
        }

        #endregion
    }
}
