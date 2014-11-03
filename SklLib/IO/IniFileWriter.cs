// IniFileWriter.cs
//
//  Copyright (C) 2008-2014 Fabrício Godoy
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
    /// Provides methods to write INI files.
    /// </summary>
    public class IniFileWriter : IniFileBase
    {
        #region Constructors

        /// <summary>
        /// Initilizes a new IniFileWriter object pointed to specified file name.
        /// </summary>
        /// <param name="fileName">The INI file name.</param>
        /// <exception cref="ArgumentNullException"><c>fileName</c> is a null reference.</exception>
        /// <exception cref="SIO.DirectoryNotFoundException">The specifield directory was not found.</exception>
        public IniFileWriter(string fileName)
            : this(fileName, IniFileBase.DefaultEncoding)
        {
        }

        /// <summary>
        /// Initilizes a new IniFileWriter object pointed to specified file name and encoding.
        /// </summary>
        /// <param name="fileName">The INI file name.</param>
        /// <param name="encoding">Encoding of INI file.</param>
        public IniFileWriter(string fileName, System.Text.Encoding encoding)
            : base(fileName, encoding)
        {
            if (SIO.File.Exists(_fileName))
                ReadFile();
        }
        
        /// <summary>
        /// Initilizes a new IniFileWriter object pointed to specified file name.
        /// </summary>
        /// <param name="fileName">The INI file name.</param>
        /// <param name="mode">Specifies how the specified file should be open.</param>
        /// <exception cref="ArgumentNullException">fileName is a null reference.</exception>
        /// <exception cref="SIO.DirectoryNotFoundException">The specifield directory was not found.</exception>
        /// <exception cref="SIO.IOException">Was specifield <see cref="SIO.FileMode.CreateNew"/> flag and the
        /// specifield file already exists.</exception>
        /// <exception cref="SIO.FileNotFoundException">Was specifield <see cref="SIO.FileMode.Open"/> flag and the
        /// specifield file already was not found.</exception>
        public IniFileWriter(string fileName, SIO.FileMode mode)
            : this(fileName, mode, IniFileBase.DefaultEncoding)
        {
        }
        
        /// <summary>
        /// Initilizes a new IniFileWriter object pointed to specified file name.
        /// </summary>
        /// <param name="fileName">The file name to writes configurations.</param>
        /// <param name="mode">Specifies how the specified file should be open.</param>
        /// <param name="encoding">Encoding of INI file.</param>
        /// <exception cref="ArgumentNullException">fileName is a null reference.</exception>
        /// <exception cref="SIO.DirectoryNotFoundException">The specifield directory was not found.</exception>
        /// <exception cref="SIO.IOException">Was specifield <see cref="SIO.FileMode.CreateNew"/> flag and the
        /// specifield file already exists.</exception>
        /// <exception cref="SIO.FileNotFoundException">Was specifield <see cref="SIO.FileMode.Open"/> flag and the
        /// specifield file already was not found.</exception>
        public IniFileWriter(string fileName, SIO.FileMode mode, System.Text.Encoding encoding)
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
            SIO.FileStream fs = new System.IO.FileStream(_fileName, System.IO.FileMode.Create,
                System.IO.FileAccess.Write, System.IO.FileShare.Read);
            SIO.StreamWriter writer = new System.IO.StreamWriter(fs, _encoding);
            writer.AutoFlush = true;

            bool first = true;
            foreach (string[] item in _buffer)
            {
                // Is section
                if (item.Length == 1)
                {
                    if (!first)
                        writer.WriteLine();     // blank line before a section.
                    writer.WriteLine(_secPrefix + item[0] + _secSuffix);
                }
                else // Is key/value
                {
                    writer.WriteLine(item[0] + _keyValueSep + item[1]);
                }
                first = false;
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
        /// Writes a value to specifield key.
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

            if (!idMatcher.IsMatch(section))
                throw new ArgumentException(resExceptions.InvalidChar_Section.Replace("%var", section));

            if (!idMatcher.IsMatch(key))
                throw new ArgumentException(resExceptions.InvalidChar_Key.Replace("%var", key));

            if (value.HasControlChar())
                throw new ArgumentException(resExceptions.InvalidChar_Value.Replace("%var", value));

            int index, count;
            // Section not found, then creates new
            if (!FindRange(section, out index, out count))
            {
                _sectionBuffer.Add(_buffer.Count);
                _buffer.Add(new string[] { section });
                _buffer.Add(new string[] { key, value });
            }
            else
            {
                int keyIndex = FindKey(key, index + 1, count);
                // Key not found, then creates new
                if (keyIndex == -1)
                {
                    _buffer.Insert(index + count, new string[] { key, value });

                    int bIdx = _sectionBuffer.IndexOf(index);
                    for (int i = bIdx + 1; i < _sectionBuffer.Count; i++)
                        _sectionBuffer[i] += 1;
                }
                else
                    _buffer[keyIndex][1] = value;
            }
        }

        #endregion
    }
}
