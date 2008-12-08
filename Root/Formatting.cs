// Formatting.cs
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
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Serialization = System.Runtime.Serialization;

namespace Root.Formatting
{
	/// <summary>
	/// Provides base methods to format any <see cref="String"/>.
	/// </summary>
	public class FormatStringBase : IMaskeable, IWriteProtected<FormatStringBase>
	{
		#region Fields

		/// <summary>
		/// Used by <see cref="FormatStringBase"/> class to stores standard expression <see cref="String"/>.
		/// </summary>
		protected string expr;
		/// <summary>
		/// Used by <see cref="FormatStringBase"/> class to stores a <see cref="Regex"/> to validade
		/// standard expressions.
		/// </summary>
		protected Regex regex;
		/// <summary>
		/// Used by <see cref="FormatStringBase"/> class to stores compact expression <see cref="String"/>.
		/// </summary>
		protected string cExpr;
		/// <summary>
		/// Used by <see cref="FormatStringBase"/> class to stores a <see cref="Regex"/> to validade
		/// compact expressions.
		/// </summary>
		protected Regex cRegex;
		/// <summary>
		/// Used by <see cref="FormatStringBase"/> class to stores a <see cref="Strings.IndexedChar"/> <see cref="Array"/>
		/// to indicate additional chars of standard expresion, comparing with compact expression.
		/// </summary>
		protected Strings.IndexedChar[] remChars;
		/// <summary>
		/// Used by <see cref="FormatStringBase"/> class to indicate whether this instance is read-only.
		/// </summary>
		protected bool isReadOnly;
		/// <summary>
		/// Used by <see cref="FormatStringBase"/> class to stores a mask <see cref="String"/>.
		/// </summary>
		protected string mask;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new writable instance of the <see cref="FormatStringBase"/> class.
		/// </summary>
		/// <remarks>
		/// The properties of the new instance can be modified if you want user-defined formatting.
		/// </remarks>
		public FormatStringBase() { isReadOnly = false; }

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets a regular expression pattern to match a Standard format.
		/// </summary>
		/// <exception cref="ArgumentNullException">The property is being set to a null
		/// reference.</exception>
		/// <exception cref="InvalidOperationException">The property is being set and this
		/// instance is read-only.</exception>
		/// <value>A valid pattern to use in <see cref="System.Text.RegularExpressions.Regex"/>
		/// class.</value>
		/// <example>
		/// The following code example demonstrates the Expression property.
		/// <code>
		/// // Initialize a new writable instance.
		/// FormatStringBase fStr = new FormatStringBase();
		/// // Defines a personalized pattern to match an string.
		/// fStr.Expression = @"^[A-F]{3}_\d{6}[JK]$";
		/// // One example of a valid string in this Expression is "CAE_842947K".
		/// </code>
		/// </example>
		public virtual string Expression
		{
			set
			{
				if (value == null)
					throw new ArgumentNullException(resExceptions.PropertyNull.Replace("%var", "Expression"));
				VerifyWritable();
				regex = new Regex(value);
				expr = value; mask = null;
			}
			get { return expr; }
		}

		/// <summary>
		/// Gets or sets a regular expression pattern to match a compact version of
		/// <see cref="Expression"/> property.
		/// </summary>
		/// <exception cref="ArgumentNullException">The property is being set to a null
		/// reference.</exception>
		/// <exception cref="InvalidOperationException">The property is being set and the
		/// <see cref="PostalCode"/> is read-only.</exception>
		/// <value>A valid pattern to use in <see cref="System.Text.RegularExpressions.Regex"/>
		/// class.</value>
		/// <example>
		/// The following code example demonstrates the CompactExpression property.
		/// <code>
		/// // Initialize a new writable instance.
		/// FormatStringBase fStr = new FormatStringBase();
		/// // Defines a personalized pattern to match a compacted version of Expression string.
		/// fStr.CompactExpression = @"^[A-F]{3}\d{6}[JK]$";
		/// // One example of a valid string in this CompactExpression is "CAE842947K".
		/// </code>
		/// </example>
		public virtual string CompactExpression
		{
			set
			{
				if (value == null)
					throw new ArgumentNullException(resExceptions.PropertyNull.Replace("%var", "CompactExpression"));
				VerifyWritable();
				cRegex = new Regex(value);
				cExpr = value;
			}
			get { return cExpr; }
		}

