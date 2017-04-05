namespace nManager.Wow.Helpers
{
    using System;
    using System.ComponentModel;

    [Serializable]
    public class TaxiLink
    {
        public uint PointA;
        public uint PointB;
        [DefaultValue("")]
        public string PointB_XY;
    }
}

