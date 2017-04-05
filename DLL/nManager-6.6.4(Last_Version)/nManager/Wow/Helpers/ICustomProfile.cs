namespace nManager.Wow.Helpers
{
    using System;

    public interface ICustomProfile
    {
        void Dispose();
        void Initialize();
        void ResetConfiguration();
        void ShowConfiguration();

        bool DontStartFights { get; set; }

        bool IgnoreFight { get; set; }
    }
}

