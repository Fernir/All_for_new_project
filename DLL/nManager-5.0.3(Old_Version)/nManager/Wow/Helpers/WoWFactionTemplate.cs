namespace nManager.Wow.Helpers
{
    using nManager.Wow.Enums;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class WoWFactionTemplate
    {
        private static DBC<FactionTemplateDbcRecord> factionTemplateDBC;
        [CompilerGenerated]
        private FactionTemplateDbcRecord factionTemplateDbcRecord_0;
        [CompilerGenerated]
        private uint uint_0;

        private WoWFactionTemplate(uint id)
        {
            this.Id = id;
            if (factionTemplateDBC == null)
            {
                factionTemplateDBC = new DBC<FactionTemplateDbcRecord>(0xc64c34);
            }
            this.Record = factionTemplateDBC.GetRow((int) id);
        }

        public static WoWFactionTemplate FromId(uint id)
        {
            return new WoWFactionTemplate(id);
        }

        public Reaction GetReactionTowards(WoWFactionTemplate otherFaction)
        {
            FactionTemplateDbcRecord record = this.Record;
            FactionTemplateDbcRecord record2 = otherFaction.Record;
            if ((record2.FightSupport & record.HostileMask) != 0)
            {
                return Reaction.Hostile;
            }
            if (record.EnemyFactions != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (record.EnemyFactions[i] == 0)
                    {
                        break;
                    }
                    if (record.EnemyFactions[i] == record2.FactionId)
                    {
                        return Reaction.Hostile;
                    }
                }
            }
            if ((record2.FightSupport & record.FriendlyMask) != 0)
            {
                return Reaction.Friendly;
            }
            if (record.FriendlyFactions != null)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (record.FriendlyFactions[j] == 0)
                    {
                        break;
                    }
                    if (record.FriendlyFactions[j] == record2.FactionId)
                    {
                        return Reaction.Friendly;
                    }
                }
            }
            if ((record.FightSupport & record2.FriendlyMask) != 0)
            {
                return Reaction.Friendly;
            }
            if (record2.FriendlyFactions != null)
            {
                for (int k = 0; k < 4; k++)
                {
                    if (record2.FriendlyFactions[k] == 0)
                    {
                        break;
                    }
                    if (record2.FriendlyFactions[k] == record.FactionId)
                    {
                        return Reaction.Friendly;
                    }
                }
            }
            uint num4 = (~(record.FactionFlags >> 12) & 2) | 1;
            return (Reaction) num4;
        }

        public uint Id
        {
            [CompilerGenerated]
            get
            {
                return this.uint_0;
            }
            [CompilerGenerated]
            private set
            {
                this.uint_0 = value;
            }
        }

        public FactionTemplateDbcRecord Record
        {
            [CompilerGenerated]
            get
            {
                return this.factionTemplateDbcRecord_0;
            }
            [CompilerGenerated]
            private set
            {
                this.factionTemplateDbcRecord_0 = value;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FactionTemplateDbcRecord
        {
            public uint Id;
            public uint FactionId;
            public uint FactionFlags;
            public uint FightSupport;
            public uint FriendlyMask;
            public uint HostileMask;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=4)]
            public uint[] EnemyFactions;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=4)]
            public uint[] FriendlyFactions;
        }
    }
}

