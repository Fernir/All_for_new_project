namespace nManager.Wow.Helpers
{
    using System;

    public interface ICombatClass
    {
        void Dispose();
        void Initialize();
        void ResetConfiguration();
        void ShowConfiguration();

        float AggroRange { get; }

        float Range { get; }
    }
}