		/// <summary>
		/// Gets or sets a Array of <see cref="Strings.IndexedChar"/> struct that indicates additional
		/// chars of Standard format, comparing with compact expression.
		/// </summary>
		/// <exception cref="ArgumentNullException">The property is being set to a null
		/// reference.</exception>
		/// <exception cref="InvalidOperationException">The property is being set and this
		/// instance is read-only.</exception>
		/// <value>A Array of <see cref="Strings.IndexedChar"/> struct</value>
		/// <example>
		/// The following code example demonstrates the RemovableChars property.
		/// <code>
		/// // Initialize a new writable instance.
		/// FormatStringBase fStr = new FormatStringBase();
		/// 
		/// // Defines a personalized pattern to match a string.
		/// fStr.Expression = @"^[A-F]{3}_\d{6}-[JK]$";
		/// // One example of a valid string in this Expression is "CAE_842947-J".
		/// 
		/// // Defines a personalized pattern to match a compacted version of Expression string.
		/// fStr.CompactExpression = @"^[A-F]{3}\d{6}[JK]$";
		/// // One example of a valid string in this CompactExpression is "CAE842947J".
		/// 
		/// // Defines aditional chars in the standard format.
		/// Strings.IndexedChar[] idxCh = new Strings.IndexedChar[2]; // has two aditional characters.
		/// idxCh[0] = new Strings.IndexedChar('_', 3);
		/// idxCh[1] = new Strings.IndexedChar('-', 10);
		/// fStr.RemovableChars = idxCh;
		/// </code>
		/// </example>
		public virtual Strings.IndexedChar[] RemovableChars
		{
			get { return remChars; }
			set
			{
				if (value == null)
					throw new ArgumentNullException(resExceptions.PropertyNull.Replace("%var", "RemovableChars"));
				VerifyWritable();
				remChars = value;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Indicates whether the <see cref="CompactExpression"/> finds a match in the input <see cref="String"/>.
		/// </summary>
		/// <param name="input">The string to search for a match.</param>
		/// <returns>true if the input string is valid; otherwise, false.</returns>
		/// <exception cref="ArgumentNullException"><c>input</c> is a null reference.</exception>
		/// <exception cref="InvalidOperationException"><see cref="CompactExpression"/> was not set.</exception>
		public bool CompactIsMatch(string input)
		{
			if (input == null)
				throw new ArgumentNullException(resExceptions.ArgumentNull.Replace("%var", "input"));
			if (cRegex == null)
				throw new InvalidOperationException(resExceptions.Invalid_PropNoSet.Replace("%var", "CompactExpression"));
			return cRegex.IsMatch(input);
		}

		/// <summary>
		/// Provides a <see cref="String"/> in standard format of compact format, generally used to show to user.
		/// </summary>
		/// <param name="input">A compact format.</param>
		/// <returns>A standard format.</returns>
		/// <exception cref="ArgumentNullException"><c>input</c> is a null reference.</exception>
		/// <exception cref="ArgumentException">Don't is a valid compact format string.</exception>
		/// <exception cref="InvalidOperationException"><see cref="RemovableChars"/> was not set.</exception>
		public string GetStandardFormat(string input)
		{
			if (!this.CompactIsMatch(input))
				throw new ArgumentException(resExceptions.InvalidArg_CompactMatch, "input");
			if (remChars == null)
				throw new InvalidOperationException(resExceptions.Invalid_PropNoSet.Replace("%var", "RemovableChars"));

			int len = remChars.Length;
			if (len == 0)
				return input;

			for (int i = 0; i < len; i++)
			{
				input = input.Insert(remChars[i].Position, remChars[i].Character.ToString());
			}
			return input;
		}

		/// <summary>
		/// Provides a <see cref="String"/> in compact format of standard format, generally used to store in DataBases.
		/// </summary>
		/// <param name="input">A standard postal code.</param>
		/// <returns>A compact postal code.</returns>
		/// <exception cref="ArgumentNullException"><c>input</c> is a null reference.</exception>
		/// <exception cref="ArgumentException">Don't is a valid standard format string.</exception>
		/// <exception cref="NullReferenceException"><see cref="RemovableChars"/> was not set.</exception>
		public string GetCompactFormat(string input)
		{
			if (!this.IsMatch(input))
				throw new ArgumentException(resExceptions.InvalidArg_StandardMatch, "input");
			if (remChars == null)
				throw new InvalidOperationException(resExceptions.Invalid_PropNoSet.Replace("%var", "RemovableChars"));

			int len = remChars.Length;
			if (len == 0)
				return input;

			int count = 0;
			for (int i = 0; i < len; i++)
			{
				input = input.Remove(remChars[i].Position - count, 1);
				count++;
			}
			return input;
		}


		/// <summary>
		/// <para>Verify whether this instance is read-only</para>
		/// If <c>true</c> causes a exception, otherwise, don't do anything.
		/// </summary>
		/// <exception cref="InvalidOperationException">The property is being set and this
		/// instance is read-only.</exception>
		protected void VerifyWritable()
		{
			if (isReadOnly)
				throw new InvalidOperationException(resExceptions.InvalidWrite);
		}

		private void VerifyNull<Tp>(ref Tp value, string property)
		{
			if (value == null)
				throw new ArgumentNullException(resExceptions.PropertyNull.Replace("%var", property));
		}

		#endregion

		#region IMaskeable Members

		/// <summary>
		/// Provides a <see cref="String"/> to use as mask.
		/// </summary>
		/// <remarks>
		/// <para>The masker classes will use this mask to filter user-typed <see cref="Char"/> and to
		/// verify a automatic-typed <see cref="Char"/>.</para>
		/// <para>To indicate a automatic-typed <see cref="Char"/>, uses "!" before of a
		/// <see cref="Char"/>.</para>
		/// <para>The Mask <see cref="String"/> need return true when used in <see cref="IsMatch"/>
		/// method, masker classes ignores "!" to match the Mask <see cref="String"/>.</para>
		/// </remarks>
		/// <exception cref="NullReferenceException"><see cref="Expression"/> not defined.</exception>
		/// <exception cref="Exception">Mask property is being set to a invalid value.</exception>
		public string Mask
		{
			get
			{
				return mask;
			}
			set
			{
				VerifyWritable();
				if (regex == null)
					throw new InvalidOperationException(resExceptions.Invalid_PropNoSet.Replace("%var", "Expression"));
				string s = value.Replace("!", "");
				int pos = s.Length - 1;
				if (s.Substring(pos) == "¬")
					s = s.Remove(pos);
				if (!regex.IsMatch(s))
					throw new Exception(resExceptions.InvalidMask);
				mask = value;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is read-only.
		/// </summary>
		/// <remarks>
		/// <para>A read-only instance guarantees that its values don't changes.</para>
		/// <para>No read-only instances can generate problems to masker classes, thus, the masker
		/// class only accept read-only instances.</para>
		/// </remarks>
		public bool IsReadOnly
		{
			get { return isReadOnly; }
		}

		/// <summary>
		/// Indicates whether this instance finds a match in the input <see cref="String"/>.
		/// </summary>
		/// <param name="input">The <see cref="String"/> to search for a match.</param>
		/// <returns>true if the input <see cref="String"/> is valid; otherwise, false.</returns>
		/// <remarks>
		/// Verify whether the input <see cref="String"/> is meatched by this instance.
		/// </remarks>
		/// <exception cref="ArgumentNullException"><c>input</c> is a null reference.</exception>
		/// <exception cref="NullReferenceException"><see cref="Expression"/> was not set.</exception>
		public bool IsMatch(string input)
		{
			if (input == null)
				throw new ArgumentNullException(resExceptions.ArgumentNull.Replace("%var", "input"));
			if (regex == null)
				throw new InvalidOperationException(resExceptions.Invalid_PropNoSet.Replace("%var", "Expression"));
			return regex.IsMatch(input);
		}

		#endregion

		#region IWriteProtected<FormatStringBase> Members

		/// <summary>
		/// Returns a read-only FormatStringBase wrapper.
		/// </summary>
		/// <returns>A read-only FormatStringBase wrapper around this instance.</returns>
		public FormatStringBase ReadOnly()
		{
			if (this.isReadOnly)
				return this;
			FormatStringBase fsb = (FormatStringBase)this.MemberwiseClone();
			fsb.isReadOnly = true;
			return fsb;
		}

		/// <summary>
		/// Creates a shallow copy of a FormatStringBase.
		/// </summary>
		/// <returns>A new FormatStringBase, not write protected, copied around this instance.</returns>
		public FormatStringBase Clone()
		{
			FormatStringBase obj = (FormatStringBase)this.MemberwiseClone();
			obj.isReadOnly = false;
			return obj;
		}

		#endregion
	}

	/// <summary>
	/// Provides methods to apply grammar rules to strings.
	/// </summary>
	[Serializable]
	public sealed class GrammarRules : IWriteProtected<GrammarRules>, Serialization.ISerializable
	{
		#region Fields

		private static Dictionary<string, GetType<GrammarRules>> _storedCultureInfo;
		private Dictionary<string, string> _accentedSuffix;
		private List<string> _loweredWords;
		private Dictionary<string, string> _tcExcepts;
		private Dictionary<string, string> _excepts;
		private string _ignoredChars;
		private bool _isReadOnly;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes an empty instance of GrammarRules class.
		/// </summary>
		public GrammarRules()
		{
			_accentedSuffix = new Dictionary<string, string>(0);
			_loweredWords = new List<string>(0);
			_tcExcepts = new Dictionary<string, string>(0);
			_excepts = new Dictionary<string, string>(0);
			_isReadOnly = false;
		}

		private GrammarRules(Serialization.SerializationInfo info, Serialization.StreamingContext context)
		{
			if (info == null)
				throw new ArgumentNullException("info", resExceptions.ArgumentNull.Replace("%var", "info"));

			_accentedSuffix = (Dictionary<string, string>)info.GetValue("_accentedSuffix", typeof(Dictionary<string, string>));
			_loweredWords = (List<string>)info.GetValue("_loweredWords", typeof(List<string>));
			_tcExcepts = (Dictionary<string, string>)info.GetValue("_tcExcepts", typeof(Dictionary<string, string>));
			_excepts = (Dictionary<string, string>)info.GetValue("_excepts", typeof(Dictionary<string, string>));
			_ignoredChars = info.GetString("_ignoredChars");
			_isReadOnly = true;
		}

		static GrammarRules()
		{
			_storedCultureInfo = new Dictionary<string, GetType<GrammarRules>>(1);
			_storedCultureInfo.Add("pt-BR", Get_ptBR);
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets a GrammarRules with values based on the current culture.
		/// </summary>
		/// <value>A GrammarRules based on the <see cref="CultureInfo"/> of the current thread.</value>
		public static GrammarRules CurrentInfo
		{
			get
			{
				string name = CultureInfo.CurrentCulture.Name;
				return GetCultureBasedInfo(name);
			}
		}

		/// <summary>
		/// Gets or sets Rules to accented suffixes.
		/// </summary>
		/// <value>A <see cref="Dictionary"/> as suffixes rule.</value>
		/// <exception cref="InvalidOperationException">The property is being set and this
		/// instance is read-only.</exception>
		/// <exception cref="ArgumentNullException">The AccentedSuffixes property is being
		/// set to a null reference.</exception>
		/// <remarks>
		/// Defines word-suffixes that is accented. This property is used in all cases that
		/// <see cref="ApplyRules"/> function is used.
		/// </remarks>
		public Dictionary<string, string> AccentedSuffixes
		{
			get
			{
				return _accentedSuffix;
			}
			set
			{
				VerifyWritable();
				VerifyNull<Dictionary<string, string>>(ref value, "AccentedSuffixes");
				_accentedSuffix = value;
			}
		}

		/// <summary>
		/// Gets or sets Rules to fully lower cases words.
		/// </summary>
		/// <value>A <see cref="List"/> as words rule.</value>
		/// <exception cref="InvalidOperationException">The property is being set and this
		/// instance is read-only.</exception>
		/// <exception cref="ArgumentNullException">The LoweredWords property is being set
		/// to a null reference.</exception>
		/// <remarks>
		/// Defines words that never apply title case. Of course, this property only is used
		/// if titleCase parameter of <see cref="ApplyRules"/> function have a true value.
		/// </remarks>
		public List<string> LoweredWords
		{
			get
			{
				return _loweredWords;
			}
			set
			{
				VerifyWritable();
				VerifyNull<List<string>>(ref value, "LoweredWords");
				_loweredWords = value;
			}
		}

		/// <summary>
		/// Gets or Sets rules exceptions, when this rule is applied other rules is ignored.
		/// </summary>
		/// <value>A <see cref="Dictionary"/> as words rule.</value>
		/// <exception cref="InvalidOperationException">The property is being set and this
		/// instance is read-only.</exception>
		/// <exception cref="ArgumentNullException">The Exceptions property is being
		/// set to a null reference.</exception>
		/// <remarks>
		/// Defines words that other rules not is applied. This property is used in all cases
		/// that <see cref="ApplyRules"/> function is used.
		/// </remarks>
		public Dictionary<string, string> Exceptions
		{
			get
			{
				return _excepts;
			}
			set
			{
				VerifyWritable();
				VerifyNull<Dictionary<string, string>>(ref value, "Exceptions");
				_excepts = value;
			}
		}

		/// <summary>
		/// Gets or sets rules exceptions, only to Title case words, when this rule is applied
		/// other rules is ignored.
		/// </summary>
		/// <value>A <see cref="Dictionary"/> as words rule.</value>
		/// <exception cref="InvalidOperationException">The property is being set and this
		/// instance is read-only.</exception>
		/// <exception cref="ArgumentNullException">The TitleCaseExceptions property is being
		/// set to a null reference.</exception>
		/// <remarks>
		/// Defines words that other rules not is applied, when titleCase parameter of
		/// <see cref="ApplyRules"/> function have a true value.
		/// </remarks>
		public Dictionary<string, string> TitleCaseExceptions
		{
			get
			{
				return _tcExcepts;
			}
			set
			{
				VerifyWritable();
				VerifyNull<Dictionary<string, string>>(ref value, "TitleCaseExceptions");
				_tcExcepts = value;
			}
		}

		/// <summary>
		/// Gets or sets characters that will be ignored by <see cref="ApplyRules"/> method.
		/// </summary>
		/// <value>A <see cref="String"/> as rule to ignored characters.</value>
		/// <exception cref="InvalidOperationException">The property is being set and this
		/// instance is read-only.</exception>
		/// <exception cref="ArgumentNullException">The IgnoredCharacters property is being
		/// set to a null reference.</exception>
		public string IgnoredCharacters
		{
			get { return _ignoredChars; }
			set
			{
				VerifyWritable();
				VerifyNull<string>(ref value, "IgnoredCharacters");
				_ignoredChars = value;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Apply rules stored rules to a <see cref="String"/>.
		/// </summary>
		/// <param name="s">A <see cref="String"/>.</param>
		/// <returns>A formatted <see cref="String"/> based on rules of this instance.</returns>
		public string ApplyRules(string s)
		{
			TextInfo txtInfo = CultureInfo.CurrentCulture.TextInfo;
			return ApplyRules(s, false, txtInfo);
		}

		/// <summary>
		/// Apply rules stored rules to a <see cref="String"/>.
		/// </summary>
		/// <param name="s">A <see cref="String"/></param>
		/// <param name="titleCase">Whether all words will be titlecase formatted.</param>
		/// <returns>A formatted <see cref="String"/> based on rules of this instance.</returns>
		public string ApplyRules(string s, bool titleCase)
		{
			TextInfo txtInfo = CultureInfo.CurrentCulture.TextInfo;
			return ApplyRules(s, titleCase, txtInfo);
		}

		/// <summary>
		/// Apply rules stored rules to a <see cref="String"/>.
		/// </summary>
		/// <param name="s">A <see cref="String"/>.</param>
		/// <param name="titleCase">Whether all words will be titlecase formatted.</param>
		/// <param name="txtInfo">A <see cref="TextInfo"/>.</param>
		/// <returns>A formatted <see cref="String"/> based on stored rules.</returns>
		public string ApplyRules(string s, bool titleCase, TextInfo txtInfo)
		{
			Strings.IndexedChar[] idxChar = ClearText(s, out s);

			if (titleCase)
				s = txtInfo.ToTitleCase(s);
			else
				s = txtInfo.ToLower(s);

			string[] sWords = s.Split(' ');
			System.Text.StringBuilder finalWord = new System.Text.StringBuilder(s.Length);

			foreach (string word in sWords)
			{
				string lWord = txtInfo.ToLower(word);
				string aux = "";

				if (titleCase)
				{
					aux = ApplyTCExceptionsRules(lWord);
					if (aux != "")
					{
						finalWord.Append(aux + " ");
						continue;
					}
				}

				aux = ApplyExceptionsRules(lWord);
				if (aux != "")
				{
					finalWord.Append(aux + " ");
					continue;
				}

				if (IsAppliedLCaseRules(lWord))
					aux = lWord;
				else
				{
					aux = ApplySuffixRules(word);
					lWord = txtInfo.ToLower(word);

					if (IsAppliedLCaseRules(lWord))
						aux = lWord;
				}

				finalWord.Append(aux + " ");
			}
			finalWord = finalWord.Remove(finalWord.Length - 1, 1);

			return RepairText(finalWord.ToString(), idxChar);
		}

		// Removes all chars that matches with any char of _ignoredChars.
		private Strings.IndexedChar[] ClearText(string str, out string strCleared)
		{
			strCleared = str;
			foreach (char c in _ignoredChars)
				strCleared = strCleared.Replace(c.ToString(), "");
			Strings.IndexedChar[] idxChar = Strings.AdditionalChars(str, strCleared);
			return idxChar;
		}

		// Restores all removed by ClearText method
		private string RepairText(string str, Strings.IndexedChar[] idxChar)
		{
			for (int i = 0; i < idxChar.Length; i++)
				str = str.Insert(idxChar[i].Position, idxChar[i].Character.ToString());
			return str;
		}

		// Applies Title Case Exception to word, otherwise returns empty string.
		private string ApplyTCExceptionsRules(string str)
		{
			if (_tcExcepts.ContainsKey(str))
				return _tcExcepts[str];
			return string.Empty;
		}

		// Applies Exception to word, otherwise returns empty string.
		private string ApplyExceptionsRules(string str)
		{
			if (_excepts.ContainsKey(str))
				return _excepts[str];
			return string.Empty;
		}

		// Applies Suffix Rules to word, otherwise returns same word.
		private string ApplySuffixRules(string str)
		{
			int idx = 0;
			foreach (string accentedKey in _accentedSuffix.Keys)
			{
				idx = str.IndexOf(accentedKey);
				if (idx != -1)
					if (str.Length == (idx + accentedKey.Length))
					{
						string aux = str;
						aux = aux.Remove(idx);
						aux += _accentedSuffix[accentedKey];
						return aux;
					}
			}
			return str;
		}

		// Returns whether is applied lower case rules to word.
		private bool IsAppliedLCaseRules(string str)
		{
			if (_loweredWords.Contains(str))
				return true;
			return false;
		}

		/// <summary>
		/// Retrieves a cached, instance of a GrammarRules based on the specified culture name.
		/// </summary>
		/// <param name="name">The name of a culture.</param>
		/// <returns>A GrammarRules object.</returns>
		public static GrammarRules GetCultureBasedInfo(string name)
		{
			if (name.IndexOf('-') == -1)
				name = name.Insert(2, "-");
			name = name.Remove(3) + name.Substring(3).ToUpper();
			if (!_storedCultureInfo.ContainsKey(name))
				throw new ArgumentException(resExceptions.UnsupportedCulture);
			return _storedCultureInfo[name]();
		}

		/// <summary>
		/// Gets the list of names of supported cultures by GrammarRules.
		/// </summary>
		/// <returns>An <see cref="Array"/> of type <see cref="String"/> that contains the cultures names.
		/// The <see cref="Array"/> of cultures is sorted.</returns>
		public string[] GetDisponibleCultures()
		{
			string[] cultureNames = new string[_storedCultureInfo.Keys.Count];
			_storedCultureInfo.Keys.CopyTo(cultureNames, 0);
			return cultureNames;
		}

		private static GrammarRules Get_ptBR()
		{
			GrammarRules gr = new GrammarRules();
			gr._accentedSuffix = new Dictionary<string, string>(17);
			gr._accentedSuffix.Add("ao", "ão");
			gr._accentedSuffix.Add("oo", "ôo");
			gr._accentedSuffix.Add("aos", "ãos");
			gr._accentedSuffix.Add("aes", "ães");
			gr._accentedSuffix.Add("eem", "êem");
			gr._accentedSuffix.Add("oes", "ões");
			gr._accentedSuffix.Add("acil", "ácil");
			gr._accentedSuffix.Add("edia", "édia");
			gr._accentedSuffix.Add("icio", "ício");
			gr._accentedSuffix.Add("ocio", "ócio");
			gr._accentedSuffix.Add("ulia", "úlia");
			gr._accentedSuffix.Add("assio", "ássio");
			gr._accentedSuffix.Add("opole", "ópole");
			gr._accentedSuffix.Add("unior", "únior");
			gr._accentedSuffix.Add("issimo", "íssimo");
			gr._accentedSuffix.Add("issima", "íssima");
			gr._loweredWords = new List<string>(13);
			gr._loweredWords.Add("a");
			gr._loweredWords.Add("ao");
			gr._loweredWords.Add("aos");
			gr._loweredWords.Add("as");
			gr._loweredWords.Add("da");
			gr._loweredWords.Add("das");
			gr._loweredWords.Add("de");
			gr._loweredWords.Add("do");
			gr._loweredWords.Add("dos");
			gr._loweredWords.Add("e");
			gr._loweredWords.Add("é");
			gr._loweredWords.Add("o");
			gr._loweredWords.Add("os");
			gr._excepts = new Dictionary<string, string>(4);
			gr._excepts.Add("fabricio", "Fabrício");
			gr._excepts.Add("fabrício", "Fabrício");
			gr._excepts.Add("godoy", "Godoy");
			gr._excepts.Add("voce", "você");
			gr._ignoredChars = "'\"!()-{}[]?,.;:";
			gr._isReadOnly = true;
			return gr;
		}

		private void VerifyWritable()
		{
			if (_isReadOnly)
				throw new InvalidOperationException(resExceptions.InvalidWrite);
		}

		private void VerifyNull<T>(ref T value, string property)
		{
			if (value == null)
				throw new ArgumentNullException(resExceptions.PropertyNull.Replace("%var", property));
		}

		#endregion

		#region IWriteProtected<GrammarRules> Members

		/// <summary>
		/// Gets a value indicating whether this instance is read-only.
		/// </summary>
		public bool IsReadOnly
		{
			get { return _isReadOnly; }
		}

		/// <summary>
		/// Returns a read-only GrammarRules wrapper.
		/// </summary>
		/// <returns>A read-only GrammarRules wrapper around this instance.</returns>
		public GrammarRules ReadOnly()
		{
			if (this._isReadOnly)
				return this;
			GrammarRules gr = (GrammarRules)this.MemberwiseClone();
			gr._isReadOnly = true;
			return gr;
		}

		/// <summary>
		/// Creates a shallow copy of the GrammarRules.
		/// </summary>
		/// <returns>A new GrammarRules, not write protected, copied around this instance.</returns>
		public GrammarRules Clone()
		{
			GrammarRules gr = (GrammarRules)this.MemberwiseClone();
			gr._isReadOnly = false;
			return gr;
		}

		#endregion

		#region ISerializable Members

		void Serialization.ISerializable.GetObjectData(Serialization.SerializationInfo info, Serialization.StreamingContext context)
		{
			if (info == null)
				throw new ArgumentNullException("info", resExceptions.ArgumentNull.Replace("%var", "info"));

			info.AddValue("_accentedSuffix", _accentedSuffix, typeof(Dictionary<string, string>));
			info.AddValue("_loweredWords", _loweredWords, typeof(List<string>));
			info.AddValue("_tcExcepts", _tcExcepts, typeof(Dictionary<string, string>));
			info.AddValue("_excepts", _excepts, typeof(Dictionary<string, string>));
			info.AddValue("_ignoredChars", _ignoredChars, typeof(string));
		}

		#endregion
	}

	/// <summary>
	/// Provides methods to format Postal Codes.
	/// </summary>
	/// <remarks>
	/// <para>This class contains information as a Postal Code is formatted and stores several Postal Code
	/// format information of specific cultures.</para>
	/// <para>To create a PostalCode for a specific culture, use <see cref="GetCultureBasedInfo"/>
	/// method. To create a PostalCode for the culture of the current thread, use the
	/// <see cref="CurrentInfo"/> property. Use the PostalCode constructor for a writable version.</para>
	/// <para>This class implements the ICloneable interface to enable duplication of PostalCode objects.
	/// It also implements IMaskeable indicating that this class can be used by Root.Windows.Forms.Mask
	/// class.</para>
	/// </remarks>
	/// <example>
	/// The following console application example show how to use PostalCode class.
	/// <code>
	/// using System;
	/// using Root.Formatting;
	/// 
	/// class Program
	/// {
	///     static void Main()
	///     {
	///         PostalCode pCode = PostalCode.GetCulturePostalCodeInfo("en-US");
	///	        string userPCode = "12345-6789";
	///         Console.WriteLine("Compacted Postal Code: {0}", pCode.GetCompactPostalCode(userPCode));
	///         string internalPCode = "987654321";
	///         Console.WriteLine("Standard Postal Code: {0}", pCode.GetStandardPostalCode(internalPCode));
	///         Console.ReadKey();
	///     }
	/// }
	/// </code>
	/// This example returns the following output.
	/// <code>
	/// Compacted Postal Code: 123456789
	/// Standard Postal Code: 98765-4321
	/// </code>
	/// </example>
	public sealed class PostalCode : FormatStringBase
	{
		#region Field

		/// <summary>
		/// Stores a <see cref="Dictionary"/>, where key is a culture name and value
		/// is a delegate to get type- and culture-specific.
		/// </summary>
		private static Dictionary<string, GetType<PostalCode>> storedCultureInfo;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new writable instance of the <see cref="PostalCode"/> class.
		/// </summary>
		/// <remarks>
		/// The properties of the new instance can be modified if you want user-defined formatting.
		/// </remarks>
		public PostalCode() : base() { }

		static PostalCode()
		{
			storedCultureInfo = new Dictionary<string, GetType<PostalCode>>(7);
			storedCultureInfo.Add("de-DE", Get_deDE);
			storedCultureInfo.Add("en-CA", Get_enCA);
			storedCultureInfo.Add("en-US", Get_enUS);
			storedCultureInfo.Add("fr-FR", Get_frFR);
			storedCultureInfo.Add("nl-NL", Get_nlNL);
			storedCultureInfo.Add("pt-BR", Get_ptBR);
			storedCultureInfo.Add("pt-PT", Get_ptPT);
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets a read-only <see cref="PostalCode"/> with informations based on the current culture.
		/// </summary>
		/// <value>A read-only <see cref="PostalCode"/> based on the
		/// <see cref="System.Globalization.CultureInfo"/> of the current thread.</value>
		public static PostalCode CurrentInfo
		{
			get
			{
				string name = CultureInfo.CurrentCulture.Name;
				return GetCultureBasedInfo(name);
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Retrieves a cached, instance of a PostalCode using the specified culture name.
		/// </summary>
		/// <param name="name">The name of a culture.</param>
		/// <returns>A PostalCode object.</returns>
		/// <exception cref="ArgumentException">name specifies a culture that is not supported.</exception>
		public static PostalCode GetCultureBasedInfo(string name)
		{
			if (name.IndexOf('-') == -1)
				name = name.Insert(2, "-");
			name = name.Remove(3) + name.Substring(3).ToUpper();
			if (!storedCultureInfo.ContainsKey(name))
				throw new ArgumentException(resExceptions.UnsupportedCulture);
			return storedCultureInfo[name]();
		}

		/// <summary>
		/// Gets the list of names of supported cultures by <see cref="PostalCode"/>.
		/// </summary>
		/// <returns>An <see cref="Array"/> of type <see cref="String"/> that contains the cultures names.
		/// The <see cref="Array"/> of cultures is sorted.</returns>
		public static string[] GetDisponibleCultures()
		{
			string[] cultureNames = new string[storedCultureInfo.Keys.Count];
			storedCultureInfo.Keys.CopyTo(cultureNames, 0);
			return cultureNames;
		}

		private static PostalCode Get_deDE()
		{
			PostalCode pc = new PostalCode();
			pc.Expression = @"^\d{5}$";
			pc.CompactExpression = @"^\d{5}$";
			pc.Mask = "00000";
			Strings.IndexedChar[] ic = new Strings.IndexedChar[0];
			pc.RemovableChars = ic;
			pc.isReadOnly = true;
			return pc;
		}

		private static PostalCode Get_enCA()
		{
			PostalCode pc = new PostalCode();
			pc.Expression = @"^[A-Y]\d[A-Z] \d[A-Z]\d$";
			pc.CompactExpression = @"^[A-Y]\d[A-Z]\d[A-Z]\d$";
			pc.Mask = "A0A! 0A0";
			Strings.IndexedChar[] ic = new Strings.IndexedChar[1];
			ic[0] = new Strings.IndexedChar(' ', 3);
			pc.RemovableChars = ic;
			pc.isReadOnly = true;
			return pc;
		}

		private static PostalCode Get_enUS()
		{
			PostalCode pc = new PostalCode();
			pc.Expression = @"^\d{5}-\d{4}$";
			pc.CompactExpression = @"^\d{9}$";
			pc.Mask = "00000!-0000";
			Strings.IndexedChar[] ic = new Strings.IndexedChar[1];
			ic[0] = new Strings.IndexedChar('-', 5);
			pc.RemovableChars = ic;
			pc.isReadOnly = true;
			return pc;
		}

		private static PostalCode Get_frFR()
		{
			return Get_deDE();
		}

		private static PostalCode Get_nlNL()
		{
			PostalCode pc = new PostalCode();
			pc.Expression = @"^\d{4} [A-EGHJ-NPR-TV-XZ]{2}$";
			pc.CompactExpression = @"^\d{4}[A-EGHJ-NPR-TV-XZ]{2}$";
			pc.Mask = "0000! AA";
			Strings.IndexedChar[] ic = new Strings.IndexedChar[1];
			ic[0] = new Strings.IndexedChar(' ', 4);
			pc.RemovableChars = ic;
			pc.isReadOnly = true;
			return pc;
		}

		private static PostalCode Get_ptBR()
		{
			PostalCode pc = new PostalCode();
			pc.Expression = @"^\d{5}-\d{3}$";
			pc.CompactExpression = @"^\d{8}$";
			pc.Mask = "00000!-000";
			Strings.IndexedChar[] ic = new Strings.IndexedChar[1];
			ic[0] = new Strings.IndexedChar('-', 5);
			pc.RemovableChars = ic;
			pc.isReadOnly = true;
			return pc;
		}

		private static PostalCode Get_ptPT()
		{
			PostalCode pc = new PostalCode();
			pc.Expression = @"^\d{4}-\d{3} [A-Z]{1,25}$";
			pc.CompactExpression = @"^\d{7}[A-Z]{1,25}$";
			pc.Mask = "0000!-000! A";
			Strings.IndexedChar[] ic = new Strings.IndexedChar[2];
			ic[0] = new Strings.IndexedChar('-', 4);
			ic[1] = new Strings.IndexedChar(' ', 8);
			pc.RemovableChars = ic;
			pc.isReadOnly = true;
			return pc;
		}

		#endregion
	}

	/// <summary>
	/// Provides methods to format Telephones.
	/// </summary>
	public sealed class Telephones : FormatStringBase
	{
		#region Fields

		/// <summary>
		/// Stores a <see cref="Dictionary"/>, where key is a culture name and value
		/// is a delegate to get type- and culture-specific.
		/// </summary>
		private static Dictionary<string, GetType<Telephones>> storedCultureInfo;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new writable instance of the <see cref="Telephones"/> class.
		/// </summary>
		/// <remarks>
		/// The properties of the new instance can be modified if you want user-defined formatting.
		/// </remarks>
		public Telephones() : base() { }

		static Telephones()
		{
			storedCultureInfo = new Dictionary<string, GetType<Telephones>>(2);
			storedCultureInfo.Add("pt-BR", Get_ptBR);
			storedCultureInfo.Add("en-US", Get_enUS);
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets a Telephones with values based on the current culture.
		/// </summary>
		/// <value>A Telephones based on the <see cref="CultureInfo"/> of the current thread.</value>
		public static Telephones CurrentInfo
		{
			get
			{
				string name = CultureInfo.CurrentCulture.Name;
				return GetCultureBasedInfo(name);
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Retrieves a cached, instance of a Telephones using the specified culture name.
		/// </summary>
		/// <param name="name">The name of a culture.</param>
		/// <returns>A Telephones object.</returns>
		/// <exception cref="ArgumentException">name specifies a culture that is not supported.</exception>
		public static Telephones GetCultureBasedInfo(string name)
		{
			if (name.IndexOf('-') == -1)
				name = name.Insert(2, "-");
			name = name.Remove(3) + name.Substring(3).ToUpper();
			if (!storedCultureInfo.ContainsKey(name))
				throw new ArgumentException(resExceptions.UnsupportedCulture);
			return storedCultureInfo[name]();
		}

		/// <summary>
		/// Gets the list of names of supported cultures by Telephones.
		/// </summary>
		/// <returns>An <see cref="Array"/> of type <see cref="String"/> that contains the cultures names.
		/// The <see cref="Array"/> of cultures is sorted.</returns>
		public static string[] GetDisponibleCultures()
		{
			string[] cultureNames = new string[storedCultureInfo.Keys.Count];
			storedCultureInfo.Keys.CopyTo(cultureNames, 0);
			return cultureNames;
		}

		private static Telephones Get_enUS()
		{
			Telephones tel = new Telephones();
			tel.Expression = @"^\([2-9][0-8]\d\) [2-9]\d{2}-\d{4}$";
			tel.CompactExpression = @"^[2-9][0-8]\d[2-9]\d{6}$";
			tel.Mask = "!(200!)! 200!-0000";
			tel.RemovableChars = Strings.AdditionalChars("(123) 456-7890", "1234567890");
			tel.isReadOnly = true;
			return tel;
		}

		private static Telephones Get_ptBR()
		{
			Telephones tel = new Telephones();
			tel.Expression = @"^\(\d{2}\) \d{4}-\d{4}$";
			tel.CompactExpression = @"^\d{10}$";
			tel.Mask = "!(00!)! 0000!-0000";
			tel.RemovableChars = Strings.AdditionalChars("(12) 3456-7890", "1234567890");
			tel.isReadOnly = true;
			return tel;
		}

		#endregion
	}
}
