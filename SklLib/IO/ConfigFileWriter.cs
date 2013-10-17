// ConfigFileWriter.cs
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
using SIO = System.IO;

namespace SklLib.IO
{
    /// <summary>
    /// Provides methods to write into configuration files.
    /// </summary>
    public class ConfigFileWriter : ConfigFileBase
    {
        #region Constructors

        /// <summary>
        /// Initilizes a new ConfigFileWriter object pointed to specified file name.
        /// </summary>
        /// <param name="fileName">The file name to writes configurations.</param>
        /// <exception cref="ArgumentNullException"><c>fileName</c> is a null reference.</exception>
        /// <exception cref="SIO.DirectoryNotFoundException">The specifield directory was not found.</exception>
        public ConfigFileWriter(string fileName)
            : this(fileName, ConfigFileBase.DefaultEncoding)
        {
        }

        /// <summary>
        /// Initilizes a new ConfigFileWriter object pointed to specified file name and encoding.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="encoding"></param>
        public ConfigFileWriter(string fileName, System.Text.Encoding encoding)
            : base(fileName, encoding)
        {
            if (SIO.File.Exists(_fileName))
                ReadFile();
        }
        
        /// <summary>
        /// Initilizes a new ConfigFileWriter object pointed to specified file name.
        /// </summary>
        /// <param name="fileName">The file name to writes configurations.</param>
        /// <param name="mode">Specifies how the specified file should be open.</param>
        /// <exception cref="ArgumentNullException">fileName is a null reference.</exception>
        /// <exception cref="SIO.DirectoryNotFoundException">The specifield directory was not found.</exception>
        /// <exception cref="SIO.IOException">Was specifield <see cref="FileMode.CreateNew"/> flag and the
        /// specifield file already exists.</exception>
        /// <exception cref="SIO.FileNotFoundException">Was specifield <see cref="FileMode.Open"/> flag and the
        /// specifield file already was not found.</exception>
        public ConfigFileWriter(string fileName, SIO.FileMode mode)
            : this(fileName, mode, ConfigFileBase.DefaultEncoding)
        {
        }
        
