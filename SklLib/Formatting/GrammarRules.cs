// GrammarRules.cs
//
//  Copyright (C) 2008, 2014 Fabrício Godoy
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
using Serialization = System.Runtime.Serialization;

namespace SklLib.Formatting
{
    /// <summary>
    /// Provides methods to apply grammar rules to strings.
    /// </summary>
    [Serializable]
    public sealed class GrammarRules : IWriteProtected<GrammarRules>, Serialization.ISerializable
    {
        #region Fields

        private static Dictionary<string, Func<GrammarRules>> _storedCultureInfo;
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
            _storedCultureInfo = new Dictionary<string, Func<GrammarRules>>(1);
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
        /// <value>A <see cref="Dictionary&lt;TKey, TValue&gt;"/> as suffixes rule.</value>
        /// <exception cref="InvalidOperationException">The property is being set and this
        /// instance is read-only.</exception>
        /// <exception cref="ArgumentNullException">The AccentedSuffixes property is being
        /// set to a null reference.</exception>
        /// <remarks>
        /// Defines word-suffixes that is accented. This property is used in all cases that
        /// <see cref="ApplyRules(string, bool, TextInfo)"/> function is used.
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
        /// <value>A <see cref="List&lt;T&gt;"/> as words rule.</value>
        /// <exception cref="InvalidOperationException">The property is being set and this
        /// instance is read-only.</exception>
        /// <exception cref="ArgumentNullException">The LoweredWords property is being set
        /// to a null reference.</exception>
        /// <remarks>
        /// Defines words that never apply title case. Of course, this property only is used
        /// if titleCase parameter of <see cref="ApplyRules(string, bool, TextInfo)"/> function have a true value.
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
        /// <value>A <see cref="Dictionary&lt;TKey, TValue&gt;"/> as words rule.</value>
        /// <exception cref="InvalidOperationException">The property is being set and this
        /// instance is read-only.</exception>
        /// <exception cref="ArgumentNullException">The Exceptions property is being
        /// set to a null reference.</exception>
        /// <remarks>
        /// Defines words that other rules not is applied. This property is used in all cases
        /// that <see cref="ApplyRules(string, bool, TextInfo)"/> function is used.
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
        /// <value>A <see cref="Dictionary&lt;TKey, TValue&gt;"/> as words rule.</value>
        /// <exception cref="InvalidOperationException">The property is being set and this
        /// instance is read-only.</exception>
        /// <exception cref="ArgumentNullException">The TitleCaseExceptions property is being
        /// set to a null reference.</exception>
        /// <remarks>
        /// Defines words that other rules not is applied, when titleCase parameter of
        /// <see cref="ApplyRules(string, bool, TextInfo)"/> function have a true value.
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
        /// Gets or sets characters that will be ignored by <see cref="ApplyRules(string, bool, TextInfo)"/> method.
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
}
