namespace nManager.Wow.Helpers
{
    using System;
    using System.Globalization;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct Coords4
    {
        public float X;
        public float Y;
        public float Z;
        public float O;
        public string GetCoordsAsString()
        {
            return ((((string.Empty + this.X.ToString(CultureInfo.InvariantCulture)) + " " + this.Y.ToString(CultureInfo.InvariantCulture)) + " " + this.Z.ToString(CultureInfo.InvariantCulture)) + " " + this.O.ToString(CultureInfo.InvariantCulture));
        }
    }
}

