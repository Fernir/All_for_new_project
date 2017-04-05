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

    public class WoWLock
    {
        private readonly LockDbcRecord _lockDBCRecord0;
        private static AnielOcoihij _roapivAhes;

        private WoWLock(uint id)
        {
            BamuodaFuhi();
            LockDbcRecord record = new LockDbcRecord();
            bool flag = false;
            for (int i = 0; i < (_roapivAhes.get_RecordsCount() - 1); i++)
            {
                record = AnielOcoihij.Jejaebiniqeuf<LockDbcRecord>(_roapivAhes.get_Rows().ElementAt<BinaryReader>(i));
                if (record.Id == id)
                {
                    flag = true;
                    break;
                }
            }
            this._lockDBCRecord0 = flag ? record : new LockDbcRecord();
        }

        [CompilerGenerated]
        private static bool <Init>b__0(Table t)
        {
            return (t.Name == "Lock");
        }

        [CompilerGenerated]
        private static bool <Init>b__1(Table t)
        {
            return (t.Name == Path.GetFileName("Lock"));
        }

        private static void BamuodaFuhi()
        {
            if (_roapivAhes == null)
            {
                DBFilesClient client = DBFilesClient.Load(Application.StartupPath + @"\Data\DBFilesClient\dblayout.xml");
                if (CS$<>9__CachedAnonymousMethodDelegate2 == null)
                {
                    CS$<>9__CachedAnonymousMethodDelegate2 = new Func<Table, bool>(WoWLock.<Init>b__0);
                }
                IEnumerable<Table> source = client.Tables.Where<Table>(CS$<>9__CachedAnonymousMethodDelegate2);
                if (!source.Any<Table>())
                {
                    if (CS$<>9__CachedAnonymousMethodDelegate3 == null)
                    {
                        CS$<>9__CachedAnonymousMethodDelegate3 = new Func<Table, bool>(WoWLock.<Init>b__1);
                    }
                    source = client.Tables.Where<Table>(CS$<>9__CachedAnonymousMethodDelegate3);
                }
                if (source.Count<Table>() == 1)
                {
                    Table def = source.First<Table>();
                    _roapivAhes = DBReaderFactory.GetReader(Application.StartupPath + @"\Data\DBFilesClient\Lock.db2", def) as AnielOcoihij;
                    Logging.Write(string.Concat(new object[] { _roapivAhes.get_FileName(), " loaded with ", _roapivAhes.get_RecordsCount(), " entries." }));
                }
                else
                {
                    Logging.Write("DB2 Lock not read-able.");
                }
            }
        }

        public static WoWLock FromId(uint id)
        {
            return new WoWLock(id);
        }

        public LockDbcRecord Record
        {
            get
            {
                return this._lockDBCRecord0;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct LockDbcRecord
        {
            public uint Id;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=8)]
            public uint[] LockType;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=8)]
            public ushort[] Skill;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=8)]
            public byte[] KeyType;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=8)]
            public byte[] Action;
        }
    }
}

