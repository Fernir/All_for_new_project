namespace nManager.Wow.Helpers
{
    using nManager;
    using nManager.Helpful;
    using nManager.Wow.Bot.Tasks;
    using nManager.Wow.Class;
    using nManager.Wow.Enums;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class Fight
    {
        public static UInt128 StartFight(UInt128 guid = new UInt128())
        {
            MovementManager.StopMove();
            WoWUnit unit = null;
            try
            {
                if (guid == 0)
                {
                    unit = new WoWUnit(nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWUnit(nManager.Wow.ObjectManager.ObjectManager.GetHostileUnitAttackingPlayer(), false, false, false).GetBaseAddress);
                }
                else
                {
                    unit = new WoWUnit(nManager.Wow.ObjectManager.ObjectManager.GetObjectByGuid(guid).GetBaseAddress);
                }
                if (nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted && (CombatClass.InAggroRange(unit) || CombatClass.InRange(unit)))
                {
                    MountTask.DismountMount(true);
                }
                InFight = true;
                if (!nManager.Wow.ObjectManager.ObjectManager.Me.IsCast)
                {
                    Interact.InteractWith(unit.GetBaseAddress, false);
                }
                Thread.Sleep(100);
                if (nManager.Wow.ObjectManager.ObjectManager.Me.GetMove && !nManager.Wow.ObjectManager.ObjectManager.Me.IsCast)
                {
                    MovementManager.StopMoveTo(true, false);
                }
                Point position = unit.Position;
                int num = 0;
            Label_00AF:
                if (unit.Position.DistanceTo(position) > 50f)
                {
                    return 0;
                }
                if (Usefuls.IsInBattleground && !Battleground.IsFinishBattleground())
                {
                    List<WoWUnit> hostileUnitAttackingPlayer = nManager.Wow.ObjectManager.ObjectManager.GetHostileUnitAttackingPlayer();
                    if (((hostileUnitAttackingPlayer.Count > 0) && (nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWUnit(hostileUnitAttackingPlayer, false, false, false).GetDistance < unit.GetDistance)) && (nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWUnit(hostileUnitAttackingPlayer, false, false, false).SummonedBy == 0))
                    {
                        return 0;
                    }
                }
                if ((nManager.Wow.ObjectManager.ObjectManager.Me.InCombat && !CombatClass.InRange(unit)) || TraceLine.TraceLineGo(unit.Position))
                {
                    bool flag;
                    List<Point> points = PathFinder.FindPath(unit.Position, out flag, true, false);
                    if ((!flag && !Usefuls.IsFlying) && (MountTask.GetMountCapacity() >= MountCapacity.Fly))
                    {
                        MountTask.Mount(true);
                    }
                    MovementManager.Go(points);
                    num = (Others.Times + ((int) ((nManager.Helpful.Math.DistanceListPoint(points) / 3f) * 1000f))) + 0x3a98;
                    while (((!nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe && !unit.IsDead) && (!unit.IsLootable && (unit.Health > 0))) && ((unit.IsValid && MovementManager.InMovement) && ((InFight && Usefuls.InGame) && (TraceLine.TraceLineGo(unit.Position) || !CombatClass.InAggroRange(unit)))))
                    {
                        if ((((unit.Type != WoWObjectType.Player) && !unit.IsTargetingMe) && ((unit.Target != 0) && !(((WoWUnit) nManager.Wow.ObjectManager.ObjectManager.GetObjectByGuid(unit.Target)).SummonedBy == nManager.Wow.ObjectManager.ObjectManager.Me.Guid))) && !(unit.Target == nManager.Wow.ObjectManager.ObjectManager.Pet.Guid))
                        {
                            return unit.Guid;
                        }
                        if ((Others.Times > num) && TraceLine.TraceLineGo(unit.Position))
                        {
                            return unit.Guid;
                        }
                        if (!unit.Position.IsValid)
                        {
                            return unit.Guid;
                        }
                        if (unit.Position.DistanceTo(position) > 50f)
                        {
                            return 0;
                        }
                        if (((nManager.Wow.ObjectManager.ObjectManager.GetNumberAttackPlayer() > 0) && !unit.IsTargetingMe) && ((unit.Target != nManager.Wow.ObjectManager.ObjectManager.Pet.Guid) || (unit.Target <= 0)))
                        {
                            return 0;
                        }
                        Thread.Sleep(50);
                    }
                }
                num = (Others.Times + ((int) ((nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(unit.Position) / 3f) * 1000f))) + 0x1388;
                if (MovementManager.InMovement)
                {
                    MovementManager.StopMove();
                }
                if (!nManager.Wow.ObjectManager.ObjectManager.Me.IsCast && (nManager.Wow.ObjectManager.ObjectManager.Me.Target != unit.Guid))
                {
                    Interact.InteractWith(unit.GetBaseAddress, false);
                }
                InFight = true;
                Thread.Sleep(200);
                if (CombatClass.InAggroRange(unit))
                {
                    if (nManager.Wow.ObjectManager.ObjectManager.Me.GetMove && !nManager.Wow.ObjectManager.ObjectManager.Me.IsCast)
                    {
                        MovementManager.StopMoveTo(true, false);
                    }
                    if (!nManager.Wow.ObjectManager.ObjectManager.Me.GetMove && nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted)
                    {
                        MountTask.DismountMount(true);
                    }
                }
                if (unit.IsDead || !unit.IsValid)
                {
                    WoWUnit unit2 = new WoWUnit(nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWUnit(nManager.Wow.ObjectManager.ObjectManager.GetHostileUnitAttackingPlayer(), false, false, false).GetBaseAddress);
                    if ((unit2.IsValid && !nManager.Wow.ObjectManager.ObjectManager.Me.IsCast) && (nManager.Wow.ObjectManager.ObjectManager.Me.Target != unit2.Guid))
                    {
                        unit = unit2;
                        Interact.InteractWith(unit.GetBaseAddress, false);
                        MovementManager.Face(unit, true);
                    }
                }
                while (((!nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe && !unit.IsDead) && (unit.IsValid && InFight)) && (unit.IsValid && !nManager.Wow.ObjectManager.ObjectManager.Me.InTransport))
                {
                    if ((((unit.Type != WoWObjectType.Player) && !unit.IsTargetingMe) && ((unit.Target != nManager.Wow.ObjectManager.ObjectManager.Pet.Guid) || (unit.Target <= 0))) && (nManager.Wow.ObjectManager.ObjectManager.GetNumberAttackPlayer() > 0))
                    {
                        return 0;
                    }
                    if (unit.IsTapped && !unit.IsTappedByMe)
                    {
                        return 0;
                    }
                    if (!unit.Position.IsValid)
                    {
                        InFight = false;
                        return unit.Guid;
                    }
                    if (((nManager.Wow.ObjectManager.ObjectManager.Me.Target != unit.Guid) && !unit.IsDead) && !nManager.Wow.ObjectManager.ObjectManager.Me.IsCast)
                    {
                        Interact.InteractWith(unit.GetBaseAddress, false);
                    }
                    if (!nManager.Wow.ObjectManager.ObjectManager.Me.IsCast && ((!nManager.Wow.ObjectManager.ObjectManager.Me.InCombat && !CombatClass.InAggroRange(unit)) || (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat && !CombatClass.InRange(unit))))
                    {
                        int num2 = Others.Random(1, 20);
                        MovementManager.MoveTo(unit);
                        if (num2 == 5)
                        {
                            MovementsAction.Jump();
                        }
                    }
                    if ((!CombatClass.InRange(unit) && !nManager.Wow.ObjectManager.ObjectManager.Me.IsCast) || TraceLine.TraceLineGo(unit.Position))
                    {
                        goto Label_00AF;
                    }
                    if ((CombatClass.InRange(unit) && nManager.Wow.ObjectManager.ObjectManager.Me.GetMove) && !nManager.Wow.ObjectManager.ObjectManager.Me.IsCast)
                    {
                        MovementManager.StopMoveTo(true, false);
                    }
                    if (nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted)
                    {
                        MountTask.DismountMount(true);
                        Interact.InteractWith(unit.GetBaseAddress, false);
                    }
                    MovementManager.Face(unit, true);
                    if (((Others.Times > num) && TraceLine.TraceLineGo(unit.Position)) && (unit.HealthPercent > 90f))
                    {
                        InFight = false;
                        return unit.Guid;
                    }
                    Thread.Sleep((int) (0x4b + Usefuls.Latency));
                    if (((Others.Times > num) && !nManager.Wow.ObjectManager.ObjectManager.Me.InCombat) && !unit.IsDead)
                    {
                        InFight = false;
                        return unit.Guid;
                    }
                }
                MovementManager.StopMoveTo(true, false);
                InFight = false;
            }
            catch (Exception exception)
            {
                Logging.WriteError("StartFight(UInt128 guid = 0, bool inBg = false): " + exception, true);
                InFight = false;
            }
            try
            {
                if (unit != null)
                {
                    return unit.Guid;
                }
            }
            catch
            {
                return 0;
            }
            return 0;
        }

        public static UInt128 StartFightDamageDealer(UInt128 guid = new UInt128())
        {
            WoWUnit unit = null;
            try
            {
                if (nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted)
                {
                    return 0;
                }
                if (guid == 0)
                {
                    unit = new WoWUnit(nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWUnit(nManager.Wow.ObjectManager.ObjectManager.GetHostileUnitAttackingPlayer(), false, false, false).GetBaseAddress);
                }
                else
                {
                    unit = new WoWUnit(nManager.Wow.ObjectManager.ObjectManager.GetObjectByGuid(guid).GetBaseAddress);
                }
                InFight = true;
                if (!nManager.Wow.ObjectManager.ObjectManager.Me.IsCast)
                {
                    if (CombatClass.InRange(unit) && (CombatClass.GetRange <= 5f))
                    {
                        Interact.InteractWith(unit.GetBaseAddress, false);
                    }
                    nManager.Wow.ObjectManager.ObjectManager.Me.Target = unit.Guid;
                }
                Thread.Sleep(100);
                Point position = unit.Position;
            Label_00A4:
                if (unit.Position.DistanceTo(position) > (CombatClass.GetRange + 5f))
                {
                    return 0;
                }
                if (Usefuls.IsInBattleground && !Battleground.IsFinishBattleground())
                {
                    List<WoWUnit> hostileUnitAttackingPlayer = nManager.Wow.ObjectManager.ObjectManager.GetHostileUnitAttackingPlayer();
                    if (((hostileUnitAttackingPlayer.Count > 0) && (nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWUnit(hostileUnitAttackingPlayer, false, false, false).GetDistance < unit.GetDistance)) && (nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWUnit(hostileUnitAttackingPlayer, false, false, false).SummonedBy == 0))
                    {
                        return 0;
                    }
                }
                Thread.Sleep(200);
                if ((CombatClass.InRange(unit) && (CombatClass.GetRange > 5f)) && (nManager.Wow.ObjectManager.ObjectManager.Me.GetMove && !nManager.Wow.ObjectManager.ObjectManager.Me.IsCast))
                {
                    Logging.Write("Your class recquires you to stop moving in order to cast spell, as this product is passive, we wont try to force stop.");
                }
                if (((nManager.Wow.ObjectManager.ObjectManager.Me.Target != unit.Guid) && !unit.IsDead) && !nManager.Wow.ObjectManager.ObjectManager.Me.IsCast)
                {
                    nManager.Wow.ObjectManager.ObjectManager.Me.Target = unit.Guid;
                    if (CombatClass.GetRange <= 5f)
                    {
                        Interact.InteractWith(unit.GetBaseAddress, false);
                    }
                }
                if (unit.IsDead || !unit.IsValid)
                {
                    unit = new WoWUnit(nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWUnit(nManager.Wow.ObjectManager.ObjectManager.GetHostileUnitAttackingPlayer(), false, false, false).GetBaseAddress);
                    if (!nManager.Wow.ObjectManager.ObjectManager.Me.IsCast && (nManager.Wow.ObjectManager.ObjectManager.Me.Target != unit.Guid))
                    {
                        Interact.InteractWith(unit.GetBaseAddress, false);
                    }
                    if (nManagerSetting.CurrentSetting.ActivateAutoFacingDamageDealer)
                    {
                        MovementManager.Face(unit, false);
                    }
                }
                while ((!nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe && !unit.IsDead) && ((unit.IsValid && InFight) && !nManager.Wow.ObjectManager.ObjectManager.Me.InTransport))
                {
                    if (!unit.Position.IsValid)
                    {
                        return unit.Guid;
                    }
                    if (((nManager.Wow.ObjectManager.ObjectManager.Me.Target != unit.Guid) && !unit.IsDead) && !nManager.Wow.ObjectManager.ObjectManager.Me.IsCast)
                    {
                        if (nManager.Wow.ObjectManager.ObjectManager.Me.Target == 0)
                        {
                            return 0;
                        }
                        if (CombatClass.GetRange <= 5f)
                        {
                            Interact.InteractWith(nManager.Wow.ObjectManager.ObjectManager.Target.GetBaseAddress, false);
                        }
                        unit = new WoWUnit(nManager.Wow.ObjectManager.ObjectManager.Target.GetBaseAddress);
                        goto Label_00A4;
                    }
                    if (nManagerSetting.CurrentSetting.ActivateAutoFacingDamageDealer)
                    {
                        MovementManager.Face(unit, false);
                    }
                    Thread.Sleep(50);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("StartFightDamageDealer(UInt128 guid = 0, bool inBg = false): " + exception, true);
            }
            if (unit != null)
            {
                return unit.Guid;
            }
            return 0;
        }

        public static void StopFight()
        {
            try
            {
                InFight = false;
            }
            catch (Exception exception)
            {
                Logging.WriteError("StopFight(): " + exception, true);
                InFight = false;
            }
        }

        public static bool InFight
        {
            [CompilerGenerated]
            get
            {
                return <InFight>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <InFight>k__BackingField = value;
            }
        }
    }
}

