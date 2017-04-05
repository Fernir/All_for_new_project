namespace nManager.Wow.Enums
{
    using System;

    [Flags]
    public enum UnitAuraFlags : byte
    {
        Active = 4,
        BasePoints = 0x40,
        Cancelable = 2,
        Duration = 0x20,
        Harmful = 0x10,
        Negative = 0x80,
        None = 0,
        Passive = 1,
        PlayerCasted = 8
    }
}

