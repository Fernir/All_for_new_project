namespace nManager.Helpful
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Security;

    [SuppressUnmanagedCodeSecurity]
    internal sealed class Mibuwiugeawi
    {
        [DllImport("user32.dll")]
        internal static extern bool GetClientRect(IntPtr hWnd, out nManager.Helpful.RECT lpRect);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        internal static extern bool GetWindowRect(IntPtr hWnd, out nManager.Helpful.RECT lpRect);
        internal static Rectangle Ileadujeuf(IntPtr edusaiguokairUreocacao)
        {
            nManager.Helpful.RECT lpRect = new nManager.Helpful.RECT();
            GetWindowRect(edusaiguokairUreocacao, out lpRect);
            return lpRect.AsRectangle;
        }

        internal static Rectangle UvuwixoparoIjihuo(IntPtr afejiebou)
        {
            Rectangle rectangle = Ileadujeuf(afejiebou);
            Rectangle rectangle2 = Xaleakaehoaho(afejiebou);
            int num = (rectangle.Width - rectangle2.Width) / 2;
            return new Rectangle(new Point(rectangle.X + num, rectangle.Y + ((rectangle.Height - rectangle2.Height) - num)), rectangle2.Size);
        }

        internal static Rectangle Xaleakaehoaho(IntPtr edusaiguokairUreocacao)
        {
            nManager.Helpful.RECT lpRect = new nManager.Helpful.RECT();
            GetClientRect(edusaiguokairUreocacao, out lpRect);
            return lpRect.AsRectangle;
        }
    }
}

