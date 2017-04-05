namespace nManager.Wow.Enums
{
    using System;

    [Flags]
    public enum UnitQuestGiverStatus
    {
        None,
        Unavailable,
        LowLevelAvailable,
        LowLevelTurnInRepeatable,
        LowLevelAvailableRepeatable,
        Incomplete,
        TurnInRepeatable,
        AvailableRepeatable,
        Available,
        TurnInInvisible,
        TurnIn
    }
}

