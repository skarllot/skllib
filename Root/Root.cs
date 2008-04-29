using System;
using System.Globalization;
using Serialization = System.Runtime.Serialization;
using stringb = System.Text.StringBuilder;

/// <summary>
/// Namespace where are located basic code.
/// </summary>
namespace Root
{
	/// <summary>
	/// Represents a generic function that returns a type-specific.
	/// </summary>
	/// <typeparam name="T">Any type-specific.</typeparam>
	/// <returns>Specified type-specific.</returns>
	public delegate T GetType<T>();

	/// <summary>
	/// Defines methods to masking support.
	/// </summary>
	/// <remarks>
	/// The IMaskeable interface contains three members, which are intented to support
	/// masking.
	/// </remarks>
	public interface IMaskeable
	{
		#region Properties
		
		/// <summary>
		/// Provides a <see cref="String"/> to use as mask.
		/// </summary>
		/// <remarks>
		/// <para>The masker classes uses a mask to filter user-typed <see cref="Char"/> and to
		/// verify a automatic-typed <see cref="Char"/>.</para>
		/// <para>To indicate a automatic-typed <see cref="Char"/>, uses "!" before of a
		/// <see cref="Char"/>.</para>
		/// <para>The Mask <see cref="String"/> need return true when used in <see cref="IsMatch"/> method,
		/// masker classes ignores "!" to match the Mask <see cref="String"/>.</para>
		/// </remarks>
		string Mask
		{ get; set; }

		/// <summary>
		/// Gets a value indicating whether this instance is read-only.
		/// </summary>
		/// <remarks>
		/// <para>Masker classes uses this property to verify whether this instance is read-only.</para>
		/// <para>No read-only instances can generate problems to masker classes, thus, the masker
		/// class only accept read-only instances.</para>
		/// </remarks>
		bool IsReadOnly
		{ get; }
		
		#endregion

		#region Method
		
		/// <summary>
		/// Indicates whether this instance finds a match in the input string.
		/// </summary>
		/// <param name="input">The string to search for a match.</param>
		/// <returns>true if this instance finds a match; otherwise, false.</returns>
		/// <remarks>
		/// <para>Masker classes uses this method to verify whether a user-typed <see cref="String"/> is
		/// match with Mask.</para>
		/// </remarks>
		bool IsMatch(string input);
		
		#endregion
	}

	/// <summary>
	/// Defines methods that supports read-only instances.
	/// </summary>
	/// <typeparam name="T">Type of current instance.</typeparam>
	public interface IWriteProtected<T>
	{
		#region Property
		
		/// <summary>
		/// Gets a value indicating whether this instance is read-only.
		/// </summary>
		bool IsReadOnly
		{ get;}

		#endregion
		
		#region Methods
		
		/// <summary>
		/// Returns a read-only type-specific wrapper.
		/// </summary>
		/// <returns>A read-only type-specific wrapper around this instance.</returns>
		T ReadOnly();

		/// <summary>
		/// Creates a shallow copy of the type-specific.
		/// </summary>
		/// <returns>A new type-specific, not write protected, copied around this instance.</returns>
		T Clone();
		
		#endregion
	}

	/// <summary>
	/// Byte multiples.
	/// </summary>
	public enum ByteMeasure : long
	{
		/// <summary>
		/// Represents a byte.
		/// </summary>
		Bytes = 1L,
		/// <summary>
		/// Represents a kilobyte (1024 B).
		/// </summary>
		Kilobytes = 1024L,
		/// <summary>
		/// Represents a megabyte (1024 KB).
		/// </summary>
		Megabytes = 1048576L,
		/// <summary>
		/// Represents a gigabyte (1024 MB).
		/// </summary>
		Gigabytes = 1073741824L,
		/// <summary>
		/// Represents a terabyte (1024 GB).
		/// </summary>
		Terabytes = 1099511627776L,
		/// <summary>
		/// Represents a petabyte (1024 TB).
		/// </summary>
		Petabytes = 1125899906842624L,
		/// <summary>
		/// Represents a exabyte (1024 PB).
		/// </summary>
		Exabytes = 1152921504606846976L
	}

	/// <summary>
	/// Linear units from International System of Units.
	/// </summary>
	public enum SILengthUnit : int
	{
		/// <summary>
		/// Millimeter length unit.
		/// </summary>
		Millimeter = 1,
		/// <summary>
		/// Centimeter length unit.
		/// </summary>
		Centimeter = 10,
		/// <summary>
		/// Decimeter length unit.
		/// </summary>
		Decimeter = 100,
		/// <summary>
		/// Meter length unit.
		/// </summary>
		Meter = 1000,
		/// <summary>
		/// Decameter length unit.
		/// </summary>
		Decameter = 10000,
		/// <summary>
		/// Hectometer length unit.
		/// </summary>
		Hectometer = 100000,
		/// <summary>
		/// Kilometer length unit.
		/// </summary>
		Kilometer = 1000000
	};

	/// <summary>
	/// Identifies a operating System version.
	/// </summary>
	public enum OSVersion
	{
		/// <summary>
		/// Unidentifield Windows Version
		/// </summary>
		Unidentifield,
		/// <summary>
		/// Windows Compact Edition oldest than 3.x
		/// </summary>
		WindowsCEOld,
		/// <summary>
		/// Windows Compact Edition 3.x
		/// </summary>
		WindowsCE3,
		/// <summary>
		/// Windows Compact Edition 4.x
		/// </summary>
		WindowsCE4,
		/// <summary>
		/// Windows Compact Edition 5.x
		/// </summary>
		WindowsCE5,
		/// <summary>
		/// Windows Compact Edition 6.x
		/// </summary>
		WindowsCE6,
		/// <summary>
		/// Windows Compact Edition newest than 6.x
		/// </summary>
		WindowsCENew,
		/// <summary>
		/// Windows 95
		/// </summary>
		Windows95,
		/// <summary>
		/// Windows 98
		/// </summary>
		Windows98,
		/// <summary>
		/// Windows 98 Second Edition
		/// </summary>
		Windows98SE,
		/// <summary>
		/// Windows Millenium Edition
		/// </summary>
		WindowsME,
		/// <summary>
		/// Windows NT 4.x
		/// </summary>
		WindowsNT4,
		/// <summary>
		/// Windows 2000
		/// </summary>
		Windows2000,
		/// <summary>
		/// Windows XP
		/// </summary>
		WindowsXP,
		/// <summary>
		/// Windows Server 2003
		/// </summary>
		WindowsServer2003,
		/// <summary>
		/// Windows Vista
		/// </summary>
		WindowsVista,
		/// <summary>
		/// Windows newest than Vista
		/// </summary>
		WindowsNew
	}

	/// <summary>
	/// Imperial linear units.
	/// </summary>
	public enum ImperialLengthUnit : int
	{
		/// <summary>
		/// Inch length unit.
		/// </summary>
		Inch = 1,
		/// <summary>
		/// Foot length unit.
		/// </summary>
		Foot = 12,
		/// <summary>
		/// Yard length unit.
		/// </summary>
		Yard = 36,
		/// <summary>
		/// Furlon length unit.
		/// </summary>
		Furlong = 7920,
		/// <summary>
		/// Mile length unit.
		/// </summary>
		Mile = 63360,
		/// <summary>
		/// League length unit.
		/// </summary>
		League = 190080
	}

	/// <summary>
	/// Represents a data size, based on bytes multiples.
	/// </summary>
	[Serializable]
	public struct DataSize : IComparable, IComparable<DataSize>, IEquatable<DataSize>, Serialization.ISerializable
	{
		#region Fields

