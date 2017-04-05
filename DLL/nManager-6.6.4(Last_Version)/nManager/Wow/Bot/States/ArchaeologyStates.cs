namespace nManager.Wow.Bot.States
{
    using nManager;
    using nManager.FiniteStateMachine;
    using nManager.Helpful;
    using nManager.Products;
    using nManager.Wow.Bot.Tasks;
    using nManager.Wow.Class;
    using nManager.Wow.Enums;
    using nManager.Wow.Helpers;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class ArchaeologyStates : nManager.FiniteStateMachine.State
    {
        private Spell _ajunoDecei;
        private Digsite _atipohokeIj = new Digsite();
        private int _cainoiqa;
        private Point _coamegeonei = new Point();
        private bool _ecocuhuawaujov;
        private bool _evepidoubaGeigicou;
        private Point _iqodounotUhaigieca;
        private int _iqotaubiefu;
        private int _japouneipusie;
        private UkaoheuboebaudEvubasejoSoukeojai _kidivun = UkaoheuboebaudEvubasejoSoukeojai._isiaraohivFumeUmuol;
        private int _mirujuaneajonoEnNauje;
        private int _puoxaukaidionKobaop;
        private bool _rugaudiRueqopauModae;
        private WoWQuestPOIPoint _suamauf;
        private nManager.Helpful.Timer _vuvoreane;
        private nManager.Helpful.Timer _xagovatBoUhav;
        private readonly List<int> BlackListDigsites = new List<int>();
        public bool CrateRestored = true;
        public int MaxTryByDigsite = 30;
        public int SolvingEveryXMin = 20;
        public bool UseKeystones = true;

        private bool AtaelorausuepFaeqeju(Point ipeqev)
        {
            return !TraceLine.TraceLineGo(new Point(ipeqev.X, ipeqev.Y, ipeqev.Z + 1000f, "None"), ipeqev, CGWorldFrameHitFlags.HitTestLiquid);
        }

        public override void Run()
        {
            try
            {
                if (!MovementManager.InMovement)
                {
                    if (this._iqotaubiefu != this._atipohokeIj.id)
                    {
                        this._cainoiqa = 0;
                    }
                    this._iqotaubiefu = this._atipohokeIj.id;
                    if (this._vuvoreane == null)
                    {
                        this._vuvoreane = new nManager.Helpful.Timer((double) ((this.SolvingEveryXMin * 0x3e8) * 60));
                    }
                    if ((this._vuvoreane.IsReady && !nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe) && !nManager.Wow.ObjectManager.ObjectManager.Me.InCombat)
                    {
                        MovementManager.StopMove();
                        LongMove.StopLongMove();
                        if ((Archaeology.SolveAllArtifact(this.UseKeystones) > 0) && this.CrateRestored)
                        {
                            Archaeology.CrateRestoredArtifact();
                        }
                        this._vuvoreane = new nManager.Helpful.Timer((double) ((this.SolvingEveryXMin * 0x3e8) * 60));
                    }
                    if (!MovementManager.InMovement)
                    {
                        int num = 0;
                        try
                        {
                            if (this._kidivun != UkaoheuboebaudEvubasejoSoukeojai._isiaraohivFumeUmuol)
                            {
                                MountTask.DismountMount(true);
                            }
                            WoWGameObject nearestWoWGameObject = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWGameObject(nManager.Wow.ObjectManager.ObjectManager.GetWoWGameObjectByEntry(Archaeology.ArchaeologyItemsFindList), false);
                            if (nearestWoWGameObject.IsValid)
                            {
                                bool flag;
                                this._puoxaukaidionKobaop = 0;
                                this._coamegeonei = new Point();
                                this._rugaudiRueqopauModae = false;
                                this._japouneipusie = 0;
                                if (this._kidivun == UkaoheuboebaudEvubasejoSoukeojai._xibucLiaji)
                                {
                                    if ((this._xagovatBoUhav != null) && this._xagovatBoUhav.IsReady)
                                    {
                                        MovementsAction.Jump();
                                        Thread.Sleep(0x5dc);
                                    }
                                    else
                                    {
                                        return;
                                    }
                                }
                                List<Point> points = PathFinder.FindPath(nearestWoWGameObject.Position, out flag, false, false);
                                if (!flag)
                                {
                                    points.Add(nearestWoWGameObject.Position);
                                }
                                MovementManager.Go(points);
                                if (this._mirujuaneajonoEnNauje > 2)
                                {
                                    MovementManager.StopMove();
                                    LongMove.StopLongMove();
                                    if (Archaeology.SolveAllArtifact(this.UseKeystones) == 0)
                                    {
                                        nManagerSetting.AddBlackList(nearestWoWGameObject.Guid, -1);
                                        Logging.Write("Black-listing bugged artifact");
                                    }
                                    else if (this.CrateRestored)
                                    {
                                        Archaeology.CrateRestoredArtifact();
                                    }
                                    this._mirujuaneajonoEnNauje = 0;
                                }
                                else
                                {
                                    Logging.Write("Loot " + nearestWoWGameObject.Name);
                                    nManager.Helpful.Timer timer = new nManager.Helpful.Timer((double) ((1000f * nManager.Helpful.Math.DistanceListPoint(points)) / 3f));
                                    while ((MovementManager.InMovement && !timer.IsReady) && (nearestWoWGameObject.GetDistance > 3.5f))
                                    {
                                        Thread.Sleep(100);
                                        if (nManager.Wow.ObjectManager.ObjectManager.Me.InInevitableCombat)
                                        {
                                            return;
                                        }
                                    }
                                    MovementManager.StopMove();
                                    Thread.Sleep(150);
                                    Interact.InteractWith(nearestWoWGameObject.GetBaseAddress, false);
                                    while (nManager.Wow.ObjectManager.ObjectManager.Me.IsCast)
                                    {
                                        Thread.Sleep(100);
                                    }
                                    if (!nManager.Wow.ObjectManager.ObjectManager.Me.InCombat)
                                    {
                                        Statistics.Farms++;
                                        this._mirujuaneajonoEnNauje++;
                                        this._kidivun = UkaoheuboebaudEvubasejoSoukeojai._xibucLiaji;
                                        if (this._xagovatBoUhav == null)
                                        {
                                            this._xagovatBoUhav = new nManager.Helpful.Timer(5000.0);
                                        }
                                        else
                                        {
                                            this._xagovatBoUhav.Reset();
                                        }
                                    }
                                }
                            }
                            else if (this._cainoiqa > this.MaxTryByDigsite)
                            {
                                this._mirujuaneajonoEnNauje = 0;
                                this.BlackListDigsites.Add(this._atipohokeIj.id);
                                Logging.Write("Black List Digsite: " + this._atipohokeIj.name);
                                this._kidivun = UkaoheuboebaudEvubasejoSoukeojai._isiaraohivFumeUmuol;
                                this._puoxaukaidionKobaop = 0;
                            }
                            else
                            {
                                bool flag2 = false;
                                if (((this._suamauf != null) && !this._suamauf.ValidPoint) && (this._suamauf.Center.DistanceTo2D(nManager.Wow.ObjectManager.ObjectManager.Me.Position) < 40f))
                                {
                                    Point middlePoint = this._suamauf.MiddlePoint;
                                    if (Usefuls.IsFlying)
                                    {
                                        flag2 = true;
                                        MovementManager.StopMove();
                                    }
                                }
                                if ((this._suamauf != null) && (flag2 || !this._suamauf.IsInside(nManager.Wow.ObjectManager.ObjectManager.Me.Position)))
                                {
                                    if ((MountTask.GetMountCapacity() == MountCapacity.Feet) || (MountTask.GetMountCapacity() == MountCapacity.Ground))
                                    {
                                        int entry = 0x1ca8d;
                                        if ((!this._ecocuhuawaujov && (ItemsManager.GetItemCount(entry) > 0)) && ((Usefuls.RealContinentId == 0x45c) && (this._suamauf.Center.DistanceTo2D(nManager.Wow.ObjectManager.ObjectManager.Me.Position) > 3000f)))
                                        {
                                            WoWItem woWItemById = nManager.Wow.ObjectManager.ObjectManager.GetWoWItemById(entry);
                                            if (((woWItemById != null) && woWItemById.IsValid) && (!ItemsManager.IsItemOnCooldown(entry) && ItemsManager.IsItemUsable(entry)))
                                            {
                                                Logging.Write("Using a Draenor Archaeologist's Lodestone");
                                                nManager.Products.Products.InManualPause = true;
                                                ItemsManager.UseItem(entry);
                                                Thread.Sleep((int) (0xbb8 + Usefuls.Latency));
                                                this._atipohokeIj = new Digsite();
                                                return;
                                            }
                                        }
                                        Logging.Write("Not inside, then go to Digsite " + this._atipohokeIj.name);
                                        Point position = nManager.Wow.ObjectManager.ObjectManager.Me.Position;
                                        if (((this._iqodounotUhaigieca == null) || (this._iqodounotUhaigieca.DistanceTo(position) > 0.1f)) && (!this._ecocuhuawaujov && !Usefuls.IsFlying))
                                        {
                                            MovementManager.StopMove();
                                            Logging.Write(string.Concat(new object[] { "Calling travel system to go to digsite ", this._atipohokeIj.name, " (", this._atipohokeIj.id, ")..." }));
                                            nManager.Products.Products.TravelToContinentId = Usefuls.ContinentId;
                                            nManager.Products.Products.TravelTo = this._suamauf.Center;
                                            nManager.Products.Products.TravelFromContinentId = Usefuls.ContinentId;
                                            nManager.Products.Products.TravelFrom = nManager.Wow.ObjectManager.ObjectManager.Me.Position;
                                            nManager.Products.Products.TargetValidationFct = new Func<Point, bool>(this._suamauf.IsInside);
                                            this._iqodounotUhaigieca = position;
                                            return;
                                        }
                                        if (this._iqodounotUhaigieca.DistanceTo(position) <= 0.1f)
                                        {
                                            this._ecocuhuawaujov = true;
                                        }
                                        MovementManager.Go(PathFinder.FindPath(this._suamauf.Center));
                                    }
                                    else if (this._suamauf.ValidPoint)
                                    {
                                        if (flag2 || !this._suamauf.IsInside(nManager.Wow.ObjectManager.ObjectManager.Me.Position))
                                        {
                                            Point point2 = new Point(this._suamauf.MiddlePoint) {
                                                Type = "flying"
                                            };
                                            if (flag2)
                                            {
                                                Logging.Write("Landing on the digsite");
                                            }
                                            else
                                            {
                                                Logging.Write(string.Concat(new object[] { "Not inside, then go to Digsite ", this._atipohokeIj.name, "; X: ", point2.X, "; Y: ", point2.Y, "; Z: ", (int) point2.Z }));
                                            }
                                            MovementManager.Go(new List<Point>(new Point[] { point2 }));
                                        }
                                    }
                                    else
                                    {
                                        Point center = this._suamauf.Center;
                                        center.Z += 40f;
                                        center.Type = "flying";
                                        Logging.Write(string.Concat(new object[] { "Go to Digsite ", this._atipohokeIj.name, "; X: ", center.X, "; Y: ", center.Y, "; Z: ", (int) center.Z }));
                                        MovementManager.Go(new List<Point>(new Point[] { center }));
                                    }
                                    this._kidivun = UkaoheuboebaudEvubasejoSoukeojai._isiaraohivFumeUmuol;
                                }
                                else
                                {
                                    this._mirujuaneajonoEnNauje = 0;
                                    nearestWoWGameObject = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWGameObject(nManager.Wow.ObjectManager.ObjectManager.GetWoWGameObjectByDisplayId(Archaeology.SurveyList), false);
                                    if (((nearestWoWGameObject.GetBaseAddress == 0) || (this._kidivun == UkaoheuboebaudEvubasejoSoukeojai._uloituasuCeudiam)) || (this._kidivun == UkaoheuboebaudEvubasejoSoukeojai._xibucLiaji))
                                    {
                                        if (Archaeology.DigsiteZoneIsAvailable(this._atipohokeIj))
                                        {
                                            if (this._kidivun == UkaoheuboebaudEvubasejoSoukeojai._isiaraohivFumeUmuol)
                                            {
                                                MountTask.DismountMount(true);
                                            }
                                            this._ajunoDecei.Launch();
                                            this._kidivun = UkaoheuboebaudEvubasejoSoukeojai._uremiosia;
                                            if (!nManager.Wow.ObjectManager.ObjectManager.Me.InCombat)
                                            {
                                                Thread.Sleep((int) (0x6d6 + Usefuls.Latency));
                                                this._puoxaukaidionKobaop++;
                                                if (this._puoxaukaidionKobaop > 3)
                                                {
                                                    if (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo2D(this._suamauf.MiddlePoint) < 5f)
                                                    {
                                                        List<Digsite> digsitesZoneAvailable;
                                                        WoWResearchSite site;
                                                        if (!this._evepidoubaGeigicou)
                                                        {
                                                            digsitesZoneAvailable = Archaeology.GetDigsitesZoneAvailable(nearestWoWGameObject.Name);
                                                        }
                                                        else
                                                        {
                                                            digsitesZoneAvailable = Archaeology.GetDigsitesZoneAvailable(null);
                                                        }
                                                        foreach (Digsite digsite in digsitesZoneAvailable)
                                                        {
                                                            if (digsite.name == nearestWoWGameObject.Name)
                                                            {
                                                                this._atipohokeIj = digsite;
                                                            }
                                                        }
                                                        if (!this._evepidoubaGeigicou)
                                                        {
                                                            site = WoWResearchSite.FromName(this._atipohokeIj.name, true);
                                                        }
                                                        else
                                                        {
                                                            site = WoWResearchSite.FromName(this._atipohokeIj.name, false);
                                                        }
                                                        this._suamauf = WoWQuestPOIPoint.FromSetId(site.Record.QuestIdPoint);
                                                        this._evepidoubaGeigicou = !this._evepidoubaGeigicou;
                                                        this._puoxaukaidionKobaop = 0;
                                                    }
                                                    else
                                                    {
                                                        if ((MountTask.GetMountCapacity() == MountCapacity.Feet) || (MountTask.GetMountCapacity() == MountCapacity.Ground))
                                                        {
                                                            Logging.Write("Too many errors, then go to Digsite " + this._atipohokeIj.name);
                                                            MovementManager.Go(PathFinder.FindPath(this._suamauf.Center));
                                                        }
                                                        else if (this._suamauf != null)
                                                        {
                                                            Point point4 = this._suamauf.MiddlePoint;
                                                            Logging.Write(string.Concat(new object[] { "Too many errors, then go to Digsite ", this._atipohokeIj.name, "; X: ", point4.X, "; Y: ", point4.Y, "; Z: ", (int) point4.Z }));
                                                            MovementManager.Go(new List<Point>(new Point[] { point4 }));
                                                        }
                                                        this._puoxaukaidionKobaop = 0;
                                                    }
                                                }
                                                else
                                                {
                                                    this._cainoiqa++;
                                                }
                                            }
                                        }
                                    }
                                    else if (this._kidivun != UkaoheuboebaudEvubasejoSoukeojai._uloituasuCeudiam)
                                    {
                                        this._puoxaukaidionKobaop = 0;
                                        if (!nManager.Wow.ObjectManager.ObjectManager.Me.InCombat || (nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted && (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying)))
                                        {
                                            float num4;
                                            float num5;
                                            float num6;
                                            float num7;
                                            float num8;
                                            bool flag3;
                                            if (nearestWoWGameObject.DisplayId == 0x2777)
                                            {
                                                num4 = 90f;
                                                num5 = 20f;
                                                num6 = 210f;
                                                num8 = 8f;
                                                num7 = 8f;
                                                this._coamegeonei = new Point();
                                                this._rugaudiRueqopauModae = false;
                                                this._japouneipusie = 0;
                                            }
                                            else if (nearestWoWGameObject.DisplayId == 0x2776)
                                            {
                                                num4 = 46f;
                                                num5 = 20f;
                                                num6 = 60f;
                                                num8 = 7f;
                                                num7 = 6.5f;
                                                this._coamegeonei = new Point();
                                                this._rugaudiRueqopauModae = false;
                                                this._japouneipusie = 0;
                                            }
                                            else
                                            {
                                                this._japouneipusie++;
                                                num8 = 4f;
                                                if (this._japouneipusie >= 10)
                                                {
                                                    this._japouneipusie = 0;
                                                    this._coamegeonei = new Point();
                                                    Point point7 = this._suamauf.MiddlePoint;
                                                    this._rugaudiRueqopauModae = false;
                                                    Logging.Write(string.Concat(new object[] { "Stuck, then go to Digsite ", this._atipohokeIj.name, "; X: ", point7.X, "; Y: ", point7.Y, "; Z: ", (int) point7.Z }));
                                                    MountTask.Mount(true, true);
                                                    MovementManager.Go(new List<Point>(new Point[] { point7 }));
                                                    return;
                                                }
                                                if (this._rugaudiRueqopauModae)
                                                {
                                                    Logging.Write("Ping-pong detected, shortening the distance");
                                                    num4 = 11f;
                                                    num5 = 6f;
                                                    num6 = 16f;
                                                    num7 = 4f;
                                                }
                                                else
                                                {
                                                    num4 = 19f;
                                                    num5 = 7f;
                                                    num6 = 41f;
                                                    num7 = 3f;
                                                }
                                            }
                                            float distance = num4;
                                            Point a = new Point(nearestWoWGameObject.Position);
                                            float orientation = nearestWoWGameObject.Orientation;
                                            Point point = nManager.Helpful.Math.GetPosition2DOfAngleAndDistance(a, orientation, distance);
                                            point.Z = PathFinder.GetZPosition(point, true);
                                            PathFinder.FindPath(point, out flag3, true, false);
                                            if (this._suamauf != null)
                                            {
                                                bool flag4 = this.AtaelorausuepFaeqeju(nManager.Wow.ObjectManager.ObjectManager.Me.Position);
                                                while (((!flag3 || (point.Z == 0f)) || !this._suamauf.IsInside(point)) || (flag4 && !this.AtaelorausuepFaeqeju(point)))
                                                {
                                                    if ((distance + num8) > num6)
                                                    {
                                                        break;
                                                    }
                                                    distance += num8;
                                                    Point p = nManager.Helpful.Math.GetPosition2DOfAngleAndDistance(a, orientation, distance);
                                                    if (this._suamauf.IsInside(p))
                                                    {
                                                        point = new Point(p) {
                                                            Z = point.Z + (distance / 10f),
                                                            Z = PathFinder.GetZPosition(point, true)
                                                        };
                                                        if (point.Z == 0f)
                                                        {
                                                            flag3 = false;
                                                        }
                                                        else if ((nManager.Helpful.Math.DistanceListPoint(PathFinder.FindLocalPath(point, out flag3, true)) > (distance * 4f)) && (distance > 30f))
                                                        {
                                                            flag3 = false;
                                                        }
                                                    }
                                                    if (!flag3)
                                                    {
                                                        float angle = nManager.Helpful.Math.FixAngle(orientation + 0.3141593f);
                                                        point = new Point(nManager.Helpful.Math.GetPosition2DOfAngleAndDistance(a, angle, distance)) {
                                                            Z = point.Z + (distance / 10f),
                                                            Z = PathFinder.GetZPosition(point, true)
                                                        };
                                                        if (point.Z == 0f)
                                                        {
                                                            flag3 = false;
                                                        }
                                                        else if ((nManager.Helpful.Math.DistanceListPoint(PathFinder.FindLocalPath(point, out flag3, true)) > (distance * 4f)) && (distance > 30f))
                                                        {
                                                            flag3 = false;
                                                        }
                                                        if (flag3)
                                                        {
                                                            Logging.Write("Angles+ for distance " + distance);
                                                        }
                                                    }
                                                    if (!flag3)
                                                    {
                                                        float num11 = nManager.Helpful.Math.FixAngle(orientation - 0.3141593f);
                                                        point = new Point(nManager.Helpful.Math.GetPosition2DOfAngleAndDistance(a, num11, distance)) {
                                                            Z = point.Z + (distance / 10f),
                                                            Z = PathFinder.GetZPosition(point, true)
                                                        };
                                                        if (point.Z == 0f)
                                                        {
                                                            flag3 = false;
                                                        }
                                                        else if ((nManager.Helpful.Math.DistanceListPoint(PathFinder.FindLocalPath(point, out flag3, true)) > (distance * 4f)) && (distance > 30f))
                                                        {
                                                            flag3 = false;
                                                        }
                                                        if (flag3)
                                                        {
                                                            Logging.Write("Angles- for distance " + distance);
                                                        }
                                                    }
                                                }
                                                if ((!flag3 || (point.Z == 0f)) || (!this._suamauf.IsInside(point) || (flag4 && !this.AtaelorausuepFaeqeju(point))))
                                                {
                                                    distance = num4;
                                                    while (((!flag3 || (point.Z == 0f)) || !this._suamauf.IsInside(point)) || (flag4 && !this.AtaelorausuepFaeqeju(point)))
                                                    {
                                                        if ((distance - num7) < num5)
                                                        {
                                                            break;
                                                        }
                                                        distance -= num7;
                                                        Point point9 = nManager.Helpful.Math.GetPosition2DOfAngleAndDistance(a, orientation, distance);
                                                        if (this._suamauf.IsInside(point9))
                                                        {
                                                            point = new Point(point9) {
                                                                Z = point.Z + (distance / 10f),
                                                                Z = PathFinder.GetZPosition(point, true)
                                                            };
                                                            if (point.Z == 0f)
                                                            {
                                                                flag3 = false;
                                                            }
                                                            else if ((nManager.Helpful.Math.DistanceListPoint(PathFinder.FindLocalPath(point, out flag3, true)) > (distance * 4f)) && (distance > 30f))
                                                            {
                                                                flag3 = false;
                                                            }
                                                        }
                                                        if (!flag3)
                                                        {
                                                            float num12 = nManager.Helpful.Math.FixAngle(orientation + 0.3141593f);
                                                            point = new Point(nManager.Helpful.Math.GetPosition2DOfAngleAndDistance(a, num12, distance)) {
                                                                Z = point.Z + (distance / 10f),
                                                                Z = PathFinder.GetZPosition(point, true)
                                                            };
                                                            if (point.Z == 0f)
                                                            {
                                                                flag3 = false;
                                                            }
                                                            else if ((nManager.Helpful.Math.DistanceListPoint(PathFinder.FindLocalPath(point, out flag3, true)) > (distance * 4f)) && (distance > 30f))
                                                            {
                                                                flag3 = false;
                                                            }
                                                            if (flag3)
                                                            {
                                                                Logging.Write("Angles+ for distance " + distance);
                                                            }
                                                        }
                                                        if (!flag3)
                                                        {
                                                            float num13 = nManager.Helpful.Math.FixAngle(orientation - 0.3141593f);
                                                            point = new Point(nManager.Helpful.Math.GetPosition2DOfAngleAndDistance(a, num13, distance)) {
                                                                Z = point.Z + (distance / 10f),
                                                                Z = PathFinder.GetZPosition(point, true)
                                                            };
                                                            if (point.Z == 0f)
                                                            {
                                                                flag3 = false;
                                                            }
                                                            else if ((nManager.Helpful.Math.DistanceListPoint(PathFinder.FindLocalPath(point, out flag3, true)) > (distance * 4f)) && (distance > 30f))
                                                            {
                                                                flag3 = false;
                                                            }
                                                            if (flag3)
                                                            {
                                                                Logging.Write("Angles- for distance " + distance);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            if (this._rugaudiRueqopauModae)
                                            {
                                                this._rugaudiRueqopauModae = false;
                                            }
                                            else if (((nearestWoWGameObject.DisplayId == 0x2775) && this._coamegeonei.IsValid) && (point.DistanceTo2D(this._coamegeonei) <= 7f))
                                            {
                                                this._rugaudiRueqopauModae = true;
                                            }
                                            if (nearestWoWGameObject.DisplayId == 0x2775)
                                            {
                                                this._coamegeonei = new Point(nManager.Wow.ObjectManager.ObjectManager.Me.Position);
                                            }
                                            if (this._rugaudiRueqopauModae)
                                            {
                                                this._kidivun = UkaoheuboebaudEvubasejoSoukeojai._isiaraohivFumeUmuol;
                                            }
                                            else
                                            {
                                                bool flag5;
                                                Logging.Write("Distance " + distance + " selected");
                                                this._kidivun = UkaoheuboebaudEvubasejoSoukeojai._uloituasuCeudiam;
                                                List<Point> listPoints = PathFinder.FindLocalPath(point, out flag5, false);
                                                if (listPoints.Count <= 0)
                                                {
                                                    Point to = nManager.Helpful.Math.GetPosition2DOfAngleAndDistance(a, orientation, 15f);
                                                    to.Z = nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z;
                                                    listPoints = PathFinder.FindLocalPath(to, out flag5, false);
                                                    if ((listPoints.Count > 0) && flag5)
                                                    {
                                                        point = new Point(to);
                                                    }
                                                }
                                                if ((!flag5 && (point.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) > 10f)) || (num >= 2))
                                                {
                                                    point.Z = PathFinder.GetZPosition(point, false);
                                                    if (point.Z == 0f)
                                                    {
                                                        point.Z = nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z + 35f;
                                                    }
                                                    else
                                                    {
                                                        point.Z += 5f;
                                                    }
                                                    if (!nManager.Wow.ObjectManager.ObjectManager.Me.InCombat || (nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted && (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying)))
                                                    {
                                                        MountTask.Mount(true, true);
                                                        LongMove.LongMoveByNewThread(point);
                                                        nManager.Helpful.Timer timer2 = new nManager.Helpful.Timer((double) ((2000f * listPoints[listPoints.Count - 1].DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position)) / 3f));
                                                        while ((LongMove.IsLongMove && !timer2.IsReady) && (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo2D(point) > 0.5f))
                                                        {
                                                            if (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat && (!nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted || (!nManagerSetting.CurrentSetting.IgnoreFightIfMounted && !Usefuls.IsFlying)))
                                                            {
                                                                LongMove.StopLongMove();
                                                                return;
                                                            }
                                                            Thread.Sleep(100);
                                                        }
                                                        LongMove.StopLongMove();
                                                        while (MovementManager.IsUnStuck)
                                                        {
                                                            Thread.Sleep(100);
                                                            LongMove.StopLongMove();
                                                        }
                                                        MovementManager.StopMove();
                                                        MountTask.DismountMount(true);
                                                        num = 0;
                                                    }
                                                }
                                                else
                                                {
                                                    float num14 = nManager.Helpful.Math.DistanceListPoint(listPoints);
                                                    float num15 = listPoints[0].DistanceTo(listPoints[listPoints.Count - 1]);
                                                    if (((MountTask.GetMountCapacity() == MountCapacity.Fly) && (num14 > 80f)) && (num14 > (num15 * 2f)))
                                                    {
                                                        Point other = new Point(nManager.Wow.ObjectManager.ObjectManager.Me.Position);
                                                        Point point12 = new Point(listPoints[listPoints.Count - 1]);
                                                        float z = other.Z;
                                                        float num17 = point12.Z;
                                                        float num18 = System.Math.Max(z, num17) + 6f;
                                                        Point point13 = new Point(other) {
                                                            Z = num18
                                                        };
                                                        Point point14 = new Point(point12) {
                                                            Z = num18
                                                        };
                                                        if (!((TraceLine.TraceLineGo(other, point13, CGWorldFrameHitFlags.HitTestAll) || TraceLine.TraceLineGo(point13, point14, CGWorldFrameHitFlags.HitTestAll)) || TraceLine.TraceLineGo(point14, point12, CGWorldFrameHitFlags.HitTestAll)))
                                                        {
                                                            Logging.Write("Flying to shortcut the path");
                                                            MountTask.Mount(true, true);
                                                            if (Usefuls.IsFlying)
                                                            {
                                                                listPoints = new List<Point>();
                                                                point13.Z += 2f;
                                                                point14.Z += 2f;
                                                                listPoints.Add(point13);
                                                                listPoints.Add(point14);
                                                                listPoints.Add(point12);
                                                            }
                                                        }
                                                    }
                                                    if ((num14 > nManagerSetting.CurrentSetting.MinimumDistanceToUseMount) && !nManagerSetting.CurrentSetting.UseGroundMount)
                                                    {
                                                        MountTask.Mount(true, false);
                                                    }
                                                    if (Usefuls.IsFlying)
                                                    {
                                                        for (int i = 0; i < listPoints.Count; i++)
                                                        {
                                                            listPoints[i].Type = "flying";
                                                        }
                                                    }
                                                    MovementManager.Go(listPoints);
                                                    float num20 = nManager.Helpful.Math.DistanceListPoint(listPoints) / 3f;
                                                    if (num20 > 200f)
                                                    {
                                                        num20 = 200f;
                                                    }
                                                    float num21 = ((1000f * num20) / 2f) + 1500f;
                                                    if (Usefuls.IsSwimming)
                                                    {
                                                        num21 /= 0.6f;
                                                    }
                                                    nManager.Helpful.Timer timer3 = new nManager.Helpful.Timer((double) num21);
                                                    while ((MovementManager.InMovement && !timer3.IsReady) && (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo2D(point) > 0.5f))
                                                    {
                                                        if (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat && (!nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted || (!nManagerSetting.CurrentSetting.IgnoreFightIfMounted && !Usefuls.IsFlying)))
                                                        {
                                                            return;
                                                        }
                                                        Thread.Sleep(100);
                                                    }
                                                    if ((nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(nearestWoWGameObject.Position) < 5f) || ((MovementManager.InMovement && !nManager.Wow.ObjectManager.ObjectManager.Me.InInevitableCombat) && timer3.IsReady))
                                                    {
                                                        num++;
                                                    }
                                                    else
                                                    {
                                                        num = 0;
                                                    }
                                                    if (!nManager.Wow.ObjectManager.ObjectManager.Me.InInevitableCombat)
                                                    {
                                                        MovementManager.StopMove();
                                                        while (MovementManager.IsUnStuck)
                                                        {
                                                            Thread.Sleep(100);
                                                            MovementManager.StopMove();
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            }
            catch
            {
            }
        }

        public override List<nManager.FiniteStateMachine.State> BeforeStates
        {
            get
            {
                return new List<nManager.FiniteStateMachine.State>();
            }
        }

        public override string DisplayName
        {
            get
            {
                return "Archaeology";
            }
        }

        public override bool NeedToRun
        {
            get
            {
                if (((Usefuls.InGame && !Usefuls.IsLoading) && (!nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe && nManager.Wow.ObjectManager.ObjectManager.Me.IsValid)) && (!nManager.Wow.ObjectManager.ObjectManager.Me.InInevitableCombat && nManager.Products.Products.IsStarted))
                {
                    if (nManager.Products.Products.InManualPause)
                    {
                        nManager.Products.Products.InManualPause = false;
                    }
                    if (!this.BlackListDigsites.Contains(this._atipohokeIj.id) && Archaeology.DigsiteZoneIsAvailable(this._atipohokeIj))
                    {
                        return true;
                    }
                    if (nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWGameObject(nManager.Wow.ObjectManager.ObjectManager.GetWoWGameObjectByEntry(Archaeology.ArchaeologyItemsFindList), false).IsValid)
                    {
                        return true;
                    }
                    List<Digsite> digsitesZoneAvailable = Archaeology.GetDigsitesZoneAvailable(null);
                    this._evepidoubaGeigicou = false;
                    if (digsitesZoneAvailable.Count > 0)
                    {
                        Digsite digsite = new Digsite {
                            id = 0,
                            name = ""
                        };
                        float maxValue = float.MaxValue;
                        float minValue = float.MinValue;
                        foreach (Digsite digsite2 in digsitesZoneAvailable)
                        {
                            if ((digsite2.PriorityDigsites > minValue) && !this.BlackListDigsites.Contains(digsite2.id))
                            {
                                minValue = digsite2.PriorityDigsites;
                            }
                        }
                        for (int i = digsitesZoneAvailable.Count - 1; i >= 0; i--)
                        {
                            if (this.BlackListDigsites.Contains(digsitesZoneAvailable[i].id) || (digsitesZoneAvailable[i].PriorityDigsites != minValue))
                            {
                                digsitesZoneAvailable.RemoveAt(i);
                            }
                        }
                        foreach (Digsite digsite3 in digsitesZoneAvailable)
                        {
                            WoWQuestPOIPoint point = WoWQuestPOIPoint.FromSetId(WoWResearchSite.FromName(digsite3.name, false).Record.QuestIdPoint);
                            float num4 = point.Center.DistanceTo2D(nManager.Wow.ObjectManager.ObjectManager.Me.Position);
                            if (num4 <= maxValue)
                            {
                                maxValue = num4;
                                digsite = digsite3;
                                this._suamauf = point;
                                this._ecocuhuawaujov = false;
                            }
                        }
                        if (digsite.id != 0)
                        {
                            if (this._ajunoDecei == null)
                            {
                                this._ajunoDecei = new Spell("Survey");
                            }
                            this._atipohokeIj = digsite;
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        public override List<nManager.FiniteStateMachine.State> NextStates
        {
            get
            {
                return new List<nManager.FiniteStateMachine.State>();
            }
        }

        public override int Priority { get; set; }

        private enum UkaoheuboebaudEvubasejoSoukeojai
        {
            _xibucLiaji,
            _uloituasuCeudiam,
            _uremiosia,
            _isiaraohivFumeUmuol
        }
    }
}

