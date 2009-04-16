// FormatStringBase.cs
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
using System.Text.RegularExpressions;

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
}
