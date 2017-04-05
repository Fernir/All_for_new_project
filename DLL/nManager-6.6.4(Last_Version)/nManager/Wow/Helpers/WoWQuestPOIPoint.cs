namespace nManager.Wow.Helpers
{
    using nManager.Helpful;
    using nManager.Wow.Class;
    using nManager.Wow.Enums;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class WoWQuestPOIPoint
    {
        private bool _caumuogem;
        private Point _hehovelupifOtia = new Point(0f, 0f, 0f, "None");
        private static BinaryReader[] _ogoiqoiIxof;
        private static QuestPOIPointDb2Record[] _qiwukopSiaqio;
        private static AnielOcoihij _qunoireutaevi;
        private Point _roefi = new Point(0f, 0f, 0f, "None");
        private readonly List<Point> _setPoints;

        private WoWQuestPOIPoint(uint setId)
        {
            BamuodaFuhi();
            this._setPoints = new List<Point>();
            bool flag = false;
            QuestPOIPointDb2Record record = new QuestPOIPointDb2Record();
            for (int i = 0; i < (_qunoireutaevi.get_RecordsCount() - 1); i++)
            {
                record = _qiwukopSiaqio[i];
                if (record.SetId == setId)
                {
                    this._setPoints.Add(new Point((float) record.X, (float) record.Y, 0f, "None"));
                    flag = true;
                }
                else if (flag)
                {
                    return;
                }
            }
        }

        [CompilerGenerated]
        private static bool <Init>b__0(Table t)
        {
            return (t.Name == "QuestPOIPoint");
        }

        [CompilerGenerated]
        private static bool <Init>b__1(Table t)
        {
            return (t.Name == Path.GetFileName("QuestPOIPoint"));
        }

        private static void BamuodaFuhi()
        {
            if (_qunoireutaevi == null)
            {
                DBFilesClient client = DBFilesClient.Load(Application.StartupPath + @"\Data\DBFilesClient\dblayout.xml");
                if (CS$<>9__CachedAnonymousMethodDelegate2 == null)
                {
                    CS$<>9__CachedAnonymousMethodDelegate2 = new Func<Table, bool>(WoWQuestPOIPoint.<Init>b__0);
                }
                IEnumerable<Table> source = client.Tables.Where<Table>(CS$<>9__CachedAnonymousMethodDelegate2);
                if (!source.Any<Table>())
                {
                    if (CS$<>9__CachedAnonymousMethodDelegate3 == null)
                    {
                        CS$<>9__CachedAnonymousMethodDelegate3 = new Func<Table, bool>(WoWQuestPOIPoint.<Init>b__1);
                    }
                    source = client.Tables.Where<Table>(CS$<>9__CachedAnonymousMethodDelegate3);
                }
                if (source.Count<Table>() == 1)
                {
                    Table def = source.First<Table>();
                    _qunoireutaevi = DBReaderFactory.GetReader(Application.StartupPath + @"\Data\DBFilesClient\QuestPOIPoint.db2", def) as AnielOcoihij;
                    if ((_ogoiqoiIxof == null) && (_qunoireutaevi != null))
                    {
                        _ogoiqoiIxof = _qunoireutaevi.get_Rows().ToArray<BinaryReader>();
                        _qiwukopSiaqio = new QuestPOIPointDb2Record[_ogoiqoiIxof.Length];
                        for (int i = 0; i < (_ogoiqoiIxof.Length - 1); i++)
                        {
                            _qiwukopSiaqio[i] = AnielOcoihij.Jejaebiniqeuf<QuestPOIPointDb2Record>(_ogoiqoiIxof[i]);
                        }
                    }
                    if (_qunoireutaevi != null)
                    {
                        Logging.Write(string.Concat(new object[] { _qunoireutaevi.get_FileName(), " loaded with ", _qunoireutaevi.get_RecordsCount(), " entries." }));
                    }
                }
                else
                {
                    Logging.Write("DB2 QuestPOIPoint not read-able.");
                }
            }
        }

        public static WoWQuestPOIPoint FromSetId(uint setId)
        {
            return new WoWQuestPOIPoint(setId);
        }

        public bool IsInside(Point p)
        {
            int num2 = this._setPoints.Count - 1;
            int x = (int) p.X;
            int y = (int) p.Y;
            bool flag = false;
            for (int i = 0; i < this._setPoints.Count; i++)
            {
                if ((((this._setPoints[i].Y < y) && (this._setPoints[num2].Y >= y)) || ((this._setPoints[num2].Y < y) && (this._setPoints[i].Y >= y))) && (((this._setPoints[i].X <= x) || (this._setPoints[num2].X <= x)) && ((this._setPoints[i].X + (((y - this._setPoints[i].Y) / (this._setPoints[num2].Y - this._setPoints[i].Y)) * (this._setPoints[num2].X - this._setPoints[i].X))) < x)))
                {
                    flag = !flag;
                }
                num2 = i;
            }
            return flag;
        }

        public Point Center
        {
            get
            {
                if (!this._roefi.IsValid)
                {
                    if (this._setPoints.Count == 0)
                    {
                        return new Point(0f, 0f, 0f, "invalid");
                    }
                    int num = 0;
                    int num2 = 0;
                    for (int i = 0; i < this._setPoints.Count; i++)
                    {
                        num += (int) this._setPoints[i].X;
                        num2 += (int) this._setPoints[i].Y;
                    }
                    float x = num / this._setPoints.Count;
                    float y = num2 / this._setPoints.Count;
                    float z = PathFinder.GetZPosition(x, y, false);
                    while ((z == 0f) && this.IsInside(new Point(x + 5f, y + 5f, 0f, "None")))
                    {
                        x += 5f;
                        y += 5f;
                        z = PathFinder.GetZPosition(x, y, true);
                    }
                    float num7 = PathFinder.GetZPosition(x, y, z + 100f, false);
                    if ((num7 - z) > 5f)
                    {
                        z = num7;
                    }
                    this._roefi = new Point(x, y, z, "None");
                }
                return this._roefi;
            }
        }

        public Point MiddlePoint
        {
            get
            {
                if (this._hehovelupifOtia.IsValid)
                {
                    return this._hehovelupifOtia;
                }
                this._hehovelupifOtia = new Point(this.Center);
                this._hehovelupifOtia.Z += 40f;
                float z = PathFinder.GetZPosition(this._hehovelupifOtia.X, this._hehovelupifOtia.Y, this._hehovelupifOtia.Z, true);
                float num2 = PathFinder.GetZPosition(this._hehovelupifOtia.X + 8f, this._hehovelupifOtia.Y + 8f, this._hehovelupifOtia.Z, true);
                if ((this.IsInside(this._hehovelupifOtia) && (z != 0f)) && ((((uint) (num2 - z)) < 11) && !TraceLine.TraceLineGo(new Point(this._hehovelupifOtia.X, this._hehovelupifOtia.Y, z, "None"), new Point(this._hehovelupifOtia.X, this._hehovelupifOtia.Y, z + 50f, "None"), CGWorldFrameHitFlags.HitTestAll)))
                {
                    this._hehovelupifOtia = new Point(this._hehovelupifOtia.X, this._hehovelupifOtia.Y, z + 5f, "None");
                    goto Label_0619;
                }
                bool flag = false;
                int num3 = 0;
            Label_0111:
                num3 += 5;
                if (this.IsInside(new Point(this._hehovelupifOtia.X + num3, this._hehovelupifOtia.Y, 0f, "None")))
                {
                    z = PathFinder.GetZPosition(this._hehovelupifOtia.X + num3, this._hehovelupifOtia.Y, this._hehovelupifOtia.Z, true);
                    num2 = PathFinder.GetZPosition((this._hehovelupifOtia.X + num3) + 8f, this._hehovelupifOtia.Y, this._hehovelupifOtia.Z, true);
                    if (((z != 0f) && (((uint) (num2 - z)) < 8)) && !TraceLine.TraceLineGo(new Point(this._hehovelupifOtia.X + num3, this._hehovelupifOtia.Y, z, "None"), new Point(this._hehovelupifOtia.X + num3, this._hehovelupifOtia.Y, z + 50f, "None"), CGWorldFrameHitFlags.HitTestAll))
                    {
                        this._hehovelupifOtia = new Point(this._hehovelupifOtia.X + num3, this._hehovelupifOtia.Y, z + 5f, "None");
                        flag = true;
                        goto Label_05E4;
                    }
                }
                if (this.IsInside(new Point(this._hehovelupifOtia.X - num3, this._hehovelupifOtia.Y, 0f, "None")))
                {
                    z = PathFinder.GetZPosition(this._hehovelupifOtia.X - num3, this._hehovelupifOtia.Y, this._hehovelupifOtia.Z, true);
                    num2 = PathFinder.GetZPosition((this._hehovelupifOtia.X - num3) - 8f, this._hehovelupifOtia.Y, this._hehovelupifOtia.Z, true);
                    if (((z != 0f) && (((uint) (num2 - z)) < 8)) && !TraceLine.TraceLineGo(new Point(this._hehovelupifOtia.X - num3, this._hehovelupifOtia.Y, z, "None"), new Point(this._hehovelupifOtia.X - num3, this._hehovelupifOtia.Y, z + 50f, "None"), CGWorldFrameHitFlags.HitTestAll))
                    {
                        this._hehovelupifOtia = new Point(this._hehovelupifOtia.X - num3, this._hehovelupifOtia.Y, z + 5f, "None");
                        flag = true;
                        goto Label_05E4;
                    }
                }
                if (this.IsInside(new Point(this._hehovelupifOtia.X, this._hehovelupifOtia.Y + num3, 0f, "None")))
                {
                    z = PathFinder.GetZPosition(this._hehovelupifOtia.X, this._hehovelupifOtia.Y + num3, this._hehovelupifOtia.Z, true);
                    num2 = PathFinder.GetZPosition(this._hehovelupifOtia.X, (this._hehovelupifOtia.Y + num3) + 8f, this._hehovelupifOtia.Z, true);
                    if (((z != 0f) && (((uint) (num2 - z)) < 8)) && !TraceLine.TraceLineGo(new Point(this._hehovelupifOtia.X, this._hehovelupifOtia.Y + num3, z, "None"), new Point(this._hehovelupifOtia.X, this._hehovelupifOtia.Y + num3, z + 50f, "None"), CGWorldFrameHitFlags.HitTestAll))
                    {
                        this._hehovelupifOtia = new Point(this._hehovelupifOtia.X, this._hehovelupifOtia.Y + num3, z + 5f, "None");
                        flag = true;
                        goto Label_05E4;
                    }
                }
                if (this.IsInside(new Point(this._hehovelupifOtia.X, this._hehovelupifOtia.Y - num3, 0f, "None")))
                {
                    z = PathFinder.GetZPosition(this._hehovelupifOtia.X, this._hehovelupifOtia.Y - num3, this._hehovelupifOtia.Z, true);
                    num2 = PathFinder.GetZPosition(this._hehovelupifOtia.X, (this._hehovelupifOtia.Y - num3) - 8f, this._hehovelupifOtia.Z, true);
                    if (((z != 0f) && (((uint) (num2 - z)) < 8)) && !TraceLine.TraceLineGo(new Point(this._hehovelupifOtia.X, this._hehovelupifOtia.Y - num3, z, "None"), new Point(this._hehovelupifOtia.X, this._hehovelupifOtia.Y - num3, z + 50f, "None"), CGWorldFrameHitFlags.HitTestAll))
                    {
                        this._hehovelupifOtia = new Point(this._hehovelupifOtia.X, this._hehovelupifOtia.Y - num3, z + 5f, "None");
                        flag = true;
                    }
                }
            Label_05E4:
                if (!flag)
                {
                    goto Label_0111;
                }
            Label_0619:
                this._caumuogem = true;
                return this._hehovelupifOtia;
            }
        }

        public List<Point> PointList
        {
            get
            {
                return this._setPoints;
            }
        }

        public bool ValidPoint
        {
            get
            {
                return this._caumuogem;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct QuestPOIPointDb2Record
        {
            public readonly uint SetId;
            public readonly short X;
            public readonly short Y;
            public readonly uint Id;
        }
    }
}