		private long _bValue;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the DataSize structure to specified bytes value.
		/// </summary>
		/// <param name="bytes">A value in bytes.</param>
		public DataSize(long bytes)
		{
			this._bValue = bytes;
		}

		/// <summary>
		/// Initializes a new instance of the LinearSize structure to specified value.
		/// </summary>
		/// <param name="value">A value.</param>
		/// <param name="mult">Greatness of value.</param>
		/// <exception cref="OverflowException">The value is greater than 8 Exabytes.</exception>
		public DataSize(long value, ByteMeasure mult)
		{
			this._bValue = value * (long)mult;
		}

		private DataSize(Serialization.SerializationInfo info, Serialization.StreamingContext context)
		{
			if (info == null)
				throw new ArgumentNullException("info", resExceptions.ArgumentNull.Replace("%var", "info"));

			_bValue = info.GetInt64("_bValue");
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the value represented by this instance in bytes.
		/// </summary>
		public long Bytes
		{
			get { return _bValue; }
			set { _bValue = value; }
		}

		/// <summary>
		/// Gets or sets the value represented by this instance in kilobytes.
		/// </summary>
		/// <exception cref="OverflowException">The value is greater than 8 Exabytes.</exception>
		public decimal Kilobytes
		{
			get { return (decimal)_bValue / (long)ByteMeasure.Kilobytes; }
			set { _bValue = (long)(value * (long)ByteMeasure.Kilobytes); }
		}

		/// <summary>
		/// Gets or sets the value represented by this instance in megabytes.
		/// </summary>
		/// <exception cref="OverflowException">The value is greater than 8 Exabytes.</exception>
		public decimal Megabytes
		{
			get { return (decimal)_bValue / (long)ByteMeasure.Megabytes; }
			set { _bValue = (long)(value * (long)ByteMeasure.Megabytes); }
		}

		/// <summary>
		/// Gets or sets the value represented by this instance in gigabytes.
		/// </summary>
		/// <exception cref="OverflowException">The value is greater than 8 Exabytes.</exception>
		public decimal Gigabytes
		{
			get { return (decimal)_bValue / (long)ByteMeasure.Gigabytes; }
			set { _bValue = (long)(value * (long)ByteMeasure.Gigabytes); }
		}

		/// <summary>
		/// Gets or sets the value represented by this instance in terabytes.
		/// </summary>
		/// <exception cref="OverflowException">The value is greater than 8 Exabytes.</exception>
		public decimal Terabytes
		{
			get { return (decimal)_bValue / (long)ByteMeasure.Terabytes; }
			set { _bValue = (long)(value * (long)ByteMeasure.Terabytes); }
		}

		/// <summary>
		/// Gets or sets the value represented by this instance in petabytes.
		/// </summary>
		/// <exception cref="OverflowException">The value is greater than 8 Exabytes.</exception>
		public decimal Petabytes
		{
			get { return (decimal)_bValue / (long)ByteMeasure.Petabytes; }
			set { _bValue = (long)(value * (long)ByteMeasure.Petabytes); }
		}

		/// <summary>
		/// Gets or sets the value represented by this instance in exabytes.
		/// </summary>
		/// <exception cref="OverflowException">The value is greater than 8 Exabytes.</exception>
		public decimal Exabytes
		{
			get { return (decimal)_bValue / (long)ByteMeasure.Exabytes; }
			set { _bValue = (long)(value * (long)ByteMeasure.Exabytes); }
		}

		#endregion

		#region Operators

		/// <summary>
		/// Adds two specified DataSize values.
		/// </summary>
		/// <param name="op1">A DataSize.</param>
		/// <param name="op2">A DataSize.</param>
		/// <returns>The DataSize result of adding op1 and op2.</returns>
		/// <exception cref="OverflowException">The return value is greater than <see cref="F:System.UInt64.MaxValue"/>.</exception>
		public static DataSize operator +(DataSize op1, DataSize op2)
		{
			return new DataSize(op1._bValue + op2._bValue);
		}

		/// <summary>
		/// Subtracts two specified DataSize values.
		/// </summary>
		/// <param name="op1">A DataSize.</param>
		/// <param name="op2">A DataSize.</param>
		/// <returns>The DataSize result of subtracting op1 from op2.</returns>
		public static DataSize operator -(DataSize op1, DataSize op2)
		{
			return new DataSize(op1._bValue - op2._bValue);
		}

		/// <summary>
		/// Multiplies two specified DataSize values.
		/// </summary>
		/// <param name="op1">A DataSize.</param>
		/// <param name="op2">A DataSize.</param>
		/// <returns>The DataSize result of multiplying op1 by op2.</returns>
		public static DataSize operator *(DataSize op1, DataSize op2)
		{
			return new DataSize(op1._bValue * op2._bValue);
		}

		/// <summary>
		/// Divides two specified DataSize values.
		/// </summary>
		/// <param name="op1">A DataSize (the dividend).</param>
		/// <param name="op2">A DataSize (the divisor).</param>
		/// <returns>The DataSize result from dividing of op1 by op2.</returns>
		public static DataSize operator /(DataSize op1, DataSize op2)
		{
			return new DataSize(op1._bValue / op2._bValue);
		}

		/// <summary>
		/// Returns the remainder resulting from dividing two specified DataSize values.
		/// </summary>
		/// <param name="op1">A DataSize.</param>
		/// <param name="op2">A DataSize.</param>
		/// <returns>The DataSize remainder resulting from dividing d1 by d2.</returns>
		public static DataSize operator %(DataSize op1, DataSize op2)
		{
			return new DataSize(op1._bValue % op2._bValue);
		}

		/// <summary>
		/// Negates the value of the specified DataSize operand.
		/// </summary>
		/// <param name="op">The DataSize operand.</param>
		/// <returns>The result of op multiplied by negative one (-1).</returns>
		public static DataSize operator -(DataSize op)
		{
			return new DataSize(op._bValue * -1);
		}

		/// <summary>
		/// Returns the value of the DataSize operand (the sign of the operand is unchanged).
		/// </summary>
		/// <param name="op">The DataSize operand.</param>
		/// <returns>The value of the operand, op.</returns>
		public static DataSize operator +(DataSize op)
		{
			return new DataSize(op._bValue);
		}

		/// <summary>
		/// Determines whether two specified instances of DataSize are equal.
		/// </summary>
		/// <param name="b">A DataSize.</param>
		/// <param name="m">A DataSize.</param>
		/// <returns>true if b and m represent the same binary measure value; otherwise, false.</returns>
		public static bool operator ==(DataSize b, DataSize m)
		{
			return b._bValue == m._bValue;
		}

		/// <summary>
		/// Determines whether two specified instances of DataSize are not equal.
		/// </summary>
		/// <param name="b">A DataSize.</param>
		/// <param name="m">A DataSize.</param>
		/// <returns>true if b and m do not represent the same binary measure value; otherwise, false</returns>
		public static bool operator !=(DataSize b, DataSize m)
		{
			return b._bValue != m._bValue;
		}

		/// <summary>
		/// Determines whether one specified DataSize is greater than another specified DataSize.
		/// </summary>
		/// <param name="b">A DataSize.</param>
		/// <param name="m">A DataSize.</param>
		/// <returns>true if b is greater than m; otherwise, false.</returns>
		public static bool operator >(DataSize b, DataSize m)
		{
			return b._bValue > m._bValue;
		}

		/// <summary>
		/// Determines whether one specified DataSize is greater than or equal to another specified DataSize.
		/// </summary>
		/// <param name="b">A DataSize.</param>
		/// <param name="m">A DataSize.</param>
		/// <returns>true if b is greater than or equal to m; otherwise, false.</returns>
		public static bool operator >=(DataSize b, DataSize m)
		{
			return b._bValue >= m._bValue;
		}

		/// <summary>
		/// Determines whether one specified DataSize is less than another specified DataSize.
		/// </summary>
		/// <param name="b">A DataSize.</param>
		/// <param name="m">A DataSize.</param>
		/// <returns>true if b is less than m; otherwise, false.</returns>
		public static bool operator <(DataSize b, DataSize m)
		{
			return b._bValue < m._bValue;
		}

		/// <summary>
		/// Determines whether one specified DataSize is less than or equal to another specified DataSize.
		/// </summary>
		/// <param name="b">A DataSize.</param>
		/// <param name="m">A DataSize.</param>
		/// <returns>true if b is less than or equal to m; otherwise, false.</returns>
		public static bool operator <=(DataSize b, DataSize m)
		{
			return b._bValue <= m._bValue;
		}

		#endregion

		#region Overrides

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public override int GetHashCode()
		{
			return ((int)_bValue ^ (int)(_bValue >> 32));
		}

		/// <summary>
		/// Returns a value indicating whether this instance and a specified <see cref="T:System.Object"/>
		/// represent the same type and value.
		/// </summary>
		/// <param name="obj">An object to compare to this instance.</param>
		/// <returns>true if value is a DataSize and equal to this instance; otherwise, false.</returns>
		public override bool Equals(object obj)
		{
			if (obj is DataSize)
				return (DataSize)obj == this;
			return false;
		}

		/// <summary>
		/// Converts the numeric value of this instance to its equivalent <see cref="T:System.String"/> representation
		/// </summary>
		/// <returns>A <see cref="T:System.String"/> representing the value of this instance.</returns>
		public override string ToString()
		{
			return this.ToString(null, NumberFormatInfo.CurrentInfo);
		}

		#endregion

		#region Methods

		/// <summary>
		/// Gets the value represented by this instance in the specified measure.
		/// </summary>
		/// <param name="mult">Specifies a measure.</param>
		/// <returns>The value represented by this instance in the specified measure.</returns>
		public decimal GetValue(ByteMeasure mult)
		{
			return (decimal)_bValue / (long)mult;
		}

		/// <summary>
		/// Converts the value of this instance to its equivalent string representation using the specified format.
		/// </summary>
		/// <param name="format">A format string.</param>
		/// <returns>A string representation of value of this instance as specified by <c>format</c>.</returns>
		/// <exception cref="FormatException">format does not contain a valid custom format pattern.</exception>
		/// <example>
		/// The following console application example show how to use DataSize.ToString method.
		/// <code>
		/// using System;
		/// using Root;
		/// 
		/// class Program
		/// {
		///     static void Main()
		///     {
		///			DataSize binMea = new DataSize();
		///			binMea.Megabytes = 15.317M;
		/// 
		///			Console.WriteLine(binMea.ToString("G|M-"));
		///				// output: "15.3169994354248046875 MB"
		/// 
		///			Console.WriteLine(binMea.ToString("G|M+"));
		///				// output: "15.3169994354248046875 Megabytes"
		/// 
		///			Console.WriteLine(binMea.ToString("N2|M-"));
		///				// output: "15.32 MB"
		/// 
		///			Console.WriteLine(binMea.ToString("G|K-"));
		///				// output: "15684.607421875 KB"
		/// 
		/// 		Console.WriteLine(binMea.ToString("N1|K-"));
		///				// output: "15,684.6 KB"
		/// 
		///			binMea.Gigabytes = 4000;
		/// 
		///			Console.WriteLine(binMea.ToString("G|>-"));
		///				// output: "3.90625 TB"
		/// 
		/// 		Console.WriteLine(binMea.ToString("N0|>-"));
		///				// output: "4 TB"
		/// 
		/// 		Console.WriteLine(binMea.ToString("G|0-"));
		///				// output: "4000 GB"
		/// 
		///         Console.ReadKey();
		///     }
		/// }
		/// </code>
		/// </example>
		public string ToString(string format)
		{
			return this.ToString(format, NumberFormatInfo.CurrentInfo);
		}

		/// <summary>
		/// Converts the value of this instance to its equivalent string representation using the specified format
		/// and culture-specific format information.
		/// </summary>
		/// <param name="format">A format string.</param>
		/// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
		/// <returns>A string representation of value of this instance as specified by <c>format</c> and <c>provider</c>.</returns>
		/// <exception cref="FormatException">format does not contain a valid custom format pattern.</exception>
		/// <example>
		/// The following console application example show how to use DataSize.ToString method.
		/// <code>
		/// using System;
		/// using System.Globalization;
		/// using Root;
		/// 
		/// class Program
		/// {
		///     static void Main()
		///     {
		///			DataSize binMea = new DataSize();
		///			binMea.Megabytes = 15.317M;
		/// 
		///			Console.WriteLine(binMea.ToString("G|M-", NumberFormatInfo.CurrentInfo));
		///				// output: "15.3169994354248046875 MB"
		/// 
		///			Console.WriteLine(binMea.ToString("G|M+", NumberFormatInfo.CurrentInfo));
		///				// output: "15.3169994354248046875 Megabytes"
		/// 
		///			Console.WriteLine(binMea.ToString("N2|M-", NumberFormatInfo.CurrentInfo));
		///				// output: "15.32 MB"
		/// 
		///			Console.WriteLine(binMea.ToString("G|K-", NumberFormatInfo.CurrentInfo));
		///				// output: "15684.607421875 KB"
		/// 
		/// 		Console.WriteLine(binMea.ToString("N1|K-", NumberFormatInfo.CurrentInfo));
		///				// output: "15,684.6 KB"
		/// 
		///			binMea.Gigabytes = 4000;
		/// 
		///			Console.WriteLine(binMea.ToString("G|>-", NumberFormatInfo.CurrentInfo));
		///				// output: "3.90625 TB"
		/// 
		/// 		Console.WriteLine(binMea.ToString("N0|>-", NumberFormatInfo.CurrentInfo));
		///				// output: "4 TB"
		/// 
		/// 		Console.WriteLine(binMea.ToString("G|0-", NumberFormatInfo.CurrentInfo));
		///				// output: "4000 GB"
		/// 
		///         Console.ReadKey();
		///     }
		/// }
		/// </code>
		/// </example>
		public string ToString(string format, IFormatProvider provider)
		{
			if (string.IsNullOrEmpty(format))
				format = "G";
			NumberFormatInfo nfi = NumberFormatInfo.GetInstance(provider);

			// Verify whether has the secondary format
			if (format.IndexOf('|') == -1)
				format = format + "|>-"; // if no, creates a standart format

			// ----- Split format-string, getting 2 formats -----
			string[] str = format.Split('|');

			if (str.Length != 2)
				throw new FormatException(resExceptions.Format_InvalidString);

			string fmtNumber = str[0];
			string fmtBinM = str[1].ToUpper();
			str = null;
			// --------------------------------------------------

			decimal value = 0.0M;	// stores a returned value
			string strMult = "";	// stores multiple spell (by select option)

			// ----- Process secondary format string -----
			if (fmtBinM.Length != 2)
				throw new FormatException(resExceptions.Format_InvalidString);

			string primaryChars = "BKMGTPE>0";	// allowed chars on first index
			string secondaryChars = "-+";		// allowed chars on second index

			int pIdx = primaryChars.IndexOf(fmtBinM[0]);	// stores index of first format-char on primaryChars
			int sIdx = secondaryChars.IndexOf(fmtBinM[1]);	// stores index of second format-char on secondaryChars

			if (pIdx == -1 || sIdx == -1)		// Verify whether has a invalid chars
				throw new FormatException(resExceptions.Format_InvalidString);

			ByteMeasure toBMult;		// Verify selected option
			if (pIdx < 7)
				toBMult = GetMultByDefined(primaryChars[pIdx]);
			else if (pIdx == 7)
				toBMult = GetMultByGreat();
			else
				toBMult = GetMultByZeroable();

			value = (decimal)this._bValue / (long)toBMult;	// stores the value in specified multiple

			// Define the multiple spell
			strMult = Enum.GetName(typeof(ByteMeasure), toBMult);
			if (sIdx == 0)
				strMult = strMult[0] == 'B' ? "B" : strMult[0] + "B";
			// -------------------------------------------

			return value.ToString(fmtNumber, nfi) + " " + strMult;
		}

		private ByteMeasure GetMultByDefined(char greatness)
		{
			string multiples = "BKMGTPE";

			int idx = multiples.IndexOf(greatness);

			if (idx == -1)
				return ByteMeasure.Bytes;
			else
			{
				long[] val = (long[])Enum.GetValues(typeof(ByteMeasure));
				return (ByteMeasure)val[idx];
			}
		}

		private ByteMeasure GetMultByGreat()
		{
			long[] values = (long[])Enum.GetValues(typeof(ByteMeasure));
			long bytes = Math.Abs(this._bValue);

			for (int i = values.Length - 1; i >= 0; i--)
			{
				if (bytes >= values[i])
					return (ByteMeasure)values[i];
			}

			return ByteMeasure.Bytes;
		}

		private ByteMeasure GetMultByZeroable()
		{
			if (this._bValue == 0)
				return ByteMeasure.Bytes;

			long[] values = (long[])Enum.GetValues(typeof(ByteMeasure));
			long bytes = Math.Abs(this._bValue);

			for (int i = values.Length - 1; i > 0; i--)
			{
				decimal val = (decimal)this._bValue / values[i];
				if ((long)val == val)
					return (ByteMeasure)values[i];
			}

			return GetMultByGreat();
		}

		#endregion

		#region IComparable Members

		/// <summary>
		/// Compares this instance to a specified object and returns an indication of their relative values.
		/// </summary>
		/// <param name="obj">A boxed DataSize object to compare, or null.</param>
		/// <returns>
		/// <para>A signed number indicating the relative values of this instance and value.</para>
		/// <para>Value Description:</para>
		/// <para>- Less than zero: This instance is less than value.</para>
		/// <para>- Zero: This instance is equal to value.</para>
		/// <para>- Greater than zero: This instance is greater than value, or value is null.</para></returns>
		public int CompareTo(object obj)
		{
			if (obj == null)
				return 1;

			if (!(obj is DataSize))
				throw new ArgumentException(resExceptions.Obj_MustBeType.Replace("%var", "DataSize"));

			DataSize other = (DataSize)obj;
			if (this > other)
				return 1;
			if (this < other)
				return -1;
			return 0;
		}

		#endregion

		#region IComparable<DataSize> Members

		/// <summary>
		/// Compares this instance to a specified DataSize object and returns an indication of their relative values.
		/// </summary>
		/// <param name="other">A DataSize object to compare.</param>
		/// <returns>
		/// <para>A signed number indicating the relative values of this instance and the value parameter.</para>
		/// <para>Value Description:</para>
		/// <para>- Less than zero: This instance is less than value.</para>
		/// <para>- Zero: This instance is equal to value.</para>
		/// <para>- Greater than zero: This instance is greater than value.</para></returns>
		public int CompareTo(DataSize other)
		{
			if (this > other)
				return 1;
			if (this < other)
				return -1;
			return 0;
		}

		#endregion

		#region IEquatable<DataSize> Members

		/// <summary>
		/// Returns a value indicating whether this instance is equal to the specified DataSize instance.
		/// </summary>
		/// <param name="other">A DataSize instance to compare to this instance.</param>
		/// <returns>true if the value parameter equals the value of this instance; otherwise, false.</returns>
		public bool Equals(DataSize other)
		{
			return this == other;
		}

		#endregion

		#region ISerializable Members

		void Serialization.ISerializable.GetObjectData(Serialization.SerializationInfo info, Serialization.StreamingContext context)
		{
			if (info == null)
				throw new ArgumentNullException("info", resExceptions.ArgumentNull.Replace("%var", "info"));

			info.AddValue("_bValue", _bValue);
		}

		#endregion
	}

