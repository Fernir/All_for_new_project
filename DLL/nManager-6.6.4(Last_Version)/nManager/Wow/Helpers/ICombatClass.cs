namespace nManager.Wow.Helpers
{
    using nManager.Wow.Class;
    using System;

    public interface ICombatClass
    {
        void Dispose();
        void Initialize();
        void ResetConfiguration();
        void ShowConfiguration();

        float AggroRange { get; }

        Spell LightHealingSpell { get; }

        float Range { get; }
    }
}

