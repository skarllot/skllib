// IniFileBase.cs
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
using stringb = System.Text.StringBuilder;
using SIO = System.IO;
using StrArrList = System.Collections.Generic.List<string[]>;
using Int32List = System.Collections.Generic.List<int>;
using System.Text.RegularExpressions;

namespace SklLib.IO
{
    /// <summary>
    /// Provides base methods to work with INI files.
    /// </summary>
    /// <remarks>
    /// See http://en.wikipedia.org/wiki/INI_file and
    /// https://code.google.com/p/minini/wiki/INI_File_Syntax for more details.
    /// </remarks>
    public abstract class IniFileBase : IValidatable
    {
        #region Fields

        /// <summary>
        /// Stores the default character that indicates beginning of a comment.
        /// </summary>
        protected const string DEFAULT_COMMENT = ";";

        /// <summary>
        /// Stores the default encoding used by this class.
        /// </summary>
        protected static readonly System.Text.Encoding DefaultEncoding;

        /// <summary>
        /// Defines the default regex rule to identifiers.
        /// </summary>
        protected const string DEFAULT_ID_RULE = @"^[A-Za-z]+[A-Za-z0-9_]*$";

        /// <summary>
        /// Stores the default separator between key and value.
        /// </summary>
        protected const string DEFAULT_KEY_VALUE_SEP = "=";

        /// <summary>
        /// Stores the default prefix that identifies sections.
        /// </summary>
        protected const string DEFAULT_SEC_PREFIX = "[";

        /// <summary>
        /// Stores the default suffix that identifies sections.
        /// </summary>
        protected const string DEFAULT_SEC_SUFFIX = "]";

        /// <summary>
        /// Regex matcher to identifiers.
        /// </summary>
        protected static Regex idMatcher;
        
        /// <summary>
        /// Stores a String array where each item is a line from config file.
        /// </summary>
        protected StrArrList _buffer;
        /// <summary>
        /// Defines if file lines, key names and key values should be trimmed.
        /// </summary>
        protected bool _canTrim;
        /// <summary>
        /// Character that indicates beginning of a comment.
        /// </summary>
        protected string _comment;
        /// <summary>
        /// Character encoding to read and write config file.
        /// </summary>
        protected System.Text.Encoding _encoding;
        /// <summary>
        /// Stores config file name.
        /// </summary>
        protected string _fileName;
        /// <summary>
        /// Separator between key and value.
        /// </summary>
        protected string _keyValueSep;
        /// <summary>
        /// Prefix that identifies sections.
        /// </summary>
        protected string _secPrefix;
        /// <summary>
        /// Suffix that identifies sections.
        /// </summary>
        protected string _secSuffix;
        /// <summary>
        /// Stores a Int32 array where each item is a index to a section in config file.
        /// </summary>
        protected Int32List _sectionBuffer;

        #endregion

        #region Constructors
        
        /// <summary>
        /// Initializes static fields.
        /// </summary>
        static IniFileBase()
        {
            DefaultEncoding = System.Text.Encoding.UTF8;
            idMatcher = new Regex(DEFAULT_ID_RULE, RegexOptions.Compiled);
        }
        
        /// <summary>
        /// Initializes a new IniFileBase.
        /// </summary>
        /// <param name="fileName">The INI file name.</param>
        /// <exception cref="ArgumentNullException"><c>fileName</c> is a null reference.</exception>
        protected IniFileBase(string fileName)
            : this(fileName, DefaultEncoding)
        {
        }
        
        /// <summary>
        /// Initializes a new IniFileBase.
        /// </summary>
        /// <param name="fileName">The INI file name.</param>
        /// <param name="encoding">Encoding of INI file.</param>
        /// <exception cref="ArgumentNullException"><c>fileName</c> is a null reference.</exception>
        /// <exception cref="SIO.DirectoryNotFoundException">Directory defined to file was not found.</exception>
        protected IniFileBase(string fileName, System.Text.Encoding encoding)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName", resExceptions.ArgumentNull.Replace("%var", "fileName"));

            this._canTrim = true;
            this._encoding = encoding;
            this._fileName = fileName;
            this._secPrefix = DEFAULT_SEC_PREFIX;
            this._secSuffix = DEFAULT_SEC_SUFFIX;
            this._keyValueSep = DEFAULT_KEY_VALUE_SEP;
            this._comment = DEFAULT_COMMENT;
            this._buffer = new StrArrList();
            this._sectionBuffer = new Int32List();
            
            this.SetBasicInfo(this._fileName);
        }
        
        #endregion
        
        #region Properties

        /// <summary>
        /// Gets or sets if file lines, key names and key values should be trimmed.
        /// </summary>
        public bool CanTrim
        {
            get { return _canTrim; }
            set { _canTrim = value; }
        }

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
        /// Check whether current file is valid INI file.
        /// </summary>
        /// <returns>True if current file is a valid INI file; otherwise, false.</returns>
        /// <exception cref="SIO.FileNotFoundException">The indicated file was not found.</exception>
        public bool IsValid()
        {
            return FillBuffer();
        }

