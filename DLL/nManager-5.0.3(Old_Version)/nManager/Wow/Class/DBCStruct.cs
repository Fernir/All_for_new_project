﻿namespace nManager.Wow.Class
{
    using System;
    using System.Runtime.InteropServices;

    public class DBCStruct
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct SpellCastTimesRec
        {
            public int Id;
            public int CastTime;
            public int SpellCastTimes;
            public int MinCastTime;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SpellMiscRec
        {
            public int unk1;
            public int int0;
            public int int4;
            public int int8;
            public int intC;
            public int int10;
            public int int14;
            public int int18;
            public int int1C_Flags;
            public int int20;
            public int int24;
            public int int28;
            public int int2C;
            public int int30;
            public int int34;
            public int int38;
            public int SpellCastTimesId;
            public int SpellDurationId;
            public int SpellRangeId;
            public float float48_TimeOrSpeedRelated;
            public int SpellVisualId;
            public int SpellVisualId_OverrideMaybe;
            public int SpellIconId;
            public int int58;
            public int int5C_Flags;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SpellRangeRec
        {
            public int Id;
            public float MinRangeHostile;
            public float MinRangeFriend;
            public float MaxRangeHostile;
            public float MaxRangeFriend;
            public int Flags;
            public int DisplayName_lang;
            public int DisplayNameShort_lang;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SpellRec
        {
            public int SpellId;
            public uint Name;
            public uint RankDescription;
            public int LongDescription;
            public int ShortDescription;
            public int SpellRuneCostId;
            public int SpellMissileID;
            public int SpellDescriptionVariableID;
            public float unk_float1;
            public int SpellScalingId;
            public int SpellAuraOptionsId;
            public int SpellAuraRestrictionsId;
            public int SpellCastingRequirementsId;
            public int SpellCategoriesId;
            public int SpellClassOptionsId;
            public int SpellCooldownsID;
            public int SpellEquippedItemsId;
            public int SpellinterruptsId;
            public int SpellLevelId;
            public int SpellReagentsId;
            public int SpellShapeshiftId;
            public int SpellTargetRestrictionsId;
            public int SpellTotemsId;
            public int ResearchProjectId;
            public int SpellMiscId;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WoWClientDB
        {
            public IntPtr VTable;
            public int NumRows;
            public int MaxIndex;
            public int MinIndex;
            public uint Unk4bytes;
            public IntPtr Data;
            public IntPtr FirstRow;
            public IntPtr Rows;
            public IntPtr Unk1;
            public uint Unk2;
            public IntPtr Unk3;
            public uint RowEntrySize;
        }
    }
}

