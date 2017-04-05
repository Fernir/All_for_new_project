namespace nManager.Wow.Helpers.PathFinderClass
{
    using nManager.Wow.Class;
    using System;
    using System.Runtime.CompilerServices;

    internal class Hop
    {
        public string FlightTarget { get; set; }

        public Point Location { get; set; }

        public HopType Type { get; set; }
    }
}