        /// <summary>
        /// Reads the file, validate and fills all buffers.
        /// </summary>
        /// <returns>True if current file is a valid INI file; otherwise, false.</returns>
        /// <exception cref="SIO.FileNotFoundException">The indicated file was not found.</exception>
        protected bool FillBuffer()
        {
            SIO.FileStream fs = new System.IO.FileStream(_fileName,
                SIO.FileMode.Open, SIO.FileAccess.Read, SIO.FileShare.Read);
            SIO.StreamReader reader = new SIO.StreamReader(fs, this._encoding, true);
            Func<bool, bool> finalize = delegate(bool res)
            {
                reader.Close();
                reader.Dispose();

                if (!res)
                {
                    _buffer.Clear();
                    _sectionBuffer.Clear();
                }
                return res;
            };

            _buffer = new StrArrList();
            _sectionBuffer = new Int32List();
            int idx = 0;
            string str;
            while (reader.Peek() != -1)
            {
                str = reader.ReadLine();
                if (_canTrim)
                    str = str.Trim();

                // Blank lines are ignored
                if (str.Length == 0)
                    continue;
                int idxSec1 = str.IndexOf(_secPrefix);
                int idxSec2 = str.IndexOf(_secSuffix);
                int idxSep = str.IndexOf(_keyValueSep);
                bool isSec = (idxSec1 == 0) && (idxSec2 > 1);
                bool isKey = idxSep > 0;
                bool isComment = str.IndexOf(_comment) == 0;

                if (!isSec && !isKey && !isComment)
                    return finalize(false);

                if (isComment)
                    continue;
                else if (isSec)
                {
                    if (idxSec2 != str.Length - _secSuffix.Length)
                        return finalize(false);

                    string section = str.Substring(idxSec1 + _secPrefix.Length,
                        idxSec2 - (idxSec1 + _secPrefix.Length));
                    if (_canTrim)
                        section = section.Trim();

                    if (section.Length == 0 ||
                        !idMatcher.IsMatch(section))
                        return finalize(false);

                    _buffer.Add(new string[] { section });
                    _sectionBuffer.Add(idx);
                }
                else
                {
                    string key = str.Substring(0, idxSep);
                    string value = str.Substring(idxSep + _keyValueSep.Length);
                    if (_canTrim)
                    {
                        key = key.Trim();
                        value = value.Trim();
                    }

                    if (key.Length == 0 ||
                        !idMatcher.IsMatch(key))
                        return finalize(false);

                    _buffer.Add(new string[] { key, value });
                }
                idx++;
            }

            return finalize(true);
        }

        /// <summary>
        /// Search a key in the current file.
        /// </summary>
        /// <param name="key">Key name to search.</param>
        /// <param name="start">Item index to start search.</param>
        /// <param name="count">How many items to search.</param>
        /// <returns>Item index where key is found; otherwise returns -1.</returns>
        protected int FindKey(string key, int start, int count)
        {
            key = key.ToLower();
            int end = count + start;
            for (int i = start; i < end; i++)
            {
                if (_buffer[i].Length == 2
                    && _buffer[i][0].ToLower() == key)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Search a key in the current file.
        /// </summary>
        /// <param name="section">Section name to search.</param>
        /// <param name="key">Key name to search.</param>
        /// <returns>Line number where key is found; otherwise returns -1.</returns>
        /// <exception cref="SectionNotFoundException">Section was not found.</exception>
        protected int FindKey(string section, string key)
        {
            int index, count;
            if (!FindRange(section, out index, out count))
                throw new SectionNotFoundException(resExceptions.SectionNotFound.Replace("%var", section));

            return FindKey(key, index, count);
        }

        /// <summary>
        /// Search a section in the current file.
        /// </summary>
        /// <param name="section">Section name to search.</param>
        /// <param name="index">Line number where is found section.</param>
        /// <param name="count">Line count from section.</param>
        /// <returns>True whether section is found; otherwise false.</returns>
        protected bool FindRange(string section, out int index, out int count)
        {
            section = section.ToLower();
            count = 0;
            index = -1;

            for (int i = 0; i < _sectionBuffer.Count; i++)
            {
                if (_buffer[_sectionBuffer[i]][0].ToLower() == section)
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
        /// <exception cref="SIO.FileLoadException">The current file is invalid.</exception>
        /// <exception cref="SIO.FileNotFoundException">The indicated file was not found.</exception>
        protected void ReadFile()
        {
            if (!FillBuffer())
                throw new SIO.FileLoadException(resExceptions.InvalidFile.Replace("%var", _fileName));
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

        #region IValidatable

        /// <summary>
        /// Validates current file as a valid INI file and executes a action.
        /// </summary>
        /// <param name="action">Action to execute after each validation.</param>
        /// <returns>True if current file is a valid INI file; otherwise, false.</returns>
        public bool Validate(Action<InvalidEventArgs> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            bool result = FillBuffer();
            if (!result) {
                action(new InvalidEventArgs(
                    string.Format("The INI file '{0}' is invalid", _fileName),
                    "FileName", _fileName ?? string.Empty));
            }

            return result;
        }

        #endregion
    }

}
