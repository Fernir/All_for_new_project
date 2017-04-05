namespace nManager.Wow.Patchables
{
    using System;

    public static class Descriptors
    {
        public const uint Multiplicator = 4;
        public static readonly uint StartDescriptors = 8;

        public enum AreaTriggerFields
        {
            BoundsRadius2D = 0x17,
            Caster = 12,
            CreatingEffectGUID = 0x19,
            DecalPropertiesID = 0x18,
            Duration = 0x10,
            SpellForVisuals = 0x15,
            SpellID = 20,
            SpellXSpellVisualID = 0x16,
            TimeToTarget = 0x11,
            TimeToTargetExtraScale = 0x13,
            TimeToTargetScale = 0x12
        }

        public enum ContainerFields
        {
            NumSlots = 0xe5,
            Slots = 0x55
        }

        public enum ConversationData
        {
        }

        public enum ConversationDynamicData
        {
        }

        public enum CorpseFields
        {
            CustomDisplayOption = 0x2d,
            DisplayID = 20,
            DynamicFlags = 0x2b,
            FacialHairStyleID = 0x29,
            FactionTemplate = 0x2c,
            Flags = 0x2a,
            Items = 0x15,
            Owner = 12,
            PartyGUID = 0x10,
            SkinID = 40
        }

        public enum DynamicObjectFields
        {
            Caster = 12,
            CastTime = 20,
            Radius = 0x13,
            SpellID = 0x12,
            SpellXSpellVisualID = 0x11,
            Type = 0x10
        }

        public enum GameObjectFields
        {
            CreatedBy = 12,
            DisplayID = 0x10,
            FactionTemplate = 0x16,
            Flags = 0x11,
            Level = 0x17,
            ParentRotation = 0x12,
            PercentHealth = 0x18,
            SpawnTrackingStateAnimID = 0x1b,
            SpawnTrackingStateAnimKitID = 0x1c,
            SpellVisualID = 0x19,
            StateSpellVisualID = 0x1a,
            StateWorldEffectID = 0x1d
        }

        public enum ItemDynamicFields
        {
        }

        public enum ItemFields
        {
            ArtifactXP = 0x52,
            ContainedIn = 0x10,
            Context = 0x51,
            CreatePlayedTime = 0x4f,
            Creator = 20,
            Durability = 0x4d,
            DynamicFlags = 0x23,
            Enchantment = 0x24,
            Expiration = 0x1d,
            GiftCreator = 0x18,
            ItemAppearanceModID = 0x54,
            MaxDurability = 0x4e,
            ModifiersMask = 80,
            Owner = 12,
            PropertySeed = 0x4b,
            RandomPropertiesID = 0x4c,
            SpellCharges = 30,
            StackCount = 0x1c
        }

        public enum ObjectFields
        {
            Data = 4,
            DynamicFlags = 10,
            EntryID = 9,
            Guid = 0,
            Scale = 11,
            Type = 8
        }

        public enum PlayerDynamicFields
        {
        }

        public enum PlayerFields
        {
            ArenaFaction = 0xe8,
            AuraVision = 0xac7,
            AvgItemLevel = 0x436,
            Avoidance = 0x917,
            BagSlotFlags = 0xacc,
            BankBagSlotFlags = 0xad0,
            BlockPercentage = 0x909,
            BuybackPrice = 0xa4a,
            BuybackTimestamp = 0xa56,
            CharacterPoints = 0x901,
            Coinage = 0x73d,
            CombatRatingExpertise = 0x908,
            CombatRatings = 0xa65,
            CritPercentage = 0x90e,
            CurrentBattlePetBreedQuality = 0x43a,
            CurrentSpecID = 0x434,
            CustomDisplayOption = 230,
            DodgePercentage = 0x90a,
            DodgePercentageFromAttribute = 0x90b,
            DuelArbiter = 0xd4,
            DuelTeam = 0xe9,
            ExploredZones = 0x91d,
            FakeInebriation = 0x432,
            FarsightObject = 0x729,
            GuildDeleteDate = 0xe3,
            GuildLevel = 0xe4,
            GuildRankID = 0xe2,
            GuildTimeStamp = 0xea,
            HairColorID = 0xe5,
            HomeRealmTimeOffset = 0xac5,
            Honor = 0x11ae,
            HonorLevel = 0x43c,
            HonorNextLevel = 0x11af,
            Inebriation = 0xe7,
            InsertItemsLeftToRight = 0xad7,
            InvSlots = 0x43d,
            KnownTitles = 0x731,
            LfgBonusFactionID = 0xac9,
            Lifesteal = 0x916,
            LifetimeHonorableKills = 0xa63,
            LocalFlags = 0xa46,
            LootSpecID = 0xaca,
            LootTargetGUID = 220,
            MainhandExpertise = 0x905,
            Mastery = 0x914,
            MaxCreatureScalingLevel = 0xab1,
            MaxLevel = 0xaaf,
            MaxTalentTiers = 0x902,
            ModDamageDoneNeg = 0xa28,
            ModDamageDonePercent = 0xa2f,
            ModDamageDonePos = 0xa21,
            ModHealingDonePercent = 0xa38,
            ModHealingDonePos = 0xa36,
            ModHealingPercent = 0xa37,
            ModPeriodicHealingDonePercent = 0xa39,
            ModPetHaste = 0xac6,
            ModResiliencePercent = 0xa41,
            ModSpellPowerPercent = 0xa40,
            ModTargetPhysicalResistance = 0xa45,
            ModTargetResistance = 0xa44,
            NextLevelXP = 0x740,
            NoReagentCostMask = 0xab2,
            NumRespecs = 0xa47,
            OffhandCritPercentage = 0x910,
            OffhandExpertise = 0x906,
            OverrideAPBySpellPowerPercent = 0xa43,
            OverrideSpellPowerByAPPercent = 0xa42,
            OverrideSpellsID = 0xac8,
            OverrideZonePVPType = 0xacb,
            ParryPercentage = 0x90c,
            ParryPercentageFromAttribute = 0x90d,
            PetSpellPower = 0xab6,
            PlayerFlags = 0xe0,
            PlayerFlagsEx = 0xe1,
            PlayerTitle = 0x431,
            Prestige = 0x43b,
            ProfessionSkillLine = 0xac1,
            PvpInfo = 0xa85,
            PvpMedals = 0xa49,
            PvpPowerDamage = 0x91b,
            PvpPowerHealing = 0x91c,
            QuestCompleted = 0xad8,
            QuestLog = 0xeb,
            RangedCritPercentage = 0x90f,
            RangedExpertise = 0x907,
            Researching = 0xab7,
            RestInfo = 0xa1d,
            ScalingPlayerLevelDelta = 0xab0,
            SelfResSpell = 0xa48,
            ShieldBlock = 0x912,
            ShieldBlockCritPercentage = 0x913,
            Skill = 0x741,
            Speed = 0x915,
            SpellCritPercentage = 0x911,
            Sturdiness = 0x918,
            SummonedBattlePetGUID = 0x72d,
            TaxiMountAnimKitID = 0x435,
            TrackCreatureMask = 0x903,
            TrackResourceMask = 0x904,
            UiHitModifier = 0xac3,
            UiSpellHitModifier = 0xac4,
            Versatility = 0x919,
            VersatilityBonus = 0x91a,
            VirtualPlayerRealm = 0x433,
            VisibleItems = 0x40b,
            WatchedFactionIndex = 0xa64,
            WeaponAtkSpeedMultipliers = 0xa3d,
            WeaponDmgMultipliers = 0xa3a,
            WowAccount = 0xd8,
            XP = 0x73f,
            YesterdayHonorableKills = 0xa62
        }

        public enum SceneObjectFields
        {
            CreatedBy = 14,
            RndSeedVal = 13,
            SceneType = 0x12,
            ScriptPackageID = 12
        }

        public enum UnitDynamicFields
        {
        }

        public enum UnitFields
        {
            AnimTier = 0x70,
            AttackPower = 0xa4,
            AttackPowerModNeg = 0xa6,
            AttackPowerModPos = 0xa5,
            AttackPowerMultiplier = 0xa7,
            AttackRoundBaseTime = 100,
            AuraState = 0x63,
            BaseHealth = 0xa2,
            BaseMana = 0xa1,
            BattlePetCompanionGUID = 0x2c,
            BattlePetCompanionNameTimestamp = 0xc3,
            BattlePetDBID = 0x30,
            BoundingRadius = 0x67,
            ChannelSpell = 50,
            ChannelSpellXSpellVisual = 0x33,
            Charm = 12,
            CharmedBy = 0x18,
            CombatReach = 0x68,
            CreatedBy = 0x20,
            CreatedBySpell = 0x7b,
            Critter = 20,
            DemonCreator = 0x24,
            DisplayID = 0x69,
            DisplayPower = 0x36,
            EffectiveLevel = 0x55,
            EmoteState = 0x7e,
            FactionTemplate = 0x59,
            Flags = 0x60,
            Flags2 = 0x61,
            Flags3 = 0x62,
            Health = 0x38,
            HoverHeight = 190,
            InteractSpellID = 0xc4,
            Level = 0x54,
            LookAtControllerID = 0xcf,
            LookAtControllerTarget = 0xd0,
            LooksLikeCreatureID = 0xce,
            LooksLikeMountID = 0xcd,
            MaxDamage = 0x6d,
            MaxHealth = 0x40,
            MaxHealthModifier = 0xbd,
            MaxItemLevel = 0xc1,
            MaxOffHandDamage = 0x6f,
            MaxPower = 0x42,
            MaxRangedDamage = 0xae,
            MinDamage = 0x6c,
            MinItemLevel = 0xc0,
            MinItemLevelCutoff = 0xbf,
            MinOffHandDamage = 110,
            MinRangedDamage = 0xad,
            ModBonusArmor = 160,
            ModCastingSpeed = 0x75,
            ModHaste = 0x77,
            ModHasteRegen = 0x79,
            ModRangedHaste = 120,
            ModSpellHaste = 0x76,
            ModTimeRate = 0x7a,
            MountDisplayID = 0x6b,
            NativeDisplayID = 0x6a,
            NpcFlags = 0x7c,
            OverrideDisplayPowerID = 0x37,
            PetExperience = 0x73,
            PetNameTimestamp = 0x72,
            PetNextLevelExperience = 0x74,
            PetNumber = 0x71,
            Power = 0x3a,
            PowerCostModifier = 0xaf,
            PowerCostMultiplier = 0xb6,
            PowerRegenFlatModifier = 0x48,
            PowerRegenInterruptedFlatModifier = 0x4e,
            RangedAttackPower = 0xa8,
            RangedAttackPowerModNeg = 170,
            RangedAttackPowerModPos = 0xa9,
            RangedAttackPowerMultiplier = 0xab,
            RangedAttackRoundBaseTime = 0x66,
            ResistanceBuffModsNegative = 0x99,
            ResistanceBuffModsPositive = 0x92,
            Resistances = 0x8b,
            ScaleDuration = 0xcc,
            ScalingLevelDelta = 0x58,
            ScalingLevelMax = 0x57,
            ScalingLevelMin = 0x56,
            SetAttackSpeedAura = 0xac,
            Sex = 0x35,
            ShapeshiftForm = 0xa3,
            StateAnimID = 0xc6,
            StateAnimKitID = 0xc7,
            StateSpellVisualID = 0xc5,
            StateWorldEffectID = 200,
            StatNegBuff = 0x87,
            StatPosBuff = 0x83,
            Stats = 0x7f,
            Summon = 0x10,
            SummonedBy = 0x1c,
            SummonedByHomeRealm = 0x34,
            Target = 40,
            VirtualItems = 90,
            WildBattlePetLevel = 0xc2
        }
    }
}

