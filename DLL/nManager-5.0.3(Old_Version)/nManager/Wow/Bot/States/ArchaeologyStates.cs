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
        private bool _AntiPingPong;
        private int _greenCount;
        private bool _inSecondDigSiteWithSameName;
        private Point _lastGreenPosition = new Point();
        private int _nbTryFarmInThisZone;
        private bool _travelDisabled;
        private Point _travelLocation;
        private readonly List<int> BlackListDigsites = new List<int>();
        public bool CrateRestored = true;
        private Digsite digsitesZone = new Digsite();
        private int LastZone;
        public int MaxTryByDigsite = 30;
        private LocState myState = LocState.LocalMove;
        private int nbCastSurveyError;
        private int nbLootAttempt;
        private WoWQuestPOIPoint qPOI;
        public int SolvingEveryXMin = 20;
        private Spell surveySpell;
        private nManager.Helpful.Timer timerAutoSolving;
        private nManager.Helpful.Timer timerLooting;
        public bool UseKeystones = true;

        private bool IsPointOutOfWater(Point p)
        {
            return !TraceLine.TraceLineGo(new Point(p.X, p.Y, p.Z + 1000f, "None"), p, CGWorldFrameHitFlags.HitTestLiquid);
        }

        public override void Run()
        {
            try
            {
                if (!MovementManager.InMovement)
                {
                    if (this.LastZone != this.digsitesZone.id)
                    {
                        this._nbTryFarmInThisZone = 0;
                    }
                    this.LastZone = this.digsitesZone.id;
                    if (this.timerAutoSolving == null)
                    {
                        this.timerAutoSolving = new nManager.Helpful.Timer((double) ((this.SolvingEveryXMin * 0x3e8) * 60));
                    }
                    if ((this.timerAutoSolving.IsReady && !nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe) && !nManager.Wow.ObjectManager.ObjectManager.Me.InCombat)
                    {
                        MovementManager.StopMove();
                        LongMove.StopLongMove();
                        if ((Archaeology.SolveAllArtifact(this.UseKeystones) > 0) && this.CrateRestored)
                        {
                            Archaeology.CrateRestoredArtifact();
                        }
                        this.timerAutoSolving = new nManager.Helpful.Timer((double) ((this.SolvingEveryXMin * 0x3e8) * 60));
                    }
                    if (!MovementManager.InMovement)
                    {
                        int num = 0;
                        try
                        {
                            if (this.myState != LocState.LocalMove)
                            {
                                MountTask.DismountMount(true);
                            }
                            WoWGameObject nearestWoWGameObject = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWGameObject(nManager.Wow.ObjectManager.ObjectManager.GetWoWGameObjectByEntry(Archaeology.ArchaeologyItemsFindList), false);
                            if (nearestWoWGameObject.IsValid)
                            {
                                bool flag;
                                this.nbCastSurveyError = 0;
                                this._lastGreenPosition = new Point();
                                this._AntiPingPong = false;
                                this._greenCount = 0;
                                if (this.myState == LocState.Looting)
                                {
                                    if ((this.timerLooting != null) && this.timerLooting.IsReady)
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
                                    MountTask.Mount(true);
                                    new Point(nearestWoWGameObject.Position);
                                    points.Add(nearestWoWGameObject.Position);
                                }
                                MovementManager.Go(points);
                                if (this.nbLootAttempt > 2)
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
                                    this.nbLootAttempt = 0;
                                }
                                else
                                {
                                    Logging.Write("Loot " + nearestWoWGameObject.Name);
                                    nManager.Helpful.Timer timer = new nManager.Helpful.Timer((double) ((1000f * nManager.Helpful.Math.DistanceListPoint(points)) / 3f));
                                    while ((MovementManager.InMovement && !timer.IsReady) && (nearestWoWGameObject.GetDistance > 3.5f))
                                    {
                                        Thread.Sleep(100);
                                        if (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat && (!nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted || (!nManagerSetting.CurrentSetting.IgnoreFightIfMounted && !Usefuls.IsFlying)))
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
                                        this.nbLootAttempt++;
                                        this.myState = LocState.Looting;
                                        if (this.timerLooting == null)
                                        {
                                            this.timerLooting = new nManager.Helpful.Timer(5000.0);
                                        }
                                        else
                                        {
                                            this.timerLooting.Reset();
                                        }
                                    }
                                }
                            }
                            else if (this._nbTryFarmInThisZone > this.MaxTryByDigsite)
                            {
                                this.nbLootAttempt = 0;
                                this.BlackListDigsites.Add(this.digsitesZone.id);
                                Logging.Write("Black List Digsite: " + this.digsitesZone.name);
                                this.myState = LocState.LocalMove;
                                this.nbCastSurveyError = 0;
                            }
                            else
                            {
                                bool flag2 = false;
                                if (((this.qPOI != null) && !this.qPOI.ValidPoint) && (this.qPOI.Center.DistanceTo2D(nManager.Wow.ObjectManager.ObjectManager.Me.Position) < 40f))
                                {
                                    Point middlePoint = this.qPOI.MiddlePoint;
                                    if (Usefuls.IsFlying)
                                    {
                                        flag2 = true;
                                        MovementManager.StopMove();
                                    }
                                }
                                if ((this.qPOI != null) && (flag2 || !this.qPOI.IsInside(nManager.Wow.ObjectManager.ObjectManager.Me.Position)))
                                {
                                    if ((MountTask.GetMountCapacity() == MountCapacity.Feet) || (MountTask.GetMountCapacity() == MountCapacity.Ground))
                                    {
                                        int entry = 0x1ca8d;
                                        if ((!this._travelDisabled && (ItemsManager.GetItemCount(entry) > 0)) && ((Usefuls.RealContinentId == 0x45c) && (this.qPOI.Center.DistanceTo2D(nManager.Wow.ObjectManager.ObjectManager.Me.Position) > 3000f)))
                                        {
                                            WoWItem woWItemById = nManager.Wow.ObjectManager.ObjectManager.GetWoWItemById(entry);
                                            if (((woWItemById != null) && woWItemById.IsValid) && (!ItemsManager.IsItemOnCooldown(entry) && ItemsManager.IsItemUsable(entry)))
                                            {
                                                Logging.Write("Using a Draenor Archaeologist's Lodestone");
                                                nManager.Products.Products.InManualPause = true;
                                                ItemsManager.UseItem(entry);
                                                Thread.Sleep((int) (0xbb8 + Usefuls.Latency));
                                                this.digsitesZone = new Digsite();
                                                return;
                                            }
                                        }
                                        Logging.Write("Not inside, then go to Digsite " + this.digsitesZone.name);
                                        Point position = nManager.Wow.ObjectManager.ObjectManager.Me.Position;
                                        if (((this._travelLocation == null) || (this._travelLocation.DistanceTo(position) > 0.1f)) && !this._travelDisabled)
                                        {
                                            Logging.Write("Calling travel system...");
                                            nManager.Products.Products.TravelToContinentId = Usefuls.ContinentId;
                                            nManager.Products.Products.TravelTo = this.qPOI.Center;
                                            nManager.Products.Products.TargetValidationFct = new Func<Point, bool>(this.qPOI.IsInside);
                                            this._travelLocation = position;
                                            return;
                                        }
                                        if (this._travelLocation.DistanceTo(position) <= 0.1f)
                                        {
                                            this._travelDisabled = true;
                                        }
                                        MovementManager.Go(PathFinder.FindPath(this.qPOI.Center));
                                    }
                                    else if (this.qPOI.ValidPoint)
                                    {
                                        if (flag2 || !this.qPOI.IsInside(nManager.Wow.ObjectManager.ObjectManager.Me.Position))
                                        {
                                            Point point2 = new Point(this.qPOI.MiddlePoint) {
                                                Type = "flying"
                                            };
                                            if (flag2)
                                            {
                                                Logging.Write("Landing on the digsite");
                                            }
                                            else
                                            {
                                                Logging.Write(string.Concat(new object[] { "Not inside, then go to Digsite ", this.digsitesZone.name, "; X: ", point2.X, "; Y: ", point2.Y, "; Z: ", (int) point2.Z }));
                                            }
                                            MovementManager.Go(new List<Point>(new Point[] { point2 }));
                                        }
                                    }
                                    else
                                    {
                                        Point center = this.qPOI.Center;
                                        center.Z += 40f;
                                        center.Type = "flying";
                                        Logging.Write(string.Concat(new object[] { "Go to Digsite ", this.digsitesZone.name, "; X: ", center.X, "; Y: ", center.Y, "; Z: ", (int) center.Z }));
                                        MovementManager.Go(new List<Point>(new Point[] { center }));
                                    }
                                    this.myState = LocState.LocalMove;
                                }
                                else
                                {
                                    this.nbLootAttempt = 0;
                                    nearestWoWGameObject = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWGameObject(nManager.Wow.ObjectManager.ObjectManager.GetWoWGameObjectByDisplayId(Archaeology.SurveyList), false);
                                    if (((nearestWoWGameObject.GetBaseAddress == 0) || (this.myState == LocState.GoingNextPoint)) || (this.myState == LocState.Looting))
                                    {
                                        if (Archaeology.DigsiteZoneIsAvailable(this.digsitesZone))
                                        {
                                            if (this.myState == LocState.LocalMove)
                                            {
                                                MountTask.DismountMount(true);
                                            }
                                            this.surveySpell.Launch();
                                            this.myState = LocState.Survey;
                                            if (!nManager.Wow.ObjectManager.ObjectManager.Me.InCombat)
                                            {
                                                Thread.Sleep((int) (0x6d6 + Usefuls.Latency));
                                                this.nbCastSurveyError++;
                                                if (this.nbCastSurveyError > 3)
                                                {
                                                    if (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo2D(this.qPOI.MiddlePoint) < 5f)
                                                    {
                                                        List<Digsite> digsitesZoneAvailable;
                                                        WoWResearchSite site;
                                                        if (!this._inSecondDigSiteWithSameName)
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
                                                                this.digsitesZone = digsite;
                                                            }
                                                        }
                                                        if (!this._inSecondDigSiteWithSameName)
                                                        {
                                                            site = WoWResearchSite.FromName(this.digsitesZone.name, true);
                                                        }
                                                        else
                                                        {
                                                            site = WoWResearchSite.FromName(this.digsitesZone.name, false);
                                                        }
                                                        this.qPOI = WoWQuestPOIPoint.FromSetId(site.Record.QuestIdPoint);
                                                        this._inSecondDigSiteWithSameName = !this._inSecondDigSiteWithSameName;
                                                        this.nbCastSurveyError = 0;
                                                    }
                                                    else
                                                    {
                                                        if ((MountTask.GetMountCapacity() == MountCapacity.Feet) || (MountTask.GetMountCapacity() == MountCapacity.Ground))
                                                        {
                                                            Logging.Write("Too many errors, then go to Digsite " + this.digsitesZone.name);
                                                            MovementManager.Go(PathFinder.FindPath(this.qPOI.Center));
                                                        }
                                                        else if (this.qPOI != null)
                                                        {
                                                            Point point4 = this.qPOI.MiddlePoint;
                                                            Logging.Write(string.Concat(new object[] { "Too many errors, then go to Digsite ", this.digsitesZone.name, "; X: ", point4.X, "; Y: ", point4.Y, "; Z: ", (int) point4.Z }));
                                                            MovementManager.Go(new List<Point>(new Point[] { point4 }));
                                                        }
                                                        this.nbCastSurveyError = 0;
                                                    }
                                                }
                                                else
                                                {
                                                    this._nbTryFarmInThisZone++;
                                                }
                                            }
                                        }
                                    }
                                    else if (this.myState != LocState.GoingNextPoint)
                                    {
                                        this.nbCastSurveyError = 0;
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
                                                this._lastGreenPosition = new Point();
                                                this._AntiPingPong = false;
                                                this._greenCount = 0;
                                            }
                                            else if (nearestWoWGameObject.DisplayId == 0x2776)
                                            {
                                                num4 = 46f;
                                                num5 = 20f;
                                                num6 = 60f;
                                                num8 = 7f;
                                                num7 = 6.5f;
                                                this._lastGreenPosition = new Point();
                                                this._AntiPingPong = false;
                                                this._greenCount = 0;
                                            }
                                            else
                                            {
                                                this._greenCount++;
                                                num8 = 4f;
                                                if (this._greenCount >= 10)
                                                {
                                                    this._greenCount = 0;
                                                    this._lastGreenPosition = new Point();
                                                    Point point7 = this.qPOI.MiddlePoint;
                                                    this._AntiPingPong = false;
                                                    Logging.Write(string.Concat(new object[] { "Stuck, then go to Digsite ", this.digsitesZone.name, "; X: ", point7.X, "; Y: ", point7.Y, "; Z: ", (int) point7.Z }));
                                                    MountTask.Mount(true);
                                                    MovementManager.Go(new List<Point>(new Point[] { point7 }));
                                                    return;
                                                }
                                                if (this._AntiPingPong)
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
                                            if (this.qPOI != null)
                                            {
                                                bool flag4 = this.IsPointOutOfWater(nManager.Wow.ObjectManager.ObjectManager.Me.Position);
                                                while (((!flag3 || (point.Z == 0f)) || !this.qPOI.IsInside(point)) || (flag4 && !this.IsPointOutOfWater(point)))
                                                {
                                                    if ((distance + num8) > num6)
                                                    {
                                                        break;
                                                    }
                                                    distance += num8;
                                                    Point p = nManager.Helpful.Math.GetPosition2DOfAngleAndDistance(a, orientation, distance);
                                                    if (this.qPOI.IsInside(p))
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
                                                if ((!flag3 || (point.Z == 0f)) || (!this.qPOI.IsInside(point) || (flag4 && !this.IsPointOutOfWater(point))))
                                                {
                                                    distance = num4;
                                                    while (((!flag3 || (point.Z == 0f)) || !this.qPOI.IsInside(point)) || (flag4 && !this.IsPointOutOfWater(point)))
                                                    {
                                                        if ((distance - num7) < num5)
                                                        {
                                                            break;
                                                        }
                                                        distance -= num7;
                                                        Point point9 = nManager.Helpful.Math.GetPosition2DOfAngleAndDistance(a, orientation, distance);
                                                        if (this.qPOI.IsInside(point9))
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
                                            if (this._AntiPingPong)
                                            {
                                                this._AntiPingPong = false;
                                            }
                                            else if (((nearestWoWGameObject.DisplayId == 0x2775) && this._lastGreenPosition.IsValid) && (point.DistanceTo2D(this._lastGreenPosition) <= 7f))
                                            {
                                                this._AntiPingPong = true;
                                            }
                                            if (nearestWoWGameObject.DisplayId == 0x2775)
                                            {
                                                this._lastGreenPosition = new Point(nManager.Wow.ObjectManager.ObjectManager.Me.Position);
                                            }
                                            if (this._AntiPingPong)
                                            {
                                                this.myState = LocState.LocalMove;
                                            }
                                            else
                                            {
                                                bool flag5;
                                                Logging.Write("Distance " + distance + " selected");
                                                this.myState = LocState.GoingNextPoint;
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
                                                        MountTask.Mount(true);
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
                                                            MountTask.Mount(true);
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
                                                        MountTask.Mount(true);
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
                                                    if ((nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(nearestWoWGameObject.Position) < 5f) || ((MovementManager.InMovement && (!nManager.Wow.ObjectManager.ObjectManager.Me.InCombat || (nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted && (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying)))) && timer3.IsReady))
                                                    {
                                                        num++;
                                                    }
                                                    else
                                                    {
                                                        num = 0;
                                                    }
                                                    if (!nManager.Wow.ObjectManager.ObjectManager.Me.InCombat || (nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted && (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying)))
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
                if ((((Usefuls.InGame && !Usefuls.IsLoadingOrConnecting) && (!nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe && nManager.Wow.ObjectManager.ObjectManager.Me.IsValid)) && (!nManager.Wow.ObjectManager.ObjectManager.Me.InCombat || (nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted && (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying)))) && nManager.Products.Products.IsStarted)
                {
                    if (nManager.Products.Products.InManualPause)
                    {
                        nManager.Products.Products.InManualPause = false;
                    }
                    if (!this.BlackListDigsites.Contains(this.digsitesZone.id) && Archaeology.DigsiteZoneIsAvailable(this.digsitesZone))
                    {
                        return true;
                    }
                    if (nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWGameObject(nManager.Wow.ObjectManager.ObjectManager.GetWoWGameObjectByEntry(Archaeology.ArchaeologyItemsFindList), false).IsValid)
                    {
                        return true;
                    }
                    List<Digsite> digsitesZoneAvailable = Archaeology.GetDigsitesZoneAvailable(null);
                    this._inSecondDigSiteWithSameName = false;
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
                                this.qPOI = point;
                                this._travelDisabled = false;
                            }
                        }
                        if (digsite.id != 0)
                        {
                            if (this.surveySpell == null)
                            {
                                this.surveySpell = new Spell("Survey");
                            }
                            this.digsitesZone = digsite;
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

        private enum LocState
        {
            Looting,
            GoingNextPoint,
            Survey,
            LocalMove
        }
    }
}

