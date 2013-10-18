// LengthSize.cs
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
using Serialization = System.Runtime.Serialization;

namespace SklLib
{
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

        /// <summary>
        /// Defines the default SI lenght unit as meter.
        /// </summary>
        public const SILengthUnit DefaultSIUnit = SILengthUnit.Meter;
        /// <summary>
        /// Defines the default imperial unit as yard.
        /// </summary>
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
        /// Get value represented by this instance in pixels.
        /// </summary>
        /// <param name="dpi">DPI (Dots Per Inch) resolution.</param>
        /// <returns>The value in pixels in a specific resolution.</returns>
        public decimal GetPixelsByDpi(float dpi)
        {
            return this.Inches * (decimal)dpi;
        }

        /// <summary>
        /// Get value represented by this instance in pixels.
        /// </summary>
        /// <param name="ppm">PPM (Points Per Millimeter) resolution.</param>
        /// <returns>The value in pixels in a specific resolution.</returns>
        public decimal GetPixelsByPpm(float ppm)
        {
            return this.Millimeters * (decimal)ppm;
        }
        
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
        /// Gets the value represented by this instance in the specified imperial unit.
        /// </summary>
        /// <param name="unit">Specifies a Imperial unit.</param>
        /// <returns>The value represented by this instance in the specified unit.</returns>
        public decimal GetValue(ImperialLengthUnit unit)
        {
            return GetImperial(unit);
        }

        /// <summary>
        /// Sets the value represented by this instance in the specified SI unit.
        /// </summary>
        /// <param name="unit">
        /// The SI unit from value.
        /// </param>
        /// <param name="value">
        /// The value to set.
        /// </param>
        public void SetValue(SILengthUnit unit, decimal value)
        {
            SetSI(value, unit);
        }

        /// <summary>
        /// Sets the value represented by this instance in the specified imperial unit.
        /// </summary>
        /// <param name="unit">
        /// The imperial unit from value.
        /// </param>
        /// <param name="value">
        /// The value to set.
        /// </param>
        public void SetValue(ImperialLengthUnit unit, decimal value)
        {
            SetImperial(value, unit);
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
}
