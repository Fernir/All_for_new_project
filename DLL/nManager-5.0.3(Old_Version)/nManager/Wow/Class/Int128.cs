namespace nManager.Wow.Class
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Text;

    [StructLayout(LayoutKind.Sequential), TypeConverter(typeof(Int128.Int128Converter))]
    public struct Int128 : IComparable<Int128>, IEquatable<Int128>, IComparable, IConvertible, IFormattable
    {
        private const ulong HiNeg = 9223372036854775808L;
        private ulong _hi;
        private ulong _lo;
        public static Int128 Zero;
        public static Int128 MaxValue;
        public static Int128 MinValue;
        private static Int128 GetMaxValue()
        {
            return new Int128(0x7fffffffffffffffL, ulong.MaxValue);
        }

        private static Int128 GetMinValue()
        {
            return new Int128(9223372036854775808L, 0L);
        }

        private static Int128 GetZero()
        {
            return new Int128();
        }

        public Int128(byte value)
        {
            this._hi = 0L;
            this._lo = value;
        }

        public Int128(bool value)
        {
            this._hi = 0L;
            this._lo = value ? ((ulong) 1) : ((ulong) 0);
        }

        public Int128(char value)
        {
            this._hi = 0L;
            this._lo = value;
        }

        public Int128(decimal value)
        {
            if (value < 0M)
            {
                Int128 num = -new Int128(-value);
                this._hi = num._hi;
                this._lo = num._lo;
            }
            else
            {
                int[] bits = decimal.GetBits(value);
                this._hi = (ulong) bits[2];
                this._lo = ((ulong) bits[0]) | (bits[1] << 0x20);
            }
        }

        public Int128(double value) : this((decimal) value)
        {
        }

        public Int128(float value) : this((decimal) value)
        {
        }

        public Int128(short value)
        {
            if (value < 0)
            {
                Int128 num = -new Int128(-(value + 1)) - 1;
                this._hi = num._hi;
                this._lo = num._lo;
            }
            else
            {
                this._hi = 0L;
                this._lo = (ulong) value;
            }
        }

        public Int128(int value)
        {
            if (value < 0)
            {
                Int128 num = -new Int128(-(value + 1)) - 1;
                this._hi = num._hi;
                this._lo = num._lo;
            }
            else
            {
                this._hi = 0L;
                this._lo = (ulong) value;
            }
        }

        public Int128(long value)
        {
            if (value < 0L)
            {
                Int128 num = -new Int128(-(value + 1L)) - 1;
                this._hi = num._hi;
                this._lo = num._lo;
            }
            else
            {
                this._hi = 0L;
                this._lo = (ulong) value;
            }
        }

        public Int128(sbyte value)
        {
            if (value < 0)
            {
                Int128 num = -new Int128(-(value + 1)) - 1;
                this._hi = num._hi;
                this._lo = num._lo;
            }
            else
            {
                this._hi = 0L;
                this._lo = (ulong) value;
            }
        }

        public Int128(ushort value)
        {
            this._hi = 0L;
            this._lo = value;
        }

        public Int128(uint value)
        {
            this._hi = 0L;
            this._lo = value;
        }

        public Int128(ulong value)
        {
            this._hi = 0L;
            this._lo = value;
        }

        public Int128(Guid value) : this(value.ToByteArray())
        {
        }

        public Int128(byte[] value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            if (value.Length != 0x10)
            {
                throw new ArgumentException(null, "value");
            }
            this._hi = BitConverter.ToUInt64(value, 8);
            this._lo = BitConverter.ToUInt64(value, 0);
        }

        private Int128(ulong hi, ulong lo)
        {
            this._hi = hi;
            this._lo = lo;
        }

        public Int128(int sign, uint[] ints)
        {
            if (ints == null)
            {
                throw new ArgumentNullException("ints");
            }
            byte[] destinationArray = new byte[8];
            byte[] buffer2 = new byte[8];
            if (ints.Length > 0)
            {
                Array.Copy(BitConverter.GetBytes(ints[0]), 0, destinationArray, 0, 4);
                if (ints.Length > 1)
                {
                    Array.Copy(BitConverter.GetBytes(ints[1]), 0, destinationArray, 4, 4);
                    if (ints.Length > 2)
                    {
                        Array.Copy(BitConverter.GetBytes(ints[2]), 0, buffer2, 0, 4);
                        if (ints.Length > 3)
                        {
                            Array.Copy(BitConverter.GetBytes(ints[3]), 0, buffer2, 4, 4);
                        }
                    }
                }
            }
            this._lo = BitConverter.ToUInt64(destinationArray, 0);
            this._hi = BitConverter.ToUInt64(buffer2, 0);
            if (sign < 0)
            {
                this._hi |= 9223372036854775808L;
            }
            else
            {
                this._hi &= (ulong) 0x7fffffffffffffffL;
            }
        }

        public int Sign
        {
            get
            {
                if ((this._hi == 0L) && (this._lo == 0L))
                {
                    return 0;
                }
                if ((this._hi & 9223372036854775808L) != 0L)
                {
                    return -1;
                }
                return 1;
            }
        }
        public override int GetHashCode()
        {
            return (this._hi.GetHashCode() ^ this._lo.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public bool Equals(Int128 obj)
        {
            return ((this._hi == obj._hi) && (this._lo == obj._lo));
        }

        public override string ToString()
        {
            return this.ToString(null, null);
        }

        public string ToString(string format)
        {
            return this.ToString(format, null);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (formatProvider == null)
            {
                formatProvider = CultureInfo.CurrentCulture;
            }
            if (!string.IsNullOrEmpty(format))
            {
                char ch = format[0];
                switch (ch)
                {
                    case 'x':
                    case 'X':
                        int num;
                        int.TryParse(format.Substring(1).Trim(), out num);
                        return this.ToHexaString(ch == 'X', num);
                }
                if (((ch != 'G') && (ch != 'g')) && ((ch != 'D') && (ch != 'd')))
                {
                    throw new NotSupportedException("Not supported format: " + format);
                }
            }
            return this.ToString((NumberFormatInfo) formatProvider.GetFormat(typeof(NumberFormatInfo)));
        }

        private string ToHexaString(bool caps, int min)
        {
            StringBuilder builder = new StringBuilder();
            string format = caps ? "X" : "x";
            if (((min < 0) || (min > 0x10)) || (this._hi != 0L))
            {
                builder.Append((min > 0x10) ? this._hi.ToString(format + (min - 0x10)) : this._hi.ToString(format));
                builder.Append(this._lo.ToString(format + "16"));
            }
            else
            {
                builder.Append(this._lo.ToString(format + min));
            }
            return builder.ToString();
        }

        private string ToString(NumberFormatInfo info)
        {
            if (this.Sign == 0)
            {
                return "0";
            }
            StringBuilder builder = new StringBuilder();
            Int128 divisor = new Int128(10);
            Int128 dividend = this;
            dividend._hi &= (ulong) 0x7fffffffffffffffL;
            do
            {
                Int128 num3;
                dividend = DivRem(dividend, divisor, out num3);
                if (((num3._lo > 0L) || (dividend.Sign != 0)) || (builder.Length == 0))
                {
                    builder.Insert(0, (char) (((ulong) 0x30L) + num3._lo));
                }
            }
            while (dividend.Sign != 0);
            string str = builder.ToString();
            if ((this.Sign < 0) && (str != "0"))
            {
                return (info.NegativeSign + str);
            }
            return str;
        }

        TypeCode IConvertible.GetTypeCode()
        {
            return TypeCode.Object;
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return (bool) this;
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            return (byte) this;
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            return (char) this;
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return (decimal) this;
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return (double) this;
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            return (short) this;
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            return (int) this;
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return (long) ((int) this);
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            return (sbyte) ((short) this);
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            return (float) this;
        }

        string IConvertible.ToString(IFormatProvider provider)
        {
            return this.ToString(null, provider);
        }

        public bool TryConvert(Type conversionType, IFormatProvider provider, out object value)
        {
            if (conversionType == typeof(bool))
            {
                value = (bool) this;
                return true;
            }
            if (conversionType == typeof(byte))
            {
                value = (byte) this;
                return true;
            }
            if (conversionType == typeof(char))
            {
                value = (char) this;
                return true;
            }
            if (conversionType == typeof(decimal))
            {
                value = (decimal) this;
                return true;
            }
            if (conversionType == typeof(double))
            {
                value = (double) this;
                return true;
            }
            if (conversionType == typeof(short))
            {
                value = (short) this;
                return true;
            }
            if (conversionType == typeof(int))
            {
                value = (int) this;
                return true;
            }
            if (conversionType == typeof(long))
            {
                value = (long) this;
                return true;
            }
            if (conversionType == typeof(sbyte))
            {
                value = (sbyte) ((short) this);
                return true;
            }
            if (conversionType == typeof(float))
            {
                value = (float) this;
                return true;
            }
            if (conversionType == typeof(string))
            {
                value = this.ToString(null, provider);
                return true;
            }
            if (conversionType == typeof(ushort))
            {
                value = (ushort) this;
                return true;
            }
            if (conversionType == typeof(uint))
            {
                value = (uint) this;
                return true;
            }
            if (conversionType == typeof(ulong))
            {
                value = (ulong) this;
                return true;
            }
            if (conversionType == typeof(byte[]))
            {
                value = this.ToByteArray();
                return true;
            }
            if (conversionType == typeof(Guid))
            {
                value = new Guid(this.ToByteArray());
                return true;
            }
            value = null;
            return false;
        }

        public static Int128 Parse(string value)
        {
            return Parse(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        public static Int128 Parse(string value, NumberStyles style)
        {
            return Parse(value, style, NumberFormatInfo.CurrentInfo);
        }

        public static Int128 Parse(string value, IFormatProvider provider)
        {
            return Parse(value, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
        }

        public static Int128 Parse(string value, NumberStyles style, IFormatProvider provider)
        {
            Int128 num;
            if (!TryParse(value, style, provider, out num))
            {
                throw new ArgumentException(null, "value");
            }
            return num;
        }

        public static bool TryParse(string value, out Int128 result)
        {
            return TryParse(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
        }

        public static bool TryParse(string value, NumberStyles style, IFormatProvider provider, out Int128 result)
        {
            result = Zero;
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }
            if (value.StartsWith("x", StringComparison.OrdinalIgnoreCase))
            {
                style |= NumberStyles.AllowHexSpecifier;
                value = value.Substring(1);
            }
            else if (value.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                style |= NumberStyles.AllowHexSpecifier;
                value = value.Substring(2);
            }
            if ((style & NumberStyles.AllowHexSpecifier) == NumberStyles.AllowHexSpecifier)
            {
                return TryParseHex(value, out result);
            }
            return TryParseNum(value, out result);
        }

        private static bool TryParseHex(string value, out Int128 result)
        {
            if (value.Length > 0x20)
            {
                throw new OverflowException();
            }
            result = Zero;
            bool flag = false;
            int num = 0;
            for (int i = value.Length - 1; i >= 0; i--)
            {
                ulong num3;
                char ch = value[i];
                if ((ch >= '0') && (ch <= '9'))
                {
                    num3 = ch - '0';
                }
                else if ((ch >= 'A') && (ch <= 'F'))
                {
                    num3 = (ulong) ((ch - 'A') + 10);
                }
                else if ((ch >= 'a') && (ch <= 'f'))
                {
                    num3 = (ulong) ((ch - 'a') + 10);
                }
                else
                {
                    return false;
                }
                if (flag)
                {
                    result._hi |= num3 << num;
                    num += 4;
                }
                else
                {
                    result._lo |= num3 << num;
                    num += 4;
                    if (num == 0x40)
                    {
                        num = 0;
                        flag = true;
                    }
                }
            }
            return true;
        }

        private static bool TryParseNum(string value, out Int128 result)
        {
            result = Zero;
            foreach (char ch in value)
            {
                byte num;
                if ((ch >= '0') && (ch <= '9'))
                {
                    num = (byte) (ch - '0');
                }
                else
                {
                    return false;
                }
                result = 10 * result;
                result += num;
            }
            return true;
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            object obj2;
            if (!this.TryConvert(conversionType, provider, out obj2))
            {
                throw new InvalidCastException();
            }
            return obj2;
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            if (this._hi != 0L)
            {
                throw new OverflowException();
            }
            return Convert.ToUInt16(this._lo);
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            if (this._hi != 0L)
            {
                throw new OverflowException();
            }
            return Convert.ToUInt32(this._lo);
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            if (this._hi != 0L)
            {
                throw new OverflowException();
            }
            return this._lo;
        }

        int IComparable.CompareTo(object obj)
        {
            return Compare(this, obj);
        }

        public static int Compare(Int128 left, object right)
        {
            if (right is Int128)
            {
                return Compare(left, (Int128) right);
            }
            if (right is bool)
            {
                return Compare(left, new Int128((bool) right));
            }
            if (right is byte)
            {
                return Compare(left, new Int128((byte) right));
            }
            if (right is char)
            {
                return Compare(left, new Int128((char) right));
            }
            if (right is decimal)
            {
                return Compare(left, new Int128((decimal) right));
            }
            if (right is double)
            {
                return Compare(left, new Int128((double) right));
            }
            if (right is short)
            {
                return Compare(left, new Int128((short) right));
            }
            if (right is int)
            {
                return Compare(left, new Int128((int) right));
            }
            if (right is long)
            {
                return Compare(left, new Int128((long) right));
            }
            if (right is sbyte)
            {
                return Compare(left, new Int128((sbyte) right));
            }
            if (right is float)
            {
                return Compare(left, new Int128((float) right));
            }
            if (right is ushort)
            {
                return Compare(left, new Int128((ushort) right));
            }
            if (right is uint)
            {
                return Compare(left, new Int128((uint) right));
            }
            if (right is ulong)
            {
                return Compare(left, new Int128((ulong) right));
            }
            byte[] buffer = right as byte[];
            if ((buffer != null) && (buffer.Length != 0x10))
            {
                return Compare(left, new Int128(buffer));
            }
            if (!(right is Guid))
            {
                throw new ArgumentException();
            }
            return Compare(left, new Int128((Guid) right));
        }

        public byte[] ToByteArray()
        {
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(BitConverter.GetBytes(this._lo), 0, dst, 0, 8);
            Buffer.BlockCopy(BitConverter.GetBytes(this._hi), 0, dst, 8, 8);
            return dst;
        }

        public static int Compare(Int128 left, Int128 right)
        {
            if (left.Sign < 0)
            {
                if (right.Sign >= 0)
                {
                    return -1;
                }
                ulong num = left._hi & ((ulong) 0x7fffffffffffffffL);
                ulong num2 = right._hi & ((ulong) 0x7fffffffffffffffL);
                if (num != num2)
                {
                    return -num.CompareTo(num2);
                }
                return -left._lo.CompareTo(right._lo);
            }
            if (right.Sign < 0)
            {
                return 1;
            }
            if (left._hi != right._hi)
            {
                return left._hi.CompareTo(right._hi);
            }
            return left._lo.CompareTo(right._lo);
        }

        public int CompareTo(Int128 value)
        {
            return Compare(this, value);
        }

        public static implicit operator Int128(bool value)
        {
            return new Int128(value);
        }

        public static implicit operator Int128(byte value)
        {
            return new Int128(value);
        }

        public static implicit operator Int128(char value)
        {
            return new Int128(value);
        }

        public static explicit operator Int128(decimal value)
        {
            return new Int128(value);
        }

        public static explicit operator Int128(double value)
        {
            return new Int128(value);
        }

        public static implicit operator Int128(short value)
        {
            return new Int128(value);
        }

        public static implicit operator Int128(int value)
        {
            return new Int128(value);
        }

        public static implicit operator Int128(long value)
        {
            return new Int128(value);
        }

        public static implicit operator Int128(sbyte value)
        {
            return new Int128(value);
        }

        public static explicit operator Int128(float value)
        {
            return new Int128(value);
        }

        public static implicit operator Int128(ushort value)
        {
            return new Int128(value);
        }

        public static implicit operator Int128(uint value)
        {
            return new Int128(value);
        }

        public static implicit operator Int128(ulong value)
        {
            return new Int128(value);
        }

        public static explicit operator bool(Int128 value)
        {
            return (value.Sign != 0);
        }

        public static explicit operator byte(Int128 value)
        {
            if (value.Sign == 0)
            {
                return 0;
            }
            if ((value.Sign < 0) || (value._lo > 0xffL))
            {
                throw new OverflowException();
            }
            return (byte) value._lo;
        }

        public static explicit operator char(Int128 value)
        {
            if (value.Sign == 0)
            {
                return '\0';
            }
            if ((value.Sign < 0) || (value._lo > 0xffffL))
            {
                throw new OverflowException();
            }
            return (char) value._lo;
        }

        public static explicit operator decimal(Int128 value)
        {
            if (value.Sign == 0)
            {
                return 0M;
            }
            return new decimal((int) (value._lo & 0xffffffffL), (int) (value._lo >> 0x20), (int) (value._hi & 0xffffffffL), value.Sign < 0, 0);
        }

        public static explicit operator double(Int128 value)
        {
            double num;
            if (value.Sign == 0)
            {
                return 0.0;
            }
            NumberFormatInfo numberFormat = CultureInfo.InvariantCulture.NumberFormat;
            if (!double.TryParse(value.ToString(numberFormat), NumberStyles.Number, (IFormatProvider) numberFormat, out num))
            {
                throw new OverflowException();
            }
            return num;
        }

        public static explicit operator float(Int128 value)
        {
            float num;
            if (value.Sign == 0)
            {
                return 0f;
            }
            NumberFormatInfo numberFormat = CultureInfo.InvariantCulture.NumberFormat;
            if (!float.TryParse(value.ToString(numberFormat), NumberStyles.Number, (IFormatProvider) numberFormat, out num))
            {
                throw new OverflowException();
            }
            return num;
        }

        public static explicit operator short(Int128 value)
        {
            if (value.Sign == 0)
            {
                return 0;
            }
            if (value._lo > 0x8000L)
            {
                throw new OverflowException();
            }
            if ((value._lo == 0x8000L) && (value.Sign > 0))
            {
                throw new OverflowException();
            }
            return (short) (((int) value._lo) * value.Sign);
        }

        public static explicit operator int(Int128 value)
        {
            if (value.Sign == 0)
            {
                return 0;
            }
            if (value._lo > 0x80000000L)
            {
                throw new OverflowException();
            }
            if ((value._lo == 0x80000000L) && (value.Sign > 0))
            {
                throw new OverflowException();
            }
            return (((int) value._lo) * value.Sign);
        }

        public static explicit operator long(Int128 value)
        {
            if (value.Sign == 0)
            {
                return 0L;
            }
            if (value._lo > 0x7fffffffffffffffL)
            {
                throw new OverflowException();
            }
            return (long) (value._lo * value.Sign);
        }

        public static explicit operator uint(Int128 value)
        {
            if (value.Sign == 0)
            {
                return 0;
            }
            if ((value.Sign < 0) || (value._lo > 0xffffffffL))
            {
                throw new OverflowException();
            }
            return (uint) value._lo;
        }

        public static explicit operator ushort(Int128 value)
        {
            if (value.Sign == 0)
            {
                return 0;
            }
            if ((value.Sign < 0) || (value._lo > 0xffffL))
            {
                throw new OverflowException();
            }
            return (ushort) value._lo;
        }

        public static explicit operator ulong(Int128 value)
        {
            if ((value.Sign < 0) || (value._hi != 0L))
            {
                throw new OverflowException();
            }
            return value._lo;
        }

        public static bool operator >(Int128 left, Int128 right)
        {
            return (Compare(left, right) > 0);
        }

        public static bool operator <(Int128 left, Int128 right)
        {
            return (Compare(left, right) < 0);
        }

        public static bool operator >=(Int128 left, Int128 right)
        {
            return (Compare(left, right) >= 0);
        }

        public static bool operator <=(Int128 left, Int128 right)
        {
            return (Compare(left, right) <= 0);
        }

        public static bool operator !=(Int128 left, Int128 right)
        {
            return (Compare(left, right) != 0);
        }

        public static bool operator ==(Int128 left, Int128 right)
        {
            return (Compare(left, right) == 0);
        }

        public static Int128 operator +(Int128 value)
        {
            return value;
        }

        public static Int128 operator -(Int128 value)
        {
            return Negate(value);
        }

        public static Int128 Negate(Int128 value)
        {
            return (new Int128(~value._hi, ~value._lo) + 1);
        }

        public Int128 ToAbs()
        {
            return Abs(this);
        }

        public static Int128 Abs(Int128 value)
        {
            if (value.Sign < 0)
            {
                return -value;
            }
            return value;
        }

        public static Int128 operator +(Int128 left, Int128 right)
        {
            Int128 num = left;
            num._hi += right._hi;
            num._lo += right._lo;
            if (num._lo < left._lo)
            {
                num._hi += (ulong) 1L;
            }
            return num;
        }

        public static Int128 operator -(Int128 left, Int128 right)
        {
            return (left + -right);
        }

        public static Int128 Add(Int128 left, Int128 right)
        {
            return (left + right);
        }

        public static Int128 Subtract(Int128 left, Int128 right)
        {
            return (left - right);
        }

        public static Int128 Divide(Int128 dividend, Int128 divisor)
        {
            Int128 num;
            return DivRem(dividend, divisor, out num);
        }

        public static Int128 DivRem(Int128 dividend, Int128 divisor, out Int128 remainder)
        {
            uint[] numArray;
            uint[] numArray2;
            if (divisor == 0)
            {
                throw new DivideByZeroException();
            }
            DivRem(dividend.ToUIn32Array(), divisor.ToUIn32Array(), out numArray, out numArray2);
            remainder = new Int128(1, numArray2);
            return new Int128(dividend.Sign * divisor.Sign, numArray);
        }

        private static void DivRem(uint[] dividend, uint[] divisor, out uint[] quotient, out uint[] remainder)
        {
            int length = GetLength(divisor);
            int len = GetLength(dividend);
            if (length <= 1)
            {
                ulong num3 = 0L;
                uint num4 = divisor[0];
                quotient = new uint[len];
                remainder = new uint[1];
                for (int i = len - 1; i >= 0; i--)
                {
                    num3 *= (ulong) 0x100000000L;
                    num3 += dividend[i];
                    ulong num6 = num3 / ((ulong) num4);
                    num3 -= num6 * num4;
                    quotient[i] = (uint) num6;
                }
                remainder[0] = (uint) num3;
            }
            else if (len >= length)
            {
                int normalizeShift = GetNormalizeShift(divisor[length - 1]);
                uint[] normalized = new uint[len + 1];
                uint[] numArray2 = new uint[length];
                Normalize(dividend, len, normalized, normalizeShift);
                Normalize(divisor, length, numArray2, normalizeShift);
                quotient = new uint[(len - length) + 1];
                for (int j = len - length; j >= 0; j--)
                {
                    long num12;
                    ulong num9 = (ulong) ((0x100000000L * normalized[j + length]) + normalized[(j + length) - 1]);
                    ulong num10 = num9 / ((ulong) numArray2[length - 1]);
                    num9 -= num10 * numArray2[length - 1];
                    do
                    {
                        if ((num10 < 0x100000000L) && ((num10 * numArray2[length - 2]) <= ((num9 * ((ulong) 0x100000000L)) + normalized[(j + length) - 2])))
                        {
                            break;
                        }
                        num10 -= (ulong) 1L;
                        num9 += numArray2[length - 1];
                    }
                    while (num9 < 0x100000000L);
                    long num11 = 0L;
                    int index = 0;
                    while (index < length)
                    {
                        ulong num14 = numArray2[index] * num10;
                        num12 = (normalized[index + j] - ((uint) num14)) - num11;
                        normalized[index + j] = (uint) num12;
                        num14 = num14 >> 0x20;
                        num12 = num12 >> 0x20;
                        num11 = ((long) num14) - num12;
                        index++;
                    }
                    num12 = normalized[j + length] - num11;
                    normalized[j + length] = (uint) num12;
                    quotient[j] = (uint) num10;
                    if (num12 < 0L)
                    {
                        quotient[j]--;
                        ulong num15 = 0L;
                        for (index = 0; index < length; index++)
                        {
                            num15 = (numArray2[index] + normalized[j + index]) + num15;
                            normalized[j + index] = (uint) num15;
                            num15 = num15 >> 0x20;
                        }
                        num15 += normalized[j + length];
                        normalized[j + length] = (uint) num15;
                    }
                }
                remainder = Unnormalize(normalized, normalizeShift);
            }
            else
            {
                quotient = new uint[0];
                remainder = dividend;
            }
        }

        private static int GetLength(uint[] uints)
        {
            int index = uints.Length - 1;
            while ((index >= 0) && (uints[index] == 0))
            {
                index--;
            }
            return (index + 1);
        }

        private static int GetNormalizeShift(uint ui)
        {
            int num = 0;
            if ((ui & 0xffff0000) == 0)
            {
                ui = ui << 0x10;
                num += 0x10;
            }
            if ((ui & 0xff000000) == 0)
            {
                ui = ui << 8;
                num += 8;
            }
            if ((ui & 0xf0000000) == 0)
            {
                ui = ui << 4;
                num += 4;
            }
            if ((ui & 0xc0000000) == 0)
            {
                ui = ui << 2;
                num += 2;
            }
            if ((ui & 0x80000000) == 0)
            {
                num++;
            }
            return num;
        }

        private static uint[] Unnormalize(uint[] normalized, int shift)
        {
            int length = GetLength(normalized);
            uint[] numArray = new uint[length];
            if (shift > 0)
            {
                int num2 = 0x20 - shift;
                uint num3 = 0;
                for (int j = length - 1; j >= 0; j--)
                {
                    numArray[j] = (normalized[j] >> shift) | num3;
                    num3 = normalized[j] << num2;
                }
                return numArray;
            }
            for (int i = 0; i < length; i++)
            {
                numArray[i] = normalized[i];
            }
            return numArray;
        }

        private static void Normalize(uint[] unormalized, int len, uint[] normalized, int shift)
        {
            int num;
            uint num2 = 0;
            if (shift > 0)
            {
                int num3 = 0x20 - shift;
                for (num = 0; num < len; num++)
                {
                    normalized[num] = (unormalized[num] << shift) | num2;
                    num2 = unormalized[num] >> num3;
                }
            }
            else
            {
                num = 0;
                while (num < len)
                {
                    normalized[num] = unormalized[num];
                    num++;
                }
            }
            while (num < normalized.Length)
            {
                normalized[num++] = 0;
            }
            if (num2 != 0)
            {
                normalized[len] = num2;
            }
        }

        public static Int128 Remainder(Int128 dividend, Int128 divisor)
        {
            Int128 num;
            DivRem(dividend, divisor, out num);
            return num;
        }

        public static Int128 operator %(Int128 dividend, Int128 divisor)
        {
            return Remainder(dividend, divisor);
        }

        public static Int128 operator /(Int128 dividend, Int128 divisor)
        {
            return Divide(dividend, divisor);
        }

        public ulong[] ToUIn64Array()
        {
            return new ulong[] { this._hi, this._lo };
        }

        public uint[] ToUIn32Array()
        {
            uint[] dst = new uint[4];
            byte[] bytes = BitConverter.GetBytes(this._lo);
            byte[] src = BitConverter.GetBytes(this._hi);
            Buffer.BlockCopy(bytes, 0, dst, 0, 4);
            Buffer.BlockCopy(bytes, 4, dst, 4, 4);
            Buffer.BlockCopy(src, 0, dst, 8, 4);
            Buffer.BlockCopy(src, 4, dst, 12, 4);
            return dst;
        }

        public static Int128 Multiply(Int128 left, Int128 right)
        {
            uint[] numArray = left.ToUIn32Array();
            uint[] numArray2 = right.ToUIn32Array();
            uint[] ints = new uint[8];
            for (int i = 0; i < numArray.Length; i++)
            {
                int index = i;
                ulong num3 = 0L;
                foreach (uint num4 in numArray2)
                {
                    num3 = (num3 + (numArray[i] * num4)) + ints[index];
                    ints[index++] = (uint) num3;
                    num3 = num3 >> 0x20;
                }
                while (num3 != 0L)
                {
                    num3 += ints[index];
                    ints[index++] = (uint) num3;
                    num3 = num3 >> 0x20;
                }
            }
            return new Int128(left.Sign * right.Sign, ints);
        }

        public static Int128 operator *(Int128 left, Int128 right)
        {
            return Multiply(left, right);
        }

        public static Int128 operator >>(Int128 value, int shift)
        {
            if (shift == 0)
            {
                return value;
            }
            if (shift < 0)
            {
                return (value << -shift);
            }
            shift = shift % 0x80;
            Int128 num = new Int128();
            if (shift > 0x3f)
            {
                num._lo = value._hi >> (shift - 0x40);
                num._hi = 0L;
                return num;
            }
            num._hi = value._hi >> shift;
            num._lo = (value._hi << (0x40 - shift)) | (value._lo >> shift);
            return num;
        }

        public static Int128 operator <<(Int128 value, int shift)
        {
            if (shift == 0)
            {
                return value;
            }
            if (shift < 0)
            {
                return (value >> -shift);
            }
            shift = shift % 0x80;
            Int128 num = new Int128();
            if (shift > 0x3f)
            {
                num._hi = value._lo << (shift - 0x40);
                num._lo = 0L;
                return num;
            }
            ulong num2 = value._lo >> (0x40 - shift);
            num._hi = num2 | (value._hi << shift);
            num._lo = value._lo << shift;
            return num;
        }

        public static Int128 operator |(Int128 left, Int128 right)
        {
            if (left == 0)
            {
                return right;
            }
            if (right == 0)
            {
                return left;
            }
            Int128 num = left;
            num._hi |= right._hi;
            num._lo |= right._lo;
            return num;
        }

        public static Int128 operator &(Int128 left, Int128 right)
        {
            if ((left == 0) || (right == 0))
            {
                return Zero;
            }
            Int128 num = left;
            num._hi &= right._hi;
            num._lo &= right._lo;
            return num;
        }

        static Int128()
        {
            Zero = GetZero();
            MaxValue = GetMaxValue();
            MinValue = GetMinValue();
        }
        public class Int128Converter : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return ((sourceType == typeof(string)) || base.CanConvertFrom(context, sourceType));
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                return ((destinationType == typeof(string)) || base.CanConvertTo(context, destinationType));
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                Int128 num;
                if ((value != null) && Int128.TryParse(string.Format("{0}", value), out num))
                {
                    return num;
                }
                return new Int128();
            }

            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (destinationType == typeof(string))
                {
                    return string.Format("{0}", value);
                }
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }
    }
}

