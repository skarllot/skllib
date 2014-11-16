// NumberWriteInfo.cs
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
using System.Globalization;
using System.Collections.Generic;

namespace SklLib.Globalization
{
    /// <summary>
    /// Defines how numeric values are written, depending on the culture.
    /// </summary>
    public class NumberWriteInfo : IWriteProtected<NumberWriteInfo>
    {
        #region Fields

        /// <summary>
        /// Stores a <see cref="Dictionary&lt;TKey, TValue&gt;"/>, where key is a culture name and value
        /// is a delegate to get type- and culture-specific.
        /// </summary>
        private static Dictionary<string, NumberWriteInfo> storedCultureInfo;
        private string[] uValues;
        private string[] dValues;
        private string[] hValues;
        private GrammarNumberWriteInfo[] tValues;
        private GrammarNumberWriteInfo[] decValues;
        private string du_Sep;
        private string hd_Sep;
        private string th_Sep;
        private string intdec_Sep;
        private GrammarNumberWriteInfo curInt;
        private GrammarNumberWriteInfo curDec;
        private bool isReadOnly;
        private SpeltNumber[] special;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes static fields of NumberWriteInfo.
        /// </summary>
        static NumberWriteInfo()
        {
            storedCultureInfo = new Dictionary<string, NumberWriteInfo>(3);
            storedCultureInfo.Add("en-US", Get_enUS());
            storedCultureInfo.Add("fr-FR", Get_frFR());
            storedCultureInfo.Add("pt-BR", Get_ptBR());
        }

