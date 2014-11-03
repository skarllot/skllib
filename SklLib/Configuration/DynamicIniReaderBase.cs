// DynamicIniReaderBase.cs
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
using System.Collections.Generic;

namespace SklLib.Configuration
{
    /// <summary>
    /// Provides base methods to implement a class to represent a configuration file reader
    /// that has dynamic named sections.
    /// </summary>
    public abstract class DynamicIniReaderBase : IniReaderBase
    {
        #region Fields

        protected IniSectionReaderBase[] dynSections;
        protected IniSectionReaderBase[] staticSections;

        #endregion

        #region Properties

        /// <summary>
        /// Gets a list of sections name that is static defined.
        /// </summary>
        protected abstract string[] StaticNamedSections { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a section instance based on its index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>A instance of IniSectionReaderBase class.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The parameter index is less than zero or is out of array bounds.</exception>
        protected IniSectionReaderBase GetDynamicSection(int index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException("Parameter index cannot be less than zero.");
            if (index >= dynSections.Length)
                throw new ArgumentOutOfRangeException("Parameter index is out of array bounds.");

            return dynSections[index];
        }

        /// <summary>
        /// Reads configuration file and populates current instance.
        /// </summary>
        /// <exception cref="ArgumentNullException"><c>fileName</c> is a null reference.</exception>
        /// <exception cref="System.IO.FileNotFoundException">The specifield file was not found.</exception>
        /// <exception cref="System.IO.FileLoadException">The current file is invalid.</exception>
        public override void LoadFile()
        {
            base.LoadFile();

            string[] sNames = StaticNamedSections;
            List<IniSectionReaderBase> dSections = new List<IniSectionReaderBase>(sections.Length);
            List<IniSectionReaderBase> sSections = new List<IniSectionReaderBase>(sections.Length);
            foreach (IniSectionReaderBase item in sections) {
                if (Array.IndexOf<string>(sNames, item.SectionName) != -1)
                    sSections.Add(item);
                else
                    dSections.Add(item);
            }

            dynSections = dSections.ToArray();
            staticSections = sSections.ToArray();
        }

        #endregion
    }
}
