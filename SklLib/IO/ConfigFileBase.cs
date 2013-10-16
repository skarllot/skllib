// ConfigFileBase.cs
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
using stringb = System.Text.StringBuilder;
using SIO = System.IO;
using StringList = System.Collections.Generic.List<string>;
using Int32List = System.Collections.Generic.List<int>;

namespace SklLib.IO
{
    /// <summary>
    /// Provides base methods to work with configuration files.
    /// </summary>
    public abstract class ConfigFileBase
    {
        #region Fields

        /// <summary>
        /// Stores the default encoding used by this class.
        /// </summary>
        protected static readonly System.Text.Encoding DefaultEncoding;
        
        /// <summary>
        /// Stores a String array where each item is a line from config file.
        /// </summary>
        protected StringList _buffer;
        /// <summary>
        /// Character encoding to read and write config file.
        /// </summary>
        protected System.Text.Encoding _encoding;
        /// <summary>
        /// Stores config file name.
        /// </summary>
        protected string _fileName;
        /// <summary>
        /// Stores a Int32 array where each item is a index to a section in config file.
        /// </summary>
        protected Int32List _sectionBuffer;

        #endregion

        #region Constructors
        
        /// <summary>
        /// Initializes static fields.
        /// </summary>
        static ConfigFileBase()
        {
            DefaultEncoding = System.Text.Encoding.UTF8;
        }
        
        /// <summary>
        /// Initializes a new ConfigFileBase.
        /// </summary>
        /// <param name="fileName">The file name to handle configurations.</param>
        protected ConfigFileBase(string fileName)
            : this(fileName, DefaultEncoding)
        {
        }
        
        /// <summary>
        /// Initializes a new ConfigFileBase.
        /// </summary>
        /// <param name="fileName">The file name to handle configurations.</param>
        /// <param name="encoding">Encoding of configuration file.</param>
        protected ConfigFileBase(string fileName, System.Text.Encoding encoding)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName", resExceptions.ArgumentNull.Replace("%var", "fileName"));
            
            this._encoding = encoding;
            this._fileName = fileName;
            
            this.SetBasicInfo(this._fileName);
        }
        
        #endregion
        
        #region Properties

        /// <summary>
        /// Gets the file name used by this instance.
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
        }

        #endregion

        #region Methods
        
        /// <summary>
        /// Check whether indicated file is valid configuration file.
        /// </summary>
        /// <param name="fileName">File to check validty.</param>
        /// <returns>True if fileName is a valid cofiguration file; otherwise, false.</returns>
        /// <exception cref="SIO.FileNotFoundException">The indicated file was not found.</exception>
        public static bool IsValidFile(string fileName)
        {
            return IsValidFile(fileName, ConfigFileBase.DefaultEncoding);
        }

        /// <summary>
        /// Check whether indicated file is valid configuration file.
        /// </summary>
        /// <param name="fileName">File to check validty.</param>
        /// <param name="encoding">Encoding to read file.</param>
        /// <returns>True if fileName is a valid cofiguration file; otherwise, false.</returns>
        /// <exception cref="SIO.FileNotFoundException">The indicated file was not found.</exception>
        public static bool IsValidFile(string fileName, System.Text.Encoding encoding)
        {
            SIO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read,
                System.IO.FileShare.Read);
            SIO.StreamReader reader = new System.IO.StreamReader(fs, encoding);

            stringb str = new stringb();
            while (reader.Peek() != -1)
            {
                str.Remove(0, str.Length);
                str.Append(reader.ReadLine());

                if (str.Length == 0)
                    continue;
                else if (str.ToString(0, 1) == "[" && str.ToString(str.Length - 1, 1) == "]")
                {
                    if (!Strings.IsAlphabeticAndNumeric(str.ToString(1, str.Length - 2)))
                        goto error;
                    else
                        continue;
                }
                else if (str.ToString().IndexOf('=') != -1)
                {
                    int idx = str.ToString().IndexOf('=');
                    if (!Strings.IsAlphabeticAndNumeric(str.ToString(0, idx)))
                        goto error;
                    else
                        continue;
                }
                else
                    goto error;
            }

            reader.Close();
            return true;

        error:
            reader.Close();
            return false;
        }

        /// <summary>
        /// Search a key in current config file.
        /// </summary>
        /// <param name="key">Key name to search.</param>
        /// <param name="start">Line number to start search.</param>
        /// <param name="count">How many lines to search.</param>
        /// <returns>Line number where key is found; otherwise returns -1.</returns>
        protected int FindKey(string key, int start, int count)
        {
            int end = count + start;
            for (int i = start; i < end; i++)
            {
                if (_buffer[i].IndexOf(key) == 0
                    && _buffer[i].IndexOf('=') == key.Length)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Search a section in current config file.
        /// </summary>
        /// <param name="section">Section name to search.</param>
        /// <param name="index">Line number where is found section.</param>
        /// <param name="count">Line count from section.</param>
        /// <returns>True whether section is found; otherwise false.</returns>
        protected bool FindRange(string section, out int index, out int count)
        {
            section = "[" + section + "]";
            count = 0;
            index = -1;

            for (int i = 0; i < _sectionBuffer.Count; i++)
            {
                if (_buffer[_sectionBuffer[i]] == section)
                {
                    index = _sectionBuffer[i];
                    if (i + 1 == _sectionBuffer.Count)
                        count = _buffer.Count - index;
                    else
                        count = _sectionBuffer[i + 1] - index;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Reads the file and fills all buffers.
        /// </summary>
        protected void ReadFile()
        {
            SIO.FileStream fs = new System.IO.FileStream(_fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read,
                System.IO.FileShare.Read);
            SIO.StreamReader reader = new System.IO.StreamReader(fs, this._encoding, true);

            _buffer = new StringList();
            _sectionBuffer = new Int32List();
            int idx = 0;
            stringb str = new stringb();
            while (reader.Peek() != -1)
            {
                str.Remove(0, str.Length);
                str.Append(reader.ReadLine());

                if (str.Length == 0)
                    continue;

                _buffer.Add(str.ToString());
                if (str.ToString(0, 1) == "[")
                    _sectionBuffer.Add(idx);
                idx++;
            }

            reader.Close();
        }

        /// <summary>
        /// Sets full path for file name and verifies directory existence.
        /// </summary>
        /// <param name="fileName">The configuration file.</param>
        protected void SetBasicInfo(string fileName)
        {
            _fileName = SIO.Path.GetFullPath(fileName);

            string dir = SIO.Path.GetDirectoryName(_fileName);
            if (!SIO.Directory.Exists(dir))
                throw new SIO.DirectoryNotFoundException(dir);
        }

        #endregion
    }

}
