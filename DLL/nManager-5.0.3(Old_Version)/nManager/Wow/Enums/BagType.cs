namespace nManager.Wow.Enums
{
    using System;

    [Flags]
    public enum BagType
    {
        AmmoPouch = 2,
        EnchantingBag = 0x40,
        EngineeringBag = 0x80,
        GemBag = 0x200,
        HerbBag = 0x20,
        InscriptionBag = 0x10,
        Keyring = 0x100,
        LeatherworkingBag = 8,
        LureBag = 0x8000,
        MiningBag = 0x400,
        None = 0x1000000,
        Quiver = 1,
        SoulBag = 4,
        Unknown = 0x800,
        Unspecified = 0,
        VanityPets = 0x1000
    }
}

