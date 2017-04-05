namespace nManager.Wow.Helpers
{
    using nManager.Helpful;
    using nManager.Wow.Enums;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class WoWFactionTemplate
    {
        private static AnielOcoihij _kuaseatuasivuwIle;
        private static BinaryReader[] _miefe;
        private static FactionTemplateDbcRecord[] _qiwukopSiaqio;
        [CompilerGenerated]
        private FactionTemplateDbcRecord factionTemplateDbcRecord_0;
        [CompilerGenerated]
        private uint uint_0;

        private WoWFactionTemplate(uint reqId)
        {
            Awoup();
            FactionTemplateDbcRecord record = new FactionTemplateDbcRecord();
            bool flag = false;
            for (int i = 0; i < (_kuaseatuasivuwIle.get_RecordsCount() - 1); i++)
            {
                record = _qiwukopSiaqio[i];
                if (record.Id == reqId)
                {
                    flag = true;
                    break;
                }
            }
            this.Record = flag ? record : new FactionTemplateDbcRecord();
        }

        [CompilerGenerated]
        private static bool <init>b__0(Table t)
        {
            return (t.Name == "FactionTemplate");
        }

        [CompilerGenerated]
        private static bool <init>b__1(Table t)
        {
            return (t.Name == Path.GetFileName("FactionTemplate"));
        }

        private static void Awoup()
        {
            if (_kuaseatuasivuwIle == null)
            {
                DBFilesClient client = DBFilesClient.Load(Application.StartupPath + @"\Data\DBFilesClient\dblayout.xml");
                if (CS$<>9__CachedAnonymousMethodDelegate2 == null)
                {
                    CS$<>9__CachedAnonymousMethodDelegate2 = new Func<Table, bool>(WoWFactionTemplate.<init>b__0);
                }
                IEnumerable<Table> source = client.Tables.Where<Table>(CS$<>9__CachedAnonymousMethodDelegate2);
                if (!source.Any<Table>())
                {
                    if (CS$<>9__CachedAnonymousMethodDelegate3 == null)
                    {
                        CS$<>9__CachedAnonymousMethodDelegate3 = new Func<Table, bool>(WoWFactionTemplate.<init>b__1);
                    }
                    source = client.Tables.Where<Table>(CS$<>9__CachedAnonymousMethodDelegate3);
                }
                if (source.Count<Table>() == 1)
                {
                    Table def = source.First<Table>();
                    _kuaseatuasivuwIle = DBReaderFactory.GetReader(Application.StartupPath + @"\Data\DBFilesClient\FactionTemplate.db2", def) as AnielOcoihij;
                    if ((_miefe == null) && (_kuaseatuasivuwIle != null))
                    {
                        _miefe = _kuaseatuasivuwIle.get_Rows().ToArray<BinaryReader>();
                        _qiwukopSiaqio = new FactionTemplateDbcRecord[_miefe.Length];
                        for (int i = 0; i < (_miefe.Length - 1); i++)
                        {
                            _qiwukopSiaqio[i] = AnielOcoihij.Jejaebiniqeuf<FactionTemplateDbcRecord>(_miefe[i]);
                        }
                    }
                    Logging.Write(string.Concat(new object[] { _kuaseatuasivuwIle.get_FileName(), " loaded with ", _kuaseatuasivuwIle.get_RecordsCount(), " entries." }));
                }
                else
                {
                    Logging.Write("DB2 FactionTemplate not read-able.");
                }
            }
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
            uint num4 = (uint) ((~(record.FactionFlags >> 12) & 2) | 1);
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
            public ushort FactionId;
            public ushort FactionFlags;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=4)]
            public ushort[] EnemyFactions;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=4)]
            public ushort[] FriendlyFactions;
            public byte FightSupport;
            public byte FriendlyMask;
            public byte HostileMask;
            public byte Unknown;
        }
    }
}

