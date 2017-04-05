namespace nManager.Wow.Class
{
    using nManager.Wow.Enums;
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Numerics;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=1), TypeConverter(typeof(UInt128.UInt128Converter))]
    public struct UInt128 : IComparable<UInt128>, IEquatable<UInt128>, IComparable, IConvertible, IFormattable
    {
        public const int SizeOf = 0x10;
        private ulong _hi;
        private ulong _lo;
        public static readonly UInt128 Zero;
        public static readonly UInt128 One;
        public static readonly UInt128 MinValue;
        public static readonly UInt128 MaxValue;
        public ulong High
        {
            get
            {
                return this._hi;
            }
            set
            {
                this._hi = value;
            }
        }
        public ulong Low
        {
            get
            {
                return this._lo;
            }
            set
            {
                this._lo = value;
            }
        }
        public GuidType GetWoWType
        {
            get
            {
                return (GuidType) ((byte) (this._hi >> 0x3a));
            }
            set
            {
                this._hi |= ((ulong) value) << 0x3a;
            }
        }
        public GuidSubType GetWoWSubType
        {
            get
            {
                return (GuidSubType) ((byte) (this._lo >> 0x38));
            }
            set
            {
                this._lo |= ((ulong) value) << 0x38;
            }
        }
        public ushort GetWoWRealmId
        {
            get
            {
                return (ushort) ((this._hi >> 0x2a) & ((ulong) 0x1fffL));
            }
            set
            {
                this._hi |= value << 0x2a;
            }
        }
        public ushort GetWoWServerId
        {
            get
            {
                return (ushort) ((this._lo >> 40) & ((ulong) 0x1fffL));
            }
            set
            {
                this._lo |= value << 40;
            }
        }
        public ushort GetWoWMapId
        {
            get
            {
                return (ushort) ((this._hi >> 0x1d) & ((ulong) 0x1fffL));
            }
            set
            {
                this._hi |= value << 0x1d;
            }
        }
        public uint GetWoWId
        {
            get
            {
                return (((uint) (this._hi & 0xffffffL)) >> 6);
            }
            set
            {
                this._hi |= value << 6;
            }
        }
        public ulong CreationBits
        {
            get
            {
                return (this._lo & ((ulong) 0xffffffffffL));
            }
            set
            {
                this._lo |= value;
            }
        }
        public UInt128(byte value)
        {
            this._hi = 0L;
            this._lo = value;
        }

        public UInt128(bool value)
        {
            this._hi = 0L;
            this._lo = value ? ((ulong) 1) : ((ulong) 0);
        }

        public UInt128(char value)
        {
            this._hi = 0L;
            this._lo = value;
        }

        public UInt128(decimal value)
        {
            int[] bits = decimal.GetBits(value);
            this._hi = (ulong) bits[2];
            this._lo = ((ulong) bits[0]) | (bits[1] << 0x20);
        }

        public UInt128(double value) : this((decimal) value)
        {
        }

        public UInt128(float value) : this((decimal) value)
        {
        }

        public UInt128(short value)
        {
            this._hi = 0L;
            this._lo = (ulong) value;
        }

        public UInt128(int value)
        {
            this._hi = 0L;
            this._lo = (ulong) value;
        }

        public UInt128(long value)
        {
            this._hi = 0L;
            this._lo = (ulong) value;
        }

        public UInt128(sbyte value)
        {
            this._hi = 0L;
            this._lo = (ulong) value;
        }

        public UInt128(ushort value)
        {
            this._hi = 0L;
            this._lo = value;
        }

        public UInt128(uint value)
        {
            this._hi = 0L;
            this._lo = value;
        }

        public UInt128(ulong value)
        {
            this._hi = 0L;
            this._lo = value;
        }

        public UInt128(Guid value) : this(value.ToByteArray())
        {
        }

        public UInt128(byte[] value)
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

        private UInt128(ulong hi, ulong lo)
        {
            this._hi = hi;
            this._lo = lo;
        }

        public UInt128(BigInteger value) : this((ulong) (value >> 0x40), (ulong) (value & -1L))
        {
        }

        public static explicit operator UInt128(BigInteger value)
        {
            return new UInt128(value);
        }

        public static implicit operator BigInteger(UInt128 value)
        {
            return value.ToBigInteger();
        }

        private BigInteger ToBigInteger()
        {
            BigInteger integer = this._hi << 0x40;
            return (integer + this._lo);
        }

        public static implicit operator UInt128(ulong value)
        {
            return new UInt128(0L, value);
        }

        public UInt128(uint[] ints)
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
        }

        public override int GetHashCode()
        {
            return (this._hi.GetHashCode() ^ this._lo.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            return ((obj is UInt128) && base.Equals(obj));
        }

        public bool Equals(UInt128 obj)
        {
            return ((this._hi == obj._hi) && (this._lo == obj._lo));
        }

        public unsafe string ToString(string format, IFormatProvider formatProvider)
        {
            BigInteger integer = *((BigInteger*) this);
            return integer.ToString(format, formatProvider);
        }

        public string ToString(string format)
        {
            return this.ToString(format, CultureInfo.CurrentCulture);
        }

        public string ToString(IFormatProvider provider)
        {
            return this.ToString("G", provider);
        }

        public override string ToString()
        {
            return this.ToString(CultureInfo.CurrentCulture);
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

        public static UInt128 Parse(string value)
        {
            return Parse(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        public static UInt128 Parse(string value, NumberStyles style)
        {
            return Parse(value, style, NumberFormatInfo.CurrentInfo);
        }

        public static UInt128 Parse(string value, IFormatProvider provider)
        {
            return Parse(value, NumberStyles.Integer, provider);
        }

        public static UInt128 Parse(string value, NumberStyles style, IFormatProvider provider)
        {
            BigInteger integer = BigInteger.Parse(value, style, provider);
            if ((integer < 0L) || (integer > MaxValue))
            {
                throw new OverflowException("Value was either too large or too small for an UInt128.");
            }
            return (UInt128) integer;
        }

        public static bool TryParse(string value, out UInt128 result)
        {
            return TryParse(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
        }

        public static bool TryParse(string value, NumberStyles style, IFormatProvider provider, out UInt128 result)
        {
            BigInteger integer;
            bool flag = BigInteger.TryParse(value, style, provider, out integer);
            if (flag && ((integer < 0L) || (integer > MaxValue)))
            {
                result = Zero;
                return false;
            }
            result = (UInt128) integer;
            return flag;
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

        public static int Compare(UInt128 left, object right)
        {
            if (right is UInt128)
            {
                return Compare(left, (UInt128) right);
            }
            if (right is bool)
            {
                return Compare(left, new UInt128((bool) right));
            }
            if (right is byte)
            {
                return Compare(left, new UInt128((byte) right));
            }
            if (right is char)
            {
                return Compare(left, new UInt128((char) right));
            }
            if (right is decimal)
            {
                return Compare(left, new UInt128((decimal) right));
            }
            if (right is double)
            {
                return Compare(left, new UInt128((double) right));
            }
            if (right is short)
            {
                return Compare(left, new UInt128((short) right));
            }
            if (right is int)
            {
                return Compare(left, new UInt128((int) right));
            }
            if (right is long)
            {
                return Compare(left, new UInt128((long) right));
            }
            if (right is sbyte)
            {
                return Compare(left, new UInt128((sbyte) right));
            }
            if (right is float)
            {
                return Compare(left, new UInt128((float) right));
            }
            if (right is ushort)
            {
                return Compare(left, new UInt128((ushort) right));
            }
            if (right is uint)
            {
                return Compare(left, new UInt128((uint) right));
            }
            if (right is ulong)
            {
                return Compare(left, new UInt128((ulong) right));
            }
            byte[] buffer = right as byte[];
            if ((buffer != null) && (buffer.Length != 0x10))
            {
                return Compare(left, new UInt128(buffer));
            }
            if (!(right is Guid))
            {
                throw new ArgumentException();
            }
            return Compare(left, new UInt128((Guid) right));
        }

        public byte[] ToByteArray()
        {
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(BitConverter.GetBytes(this._lo), 0, dst, 0, 8);
            Buffer.BlockCopy(BitConverter.GetBytes(this._hi), 0, dst, 8, 8);
            return dst;
        }

        public static int Compare(UInt128 left, UInt128 right)
        {
            if (left._hi != right._hi)
            {
                return left._hi.CompareTo(right._hi);
            }
            return left._lo.CompareTo(right._lo);
        }

        public int CompareTo(UInt128 value)
        {
            return Compare(this, value);
        }

        public static implicit operator UInt128(bool value)
        {
            return new UInt128(value);
        }

        public static implicit operator UInt128(byte value)
        {
            return new UInt128(value);
        }

        public static implicit operator UInt128(char value)
        {
            return new UInt128(value);
        }

        public static explicit operator UInt128(decimal value)
        {
            return new UInt128(value);
        }

        public static explicit operator UInt128(double value)
        {
            return new UInt128(value);
        }

        public static implicit operator UInt128(short value)
        {
            return new UInt128(value);
        }

        public static implicit operator UInt128(int value)
        {
            return new UInt128(value);
        }

        public static implicit operator UInt128(long value)
        {
            return new UInt128(value);
        }

        public static implicit operator UInt128(sbyte value)
        {
            return new UInt128(value);
        }

        public static explicit operator UInt128(float value)
        {
            return new UInt128(value);
        }

        public static implicit operator UInt128(ushort value)
        {
            return new UInt128(value);
        }

        public static implicit operator UInt128(uint value)
        {
            return new UInt128(value);
        }

        public static explicit operator bool(UInt128 value)
        {
            return (value != 0);
        }

        public static explicit operator byte(UInt128 value)
        {
            if (value == 0)
            {
                return 0;
            }
            if (value._lo > 0xffL)
            {
                throw new OverflowException();
            }
            return (byte) value._lo;
        }

        public static explicit operator char(UInt128 value)
        {
            if (value == 0)
            {
                return '\0';
            }
            if (value._lo > 0xffffL)
            {
                throw new OverflowException();
            }
            return (char) value._lo;
        }

        public static explicit operator decimal(UInt128 value)
        {
            if (value == 0)
            {
                return 0M;
            }
            return new decimal((int) (value._lo & 0xffffffffL), (int) (value._lo >> 0x20), (int) (value._hi & 0xffffffffL), false, 0);
        }

        public static explicit operator double(UInt128 value)
        {
            double num;
            if (value == 0)
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

        public static explicit operator float(UInt128 value)
        {
            float num;
            if (value == 0)
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

        public static explicit operator short(UInt128 value)
        {
            if (value == 0)
            {
                return 0;
            }
            if (value._lo > 0x7fffL)
            {
                throw new OverflowException();
            }
            return (short) ((int) value._lo);
        }

        public static explicit operator int(UInt128 value)
        {
            if (value == 0)
            {
                return 0;
            }
            if (value._lo > 0x7fffffffL)
            {
                throw new OverflowException();
            }
            return (int) value._lo;
        }

        public static explicit operator long(UInt128 value)
        {
            if (value == 0)
            {
                return 0L;
            }
            if (value._lo > 0x7fffffffffffffffL)
            {
                throw new OverflowException();
            }
            return (long) value._lo;
        }

        public static explicit operator uint(UInt128 value)
        {
            if (value == 0)
            {
                return 0;
            }
            if (value._lo > 0xffffffffL)
            {
                throw new OverflowException();
            }
            return (uint) value._lo;
        }

        public static explicit operator ushort(UInt128 value)
        {
            if (value == 0)
            {
                return 0;
            }
            if (value._lo > 0xffffL)
            {
                throw new OverflowException();
            }
            return (ushort) value._lo;
        }

        public static explicit operator ulong(UInt128 value)
        {
            if (value._hi != 0L)
            {
                throw new OverflowException("Value was too large for a UInt64.");
            }
            return value._lo;
        }

        public static bool operator >(UInt128 left, UInt128 right)
        {
            return (Compare(left, right) > 0);
        }

        public static bool operator <(UInt128 left, UInt128 right)
        {
            return (Compare(left, right) < 0);
        }

        public static bool operator >=(UInt128 left, UInt128 right)
        {
            return (Compare(left, right) >= 0);
        }

        public static bool operator <=(UInt128 left, UInt128 right)
        {
            return (Compare(left, right) <= 0);
        }

        public static bool operator !=(UInt128 left, UInt128 right)
        {
            return (Compare(left, right) != 0);
        }

        public static bool operator ==(UInt128 left, UInt128 right)
        {
            return (Compare(left, right) == 0);
        }

        public static UInt128 operator +(UInt128 value)
        {
            return value;
        }

        public static UInt128 operator -(UInt128 value)
        {
            return Negate(value);
        }

        public static UInt128 Negate(UInt128 value)
        {
            return (new UInt128(~value._hi, ~value._lo) + 1);
        }

        public UInt128 ToAbs()
        {
            return Abs(this);
        }

        public static UInt128 Abs(UInt128 value)
        {
            return value;
        }

        public static UInt128 operator +(UInt128 left, UInt128 right)
        {
            return Add(left, right);
        }

        public static UInt128 operator -(UInt128 left, UInt128 right)
        {
            return Subtract(left, right);
        }

        public static UInt128 Add(UInt128 left, UInt128 right)
        {
            ulong lo = left._lo + right._lo;
            bool flag = lo < Math.Max(left._lo, right._lo);
            return new UInt128((left._hi + right._hi) + (flag ? ((ulong) 1) : ((ulong) 0)), lo);
        }

        public static UInt128 Subtract(UInt128 left, UInt128 right)
        {
            ulong lo = left._lo - right._lo;
            bool flag = lo > left._lo;
            return new UInt128((left._hi - right._hi) - (flag ? ((ulong) 1) : ((ulong) 0)), lo);
        }

        public static UInt128 Divide(UInt128 dividend, UInt128 divisor)
        {
            UInt128 num;
            return DivRem(dividend, divisor, out num);
        }

        public static UInt128 DivRem(UInt128 dividend, UInt128 divisor, out UInt128 remainder)
        {
            uint[] numArray;
            uint[] numArray2;
            if (divisor == 0)
            {
                throw new DivideByZeroException();
            }
            DivRem(dividend.ToUIn32Array(), divisor.ToUIn32Array(), out numArray, out numArray2);
            remainder = new UInt128(numArray2);
            return new UInt128(numArray);
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

        public static UInt128 Remainder(UInt128 dividend, UInt128 divisor)
        {
            UInt128 num;
            DivRem(dividend, divisor, out num);
            return num;
        }

        public static UInt128 operator %(UInt128 dividend, UInt128 divisor)
        {
            return Remainder(dividend, divisor);
        }

        public static UInt128 operator /(UInt128 dividend, UInt128 divisor)
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

        public static UInt128 Multiply(UInt128 left, UInt128 right)
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
            return new UInt128(ints);
        }

        public static UInt128 operator *(UInt128 left, UInt128 right)
        {
            return Multiply(left, right);
        }

        public static UInt128 operator >>(UInt128 value, int shift)
        {
            return RightShift(value, shift);
        }

        public static UInt128 operator <<(UInt128 value, int shift)
        {
            return LeftShift(value, shift);
        }

        public static UInt128 RightShift(UInt128 value, int numberOfBits)
        {
            if (numberOfBits >= 0x80)
            {
                return Zero;
            }
            if (numberOfBits >= 0x40)
            {
                return new UInt128(0L, value._hi >> (numberOfBits - 0x40));
            }
            if (numberOfBits == 0)
            {
                return value;
            }
            return new UInt128(value._hi >> numberOfBits, (value._lo >> numberOfBits) + (value._hi << (0x40 - numberOfBits)));
        }

        public static UInt128 LeftShift(UInt128 value, int numberOfBits)
        {
            numberOfBits = numberOfBits % 0x80;
            if (numberOfBits >= 0x40)
            {
                return new UInt128(value._lo << (numberOfBits - 0x40), 0L);
            }
            if (numberOfBits == 0)
            {
                return value;
            }
            return new UInt128((value._hi << numberOfBits) + (value._lo >> (0x40 - numberOfBits)), value._lo << numberOfBits);
        }

        public static UInt128 operator |(UInt128 left, UInt128 right)
        {
            if (left == 0)
            {
                return right;
            }
            if (right == 0)
            {
                return left;
            }
            UInt128 num = left;
            num._hi |= right._hi;
            num._lo |= right._lo;
            return num;
        }

        public static UInt128 operator &(UInt128 left, UInt128 right)
        {
            return BitwiseAnd(left, right);
        }

        public static UInt128 BitwiseAnd(UInt128 left, UInt128 right)
        {
            return new UInt128(left._hi & right._hi, left._lo & right._lo);
        }

        static UInt128()
        {
            Zero = 0;
            One = 1;
            MinValue = 0;
            MaxValue = new UInt128(ulong.MaxValue, ulong.MaxValue);
        }
        public class UInt128Converter : TypeConverter
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
                UInt128 num;
                if ((value != null) && UInt128.TryParse(string.Format("{0}", value), out num))
                {
                    return num;
                }
                return new UInt128();
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

