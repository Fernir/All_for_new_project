namespace nManager.Wow.Patchables
{
    using System;

    public static class Addresses
    {
        public enum ActivateSettings
        {
            Activate_Offset = 0x34,
            AutoDismount_Activate_Pointer = 0xda574c,
            AutoInteract_Activate_Pointer = 0xda5744,
            AutoLoot_Activate_Pointer = 0xda5764,
            AutoSelfCast_Activate_Pointer = 0xda5774
        }

        public enum Battleground
        {
            MaxBattlegroundId = 0xe03a2c,
            PvpExitWindow = 0xe03a60,
            StatPvp = 0xbc0688
        }

        public enum Chat
        {
            chatBufferPos = 0xe01894,
            chatBufferStart = 0xda7518,
            msgFormatedChat = 0x65,
            NextMessage = 0x17e8
        }

        public enum ClickToMove
        {
            CTM = 0xd2f560,
            CTM_PUSH = 0xd2f57c,
            CTM_X = 0xd2f5e4,
            CTM_Y = 0xd2f5e8,
            CTM_Z = 0xd2f5ec
        }

        public enum CorpsePlayer
        {
            X = 0xda594c,
            Y = 0xda5950,
            Z = 0xda5954
        }

        public enum DBC
        {
            FactionTemplate = 0xc64c34,
            ItemClass = 0xc68a90,
            ItemSubClass = 0xc698fc,
            Lock = 0xc6aa0c,
            Map = 0xc74bb8,
            QuestPOIPoint = 0xc6d890,
            ResearchSite = 0xc6e868,
            SpellCategories = 0xc6d85c
        }

        public enum EventsListener
        {
            BaseEvents = 0xc054bc,
            EventOffsetCount = 0x48,
            EventOffsetName = 0x18,
            EventsCount = 0xc054b8
        }

        public enum Fishing
        {
            BobberHasMoved = 260
        }

        public enum FunctionWow
        {
            CGUnit_C__InitializeTrackingState = 0x30d869,
            CGUnit_C__Interact = 0x94fb3e,
            CGWorldFrame__Intersect = 0x5676d0,
            ClntObjMgrGetActivePlayerObj = 0x3c47,
            FrameScript__GetLocalizedText = 0x2fe479,
            FrameScript_ExecuteBuffer = 0x27dd1,
            IsOutdoors = 0,
            Spell_C_HandleTerrainClick = 0x2859b2,
            strlen = 0x6c9e50,
            UnitCanAttack = 0
        }

        public enum GameInfo
        {
            AreaId = 0xbb3f78,
            buildWoWVersionString = 0xc5db08,
            gameState = 0xda55b2,
            GetTime = 0xc05060,
            isLoading = 0xbf8754,
            LastHardwareAction = 0xd0e090,
            MapTextureId = 0xbc0a64,
            SubAreaId = 0xbb3f70,
            subZoneMap = 0xda55a4,
            TextBoxActivated = 0xbbe9ac,
            zoneMap = 0xda55a8
        }

        public enum GameObject
        {
            CachedCastBarCaption = 12,
            CachedData0 = 20,
            CachedIconName = 8,
            CachedName = 180,
            CachedQuestItem1 = 0x9c,
            CachedSize = 0x98,
            DBCacheRow = 0x274,
            GAMEOBJECT_FIELD_X = 680,
            GAMEOBJECT_FIELD_Y = 0x2ac,
            GAMEOBJECT_FIELD_Z = 0x2b0,
            PackedRotationQuaternion = 0x150,
            TransformationMatrice = 0x278
        }

        public enum Hooking
        {
            DX_DEVICE = 0xc12248,
            DX_DEVICE_IDX = 0x28b8,
            ENDSCENE_IDX = 0xa8
        }

        public enum Login
        {
            realmName = 0xe981c6
        }

        public enum MovementFlagsOffsets
        {
            Offset1 = 300,
            Offset2 = 0x40
        }

        public enum ObjectManager
        {
            continentId = 0x108,
            firstObject = 0xd8,
            localGuid = 0xf8,
            nextObject = 60,
            objectGUID = 40,
            objectTYPE = 12
        }

        public class ObjectManagerClass
        {
            public static uint clientConnection;
            public static uint sCurMgr;
        }

        public enum Party
        {
            NumOfPlayers = 0xcc,
            NumOfPlayersSuBGroup = 0xd0,
            PartyOffset = 0xe031f4,
            PlayerGuid = 0x10
        }

        public enum PetBattle
        {
            IsInBattle = 0xba8a10
        }

        public enum Player
        {
            LocalPlayerSpellsOnCooldown = 0xc8ac80,
            petGUID = 0xe18ba0,
            playerName = 0xe981d0,
            RetrieveCorpseWindow = 0xda5614,
            RuneStartCooldown = 0xf18aa8,
            SkillMaxValue = 0x400,
            SkillValue = 0x200
        }

        public enum PlayerNameStore
        {
            PlayerNameNextOffset = 20,
            PlayerNameStorePtr = 0xc60428,
            PlayerNameStringOffset = 0x11
        }

        public enum PowerIndex
        {
            Multiplicator = 0x10,
            PowerIndexArrays = 0xd2f1bc
        }

        public enum Quests
        {
            QuestGiverStatus = 0xf4
        }

        public enum SpellBook
        {
            FirstTalentBookPtr = 0xe0309c,
            KnownAllSpells = 0xe02ee0,
            MountBookMountsPtr = 0xe02f44,
            MountBookNumMounts = 0xe02f40,
            NextTalentBookPtr = 0xe03094,
            SpellBookNumSpells = 0xe02ee4,
            SpellBookSpellsPtr = 0xe02ee8,
            SpellDBCMaxIndex = 0x30d40,
            TalentBookOverrideSpellId = 0x1c,
            TalentBookSpellId = 20
        }

        public enum UnitBaseGetUnitAura
        {
            AuraSize = 0x48,
            AuraStructCasterLevel = 0x3a,
            AuraStructCount = 0x39,
            AuraStructCreatorGuid = 0x20,
            AuraStructDuration = 60,
            AuraStructFlag = 0x34,
            AuraStructMask = 0x35,
            AuraStructSpellEndTime = 0x40,
            AuraStructSpellId = 0x30,
            AuraStructUnk1 = 0x3b,
            AuraStructUnk2 = 0x44,
            AuraTable1 = 0x1108,
            AuraTable2 = 0x480
        }

        public enum UnitField
        {
            CachedIsBoss = 0x5c,
            CachedModelId1 = 0x6c,
            CachedName = 0x7c,
            CachedQuestItem1 = 60,
            CachedSubName = 0,
            CachedTypeFlag = 0x24,
            CachedUnitClassification = 0x2c,
            CanInterrupt = 0xf2c,
            CanInterruptOffset = 0xe02ea0,
            CanInterruptOffset2 = 0xe02ea4,
            CanInterruptOffset3 = 0xe02ea8,
            CastingSpellEndTime = 0xfb4,
            CastingSpellID = 0xf98,
            CastingSpellStartTime = 0xfb0,
            ChannelSpellEndTime = 0xfc0,
            ChannelSpellID = 0xfb8,
            ChannelSpellStartTime = 0xfbc,
            DBCacheRow = 0xc38,
            TransportGUID = 0xab0,
            UNIT_FIELD_R = 0xad0,
            UNIT_FIELD_X = 0xac0,
            UNIT_FIELD_Y = 0xac4,
            UNIT_FIELD_Z = 0xac8
        }

        public enum VMT
        {
            CGUnit_C__GetFacing = 0x2c
        }
    }
}

