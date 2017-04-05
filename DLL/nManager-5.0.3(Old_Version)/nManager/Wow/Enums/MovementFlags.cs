namespace nManager.Wow.Enums
{
    using System;

    [Flags]
    public enum MovementFlags : uint
    {
        Ascending = 0x200000,
        CanFly = 0x800000,
        Descending = 0x400000,
        Falling = 0x800,
        FallingFar = 0x1000,
        FallMask = 0x1800,
        Flying = 0x1000000,
        Hover = 0x20000000,
        Levitating = 0x200,
        MotionMask = 0xff,
        MoveMask = 0x3f,
        MovingBackwards = 2,
        MovingForward = 1,
        None = 0,
        PendingBackward = 0x10000,
        PendingForward = 0x8000,
        PendingRoot = 0x80000,
        PendingStop = 0x2000,
        PendingStrafeLeft = 0x20000,
        PendingStrafeRight = 0x40000,
        PendingStrafeStop = 0x4000,
        PitchDown = 0x80,
        PitchMask = 0xc0,
        PitchUp = 0x40,
        PlayerFlag = 0x80000000,
        Root = 0x400,
        SafeFall = 0x10000000,
        SplineElevation = 0x2000000,
        SplineEnabled = 0x4000000,
        StrafeMask = 12,
        StrafingLeft = 4,
        StrafingRight = 8,
        Swimming = 0x100000,
        TurningLeft = 0x10,
        TurningRight = 0x20,
        TurnMask = 0x30,
        Walk = 0x100,
        WaterWalking = 0x8000000
    }
}

