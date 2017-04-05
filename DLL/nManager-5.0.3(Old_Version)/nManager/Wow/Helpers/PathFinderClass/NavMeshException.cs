namespace nManager.Wow.Helpers.PathFinderClass
{
    using DetourLayer;
    using nManager.Helpful;
    using System;
    using System.Runtime.CompilerServices;

    internal class NavMeshException : Exception
    {
        public NavMeshException(DetourStatus status, string text) : base(string.Concat(new object[] { text, " (", status, ")" }))
        {
            try
            {
                this.Status = status;
            }
            catch (Exception exception)
            {
                Logging.WriteError("NavMeshException(DetourStatus status, string text): " + exception, true);
            }
        }

        public DetourStatus Status { get; private set; }
    }
}

