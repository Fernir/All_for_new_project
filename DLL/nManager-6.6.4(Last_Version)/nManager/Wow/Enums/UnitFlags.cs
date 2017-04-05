namespace nManager.Wow.Enums
{
    using System;

    [Flags]
    public enum UnitFlags : uint
    {
        CannotSwim = 0x4000,
        CanPerformActionMask1 = 0x60000,
        Confused = 0x400000,
        Feared = 0x800000,
        ImmuneToNpc = 0x200,
        ImmuneToPlayer = 0x100,
        InCombat = 0x80000,
        Influenced = 4,
        Looting = 0x400,
        MainHandDisarmed = 0x200000,
        Mount = 0x8000000,
        NoAttack = 0x80,
        NoAttack2 = 0x10000,
        None = 0,
        NotSelectable = 0x2000000,
        OnlySwim = 0x8000,
        OnTaxi = 0x100000,
        Pacified = 0x20000,
        PetInCombat = 0x800,
        PlayerCannotAttack = 2,
        PlusMob = 0x40,
        PossessedByPlayer = 0x1000000,
        Preparation = 0x20,
        PreventEmotes = 0x20000000,
        PreventKneelingWhenLooting = 0x10000000,
        PVPAttackable = 8,
        PvPFlagged = 0x1000,
        Rename = 0x10,
        ServerControlled = 1,
        Sheath = 0x40000000,
        Sheathe = 0x40000000,
        Silenced = 0x2000,
        Skinnable = 0x4000000,
        Stunned = 0x40000,
        Unk31 = 0x80000000
    }
}

