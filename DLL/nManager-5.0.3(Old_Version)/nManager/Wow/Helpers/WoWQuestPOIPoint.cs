namespace nManager.Wow.Helpers
{
    using nManager.Wow.Class;
    using nManager.Wow.Enums;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public class WoWQuestPOIPoint
    {
        private Point _center = new Point(0f, 0f, 0f, "None");
        private Point _middlePoint = new Point(0f, 0f, 0f, "None");
        private bool _middlePointSet;
        private static DB2<QuestPOIPointDb2Record> _qpPointDB2;
        private readonly List<Point> _setPoints = new List<Point>();

        private WoWQuestPOIPoint(uint setId)
        {
            if (_qpPointDB2 == null)
            {
                _qpPointDB2 = new DB2<QuestPOIPointDb2Record>(0xc6d890);
            }
            bool flag = false;
            for (int i = _qpPointDB2.MinIndex; i <= _qpPointDB2.MaxIndex; i++)
            {
                QuestPOIPointDb2Record row = _qpPointDB2.GetRow(i);
                if (row.Id == i)
                {
                    if (row.SetId == setId)
                    {
                        this._setPoints.Add(new Point((float) row.X, (float) row.Y, 0f, "None"));
                        flag = true;
                    }
                    else if (flag)
                    {
                        return;
                    }
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
                if (!this._center.IsValid)
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
                    this._center = new Point(x, y, z, "None");
                }
                return this._center;
            }
        }

        public Point MiddlePoint
        {
            get
            {
                if (this._middlePoint.IsValid)
                {
                    return this._middlePoint;
                }
                this._middlePoint = new Point(this.Center);
                this._middlePoint.Z += 40f;
                float z = PathFinder.GetZPosition(this._middlePoint.X, this._middlePoint.Y, this._middlePoint.Z, true);
                float num2 = PathFinder.GetZPosition(this._middlePoint.X + 8f, this._middlePoint.Y + 8f, this._middlePoint.Z, true);
                if ((this.IsInside(this._middlePoint) && (z != 0f)) && ((((uint) (num2 - z)) < 11) && !TraceLine.TraceLineGo(new Point(this._middlePoint.X, this._middlePoint.Y, z, "None"), new Point(this._middlePoint.X, this._middlePoint.Y, z + 50f, "None"), CGWorldFrameHitFlags.HitTestAll)))
                {
                    this._middlePoint = new Point(this._middlePoint.X, this._middlePoint.Y, z + 5f, "None");
                    goto Label_0619;
                }
                bool flag = false;
                int num3 = 0;
            Label_0111:
                num3 += 5;
                if (this.IsInside(new Point(this._middlePoint.X + num3, this._middlePoint.Y, 0f, "None")))
                {
                    z = PathFinder.GetZPosition(this._middlePoint.X + num3, this._middlePoint.Y, this._middlePoint.Z, true);
                    num2 = PathFinder.GetZPosition((this._middlePoint.X + num3) + 8f, this._middlePoint.Y, this._middlePoint.Z, true);
                    if (((z != 0f) && (((uint) (num2 - z)) < 8)) && !TraceLine.TraceLineGo(new Point(this._middlePoint.X + num3, this._middlePoint.Y, z, "None"), new Point(this._middlePoint.X + num3, this._middlePoint.Y, z + 50f, "None"), CGWorldFrameHitFlags.HitTestAll))
                    {
                        this._middlePoint = new Point(this._middlePoint.X + num3, this._middlePoint.Y, z + 5f, "None");
                        flag = true;
                        goto Label_05E4;
                    }
                }
                if (this.IsInside(new Point(this._middlePoint.X - num3, this._middlePoint.Y, 0f, "None")))
                {
                    z = PathFinder.GetZPosition(this._middlePoint.X - num3, this._middlePoint.Y, this._middlePoint.Z, true);
                    num2 = PathFinder.GetZPosition((this._middlePoint.X - num3) - 8f, this._middlePoint.Y, this._middlePoint.Z, true);
                    if (((z != 0f) && (((uint) (num2 - z)) < 8)) && !TraceLine.TraceLineGo(new Point(this._middlePoint.X - num3, this._middlePoint.Y, z, "None"), new Point(this._middlePoint.X - num3, this._middlePoint.Y, z + 50f, "None"), CGWorldFrameHitFlags.HitTestAll))
                    {
                        this._middlePoint = new Point(this._middlePoint.X - num3, this._middlePoint.Y, z + 5f, "None");
                        flag = true;
                        goto Label_05E4;
                    }
                }
                if (this.IsInside(new Point(this._middlePoint.X, this._middlePoint.Y + num3, 0f, "None")))
                {
                    z = PathFinder.GetZPosition(this._middlePoint.X, this._middlePoint.Y + num3, this._middlePoint.Z, true);
                    num2 = PathFinder.GetZPosition(this._middlePoint.X, (this._middlePoint.Y + num3) + 8f, this._middlePoint.Z, true);
                    if (((z != 0f) && (((uint) (num2 - z)) < 8)) && !TraceLine.TraceLineGo(new Point(this._middlePoint.X, this._middlePoint.Y + num3, z, "None"), new Point(this._middlePoint.X, this._middlePoint.Y + num3, z + 50f, "None"), CGWorldFrameHitFlags.HitTestAll))
                    {
                        this._middlePoint = new Point(this._middlePoint.X, this._middlePoint.Y + num3, z + 5f, "None");
                        flag = true;
                        goto Label_05E4;
                    }
                }
                if (this.IsInside(new Point(this._middlePoint.X, this._middlePoint.Y - num3, 0f, "None")))
                {
                    z = PathFinder.GetZPosition(this._middlePoint.X, this._middlePoint.Y - num3, this._middlePoint.Z, true);
                    num2 = PathFinder.GetZPosition(this._middlePoint.X, (this._middlePoint.Y - num3) - 8f, this._middlePoint.Z, true);
                    if (((z != 0f) && (((uint) (num2 - z)) < 8)) && !TraceLine.TraceLineGo(new Point(this._middlePoint.X, this._middlePoint.Y - num3, z, "None"), new Point(this._middlePoint.X, this._middlePoint.Y - num3, z + 50f, "None"), CGWorldFrameHitFlags.HitTestAll))
                    {
                        this._middlePoint = new Point(this._middlePoint.X, this._middlePoint.Y - num3, z + 5f, "None");
                        flag = true;
                    }
                }
            Label_05E4:
                if (!flag)
                {
                    goto Label_0111;
                }
            Label_0619:
                this._middlePointSet = true;
                return this._middlePoint;
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
                return this._middlePointSet;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct QuestPOIPointDb2Record
        {
            public readonly uint Id;
            public readonly int X;
            public readonly int Y;
            public readonly uint SetId;
        }
    }
}

