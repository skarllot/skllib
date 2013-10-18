// ConfigFileReader.cs
//
//  Copyright (C) 2008-2013 Fabrício Godoy
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
using Generics = System.Collections.Generic;
using SIO = System.IO;

namespace SklLib.IO
{
    /// <summary>
    /// Provides methods to read configuration files.
    /// </summary>
    public class ConfigFileReader : ConfigFileBase
    {
        #region Constructors
        
        /// <summary>
        /// Initilizes a new ConfigFileReader object pointed to specified file name.
        /// </summary>
        /// <param name="fileName">The file name to reads configurations.</param>
        /// <exception cref="ArgumentNullException"><c>fileName</c> is a null reference.</exception>
        /// <exception cref="SIO.FileNotFoundException">The specifield file was not found.</exception>
        public ConfigFileReader(string fileName)
            : this(fileName, ConfigFileBase.DefaultEncoding)
        {
        }

        /// <summary>
        /// Initilizes a new ConfigFileReader object pointed to specified file name and encoding.
        /// </summary>
        /// <param name="fileName">The file name to reads configurations.</param>
        /// <param name="encoding">Encoding of configuration file.</param>
        /// <exception cref="ArgumentNullException"><c>fileName</c> is a null reference.</exception>
        /// <exception cref="SIO.FileNotFoundException">The specifield file was not found.</exception>
        public ConfigFileReader(string fileName, System.Text.Encoding encoding)
            : base(fileName, encoding)
        {
            if (!SIO.File.Exists(fileName))
                throw new SIO.FileNotFoundException(resExceptions.FileNotFound.Replace("%var", fileName), fileName);

            ReadFile();
        }

        #endregion

        #region Methods
        
        /// <summary>
        /// Reads the value into specifield key.
        /// </summary>
        /// <param name="section">The section where key is found.</param>
        /// <param name="key">The key name.</param>
        /// <returns>Value stored into key.</returns>
        /// <exception cref="ArgumentNullException">section or key parameter is a null reference.</exception>
        /// <exception cref="SectionNotFoundException">section was not found.</exception>
        /// <exception cref="KeyNotFoundException">key was not found.</exception>
        /// <exception cref="SIO.FileLoadException">The key has a invalid value.</exception>
        public string ReadValue(string section, string key)
        {
            if (section == null)
                throw new ArgumentNullException("section", resExceptions.ArgumentNull.Replace("%var", "section"));
            if (key == null)
                throw new ArgumentNullException("key", resExceptions.ArgumentNull.Replace("%var", "key"));

            int keyIndex = FindKey(section, key);
            if (keyIndex == -1)
                throw new KeyNotFoundException(resExceptions.KeyNotFound.Replace("%var", key));

            return _buffer[keyIndex][1];
        }

        /// <summary>
        /// Reads all sections name.
        /// </summary>
        /// <returns>All sections name read.</returns>
        public string[] ReadSectionsName()
        {
            int len = _sectionBuffer.Count;
            string[] sects = new string[len];

            for (int i = 0; i < len; i++)
                sects[i] = _buffer[_sectionBuffer[i]][0];

            return sects;
        }

        /// <summary>
        /// Reads all keys and values into specifield section.
        /// </summary>
        /// <param name="section">The section where keys are found.</param>
        /// <returns>All keys and values stored into section.</returns>
        /// <exception cref="ArgumentNullException">section parameter is a null reference.</exception>
        /// <exception cref="SectionNotFoundException">section was not found.</exception>
        public Generics.KeyValuePair<string, string>[] ReadKeysValues(string section)
        {
            if (section == null)
                throw new ArgumentNullException("section", resExceptions.ArgumentNull.Replace("%var", "section"));

            int idx = 0, count = 0;
            if (!base.FindRange(section, out idx, out count))
                throw new SectionNotFoundException(resExceptions.SectionNotFound.Replace("%var", section));

            if (count == 0)
                return new Generics.KeyValuePair<string, string>[0];
            Generics.KeyValuePair<string, string>[] ret =
                new Generics.KeyValuePair<string, string>[count];

            for (int i = 0; i < count - 1; i++)
            {
                idx++;
                ret[i] = new Generics.KeyValuePair<string, string>(
                    _buffer[idx][0], _buffer[idx][1]);
            }

            return ret;
        }

        /// <summary>
        /// Clears any buffered data and reloads them again.
        /// </summary>
        public void ReloadFile()
        {
            base.ReadFile();
        }
        
        #endregion
    }
    
}