	/// <summary>
	/// Represents a size of length, and ables a conversion between measures.
	/// </summary>
	[Serializable]
	public struct LengthSize : IComparable, IComparable<LengthSize>, IEquatable<LengthSize>, Serialization.ISerializable
	{
		#region Fields

		private decimal _val;
		private decimal _usVal;
		private bool _isImperial;

		public const SILengthUnit DefaultSIUnit = SILengthUnit.Meter;
		public const ImperialLengthUnit DefaultImperialUnit = ImperialLengthUnit.Yard;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the LinearSize structure to specified value and measure.
		/// </summary>
		/// <param name="value">Value of measurement.</param>
		/// <param name="unit">Linear measure unit of specified value.</param>
		public LengthSize(decimal value, SILengthUnit unit)
			: this()
		{
			SetSI(value, unit);
		}

		/// <summary>
		/// Initializes a new instance of the LinearSize structure to specified value and measure.
		/// </summary>
		/// <param name="value">Value of measurement.</param>
		/// <param name="unit">Linear measure unit of specified value.</param>
		public LengthSize(decimal value, ImperialLengthUnit unit)
			: this()
		{
			SetImperial(value, unit);
		}

		private LengthSize(Serialization.SerializationInfo info, Serialization.StreamingContext context)
			: this()
		{
			if (info == null)
				throw new ArgumentNullException("info", resExceptions.ArgumentNull.Replace("%var", "info"));

			_isImperial = info.GetBoolean("_isImperial");
			if (!_isImperial)
				this.SetSI(info.GetDecimal("_val"), DefaultSIUnit);
			else
				this.SetImperial(info.GetDecimal("_usVal"), DefaultImperialUnit);
		}

