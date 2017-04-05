namespace nManager.Wow.Class
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class Quaternion
    {
        public Quaternion(long packedData)
        {
            this.X = (packedData >> 0x2a) * 4.768372E-07f;
            this.Y = ((packedData << 0x16) >> 0x2b) * 9.536743E-07f;
            this.Z = ((packedData << 0x2b) >> 0x2b) * 9.536743E-07f;
            double num = ((this.X * this.X) + (this.Y * this.Y)) + (this.Z * this.Z);
            if (Math.Abs((double) (num - 1.0)) >= 9.5367431640625E-07)
            {
                this.W = (float) Math.Sqrt(1.0 - num);
            }
            else
            {
                this.W = 0f;
            }
        }

        public Quaternion(float w, Point v)
        {
            this.X = v.X;
            this.Y = v.Y;
            this.Z = v.Z;
            this.W = w;
        }

        public Quaternion(float x, float y, float z, float w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        public override string ToString()
        {
            return string.Format("X: {0}, Y: {1}, Z: {2}, W: {3}", new object[] { this.X, this.Y, this.Z, this.W });
        }

        public float W { get; private set; }

        public float X { get; private set; }

        public float Y { get; private set; }

        public float Z { get; private set; }
    }
}

