namespace nManager.Wow.Bot.States
{
    using nManager.FiniteStateMachine;
    using nManager.Helpful;
    using nManager.Products;
    using nManager.Wow.Bot.Tasks;
    using nManager.Wow.Class;
    using nManager.Wow.Helpers;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    public class Travel : nManager.FiniteStateMachine.State
    {
        private Portals _availablePortals;
        private List<TaxiLink> _availableTaxiLinks;
        private List<Taxi> _availableTaxis;
        private Transports _availableTransports;
        private List<Transport> _generatedRoutePath = new List<Transport>();
        private List<Taxi> _unknownTaxis = new List<Taxi>();
        private bool _unknownTaxisChecked;

        public static  event EventHandler AutomaticallyTookTaxi;

        private void EnterTransportOrTakePortal(Transport selectedTransport)
        {
            if (selectedTransport is Portal)
            {
                Portal portal = selectedTransport as Portal;
                WoWGameObject obj2 = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWGameObject(nManager.Wow.ObjectManager.ObjectManager.GetWoWGameObjectByEntry((int) portal.Id), nManager.Wow.ObjectManager.ObjectManager.Me.Position, false);
                bool flag = true;
                while (flag)
                {
                    if (Usefuls.IsFlying)
                    {
                        MountTask.DismountMount(true);
                    }
                    if (obj2.IsValid)
                    {
                        if (obj2.GetDistance > 4f)
                        {
                            MovementManager.Go(PathFinder.FindPath(obj2.Position));
                            while (obj2.GetDistance > 4f)
                            {
                                if (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat || nManager.Wow.ObjectManager.ObjectManager.Me.IsDead)
                                {
                                    return;
                                }
                                Thread.Sleep(150);
                            }
                        }
                        MountTask.DismountMount(true);
                        Thread.Sleep(150);
                        Interact.InteractWith(obj2.GetBaseAddress, false);
                        TravelPatientlybyTaxiOrPortal(false);
                        flag = false;
                    }
                    else
                    {
                        if (portal.APoint.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) > 4f)
                        {
                            this.GoToDepartureQuayOrPortal(selectedTransport);
                            this.EnterTransportOrTakePortal(selectedTransport);
                            return;
                        }
                        obj2 = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWGameObject(nManager.Wow.ObjectManager.ObjectManager.GetWoWGameObjectByEntry((int) portal.Id), nManager.Wow.ObjectManager.ObjectManager.Me.Position, false);
                    }
                }
            }
            else if (selectedTransport is Taxi)
            {
                Taxi taxi = selectedTransport as Taxi;
                WoWUnit unit = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWUnit(nManager.Wow.ObjectManager.ObjectManager.GetWoWUnitByEntry((int) taxi.Id, false), nManager.Wow.ObjectManager.ObjectManager.Me.Position, false);
                bool flag2 = true;
                while (flag2)
                {
                    if (Usefuls.IsFlying)
                    {
                        MountTask.DismountMount(true);
                    }
                    if (unit.IsValid)
                    {
                        if (unit.GetDistance > 4f)
                        {
                            MovementManager.Go(PathFinder.FindPath(unit.Position));
                            while (unit.GetDistance > 4f)
                            {
                                if (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat || nManager.Wow.ObjectManager.ObjectManager.Me.IsDead)
                                {
                                    return;
                                }
                                Thread.Sleep(150);
                            }
                        }
                        Taxi taxi2 = this.FindNextTaxiHopFor(taxi, true);
                        if (taxi2 == null)
                        {
                            Logging.Write("There is a problem with taxi links, some are missing to complete the minimal graph");
                            return;
                        }
                        MountTask.DismountMount(true);
                        Interact.InteractWith(unit.GetBaseAddress, true);
                        Thread.Sleep((int) (250 + Usefuls.Latency));
                        if (!Gossip.IsTaxiWindowOpen())
                        {
                            Gossip.SelectGossip(Gossip.GossipOption.Taxi);
                            Thread.Sleep((int) (250 + Usefuls.Latency));
                        }
                        if (!Gossip.IsTaxiWindowOpen())
                        {
                            Interact.InteractWith(unit.GetBaseAddress, true);
                            Thread.Sleep((int) (250 + Usefuls.Latency));
                        }
                        if (!Gossip.IsTaxiWindowOpen())
                        {
                            Gossip.SelectGossip(Gossip.GossipOption.Taxi);
                            Thread.Sleep((int) (250 + Usefuls.Latency));
                        }
                        if (!Gossip.IsTaxiWindowOpen())
                        {
                            Logging.Write("There is a problem with taxi master");
                            return;
                        }
                        this._unknownTaxis.Remove(taxi);
                        if (!this._unknownTaxisChecked)
                        {
                            this._unknownTaxisChecked = true;
                            List<Taxi> allTaxisAvailable = Gossip.GetAllTaxisAvailable();
                            using (List<Taxi>.Enumerator enumerator = this._availableTaxis.GetEnumerator())
                            {
                                while (enumerator.MoveNext())
                                {
                                    Predicate<Taxi> match = null;
                                    Taxi oneTaxi = enumerator.Current;
                                    if (match == null)
                                    {
                                        match = x => (x.Xcoord == oneTaxi.Xcoord) && (x.Ycoord == oneTaxi.Ycoord);
                                    }
                                    Taxi taxi3 = allTaxisAvailable.Find(match);
                                    if (((taxi3 == null) || (taxi3.Xcoord == "")) && ((oneTaxi.Faction == Npc.FactionType.Neutral) || (oneTaxi.Faction.ToString() == nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction)))
                                    {
                                        this._unknownTaxis.Add(oneTaxi);
                                    }
                                }
                            }
                        }
                        CombatClass.DisposeCombatClass();
                        Gossip.TakeTaxi(taxi2.Xcoord, taxi2.Ycoord);
                        if (AutomaticallyTookTaxi != null)
                        {
                            TaxiEventArgs e = new TaxiEventArgs {
                                From = unit.Entry,
                                To = (int) taxi2.Id
                            };
                            AutomaticallyTookTaxi(this, e);
                        }
                        Logging.Write("Flying to " + taxi2.Name);
                        flag2 = false;
                    }
                    else
                    {
                        if (taxi.APoint.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) > 4f)
                        {
                            this.GoToDepartureQuayOrPortal(selectedTransport);
                            this.EnterTransportOrTakePortal(selectedTransport);
                            return;
                        }
                        unit = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWUnit(nManager.Wow.ObjectManager.ObjectManager.GetWoWUnitByEntry((int) taxi.Id, false), nManager.Wow.ObjectManager.ObjectManager.Me.Position, false);
                    }
                }
            }
            else
            {
                Logging.Write(string.Concat(new object[] { "Transport ", selectedTransport.Name, "(", selectedTransport.Id, ") arrived at the quay, entering transport." }));
                while (!nManager.Wow.ObjectManager.ObjectManager.Me.InTransport)
                {
                    if ((nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(selectedTransport.ArrivalIsA ? selectedTransport.BOutsidePoint : selectedTransport.AOutsidePoint) > 10f) && (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(selectedTransport.ArrivalIsA ? selectedTransport.BInsidePoint : selectedTransport.AInsidePoint) > nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(selectedTransport.ArrivalIsA ? selectedTransport.BOutsidePoint : selectedTransport.AOutsidePoint)))
                    {
                        this.GoToDepartureQuayOrPortal(selectedTransport);
                        this.EnterTransportOrTakePortal(selectedTransport);
                        Logging.Write(string.Concat(new object[] { "Failed to enter transport ", selectedTransport.Name, "(", selectedTransport.Id, ") going back to the quay." }));
                    }
                    MovementManager.MoveTo(selectedTransport.ArrivalIsA ? selectedTransport.BInsidePoint : selectedTransport.AInsidePoint, false);
                    bool flag3 = true;
                    while (flag3)
                    {
                        if (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat || nManager.Wow.ObjectManager.ObjectManager.Me.IsDead)
                        {
                            return;
                        }
                        if (nManager.Wow.ObjectManager.ObjectManager.Me.InTransport)
                        {
                            flag3 = false;
                            Thread.Sleep(0x3e8);
                        }
                        if (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(selectedTransport.ArrivalIsA ? selectedTransport.BInsidePoint : selectedTransport.AInsidePoint) <= 2f)
                        {
                            flag3 = false;
                            MovementManager.StopMove();
                            Thread.Sleep(100);
                            if (!nManager.Wow.ObjectManager.ObjectManager.Me.InTransport)
                            {
                                MovementsAction.Jump();
                            }
                        }
                        Thread.Sleep(500);
                    }
                }
                Thread.Sleep(100);
                MovementManager.StopMove();
                Logging.Write(string.Concat(new object[] { "Successfuly entered transport ", selectedTransport.Name, "(", selectedTransport.Id, "), waiting to arrive at destination." }));
            }
        }

        private Taxi FindNextTaxiHopFor(Taxi taxi, bool display = false)
        {
            List<List<uint>> list3;
            Predicate<Taxi> match = null;
            List<TaxiLink> list = this._availableTaxiLinks.ToList<TaxiLink>();
            uint num = taxi.Id;
            uint endId = taxi.EndOfPath;
            bool flag = true;
            for (List<List<uint>> list2 = new List<List<uint>> {
                new List<uint> { num }
            }; flag && (list.Count > 0); list2 = list3)
            {
                flag = false;
                bool flag2 = false;
                list3 = new List<List<uint>>();
                foreach (List<uint> list4 in list2)
                {
                    uint currentHop = list4.Last<uint>();
                    foreach (TaxiLink link in list.FindAll(delegate (TaxiLink x) {
                        if (x.PointA != currentHop)
                        {
                            return x.PointB == currentHop;
                        }
                        return true;
                    }))
                    {
                        list.Remove(link);
                        uint target = (link.PointA == currentHop) ? link.PointB : link.PointA;
                        Taxi item = this._availableTaxis.Find(x => x.Id == target);
                        if ((((item != null) && (item.Id != 0)) && ((item.Faction == Npc.FactionType.Neutral) || (item.Faction.ToString() == nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction))) && !this._unknownTaxis.Contains(item))
                        {
                            if (target == endId)
                            {
                                flag2 = true;
                            }
                            List<uint> list6 = new List<uint>();
                            list6.AddRange(list4);
                            list6.Add(target);
                            list3.Add(list6);
                            flag = true;
                        }
                    }
                }
                if (flag2)
                {
                    float maxValue = float.MaxValue;
                    List<uint> bestPathFound = new List<uint>();
                    foreach (List<uint> list7 in list3)
                    {
                        if (list7.Last<uint>() == endId)
                        {
                            Taxi taxi3 = null;
                            float num3 = 0f;
                            using (List<uint>.Enumerator enumerator4 = list7.GetEnumerator())
                            {
                                while (enumerator4.MoveNext())
                                {
                                    Predicate<Taxi> predicate = null;
                                    Predicate<Taxi> predicate2 = null;
                                    uint id = enumerator4.Current;
                                    if (taxi3 == null)
                                    {
                                        if (predicate == null)
                                        {
                                            predicate = x => x.Id == id;
                                        }
                                        taxi3 = this._availableTaxis.Find(predicate);
                                    }
                                    else
                                    {
                                        if (predicate2 == null)
                                        {
                                            predicate2 = x => x.Id == id;
                                        }
                                        Taxi taxi4 = this._availableTaxis.Find(predicate2);
                                        num3 += taxi3.Position.DistanceTo(taxi4.Position);
                                        taxi3 = taxi4;
                                    }
                                }
                            }
                            if (num3 < maxValue)
                            {
                                maxValue = num3;
                                bestPathFound = list7;
                            }
                        }
                    }
                    if (display)
                    {
                        Logging.Write("Taxi travel plan: " + string.Join<uint>(", ", bestPathFound));
                    }
                    if (bestPathFound.Count > 1)
                    {
                        return this._availableTaxis.Find(x => x.Id == bestPathFound[1]);
                    }
                    if (match == null)
                    {
                        match = x => x.Id == endId;
                    }
                    return this._availableTaxis.Find(match);
                }
            }
            return null;
        }

        private List<Transport> GetAllTransportsThatDirectlyGoToDestination(Point travelTo, Point travelFrom, int travelToContinentId, int travelFromContinentId)
        {
            List<Transport> list = new List<Transport>();
            List<Transport> collection = this.GetTransportsThatDirectlyGoToDestination(travelTo, travelFrom, travelToContinentId, travelFromContinentId);
            List<Portal> list3 = this.GetPortalsThatDirectlyGoToDestination(travelTo, travelFrom, travelToContinentId, travelFromContinentId);
            Taxi item = this.GetTaxisThatDirectlyGoToDestination(travelTo, travelFrom, travelToContinentId, travelFromContinentId);
            list.AddRange(collection);
            list.AddRange(list3);
            if (item != null)
            {
                list.Add(item);
            }
            return list;
        }

        private List<Transport> GetAllTransportsThatGoesToDestination(Point travelTo, int travelToContinentId)
        {
            List<Transport> list = new List<Transport>();
            List<Transport> transportsThatGoesToDestination = this.GetTransportsThatGoesToDestination(travelTo, travelToContinentId);
            List<Portal> portalsThatGoesToDestination = this.GetPortalsThatGoesToDestination(travelTo, travelToContinentId);
            Taxi taxiThatGoesToDestination = this.GetTaxiThatGoesToDestination(travelTo, travelToContinentId);
            list.AddRange(transportsThatGoesToDestination);
            list.AddRange(portalsThatGoesToDestination);
            if (taxiThatGoesToDestination != null)
            {
                list.Add(taxiThatGoesToDestination);
            }
            return list;
        }

        private KeyValuePair<Transport, float> GetBestDirectWayTransport(Point travelFrom, Point travelTo, int travelFromContinentId, int travelToContinentId)
        {
            Transport key = new Transport();
            float maxValue = float.MaxValue;
            foreach (Transport transport2 in this.GetAllTransportsThatDirectlyGoToDestination(travelTo, travelFrom, travelToContinentId, travelFromContinentId))
            {
                float num2;
                uint id = 0;
                if (transport2 is Portal)
                {
                    Portal portal = transport2 as Portal;
                    List<Point> listPoints = PathFinder.FindPath(travelFrom, portal.APoint, Usefuls.ContinentNameMpqByContinentId(travelFromContinentId));
                    List<Point> list3 = PathFinder.FindPath(portal.BPoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId));
                    num2 = nManager.Helpful.Math.DistanceListPoint(listPoints) + nManager.Helpful.Math.DistanceListPoint(list3);
                    id = portal.Id;
                }
                else if (transport2 is Taxi)
                {
                    Taxi taxi = transport2 as Taxi;
                    List<Point> list4 = PathFinder.FindPath(travelFrom, taxi.APoint, Usefuls.ContinentNameMpqByContinentId(travelFromContinentId));
                    List<Point> list5 = PathFinder.FindPath(taxi.BPoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId));
                    num2 = nManager.Helpful.Math.DistanceListPoint(list4) + nManager.Helpful.Math.DistanceListPoint(list5);
                    if (travelFromContinentId == travelToContinentId)
                    {
                        num2 += taxi.APoint.DistanceTo(taxi.BPoint) / 2.5f;
                    }
                    id = taxi.Id;
                }
                else if (transport2.ArrivalIsA)
                {
                    List<Point> list6 = PathFinder.FindPath(travelFrom, transport2.BOutsidePoint, Usefuls.ContinentNameMpqByContinentId(travelFromContinentId));
                    List<Point> list7 = PathFinder.FindPath(transport2.AOutsidePoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId));
                    num2 = nManager.Helpful.Math.DistanceListPoint(list6) + nManager.Helpful.Math.DistanceListPoint(list7);
                    id = transport2.Id;
                }
                else
                {
                    List<Point> list8 = PathFinder.FindPath(travelFrom, transport2.AOutsidePoint, Usefuls.ContinentNameMpqByContinentId(travelFromContinentId));
                    List<Point> list9 = PathFinder.FindPath(transport2.BOutsidePoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId));
                    num2 = nManager.Helpful.Math.DistanceListPoint(list8) + nManager.Helpful.Math.DistanceListPoint(list9);
                    id = transport2.Id;
                }
                if (num2 < maxValue)
                {
                    key = transport2;
                    key.Id = id;
                    maxValue = num2;
                }
            }
            if (key.Id == 0)
            {
                return new KeyValuePair<Transport, float>(new Transport(), float.MaxValue);
            }
            return new KeyValuePair<Transport, float>(key, maxValue);
        }

        private KeyValuePair<List<Transport>, float> GetBestTwoWayTransport(Point travelFrom, Point travelTo, int travelFromContinentId, int travelToContinentId)
        {
            List<Transport> key = new List<Transport>();
            float maxValue = float.MaxValue;
            foreach (Transport transport in this.GetAllTransportsThatGoesToDestination(travelTo, travelToContinentId))
            {
                float num2;
                if (transport is Portal)
                {
                    Portal portal = transport as Portal;
                    if (portal.AContinentId == travelFromContinentId)
                    {
                        KeyValuePair<Transport, float> pair = this.GetBestDirectWayTransport(travelFrom, portal.APoint, travelFromContinentId, portal.BContinentId);
                        List<Point> listPoints = PathFinder.FindPath(portal.BPoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId));
                        num2 = pair.Value + nManager.Helpful.Math.DistanceListPoint(listPoints);
                        if (num2 < maxValue)
                        {
                            key = new List<Transport> {
                                pair.Key,
                                transport
                            };
                            maxValue = num2;
                        }
                    }
                }
                else if (!(transport is Taxi))
                {
                    if (transport.ArrivalIsA)
                    {
                        if (transport.BContinentId == travelFromContinentId)
                        {
                            KeyValuePair<Transport, float> pair2 = this.GetBestDirectWayTransport(travelFrom, transport.BOutsidePoint, travelFromContinentId, transport.AContinentId);
                            List<Point> list5 = PathFinder.FindPath(transport.AOutsidePoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId));
                            num2 = pair2.Value + nManager.Helpful.Math.DistanceListPoint(list5);
                            if (num2 < maxValue)
                            {
                                key = new List<Transport> {
                                    pair2.Key,
                                    transport
                                };
                                maxValue = num2;
                            }
                        }
                    }
                    else if (transport.AContinentId == travelFromContinentId)
                    {
                        KeyValuePair<Transport, float> pair3 = this.GetBestDirectWayTransport(travelFrom, transport.AOutsidePoint, travelFromContinentId, transport.BContinentId);
                        List<Point> list7 = PathFinder.FindPath(transport.BOutsidePoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId));
                        num2 = pair3.Value + nManager.Helpful.Math.DistanceListPoint(list7);
                        if (num2 < maxValue)
                        {
                            key = new List<Transport> {
                                pair3.Key,
                                transport
                            };
                            maxValue = num2;
                        }
                    }
                }
            }
            if (key.Count == 2)
            {
                return new KeyValuePair<List<Transport>, float>(key, maxValue);
            }
            return new KeyValuePair<List<Transport>, float>(new List<Transport>(), float.MaxValue);
        }

        private List<Portal> GetPortalsThatDirectlyGoToDestination(Point travelTo, Point travelFrom, int travelToContinentId, int travelFromContinentId)
        {
            List<Portal> list = new List<Portal>();
            foreach (Portal portal in this.GetPortalsThatGoesToDestination(travelTo, travelToContinentId))
            {
                if (portal.AContinentId == travelFromContinentId)
                {
                    bool flag;
                    PathFinder.FindPath(portal.APoint, travelFrom, Usefuls.ContinentNameMpqByContinentId(travelFromContinentId), out flag, true, false, false);
                    if (flag)
                    {
                        list.Add(portal);
                    }
                }
            }
            return list;
        }

        private List<Portal> GetPortalsThatGoesToDestination(Point travelTo, int travelToContinentId)
        {
            List<Portal> list = new List<Portal>();
            foreach (Portal portal in this._availablePortals.Items)
            {
                if (((portal.Faction == Npc.FactionType.Neutral) || (portal.Faction.ToString() == nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction)) && (portal.BContinentId == travelToContinentId))
                {
                    bool flag;
                    PathFinder.FindPath(portal.BPoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId), out flag, true, false, false);
                    if (flag)
                    {
                        list.Add(portal);
                    }
                }
            }
            return list;
        }

        private Taxi GetTaxisThatDirectlyGoToDestination(Point travelTo, Point travelFrom, int travelToContinentId, int travelFromContinentId)
        {
            Point currentPosition;
            Taxi taxiThatGoesToDestination = this.GetTaxiThatGoesToDestination(travelTo, travelToContinentId);
            if (taxiThatGoesToDestination != null)
            {
                currentPosition = nManager.Wow.ObjectManager.ObjectManager.Me.Position;
                this._availableTaxis.Sort(delegate (Taxi x, Taxi y) {
                    if (currentPosition.DistanceTo(x.Position) >= currentPosition.DistanceTo(y.Position))
                    {
                        return 1;
                    }
                    return -1;
                });
                uint num = 0;
                foreach (Taxi taxi2 in this._availableTaxis)
                {
                    if (num >= 3)
                    {
                        break;
                    }
                    if (taxi2.Position == taxiThatGoesToDestination.Position)
                    {
                        return null;
                    }
                    if (((taxi2.Faction == Npc.FactionType.Neutral) || (taxi2.Faction.ToString() == nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction)) && (taxi2.ContinentId == travelFromContinentId))
                    {
                        taxi2.EndOfPath = taxiThatGoesToDestination.Id;
                        if (this.IsTaxiLinked(taxi2))
                        {
                            bool flag;
                            num++;
                            PathFinder.FindPath(taxi2.Position, travelFrom, Usefuls.ContinentNameMpqByContinentId(travelFromContinentId), out flag, true, false, false);
                            if (flag)
                            {
                                taxi2.APoint = taxi2.Position;
                                taxi2.BPoint = taxiThatGoesToDestination.Position;
                                return taxi2;
                            }
                        }
                    }
                }
            }
            return null;
        }

        private Taxi GetTaxiThatGoesToDestination(Point travelTo, int travelToContinentId)
        {
            this._availableTaxis.Sort(delegate (Taxi x, Taxi y) {
                if (travelTo.DistanceTo(x.Position) >= travelTo.DistanceTo(y.Position))
                {
                    return 1;
                }
                return -1;
            });
            uint num = 0;
            foreach (Taxi taxi in this._availableTaxis)
            {
                if (num >= 3)
                {
                    break;
                }
                if (((taxi.Faction == Npc.FactionType.Neutral) || (taxi.Faction.ToString() == nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction)) && (((taxi.ContinentId == travelToContinentId) && this.IsTaxiLinkedFast(taxi)) && !this._unknownTaxis.Contains(taxi)))
                {
                    bool flag;
                    num++;
                    List<Point> source = PathFinder.FindPath(taxi.Position, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId), out flag, true, false, false);
                    if (flag || ((!flag && (source.Count >= 1)) && this.IsPointValidAsTarget(source.Last<Point>())))
                    {
                        return taxi;
                    }
                }
            }
            return null;
        }

        private List<Transport> GetTransportsThatDirectlyGoToDestination(Point travelTo, Point travelFrom, int travelToContinentId, int travelFromContinentId)
        {
            List<Transport> list = new List<Transport>();
            foreach (Transport transport in this.GetTransportsThatGoesToDestination(travelTo, travelToContinentId))
            {
                if (transport.ArrivalIsA)
                {
                    if (transport.BContinentId == travelFromContinentId)
                    {
                        bool flag;
                        PathFinder.FindPath(transport.BOutsidePoint, travelFrom, Usefuls.ContinentNameMpqByContinentId(travelFromContinentId), out flag, true, false, false);
                        if (flag)
                        {
                            list.Add(transport);
                        }
                    }
                }
                else if (transport.AContinentId == travelFromContinentId)
                {
                    bool flag2;
                    PathFinder.FindPath(transport.AOutsidePoint, travelFrom, Usefuls.ContinentNameMpqByContinentId(travelFromContinentId), out flag2, true, false, false);
                    if (flag2)
                    {
                        list.Add(transport);
                    }
                }
            }
            return list;
        }

        private List<Transport> GetTransportsThatGoesToDestination(Point travelTo, int travelToContinentId)
        {
            List<Transport> list = new List<Transport>();
            foreach (Transport transport in this._availableTransports.Items)
            {
                if (((transport.Faction == Npc.FactionType.Neutral) || (transport.Faction.ToString() == nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction)) && ((transport.AContinentId == travelToContinentId) || (transport.BContinentId == travelToContinentId)))
                {
                    if ((transport.AContinentId == travelToContinentId) && (transport.BContinentId != travelToContinentId))
                    {
                        bool flag;
                        PathFinder.FindPath(transport.AOutsidePoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId), out flag, true, false, false);
                        if (flag)
                        {
                            transport.ArrivalIsA = true;
                            list.Add(transport);
                        }
                    }
                    else if ((transport.BContinentId == travelToContinentId) && (transport.AContinentId != travelToContinentId))
                    {
                        bool flag2;
                        PathFinder.FindPath(transport.BOutsidePoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId), out flag2, true, false, false);
                        if (flag2)
                        {
                            list.Add(transport);
                        }
                    }
                    else if ((transport.AContinentId == travelToContinentId) && (transport.BContinentId == travelToContinentId))
                    {
                        bool flag3;
                        PathFinder.FindPath(transport.AOutsidePoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId), out flag3, true, false, false);
                        if (flag3)
                        {
                            transport.ArrivalIsA = true;
                            list.Add(transport);
                        }
                        PathFinder.FindPath(transport.BOutsidePoint, travelTo, Usefuls.ContinentNameMpqByContinentId(travelToContinentId), out flag3, true, false, false);
                        if (flag3)
                        {
                            transport.ArrivalIsA = false;
                            list.Add(transport);
                        }
                    }
                }
            }
            return list;
        }

        private void GoToDepartureQuayOrPortal(Transport selectedTransport)
        {
            if (selectedTransport is Portal)
            {
                Portal portal = selectedTransport as Portal;
                Logging.Write(string.Concat(new object[] { "Going to portal ", portal.Name, " (", portal.Id, ") to travel." }));
                MovementManager.Go(PathFinder.FindPath(portal.APoint));
                bool flag = true;
                while (flag)
                {
                    if (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat || nManager.Wow.ObjectManager.ObjectManager.Me.IsDead)
                    {
                        return;
                    }
                    if (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(portal.APoint) < 2f)
                    {
                        flag = false;
                    }
                    Thread.Sleep(100);
                }
                MovementManager.StopMove();
            }
            else if (selectedTransport is Taxi)
            {
                Taxi taxi = selectedTransport as Taxi;
                Logging.Write("Going to taxi " + taxi.Name + " to travel.");
                MovementManager.Go(PathFinder.FindPath(taxi.APoint));
                bool flag2 = true;
                while (flag2)
                {
                    if (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat || nManager.Wow.ObjectManager.ObjectManager.Me.IsDead)
                    {
                        return;
                    }
                    if (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(taxi.APoint) < 4f)
                    {
                        flag2 = false;
                    }
                    Thread.Sleep(100);
                }
                MovementManager.StopMove();
            }
            else
            {
                List<Point> points = selectedTransport.ArrivalIsA ? PathFinder.FindPath(selectedTransport.BOutsidePoint) : PathFinder.FindPath(selectedTransport.AOutsidePoint);
                MovementManager.Go(points);
                Logging.Write(string.Concat(new object[] { "Going to departure quay of ", selectedTransport.Name, "(", selectedTransport.Id, ") to travel." }));
                bool flag3 = true;
                while (flag3)
                {
                    if (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat || nManager.Wow.ObjectManager.ObjectManager.Me.IsDead)
                    {
                        return;
                    }
                    if (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(selectedTransport.ArrivalIsA ? selectedTransport.BOutsidePoint : selectedTransport.AOutsidePoint) < 2f)
                    {
                        flag3 = false;
                    }
                    if (!MovementManager.InMoveTo && !MovementManager.InMovement)
                    {
                        flag3 = false;
                    }
                    Thread.Sleep(100);
                }
                MovementManager.StopMove();
                Logging.Write(string.Concat(new object[] { "Arrived at departure quay of ", selectedTransport.Name, "(", selectedTransport.Id, "), waiting for transport." }));
            }
        }

        private bool IsPointValidAsTarget(Point position)
        {
            if (this.TargetValidationFct != null)
            {
                return this.TargetValidationFct(position);
            }
            return false;
        }

        private bool IsTaxiLinked(Taxi taxi)
        {
            return (this.FindNextTaxiHopFor(taxi, false) != null);
        }

        private bool IsTaxiLinkedFast(Taxi taxi)
        {
            using (List<TaxiLink>.Enumerator enumerator = this._availableTaxiLinks.FindAll(delegate (TaxiLink x) {
                if (x.PointA != taxi.Id)
                {
                    return x.PointB == taxi.Id;
                }
                return true;
            }).GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    Taxi taxi2;
                    Predicate<Taxi> match = null;
                    Predicate<Taxi> predicate2 = null;
                    TaxiLink lnk = enumerator.Current;
                    if (lnk.PointA == taxi.Id)
                    {
                        if (match == null)
                        {
                            match = x => x.Id == lnk.PointB;
                        }
                        taxi2 = this._availableTaxis.Find(match);
                    }
                    else
                    {
                        if (predicate2 == null)
                        {
                            predicate2 = x => x.Id == lnk.PointA;
                        }
                        taxi2 = this._availableTaxis.Find(predicate2);
                    }
                    if (((taxi2 != null) && (taxi2.Id != 0)) && ((taxi2.Faction == Npc.FactionType.Neutral) || (taxi2.Faction.ToString() == nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction)))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void LeaveTransport(Transport selectedTransport)
        {
            Logging.Write(string.Concat(new object[] { "Transport ", selectedTransport.Name, "(", selectedTransport.Id, ") arrived at destination, leaving to the arrival quay." }));
            MovementManager.MoveTo(selectedTransport.ArrivalIsA ? selectedTransport.AOutsidePoint : selectedTransport.BOutsidePoint, false);
            bool flag = true;
            while (flag)
            {
                if (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat || nManager.Wow.ObjectManager.ObjectManager.Me.IsDead)
                {
                    return;
                }
                if (!nManager.Wow.ObjectManager.ObjectManager.Me.InTransport)
                {
                    flag = false;
                }
                if (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(selectedTransport.ArrivalIsA ? selectedTransport.AOutsidePoint : selectedTransport.BOutsidePoint) < 5f)
                {
                    flag = false;
                }
                Thread.Sleep(500);
            }
        }

        public override void Run()
        {
            Logging.Write(string.Concat(new object[] { "Start travel from ", nManager.Wow.ObjectManager.ObjectManager.Me.Position, " ", Usefuls.ContinentNameMpqByContinentId(Usefuls.ContinentId), " to ", this.TravelTo, " ", Usefuls.ContinentNameMpqByContinentId(this.TravelToContinentId), "." }));
            MovementManager.StopMove();
            foreach (Transport transport in this._generatedRoutePath)
            {
                this.GoToDepartureQuayOrPortal(transport);
                if (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat || nManager.Wow.ObjectManager.ObjectManager.Me.IsDead)
                {
                    return;
                }
                if (!(transport is Portal) && !(transport is Taxi))
                {
                    this.WaitForTransport(transport);
                    if (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat || nManager.Wow.ObjectManager.ObjectManager.Me.IsDead)
                    {
                        return;
                    }
                }
                this.EnterTransportOrTakePortal(transport);
                if (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat || nManager.Wow.ObjectManager.ObjectManager.Me.IsDead)
                {
                    return;
                }
                if (transport is Taxi)
                {
                    TravelPatientlybyTaxiOrPortal(false);
                }
                else if (!(transport is Portal))
                {
                    this.TravelPatiently(transport);
                    if (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat || nManager.Wow.ObjectManager.ObjectManager.Me.IsDead)
                    {
                        return;
                    }
                    this.LeaveTransport(transport);
                }
            }
            this.TravelToContinentId = 0x98967f;
            this.TravelTo = new Point();
            this.TargetValidationFct = null;
            Logging.Write("Travel is terminated, waiting for product to take the control back.");
        }

        private void TravelPatiently(Transport selectedTransport)
        {
            WoWGameObject obj2 = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWGameObject(nManager.Wow.ObjectManager.ObjectManager.GetWoWGameObjectByEntry((int) selectedTransport.Id), nManager.Wow.ObjectManager.ObjectManager.Me.Position, false);
            bool flag = true;
            int num = 0;
            int num2 = 0;
            while (flag)
            {
                if ((!nManager.Wow.ObjectManager.ObjectManager.Me.InTransport && Usefuls.InGame) && !Usefuls.IsLoadingOrConnecting)
                {
                    if (num > 5)
                    {
                        flag = false;
                    }
                    num++;
                    Thread.Sleep(300);
                }
                if (selectedTransport.ArrivalIsA && obj2.Position.Equals(selectedTransport.APoint))
                {
                    flag = false;
                }
                if (!selectedTransport.ArrivalIsA && obj2.Position.Equals(selectedTransport.BPoint))
                {
                    flag = false;
                }
                if (!obj2.IsValid && (num2 < 5))
                {
                    obj2 = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWGameObject(nManager.Wow.ObjectManager.ObjectManager.GetWoWGameObjectByEntry((int) selectedTransport.Id), nManager.Wow.ObjectManager.ObjectManager.Me.Position, false);
                    num2++;
                }
                else if (!obj2.IsValid && (num2 >= 5))
                {
                    flag = false;
                }
                Thread.Sleep(500);
            }
        }

        public static void TravelPatientlybyTaxiOrPortal(bool ignoreCombatClass = false)
        {
            bool flag = true;
            Point position = nManager.Wow.ObjectManager.ObjectManager.Me.Position;
            while (flag)
            {
                Thread.Sleep(0x3e8);
                if (Usefuls.InGame && !Usefuls.IsLoadingOrConnecting)
                {
                    if (!nManager.Wow.ObjectManager.ObjectManager.Me.OnTaxi && (position.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) < 1f))
                    {
                        flag = false;
                    }
                    else
                    {
                        position = nManager.Wow.ObjectManager.ObjectManager.Me.Position;
                    }
                }
            }
            if (!ignoreCombatClass)
            {
                CombatClass.LoadCombatClass();
            }
        }

        private void WaitForTransport(Transport selectedTransport)
        {
            WoWGameObject obj2 = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWGameObject(nManager.Wow.ObjectManager.ObjectManager.GetWoWGameObjectByEntry((int) selectedTransport.Id), nManager.Wow.ObjectManager.ObjectManager.Me.Position, false);
            bool flag = true;
            while (flag)
            {
                if (Usefuls.IsFlying)
                {
                    MountTask.DismountMount(true);
                }
                if (obj2.IsValid)
                {
                    if ((selectedTransport.ArrivalIsA ? selectedTransport.BPoint : selectedTransport.APoint).Equals(obj2.Position))
                    {
                        flag = false;
                    }
                    if (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(selectedTransport.ArrivalIsA ? selectedTransport.BOutsidePoint : selectedTransport.AOutsidePoint) > 5f)
                    {
                        this.GoToDepartureQuayOrPortal(selectedTransport);
                        return;
                    }
                    Thread.Sleep(100);
                }
                else
                {
                    obj2 = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWGameObject(nManager.Wow.ObjectManager.ObjectManager.GetWoWGameObjectByEntry((int) selectedTransport.Id), nManager.Wow.ObjectManager.ObjectManager.Me.Position, false);
                }
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
                return "Travel";
            }
        }

        private List<Transport> GenerateRoutePath
        {
            get
            {
                Point travelTo = this.TravelTo;
                int travelToContinentId = this.TravelToContinentId;
                int continentId = Usefuls.ContinentId;
                Point position = nManager.Wow.ObjectManager.ObjectManager.Me.Position;
                KeyValuePair<Transport, float> pair = this.GetBestDirectWayTransport(position, travelTo, continentId, travelToContinentId);
                if (continentId == travelToContinentId)
                {
                    bool flag;
                    List<Point> source = PathFinder.FindPath(position, travelTo, Usefuls.ContinentNameMpq, out flag, true, false, false);
                    if ((flag || ((!flag && (source.Count >= 1)) && this.IsPointValidAsTarget(source.Last<Point>()))) && (pair.Value > nManager.Helpful.Math.DistanceListPoint(source)))
                    {
                        this.TravelToContinentId = 0x98967f;
                        this.TravelTo = new Point();
                        this.TargetValidationFct = null;
                        Logging.Write("Travel: Found a faster path without using Transports. Cancelling Travel.");
                        return new List<Transport>();
                    }
                }
                if (pair.Key.Id != 0)
                {
                    Logging.Write("Travel: Found direct way travel.");
                    return new List<Transport> { pair.Key };
                }
                KeyValuePair<List<Transport>, float> pair2 = this.GetBestTwoWayTransport(position, travelTo, continentId, travelToContinentId);
                if (((pair.Key.Id != 0) && (pair2.Key.Count == 2)) && (pair.Value <= pair2.Value))
                {
                    Logging.Write("Travel: Found a direct way travel that is faster than a 2-way travel.");
                    return new List<Transport> { pair.Key };
                }
                if (((pair.Key.Id != 0) && (pair2.Key.Count == 2)) && (pair.Value > pair2.Value))
                {
                    Logging.Write("Travel: Found a 2-way travel that is faster than a direct way travel.");
                    return pair2.Key;
                }
                this.TravelToContinentId = 0x98967f;
                this.TravelTo = new Point();
                this.TargetValidationFct = null;
                Logging.Write("Travel: Couldn't find a travel path. Checked up to 2 way travel.");
                return new List<Transport>();
            }
        }

        public override bool NeedToRun
        {
            get
            {
                if (this._availableTransports == null)
                {
                    this._availableTransports = XmlSerializer.Deserialize<Transports>(Application.StartupPath + @"\Data\TransportsDB.xml");
                }
                if (this._availablePortals == null)
                {
                    this._availablePortals = XmlSerializer.Deserialize<Portals>(Application.StartupPath + @"\Data\PortalsDB.xml");
                }
                if (this._availableTaxis == null)
                {
                    this._availableTaxis = XmlSerializer.Deserialize<List<Taxi>>(Application.StartupPath + @"\Data\TaxiList.xml");
                }
                if (this._availableTaxiLinks == null)
                {
                    this._availableTaxiLinks = XmlSerializer.Deserialize<List<TaxiLink>>(Application.StartupPath + @"\Data\TaxiLinks.xml");
                }
                if (((this._availableTransports == null) || (this._availablePortals == null)) || ((this._availableTaxis == null) || (this._availableTaxiLinks == null)))
                {
                    return false;
                }
                if (!nManager.Products.Products.IsStarted || !this.NeedToTravel)
                {
                    return false;
                }
                this._generatedRoutePath = this.GenerateRoutePath;
                return (this._generatedRoutePath.Count > 0);
            }
        }

        private bool NeedToTravel
        {
            get
            {
                return (this.TravelToContinentId != 0x98967f);
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

        public Func<Point, bool> TargetValidationFct
        {
            get
            {
                return nManager.Products.Products.TargetValidationFct;
            }
            set
            {
                nManager.Products.Products.TargetValidationFct = value;
            }
        }

        private Point TravelTo
        {
            get
            {
                return nManager.Products.Products.TravelTo;
            }
            set
            {
                nManager.Products.Products.TravelTo = value;
            }
        }

        private int TravelToContinentId
        {
            get
            {
                return nManager.Products.Products.TravelToContinentId;
            }
            set
            {
                nManager.Products.Products.TravelToContinentId = value;
            }
        }

        public class TaxiEventArgs : EventArgs
        {
            public int From { get; set; }

            public int To { get; set; }
        }
    }
}