		#endregion

		#region Static Methods

		/// <summary>
		/// Creates a LinearSize structure based in the specified pixels and dpi (Dots per Inch) resolution.
		/// </summary>
		/// <param name="pixels">Measure in pixels.</param>
		/// <param name="dpi">dpi resolution.</param>
		/// <returns>A LinearSize structure.</returns>
		public static LengthSize PixelsPerInches(long pixels, float dpi)
		{
			decimal num = pixels / Convert.ToDecimal(dpi);
			LengthSize lm = new LengthSize(num, ImperialLengthUnit.Inch);
			return lm;
		}

		/// <summary>
		/// Creates a LinearSize structure based in the specified pixels and ppm (Points per Millimeters).
		/// </summary>
		/// <param name="pixels">Measure in pixels.</param>
		/// <param name="ppm">ppm resolution.</param>
		/// <returns>A LinearSize structure.</returns>
		public static LengthSize PixelsPerMillimeters(long pixels, float ppm)
		{
			decimal num = pixels / Convert.ToDecimal(ppm);
			LengthSize mu = new LengthSize(num, SILengthUnit.Millimeter);
			return mu;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the value represented by this instance in millimeters.
		/// </summary>
		public decimal Millimeters
		{
			get
			{ return GetSI(SILengthUnit.Millimeter); }
			set
			{ SetSI(value, SILengthUnit.Millimeter); }
		}

		/// <summary>
		/// Gets or sets the value represented by this instance in centimeters.
		/// </summary>
		public decimal Centimeters
		{
			get
			{ return GetSI(SILengthUnit.Centimeter); }
			set
			{ SetSI(value, SILengthUnit.Centimeter); }
		}

		/// <summary>
		/// Gets or sets the value represented by this instance in decimeters.
		/// </summary>
		public decimal Decimeters
		{
			get
			{ return GetSI(SILengthUnit.Decimeter); }
			set
			{ SetSI(value, SILengthUnit.Decimeter); }
		}

		/// <summary>
		/// Gets or sets the value represented by this instance in meters.
		/// </summary>
		public decimal Meters
		{
			get
			{ return GetSI(SILengthUnit.Meter); }
			set
			{ SetSI(value, SILengthUnit.Meter); }
		}

		/// <summary>
		/// Gets or sets the value represented by this instance in decameters.
		/// </summary>
		public decimal Decameters
		{
			get
			{ return GetSI(SILengthUnit.Decameter); }
			set
			{ SetSI(value, SILengthUnit.Decameter); }
		}

		/// <summary>
		/// Gets or sets the value represented by this instance in hectometers.
		/// </summary>
		public decimal Hectometers
		{
			get
			{ return GetSI(SILengthUnit.Hectometer); }
			set
			{ SetSI(value, SILengthUnit.Hectometer); }
		}

		/// <summary>
		/// Gets or sets the value represented by this instance in kilometers.
		/// </summary>
		public decimal Kilometers
		{
			get
			{ return GetSI(SILengthUnit.Kilometer); }
			set
			{ SetSI(value, SILengthUnit.Kilometer); }
		}

		/// <summary>
		/// Gets or sets the value represented by this instance in inches.
		/// </summary>
		public decimal Inches
		{
			get
			{ return GetImperial(ImperialLengthUnit.Inch); }
			set
			{ SetImperial(value, ImperialLengthUnit.Inch); }
		}

		/// <summary>
		/// Gets or sets the value represented by this instance in feet.
		/// </summary>
		public decimal Feet
		{
			get
			{ return GetImperial(ImperialLengthUnit.Foot); }
			set
			{ SetImperial(value, ImperialLengthUnit.Foot); }
		}

		/// <summary>
		/// Gets or sets the value represented by this instance in yards.
		/// </summary>
		public decimal Yards
		{
			get
			{ return GetImperial(ImperialLengthUnit.Yard); }
			set
			{ SetImperial(value, ImperialLengthUnit.Yard); }
		}

		/// <summary>
		/// Gets or sets the value represented by this instance in furlongs.
		/// </summary>
		public decimal Furlongs
		{
			get
			{ return GetImperial(ImperialLengthUnit.Furlong); }
			set
			{ SetImperial(value, ImperialLengthUnit.Furlong); }
		}

		/// <summary>
		/// Gets or sets the value represented by this instance in miles.
		/// </summary>
		public decimal Miles
		{
			get
			{ return GetImperial(ImperialLengthUnit.Mile); }
			set
			{ SetImperial(value, ImperialLengthUnit.Mile); }
		}

		/// <summary>
		/// Gets or sets the value represented by this instance in leagues.
		/// </summary>
		public decimal Leagues
		{
			get
			{ return GetImperial(ImperialLengthUnit.League); }
			set
			{ SetImperial(value, ImperialLengthUnit.League); }
		}

		#endregion

		#region Operators

		/// <summary>
		/// Adds two specified LinearSize values.
		/// </summary>
		/// <param name="op1">A LinearSize.</param>
		/// <param name="op2">A LinearSize.</param>
		/// <returns>The LinearSize result of adding op1 and op2.</returns>
		public static LengthSize operator +(LengthSize op1, LengthSize op2)
		{
			LengthSize lm = new LengthSize();
			lm._val = op1._val + op2._val;
			lm._usVal = op1._usVal + op2._usVal;
			lm._isImperial = op1._isImperial == op2._isImperial;
			return lm;
		}

		/// <summary>
		/// Subtracts two specified LinearSize values.
		/// </summary>
		/// <param name="op1">A LinearSize.</param>
		/// <param name="op2">A LinearSize.</param>
		/// <returns>The LinearSize result of subtracting op1 from op2.</returns>
		public static LengthSize operator -(LengthSize op1, LengthSize op2)
		{
			LengthSize lm = new LengthSize();
			lm._val = op1._val - op2._val;
			lm._usVal = op1._usVal - op2._usVal;
			lm._isImperial = (op1._isImperial == op2._isImperial);
			return lm;
		}

		/// <summary>
		/// Multiplies two specified LinearSize values.
		/// </summary>
		/// <param name="op1">A LinearSize.</param>
		/// <param name="op2">A LinearSize.</param>
		/// <returns>The LinearSize result of multiplying op1 by op2.</returns>
		public static LengthSize operator *(LengthSize op1, LengthSize op2)
		{
			LengthSize lm = new LengthSize();
			lm._val = op1._val * op2._val;
			lm._usVal = op1._usVal * op2._usVal;
			lm._isImperial = (op1._isImperial == op2._isImperial);
			return lm;
		}

		/// <summary>
		/// Divides two specified LinearSize values.
		/// </summary>
		/// <param name="op1">A LinearSize (the dividend).</param>
		/// <param name="op2">A LinearSize (the divisor).</param>
		/// <returns>The LinearSize result of op1 by op2.</returns>
		public static LengthSize operator /(LengthSize op1, LengthSize op2)
		{
			LengthSize lm = new LengthSize();
			lm._val = op1._val / op2._val;
			lm._usVal = op1._usVal / op2._usVal;
			lm._isImperial = (op1._isImperial == op2._isImperial);
			return lm;
		}

		/// <summary>
		/// Returns the remainder resulting from dividing two specified LinearSize values.
		/// </summary>
		/// <param name="op1">A LinearSize.</param>
		/// <param name="op2">A LinearSize.</param>
		/// <returns>The LinearSize remainder resulting from dividing d1 by d2.</returns>
		public static LengthSize operator %(LengthSize op1, LengthSize op2)
		{
			LengthSize lm = new LengthSize();
			lm._val = op1._val % op2._val;
			lm._usVal = op1._usVal % op2._usVal;
			lm._isImperial = (op1._isImperial == op2._isImperial);
			return lm;
		}

		/// <summary>
		/// Determines whether two specified instances of LinearSize are equal.
		/// </summary>
		/// <param name="l">A LinearSize.</param>
		/// <param name="m">A LinearSize.</param>
		/// <returns>true if l and m represent the same linear measure value; otherwise, false.</returns>
		public static bool operator ==(LengthSize l, LengthSize m)
		{
			return (l._val == m._val) && (l._usVal == m._usVal);
		}

		/// <summary>
		/// Determines whether two specified instances of LinearSize are not equal.
		/// </summary>
		/// <param name="l">A LinearSize.</param>
		/// <param name="m">A LinearSize.</param>
		/// <returns>true if l and m do not represent the same linear measure value; otherwise, false</returns>
		public static bool operator !=(LengthSize l, LengthSize m)
		{
			return (l._val != m._val) || (l._usVal != m._usVal);
		}

		/// <summary>
		/// Determines whether one specified LinearSize is greater than another specified LinearSize.
		/// </summary>
		/// <param name="l">A LinearSize.</param>
		/// <param name="m">A LinearSize.</param>
		/// <returns>true if l is greater than m; otherwise, false.</returns>
		public static bool operator >(LengthSize l, LengthSize m)
		{
			return l._val > m._val;
		}

		/// <summary>
		/// Determines whether one specified LinearSize is greater than or equal to another specified LinearSize.
		/// </summary>
		/// <param name="l">A LinearSize.</param>
		/// <param name="m">A LinearSize.</param>
		/// <returns>true if l is greater than or equal to m; otherwise, false.</returns>
		public static bool operator >=(LengthSize l, LengthSize m)
		{
			return l._val >= m._val;
		}

		/// <summary>
		/// Determines whether one specified LinearSize is less than another specified LinearSize.
		/// </summary>
		/// <param name="l">A LinearSize.</param>
		/// <param name="m">A LinearSize.</param>
		/// <returns>true if l is less than m; otherwise, false.</returns>
		public static bool operator <(LengthSize l, LengthSize m)
		{
			return l._val < m._val;
		}

		/// <summary>
		/// Determines whether one specified LinearSize is less than or equal to another specified LinearSize.
		/// </summary>
		/// <param name="l">A LinearSize.</param>
		/// <param name="m">A LinearSize.</param>
		/// <returns>true if l is less than or equal to m; otherwise, false.</returns>
		public static bool operator <=(LengthSize l, LengthSize m)
		{
			return l._val <= m._val;
		}

		#endregion

		#region Overrides

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public override int GetHashCode()
		{
			return _val.GetHashCode();
		}

		/// <summary>
		/// Returns a value indicating whether this instance and a specified <see cref="T:System.Object"/>
		/// represent the same type and value.
		/// </summary>
		/// <param name="obj">An object to compare to this instance.</param>
		/// <returns>true if value is a LinearSize and equal to this instance; otherwise, false.</returns>
		public override bool Equals(object obj)
		{
			if (obj is LengthSize)
				return (LengthSize)obj == this;

			return false;
		}

		/// <summary>
		/// Converts the numeric value of this instance to its equivalent <see cref="T:System.String"/> representation
		/// </summary>
		/// <returns>A <see cref="T:System.String"/> representing the value of this instance.</returns>
		public override string ToString()
		{
			int[] lmValues;
			decimal workVal;
			if (!_isImperial)
			{
				lmValues = (int[])Enum.GetValues(typeof(SILengthUnit));
				workVal = _val * (decimal)DefaultSIUnit;
			}
			else
			{
				lmValues = (int[])Enum.GetValues(typeof(ImperialLengthUnit));
				workVal = _usVal * (decimal)DefaultImperialUnit;
			}

			int mult = workVal < 0 ? -1 : 1;
			workVal *= mult;
			for (int i = lmValues.Length - 1; i >= 0; i--)
			{
				if (workVal >= lmValues[i])
				{
					float val = (float)((workVal * mult) / lmValues[i]);
					string name;
					if (!_isImperial)
						name = GetSIName(lmValues[i], (decimal)val);
					else
						name = GetImperialName(lmValues[i], i, (decimal)val);

					return val.ToString() + " " + name;
				}
			}

			if (!_isImperial)
				return "0 " + GetSIName((int)DefaultSIUnit, 0M);
			else
				return "0 " + GetImperialName((int)DefaultImperialUnit, 0, 0M);
		}

		#endregion

		#region Methods

		/// <summary>
		/// Gets the value represented by this instance in the specified SI unit.
		/// </summary>
		/// <param name="unit">Specifies a SI unit.</param>
		/// <returns>The value represented by this instance in the specified unit.</returns>
		public decimal GetValue(SILengthUnit unit)
		{
			return GetSI(unit);
		}

		/// <summary>
		/// Gets the value represented by this instance in the specified Imperial unit.
		/// </summary>
		/// <param name="unit">Specifies a Imperial unit.</param>
		/// <returns>The value represented by this instance in the specified unit.</returns>
		public decimal GetValue(ImperialLengthUnit unit)
		{
			return GetImperial(unit);
		}

		private void SetSI(decimal value, SILengthUnit unit)
		{
			_val = value * ((decimal)unit / (decimal)DefaultSIUnit);
			_usVal = _val / 0.9144m;
			_isImperial = false;
		}

		private void SetImperial(decimal value, ImperialLengthUnit unit)
		{
			_usVal = value * ((decimal)unit / (decimal)DefaultImperialUnit);
			_val = _usVal * 0.9144m;
			_isImperial = true;
		}

		private decimal GetSI(SILengthUnit unit)
		{
			return _val / ((decimal)unit / (decimal)DefaultSIUnit);
		}

		private decimal GetImperial(ImperialLengthUnit unit)
		{
			return _usVal / ((decimal)unit / (decimal)DefaultImperialUnit);
		}

		private static string GetSIName(int enumvalue, decimal value)
		{
			string name;
			if (value == 0M || value == 1M || value == -1M)
				name = Enum.GetName(typeof(SILengthUnit), enumvalue);
			else
				name = Enum.GetName(typeof(SILengthUnit), enumvalue) + "s";
			return name;
		}

		private static string GetImperialName(int enumvalue, int index, decimal value)
		{
			string name;
			if (value == 0M || value == 1M || value == -1M)
				name = Enum.GetName(typeof(ImperialLengthUnit), enumvalue);
			else
			{
				string[] names = new string[] { "Inches", "Feet", "Yards", "Furlongs", "Miles", "Leagues" };
				name = names[index];
			}
			return name;
		}

		#endregion

		#region IComparable Members

		/// <summary>
		/// Compares this instance to a specified object and returns an indication of their relative values.
		/// </summary>
		/// <param name="obj">A boxed LinearSize object to compare, or null.</param>
		/// <returns>
		/// <para>A signed number indicating the relative values of this instance and value.</para>
		/// <para>Value Description:</para>
		/// <para>- Less than zero: This instance is less than value.</para>
		/// <para>- Zero: This instance is equal to value.</para>
		/// <para>- Greater than zero: This instance is greater than value, or value is null.</para></returns>
		public int CompareTo(object obj)
		{
			if (obj == null)
				return 1;

			if (!(obj is LengthSize))
				throw new ArgumentException(resExceptions.Obj_MustBeType.Replace("%var", "LinearSize"));

			LengthSize other = (LengthSize)obj;
			if (this > other)
				return 1;
			if (this < other)
				return -1;
			return 0;
		}

		#endregion

		#region IComparable<LinearSize> Members

		/// <summary>
		/// Compares this instance to a specified LinearSize object and returns an indication of their relative values.
		/// </summary>
		/// <param name="other">A LinearSize object to compare.</param>
		/// <returns>
		/// <para>A signed number indicating the relative values of this instance and the value parameter.</para>
		/// <para>Value Description:</para>
		/// <para>- Less than zero: This instance is less than value.</para>
		/// <para>- Zero: This instance is equal to value.</para>
		/// <para>- Greater than zero: This instance is greater than value.</para></returns>
		public int CompareTo(LengthSize other)
		{
			if (this > other)
				return 1;
			if (this < other)
				return -1;
			return 0;
		}

		#endregion

		#region IEquatable<LinearSize> Members

		/// <summary>
		/// Returns a value indicating whether this instance is equal to the specified LinearSize instance.
		/// </summary>
		/// <param name="other">A LinearSize instance to compare to this instance.</param>
		/// <returns>true if the value parameter equals the value of this instance; otherwise, false.</returns>
		public bool Equals(LengthSize other)
		{
			return this == other;
		}

		#endregion

		#region ISerializable Members

		void Serialization.ISerializable.GetObjectData(Serialization.SerializationInfo info, Serialization.StreamingContext context)
		{
			if (info == null)
				throw new ArgumentNullException("info", resExceptions.ArgumentNull.Replace("%var", "info"));

			if (!_isImperial)
				info.AddValue("_val", _val);
			else
				info.AddValue("_usVal", _usVal);
			info.AddValue("_isImperial", _isImperial);
		}

		#endregion
	}


