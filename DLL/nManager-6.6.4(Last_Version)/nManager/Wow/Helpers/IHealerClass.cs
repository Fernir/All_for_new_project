namespace nManager.Wow.Helpers
{
    using System;

    public interface IHealerClass
    {
        void Dispose();
        void Initialize();
        void ResetConfiguration();
        void ShowConfiguration();

        float Range { get; }
    }
}

