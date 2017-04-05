namespace nManager.Wow.Enums
{
    using System;

    [Flags]
    public enum UnitFlags : uint
    {
        CanPerformAction_Mask1 = 0x60000,
        Combat = 0x80000,
        Confused = 0x400000,
        Dazed = 0x20000000,
        Disarmed = 0x200000,
        Flag_14_0x4000 = 0x4000,
        Flag_15_0x8000 = 0x8000,
        Flag_9_0x200 = 0x200,
        Fleeing = 0x800000,
        Influenced = 4,
        Looting = 0x400,
        Mounted = 0x8000000,
        None = 0,
        NotAttackable = 0x100,
        NotSelectable = 0x2000000,
        Pacified = 0x20000,
        PetInCombat = 0x800,
        PlayerControlled = 8,
        PlusMob = 0x40,
        Possessed = 0x1000000,
        Preparation = 0x20,
        PvPFlagged = 0x1000,
        SelectableNotAttackable_1 = 2,
        SelectableNotAttackable_2 = 0x80,
        SelectableNotAttackable_3 = 0x10000,
        Sheathe = 0x40000000,
        Silenced = 0x2000,
        Sitting = 1,
        Skinnable = 0x4000000,
        Stunned = 0x40000,
        TaxiFlight = 0x100000,
        Totem = 0x10
    }
}