	/// <summary>
	/// Stores a complete telephone number; includes country code, area code and
	/// subscriber number.
	/// </summary>
	public struct Telephone: IEquatable<Telephone>
	{
		#region Fields

		private short _country; //Globalization.CountryCallingCodes country;
		private short _countryLen;
		private short _area;
		private short _areaLen;
		private int _number;
		private short _numberLen;

		#endregion

		#region Constructors

		public Telephone(short countryCode, short areaCode, int subscriberNumber)
		{
			if (countryCode > 999)
				throw new OverflowException(resExceptions.TooBig_Number.Replace("%var", "countryCode").Replace("%value", "999"));
			if (subscriberNumber < 100)
				throw new OverflowException(resExceptions.TooSmall_Number.Replace("%var", "subscriberNumber")
					.Replace("%value", "100"));
			_country = countryCode; //(Globalization.CountryCallingCodes)countryCode;
			_area = areaCode;
			_number = subscriberNumber;

			_countryLen = (short)_country.ToString().Length;
			_areaLen = (short)_area.ToString().Length;
			_numberLen = (short)_number.ToString().Length;
		}

		#endregion

		#region Operators

		/// <summary>
		/// Determines whether two specified instances of Telephone are equal.
		/// </summary>
		/// <param name="l">A Telephone.</param>
		/// <param name="m">A Telephone.</param>
		/// <returns>true if tel1 and tel2 represent the same telephone value; otherwise, false.</returns>
		public static bool operator ==(Telephone tel1, Telephone tel2)
		{
			return (tel1._area == tel2._area) && (tel1._country == tel2._country) && (tel1._number == tel2._number);
		}

