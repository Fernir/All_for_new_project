namespace nManager.Wow.Helpers.PathFinderClass
{
    using DetourLayer;
    using nManager.Helpful;
    using System;
    using System.Runtime.CompilerServices;

    internal class Unafileune : Exception
    {
        public Unafileune(DetourStatus status, string text) : base(string.Concat(new object[] { text, " (", status, ")" }))
        {
            try
            {
                this.set_Status(status);
            }
            catch (Exception exception)
            {
                Logging.WriteError("NavMeshException(DetourStatus status, string text): " + exception, true);
            }
        }

        public DetourStatus _ulaoxouwiukoul { get; private set; }
    }
}

