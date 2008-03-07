using System;
using System.Globalization;
using System.Collections.Generic;

namespace Root.Globalization
{
	/// <summary>
	/// Defines how numeric values are written, depending on the culture.
	/// </summary>
	public class NumberWriteInfo : IWriteProtected<NumberWriteInfo>
	{
		#region Fields

		/// <summary>
		/// Stores a <see cref="Dictionary<TKey, TValue>"/>, where key is a culture name and value
		/// is a delegate to get type- and culture-specific.
		/// </summary>
		private static Dictionary<string, GetType<NumberWriteInfo>> storedCultureInfo;
		private string[] uValues;
		private string[] dValues;
		private string[] hValues;
		private string[] tValues;
		private string[] decValues;
		private string du_Sep;
		private string hd_Sep;
		private string th_Sep;
		private string intdec_Sep;
		private string curInt;
		private string curDec;
		private bool isReadOnly;
		private WriteNumber[] special;

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
		public string[] ThousandValues
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
		public string[] DecimalValues
		{
			get { return decValues; }
			set
			{
				VerifyWritable();
				decValues = value;
			}
		}

		/// <summary>
		/// Gets or sets a String to separates Dozen numbers of the Units numbers (example: "-").
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
		/// Gets or sets a String to separates Hundred numbers of the Dozen numbers (example: " and ").
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
		/// Gets or sets how currency integer name is written (example: "dollar&amp;dollars").
		/// </summary>
		/// <exception cref="InvalidOperationException">The property is being set and this
		/// instance is read-only.</exception>
		public string CurrencyIntegerName
		{
			get { return curInt; }
			set
			{
				VerifyWritable();
				curInt = value;
			}
		}