		/// <summary>
		/// Determines whether two specified instances of Telephone are not equal.
		/// </summary>
		/// <param name="l">A Telephone.</param>
		/// <param name="m">A Telephone.</param>
		/// <returns>true if tel1 and tel2 do not represent the same telephone value; otherwise, false</returns>
		public static bool operator !=(Telephone tel1, Telephone tel2)
		{
			return (tel1._area != tel2._area) || (tel1._country != tel2._country) || (tel1._number != tel2._number);
		}

		#endregion

		#region Overrides

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public override int GetHashCode()
		{
			return _country.GetHashCode() ^ _area.GetHashCode() ^ _number.GetHashCode();
		}

		/// <summary>
		/// Returns a value indicating whether this instance and a specified <see cref="T:System.Object"/>
		/// represent the same type and value.
		/// </summary>
		/// <param name="obj">An object to compare to this instance.</param>
		/// <returns>true if value is a Telephone and equal to this instance; otherwise, false.</returns>
		public override bool Equals(object obj)
		{
			if (obj is Telephone)
				return (Telephone)obj == this;
			return false;
		}

		/// <summary>
		/// Converts the numeric value of this instance to its equivalent <see cref="T:System.String"/> representation.
		/// </summary>
		/// <returns>A <see cref="T:System.String"/> representing the value of this instance.</returns>
		public override string ToString()
		{
			return this.ToString(null);
		}

