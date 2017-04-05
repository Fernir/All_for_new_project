namespace nManager.Wow.Helpers
{
    using nManager.Helpful;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class WoWItemSubClass
    {
        private ItemSubClassDbcRecord _menaufiwagerVi;
        private static AnielOcoihij _qogiejeajuecod;

        private WoWItemSubClass(string name, uint iClass)
        {
            BamuodaFuhi();
            ItemSubClassDbcRecord record = new ItemSubClassDbcRecord();
            bool flag = false;
            for (int i = 0; i < (_qogiejeajuecod.get_RecordsCount() - 1); i++)
            {
                record = AnielOcoihij.Jejaebiniqeuf<ItemSubClassDbcRecord>(_qogiejeajuecod.get_Rows().ElementAt<BinaryReader>(i));
                if ((record.LongName() == name) && (record.ClassId == iClass))
                {
                    flag = true;
                    break;
                }
            }
            this._menaufiwagerVi = flag ? record : new ItemSubClassDbcRecord();
        }

        [CompilerGenerated]
        private static bool <Init>b__0(Table t)
        {
            return (t.Name == "ItemSubClass");
        }

        [CompilerGenerated]
        private static bool <Init>b__1(Table t)
        {
            return (t.Name == Path.GetFileName("ItemSubClass"));
        }

        private static void BamuodaFuhi()
        {
            if (_qogiejeajuecod == null)
            {
                DBFilesClient client = DBFilesClient.Load(Application.StartupPath + @"\Data\DBFilesClient\dblayout.xml");
                if (CS$<>9__CachedAnonymousMethodDelegate2 == null)
                {
                    CS$<>9__CachedAnonymousMethodDelegate2 = new Func<Table, bool>(WoWItemSubClass.<Init>b__0);
                }
                IEnumerable<Table> source = client.Tables.Where<Table>(CS$<>9__CachedAnonymousMethodDelegate2);
                if (!source.Any<Table>())
                {
                    if (CS$<>9__CachedAnonymousMethodDelegate3 == null)
                    {
                        CS$<>9__CachedAnonymousMethodDelegate3 = new Func<Table, bool>(WoWItemSubClass.<Init>b__1);
                    }
                    source = client.Tables.Where<Table>(CS$<>9__CachedAnonymousMethodDelegate3);
                }
                if (source.Count<Table>() == 1)
                {
                    Table def = source.First<Table>();
                    _qogiejeajuecod = DBReaderFactory.GetReader(Application.StartupPath + @"\Data\DBFilesClient\ItemSubClass.db2", def) as AnielOcoihij;
                    Logging.Write(string.Concat(new object[] { _qogiejeajuecod.get_FileName(), " loaded with ", _qogiejeajuecod.get_RecordsCount(), " entries." }));
                }
                else
                {
                    Logging.Write("DB2 ItemSubClass not read-able.");
                }
            }
        }

        public static WoWItemSubClass FromNameAndClass(string name, uint iClass)
        {
            return new WoWItemSubClass(name, iClass);
        }

        public string LongName
        {
            get
            {
                return this._menaufiwagerVi.LongName();
            }
        }

        public string Name
        {
            get
            {
                return this._menaufiwagerVi.Name();
            }
        }

        public ItemSubClassDbcRecord Record
        {
            get
            {
                return this._menaufiwagerVi;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ItemSubClassDbcRecord
        {
            public uint Index;
            public uint SubClassNameOffset;
            public uint SubClassLongNameOffset;
            public ushort field0C;
            public byte ClassId;
            public byte SubClassId;
            public byte field10;
            public byte field11;
            public byte field12;
            public byte field13;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=4)]
            public byte[] field14;
            public string Name()
            {
                string str;
                if ((WoWItemSubClass._qogiejeajuecod.get_StringTable() != null) && WoWItemSubClass._qogiejeajuecod.get_StringTable().TryGetValue((int) this.SubClassNameOffset, out str))
                {
                    return str;
                }
                return "";
            }

            public string LongName()
            {
                string str;
                if ((WoWItemSubClass._qogiejeajuecod.get_StringTable() != null) && WoWItemSubClass._qogiejeajuecod.get_StringTable().TryGetValue((int) this.SubClassLongNameOffset, out str))
                {
                    return str;
                }
                return "";
            }
        }
    }
}

