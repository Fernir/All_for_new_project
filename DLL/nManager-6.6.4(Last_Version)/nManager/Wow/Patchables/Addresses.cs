namespace nManager.Wow.Patchables
{
    using System;

    public static class Addresses
    {
        public enum ActivateSettings
        {
            Activate_Offset = 0x34,
            AutoDismount_Activate_Pointer = 0xf3dec4,
            AutoInteract_Activate_Pointer = 0xf3debc,
            AutoLoot_Activate_Pointer = 0xf3dedc,
            AutoSelfCast_Activate_Pointer = 0xf3dee8
        }

        public enum Battleground
        {
            MaxBattlegroundId = 0xfabda0,
            PvpExitWindow = 0xfab4a4,
            StatPvp = 0xd00298
        }

        public enum Chat
        {
            chatBufferPos = 0xff990c0,
            chatBufferStart = 0xf3f660,
            msgFormatedChat = 0x65,
            NextMessage = 0x17e8
        }

        public enum ClickToMove
        {
            CTM = 0xec43c8,
            CTM_X = 0xec43f0,
            CTM_Y = 0xec43f4,
            CTM_Z = 0xec43f8
        }

        public enum CorpsePlayer
        {
            X = 0xf3eddc,
            Y = 0xf3ede0,
            Z = 0xf3ede4
        }

        public enum DBC
        {
            FactionTemplate = 0,
            ItemClass = 0,
            ItemSubClass = 0,
            Lock = 0,
            Map = 0,
            QuestPOIPoint = 0,
            ResearchSite = 0,
            Spell = 0,
            SpellCategories = 0,
            Unknown = 0
        }

        public enum EventsListener
        {
            BaseEvents = 0xd75fd4,
            EventOffsetCount = 0x48,
            EventOffsetName = 0x18,
            EventsCount = 0xd75fd0
        }

        public enum Fishing
        {
            BobberHasMoved = 0xf8
        }

        public enum FunctionWow
        {
            CGPlayer_C__ClickToMove = 0x32fe34,
            CGUnit_C__Interact = 0x5341d,
            CGWorldFrame__Intersect = 0x6434bf,
            ClntObjMgrGetActivePlayerObj = 0x8dd5a,
            CTMChecker = 0x341fb9,
            CTMChecker2 = 0x550b85,
            FrameScript__GetLocalizedText = 0x32a5c0,
            FrameScript_ExecuteBuffer = 0xb2e28,
            GetTargetInfo = 0xac08e,
            IsOutdoors = 0,
            RetFromFunctionBelow = 0x1a5de3,
            ReturnFunc = 0x189004,
            Spell_C_HandleTerrainClick = 0x2dbe18,
            SpellChecker = 0xd88f20,
            SpellCheckerOff1 = 0xe44,
            SpellCheckerOff2 = 0xe48,
            SpellFixer = 0x10e2c3,
            strlen = 0x7ba7c0,
            UnitCanAttack = 0,
            WowClientDB2__GetRowPointer = 0x21aa3f,
            WoWTextCaller = 0x7415e7
        }

        public enum GameInfo
        {
            AreaId = 0xcf6fe8,
            buildWoWVersionString = 0xdcaac0,
            gameState = 0xf3de96,
            GetTime = 0xd75cc0,
            isLoading = 0xd690f0,
            LastHardwareAction = 0xd0e090,
            MapTextureId = 0xcfffa8,
            SubAreaId = 0xcf6fec,
            subZoneMap = 0xf3e0c0,
            TextBoxActivated = 0xbbe9ac,
            zoneMap = 0xf3e0b8
        }

        public enum GameObject
        {
            CachedCastBarCaption = 12,
            CachedData0 = 20,
            CachedIconName = 8,
            CachedName = 180,
            CachedQuestItem1 = 0x9c,
            CachedSize = 0x98,
            DBCacheRow = 620,
            GAMEOBJECT_FIELD_R = 0x148,
            GAMEOBJECT_FIELD_X = 0x138,
            GAMEOBJECT_FIELD_Y = 0x13c,
            GAMEOBJECT_FIELD_Z = 320,
            PackedRotationQuaternion = 0x148,
            TransformationMatrice = 0x270
        }

        public enum Hooking
        {
            DX_DEVICE = 0xd88f20,
            DX_DEVICE_IDX = 0x256c,
            ENDSCENE_IDX = 0xa8
        }

        public enum Login
        {
            realmName = 0x101fd3c,
            realmNameOffset = 0x394
        }

        public enum MovementFlagsOffsets
        {
            Offset1 = 0x124,
            Offset2 = 0x40
        }

        public enum ObjectManager
        {
            continentId = 0x108,
            firstObject = 0xd8,
            localGuid = 0xf8,
            nextObject = 0x44,
            objectGUID = 0x30,
            objectTYPE = 0x10
        }

        public class ObjectManagerClass
        {
            public static uint clientConnection;
            public static uint sCurMgr;
        }

        public enum Party
        {
            NumOfPlayers = 200,
            NumOfPlayersSuBGroup = 0xcc,
            PartyOffset = 0xf9c84c,
            PlayerGuid = 0x10
        }

        public enum PetBattle
        {
            IsInBattle = 0xcfe718
        }

        public enum Player
        {
            LocalPlayerSpellsOnCooldown = 0xdf75a0,
            petGUID = 0xfb2138,
            playerName = 0x1020550,
            RetrieveCorpseWindow = 0xf3ed64,
            RuneStartCooldown = 0xf18aa8,
            SkillMaxValue = 0x400,
            SkillValue = 0x200
        }

        public enum PlayerNameStore
        {
            PlayerNameNextOffset = 20,
            PlayerNameStorePtr = 0xdd5de0,
            PlayerNameStringOffset = 0x11
        }

        public enum PowerIndex
        {
            Multiplicator = 0x12,
            PowerIndexArrays = 0xec4454
        }

        public enum Quests
        {
            FlightMasterStatus = 240,
            QuestGiverStatus = 0xec,
            QuestId = 0xfcf6dc
        }

        public enum SpellBook
        {
            FirstTalentBookPtr = 0xf9c6d4,
            KnownAllSpells = 0xf9c5d8,
            MountBookMountsPtr = 0xf9c63c,
            MountBookNumMounts = 0xf9c638,
            NextTalentBookPtr = 0xf9c6cc,
            SpellBookNumSpells = 0xf9c5dc,
            SpellBookSpellsPtr = 0xf9c5e0,
            SpellDBCMaxIndex = 0x30d40,
            TalentBookOverrideSpellId = 0x1c,
            TalentBookSpellId = 20
        }

        public enum TargetSystem
        {
            Focus = 0x80,
            PtrToVMT = 0xcff3f4,
            Target = 40
        }

        public enum UnitBaseGetUnitAura
        {
            AuraSize = 0x88,
            AuraStructCasterLevel = 0x62,
            AuraStructCount = 0x61,
            AuraStructCreatorGuid = 0x48,
            AuraStructDuration = 100,
            AuraStructFlag = 0x5c,
            AuraStructMask = 0x5d,
            AuraStructSpellEndTime = 0x68,
            AuraStructSpellId = 0x58,
            AuraStructUnk1 = 0x63,
            AuraStructUnk2 = 0x6c,
            AuraTable1 = 0x1190,
            AuraTable2 = 0x880
        }

        public enum UnitField
        {
            CachedIsBoss = 0x60,
            CachedModelId1 = 0x6c,
            CachedName = 0x80,
            CachedQuestItem1 = 60,
            CachedSubName = 0,
            CachedTypeFlag = 0x24,
            CachedUnitClassification = 0x2c,
            CanInterrupt = 0xfac,
            CanInterruptOffset = 0xe02ea0,
            CanInterruptOffset2 = 0xe02ea4,
            CanInterruptOffset3 = 0xe02ea8,
            CastingSpellEndTime = 0x107c,
            CastingSpellID = 0x104c,
            CastingSpellStartTime = 0x1078,
            ChannelSpellEndTime = 0x108c,
            ChannelSpellID = 0x1080,
            ChannelSpellStartTime = 0x1088,
            DBCacheRow = 0xc68,
            TransportGUID = 0xac8,
            UNIT_FIELD_R = 0xae8,
            UNIT_FIELD_X = 0xad8,
            UNIT_FIELD_Y = 0xadc,
            UNIT_FIELD_Z = 0xae0,
            UNIT_VELOCITY = 140
        }

        public enum VMT
        {
            CGUnit_C__GetFacing = 0x39
        }
    }
}

