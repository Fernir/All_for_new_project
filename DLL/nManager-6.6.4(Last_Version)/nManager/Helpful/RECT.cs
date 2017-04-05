namespace nManager.Helpful
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    internal struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
        public RECT(int left, int top, int right, int bottom)
        {
            this.Left = left;
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
        }

        public Rectangle AsRectangle
        {
            get
            {
                return new Rectangle(this.Left, this.Top, this.Right - this.Left, this.Bottom - this.Top);
            }
        }
        public static nManager.Helpful.RECT FromXYWH(int x, int y, int width, int height)
        {
            return new nManager.Helpful.RECT(x, y, x + width, y + height);
        }

        public static nManager.Helpful.RECT FromRectangle(Rectangle rect)
        {
            return new nManager.Helpful.RECT(rect.Left, rect.Top, rect.Right, rect.Bottom);
        }
    }
}

