namespace nManager.Wow.Helpers
{
    using System;
    using System.Globalization;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct Coords3
    {
        public float X;
        public float Y;
        public float Z;
        public string GetCoords()
        {
            return (((string.Empty + this.X.ToString(CultureInfo.InvariantCulture)) + " " + this.Y.ToString(CultureInfo.InvariantCulture)) + " " + this.Z.ToString(CultureInfo.InvariantCulture));
        }
    }
}

