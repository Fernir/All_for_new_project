namespace nManager.Wow.Class
{
    using nManager.Helpful;
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [Serializable]
    public class Point : Vector3
    {
        public Point()
        {
            base.X = 0f;
            base.Y = 0f;
            base.Z = 0f;
            this.Type = "None";
        }

        public Point(float[] array)
        {
            base.X = array[0];
            base.Y = array[1];
            base.Z = array[2];
            this.Type = "None";
        }

        public Point(Point other)
        {
            base.X = other.X;
            base.Y = other.Y;
            base.Z = other.Z;
            this.Type = other.Type;
        }

        public Point(Vector3 v)
        {
            base.X = v.X;
            base.Y = v.Y;
            base.Z = v.Z;
            this.Type = "None";
        }

        public Point(string v)
        {
            string[] strArray = v.Split(new char[] { ';' });
            if (!v.Contains(";") || (strArray.Length < 3))
            {
                base.X = 0f;
                base.Y = 0f;
                base.Z = 0f;
            }
            else
            {
                base.X = Others.ToSingle(strArray[0]);
                base.Y = Others.ToSingle(strArray[1]);
                base.Z = Others.ToSingle(strArray[2]);
                if (((base.X == 0f) || (base.Y == 0f)) || (base.Z == 0f))
                {
                    base.X = 0f;
                    base.Y = 0f;
                    base.Z = 0f;
                }
            }
            if (strArray.Length < 4)
            {
                this.Type = "None";
            }
            else
            {
                switch (strArray[3].ToLower())
                {
                    case "swimming":
                        this.Type = "Swimming";
                        return;

                    case "flying":
                        this.Type = "Flying";
                        return;
                }
                this.Type = "None";
            }
        }

        public Point(float x, float y, float z, string type = "None")
        {
            base.X = x;
            base.Y = y;
            base.Z = z;
            this.Type = type;
        }

        public bool Equals(Point obj)
        {
            if (System.Math.Abs((float) (base.X - obj.X)) > 0.001)
            {
                return false;
            }
            if (System.Math.Abs((float) (base.Y - obj.Y)) > 0.001)
            {
                return false;
            }
            if (System.Math.Abs((float) (base.Z - obj.Z)) > 0.001)
            {
                return false;
            }
            if (this.Type != obj.Type)
            {
                return false;
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            return ((obj is Point) && base.Equals((Vector3) (obj as Point)));
        }

        public override int GetHashCode()
        {
            return ((base.GetHashCode() * 0x18d) ^ ((this.Type != null) ? this.Type.GetHashCode() : 0));
        }

        public override string ToString()
        {
            return string.Format("{0} ; {1} ; {2} ; {3}", new object[] { base.X, base.Y, base.Z, this.Type });
        }

        [DefaultValue("None")]
        public string Type { get; set; }
    }
}

