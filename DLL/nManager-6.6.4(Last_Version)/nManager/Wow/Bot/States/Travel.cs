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
        private Portals _anuahiuxeqajuhAxeoj;
        private List<Taxi> _fiohieweDe;
        private bool _ivitupewofi;
        private List<Taxi> _niewakoEkegoeta = new List<Taxi>();
        private CustomPaths _soigoti;
        private List<Transport> _tiesaCoe = new List<Transport>();
        private Transports _tuapugoal;
        private List<TaxiLink> _uvuqaduv;

        public static  event EventHandler AutomaticallyTookTaxi;

        private Taxi Ariaguxopeunad(Taxi heowubIrOwuodo, bool jaidosa = false)
        {
            List<List<uint>> list3;
            Predicate<Taxi> match = null;
            List<TaxiLink> list = this._uvuqaduv.ToList<TaxiLink>();
            uint num = heowubIrOwuodo.Id;
            uint endId = heowubIrOwuodo.EndOfPath;
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
                    <>c__DisplayClass10 class3;
                    uint currentHop = list4.Last<uint>();
                    foreach (TaxiLink link in list.FindAll(new Predicate<TaxiLink>(class3.<FindNextTaxiHopFor>b__7)))
                    {
                        <>c__DisplayClass12 class2;
                        list.Remove(link);
                        uint target = (link.PointA == currentHop) ? link.PointB : link.PointA;
                        Taxi item = this._fiohieweDe.Find(new Predicate<Taxi>(class2.<FindNextTaxiHopFor>b__8));
                        if ((((item != null) && (item.Id != 0)) && ((item.Faction == Npc.FactionType.Neutral) || (item.Faction.ToString() == nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction))) && !this._niewakoEkegoeta.Contains(item))
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
                            Taxi taxi2 = null;
                            float num3 = 0f;
                            using (List<uint>.Enumerator enumerator4 = list7.GetEnumerator())
                            {
                                while (enumerator4.MoveNext())
                                {
                                    <>c__DisplayClass18 class4;
                                    Predicate<Taxi> predicate = null;
                                    Predicate<Taxi> predicate2 = null;
                                    uint id = enumerator4.Current;
                                    if (taxi2 == null)
                                    {
                                        if (predicate == null)
                                        {
                                            predicate = new Predicate<Taxi>(class4.<FindNextTaxiHopFor>b__9);
                                        }
                                        taxi2 = this._fiohieweDe.Find(predicate);
                                    }
                                    else
                                    {
                                        if (predicate2 == null)
                                        {
                                            predicate2 = new Predicate<Taxi>(class4.<FindNextTaxiHopFor>b__a);
                                        }
                                        Taxi taxi3 = this._fiohieweDe.Find(predicate2);
                                        num3 += taxi2.Position.DistanceTo(taxi3.Position);
                                        taxi2 = taxi3;
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
                    if (jaidosa)
                    {
                        Logging.Write("Taxi travel plan: " + string.Join<uint>(", ", bestPathFound));
                    }
                    if (bestPathFound.Count > 1)
                    {
                        <>c__DisplayClass14 class5;
                        return this._fiohieweDe.Find(new Predicate<Taxi>(class5.<FindNextTaxiHopFor>b__b));
                    }
                    if (match == null)
                    {
                        <>c__DisplayClasse classe;
                        match = new Predicate<Taxi>(classe.<FindNextTaxiHopFor>b__c);
                    }
                    return this._fiohieweDe.Find(match);
                }
            }
            return null;
        }

        private List<Transport> Ceafi(Point duedainoinNiaga, Point loimacovuoUpip, int borekeoqiusosa, int toceutodCearakeoj)
        {
            List<Transport> list = new List<Transport>();
            foreach (Transport transport in this.UquewibiKa(duedainoinNiaga, borekeoqiusosa))
            {
                if (transport.ArrivalIsA)
                {
                    bool flag;
                    if (transport.BContinentId == toceutodCearakeoj)
                    {
                        PathFinder.FindPath(transport.BOutsidePoint, loimacovuoUpip, Usefuls.ContinentNameMpqByContinentId(toceutodCearakeoj), out flag, true, false, false);
                        if (flag)
                        {
                            list.Add(transport);
                            continue;
                        }
                    }
                    if (transport.BLift > 0)
                    {
                        transport.UseBLift = true;
                        Transport transport2 = this.Okudofikab(transport.BLift);
                        if (transport2.AContinentId == toceutodCearakeoj)
                        {
                            PathFinder.FindPath(((transport2 is CustomPath) || (transport2 is Portal)) ? transport2.APoint : transport2.AOutsidePoint, loimacovuoUpip, Usefuls.ContinentNameMpqByContinentId(toceutodCearakeoj), out flag, true, false, false);
                            if (flag)
                            {
                                list.Add(transport);
                                continue;
                            }
                        }
                        if ((!(transport2 is Portal) && (!(transport2 is CustomPath) || (transport2 as CustomPath).RoundTrip)) && (transport2.BContinentId == toceutodCearakeoj))
                        {
                            PathFinder.FindPath((transport2 is CustomPath) ? transport2.BPoint : transport2.BOutsidePoint, loimacovuoUpip, Usefuls.ContinentNameMpqByContinentId(toceutodCearakeoj), out flag, true, false, false);
                            if (flag)
                            {
                                list.Add(transport);
                            }
                        }
                    }
                }
                else
                {
                    bool flag2;
                    if (transport.AContinentId == toceutodCearakeoj)
                    {
                        PathFinder.FindPath(transport.AOutsidePoint, loimacovuoUpip, Usefuls.ContinentNameMpqByContinentId(toceutodCearakeoj), out flag2, true, false, false);
                        if (flag2)
                        {
                            list.Add(transport);
                            continue;
                        }
                    }
                    if (transport.ALift > 0)
                    {
                        transport.UseALift = true;
                        Transport transport3 = this.Okudofikab(transport.ALift);
                        if (transport3.AContinentId == toceutodCearakeoj)
                        {
                            PathFinder.FindPath(((transport3 is CustomPath) || (transport3 is Portal)) ? transport3.APoint : transport3.AOutsidePoint, loimacovuoUpip, Usefuls.ContinentNameMpqByContinentId(toceutodCearakeoj), out flag2, true, false, false);
                            if (flag2)
                            {
                                list.Add(transport);
                                continue;
                            }
                        }
                        if ((!(transport3 is Portal) && (!(transport3 is CustomPath) || (transport3 as CustomPath).RoundTrip)) && (transport3.BContinentId == toceutodCearakeoj))
                        {
                            PathFinder.FindPath((transport3 is CustomPath) ? transport3.BPoint : transport3.BOutsidePoint, loimacovuoUpip, Usefuls.ContinentNameMpqByContinentId(toceutodCearakeoj), out flag2, true, false, false);
                            if (flag2)
                            {
                                list.Add(transport);
                            }
                        }
                    }
                }
            }
            return list;
        }

        private Transport DedejiewonIf(Transport giqae)
        {
            bool flag;
            if (giqae is Taxi)
            {
                return new Transport();
            }
            if (((!(giqae is Portal) && !(giqae is CustomPath)) || ((giqae is CustomPath) && (giqae as CustomPath).RoundTrip)) && giqae.ArrivalIsA)
            {
                if (!giqae.UseBLift)
                {
                    return new Transport();
                }
                Transport transport = this.Okudofikab(giqae.BLift);
                PathFinder.FindPath(this.Foafupoisi(), (transport is CustomPath) ? transport.APoint : transport.AOutsidePoint, Usefuls.ContinentNameMpqByContinentId(this.Ugomiajo()), out flag, true, false, false);
                if (!flag)
                {
                    transport.ArrivalIsA = true;
                }
                return transport;
            }
            if (!giqae.UseALift)
            {
                return new Transport();
            }
            Transport transport2 = this.Okudofikab(giqae.ALift);
            PathFinder.FindPath(this.Foafupoisi(), ((transport2 is CustomPath) || (transport2 is Portal)) ? transport2.APoint : transport2.AOutsidePoint, Usefuls.ContinentNameMpqByContinentId(this.Ugomiajo()), out flag, true, false, false);
            if (!flag)
            {
                if (((transport2 is Portal) || (transport2 is CustomPath)) && (!(transport2 is CustomPath) || !(transport2 as CustomPath).RoundTrip))
                {
                    return transport2;
                }
                transport2.ArrivalIsA = true;
            }
            return transport2;
        }

        private KeyValuePair<Transport, float> EdokafauUbiIl(Point loimacovuoUpip, Point duedainoinNiaga, int toceutodCearakeoj, int borekeoqiusosa)
        {
            Transport key = new Transport();
            float maxValue = float.MaxValue;
            foreach (Transport transport2 in this.KedigutuijaopAheof(duedainoinNiaga, loimacovuoUpip, borekeoqiusosa, toceutodCearakeoj))
            {
                bool flag;
                List<Point> list4;
                List<Point> list6;
                float num4;
                float num5;
                float num2 = 0f;
                uint id = 0;
                if (transport2 is Taxi)
                {
                    Taxi taxi = transport2 as Taxi;
                    List<Point> list2 = PathFinder.FindPath(loimacovuoUpip, taxi.APoint, Usefuls.ContinentNameMpqByContinentId(toceutodCearakeoj));
                    List<Point> list3 = PathFinder.FindPath(taxi.BPoint, duedainoinNiaga, Usefuls.ContinentNameMpqByContinentId(borekeoqiusosa));
                    num2 = nManager.Helpful.Math.DistanceListPoint(list2) + nManager.Helpful.Math.DistanceListPoint(list3);
                    if (toceutodCearakeoj == borekeoqiusosa)
                    {
                        num2 += taxi.APoint.DistanceTo(taxi.BPoint) / 2.5f;
                    }
                    id = taxi.Id;
                    goto Label_081E;
                }
                List<Point> listPoints = new List<Point>();
                List<Point> list7 = new List<Point>();
                if (((transport2 is Portal) || ((transport2 is CustomPath) && !(transport2 as CustomPath).RoundTrip)) || !transport2.ArrivalIsA)
                {
                    goto Label_041F;
                }
                if (!transport2.UseBLift)
                {
                    list4 = PathFinder.FindPath(loimacovuoUpip, (transport2 is CustomPath) ? transport2.BPoint : transport2.BOutsidePoint, Usefuls.ContinentNameMpqByContinentId(toceutodCearakeoj), out flag, true, false, false);
                    if (flag)
                    {
                        goto Label_0287;
                    }
                    continue;
                }
                Transport transport3 = this.Okudofikab(transport2.BLift);
                list4 = PathFinder.FindPath(loimacovuoUpip, ((transport3 is Portal) || (transport3 is CustomPath)) ? transport3.APoint : transport3.AOutsidePoint, Usefuls.ContinentNameMpqByContinentId(toceutodCearakeoj), out flag, true, false, false);
                if ((flag && !(transport3 is Portal)) && (!(transport3 is CustomPath) || (transport3 as CustomPath).RoundTrip))
                {
                    listPoints = PathFinder.FindPath((transport2 is CustomPath) ? transport2.BPoint : transport2.BOutsidePoint, (transport3 is CustomPath) ? transport3.BPoint : transport3.BOutsidePoint, Usefuls.ContinentNameMpqByContinentId(transport2.BContinentId), out flag, true, false, false);
                    if (flag)
                    {
                        goto Label_0287;
                    }
                    continue;
                }
                list4 = PathFinder.FindPath(loimacovuoUpip, ((transport3 is Portal) || (transport3 is CustomPath)) ? transport3.BPoint : transport3.BOutsidePoint, Usefuls.ContinentNameMpqByContinentId(toceutodCearakeoj), out flag, true, false, false);
                if (!flag)
                {
                    continue;
                }
                listPoints = PathFinder.FindPath((transport2 is CustomPath) ? transport2.BPoint : transport2.BOutsidePoint, ((transport3 is Portal) || (transport3 is CustomPath)) ? transport3.APoint : transport3.AOutsidePoint, Usefuls.ContinentNameMpqByContinentId(transport2.BContinentId), out flag, true, false, false);
                if (!flag)
                {
                    continue;
                }
            Label_0287:
                if (!transport2.UseALift)
                {
                    list6 = PathFinder.FindPath(duedainoinNiaga, (transport2 is CustomPath) ? transport2.APoint : transport2.AOutsidePoint, Usefuls.ContinentNameMpqByContinentId(borekeoqiusosa), out flag, true, false, false);
                    if (flag)
                    {
                        goto Label_0791;
                    }
                }
                else
                {
                    Transport transport4 = this.Okudofikab(transport2.ALift);
                    list6 = PathFinder.FindPath(duedainoinNiaga, ((transport4 is Portal) || (transport4 is CustomPath)) ? transport4.APoint : transport4.AOutsidePoint, Usefuls.ContinentNameMpqByContinentId(borekeoqiusosa), out flag, true, false, false);
                    if ((flag && !(transport4 is Portal)) && (!(transport4 is CustomPath) || (transport4 as CustomPath).RoundTrip))
                    {
                        list7 = PathFinder.FindPath((transport2 is CustomPath) ? transport2.APoint : transport2.AOutsidePoint, (transport4 is CustomPath) ? transport4.BPoint : transport4.BOutsidePoint, Usefuls.ContinentNameMpqByContinentId(transport2.AContinentId), out flag, true, false, false);
                        if (flag)
                        {
                            goto Label_0791;
                        }
                    }
                    else
                    {
                        list6 = PathFinder.FindPath(duedainoinNiaga, ((transport4 is Portal) || (transport4 is CustomPath)) ? transport4.BPoint : transport4.BOutsidePoint, Usefuls.ContinentNameMpqByContinentId(borekeoqiusosa), out flag, true, false, false);
                        if (flag)
                        {
                            list7 = PathFinder.FindPath((transport2 is CustomPath) ? transport2.APoint : transport2.AOutsidePoint, ((transport4 is Portal) || (transport4 is CustomPath)) ? transport4.APoint : transport4.AOutsidePoint, Usefuls.ContinentNameMpqByContinentId(transport2.AContinentId), out flag, true, false, false);
                            if (flag)
                            {
                                goto Label_0791;
                            }
                        }
                    }
                }
                continue;
            Label_041F:
                if (!transport2.UseALift)
                {
                    list4 = PathFinder.FindPath(loimacovuoUpip, ((transport2 is CustomPath) || (transport2 is Portal)) ? transport2.APoint : transport2.AOutsidePoint, Usefuls.ContinentNameMpqByContinentId(toceutodCearakeoj), out flag, true, false, false);
                    if (flag)
                    {
                        goto Label_05CA;
                    }
                    continue;
                }
                Transport transport5 = this.Okudofikab(transport2.ALift);
                list4 = PathFinder.FindPath(loimacovuoUpip, ((transport5 is Portal) || (transport5 is CustomPath)) ? transport5.APoint : transport5.AOutsidePoint, Usefuls.ContinentNameMpqByContinentId(toceutodCearakeoj), out flag, true, false, false);
                if ((flag && !(transport5 is Portal)) && (!(transport5 is CustomPath) || (transport5 as CustomPath).RoundTrip))
                {
                    listPoints = PathFinder.FindPath(((transport2 is CustomPath) || (transport2 is Portal)) ? transport2.APoint : transport2.AOutsidePoint, (transport5 is CustomPath) ? transport5.BPoint : transport5.BOutsidePoint, Usefuls.ContinentNameMpqByContinentId(transport2.AContinentId), out flag, true, false, false);
                    if (flag)
                    {
                        goto Label_05CA;
                    }
                    continue;
                }
                list4 = PathFinder.FindPath(loimacovuoUpip, ((transport5 is Portal) || (transport5 is CustomPath)) ? transport5.BPoint : transport5.BOutsidePoint, Usefuls.ContinentNameMpqByContinentId(toceutodCearakeoj), out flag, true, false, false);
                if (!flag)
                {
                    continue;
                }
                listPoints = PathFinder.FindPath(((transport2 is CustomPath) || (transport2 is Portal)) ? transport2.APoint : transport2.AOutsidePoint, ((transport5 is Portal) || (transport5 is CustomPath)) ? transport5.APoint : transport5.AOutsidePoint, Usefuls.ContinentNameMpqByContinentId(transport2.AContinentId), out flag, true, false, false);
                if (!flag)
                {
                    continue;
                }
            Label_05CA:
                num4 = nManager.Helpful.Math.DistanceListPoint(list4) + nManager.Helpful.Math.DistanceListPoint(listPoints);
                if (num4 > maxValue)
                {
                    continue;
                }
                if (!transport2.UseBLift)
                {
                    list6 = PathFinder.FindPath(duedainoinNiaga, ((transport2 is CustomPath) || (transport2 is Portal)) ? transport2.BPoint : transport2.BOutsidePoint, Usefuls.ContinentNameMpqByContinentId(borekeoqiusosa), out flag, true, false, false);
                    if (flag)
                    {
                        goto Label_0791;
                    }
                    continue;
                }
                Transport transport6 = this.Okudofikab(transport2.BLift);
                list6 = PathFinder.FindPath(duedainoinNiaga, ((transport2 is CustomPath) || (transport2 is Portal)) ? transport2.APoint : transport2.AOutsidePoint, Usefuls.ContinentNameMpqByContinentId(borekeoqiusosa), out flag, true, false, false);
                if ((flag && !(transport6 is Portal)) && (!(transport6 is CustomPath) || (transport6 as CustomPath).RoundTrip))
                {
                    list7 = PathFinder.FindPath(((transport2 is CustomPath) || (transport2 is Portal)) ? transport2.BPoint : transport2.BOutsidePoint, (transport6 is CustomPath) ? transport6.BPoint : transport6.BOutsidePoint, Usefuls.ContinentNameMpqByContinentId(transport2.BContinentId), out flag, true, false, false);
                    if (flag)
                    {
                        goto Label_0791;
                    }
                    continue;
                }
                list6 = PathFinder.FindPath(duedainoinNiaga, ((transport6 is Portal) || (transport6 is CustomPath)) ? transport6.BPoint : transport6.BOutsidePoint, Usefuls.ContinentNameMpqByContinentId(borekeoqiusosa), out flag, true, false, false);
                if (!flag)
                {
                    continue;
                }
                list7 = PathFinder.FindPath(((transport2 is CustomPath) || (transport2 is Portal)) ? transport2.BPoint : transport2.BOutsidePoint, ((transport6 is Portal) || (transport6 is CustomPath)) ? transport6.APoint : transport6.AOutsidePoint, Usefuls.ContinentNameMpqByContinentId(transport2.BContinentId), out flag, true, false, false);
                if (!flag)
                {
                    continue;
                }
            Label_0791:
                num5 = 0f;
                if (transport2.Id == 0x2e855)
                {
                    num5 = (((transport2 is CustomPath) || (transport2 is Portal)) ? transport2.APoint : transport2.AOutsidePoint).DistanceTo2D(((transport2 is CustomPath) || (transport2 is Portal)) ? transport2.BPoint : transport2.BOutsidePoint);
                }
                num2 = (((num5 + nManager.Helpful.Math.DistanceListPoint(list4)) + nManager.Helpful.Math.DistanceListPoint(listPoints)) + nManager.Helpful.Math.DistanceListPoint(list6)) + nManager.Helpful.Math.DistanceListPoint(list7);
                id = transport2.Id;
            Label_081E:
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

        private bool Ejijeokatova(Transport ovoiwulofabu)
        {
            if (ovoiwulofabu is Portal)
            {
                Portal portal = ovoiwulofabu as Portal;
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
                                if (nManager.Wow.ObjectManager.ObjectManager.Me.InInevitableCombat || nManager.Wow.ObjectManager.ObjectManager.Me.IsDead)
                                {
                                    return false;
                                }
                                Thread.Sleep(150);
                            }
                        }
                        MountTask.DismountMount(true);
                        Thread.Sleep(500);
                        Interact.InteractWith(obj2.GetBaseAddress, false);
                        Thread.Sleep(150);
                        Interact.InteractWith(obj2.GetBaseAddress, false);
                        Thread.Sleep(300);
                        while (nManager.Wow.ObjectManager.ObjectManager.Me.IsCasting)
                        {
                            Thread.Sleep(100);
                        }
                        TravelPatientlybyTaxiOrPortal(false);
                        flag = false;
                    }
                    else
                    {
                        obj2 = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWGameObject(nManager.Wow.ObjectManager.ObjectManager.GetWoWGameObjectByEntry((int) portal.Id), nManager.Wow.ObjectManager.ObjectManager.Me.Position, false);
                        if (portal.AContinentId == portal.BContinentId)
                        {
                            if (portal.APoint.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) >= portal.BPoint.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position))
                            {
                                return false;
                            }
                            if (portal.APoint.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) > 4f)
                            {
                                this.Eresunaowep(ovoiwulofabu, false);
                                this.Ejijeokatova(ovoiwulofabu);
                                return false;
                            }
                        }
                        else
                        {
                            if (Usefuls.ContinentId == portal.BContinentId)
                            {
                                return false;
                            }
                            if (portal.APoint.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) > 4f)
                            {
                                this.Eresunaowep(ovoiwulofabu, false);
                                this.Ejijeokatova(ovoiwulofabu);
                                return false;
                            }
                        }
                    }
                }
            }
            else if (ovoiwulofabu is CustomPath)
            {
                CustomPath path = ovoiwulofabu as CustomPath;
                for (bool flag2 = true; flag2; flag2 = false)
                {
                    if ((path.ArrivalIsA ? path.BPoint : path.APoint).DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) > 4f)
                    {
                        bool flag3;
                        bool flag4;
                        List<Point> listPoints = PathFinder.FindPath(path.ArrivalIsA ? path.APoint : path.BPoint, out flag3, true, false);
                        List<Point> list3 = PathFinder.FindPath(path.ArrivalIsA ? path.BPoint : path.APoint, out flag4, true, false);
                        if (flag3)
                        {
                            if (!flag4)
                            {
                                return false;
                            }
                            if (nManager.Helpful.Math.DistanceListPoint(listPoints) <= nManager.Helpful.Math.DistanceListPoint(list3))
                            {
                                return false;
                            }
                        }
                        this.Eresunaowep(ovoiwulofabu, false);
                        this.Ejijeokatova(ovoiwulofabu);
                        return false;
                    }
                    List<Point> points = path.Points;
                    if (path.ArrivalIsA)
                    {
                        List<Point> list5 = new List<Point>();
                        list5.AddRange(path.Points);
                        list5.Reverse();
                        points = list5;
                    }
                    if (points.Count > 0)
                    {
                        Point point = points[0];
                        if (path.UseMount && ((PathFinder.GetZPosition(point, false) + 20f) < point.Z))
                        {
                            if (MountTask.GetMountCapacity() >= MountCapacity.Fly)
                            {
                                MountTask.Mount(true, true);
                                MountTask.Takeoff();
                            }
                        }
                        else if (path.UseMount)
                        {
                            MountTask.Mount(true, false);
                        }
                    }
                    bool flag5 = true;
                    if (!path.UseMount)
                    {
                        if (MountTask.AllowMounting)
                        {
                            MountTask.AllowMounting = false;
                        }
                        else
                        {
                            flag5 = false;
                        }
                        MountTask.DismountMount(true);
                    }
                    Thread.Sleep(500);
                    MovementManager.Go(points);
                    while (MovementManager.InMovement)
                    {
                        if ((!nManager.Products.Products.IsStarted || nManager.Wow.ObjectManager.ObjectManager.Me.InInevitableCombat) || nManager.Wow.ObjectManager.ObjectManager.Me.IsDead)
                        {
                            return false;
                        }
                        Thread.Sleep(150);
                    }
                    if (!path.UseMount && flag5)
                    {
                        MountTask.AllowMounting = true;
                    }
                }
            }
            else if (ovoiwulofabu is Taxi)
            {
                Taxi heowubIrOwuodo = ovoiwulofabu as Taxi;
                WoWUnit unit = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWUnit(nManager.Wow.ObjectManager.ObjectManager.GetWoWUnitByEntry((int) heowubIrOwuodo.Id, false), false, false, false);
                bool flag6 = true;
                while (flag6)
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
                                if (nManager.Wow.ObjectManager.ObjectManager.Me.InInevitableCombat || nManager.Wow.ObjectManager.ObjectManager.Me.IsDead)
                                {
                                    return false;
                                }
                                Thread.Sleep(150);
                            }
                        }
                        Taxi item = this.Ariaguxopeunad(heowubIrOwuodo, true);
                        if (item == null)
                        {
                            Logging.Write("There is a problem with taxi links, some are missing to complete the minimal graph");
                            return false;
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
                            return false;
                        }
                        this._niewakoEkegoeta.Remove(heowubIrOwuodo);
                        if (!this._ivitupewofi)
                        {
                            this._ivitupewofi = true;
                            List<Taxi> allTaxisAvailable = Gossip.GetAllTaxisAvailable();
                            using (List<Taxi>.Enumerator enumerator = this._fiohieweDe.GetEnumerator())
                            {
                                while (enumerator.MoveNext())
                                {
                                    Predicate<Taxi> match = null;
                                    Taxi oneTaxi = enumerator.Current;
                                    if (match == null)
                                    {
                                        <>c__DisplayClass4 class2;
                                        match = new Predicate<Taxi>(class2.<EnterTransportOrTakePortal>b__2);
                                    }
                                    Taxi taxi3 = allTaxisAvailable.Find(match);
                                    if (((taxi3 == null) || (taxi3.Xcoord == "")) && ((oneTaxi.Faction == Npc.FactionType.Neutral) || (oneTaxi.Faction.ToString() == nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction)))
                                    {
                                        this._niewakoEkegoeta.Add(oneTaxi);
                                    }
                                }
                            }
                        }
                        CombatClass.DisposeCombatClass();
                        Taxi taxi4 = this.UkufuiciujieAxeApov(heowubIrOwuodo.EndOfPath);
                        if ((taxi4.Id != item.Id) && !this._niewakoEkegoeta.Contains(taxi4))
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                Gossip.TakeTaxi(taxi4.Xcoord, taxi4.Ycoord);
                                Thread.Sleep(0x7d0);
                            }
                            if (nManager.Wow.ObjectManager.ObjectManager.Me.OnTaxi)
                            {
                                if (_eboureivaexiaWufucono != null)
                                {
                                    TaxiEventArgs e = new TaxiEventArgs {
                                        From = unit.Entry,
                                        To = (int) taxi4.Id
                                    };
                                    _eboureivaexiaWufucono(this, e);
                                }
                                Logging.Write("Flying directly to " + item.Name);
                                return false;
                            }
                        }
                        if (this._niewakoEkegoeta.Contains(item))
                        {
                            Logging.Write("Cannot fly to " + item.Name + " yet, releasing travel.");
                            return true;
                        }
                        Gossip.TakeTaxi(item.Xcoord, item.Ycoord);
                        if (_eboureivaexiaWufucono != null)
                        {
                            TaxiEventArgs args2 = new TaxiEventArgs {
                                From = unit.Entry,
                                To = (int) item.Id
                            };
                            _eboureivaexiaWufucono(this, args2);
                        }
                        Logging.Write("Flying to " + item.Name);
                        flag6 = false;
                    }
                    else
                    {
                        if (heowubIrOwuodo.APoint.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) > 4f)
                        {
                            this.Eresunaowep(ovoiwulofabu, false);
                            this.Ejijeokatova(ovoiwulofabu);
                            return false;
                        }
                        unit = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWUnit(nManager.Wow.ObjectManager.ObjectManager.GetWoWUnitByEntry((int) heowubIrOwuodo.Id, false), false, false, false);
                    }
                }
            }
            else
            {
                Logging.Write(string.Concat(new object[] { "Transport ", ovoiwulofabu.Name, "(", ovoiwulofabu.Id, ") arrived at the quay, entering transport." }));
                while (!nManager.Wow.ObjectManager.ObjectManager.Me.InTransport)
                {
                    if ((nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(ovoiwulofabu.ArrivalIsA ? ovoiwulofabu.BOutsidePoint : ovoiwulofabu.AOutsidePoint) > 10f) && (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(ovoiwulofabu.ArrivalIsA ? ovoiwulofabu.BInsidePoint : ovoiwulofabu.AInsidePoint) > nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(ovoiwulofabu.ArrivalIsA ? ovoiwulofabu.BOutsidePoint : ovoiwulofabu.AOutsidePoint)))
                    {
                        Logging.Write(string.Concat(new object[] { "Failed to enter transport ", ovoiwulofabu.Name, "(", ovoiwulofabu.Id, ") going back to the quay." }));
                        this.Eresunaowep(ovoiwulofabu, false);
                        this.Ejijeokatova(ovoiwulofabu);
                    }
                    MovementManager.MoveTo(ovoiwulofabu.ArrivalIsA ? ovoiwulofabu.BInsidePoint : ovoiwulofabu.AInsidePoint, false);
                    bool flag7 = true;
                    while (flag7)
                    {
                        if (nManager.Wow.ObjectManager.ObjectManager.Me.InInevitableCombat || nManager.Wow.ObjectManager.ObjectManager.Me.IsDead)
                        {
                            return false;
                        }
                        if (nManager.Wow.ObjectManager.ObjectManager.Me.InTransport)
                        {
                            flag7 = false;
                            Thread.Sleep(0x3e8);
                        }
                        if (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(ovoiwulofabu.ArrivalIsA ? ovoiwulofabu.BInsidePoint : ovoiwulofabu.AInsidePoint) <= 2f)
                        {
                            flag7 = false;
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
                Logging.Write(string.Concat(new object[] { "Successfuly entered transport ", ovoiwulofabu.Name, "(", ovoiwulofabu.Id, "), waiting to arrive at destination." }));
            }
            return false;
        }

        private void Eresunaowep(Transport ovoiwulofabu, bool ucoufagalera = false)
        {
            MovementManager.StopMove();
            if (ovoiwulofabu is Portal)
            {
                Portal portal = ovoiwulofabu as Portal;
                if (!ucoufagalera)
                {
                    Logging.Write(string.Concat(new object[] { "Going to portal ", portal.Name, " (", portal.Id, ") to travel." }));
                }
                MovementManager.Go(PathFinder.FindPath(portal.APoint));
                bool flag = true;
                while (flag)
                {
                    if ((!nManager.Products.Products.IsStarted || nManager.Wow.ObjectManager.ObjectManager.Me.InInevitableCombat) || nManager.Wow.ObjectManager.ObjectManager.Me.IsDead)
                    {
                        return;
                    }
                    if (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(portal.APoint) < 2f)
                    {
                        flag = false;
                    }
                    if (!MovementManager.InMoveTo && !MovementManager.InMovement)
                    {
                        flag = false;
                    }
                    Thread.Sleep(100);
                }
                MovementManager.StopMove();
                if (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(portal.APoint) >= 2f)
                {
                    this.Eresunaowep(ovoiwulofabu, true);
                }
            }
            else if (ovoiwulofabu is CustomPath)
            {
                CustomPath path = ovoiwulofabu as CustomPath;
                if (!ucoufagalera)
                {
                    Logging.Write(string.Concat(new object[] { "Going to CustomPath ", path.Name, " (", path.Id, ") to travel." }));
                }
                MovementManager.Go(PathFinder.FindPath(path.ArrivalIsA ? path.BPoint : path.APoint));
                bool flag2 = true;
                while (flag2)
                {
                    if ((!nManager.Products.Products.IsStarted || nManager.Wow.ObjectManager.ObjectManager.Me.InInevitableCombat) || nManager.Wow.ObjectManager.ObjectManager.Me.IsDead)
                    {
                        return;
                    }
                    if (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(path.ArrivalIsA ? path.BPoint : path.APoint) < 2f)
                    {
                        flag2 = false;
                    }
                    if (!MovementManager.InMoveTo && !MovementManager.InMovement)
                    {
                        flag2 = false;
                    }
                    Thread.Sleep(100);
                }
                MovementManager.StopMove();
                if (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(path.ArrivalIsA ? path.BPoint : path.APoint) >= 2f)
                {
                    this.Eresunaowep(ovoiwulofabu, true);
                }
            }
            else if (ovoiwulofabu is Taxi)
            {
                Taxi taxi = ovoiwulofabu as Taxi;
                if (!ucoufagalera)
                {
                    Logging.Write("Going to taxi " + taxi.Name + " to travel.");
                }
                if ((((Usefuls.ContinentId == 0x4c4) && (Usefuls.AreaId != 0x1d4e)) && (Usefuls.IsOutdoors && (taxi.APoint.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) > 100f))) && (((ItemsManager.GetItemCount(0x22925) > 0) && ItemsManager.IsItemUsable(0x22925)) && !ItemsManager.IsItemOnCooldown(0x22925)))
                {
                    ItemsManager.UseItem(0x22925);
                    Thread.Sleep(250);
                    while (nManager.Wow.ObjectManager.ObjectManager.Me.IsCasting)
                    {
                        Thread.Sleep(250);
                        if ((!nManager.Products.Products.IsStarted || nManager.Wow.ObjectManager.ObjectManager.Me.InInevitableCombat) || nManager.Wow.ObjectManager.ObjectManager.Me.IsDead)
                        {
                            return;
                        }
                    }
                    if ((nManager.Products.Products.IsStarted && !nManager.Wow.ObjectManager.ObjectManager.Me.InInevitableCombat) && !nManager.Wow.ObjectManager.ObjectManager.Me.IsDead)
                    {
                        Thread.Sleep(0x2710);
                        this.IkodoApejoesDirefatui(0x98967f);
                        this.Nikisoego(new Point());
                        this.AfahueHoeha(new Point());
                        this.ItobikepGeuImiat(false);
                        this.TargetValidationFct = null;
                        this._tiesaCoe = new List<Transport>();
                        Logging.Write("We've used Flight Master Wistle, waiting for product to regenerate travel path.");
                    }
                }
                else
                {
                    MovementManager.Go(PathFinder.FindPath(taxi.APoint));
                    bool flag3 = true;
                    while (flag3)
                    {
                        if ((!nManager.Products.Products.IsStarted || nManager.Wow.ObjectManager.ObjectManager.Me.InInevitableCombat) || nManager.Wow.ObjectManager.ObjectManager.Me.IsDead)
                        {
                            return;
                        }
                        if (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(taxi.APoint) < 4f)
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
                    if (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(taxi.APoint) >= 4f)
                    {
                        this.Eresunaowep(ovoiwulofabu, true);
                    }
                }
            }
            else
            {
                List<Point> points = ovoiwulofabu.ArrivalIsA ? PathFinder.FindPath(ovoiwulofabu.BOutsidePoint) : PathFinder.FindPath(ovoiwulofabu.AOutsidePoint);
                MovementManager.Go(points);
                if (!ucoufagalera)
                {
                    Logging.Write(string.Concat(new object[] { "Going to departure quay of ", ovoiwulofabu.Name, "(", ovoiwulofabu.Id, ") to travel." }));
                }
                bool flag4 = true;
                while (flag4)
                {
                    if ((!nManager.Products.Products.IsStarted || nManager.Wow.ObjectManager.ObjectManager.Me.InInevitableCombat) || nManager.Wow.ObjectManager.ObjectManager.Me.IsDead)
                    {
                        return;
                    }
                    if (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(ovoiwulofabu.ArrivalIsA ? ovoiwulofabu.BOutsidePoint : ovoiwulofabu.AOutsidePoint) < 2f)
                    {
                        flag4 = false;
                    }
                    if (!MovementManager.InMoveTo && !MovementManager.InMovement)
                    {
                        flag4 = false;
                    }
                    Thread.Sleep(100);
                }
                MovementManager.StopMove();
                if (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(ovoiwulofabu.ArrivalIsA ? ovoiwulofabu.BOutsidePoint : ovoiwulofabu.AOutsidePoint) < 2f)
                {
                    Logging.Write(string.Concat(new object[] { "Arrived at departure quay of ", ovoiwulofabu.Name, "(", ovoiwulofabu.Id, "), waiting for transport." }));
                }
                else
                {
                    this.Eresunaowep(ovoiwulofabu, true);
                }
            }
        }

        private void Eteoveo(Transport ovoiwulofabu)
        {
            WoWGameObject obj2 = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWGameObject(nManager.Wow.ObjectManager.ObjectManager.GetWoWGameObjectByEntry((int) ovoiwulofabu.Id), nManager.Wow.ObjectManager.ObjectManager.Me.Position, false);
            bool flag = true;
            int num = 0;
            int num2 = 0;
            while (flag)
            {
                if (nManager.Products.Products.InAutoPause)
                {
                    num = 0;
                    num2 = 0;
                    Thread.Sleep(500);
                }
                else
                {
                    if ((!nManager.Wow.ObjectManager.ObjectManager.Me.InTransport && Usefuls.InGame) && !Usefuls.IsLoading)
                    {
                        if (num > 5)
                        {
                            flag = false;
                        }
                        num++;
                        Thread.Sleep(300);
                    }
                    if (ovoiwulofabu.ArrivalIsA && obj2.Position.Equals(ovoiwulofabu.APoint))
                    {
                        flag = false;
                    }
                    if (!ovoiwulofabu.ArrivalIsA && obj2.Position.Equals(ovoiwulofabu.BPoint))
                    {
                        flag = false;
                    }
                    if (!obj2.IsValid && (num2 < 5))
                    {
                        obj2 = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWGameObject(nManager.Wow.ObjectManager.ObjectManager.GetWoWGameObjectByEntry((int) ovoiwulofabu.Id), nManager.Wow.ObjectManager.ObjectManager.Me.Position, false);
                        num2++;
                    }
                    else if (!obj2.IsValid && (num2 >= 5))
                    {
                        flag = false;
                    }
                    Thread.Sleep(500);
                }
            }
        }

        private bool EvesavuqomoejeRoapobiv(Taxi heowubIrOwuodo)
        {
            return (this.Ariaguxopeunad(heowubIrOwuodo, false) != null);
        }

        private List<CustomPath> Fuduvoalue(Point duedainoinNiaga, int borekeoqiusosa, Point loimacovuoUpip, int toceutodCearakeoj)
        {
            List<CustomPath> list = new List<CustomPath>();
            foreach (CustomPath path in this._soigoti.Items)
            {
                if (((((path.Faction == Npc.FactionType.Neutral) || (path.Faction.ToString() == nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction)) && ((path.AContinentId == borekeoqiusosa) || (path.BContinentId == borekeoqiusosa))) && (((path.AContinentId == toceutodCearakeoj) || (path.BContinentId == toceutodCearakeoj)) && ((path.AContinentId != borekeoqiusosa) || (path.BContinentId == toceutodCearakeoj)))) && ((((path.BContinentId != borekeoqiusosa) || (path.AContinentId == toceutodCearakeoj)) && (((path.AContinentId != borekeoqiusosa) || path.AllowFar) || (duedainoinNiaga.DistanceTo(path.APoint) <= 2000f))) && (((path.BContinentId != borekeoqiusosa) || path.AllowFar) || (duedainoinNiaga.DistanceTo(path.BPoint) <= 2000f))))
                {
                    bool flag;
                    float num = duedainoinNiaga.DistanceTo(loimacovuoUpip);
                    if (((path.AContinentId == borekeoqiusosa) && (path.BContinentId == toceutodCearakeoj)) && (path.APoint.DistanceTo(duedainoinNiaga) < path.BPoint.DistanceTo(duedainoinNiaga)))
                    {
                        if ((path.AContinentId != path.BContinentId) || (duedainoinNiaga.DistanceTo(path.APoint) <= (num + 1000f)))
                        {
                            PathFinder.FindPath(path.APoint, duedainoinNiaga, Usefuls.ContinentNameMpqByContinentId(borekeoqiusosa), out flag, true, false, false);
                            if (flag && path.RoundTrip)
                            {
                                path.ArrivalIsA = true;
                                list.Add(path);
                            }
                        }
                    }
                    else if (((path.BContinentId == borekeoqiusosa) && (path.AContinentId == toceutodCearakeoj)) && ((path.AContinentId != path.BContinentId) || (duedainoinNiaga.DistanceTo(path.BPoint) <= (num + 1000f))))
                    {
                        PathFinder.FindPath(path.BPoint, duedainoinNiaga, Usefuls.ContinentNameMpqByContinentId(borekeoqiusosa), out flag, true, false, false);
                        if (flag)
                        {
                            path.ArrivalIsA = false;
                            list.Add(path);
                        }
                    }
                }
            }
            return list;
        }

        private bool HekuohiIhuafio(Point acekeAcepok)
        {
            if (this.TargetValidationFct != null)
            {
                return this.TargetValidationFct(acekeAcepok);
            }
            return false;
        }

        private bool IjiewouluUt(Taxi heowubIrOwuodo)
        {
            <>c__DisplayClass1d classd;
            Taxi taxi = heowubIrOwuodo;
            using (List<TaxiLink>.Enumerator enumerator = this._uvuqaduv.FindAll(new Predicate<TaxiLink>(classd.<IsTaxiLinkedFast>b__1a)).GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    Taxi taxi;
                    <>c__DisplayClass21 class2;
                    Predicate<Taxi> match = null;
                    Predicate<Taxi> predicate2 = null;
                    TaxiLink lnk = enumerator.Current;
                    if (lnk.PointA == taxi.Id)
                    {
                        if (match == null)
                        {
                            match = new Predicate<Taxi>(class2.<IsTaxiLinkedFast>b__1b);
                        }
                        taxi = this._fiohieweDe.Find(match);
                    }
                    else
                    {
                        if (predicate2 == null)
                        {
                            predicate2 = new Predicate<Taxi>(class2.<IsTaxiLinkedFast>b__1c);
                        }
                        taxi = this._fiohieweDe.Find(predicate2);
                    }
                    if (((taxi != null) && (taxi.Id != 0)) && ((taxi.Faction == Npc.FactionType.Neutral) || (taxi.Faction.ToString() == nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction)))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private List<Portal> IleworevadojUdea(Point duedainoinNiaga, Point loimacovuoUpip, int borekeoqiusosa, int toceutodCearakeoj)
        {
            List<Portal> list = new List<Portal>();
            foreach (Portal portal in this.Qiwuo(duedainoinNiaga, borekeoqiusosa))
            {
                if (portal.AContinentId == toceutodCearakeoj)
                {
                    bool flag;
                    PathFinder.FindPath(portal.APoint, loimacovuoUpip, Usefuls.ContinentNameMpqByContinentId(toceutodCearakeoj), out flag, true, false, false);
                    if (flag)
                    {
                        list.Add(portal);
                    }
                }
            }
            return list;
        }

        private List<Transport> KedigutuijaopAheof(Point duedainoinNiaga, Point loimacovuoUpip, int borekeoqiusosa, int toceutodCearakeoj)
        {
            List<Transport> list = new List<Transport>();
            List<Transport> collection = this.Ceafi(duedainoinNiaga, loimacovuoUpip, borekeoqiusosa, toceutodCearakeoj);
            List<Portal> list3 = this.IleworevadojUdea(duedainoinNiaga, loimacovuoUpip, borekeoqiusosa, toceutodCearakeoj);
            List<CustomPath> list4 = this.Leiciukup(duedainoinNiaga, loimacovuoUpip, borekeoqiusosa, toceutodCearakeoj);
            Taxi item = this.LumoisuiruotOg(duedainoinNiaga, loimacovuoUpip, borekeoqiusosa, toceutodCearakeoj);
            list.AddRange(collection);
            list.AddRange(list4);
            list.AddRange(list3);
            if (item != null)
            {
                list.Add(item);
            }
            return list;
        }

        private List<CustomPath> Leiciukup(Point duedainoinNiaga, Point loimacovuoUpip, int borekeoqiusosa, int toceutodCearakeoj)
        {
            List<CustomPath> list = new List<CustomPath>();
            foreach (CustomPath path in this.Fuduvoalue(duedainoinNiaga, borekeoqiusosa, loimacovuoUpip, toceutodCearakeoj))
            {
                if (path.ArrivalIsA)
                {
                    if (path.BContinentId == toceutodCearakeoj)
                    {
                        bool flag;
                        PathFinder.FindPath(path.BPoint, loimacovuoUpip, Usefuls.ContinentNameMpqByContinentId(toceutodCearakeoj), out flag, true, false, false);
                        if (flag)
                        {
                            list.Add(path);
                        }
                    }
                }
                else if (path.AContinentId == toceutodCearakeoj)
                {
                    bool flag2;
                    PathFinder.FindPath(path.APoint, loimacovuoUpip, Usefuls.ContinentNameMpqByContinentId(toceutodCearakeoj), out flag2, true, false, false);
                    if (flag2)
                    {
                        list.Add(path);
                    }
                }
            }
            return list;
        }

        private Taxi LumoisuiruotOg(Point duedainoinNiaga, Point loimacovuoUpip, int borekeoqiusosa, int toceutodCearakeoj)
        {
            Point travelFrom = loimacovuoUpip;
            Taxi taxi = this.OxibixexujehOkeo(duedainoinNiaga, borekeoqiusosa);
            if (taxi != null)
            {
                <>c__DisplayClass28 class2;
                this._fiohieweDe.Sort(new Comparison<Taxi>(class2.<GetTaxisThatDirectlyGoToDestination>b__27));
                uint num = 0;
                for (int i = 0; i < this._fiohieweDe.Count; i++)
                {
                    Taxi heowubIrOwuodo = this.SopoimeAkuofafu(this._fiohieweDe[i]);
                    if (num >= 3)
                    {
                        break;
                    }
                    if (heowubIrOwuodo.Position == taxi.Position)
                    {
                        return null;
                    }
                    if (((heowubIrOwuodo.Faction == Npc.FactionType.Neutral) || (heowubIrOwuodo.Faction.ToString() == nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction)) && (heowubIrOwuodo.ContinentId == toceutodCearakeoj))
                    {
                        heowubIrOwuodo.EndOfPath = taxi.Id;
                        if (this.EvesavuqomoejeRoapobiv(heowubIrOwuodo))
                        {
                            bool flag;
                            num++;
                            PathFinder.FindPath(heowubIrOwuodo.Position, travelFrom, Usefuls.ContinentNameMpqByContinentId(toceutodCearakeoj), out flag, true, false, false);
                            if (flag)
                            {
                                heowubIrOwuodo.APoint = heowubIrOwuodo.Position;
                                heowubIrOwuodo.BPoint = taxi.Position;
                                return heowubIrOwuodo;
                            }
                        }
                    }
                }
            }
            return null;
        }

        private KeyValuePair<Transport, float> Mumeo(Point loimacovuoUpip, Point duedainoinNiaga, int toceutodCearakeoj, int borekeoqiusosa)
        {
            bool flag;
            Taxi taxi = this.LumoisuiruotOg(duedainoinNiaga, loimacovuoUpip, borekeoqiusosa, toceutodCearakeoj);
            if (taxi == null)
            {
                return new KeyValuePair<Transport, float>(new Transport(), float.MaxValue);
            }
            List<Point> listPoints = PathFinder.FindPath(loimacovuoUpip, taxi.APoint, Usefuls.ContinentNameMpqByContinentId(toceutodCearakeoj), out flag, true, false, false);
            if (!flag)
            {
                return new KeyValuePair<Transport, float>(new Transport(), float.MaxValue);
            }
            List<Point> list2 = PathFinder.FindPath(taxi.BPoint, duedainoinNiaga, Usefuls.ContinentNameMpqByContinentId(borekeoqiusosa), out flag, true, false, false);
            if (!flag)
            {
                return new KeyValuePair<Transport, float>(new Transport(), float.MaxValue);
            }
            float num = nManager.Helpful.Math.DistanceListPoint(listPoints) + nManager.Helpful.Math.DistanceListPoint(list2);
            if (toceutodCearakeoj == borekeoqiusosa)
            {
                num += taxi.APoint.DistanceTo(taxi.BPoint) / 2.5f;
            }
            Transport key = taxi;
            key.Id = taxi.Id;
            if (key.Id == 0)
            {
                return new KeyValuePair<Transport, float>(new Transport(), float.MaxValue);
            }
            return new KeyValuePair<Transport, float>(key, num);
        }

        private Transport Okudofikab(uint baexodekusuidMeilio)
        {
            foreach (Transport transport in this._tuapugoal.Items)
            {
                if (transport.Id == baexodekusuidMeilio)
                {
                    return transport;
                }
            }
            return new Transport();
        }

        private Taxi OxibixexujehOkeo(Point duedainoinNiaga, int borekeoqiusosa)
        {
            <>c__DisplayClass24 class2;
            Point travelTo = duedainoinNiaga;
            this._fiohieweDe.Sort(new Comparison<Taxi>(class2.<GetTaxiThatGoesToDestination>b__23));
            uint num = 0;
            foreach (Taxi taxi in this._fiohieweDe)
            {
                if (num >= 3)
                {
                    break;
                }
                if (((taxi.Faction == Npc.FactionType.Neutral) || (taxi.Faction.ToString() == nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction)) && (((taxi.ContinentId == borekeoqiusosa) && this.IjiewouluUt(taxi)) && !this._niewakoEkegoeta.Contains(taxi)))
                {
                    bool flag;
                    num++;
                    List<Point> source = PathFinder.FindPath(taxi.Position, travelTo, Usefuls.ContinentNameMpqByContinentId(borekeoqiusosa), out flag, true, false, false);
                    if (flag || ((!flag && (source.Count >= 1)) && this.HekuohiIhuafio(source.Last<Point>())))
                    {
                        return taxi;
                    }
                }
            }
            return null;
        }

        private List<Portal> Qiwuo(Point duedainoinNiaga, int borekeoqiusosa)
        {
            List<Portal> list = new List<Portal>();
            foreach (Portal portal in this._anuahiuxeqajuhAxeoj.Items)
            {
                if ((portal.BContinentId == borekeoqiusosa) && ((portal.AContinentId != portal.BContinentId) || ((duedainoinNiaga.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) + 1000f) >= portal.BPoint.DistanceTo(duedainoinNiaga))))
                {
                    bool flag;
                    PathFinder.FindPath(portal.BPoint, duedainoinNiaga, Usefuls.ContinentNameMpqByContinentId(borekeoqiusosa), out flag, true, false, false);
                    if (flag || (portal.BPoint.DistanceTo(duedainoinNiaga) < 5f))
                    {
                        list.Add(portal);
                    }
                }
            }
            return list;
        }

        public override void Run()
        {
            MovementManager.StopMove();
            string str = "";
            string str2 = "one";
            if (this._tiesaCoe.Count == 2)
            {
                str2 = "two";
            }
            else if (this._tiesaCoe.Count == 3)
            {
                str2 = "three";
            }
            if (str2 != "one")
            {
                str = "s";
            }
            Logging.Write("Travel: Our travel plan consists of " + str2 + " transport" + str + " for today's journey.");
            foreach (Transport transport in this._tiesaCoe)
            {
                if (transport is Taxi)
                {
                    Taxi taxi = transport as Taxi;
                    Logging.Write(string.Concat(new object[] { "Travel: Taxi ", taxi.Name, "(", taxi.Id, "), to ", this.UkufuiciujieAxeApov(taxi.EndOfPath).Name, "(", taxi.EndOfPath, ")" }));
                }
                else if ((transport is CustomPath) || (transport is Portal))
                {
                    if ((transport is CustomPath) && transport.ArrivalIsA)
                    {
                        Logging.Write(string.Concat(new object[] { "Travel: ", transport.Name, "(", transport.Id, "), from ", Usefuls.ContinentNameMpqByContinentId(transport.BContinentId), " (", transport.BPoint, ") to ", Usefuls.ContinentNameMpqByContinentId(transport.AContinentId), " (", transport.APoint, ")" }));
                    }
                    else
                    {
                        Logging.Write(string.Concat(new object[] { "Travel: ", transport.Name, "(", transport.Id, "), from ", Usefuls.ContinentNameMpqByContinentId(transport.AContinentId), " (", transport.APoint, ") to ", Usefuls.ContinentNameMpqByContinentId(transport.BContinentId), " (", transport.BPoint, ")" }));
                    }
                }
                else if (transport.ArrivalIsA)
                {
                    Logging.Write(string.Concat(new object[] { "Travel: ", transport.Name, "(", transport.Id, "), from ", Usefuls.ContinentNameMpqByContinentId(transport.BContinentId), " (", transport.BOutsidePoint, ") to ", Usefuls.ContinentNameMpqByContinentId(transport.AContinentId), " (", transport.AOutsidePoint, ")" }));
                }
                else
                {
                    Logging.Write(string.Concat(new object[] { "Travel: ", transport.Name, "(", transport.Id, "), from ", Usefuls.ContinentNameMpqByContinentId(transport.AContinentId), " (", transport.AOutsidePoint, ") to ", Usefuls.ContinentNameMpqByContinentId(transport.BContinentId), " (", transport.BOutsidePoint, ")" }));
                }
            }
            if (this._tiesaCoe.Count > 1)
            {
                Logging.Write("Travel: We will recalculate travel path once arrived to make sure we are always on the fastest path.");
            }
            foreach (Transport transport2 in this._tiesaCoe)
            {
                this.Eresunaowep(transport2, false);
                if (nManager.Wow.ObjectManager.ObjectManager.Me.InInevitableCombat || nManager.Wow.ObjectManager.ObjectManager.Me.IsDead)
                {
                    return;
                }
                if ((!(transport2 is Portal) && !(transport2 is Taxi)) && !(transport2 is CustomPath))
                {
                    this.Veuqucim(transport2);
                    if (nManager.Wow.ObjectManager.ObjectManager.Me.InInevitableCombat || nManager.Wow.ObjectManager.ObjectManager.Me.IsDead)
                    {
                        return;
                    }
                }
                if (this.Ejijeokatova(transport2))
                {
                    this._tiesaCoe = this.Rumuq();
                    if (!CombatClass.IsAliveCombatClass)
                    {
                        CombatClass.LoadCombatClass();
                    }
                    return;
                }
                if (nManager.Wow.ObjectManager.ObjectManager.Me.InInevitableCombat || nManager.Wow.ObjectManager.ObjectManager.Me.IsDead)
                {
                    return;
                }
                if (transport2 is Taxi)
                {
                    TravelPatientlybyTaxiOrPortal(false);
                }
                else if (!(transport2 is Portal) && !(transport2 is CustomPath))
                {
                    this.Eteoveo(transport2);
                    if (nManager.Wow.ObjectManager.ObjectManager.Me.InInevitableCombat || nManager.Wow.ObjectManager.ObjectManager.Me.IsDead)
                    {
                        return;
                    }
                    this.Udikaikarire(transport2);
                }
                break;
            }
            this.IkodoApejoesDirefatui(0x98967f);
            this.Nikisoego(new Point());
            this.AfahueHoeha(new Point());
            this.ItobikepGeuImiat(false);
            this.TargetValidationFct = null;
            this._tiesaCoe = new List<Transport>();
            if (!CombatClass.IsAliveCombatClass)
            {
                CombatClass.LoadCombatClass();
            }
            Logging.Write("Travel is terminated, waiting for product to take the control back.");
        }

        private Taxi SopoimeAkuofafu(Taxi heowubIrOwuodo)
        {
            return new Taxi { Faction = heowubIrOwuodo.Faction, Id = heowubIrOwuodo.Id, Name = heowubIrOwuodo.Name, ContinentId = heowubIrOwuodo.ContinentId, Position = heowubIrOwuodo.Position, Xcoord = heowubIrOwuodo.Xcoord, Ycoord = heowubIrOwuodo.Ycoord };
        }

        public static void TravelPatientlybyTaxiOrPortal(bool ignoreCombatClass = false)
        {
            bool flag = true;
            Point position = nManager.Wow.ObjectManager.ObjectManager.Me.Position;
            while (flag)
            {
                Thread.Sleep(0x3e8);
                if (Usefuls.InGame && !Usefuls.IsLoading)
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

        private void Udikaikarire(Transport ovoiwulofabu)
        {
            Logging.Write(string.Concat(new object[] { "Transport ", ovoiwulofabu.Name, "(", ovoiwulofabu.Id, ") arrived at destination, leaving to the arrival quay." }));
            MovementManager.MoveTo(ovoiwulofabu.ArrivalIsA ? ovoiwulofabu.AOutsidePoint : ovoiwulofabu.BOutsidePoint, false);
            bool flag = true;
            while (flag)
            {
                if (nManager.Wow.ObjectManager.ObjectManager.Me.InInevitableCombat || nManager.Wow.ObjectManager.ObjectManager.Me.IsDead)
                {
                    return;
                }
                if (!nManager.Wow.ObjectManager.ObjectManager.Me.InTransport)
                {
                    flag = false;
                }
                if (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(ovoiwulofabu.ArrivalIsA ? ovoiwulofabu.AOutsidePoint : ovoiwulofabu.BOutsidePoint) < 5f)
                {
                    flag = false;
                }
                Thread.Sleep(500);
            }
        }

        private Transport Udioneveroag(Transport giqae)
        {
            bool flag;
            if (((!(giqae is Portal) && !(giqae is CustomPath)) || ((giqae is CustomPath) && (giqae as CustomPath).RoundTrip)) && giqae.ArrivalIsA)
            {
                if (!giqae.UseALift)
                {
                    return new Transport();
                }
                Transport transport = this.Okudofikab(giqae.ALift);
                PathFinder.FindPath(this.PiuvuqauhIleu(), (transport is CustomPath) ? transport.BPoint : transport.BOutsidePoint, Usefuls.ContinentNameMpqByContinentId(this.Akuotowupeqer()), out flag, true, false, false);
                if (!flag)
                {
                    transport.ArrivalIsA = true;
                }
                return transport;
            }
            if (!giqae.UseBLift)
            {
                return new Transport();
            }
            Transport transport2 = this.Okudofikab(giqae.BLift);
            PathFinder.FindPath(this.PiuvuqauhIleu(), ((transport2 is CustomPath) || (transport2 is Portal)) ? transport2.BPoint : transport2.BOutsidePoint, Usefuls.ContinentNameMpqByContinentId(this.Akuotowupeqer()), out flag, true, false, false);
            if (!flag)
            {
                if (((transport2 is Portal) || (transport2 is CustomPath)) && (!(transport2 is CustomPath) || !(transport2 as CustomPath).RoundTrip))
                {
                    return transport2;
                }
                transport2.ArrivalIsA = true;
            }
            return transport2;
        }

        private Taxi UkufuiciujieAxeApov(uint guoluacuigut)
        {
            foreach (Taxi taxi in this._fiohieweDe)
            {
                if (taxi.Id == guoluacuigut)
                {
                    return taxi;
                }
            }
            return new Taxi();
        }

        private List<Transport> UquewibiKa(Point duedainoinNiaga, int borekeoqiusosa)
        {
            List<Transport> list = new List<Transport>();
            foreach (Transport transport in this._tuapugoal.Items)
            {
                if (((transport.Faction == Npc.FactionType.Neutral) || (transport.Faction.ToString() == nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction)) && ((transport.AContinentId == borekeoqiusosa) || (transport.BContinentId == borekeoqiusosa)))
                {
                    if ((transport.AContinentId == borekeoqiusosa) && (transport.BContinentId != borekeoqiusosa))
                    {
                        bool flag;
                        transport.ArrivalIsA = true;
                        PathFinder.FindPath(transport.AOutsidePoint, duedainoinNiaga, Usefuls.ContinentNameMpqByContinentId(borekeoqiusosa), out flag, true, false, false);
                        if (flag)
                        {
                            list.Add(transport);
                        }
                        else if (transport.ALift > 0)
                        {
                            Transport transport2 = this.Okudofikab(transport.ALift);
                            if (transport2.Id > 0)
                            {
                                transport.UseALift = true;
                                if (!(transport2 is Portal) && (!(transport2 is CustomPath) || (transport2 as CustomPath).RoundTrip))
                                {
                                    PathFinder.FindPath((transport2 is CustomPath) ? transport2.APoint : transport2.AOutsidePoint, duedainoinNiaga, Usefuls.ContinentNameMpqByContinentId(borekeoqiusosa), out flag, true, false, false);
                                    if (flag)
                                    {
                                        list.Add(transport);
                                        continue;
                                    }
                                }
                                PathFinder.FindPath(((transport2 is CustomPath) || (transport2 is Portal)) ? transport2.BPoint : transport2.BOutsidePoint, duedainoinNiaga, Usefuls.ContinentNameMpqByContinentId(borekeoqiusosa), out flag, true, false, false);
                                if (flag)
                                {
                                    list.Add(transport);
                                }
                            }
                        }
                    }
                    else if ((transport.BContinentId == borekeoqiusosa) && (transport.AContinentId != borekeoqiusosa))
                    {
                        bool flag2;
                        transport.ArrivalIsA = false;
                        PathFinder.FindPath(transport.BOutsidePoint, duedainoinNiaga, Usefuls.ContinentNameMpqByContinentId(borekeoqiusosa), out flag2, true, false, false);
                        if (flag2)
                        {
                            list.Add(transport);
                        }
                        else if (transport.BLift > 0)
                        {
                            Transport transport3 = this.Okudofikab(transport.BLift);
                            if (transport3.Id > 0)
                            {
                                transport.UseBLift = true;
                                if (!(transport3 is Portal) && (!(transport3 is CustomPath) || (transport3 as CustomPath).RoundTrip))
                                {
                                    PathFinder.FindPath((transport3 is CustomPath) ? transport3.APoint : transport3.AOutsidePoint, duedainoinNiaga, Usefuls.ContinentNameMpqByContinentId(borekeoqiusosa), out flag2, true, false, false);
                                    if (flag2)
                                    {
                                        list.Add(transport);
                                        continue;
                                    }
                                }
                                PathFinder.FindPath(((transport3 is CustomPath) || (transport3 is Portal)) ? transport3.BPoint : transport3.BOutsidePoint, duedainoinNiaga, Usefuls.ContinentNameMpqByContinentId(borekeoqiusosa), out flag2, true, false, false);
                                if (flag2)
                                {
                                    list.Add(transport);
                                }
                            }
                        }
                    }
                    else if ((transport.AContinentId == borekeoqiusosa) && (transport.BContinentId == borekeoqiusosa))
                    {
                        bool flag3;
                        float num = nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(duedainoinNiaga);
                        if (transport.AOutsidePoint.DistanceTo(duedainoinNiaga) < transport.BOutsidePoint.DistanceTo(duedainoinNiaga))
                        {
                            if ((num + 1000f) >= transport.AOutsidePoint.DistanceTo(duedainoinNiaga))
                            {
                                transport.ArrivalIsA = true;
                                PathFinder.FindPath(transport.AOutsidePoint, duedainoinNiaga, Usefuls.ContinentNameMpqByContinentId(borekeoqiusosa), out flag3, true, false, false);
                                if (flag3)
                                {
                                    list.Add(transport);
                                }
                                else if (transport.ALift > 0)
                                {
                                    Transport transport4 = this.Okudofikab(transport.ALift);
                                    if (transport4.Id > 0)
                                    {
                                        transport.UseALift = true;
                                        if (!(transport4 is Portal) && (!(transport4 is CustomPath) || (transport4 as CustomPath).RoundTrip))
                                        {
                                            PathFinder.FindPath((transport4 is CustomPath) ? transport4.APoint : transport4.AOutsidePoint, duedainoinNiaga, Usefuls.ContinentNameMpqByContinentId(borekeoqiusosa), out flag3, true, false, false);
                                            if (flag3)
                                            {
                                                list.Add(transport);
                                                continue;
                                            }
                                        }
                                        PathFinder.FindPath(((transport4 is CustomPath) || (transport4 is Portal)) ? transport4.BPoint : transport4.BOutsidePoint, duedainoinNiaga, Usefuls.ContinentNameMpqByContinentId(borekeoqiusosa), out flag3, true, false, false);
                                        if (flag3)
                                        {
                                            list.Add(transport);
                                        }
                                    }
                                }
                            }
                        }
                        else if ((num + 1000f) >= transport.BOutsidePoint.DistanceTo(duedainoinNiaga))
                        {
                            transport.UseALift = false;
                            transport.ArrivalIsA = false;
                            PathFinder.FindPath(transport.BOutsidePoint, duedainoinNiaga, Usefuls.ContinentNameMpqByContinentId(borekeoqiusosa), out flag3, true, false, false);
                            if (flag3)
                            {
                                list.Add(transport);
                            }
                            else if (transport.BLift > 0)
                            {
                                Transport transport5 = this.Okudofikab(transport.BLift);
                                if (transport5.Id > 0)
                                {
                                    transport.UseBLift = true;
                                    if (!(transport5 is Portal) && (!(transport5 is CustomPath) || (transport5 as CustomPath).RoundTrip))
                                    {
                                        PathFinder.FindPath((transport5 is CustomPath) ? transport5.APoint : transport5.AOutsidePoint, duedainoinNiaga, Usefuls.ContinentNameMpqByContinentId(borekeoqiusosa), out flag3, true, false, false);
                                        if (flag3)
                                        {
                                            list.Add(transport);
                                            continue;
                                        }
                                    }
                                    PathFinder.FindPath(((transport5 is CustomPath) || (transport5 is Portal)) ? transport5.BPoint : transport5.BOutsidePoint, duedainoinNiaga, Usefuls.ContinentNameMpqByContinentId(borekeoqiusosa), out flag3, true, false, false);
                                    if (flag3)
                                    {
                                        list.Add(transport);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return list;
        }

        private void Veuqucim(Transport ovoiwulofabu)
        {
            WoWGameObject obj2 = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWGameObject(nManager.Wow.ObjectManager.ObjectManager.GetWoWGameObjectByEntry((int) ovoiwulofabu.Id), nManager.Wow.ObjectManager.ObjectManager.Me.Position, false);
            bool flag = true;
            int num = 0;
            while (flag)
            {
                if (Usefuls.IsFlying)
                {
                    MountTask.DismountMount(true);
                }
                if (obj2.IsValid)
                {
                    if ((ovoiwulofabu.ArrivalIsA ? ovoiwulofabu.BPoint : ovoiwulofabu.APoint).DistanceTo(obj2.Position) < 0.2f)
                    {
                        if (num > 5)
                        {
                            flag = false;
                        }
                        num++;
                        Thread.Sleep(300);
                    }
                    if (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(ovoiwulofabu.ArrivalIsA ? ovoiwulofabu.BOutsidePoint : ovoiwulofabu.AOutsidePoint) > 5f)
                    {
                        this.Eresunaowep(ovoiwulofabu, false);
                        return;
                    }
                    Thread.Sleep(100);
                }
                else
                {
                    obj2 = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWGameObject(nManager.Wow.ObjectManager.ObjectManager.GetWoWGameObjectByEntry((int) ovoiwulofabu.Id), nManager.Wow.ObjectManager.ObjectManager.Me.Position, false);
                }
            }
        }

        private List<Transport> Xiriceuc(Point duedainoinNiaga, int borekeoqiusosa, Point loimacovuoUpip, int toceutodCearakeoj)
        {
            List<Transport> list = new List<Transport>();
            List<Transport> collection = this.UquewibiKa(duedainoinNiaga, borekeoqiusosa);
            List<CustomPath> list3 = this.Fuduvoalue(duedainoinNiaga, borekeoqiusosa, loimacovuoUpip, toceutodCearakeoj);
            List<Portal> list4 = this.Qiwuo(duedainoinNiaga, borekeoqiusosa);
            Taxi item = this.OxibixexujehOkeo(duedainoinNiaga, borekeoqiusosa);
            list.AddRange(collection);
            list.AddRange(list3);
            list.AddRange(list4);
            if (item != null)
            {
                list.Add(item);
            }
            return list;
        }

        private int _asifowMit
        {
            get
            {
                return nManager.Products.Products.TravelFromContinentId;
            }
            set
            {
                nManager.Products.Products.TravelFromContinentId = value;
            }
        }

        private bool _axuqomia
        {
            get
            {
                return nManager.Products.Products.ForceTravel;
            }
            set
            {
                nManager.Products.Products.ForceTravel = value;
            }
        }

        private bool _faigigouliIveabo
        {
            get
            {
                return (this.Akuotowupeqer() != 0x98967f);
            }
        }

        private List<Transport> _garouqaXoEvueri
        {
            get
            {
                int borekeoqiusosa = this.Akuotowupeqer();
                if (borekeoqiusosa == this.Ugomiajo())
                {
                    bool flag2;
                    Logging.Write(string.Concat(new object[] { "Generating Travel from ", this.Foafupoisi(), " ", Usefuls.ContinentNameMpqByContinentId(this.Ugomiajo()), " to ", this.PiuvuqauhIleu(), " ", Usefuls.ContinentNameMpqByContinentId(this.Akuotowupeqer()), ", Distance: ", this.Foafupoisi().DistanceTo(this.PiuvuqauhIleu()) }));
                    List<Point> listPoints = PathFinder.FindPath(this.Foafupoisi(), this.PiuvuqauhIleu(), Usefuls.ContinentNameMpqByContinentId(this.Ugomiajo()), out flag2, true, false, false);
                    if (flag2 && (nManager.Helpful.Math.DistanceListPoint(listPoints) <= 400f))
                    {
                        this.IkodoApejoesDirefatui(0x98967f);
                        this.Nikisoego(new Point());
                        this.AfahueHoeha(new Point());
                        this.ItobikepGeuImiat(false);
                        this.TargetValidationFct = null;
                        Logging.Write("Travel: We are close enough and we have a valid path. Cancelling Travel.");
                        return new List<Transport>();
                    }
                }
                else
                {
                    Logging.Write(string.Concat(new object[] { "Generating Travel from ", this.Foafupoisi(), " ", Usefuls.ContinentNameMpqByContinentId(this.Ugomiajo()), " to ", this.PiuvuqauhIleu(), " ", Usefuls.ContinentNameMpqByContinentId(this.Akuotowupeqer()), "." }));
                }
                KeyValuePair<Transport, float> pair = this.EdokafauUbiIl(this.Foafupoisi(), this.PiuvuqauhIleu(), this.Ugomiajo(), borekeoqiusosa);
                List<Transport> list2 = new List<Transport>();
                float num2 = 0f;
                if (pair.Key.Id != 0)
                {
                    Logging.Write("Travel: Found direct way travel.");
                    bool flag3 = false;
                    float num3 = 0f;
                    Transport item = this.DedejiewonIf(pair.Key);
                    KeyValuePair<Transport, float> pair2 = new KeyValuePair<Transport, float>(new Transport(), float.MaxValue);
                    bool flag4 = false;
                    if (!(pair.Key is Taxi))
                    {
                        bool flag;
                        Point duedainoinNiaga = null;
                        Point point2 = null;
                        if (item.Id > 0)
                        {
                            if ((item.ArrivalIsA && !(item is Portal)) && (!(item is CustomPath) || (item as CustomPath).RoundTrip))
                            {
                                duedainoinNiaga = (item is CustomPath) ? item.BPoint : item.BOutsidePoint;
                            }
                            else
                            {
                                duedainoinNiaga = ((item is Portal) || (item is CustomPath)) ? item.APoint : item.AOutsidePoint;
                            }
                        }
                        if ((pair.Key.ArrivalIsA && !(pair.Key is Portal)) && (!(pair.Key is CustomPath) || (pair.Key as CustomPath).RoundTrip))
                        {
                            point2 = (pair.Key is CustomPath) ? pair.Key.BPoint : pair.Key.BOutsidePoint;
                        }
                        else
                        {
                            point2 = ((pair.Key is Portal) || (pair.Key is CustomPath)) ? pair.Key.APoint : pair.Key.AOutsidePoint;
                        }
                        int num4 = item.ArrivalIsA ? item.BContinentId : item.AContinentId;
                        int num5 = pair.Key.ArrivalIsA ? pair.Key.BContinentId : pair.Key.AContinentId;
                        KeyValuePair<Transport, float> pair3 = this.Mumeo(this.Foafupoisi(), point2, this.Ugomiajo(), num5);
                        KeyValuePair<Transport, float> pair4 = new KeyValuePair<Transport, float>(new Transport(), float.MaxValue);
                        if ((item.Id > 0) && (duedainoinNiaga != null))
                        {
                            pair4 = this.Mumeo(this.Foafupoisi(), duedainoinNiaga, this.Ugomiajo(), num4);
                        }
                        List<Point> list3 = PathFinder.FindPath(this.Foafupoisi(), ((duedainoinNiaga != null) && duedainoinNiaga.IsValid) ? duedainoinNiaga : point2, Usefuls.ContinentNameMpqByContinentId(this.Ugomiajo()), out flag, true, false, false);
                        if (flag)
                        {
                            num3 = nManager.Helpful.Math.DistanceListPoint(list3);
                            if ((num3 >= pair4.Value) || (num3 >= pair3.Value))
                            {
                                flag3 = true;
                            }
                        }
                        else
                        {
                            flag3 = true;
                        }
                        if ((pair4.Key.Id != 0) && (pair3.Key.Id != 0))
                        {
                            pair2 = (pair4.Value > pair3.Value) ? pair3 : pair4;
                            flag4 = pair4.Value <= pair3.Value;
                        }
                        else if (pair4.Key.Id != 0)
                        {
                            pair2 = pair4;
                            flag4 = true;
                        }
                        else if (pair3.Key.Id != 0)
                        {
                            pair2 = pair3;
                        }
                    }
                    if (flag3)
                    {
                        if ((item.Id > 0) && ((pair2.Key.Id == 0) || flag4))
                        {
                            if ((pair2.Key.Id != 0) && flag4)
                            {
                                list2.Add(pair2.Key);
                                num2 -= num3;
                                num2 += pair2.Value;
                            }
                            list2.Add(item);
                        }
                        else if ((pair2.Key.Id != 0) && !flag4)
                        {
                            list2.Add(pair2.Key);
                            num2 -= num3;
                            num2 += pair2.Value;
                        }
                    }
                    else if (item.Id > 0)
                    {
                        list2.Add(item);
                    }
                    list2.Add(pair.Key);
                    num2 += pair.Value;
                    Transport transport2 = this.Udioneveroag(pair.Key);
                    if (transport2.Id > 0)
                    {
                        list2.Add(transport2);
                    }
                }
                if ((this.Ugomiajo() == borekeoqiusosa) && (list2.Count > 0))
                {
                    bool flag5;
                    List<Point> source = PathFinder.FindPath(this.Foafupoisi(), this.PiuvuqauhIleu(), Usefuls.ContinentNameMpqByContinentId(this.Ugomiajo()), out flag5, true, false, false);
                    if (flag5 || ((!flag5 && (source.Count >= 1)) && (this.HekuohiIhuafio(source.Last<Point>()) && !(pair.Key is CustomPath))))
                    {
                        if ((num2 > nManager.Helpful.Math.DistanceListPoint(source)) && !this.Usikeapoafuw())
                        {
                            this.IkodoApejoesDirefatui(0x98967f);
                            this.Nikisoego(new Point());
                            this.AfahueHoeha(new Point());
                            this.TargetValidationFct = null;
                            Logging.Write("Travel: Found a faster path without using Transports. Cancelling Travel.");
                            return new List<Transport>();
                        }
                        if (this.Usikeapoafuw())
                        {
                            Logging.Write("Travel: Found a faster path without using Transports but ForceTravel is activated.");
                        }
                    }
                }
                if (list2.Count > 0)
                {
                    return list2;
                }
                this.IkodoApejoesDirefatui(0x98967f);
                this.Nikisoego(new Point());
                this.AfahueHoeha(new Point());
                this.ItobikepGeuImiat(false);
                this.TargetValidationFct = null;
                Logging.Write("Travel: Couldn't find a travel path.");
                return new List<Transport>();
            }
        }

        private Point _iloesexas
        {
            get
            {
                return nManager.Products.Products.TravelFrom;
            }
            set
            {
                nManager.Products.Products.TravelFrom = value;
            }
        }

        private Point _jiakaoPeotei
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

        private int _omuhoxehi
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

        public override bool NeedToRun
        {
            get
            {
                if ((!nManager.Products.Products.IsStarted || nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe) || (nManager.Wow.ObjectManager.ObjectManager.Me.InInevitableCombat || !this.AkefopuvBira()))
                {
                    return false;
                }
                this._tuapugoal = XmlSerializer.Deserialize<Transports>(Application.StartupPath + @"\Data\TransportsDB.xml");
                for (int i = this._tuapugoal.Items.Count - 1; i > 0; i--)
                {
                    Transport transport = this._tuapugoal.Items[i];
                    if ((transport.Faction != Npc.FactionType.Neutral) && (transport.Faction.ToString() != nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction))
                    {
                        this._tuapugoal.Items.RemoveAt(i);
                    }
                }
                this._anuahiuxeqajuhAxeoj = XmlSerializer.Deserialize<Portals>(Application.StartupPath + @"\Data\PortalsDB.xml");
                for (int j = this._anuahiuxeqajuhAxeoj.Items.Count - 1; j >= 0; j--)
                {
                    Portal portal = this._anuahiuxeqajuhAxeoj.Items[j];
                    if ((portal.Faction != Npc.FactionType.Neutral) && (portal.Faction.ToString() != nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction))
                    {
                        this._anuahiuxeqajuhAxeoj.Items.RemoveAt(j);
                    }
                    else if ((portal.RequireQuestId > 0) && !Quest.IsQuestFlaggedCompletedLUA(portal.RequireQuestId))
                    {
                        this._anuahiuxeqajuhAxeoj.Items.RemoveAt(j);
                    }
                    else if ((portal.RequireAchivementId > 0) && !Usefuls.IsCompletedAchievement(portal.RequireAchivementId, true))
                    {
                        this._anuahiuxeqajuhAxeoj.Items.RemoveAt(j);
                    }
                }
                this._soigoti = XmlSerializer.Deserialize<CustomPaths>(Application.StartupPath + @"\Data\CustomPathsDB.xml");
                for (int k = this._soigoti.Items.Count - 1; k >= 0; k--)
                {
                    CustomPath path = this._soigoti.Items[k];
                    if ((path.Faction != Npc.FactionType.Neutral) && (path.Faction.ToString() != nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction))
                    {
                        this._soigoti.Items.RemoveAt(k);
                    }
                    else if ((path.RequireQuestId > 0) && !Quest.IsQuestFlaggedCompletedLUA(path.RequireQuestId))
                    {
                        this._soigoti.Items.RemoveAt(k);
                    }
                    else if ((path.RequireAchivementId > 0) && !Usefuls.IsCompletedAchievement(path.RequireAchivementId, true))
                    {
                        this._soigoti.Items.RemoveAt(k);
                    }
                }
                if (this._fiohieweDe == null)
                {
                    this._fiohieweDe = XmlSerializer.Deserialize<List<Taxi>>(Application.StartupPath + @"\Data\TaxiList.xml");
                    for (int m = this._fiohieweDe.Count - 1; m >= 0; m--)
                    {
                        Taxi taxi = this._fiohieweDe[m];
                        if ((taxi.Faction != Npc.FactionType.Neutral) && (taxi.Faction.ToString() != nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction))
                        {
                            this._fiohieweDe.RemoveAt(m);
                        }
                    }
                }
                if (this._uvuqaduv == null)
                {
                    this._uvuqaduv = XmlSerializer.Deserialize<List<TaxiLink>>(Application.StartupPath + @"\Data\TaxiLinks.xml");
                    for (int n = this._uvuqaduv.Count - 1; n >= 0; n--)
                    {
                        TaxiLink link = this._uvuqaduv[n];
                        if (link.PointB <= 0)
                        {
                            this._uvuqaduv.RemoveAt(n);
                        }
                    }
                }
                if (((this._tuapugoal == null) || (this._anuahiuxeqajuhAxeoj == null)) || (((this._soigoti == null) || (this._fiohieweDe == null)) || (this._uvuqaduv == null)))
                {
                    return false;
                }
                if (this._tiesaCoe.Count > 0)
                {
                    return true;
                }
                this._tiesaCoe = this.Rumuq();
                return (this._tiesaCoe.Count > 0);
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

        public class TaxiEventArgs : EventArgs
        {
            public int From { get; set; }

            public int To { get; set; }
        }
    }
}