		#endregion

		#region Methods

		public string ToString(string format)
		{
			if (string.IsNullOrEmpty(format))
				format = "+cc (ac) sn";
			format = format.Replace("cc", Formatting.Numbers.ToStringKeepLength(_country, _countryLen));
			format = format.Replace("ac", Formatting.Numbers.ToStringKeepLength(_area, _areaLen));
			format = format.Replace("sn", Formatting.Numbers.ToStringKeepLength(_number, _numberLen));
			return format;
		}

		public string ToBasicTelephoneString()
		{
			return string.Empty;
		}

		public string ToLocalTelephoneString()
		{
			return string.Empty;
		}

		public string ToCompleteTelephoneString()
		{
			return string.Empty;
		}

		public static Telephone FromString(string tel, string mask)
		{
			return new Telephone();
		}

		#endregion

		/// <summary>
		/// Alphabetic mnemonic system used to telephone numbers.
		/// </summary>
		/// <remarks>
		/// Another oddity of NANP telephone numbering is the popularity of alphabetic dialing. On most US and Canadian telephones,
		/// three letters appear on each number button from 2 through 9. This accommodates 24 letters. Historically, the letters Q
		/// and Z were omitted, though on some modern telephones, they are added.
		/// </remarks>
		public enum AlphabeticMnemonicSystem
		{
			/// <summary>
			/// Letter "A" is button 2.
			/// </summary>
			A = 2,
			/// <summary>
			/// Letter "B" is button 2.
			/// </summary>
			B = 2,
			/// <summary>
			/// Letter "C" is button 2.
			/// </summary>
			C = 2,
			/// <summary>
			/// Letter "D" is button 3.
			/// </summary>
			D = 3,
			/// <summary>
			/// Letter "E" is button 3.
			/// </summary>
			E = 3,
			/// <summary>
			/// Letter "F" is button 3.
			/// </summary>
			F = 3,
			/// <summary>
			/// Letter "G" is button 4.
			/// </summary>
			G = 4,
			/// <summary>
			/// Letter "H" is button 4.
			/// </summary>
			H = 4,
			/// <summary>
			/// Letter "I" is button 4.
			/// </summary>
			I = 4,
			/// <summary>
			/// Letter "J" is button 5.
			/// </summary>
			J = 5,
			/// <summary>
			/// Letter "K" is button 5.
			/// </summary>
			K = 5,
			/// <summary>
			/// Letter "L" is button 5.
			/// </summary>
			L = 5,
			/// <summary>
			/// Letter "M" is button 6.
			/// </summary>
			M = 6,
			/// <summary>
			/// Letter "N" is button 6.
			/// </summary>
			N = 6,
			/// <summary>
			/// Letter "O" is button 6.
			/// </summary>
			O = 6,
			/// <summary>
			/// Letter "P" is button 7.
			/// </summary>
			P = 7,
			/// <summary>
			/// Letter "Q" is button 7.
			/// </summary>
			Q = 7,
			/// <summary>
			/// Letter "R" is button 7.
			/// </summary>
			R = 7,
			/// <summary>
			/// Letter "S" is button 7.
			/// </summary>
			S = 7,
			/// <summary>
			/// Letter "T" is button 8.
			/// </summary>
			T = 8,
			/// <summary>
			/// Letter "U" is button 8.
			/// </summary>
			U = 8,
			/// <summary>
			/// Letter "V" is button 8.
			/// </summary>
			V = 8,
			/// <summary>
			/// Letter "W" is button 9.
			/// </summary>
			W = 9,
			/// <summary>
			/// Letter "X" is button 9.
			/// </summary>
			X = 9,
			/// <summary>
			/// Letter "Y" is button 9.
			/// </summary>
			Y = 9,
			/// <summary>
			/// Letter "Z" is button 9.
			/// </summary>
			Z = 9
		}

