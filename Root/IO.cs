using System;
using stringb = System.Text.StringBuilder;
using DllImport = System.Runtime.InteropServices.DllImportAttribute;
using SIO = System.IO;
using StringList = System.Collections.Generic.List<string>;
using Int32List = System.Collections.Generic.List<int>;
using Generics = System.Collections.Generic;

namespace Root.IO
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
			SIO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read,
				System.IO.FileShare.Read);
			SIO.StreamReader reader = new System.IO.StreamReader(fs);

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
			: base(fileName)
		{
			if (SIO.File.Exists(_fileName))
				ReadFile();
		}

		public ConfigFileWriter(string fileName, System.Text.Encoding encoding)
			: this(fileName, SIO.FileMode.Create, ConfigFileBase.DefaultEncoding)
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
		public void ClearSection(string section, bool remSection)
		{
			if (section == null)
				throw new ArgumentNullException("section", resExceptions.ArgumentNull.Replace("%var", "section"));

			int index, count;
			if (!FindRange(section, out index, out count))
				throw new Exception(resExceptions.SectionNotFound.Replace("%var", section));

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
					writer.WriteLine();		// blank line before a section.
				
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
		/// <exception cref="Exception">section or key was not found.</exception>
		public void DeleteKey(string section, string key)
		{
			if (section == null)
				throw new ArgumentNullException("section", resExceptions.ArgumentNull.Replace("%var", "section"));
			if (key == null)
				throw new ArgumentNullException("key", resExceptions.ArgumentNull.Replace("%var", "key"));

			int index, count;
			if (!FindRange(section, out index, out count))
				throw new Exception(resExceptions.SectionNotFound.Replace("%var", section));

			int keyIndex = FindKey(key, index + 1, count);
			if (keyIndex == -1)
				throw new Exception(resExceptions.KeyNotFound.Replace("%var", key));

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
			: base(fileName)
		{
			if (fileName == null)
				throw new ArgumentNullException("fileName", resExceptions.ArgumentNull.Replace("%var", "fileName"));
			if (!SIO.File.Exists(fileName))
				throw new SIO.FileNotFoundException(resExceptions.FileNotFound.Replace("%var", fileName), fileName);

			SetBasicInfo(fileName);
			ReadFile();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Reads the value into specifield key [Deprecated].
		/// </summary>
		/// <param name="section">The section where key is found.</param>
		/// <param name="key">The key name.</param>
		/// <returns>Value stored into key.</returns>
		/// <exception cref="ArgumentNullException">section or key parameter is a null reference.</exception>
		/// <exception cref="Exception">section or key was not found.</exception>
		/// <exception cref="SIO.FileLoadException">The key has a invalid value.</exception>
		[Obsolete("ReadKey is deprecated, use ReadValue instead.", true)]
		public string ReadKey(string section, string key)
		{
			return this.ReadValue(section, key);
		}
		
		/// <summary>
		/// Reads the value into specifield key.
		/// </summary>
		/// <param name="section">The section where key is found.</param>
		/// <param name="key">The key name.</param>
		/// <returns>Value stored into key.</returns>
		/// <exception cref="ArgumentNullException">section or key parameter is a null reference.</exception>
		/// <exception cref="Exception">section or key was not found.</exception>
		/// <exception cref="SIO.FileLoadException">The key has a invalid value.</exception>
		public string ReadValue(string section, string key)
		{
			if (section == null)
				throw new ArgumentNullException("section", resExceptions.ArgumentNull.Replace("%var", "section"));
			if (key == null)
				throw new ArgumentNullException("key", resExceptions.ArgumentNull.Replace("%var", "key"));

			int index, count;
			if (!FindRange(section, out index, out count))
				throw new Exception(resExceptions.SectionNotFound.Replace("%var", section));

			int keyIndex = FindKey(key, index + 1, count);
			if (keyIndex == -1)
				throw new Exception(resExceptions.KeyNotFound.Replace("%var", key));

			string str = _buffer[keyIndex];
			int dIdx = str.IndexOf('=');
			if (dIdx == -1 || dIdx == str.Length - 1)
				throw new SIO.FileLoadException(resExceptions.InvalidFile.Replace("%var", _fileName), _fileName);

			return str.Substring(dIdx + 1);
		}

		/// <summary>
		/// Reads all sections name.
		/// </summary>
		/// <returns>All sections name read.</returns>
		public string[] ReadSectionsName()
		{
			string[] sects = new string[base._sectionBuffer.Count];

			for (int i = 0; i < base._sectionBuffer.Count; i++)
			{
				sects[i] = base._buffer[base._sectionBuffer[i]];
				sects[i] = sects[i].Substring(1, sects[i].Length - 2);
			}

			return sects;
		}

		/// <summary>
		/// Reads all keys and values into specifield section.
		/// </summary>
		/// <param name="section">The section where keys are found.</param>
		/// <returns>All keys and values stored into section.</returns>
		public Generics.KeyValuePair<string, string>[] ReadKeysValues(string section)
		{
			int idx = 0, count = 0;
			if (!base.FindRange(section, out idx, out count))
				throw new Exception(resExceptions.SectionNotFound.Replace("%var", section));

			if (count == 0)
				return new Generics.KeyValuePair<string, string>[0];
			Generics.KeyValuePair<string, string>[] ret =
				new Generics.KeyValuePair<string, string>[count];

			string[] keyValue;
			for (int i = 0; i < count - 1; i++)
			{
				idx++;
				keyValue = base._buffer[idx].Split('=');
				
				ret[i] = new Generics.KeyValuePair<string, string>(
					keyValue[0], keyValue[1]);
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