		/// <summary>
		/// Gets or sets how currency decimal name is written (example: "cent&amp;cents").
		/// </summary>
		/// <exception cref="InvalidOperationException">The property is being set and this
		/// instance is read-only.</exception>
		public string CurrencyDecimalName
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
		public WriteNumber[] SpecialCases
		{
			get { return special; }
			set
			{
				VerifyWritable();
				special = value;
			}
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes static fields of NumberWriteInfo.
		/// </summary>
		static NumberWriteInfo()
		{
			storedCultureInfo = new Dictionary<string, GetType<NumberWriteInfo>>(3);
			storedCultureInfo.Add("en-US", Get_enUS);
			storedCultureInfo.Add("fr-FR", Get_frFR);
			storedCultureInfo.Add("pt-BR", Get_ptBR);
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

			return storedCultureInfo[name]();
		}

		/// <summary>
		/// Gets the list of names of supported cultures by NumberWriteInfo.
		/// </summary>
		/// <returns>An <see cref="Array"/> of type <see cref="String"/> that contains the cultures names.
		/// The <see cref="Array"/> of cultures is sorted.</returns>
		public static string[] GetDisponibleCultures()
		{
			string[] cultureNames = new string[storedCultureInfo.Keys.Count];
			storedCultureInfo.Keys.CopyTo(cultureNames, 0);
			return cultureNames;
		}


		private static NumberWriteInfo Get_enUS()
		{
			NumberWriteInfo nwi = new NumberWriteInfo();

			string[] uv = new string[] { "zero", "one", "two", "three", "four", "five", "six",
					"seven", "eight", "nine", "ten", "eleven", "twelve", "threeteen", "fourteen", "fiveteen",
					"sixteen", "seventeen", "eighteen", "nineteen" };
			nwi.UnityValues = uv;

			string[] dv = new string[] { "", "ten", "twenty", "thirty", "forty", "fifty",
					"sixty", "seventy", "eighty", "ninety" };
			nwi.DozenValues = dv;

			string[] hv = new string[] { "", "one hundred", "two hundred", "three hundred",
					"four hundred", "five hundred", "six hundred", "seven hundred", "eight hundred",
					"nine hundred" };
			nwi.HundredValues = hv;

			string[] tv = new string[] { "", "thousand", "million", "billion", "trillion",
					"quadrillion", "quintillion", "sextillion", "septillion", "octillion", "nonillion",
					"decillion", "undecillion", "duodecillion" };
			nwi.ThousandValues = tv;

			string[] decv = new string[] { "", "tenth&tenths", "hundredth&hundredth",
					"thousandth&thousandths", "ten-thousandth&ten-thousandths",
					"hundred-thousandth&hundred-thousandths", "millionth&millionths",
					"ten-millionth&ten-millionths", "hundred-millionth&hundred-millionths",
					"billionth&billionths", "ten-billionth&ten-billionths",
					"hundred-billionth&hundred-billionths", "trillionth&trillionths",
					"ten-trillionth&ten-trillionths", "hundred-trillionth&hundred-trillionths" };
			nwi.DecimalValues = decv;

			WriteNumber[] wn = new WriteNumber[0];
			nwi.SpecialCases = wn;

			nwi.DozenUnitSeparator = "-";
			nwi.HundredDozenSeparator = " and ";
			nwi.ThousandsSeparator = ", ";
			nwi.IntegerDecimalSeparator = " and ";
			nwi.CurrencyIntegerName = "Dollar&Dollars";
			nwi.CurrencyDecimalName = "Cent&Cents";

			return nwi;
		}

		private static NumberWriteInfo Get_frFR()
		{
			NumberWriteInfo nwi = new NumberWriteInfo();

			string[] uv = new string[] { "zéro", "un", "deux", "trois", "quatre", "cinq", "six",
					"sept", "huit", "neuf", "dix", "onze", "douze", "treize", "quatorze", "quinze",
					"seize", "dix-sept", "dix-huit", "dix-neuf" };
			nwi.UnityValues = uv;

			string[] dv = new string[] { "", "dix", "vingt", "trente", "quarante", "cinquante",
					"soixante", "soixante-dix", "quatre-vingt", "quatre-vingt-dix" };
			nwi.DozenValues = dv;

			string[] hv = new string[] { "", "cent", "deux cents", "trois cents",
					"quatre cents", "cinq cents", "six cents", "sept cents", "huit cents",
					"neuf cents" };
			nwi.HundredValues = hv;

			string[] tv = new string[] { "", "mille", "million", "milliard", "billion" };
			nwi.ThousandValues = tv;

			string[] decv = new string[] { };
			nwi.DecimalValues = decv;

			WriteNumber[] wn = new WriteNumber[18];
			wn[0] = new WriteNumber(21, "vingt et un");
			wn[1] = new WriteNumber(31, "trente et un");
			wn[2] = new WriteNumber(41, "quarante et un");
			wn[3] = new WriteNumber(51, "cinquante et un");
			wn[4] = new WriteNumber(61, "soixante et un");
			wn[5] = new WriteNumber(71, "soixante et onze");
			wn[6] = new WriteNumber(72, "soixante et douze");
			wn[7] = new WriteNumber(73, "soixante et treize");
			wn[8] = new WriteNumber(74, "soixante et quatorze");
			wn[9] = new WriteNumber(75, "soixante et quinze");
			wn[10] = new WriteNumber(76, "soixante et seize");
			wn[11] = new WriteNumber(80, "quatre-vingts");
			wn[12] = new WriteNumber(91, "quatre-vingt-onze");
			wn[13] = new WriteNumber(92, "quatre-vingt-douze");
			wn[14] = new WriteNumber(93, "quatre-vingt-treize");
			wn[15] = new WriteNumber(94, "quatre-vingt-quatorze");
			wn[16] = new WriteNumber(95, "quatre-vingt-quinze");
			wn[17] = new WriteNumber(96, "quatre-vingt-seize");
			nwi.SpecialCases = wn;

			nwi.DozenUnitSeparator = "-";
			nwi.HundredDozenSeparator = " ";
			nwi.ThousandsSeparator = " ";
			nwi.IntegerDecimalSeparator = " ";
			nwi.CurrencyIntegerName = "Euro&Euros";
			nwi.CurrencyDecimalName = "Cent&Cents";

			return nwi;
		}

		private static NumberWriteInfo Get_ptBR()
		{
			NumberWriteInfo nwi = new NumberWriteInfo();

			string[] uv = new string[] { "zero", "um", "dois", "três", "quatro", "cinco", "seis",
					"sete", "oito", "nove", "dez", "onze", "doze", "treze", "quatorze", "quinze",
					"dezesseis", "dezessete", "dezoito", "dezenove" };
			nwi.UnityValues = uv;

			string[] dv = new string[] { "", "dez", "vinte", "trinta", "quarenta", "cinquenta",
					"sessenta", "setenta", "oitenta", "noventa" };
			nwi.DozenValues = dv;

			string[] hv = new string[] { "", "cento", "duzentos", "trezentos", "quatrocentos",
					"quinhentos", "seissentos", "setecentos", "oitocentos", "novecentos" };
			nwi.HundredValues = hv;

			string[] tv = new string[] { "", "mil", "milhão&milhões", "bilhão&bilhões", "trilhão&trilhões",
					"quatrilhão&quatrilhões", "quintilhão&quintilhões", "sextilhão&sextilhões",
					"setilhão&setilhões", "octilhão&octilhões" };
			nwi.ThousandValues = tv;

			string[] decv = new string[] { "", "décimo&décimos", "centésimo&centésimos",
					"milésimo&milésimos", "décimo-milésimo&décimos-milésimos",
					"centésimo-milésimo&centésimos-milésimos", "milionésimo&milionésimos",
					"décimo-milionésimo&décimos-milionésimos", "centésimo-milionésimo&centésimos-milionésimos",
					"bilionésimo&bilionésimos", "décimo-bilionésimo&décimos-bilionésimos",
					"centésimo-bilionésimo&centésimos-bilionésimos", "trilionésimo&trilionésimos",
					"décimo-trilionésimo&décimos-trilionésimos",
					"centésimo-trilionésimo&centésimos-trilionésimos" };
			nwi.DecimalValues = decv;

			WriteNumber[] wn = new WriteNumber[1];
			wn[0] = new WriteNumber(100, "cem");
			nwi.SpecialCases = wn;

			nwi.DozenUnitSeparator = " e ";
			nwi.HundredDozenSeparator = " e ";
			nwi.ThousandsSeparator = " e ";
			nwi.IntegerDecimalSeparator = " e ";
			nwi.CurrencyIntegerName = "Real&Reais";
			nwi.CurrencyDecimalName = "Centavo&Centavos";

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
		
		/// <summary>
		/// Stores how writes a number.
		/// </summary>
		public struct WriteNumber
		{
			/// <summary>
			/// A value.
			/// </summary>
			public long value;

			/// <summary>
			/// How <c>value</c> is written.
			/// </summary>
			public string write;

			/// <summary>
			/// Initializes the WriteNumber struct with loaded values.
			/// </summary>
			/// <param name="v">A value.</param>
			/// <param name="w">How <c>v</c> is written.</param>
			public WriteNumber(long v, string w)
			{
				value = v;
				write = w;
			}
		}

	}
}
