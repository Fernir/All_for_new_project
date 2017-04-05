namespace nManager.Wow.Enums
{
    using System;

    [Flags]
    public enum CGWorldFrameHitFlags : uint
    {
        HitTestAll = 0x10151,
        HitTestAllButLiquid = 0x151,
        HitTestBoundingModels = 1,
        HitTestGround = 0x100,
        HitTestGroundAndStructures = 0x100111,
        HitTestLiquid = 0x10000,
        HitTestLOS = 0x100011,
        HitTestMovableObjects = 0x100000,
        HitTestNothing = 0,
        HitTestUnknown = 0x40,
        HitTestUnknown2 = 0x20000,
        HitTestWMO = 0x10
    }
}

