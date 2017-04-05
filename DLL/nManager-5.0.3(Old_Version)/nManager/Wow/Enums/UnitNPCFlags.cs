namespace nManager.Wow.Enums
{
    using System;

    [Flags]
    public enum UnitNPCFlags
    {
        Auctioneer = 0x200000,
        Banker = 0x20000,
        BattleMaster = 0x100000,
        BattlePetTrainer = 0x20,
        CanRepair = 0x1000,
        CanTrain = 0x10,
        DailyQuests = 0x40,
        ForgeMaster = 0x8000000,
        Gossip = 1,
        GuildBanker = 0x800000,
        Innkeeper = 0x10000,
        MailInfo = 0x4000000,
        None = 0,
        QuestGiver = 2,
        Reagents = 0x800,
        SellsFood = 0x200,
        SpiritGuide = 0x8000,
        SpiritHealer = 0x4000,
        StableMaster = 0x400000,
        Taxi = 0x2000,
        Transmogrifier = 0x10000000,
        Vendor = 0x80,
        VoidStorageBanker = 0x20000000
    }
}