        /// <summary>
        /// Initializes an empty instance of <see cref="NumberWriteInfo"/>.
        /// </summary>
        public NumberWriteInfo()
        {
            uValues = new string[0];
            dValues = new string[0];
            hValues = new string[0];
            tValues = new GrammarNumberWriteInfo[0];
            decValues = new GrammarNumberWriteInfo[0];
            du_Sep = string.Empty;
            hd_Sep = string.Empty;
            th_Sep = string.Empty;
            intdec_Sep = string.Empty;
            curInt = new GrammarNumberWriteInfo(string.Empty);
            curDec = new GrammarNumberWriteInfo(string.Empty);
            isReadOnly = false;
            special = new SpeltNumber[0];
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a type-specific with values based on the current culture.
        /// </summary>
        /// <value>A type-specific based on the <see cref="CultureInfo"/> of the current thread.</value>
        public static NumberWriteInfo CurrentInfo
        {
            get
            {
                string name = CultureInfo.CurrentCulture.Name;
                return GetCultureBasedInfo(name);
            }
        }

        /// <summary>
        /// Gets or sets how unit values are written (example: "four").
        /// </summary>
        /// <exception cref="InvalidOperationException">The property is being set and this
        /// instance is read-only.</exception>
        public string[] UnityValues
        {
            get { return uValues; }
            set
            {
                VerifyWritable();
                uValues = value;
            }
        }

        /// <summary>
        /// Gets or sets how dozen values are writen (example: "twenty").
        /// </summary>
        /// <exception cref="InvalidOperationException">The property is being set and this
        /// instance is read-only.</exception>
        public string[] DozenValues
        {
            get { return dValues; }
            set
            {
                VerifyWritable();
                dValues = value;
            }
        }

        /// <summary>
        /// Gets or sets how hundred values are written (example: "Five Hundred").
        /// </summary>
        /// <exception cref="InvalidOperationException">The property is being set and this
        /// instance is read-only.</exception>
        public string[] HundredValues
        {
            get { return hValues; }
            set
            {
                VerifyWritable();
                hValues = value;
            }
        }

        /// <summary>
        /// Gets or sets how thousand values are written (example: "billion").
        /// </summary>
        /// <exception cref="InvalidOperationException">The property is being set and this
        /// instance is read-only.</exception>
        public GrammarNumberWriteInfo[] ThousandValues
        {
            get { return tValues; }
            set
            {
                VerifyWritable();
                tValues = value;
            }
        }

        /// <summary>
        /// Gets or sets how decimal values are written (example: "ten-thousandth").
        /// </summary>
        /// <exception cref="InvalidOperationException">The property is being set and this
        /// instance is read-only.</exception>
        public GrammarNumberWriteInfo[] DecimalValues
        {
            get { return decValues; }
            set
            {
                VerifyWritable();
                decValues = value;
            }
        }

        /// <summary>
        /// Gets or sets a String to separates Dozen numbers of the Units numbers (e.g: The '-' from 'twenty-one').
        /// </summary>
        /// <exception cref="InvalidOperationException">The property is being set and this
        /// instance is read-only.</exception>
        public string DozenUnitSeparator
        {
            get { return du_Sep; }
            set
            {
                VerifyWritable();
                du_Sep = value;
            }
        }

        /// <summary>
        /// Gets or sets a String to separates Hundred numbers of the Dozen numbers (e.g: The ' and ' from 'one hundred and one').
        /// </summary>
        /// <exception cref="InvalidOperationException">The property is being set and this
        /// instance is read-only.</exception>
        public string HundredDozenSeparator
        {
            get { return hd_Sep; }
            set
            {
                VerifyWritable();
                hd_Sep = value;
            }
        }

        /// <summary>
        /// Gets or sets separator used between Thousand values.
        /// </summary>
        /// <exception cref="InvalidOperationException">The property is being set and this
        /// instance is read-only.</exception>
        public string ThousandsSeparator
        {
            get { return th_Sep; }
            set
            {
                VerifyWritable();
                th_Sep = value;
            }
        }

        /// <summary>
        /// Gets or sets separator used between integer values and decimal values.
        /// </summary>
        /// <exception cref="InvalidOperationException">The property is being set and this
        /// instance is read-only.</exception>
        public string IntegerDecimalSeparator
        {
            get { return intdec_Sep; }
            set
            {
                VerifyWritable();
                intdec_Sep = value;
            }
        }

        /// <summary>
        /// Gets or sets how currency integer name is written (example: "dollar and dollars").
        /// </summary>
        /// <exception cref="InvalidOperationException">The property is being set and this
        /// instance is read-only.</exception>
        public GrammarNumberWriteInfo CurrencyIntegerName
        {
            get { return curInt; }
            set
            {
                VerifyWritable();
                curInt = value;
            }
        }

        /// <summary>
        /// Gets or sets how currency decimal name is written (example: "cent and cents").
        /// </summary>
        /// <exception cref="InvalidOperationException">The property is being set and this
        /// instance is read-only.</exception>
        public GrammarNumberWriteInfo CurrencyDecimalName
        {
            get { return curDec; }
            set
            {
                VerifyWritable();
                curDec = value;
            }
        }

        /// <summary>
        /// Gets or sets how specific values are written.
        /// </summary>
        /// <exception cref="InvalidOperationException">The property is being set and this
        /// instance is read-only.</exception>
        public SpeltNumber[] SpecialCases
        {
            get { return special; }
            set
            {
                VerifyWritable();
                special = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Retrieves a cached, instance of a NumberWriteInfo using the specified culture name.
        /// </summary>
        /// <param name="name">The name of a culture.</param>
        /// <returns>A NumberWriteInfo object.</returns>
        /// <exception cref="ArgumentException"><c>name</c> specifies a culture that is not
        /// supported.</exception>
        public static NumberWriteInfo GetCultureBasedInfo(string name)
        {
            if (!storedCultureInfo.ContainsKey(name))
                throw new ArgumentException(resExceptions.UnsupportedCulture);

            return storedCultureInfo[name];
        }

        /// <summary>
        /// Gets the list of names of supported cultures by NumberWriteInfo.
        /// </summary>
        /// <returns>An <see cref="IEnumerable&lt;T&gt;"/> of type <see cref="String"/> that contains the cultures names.
        /// The <see cref="Array"/> of cultures is sorted.</returns>
        public static IEnumerable<string> GetDisponibleCultures()
        {
            return storedCultureInfo.Keys;
        }


        private static NumberWriteInfo Get_enUS()
        {
            NumberWriteInfo nwi = new NumberWriteInfo();

            nwi.UnityValues = new string[] {
                "zero", "one", "two", "three", "four", "five", "six", "seven",
                "eight", "nine", "ten", "eleven", "twelve", "threeteen",
                "fourteen", "fiveteen", "sixteen", "seventeen", "eighteen",
                "nineteen"
            };

            nwi.DozenValues = new string[] {
                "", "ten", "twenty", "thirty", "forty", "fifty", "sixty",
                "seventy", "eighty", "ninety"
            };

            nwi.HundredValues = new string[] {
                "", "one hundred", "two hundred", "three hundred",
                "four hundred", "five hundred", "six hundred", "seven hundred",
                "eight hundred", "nine hundred"
            };

            nwi.ThousandValues = new GrammarNumberWriteInfo[] {
                new GrammarNumberWriteInfo(string.Empty),
                new GrammarNumberWriteInfo("thousand"),
                new GrammarNumberWriteInfo("million"),
                new GrammarNumberWriteInfo("billion"),
                new GrammarNumberWriteInfo("trillion"),
                new GrammarNumberWriteInfo("quadrillion"),
                new GrammarNumberWriteInfo("quintillion"),
                new GrammarNumberWriteInfo("sextillion"),
                new GrammarNumberWriteInfo("septillion"),
                new GrammarNumberWriteInfo("octillion"),
                new GrammarNumberWriteInfo("nonillion"),
                new GrammarNumberWriteInfo("decillion"),
                new GrammarNumberWriteInfo("undecillion"),
                new GrammarNumberWriteInfo("duodecillion")
            };

            nwi.DecimalValues = new GrammarNumberWriteInfo[] {
                new GrammarNumberWriteInfo(string.Empty),
                new GrammarNumberWriteInfo("tenth", "tenths"),
                new GrammarNumberWriteInfo("hundredth", "hundredths"),
                new GrammarNumberWriteInfo("thousandth", "thousandths"),
                new GrammarNumberWriteInfo("ten-thousandth", "ten-thousandths"),
                new GrammarNumberWriteInfo("hundred-thousandth", "hundred-thousandths"),
                new GrammarNumberWriteInfo("millionth", "millionths"),
                new GrammarNumberWriteInfo("ten-millionth", "ten-millionths"),
                new GrammarNumberWriteInfo("hundred-millionth", "hundred-millionths"),
                new GrammarNumberWriteInfo("billionth", "billionths"),
                new GrammarNumberWriteInfo("ten-billionth", "ten-billionths"),
                new GrammarNumberWriteInfo("hundred-billionth", "hundred-billionths"),
                new GrammarNumberWriteInfo("trillionth", "trillionths"),
                new GrammarNumberWriteInfo("ten-trillionth", "ten-trillionths"),
                new GrammarNumberWriteInfo("hundred-trillionth", "hundred-trillionths")
            };

            SpeltNumber[] wn = new SpeltNumber[0];
            nwi.SpecialCases = wn;

            nwi.DozenUnitSeparator = "-";
            nwi.HundredDozenSeparator = " and ";
            nwi.ThousandsSeparator = ", ";
            nwi.IntegerDecimalSeparator = " and ";
            nwi.CurrencyIntegerName = new GrammarNumberWriteInfo("Dollar", "Dollars");
            nwi.CurrencyDecimalName = new GrammarNumberWriteInfo("Cent", "Cents");
            nwi.isReadOnly = true;

            return nwi;
        }

        private static NumberWriteInfo Get_frFR()
        {
            NumberWriteInfo nwi = new NumberWriteInfo();

            nwi.UnityValues = new string[] {
                "zéro", "un", "deux", "trois", "quatre", "cinq", "six", "sept",
                "huit", "neuf", "dix", "onze", "douze", "treize", "quatorze",
                "quinze", "seize", "dix-sept", "dix-huit", "dix-neuf"
            };

            nwi.DozenValues = new string[] {
                "", "dix", "vingt", "trente", "quarante", "cinquante",
                "soixante", "soixante-dix", "quatre-vingt", "quatre-vingt-dix"
            };

            nwi.HundredValues = new string[] {
                "", "cent", "deux cents", "trois cents", "quatre cents",
                "cinq cents", "six cents", "sept cents", "huit cents",
                "neuf cents"
            };

            nwi.ThousandValues = new GrammarNumberWriteInfo[] {
                new GrammarNumberWriteInfo(string.Empty),
                new GrammarNumberWriteInfo("mille"),
                new GrammarNumberWriteInfo("million"),
                new GrammarNumberWriteInfo("milliard"),
                new GrammarNumberWriteInfo("billion")
            };

            nwi.DecimalValues = new GrammarNumberWriteInfo[] { };

            nwi.SpecialCases = new SpeltNumber[] {
                new SpeltNumber(21, "vingt et un"),
                new SpeltNumber(31, "trente et un"),
                new SpeltNumber(41, "quarante et un"),
                new SpeltNumber(51, "cinquante et un"),
                new SpeltNumber(61, "soixante et un"),
                new SpeltNumber(71, "soixante et onze"),
                new SpeltNumber(72, "soixante et douze"),
                new SpeltNumber(73, "soixante et treize"),
                new SpeltNumber(74, "soixante et quatorze"),
                new SpeltNumber(75, "soixante et quinze"),
                new SpeltNumber(76, "soixante et seize"),
                new SpeltNumber(80, "quatre-vingts"),
                new SpeltNumber(91, "quatre-vingt-onze"),
                new SpeltNumber(92, "quatre-vingt-douze"),
                new SpeltNumber(93, "quatre-vingt-treize"),
                new SpeltNumber(94, "quatre-vingt-quatorze"),
                new SpeltNumber(95, "quatre-vingt-quinze"),
                new SpeltNumber(96, "quatre-vingt-seize")
            };

            nwi.DozenUnitSeparator = "-";
            nwi.HundredDozenSeparator = " ";
            nwi.ThousandsSeparator = " ";
            nwi.IntegerDecimalSeparator = " ";
            nwi.CurrencyIntegerName = new GrammarNumberWriteInfo("Euro", "Euros");
            nwi.CurrencyDecimalName = new GrammarNumberWriteInfo("Cent", "Cents");
            nwi.isReadOnly = true;

            return nwi;
        }

        private static NumberWriteInfo Get_ptBR()
        {
            NumberWriteInfo nwi = new NumberWriteInfo();

            nwi.UnityValues = new string[] {
                "zero", "um", "dois", "três", "quatro", "cinco", "seis",
                "sete", "oito", "nove", "dez", "onze", "doze", "treze",
                "quatorze", "quinze", "dezesseis", "dezessete", "dezoito",
                "dezenove"
            };

            nwi.DozenValues = new string[] {
                "", "dez", "vinte", "trinta", "quarenta", "cinquenta",
                "sessenta", "setenta", "oitenta", "noventa"
            };

            nwi.HundredValues = new string[] {
                "", "cento", "duzentos", "trezentos", "quatrocentos",
                "quinhentos", "seissentos", "setecentos", "oitocentos",
                "novecentos"
            };

            nwi.ThousandValues = new GrammarNumberWriteInfo[] {
                new GrammarNumberWriteInfo(string.Empty),
                new GrammarNumberWriteInfo("mil"),
                new GrammarNumberWriteInfo("milhão", "milhões"),
                new GrammarNumberWriteInfo("bilhão", "bilhões"),
                new GrammarNumberWriteInfo("trilhão", "trilhões"),
                new GrammarNumberWriteInfo("quatrilhão", "quatrilhões"),
                new GrammarNumberWriteInfo("quintilhão", "quintilhões"),
                new GrammarNumberWriteInfo("sextilhão", "sextilhões"),
                new GrammarNumberWriteInfo("setilhão", "setilhões"),
                new GrammarNumberWriteInfo("octilhão", "octilhões")
            };

            nwi.DecimalValues = new GrammarNumberWriteInfo[] {
                new GrammarNumberWriteInfo(string.Empty),
                new GrammarNumberWriteInfo("décimo", "décimos"),
                new GrammarNumberWriteInfo("centésimo", "centésimos"),
                new GrammarNumberWriteInfo("milésimo", "milésimos"),
                new GrammarNumberWriteInfo("décimo-milésimo", "décimos-milésimos"),
                new GrammarNumberWriteInfo("centésimo-milésimo", "centésimos-milésimos"),
                new GrammarNumberWriteInfo("milionésimo", "milionésimos"),
                new GrammarNumberWriteInfo("décimo-milionésimo", "décimos-milionésimos"),
                new GrammarNumberWriteInfo("centésimo-milionésimo", "centésimos-milionésimos"),
                new GrammarNumberWriteInfo("bilionésimo", "bilionésimos"),
                new GrammarNumberWriteInfo("décimo-bilionésimo", "décimos-bilionésimos"),
                new GrammarNumberWriteInfo("centésimo-bilionésimo", "centésimos-bilionésimos"),
                new GrammarNumberWriteInfo("trilionésimo", "trilionésimos"),
                new GrammarNumberWriteInfo("décimo-trilionésimo", "décimos-trilionésimos"),
                new GrammarNumberWriteInfo("centésimo-trilionésimo", "centésimos-trilionésimos")
            };

            nwi.SpecialCases = new SpeltNumber[] {
                new SpeltNumber(100, "cem")
            };

            nwi.DozenUnitSeparator = " e ";
            nwi.HundredDozenSeparator = " e ";
            nwi.ThousandsSeparator = " e ";
            nwi.IntegerDecimalSeparator = " e ";
            nwi.CurrencyIntegerName = new GrammarNumberWriteInfo("Real", "Reais");
            nwi.CurrencyDecimalName = new GrammarNumberWriteInfo("Centavo", "Centavos");
            nwi.isReadOnly = true;

            return nwi;
        }

        private void VerifyWritable()
        {
            if (isReadOnly)
                throw new InvalidOperationException(resExceptions.InvalidWrite);
        }

        #endregion

        #region IWriteProtected<NumberWriteInfo> Members

        /// <summary>
        /// Gets a value indicating whether this instance is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return isReadOnly; }
        }

        /// <summary>
        /// Returns a read-only NumberWriteInfo wrapper.
        /// </summary>
        /// <returns>A read-only NumberWriteInfo wrapper around this instance.</returns>
        public NumberWriteInfo ReadOnly()
        {
            if (this.isReadOnly)
                return this;
            NumberWriteInfo nwi = (NumberWriteInfo)this.MemberwiseClone();
            nwi.isReadOnly = true;
            return nwi;
        }

        /// <summary>
        /// Creates a shallow copy of the NumberWriteInfo.
        /// </summary>
        /// <returns>A new NumberWriteInfo, not write protected, copied around this instance.</returns>
        public NumberWriteInfo Clone()
        {
            NumberWriteInfo nwi = (NumberWriteInfo)this.MemberwiseClone();
            nwi.isReadOnly = false;
            return nwi;
        }

        #endregion
    }
}
