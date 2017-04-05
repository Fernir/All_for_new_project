namespace nManager.Wow.Class
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [Serializable]
    public class Matrix4
    {
        private Matrix4()
        {
            this.set_X(new MatrixX());
            this.set_Y(new MatrixY());
            this.Z = new MatrixZ();
            this.W = new MatrixW();
        }

        public Matrix4(Matrix4 matrix)
        {
            this.set_X(matrix.get_X());
            this.set_Y(matrix.get_Y());
            this.Z = matrix.Z;
            this.W = matrix.W;
        }

        public Matrix4(MatrixX x, MatrixY y, MatrixZ z, MatrixW w)
        {
            this.set_X(x);
            this.set_Y(y);
            this.Z = z;
            this.W = w;
        }

        public Matrix4 Invert()
        {
            Matrix4 matrix = this;
            float num = (this.xx * this.yy) - (this.yx * this.xy);
            float num2 = (this.xx * this.yz) - (this.yx * this.xz);
            float num3 = (this.xx * this.yw) - (this.yx * this.xw);
            float num4 = (this.xy * this.yz) - (this.yy * this.xz);
            float num5 = (this.xy * this.yw) - (this.yy * this.xw);
            float num6 = (this.xz * this.yw) - (this.yz * this.xw);
            float num7 = (this.zz * this.ww) - (this.wz * this.zw);
            float num8 = (this.zy * this.ww) - (this.wy * this.zw);
            float num9 = (this.zy * this.wz) - (this.wy * this.zz);
            float num10 = (this.zx * this.ww) - (this.wx * this.zw);
            float num11 = (this.zx * this.wz) - (this.wx * this.zz);
            float num12 = (this.zx * this.wy) - (this.wx * this.zy);
            float num13 = 1f / ((((((num * num7) - (num2 * num8)) + (num3 * num9)) + (num4 * num10)) - (num5 * num11)) + (num6 * num12));
            matrix.xx = (((this.yy * num7) - (this.yz * num8)) + (this.yw * num9)) * num13;
            matrix.xy = (((-this.xy * num7) + (this.xz * num8)) - (this.xw * num9)) * num13;
            matrix.xz = (((this.wy * num6) - (this.wz * num5)) + (this.ww * num4)) * num13;
            matrix.xw = (((-this.zy * num6) + (this.zz * num5)) - (this.zw * num4)) * num13;
            matrix.yx = (((-this.yx * num7) + (this.yz * num10)) - (this.yw * num11)) * num13;
            matrix.yy = (((this.xx * num7) - (this.xz * num10)) + (this.xw * num11)) * num13;
            matrix.yz = (((-this.wx * num6) + (this.wz * num3)) - (this.ww * num2)) * num13;
            matrix.yw = (((this.zx * num6) - (this.zz * num3)) + (this.zw * num2)) * num13;
            matrix.zx = (((this.yx * num8) - (this.yy * num10)) + (this.yw * num12)) * num13;
            matrix.zy = (((-this.xx * num8) + (this.xy * num10)) - (this.xw * num12)) * num13;
            matrix.zz = (((this.wx * num5) - (this.wy * num3)) + (this.ww * num)) * num13;
            matrix.zw = (((-this.zx * num5) + (this.zy * num3)) - (this.zw * num)) * num13;
            return matrix;
        }

        public override string ToString()
        {
            return string.Format("xx={0}, yx={1}, zx={2}, wx={3}, xy={4}, yy={5}, zy={6}, wy={7}, xz={8}, yz={9}, zz={10}, wz={11}, xw={12}, yw={13}, zw={14}, ww={15}", new object[] { this.xx, this.yx, this.zx, this.wx, this.xy, this.yy, this.zy, this.wy, this.xz, this.yz, this.zz, this.wz, this.xw, this.yw, this.zw, this.ww });
        }

        private MatrixX _povibepiabeifeIju { get; set; }

        private MatrixY _ufeusuawoututu { get; set; }

        public MatrixW W { get; set; }

        public float ww
        {
            get
            {
                return this.W.ww;
            }
            set
            {
                this.W.ww = value;
            }
        }

        public float wx
        {
            get
            {
                return this.get_X().wx;
            }
            set
            {
                this.get_X().wx = value;
            }
        }

        public float wy
        {
            get
            {
                return this.get_Y().wy;
            }
            set
            {
                this.get_Y().wy = value;
            }
        }

        public float wz
        {
            get
            {
                return this.Z.wz;
            }
            set
            {
                this.Z.wz = value;
            }
        }

        public float xw
        {
            get
            {
                return this.W.xw;
            }
            set
            {
                this.W.xw = value;
            }
        }

        public float xx
        {
            get
            {
                return this.get_X().xx;
            }
            set
            {
                this.get_X().xx = value;
            }
        }

        public float xy
        {
            get
            {
                return this.get_Y().xy;
            }
            set
            {
                this.get_Y().xy = value;
            }
        }

        public float xz
        {
            get
            {
                return this.Z.xz;
            }
            set
            {
                this.Z.xz = value;
            }
        }

        public float yw
        {
            get
            {
                return this.W.yw;
            }
            set
            {
                this.W.yw = value;
            }
        }

        public float yx
        {
            get
            {
                return this.get_X().yx;
            }
            set
            {
                this.get_X().yx = value;
            }
        }

        public float yy
        {
            get
            {
                return this.get_Y().yy;
            }
            set
            {
                this.get_Y().yy = value;
            }
        }

        public float yz
        {
            get
            {
                return this.Z.yz;
            }
            set
            {
                this.Z.yz = value;
            }
        }

        public MatrixZ Z { get; set; }

        public float zw
        {
            get
            {
                return this.W.zw;
            }
            set
            {
                this.W.zw = value;
            }
        }

        public float zx
        {
            get
            {
                return this.get_X().zx;
            }
            set
            {
                this.get_X().zx = value;
            }
        }

        public float zy
        {
            get
            {
                return this.get_Y().zy;
            }
            set
            {
                this.get_Y().zy = value;
            }
        }

        public float zz
        {
            get
            {
                return this.Z.zz;
            }
            set
            {
                this.Z.zz = value;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MatrixColumn
        {
            public float m1;
            public float m2;
            public float m3;
            public float m4;
        }

        public class MatrixW
        {
            public float ww;
            public float xw;
            public float yw;
            public float zw;

            public MatrixW()
            {
                this.xw = 0f;
                this.yw = 0f;
                this.zw = 0f;
                this.ww = 0f;
            }

            public MatrixW(Matrix4.MatrixW matrix)
            {
                this.xw = matrix.xw;
                this.yw = matrix.yw;
                this.zw = matrix.zw;
                this.ww = matrix.ww;
            }

            public MatrixW(float x, float y, float z, float w)
            {
                this.xw = x;
                this.yw = y;
                this.zw = z;
                this.ww = w;
            }
        }

        public class MatrixX
        {
            public float wx;
            public float xx;
            public float yx;
            public float zx;

            public MatrixX()
            {
                this.xx = 0f;
                this.yx = 0f;
                this.zx = 0f;
                this.wx = 0f;
            }

            public MatrixX(Matrix4.MatrixX matrix)
            {
                this.xx = matrix.xx;
                this.yx = matrix.yx;
                this.zx = matrix.zx;
                this.wx = matrix.wx;
            }

            public MatrixX(float x, float y, float z, float w)
            {
                this.xx = x;
                this.yx = y;
                this.zx = z;
                this.wx = w;
            }
        }

        public class MatrixY
        {
            public float wy;
            public float xy;
            public float yy;
            public float zy;

            public MatrixY()
            {
                this.xy = 0f;
                this.yy = 0f;
                this.zy = 0f;
                this.wy = 0f;
            }

            public MatrixY(Matrix4.MatrixY matrix)
            {
                this.xy = matrix.xy;
                this.yy = matrix.yy;
                this.zy = matrix.zy;
                this.wy = matrix.wy;
            }

            public MatrixY(float x, float y, float z, float w)
            {
                this.xy = x;
                this.yy = y;
                this.zy = z;
                this.wy = w;
            }
        }

        public class MatrixZ
        {
            public float wz;
            public float xz;
            public float yz;
            public float zz;

            public MatrixZ()
            {
                this.xz = 0f;
                this.yz = 0f;
                this.zz = 0f;
                this.wz = 0f;
            }

            public MatrixZ(Matrix4.MatrixZ matrix)
            {
                this.xz = matrix.xz;
                this.yz = matrix.yz;
                this.zz = matrix.zz;
                this.wz = matrix.wz;
            }

            public MatrixZ(float x, float y, float z, float w)
            {
                this.xz = x;
                this.yz = y;
                this.zz = z;
                this.wz = w;
            }
        }
    }
}