        /// <summary>
        /// Initilizes a new ConfigFileWriter object pointed to specified file name.
        /// </summary>
        /// <param name="fileName">The file name to writes configurations.</param>
        /// <param name="mode">Specifies how the specified file should be open.</param>
        /// <exception cref="ArgumentNullException">fileName is a null reference.</exception>
        /// <exception cref="SIO.DirectoryNotFoundException">The specifield directory was not found.</exception>
        /// <exception cref="SIO.IOException">Was specifield <see cref="FileMode.CreateNew"/> flag and the
        /// specifield file already exists.</exception>
        /// <exception cref="SIO.FileNotFoundException">Was specifield <see cref="FileMode.Open"/> flag and the
        /// specifield file already was not found.</exception>
        public ConfigFileWriter(string fileName, SIO.FileMode mode, System.Text.Encoding encoding)
            : base(fileName, encoding)
        {
            bool exists = SIO.File.Exists(_fileName);

            if (mode == SIO.FileMode.CreateNew && exists)
                throw new SIO.IOException(resExceptions.FileExists.Replace("%var", _fileName));
            else if ((mode == SIO.FileMode.Create || mode == SIO.FileMode.Truncate) && exists)
            {
                SIO.File.Delete(_fileName);
                SIO.StreamWriter sw = new SIO.StreamWriter(_fileName, false, _encoding);
                sw.Close();
                sw.Dispose();
                exists = false;
            }
            else if (mode == SIO.FileMode.Open && !exists)
                throw new SIO.FileNotFoundException(resExceptions.FileNotFound.Replace("%var", _fileName), _fileName);

            if (exists)
                ReadFile();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clears entire section, including all entries within the section.
        /// </summary>
        /// <param name="section">The section name.</param>
        /// <param name="remSection">Specifies whether should remove the section itself.</param>
        /// <exception cref="ArgumentNullException">section parameter is a null reference.</exception>
        /// <exception cref="SectionNotFoundException">section was not found.</exception>
        public void ClearSection(string section, bool remSection)
        {
            if (section == null)
                throw new ArgumentNullException("section", resExceptions.ArgumentNull.Replace("%var", "section"));

            int index, count;
            if (!FindRange(section, out index, out count))
                throw new SectionNotFoundException(resExceptions.SectionNotFound.Replace("%var", section));

            _buffer.RemoveRange(index, count);

            int bIdx = _sectionBuffer.IndexOf(index);
            _sectionBuffer.RemoveAt(bIdx);
            for (int i = bIdx; i < _sectionBuffer.Count; i++)
                _sectionBuffer[i] -= count;
        }

        /// <summary>
        /// Writes all changes to file pointed by this instance.
        /// </summary>
        public void WriteChanges()
        {
            SIO.FileStream fs = new System.IO.FileStream(_fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write,
                System.IO.FileShare.Read);
            SIO.StreamWriter writer = new System.IO.StreamWriter(fs, _encoding);
            writer.AutoFlush = true;

            for (int i = 0; i < _buffer.Count; i++)
            {
                if (i != 0 && _sectionBuffer.Contains(i))
                    writer.WriteLine();        // blank line before a section.
                
                writer.WriteLine(_buffer[i]);
            }

            writer.Close();
            writer.Dispose();
        }

        /// <summary>
        /// Deletes the specifield key.
        /// </summary>
        /// <param name="section">The section name where key is found.</param>
        /// <param name="key">The key name.</param>
        /// <exception cref="ArgumentNullException">section or key parameter is a null reference.</exception>
        /// <exception cref="SectionNotFoundException">section was not found.</exception>
        /// <exception cref="KeyNotFoundException">key was not found.</exception>
        public void DeleteKey(string section, string key)
        {
            if (section == null)
                throw new ArgumentNullException("section", resExceptions.ArgumentNull.Replace("%var", "section"));
            if (key == null)
                throw new ArgumentNullException("key", resExceptions.ArgumentNull.Replace("%var", "key"));

            int index, count;
            if (!FindRange(section, out index, out count))
                throw new SectionNotFoundException(resExceptions.SectionNotFound.Replace("%var", section));

            int keyIndex = FindKey(key, index + 1, count);
            if (keyIndex == -1)
                throw new KeyNotFoundException(resExceptions.KeyNotFound.Replace("%var", key));

            _buffer.RemoveAt(keyIndex);

            int bIdx = base._sectionBuffer.IndexOf(index);
            for (int i = bIdx + 1; i < _sectionBuffer.Count; i++)
                _sectionBuffer[i] -= 1;
        }

        /// <summary>
        /// Writes a value into specifield key.
        /// </summary>
        /// <param name="section">The section where key is found.</param>
        /// <param name="key">The key name.</param>
        /// <param name="value">The key value.</param>
        /// <exception cref="ArgumentNullException">section, key or value parameter is a null reference.</exception>
        /// <exception cref="ArgumentException">section, key or value has invalid characters.</exception>
        public void WriteKey(string section, string key, string value)
        {
            if (section == null)
                throw new ArgumentNullException("section", resExceptions.ArgumentNull.Replace("%var", "section"));
            if (key == null)
                throw new ArgumentNullException("key", resExceptions.ArgumentNull.Replace("%var", "key"));
            if (value == null)
                throw new ArgumentNullException("value", resExceptions.ArgumentNull.Replace("%var", "value"));

            if (!Strings.IsAlphabeticAndNumeric(section))
                throw new ArgumentException(resExceptions.InvalidChar_Section.Replace("%var", section));

            if (!Strings.IsAlphabeticAndNumeric(key))
                throw new ArgumentException(resExceptions.InvalidChar_Key.Replace("%var", key));

            if (Strings.HasControlChar(value))
                throw new ArgumentException(resExceptions.InvalidChar_Value.Replace("%var", value));

            string keyLine = key + "=" + value;

            int index, count;
            // Section not found, then creates new
            if (!FindRange(section, out index, out count))
            {
                _sectionBuffer.Add(_buffer.Count);
                _buffer.Add("[" + section + "]");
                _buffer.Add(keyLine);
                return;
            }

            int keyIndex = FindKey(key, index + 1, count);
            if (keyIndex == -1)
            {
                _buffer.Insert(index + count, key + "=" + value);

                int bIdx = _sectionBuffer.IndexOf(index);
                for (int i = bIdx + 1; i < _sectionBuffer.Count; i++)
                    _sectionBuffer[i] += 1;
            }
            else
                _buffer[keyIndex] = key + "=" + value;
        }

        #endregion
    }
}