		#region IEquatable<Telephone> Members

		/// <summary>
		/// Returns a value indicating whether this instance is equal to the specified Telephone instance.
		/// </summary>
		/// <param name="other">A Telephone instance to compare to this instance.</param>
		/// <returns>true if the value parameter equals the value of this instance; otherwise, false.</returns>
		public bool Equals(Telephone other)
		{
			return this == other;
		}

		#endregion
	}

	/// <summary>
	/// Provides basic information from current operating system.
	/// </summary>
	public static class OSInformation
	{
		#region Fields

		private static bool _isAspNetServer;
		private static bool _isPostWin2k;
		private static bool _isWin2k;
		private static bool _isWin2k3;
		private static bool _isWin9x;
		private static bool _isWinHttp51;
		private static bool _isWinNt;
		private static OSVersion _os;

		#endregion

		#region Properties

		/// <summary>
		/// Gets whether this machine is ASP.NET Server.
		/// </summary>
		public static bool IsAspNetServer
		{
			get { return _isAspNetServer; }
		}

		/// <summary>
		/// Gets whether current operating system is newest than Windows 2000.
		/// </summary>
		public static bool IsPostWindows2000
		{
			get { return _isPostWin2k; }
		}

		/// <summary>
		/// Gets whether current operating system is Windows 2000.
		/// </summary>
		public static bool IsWindows2000
		{
			get { return _isWin2k; }
		}

		/// <summary>
		/// Gets whether current operating system is Windows 2003.
		/// </summary>
		public static bool IsWindows2003
		{
			get { return _isWin2k3; }
		}

		/// <summary>
		/// Gets whether current operating system is Windows 9x.
		/// </summary>
		public static bool IsWindows9x
		{
			get { return _isWin9x; }
		}

		/// <summary>
		/// Gets whether current machine have installed HTTP 5.1.
		/// </summary>
		public static bool IsHttp51
		{
			get { return _isWinHttp51; }
		}

		/// <summary>
		/// Gets whether current operating system is Windows NT Family.
		/// </summary>
		public static bool IsWindowsNT
		{
			get { return _isWinNt; }
		}

		/// <summary>
		/// Gets current operating system version.
		/// </summary>
		public static OSVersion GetOSVersion
		{
			get { return _os; }
		}

		#endregion

		#region Constructor

		static OSInformation()
		{
			OperatingSystem os = Environment.OSVersion;
			int major = os.Version.Major;
			int minor = os.Version.Minor;

			if (os.Platform == PlatformID.Win32Windows)
			{
				if (major == 4)
				{
					if (minor == 0)
						_os = OSVersion.Windows95;
					else if (minor == 1)
						_os = OSVersion.Windows98;
					else if (minor == 2)
						_os = OSVersion.Windows98SE;
					else if (minor == 9)
						_os = OSVersion.WindowsME;
					else
						_os = OSVersion.Unidentifield;
				}
				_isWin9x = true;
			}
			else if (os.Platform == PlatformID.Win32NT)
			{
				try
				{
					_isAspNetServer =
						System.Threading.Thread.GetDomain().GetData(".appDomain") != null;
				}
				catch { }

				_isWinNt = true;
				//_isWin2k = true;
				if (major == 4)
					_os = OSVersion.WindowsNT4;
				else if (major == 5)
				{
					if (minor == 0)
					{
						_os = OSVersion.Windows2000;
						_isWin2k = true;
						_isWinHttp51 = os.Version.MajorRevision >= 3;
					}
					else
					{
						_isPostWin2k = true;
						if (minor == 1)
						{
							_os = OSVersion.WindowsXP;
							_isWinHttp51 = os.Version.MajorRevision >= 1;
						}
						else
						{
							_isWinHttp51 = true;
							_isWin2k3 = true;
							if (minor == 2)
								_os = OSVersion.WindowsServer2003;
						}
					}
				}
				else if (major == 6)
				{
					_os = OSVersion.WindowsVista;
					_isPostWin2k = true;
					_isWinHttp51 = true;	// probably newest
				}
				else if (major > 6)
				{
					_os = OSVersion.WindowsNew;
					_isPostWin2k = true;
					_isWinHttp51 = true;	// propably newest
				}
				else
					_os = OSVersion.Unidentifield;
			}
			else if (os.Platform == PlatformID.WinCE)
			{
				if (major < 3)
					_os = OSVersion.WindowsCEOld;
				else if (major == 3)
					_os = OSVersion.WindowsCE3;
				else if (major == 4)
					_os = OSVersion.WindowsCE4;
				else if (major == 5)
					_os = OSVersion.WindowsCE5;
				else if (major == 6)
					_os = OSVersion.WindowsCE6;
				else
					_os = OSVersion.WindowsCENew;
			}
			else
				_os = OSVersion.Unidentifield;
		}

		#endregion
	}

	/// <summary>
	/// Stores a object protected by multiaccess.
	/// </summary>
	/// <typeparam name="T">Type of object to protect.</typeparam>
	public class LockedMultiAccess<T>
	{
		#region Fields

		private T userVar;
		private object isLocked;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes LockedMultiAccess.
		/// </summary>
		public LockedMultiAccess()
		{
			isLocked = (object)false;
		}

		/// <summary>
		/// Initializes LockedMultiAccess and stores a initial value.
		/// </summary>
		/// <param name="value"></param>
		public LockedMultiAccess(T value)
		{
			userVar = value;
			isLocked = (object)false;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the object stored by this instance directly.
		/// </summary>
		public T DirectAccess
		{
			get { return userVar; }
			set { userVar = value; }
		}

		/// <summary>
		/// Gets or sets the object stored by this instance; If the object is locked then
		/// waits to unlock.
		/// </summary>
		public T SafeAccess
		{
			get
			{
				LockObject();
				T obj = userVar;
				UnLockObject();
				return obj;
			}
			set
			{
				LockObject();
				userVar = value;
				UnLockObject();
			}
		}

		/// <summary>
		/// Gets a boolean indicating whether current object is locked.
		/// </summary>
		public bool IsLocked
		{
			get
			{
				if (!System.Threading.Monitor.TryEnter(isLocked))
					return true;
				System.Threading.Monitor.Exit(isLocked);
				return false;
				//System.Threading.Thread.Sleep(1);
				//bool b1 = (bool)isLocked;
				//return b1;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Locks stored object to be accessed by <see cref="SafeAccess"/>, if the object is locked then
		/// waits to unlock.
		/// </summary>
		public void LockObject()
		{
			System.Threading.Monitor.Enter(this.isLocked);
		//inicio:
		//    while ((bool)isLocked)
		//    {
		//        System.Threading.Thread.Sleep(1);
		//    }
		//    if (!internalIsLocked(true))
		//        goto inicio;
		}

		/// <summary>
		/// Unlocks stored object to be accessed by <see cref="SafeAccess"/>.
		/// </summary>
		public void UnLockObject()
		{
			System.Threading.Monitor.Exit(this.isLocked);
			//internalIsLocked(false);
			//System.Threading.Thread.Sleep(1);
		}

		//private bool internalIsLocked(bool value)
		//{
		//    lock (isLocked)
		//    {
		//        if ((bool)isLocked && value)
		//            return false;

		//        isLocked = (object)value;
		//        return true;
		//    }
		//}

		#endregion
	}

}
