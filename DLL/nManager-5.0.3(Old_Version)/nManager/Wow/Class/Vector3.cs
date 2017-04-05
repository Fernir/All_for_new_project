namespace nManager.Wow.Class
{
    using nManager.Wow.ObjectManager;
    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Xml.Serialization;

    [Serializable]
    public class Vector3 : IComparable<Vector3>, IEquatable<Vector3>, IComparable, IFormattable
    {
        public static readonly Vector3 origin = new Vector3(0f, 0f, 0f);
        public static readonly Vector3 xAxis = new Vector3(1f, 0f, 0f);
        public static readonly Vector3 yAxis = new Vector3(0f, 1f, 0f);
        public static readonly Vector3 zAxis = new Vector3(0f, 0f, 1f);

        public Vector3()
        {
            this.X = 0f;
            this.Y = 0f;
            this.Z = 0f;
        }

        public Vector3(Vector3 other)
        {
            this.X = other.X;
            this.Y = other.Y;
            this.Z = other.Z;
        }

        public Vector3(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public static float Abs(Vector3 v1)
        {
            return v1.Magnitude;
        }

        public float Angle2D(Vector3 other)
        {
            return Angle2D(this, other);
        }

        public static float Angle2D(Vector3 v1, Vector3 v2)
        {
            double num = Math.Atan2((double) v2.Y, (double) v2.X) - Math.Atan2((double) v1.Y, (double) v1.X);
            if (num > 3.1415926535897931)
            {
                num -= 6.2831853071795862;
            }
            else if (num < -3.1415926535897931)
            {
                num += 6.2831853071795862;
            }
            return (float) num;
        }

        public int CompareTo(Vector3 other)
        {
            if (this < other)
            {
                return -1;
            }
            if (this > other)
            {
                return 1;
            }
            return 0;
        }

        public int CompareTo(object obj)
        {
            if ((obj == null) || (base.GetType() != obj.GetType()))
            {
                throw new ArgumentException("Cannot compare a Vector to a non-Vector and you passed a " + obj.GetType().ToString());
            }
            Vector3 vector = obj as Vector3;
            if (this < vector)
            {
                return -1;
            }
            if (this > vector)
            {
                return 1;
            }
            return 0;
        }

        public Vector3 Cross(Vector3 other)
        {
            return Cross(this, other);
        }

        public static Vector3 Cross(Vector3 v1, Vector3 v2)
        {
            return new Vector3((v1.Y * v2.Z) - (v1.Z * v2.Y), (v1.Z * v2.X) - (v1.X * v2.Z), (v1.X * v2.Y) - (v1.Y * v2.X));
        }

        public float DistanceTo(Vector3 other)
        {
            return (this - other).Magnitude;
        }

        public float DistanceTo2D(Vector3 other)
        {
            return DistanceTo2D(this, other);
        }

        public static float DistanceTo2D(Vector3 v1, Vector3 v2)
        {
            return (float) Math.Sqrt((double) (((v1.X - v2.X) * (v1.X - v2.X)) + ((v1.Y - v2.Y) * (v1.Y - v2.Y))));
        }

        public float DistanceZ(Vector3 other)
        {
            return DistanceZ(this, other);
        }

        public static float DistanceZ(Vector3 v1, Vector3 v2)
        {
            return Math.Abs((float) (v1.Z - v2.Z));
        }

        public float Dot(Vector3 other)
        {
            return Dot(this, other);
        }

        public static float Dot(Vector3 v1, Vector3 v2)
        {
            return (((v1.X * v2.X) + (v1.Y * v2.Y)) + (v1.Z * v2.Z));
        }

        public bool Equals(Vector3 other)
        {
            return (other == this);
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || (base.GetType() != obj.GetType()))
            {
                return false;
            }
            Vector3 vector = obj as Vector3;
            return (vector == this);
        }

        public Vector3 GameObjectLocalScale(WoWGameObject o)
        {
            float size = o.Size;
            if (size == 0f)
            {
                size = 1f;
            }
            return new Vector3(size, size, size);
        }

        public override int GetHashCode()
        {
            return ((((this.X.GetHashCode() * -17) + (this.Y.GetHashCode() * 7)) + (this.Z.GetHashCode() * 3)) % 0x7fffffff);
        }

        public bool IsUnitVector()
        {
            return IsUnitVector(this);
        }

        public static bool IsUnitVector(Vector3 v1)
        {
            return (v1.Magnitude == 1f);
        }

        public void Normalize()
        {
            Vector3 vector = Normalize(this);
            this.X = vector.X;
            this.Y = vector.Y;
            this.Z = vector.Z;
        }

        public static Vector3 Normalize(Vector3 v1)
        {
            if (v1.Magnitude == 0f)
            {
                throw new DivideByZeroException("Cannot normalize a vector of magnitude zero");
            }
            float num = 1f / v1.Magnitude;
            return new Vector3(v1.X * num, v1.Y * num, v1.Z * num);
        }

        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static Vector3 operator /(Vector3 v1, float s2)
        {
            return new Vector3(v1.X / s2, v1.Y / s2, v1.Z / s2);
        }

        public static bool operator ==(Vector3 v1, Vector3 v2)
        {
            if (v1 == null)
            {
                return (v2 == null);
            }
            if (v2 == null)
            {
                return (v1 == null);
            }
            return (((v1.X == v2.X) && (v1.Y == v2.Y)) && (v1.Z == v2.Z));
        }

        public static bool operator >(Vector3 v1, Vector3 v2)
        {
            return (v1.Magnitude > v2.Magnitude);
        }

        public static bool operator >=(Vector3 v1, Vector3 v2)
        {
            return (v1.Magnitude >= v2.Magnitude);
        }

        public static bool operator !=(Vector3 v1, Vector3 v2)
        {
            return !(v1 == v2);
        }

        public static bool operator <(Vector3 v1, Vector3 v2)
        {
            return (v1.Magnitude < v2.Magnitude);
        }

        public static bool operator <=(Vector3 v1, Vector3 v2)
        {
            return (v1.Magnitude <= v2.Magnitude);
        }

        public static Vector3 operator *(Vector3 v1, float s2)
        {
            return new Vector3(v1.X * s2, v1.Y * s2, v1.Z * s2);
        }

        public static Vector3 operator *(float s1, Vector3 v2)
        {
            return (Vector3) (v2 * s1);
        }

        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        public static Vector3 operator -(Vector3 v1)
        {
            return new Vector3(-v1.X, -v1.Y, -v1.Z);
        }

        public static Vector3 operator +(Vector3 v1)
        {
            return new Vector3(v1.X, v1.Y, v1.Z);
        }

        public static Vector3 Pitch(Vector3 v1, float degree)
        {
            float x = v1.X;
            float y = (v1.Y * ((float) Math.Cos((double) degree))) - (v1.Z * ((float) Math.Sin((double) degree)));
            return new Vector3(x, y, (v1.Y * ((float) Math.Sin((double) degree))) + (v1.Z * ((float) Math.Cos((double) degree))));
        }

        public void Roll(float degree)
        {
            Vector3 vector = Roll(this, degree);
            this.X = vector.X;
            this.Y = vector.Y;
            this.Z = vector.Z;
        }

        public static Vector3 Roll(Vector3 v1, float degree)
        {
            float x = (v1.X * ((float) Math.Cos((double) degree))) - (v1.Y * ((float) Math.Sin((double) degree)));
            float y = (v1.X * ((float) Math.Sin((double) degree))) + (v1.Y * ((float) Math.Cos((double) degree)));
            return new Vector3(x, y, v1.Z);
        }

        public override string ToString()
        {
            return string.Format("({0}, {1}, {2})", this.X, this.Y, this.Z);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if ((format == null) || (format == ""))
            {
                return string.Format("({0}, {1}, {2})", this.X, this.Y, this.Z);
            }
            char ch = format[0];
            string str = null;
            if (format.Length > 1)
            {
                str = format.Substring(1);
            }
            switch (ch)
            {
                case 'x':
                    return this.X.ToString(str, formatProvider);

                case 'y':
                    return this.Y.ToString(str, formatProvider);

                case 'z':
                    return this.Z.ToString(str, formatProvider);
            }
            return string.Format("({0}, {1}, {2})", this.X.ToString(format, formatProvider), this.Y.ToString(format, formatProvider), this.Z.ToString(format, formatProvider));
        }

        public Vector3 Transform(Matrix4 matrix)
        {
            return new Vector3 { X = (((this.X * matrix.xx) + (this.Y * matrix.xy)) + (this.Z * matrix.xz)) + matrix.xw, Y = (((this.X * matrix.yx) + (this.Y * matrix.yy)) + (this.Z * matrix.yz)) + matrix.yw, Z = (((this.X * matrix.zx) + (this.Y * matrix.zy)) + (this.Z * matrix.zz)) + matrix.zw };
        }

        public Vector3 TransformInvert(Matrix4 m)
        {
            return this.Transform(m.Invert());
        }

        public Vector3 TransformInvert(WoWGameObject o)
        {
            return this.TransformInvert(o.WorldMatrix);
        }

        public void Yaw(float degree)
        {
            Vector3 vector = Yaw(this, degree);
            this.X = vector.X;
            this.Y = vector.Y;
            this.Z = vector.Z;
        }

        public static Vector3 Yaw(Vector3 v1, float degree)
        {
            float x = (v1.Z * ((float) Math.Sin((double) degree))) + (v1.X * ((float) Math.Cos((double) degree)));
            float y = v1.Y;
            return new Vector3(x, y, (v1.Z * ((float) Math.Cos((double) degree))) - (v1.X * ((float) Math.Sin((double) degree))));
        }

        [XmlIgnore]
        public float[] Array
        {
            get
            {
                return new float[] { this.X, this.Y, this.Z };
            }
            set
            {
                if (value.Length != 3)
                {
                    throw new ArgumentException("Array must contain exactly three values: [x,y,z]");
                }
                this.X = value[0];
                this.Y = value[1];
                this.Z = value[2];
            }
        }

        [XmlIgnore]
        public bool IsValid
        {
            get
            {
                return (this != origin);
            }
        }

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return this.X;

                    case 1:
                        return this.Y;

                    case 2:
                        return this.Z;
                }
                throw new ArgumentException("Array must contain exactly three values: [x,y,z]", "index");
            }
            set
            {
                switch (index)
                {
                    case 0:
                        this.X = value;
                        return;

                    case 1:
                        this.Y = value;
                        return;

                    case 2:
                        this.Z = value;
                        return;
                }
                throw new ArgumentException("Array must contain exactly three values: [x,y,z]", "index");
            }
        }

        [XmlIgnore]
        public float Magnitude
        {
            get
            {
                return (float) Math.Sqrt((double) (((this.X * this.X) + (this.Y * this.Y)) + (this.Z * this.Z)));
            }
            set
            {
                if ((value >= 0f) && (this != origin))
                {
                    Vector3 vector = (Vector3) (this * (value / this.Magnitude));
                    this.X = vector.X;
                    this.Y = vector.Y;
                    this.Z = vector.Z;
                }
            }
        }

        public float X { get; set; }

        public float Y { get; set; }

        public float Z { get; set; }
    }
}

