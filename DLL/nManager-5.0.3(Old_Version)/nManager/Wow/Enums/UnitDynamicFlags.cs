namespace nManager.Wow.Enums
{
    using System;

    [Flags]
    public enum UnitDynamicFlags
    {
        Dead = 0x40,
        Invisible = 1,
        IsTappedByAllThreatList = 0x100,
        Lootable = 2,
        None = 0,
        ReferAFriendLinked = 0x80,
        SpecialInfo = 0x20,
        Tapped = 8,
        TappedByMe = 0x10,
        TrackUnit = 4
    }
}

