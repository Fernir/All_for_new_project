namespace nManager.Wow.Helpers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;

    internal class AnielOcoihij : IClientDBReader
    {
        private SortedDictionary<int, byte[]> _aqouraeqaisome;
        private const int _arineoteap = 0x30;
        public const uint _iniwateluneax = 0x35424457;
        private List<Onuseifouveile> _utuavau;

        public AnielOcoihij(Stream stream) : this(new BinaryReader(stream, Encoding.UTF8), false)
        {
        }

        public AnielOcoihij(string fileName) : this(new FileStream(fileName, FileMode.Open))
        {
            this.set_FileName(fileName);
        }

        public AnielOcoihij(BinaryReader reader, bool headerOnly = false)
        {
            this._aqouraeqaisome = new SortedDictionary<int, byte[]>();
            int num = this.EmuliogenoFogiwever(reader);
            if (!headerOnly)
            {
                using (reader)
                {
                    long num2 = 0x30 + ((this.get_HasIndexTable() ? (this.get_FieldsCount() - 1) : this.get_FieldsCount()) * 4);
                    long num4 = reader.BaseStream.Length - this.get_CopyTableSize();
                    long num5 = num4 - (this.get_HasIndexTable() ? ((long) (num * 4)) : ((long) 0));
                    long num6 = num5 - (this.get_IsSparseTable() ? ((long) 0) : ((long) this.get_StringTableSize()));
                    int[] numArray = null;
                    if (this.get_HasIndexTable())
                    {
                        reader.BaseStream.Position = num5;
                        numArray = new int[num];
                        for (int i = 0; i < num; i++)
                        {
                            numArray[i] = reader.ReadInt32();
                        }
                    }
                    if (this.get_IsSparseTable())
                    {
                        reader.BaseStream.Position = this.get_StringTableSize();
                        int num8 = (this.get_MaxId() - this.get_MinId()) + 1;
                        for (int j = 0; j < num8; j++)
                        {
                            int num10 = reader.ReadInt32();
                            int count = reader.ReadInt16();
                            if ((num10 != 0) && (count != 0))
                            {
                                int num12 = this.get_MinId() + j;
                                long position = reader.BaseStream.Position;
                                reader.BaseStream.Position = num10;
                                byte[] sourceArray = reader.ReadBytes(count);
                                byte[] destinationArray = new byte[sourceArray.Length + 4];
                                Array.Copy(BitConverter.GetBytes(num12), destinationArray, 4);
                                Array.Copy(sourceArray, 0, destinationArray, 4, sourceArray.Length);
                                this._aqouraeqaisome.Add(num12, destinationArray);
                                reader.BaseStream.Position = position;
                            }
                        }
                    }
                    else
                    {
                        reader.BaseStream.Position = num2;
                        for (int k = 0; k < num; k++)
                        {
                            reader.BaseStream.Position = num2 + (k * this.get_RecordSize());
                            byte[] buffer3 = reader.ReadBytes(this.get_RecordSize());
                            if (this.get_HasIndexTable())
                            {
                                byte[] buffer4 = new byte[this.get_RecordSize() + 4];
                                Array.Copy(BitConverter.GetBytes(numArray[k]), buffer4, 4);
                                Array.Copy(buffer3, 0, buffer4, 4, buffer3.Length);
                                this._aqouraeqaisome.Add(numArray[k], buffer4);
                            }
                            else
                            {
                                int num15 = (0x20 - this._utuavau[this.get_IdIndex()]._kapetUxu) >> 3;
                                int num16 = this._utuavau[this.get_IdIndex()]._kauvireaCi;
                                int key = 0;
                                for (int m = 0; m < num15; m++)
                                {
                                    key |= buffer3[num16 + m] << (m * 8);
                                }
                                this._aqouraeqaisome.Add(key, buffer3);
                            }
                        }
                        reader.BaseStream.Position = num6;
                        this.set_StringTable(new Dictionary<int, string>());
                        while (reader.BaseStream.Position != (num6 + this.get_StringTableSize()))
                        {
                            int num19 = (int) (reader.BaseStream.Position - num6);
                            this.get_StringTable()[num19] = reader.Boutoehaidiad();
                        }
                    }
                    if ((num4 != reader.BaseStream.Length) && (this.get_CopyTableSize() != 0))
                    {
                        reader.BaseStream.Position = num4;
                        while (reader.BaseStream.Position != reader.BaseStream.Length)
                        {
                            int num20 = reader.ReadInt32();
                            int num21 = reader.ReadInt32();
                            byte[] buffer5 = this._aqouraeqaisome[num21];
                            byte[] buffer6 = new byte[buffer5.Length];
                            Array.Copy(buffer5, buffer6, buffer6.Length);
                            Array.Copy(BitConverter.GetBytes(num20), buffer6, 4);
                            this._aqouraeqaisome.Add(num20, buffer6);
                        }
                    }
                }
            }
        }

        public int EmuliogenoFogiwever(BinaryReader agawipiDoaj)
        {
            if (agawipiDoaj.BaseStream.Length < 0x30L)
            {
                throw new InvalidDataException(string.Format("File {0} is corrupted!", this.get_FileName()));
            }
            if (agawipiDoaj.ReadUInt32() != 0x35424457)
            {
                throw new InvalidDataException(string.Format("File {0} isn't valid DB2 file!", this.get_FileName()));
            }
            int num = agawipiDoaj.ReadInt32();
            this.set_FieldsCount(agawipiDoaj.ReadInt32());
            this.set_RecordSize(agawipiDoaj.ReadInt32());
            this.set_StringTableSize(agawipiDoaj.ReadInt32());
            this.set_TableHash(agawipiDoaj.ReadUInt32());
            this.set_LayoutHash(agawipiDoaj.ReadUInt32());
            this.set_MinId(agawipiDoaj.ReadInt32());
            this.set_MaxId(agawipiDoaj.ReadInt32());
            this.set_Locale(agawipiDoaj.ReadInt32());
            this.set_CopyTableSize(agawipiDoaj.ReadInt32());
            ushort num2 = agawipiDoaj.ReadUInt16();
            this.set_IdIndex(agawipiDoaj.ReadUInt16());
            this.set_IsSparseTable((num2 & 1) != 0);
            this.set_HasIndexTable((num2 & 4) != 0);
            this._utuavau = new List<Onuseifouveile>();
            for (int i = 0; i < this.get_FieldsCount(); i++)
            {
                Onuseifouveile item = new Onuseifouveile {
                    _kapetUxu = agawipiDoaj.ReadInt16(),
                    _kauvireaCi = (short) (agawipiDoaj.ReadInt16() + (this.get_HasIndexTable() ? 4 : 0))
                };
                this._utuavau.Add(item);
            }
            if (this.get_HasIndexTable())
            {
                this.set_FieldsCount(this.get_FieldsCount() + 1);
                this._utuavau.Insert(0, new Onuseifouveile());
            }
            return num;
        }

        public static T Jejaebiniqeuf<T>(BinaryReader agawipiDoaj)
        {
            GCHandle handle = GCHandle.Alloc(agawipiDoaj.ReadBytes(Marshal.SizeOf(typeof(T))), GCHandleType.Pinned);
            T local = (T) Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();
            return local;
        }

        public void Save(DataTable table, Table def, string path)
        {
            int IDColumn = table.Columns.IndexOf(table.PrimaryKey[0]);
            IEnumerable<DataRow> source = table.Rows.Cast<DataRow>();
            IEnumerable<int> enumerable2 = from r in source select (int) r[IDColumn];
            UruocuoraeXeowuhaxe comparer = new UruocuoraeXeowuhaxe();
            comparer.set_IdColumnIndex(IDColumn);
            DataRow[] rowArray = source.Distinct<DataRow>(comparer).ToArray<DataRow>();
            var typeArray = (from g in source.GroupBy<DataRow, DataRow>(r => r, comparer)
                where g.Count<DataRow>() > 1
                select new { Key = g.Key, Copies = (from r in g
                    where r != g.Key
                    select r).ToArray<DataRow>() }).ToArray();
            int num = enumerable2.Min();
            int num2 = enumerable2.Max();
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                using (MemoryStream stream2 = new MemoryStream())
                {
                    using (BinaryWriter writer = new BinaryWriter(stream2))
                    {
                        writer.Write((uint) 0x35424457);
                        writer.Write(rowArray.Length);
                        writer.Write(this.get_HasIndexTable() ? (this.get_FieldsCount() - 1) : this.get_FieldsCount());
                        writer.Write(this.get_RecordSize());
                        writer.Write(2);
                        writer.Write(this.get_TableHash());
                        writer.Write(this.get_LayoutHash());
                        writer.Write(num);
                        writer.Write(num2);
                        writer.Write(this.get_Locale());
                        writer.Write(0);
                        ushort num3 = 0;
                        if (this.get_HasIndexTable())
                        {
                            num3 = (ushort) (num3 | 4);
                        }
                        writer.Write(num3);
                        writer.Write((ushort) IDColumn);
                        for (int i = 0; i < this._utuavau.Count; i++)
                        {
                            if (!this.get_HasIndexTable() || (i != 0))
                            {
                                writer.Write(this._utuavau[i]._kapetUxu);
                                writer.Write(this.get_HasIndexTable() ? ((short) (this._utuavau[i]._kauvireaCi - 4)) : this._utuavau[i]._kauvireaCi);
                            }
                        }
                        TypeCode[] codeArray = (from c in table.Columns.Cast<DataColumn>() select Type.GetTypeCode(c.DataType)).ToArray<TypeCode>();
                        Dictionary<string, int> dictionary = new Dictionary<string, int>();
                        dictionary[""] = 0;
                        MemoryStream stream3 = new MemoryStream();
                        stream3.WriteByte(0);
                        stream3.WriteByte(0);
                        List<Field> fields = def.Fields;
                        int count = fields.Count;
                        int[] numArray = (from f in fields select f.ArraySize).ToArray<int>();
                        foreach (DataRow row in rowArray)
                        {
                            int index = 0;
                            for (int j = 0; j < count; j++)
                            {
                                if (this.get_HasIndexTable() && (j == 0))
                                {
                                    index++;
                                }
                                else
                                {
                                    int num8 = numArray[j];
                                    for (int k = 0; k < num8; k++)
                                    {
                                        string str;
                                        switch (codeArray[index])
                                        {
                                            case TypeCode.SByte:
                                                writer.Write(row.Field<sbyte>(index));
                                                goto Label_054A;

                                            case TypeCode.Byte:
                                                writer.Write(row.Field<byte>(index));
                                                goto Label_054A;

                                            case TypeCode.Int16:
                                                writer.Write(row.Field<short>(index));
                                                goto Label_054A;

                                            case TypeCode.UInt16:
                                                writer.Write(row.Field<ushort>(index));
                                                goto Label_054A;

                                            case TypeCode.Int32:
                                            {
                                                int num10 = (0x20 - this._utuavau[j]._kapetUxu) >> 3;
                                                byte[] buffer = BitConverter.GetBytes(row.Field<int>(index));
                                                writer.Write(buffer, 0, num10);
                                                goto Label_054A;
                                            }
                                            case TypeCode.UInt32:
                                            {
                                                int num11 = (0x20 - this._utuavau[j]._kapetUxu) >> 3;
                                                byte[] buffer2 = BitConverter.GetBytes(row.Field<uint>(index));
                                                writer.Write(buffer2, 0, num11);
                                                goto Label_054A;
                                            }
                                            case TypeCode.Int64:
                                            {
                                                int num12 = (0x20 - this._utuavau[j]._kapetUxu) >> 3;
                                                byte[] buffer3 = BitConverter.GetBytes(row.Field<long>(index));
                                                writer.Write(buffer3, 0, num12);
                                                goto Label_054A;
                                            }
                                            case TypeCode.UInt64:
                                            {
                                                int num13 = (0x20 - this._utuavau[j]._kapetUxu) >> 3;
                                                byte[] buffer4 = BitConverter.GetBytes(row.Field<ulong>(index));
                                                writer.Write(buffer4, 0, num13);
                                                goto Label_054A;
                                            }
                                            case TypeCode.Single:
                                                writer.Write(row.Field<float>(index));
                                                goto Label_054A;

                                            case TypeCode.Double:
                                                writer.Write(row.Field<double>(index));
                                                goto Label_054A;

                                            case TypeCode.String:
                                                int num14;
                                                str = row.Field<string>(index);
                                                if (!dictionary.TryGetValue(str, out num14))
                                                {
                                                    break;
                                                }
                                                writer.Write(num14);
                                                goto Label_054A;

                                            default:
                                                throw new Exception("Unknown TypeCode " + codeArray[index]);
                                        }
                                        byte[] bytes = Encoding.UTF8.GetBytes(str);
                                        if (bytes.Length == 0)
                                        {
                                            throw new Exception("should not happen");
                                        }
                                        dictionary[str] = (int) stream3.Position;
                                        writer.Write((int) stream3.Position);
                                        stream3.Write(bytes, 0, bytes.Length);
                                        stream3.WriteByte(0);
                                    Label_054A:
                                        index++;
                                    }
                                }
                            }
                            long num15 = stream2.Position % 4L;
                            if (num15 != 0L)
                            {
                                stream2.Position += 4L - num15;
                            }
                        }
                        long position = stream2.Position;
                        stream2.Position = 0x10L;
                        writer.Write((int) stream3.Length);
                        stream2.Position = position;
                        stream3.Position = 0L;
                        stream3.CopyTo(stream2);
                        if (this.get_HasIndexTable())
                        {
                            foreach (DataRow row2 in rowArray)
                            {
                                writer.Write(row2.Field<int>(IDColumn));
                            }
                        }
                        if (typeArray.Length > 0)
                        {
                            int num17 = 0;
                            foreach (var type in typeArray)
                            {
                                int num18 = type.Key.Field<int>(IDColumn);
                                foreach (DataRow row3 in type.Copies)
                                {
                                    writer.Write(row3.Field<int>(IDColumn));
                                    writer.Write(num18);
                                    num17 += 8;
                                }
                            }
                            long num19 = stream2.Position;
                            stream2.Position = 40L;
                            writer.Write(num17);
                            stream2.Position = num19;
                        }
                        stream2.Position = 0L;
                        stream2.CopyTo(stream);
                    }
                }
            }
        }

        public int _agevebofakus
        {
            get
            {
                return this._aqouraeqaisome.Count;
            }
        }

        public int _ajileipeiOmabNalotetes { get; private set; }

        public int _asenogius { get; private set; }

        public Dictionary<int, string> _boacinomaloe { get; private set; }

        public bool _eduveawecao { get; private set; }

        public List<Onuseifouveile> _efitea
        {
            get
            {
                return this._utuavau;
            }
        }

        public uint _heikulPuvicioxe { get; private set; }

        public int _hohuobuhomElaupiowu { get; private set; }

        public int _ininecuesaegunEpue { get; private set; }

        public uint _ocafameisaoveDaviowa { get; private set; }

        public IEnumerable<BinaryReader> _ohiqa
        {
            get
            {
                foreach (KeyValuePair<int, byte[]> iteratorVariable0 in this._aqouraeqaisome)
                {
                    yield return new BinaryReader(new MemoryStream(iteratorVariable0.Value), Encoding.UTF8);
                }
            }
        }

        public string _oleuniJuheuci { get; private set; }

        public int _omoneUjiukoev { get; private set; }

        public int _peawauvahiQujiavi { get; private set; }

        public bool _ridudar { get; private set; }

        public int _tameicuodAsebeloaq { get; private set; }

        public int _waqomeafuafe { get; private set; }

    }
}

