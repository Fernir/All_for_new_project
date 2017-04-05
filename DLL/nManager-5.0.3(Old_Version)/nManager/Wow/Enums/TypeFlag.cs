namespace nManager.Wow.Enums
{
    using System;

    [Flags]
    public enum TypeFlag
    {
        ENGENEERING_LOOT = 0x8000,
        HERB_LOOT = 0x100,
        MINING_LOOT = 0x200,
        None = 0
    }
}

