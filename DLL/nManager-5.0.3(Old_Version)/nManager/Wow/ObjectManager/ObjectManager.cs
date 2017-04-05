namespace nManager.Wow.ObjectManager
{
    using nManager;
    using nManager.Helpful;
    using nManager.Wow;
    using nManager.Wow.Class;
    using nManager.Wow.Enums;
    using nManager.Wow.Helpers;
    using nManager.Wow.Patchables;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;

    public static class ObjectManager
    {
        private static List<WoWGameObject> _gameobjectList;
        private static uint _lastTargetBase;
        private static List<WoWObject> _objectList;
        private static List<WoWPlayer> _playerList;
        private static List<WoWUnit> _unitList;
        public static List<UInt128> BlackListMobAttack = new List<UInt128>();
        private static readonly object Locker = new object();

        static ObjectManager()
        {
            try
            {
                ObjectDictionary = new ConcurrentDictionary<UInt128, WoWObject>();
                Me = new WoWPlayer(0);
            }
            catch (Exception exception)
            {
                Logging.WriteError("ObjectManager(): " + exception, true);
            }
        }

        public static List<WoWUnit> GetFriendlyUnits()
        {
            List<WoWUnit> list3;
            try
            {
                lock (Locker)
                {
                    List<WoWUnit> list = new List<WoWUnit>();
                    List<UInt128> partyPlayersGUID = nManager.Wow.Helpers.Party.GetPartyPlayersGUID();
                    for (int i = 0; i < _unitList.Count; i++)
                    {
                        WoWUnit item = _unitList[i];
                        if (partyPlayersGUID.Contains(item.Guid))
                        {
                            list.Add(item);
                        }
                    }
                    for (int j = 0; j < _playerList.Count; j++)
                    {
                        WoWUnit unit2 = _playerList[j];
                        if (partyPlayersGUID.Contains(unit2.Guid))
                        {
                            list.Add(unit2);
                        }
                    }
                    if (list.Count == 0)
                    {
                        list.Add(Me);
                    }
                    list3 = list;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetFriendlyUnits(): " + exception, true);
                list3 = new List<WoWUnit>();
            }
            return list3;
        }

        public static List<WoWUnit> GetHostileUnitAttackingPlayer()
        {
            List<WoWUnit> list3;
            try
            {
                List<WoWUnit> list = new List<WoWUnit>();
                List<WoWUnit> unitTargetingPlayer = GetUnitTargetingPlayer();
                Memory.WowMemory.GameFrameLock();
                foreach (WoWUnit unit in unitTargetingPlayer)
                {
                    if (unit.IsHostile && unit.InCombat)
                    {
                        list.Add(unit);
                    }
                }
                Memory.WowMemory.GameFrameUnLock();
                list3 = list;
            }
            finally
            {
                Memory.WowMemory.GameFrameUnLock();
            }
            return list3;
        }

        public static List<WoWUnit> GetHostileUnitTargetingPlayer()
        {
            List<WoWUnit> list3;
            try
            {
                List<WoWUnit> list = new List<WoWUnit>();
                List<WoWUnit> unitTargetingPlayer = GetUnitTargetingPlayer();
                Memory.WowMemory.GameFrameLock();
                foreach (WoWUnit unit in unitTargetingPlayer)
                {
                    if (unit.IsHostile)
                    {
                        list.Add(unit);
                    }
                }
                Memory.WowMemory.GameFrameUnLock();
                list3 = list;
            }
            finally
            {
                Memory.WowMemory.GameFrameUnLock();
            }
            return list3;
        }

        public static WoWGameObject GetNearestWoWGameObject(List<WoWGameObject> listWoWGameObject, bool ignoreBlackList = false)
        {
            try
            {
                WoWGameObject obj2 = new WoWGameObject(0);
                float getDistance = 9999999f;
                foreach (WoWGameObject obj3 in listWoWGameObject)
                {
                    if ((obj3.GetDistance < getDistance) && (!nManagerSetting.IsBlackListed(obj3.Guid) || ignoreBlackList))
                    {
                        obj2 = obj3;
                        getDistance = obj3.GetDistance;
                    }
                }
                return obj2;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetNearestWoWGameObject(List<WoWGameObject> listWoWGameObject): " + exception, true);
            }
            return new WoWGameObject(0);
        }

        public static WoWGameObject GetNearestWoWGameObject(List<WoWGameObject> listWoWGameObject, Point point, bool ignoreBlackList = false)
        {
            try
            {
                WoWGameObject obj2 = new WoWGameObject(0);
                float num = 9999999f;
                foreach (WoWGameObject obj3 in listWoWGameObject)
                {
                    if ((obj3.Position.DistanceTo(point) < num) && (!nManagerSetting.IsBlackListed(obj3.Guid) || ignoreBlackList))
                    {
                        obj2 = obj3;
                        num = obj3.Position.DistanceTo(point);
                    }
                }
                return obj2;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetNearestWoWGameObject(List<WoWGameObject> listWoWGameObject, Point point): " + exception, true);
            }
            return new WoWGameObject(0);
        }

        public static WoWPlayer GetNearestWoWPlayer(List<WoWPlayer> listWoWPlayer)
        {
            try
            {
                WoWPlayer player = new WoWPlayer(0);
                float getDistance = 9999999f;
                foreach (WoWPlayer player2 in listWoWPlayer)
                {
                    if (player2.GetDistance < getDistance)
                    {
                        player = player2;
                        getDistance = player2.GetDistance;
                    }
                }
                return player;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetNearestWoWPlayer(List<WoWPlayer> listWoWPlayer): " + exception, true);
            }
            return new WoWPlayer(0);
        }

        public static WoWUnit GetNearestWoWUnit(List<WoWUnit> listWoWUnit, Point point, bool ignoreBlackList = false)
        {
            try
            {
                WoWUnit unit = new WoWUnit(0);
                float getDistance = 9999999f;
                foreach (WoWUnit unit2 in listWoWUnit)
                {
                    if ((point.DistanceTo(unit2.Position) < getDistance) && (!nManagerSetting.IsBlackListed(unit2.Guid) || ignoreBlackList))
                    {
                        unit = unit2;
                        getDistance = unit2.GetDistance;
                    }
                }
                return unit;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetNearestWoWUnit(List<WoWUnit> listWoWUnit, Point point): " + exception, true);
            }
            return new WoWUnit(0);
        }

        public static WoWUnit GetNearestWoWUnit(List<WoWUnit> listWoWUnit, bool ignorenotSelectable = false, bool ignoreBlackList = false, bool allowPlayedControlled = false)
        {
            try
            {
                WoWUnit unit = new WoWUnit(0);
                float getDistance = 9999999f;
                foreach (WoWUnit unit2 in listWoWUnit)
                {
                    if ((((unit2.GetDistance <= getDistance) && (!nManagerSetting.IsBlackListed(unit2.Guid) || ignoreBlackList)) && (ignorenotSelectable || !unit2.NotSelectable)) && ((!unit2.IsTapped || (unit2.IsTapped && unit2.IsTappedByMe)) && (!unit2.PlayerControlled || allowPlayedControlled)))
                    {
                        unit = unit2;
                        getDistance = unit2.GetDistance;
                    }
                }
                return unit;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetNearestWoWUnit(List<WoWUnit> listWoWUnit): " + exception, true);
            }
            return new WoWUnit(0);
        }

        public static int GetNumberAttackPlayer()
        {
            try
            {
                return GetHostileUnitAttackingPlayer().Count;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetNumberAttackPlayer(): " + exception, true);
            }
            return 0;
        }

        public static int GetNumberWoWUnitZone(Point point, int distanceSearch)
        {
            try
            {
                int num = 0;
                foreach (WoWUnit unit in GetObjectWoWUnit())
                {
                    if ((point.DistanceTo(unit.Position) <= distanceSearch) && !unit.IsDead)
                    {
                        num++;
                    }
                }
                return num;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetNumberWoWUnitZone(Point point, int distanceSearch): " + exception, true);
            }
            return 0;
        }

        public static WoWObject GetObjectByGuid(UInt128 guid)
        {
            WoWObject obj2;
            try
            {
                lock (Locker)
                {
                    obj2 = ObjectDictionary.ContainsKey(guid) ? ObjectDictionary[guid] : new WoWObject(0);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetObjectByGuid(UInt128 guid): " + exception, true);
                obj2 = new WoWObject(0);
            }
            return obj2;
        }

        public static List<WoWContainer> GetObjectWoWContainer()
        {
            try
            {
                List<WoWContainer> list = new List<WoWContainer>();
                foreach (WoWObject obj2 in ObjectList)
                {
                    if (obj2.Type == WoWObjectType.Container)
                    {
                        list.Add(new WoWContainer(obj2.GetBaseAddress));
                    }
                }
                return list;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetObjectWoWContainer(): " + exception, true);
                return new List<WoWContainer>();
            }
        }

        public static List<WoWCorpse> GetObjectWoWCorpse()
        {
            try
            {
                List<WoWCorpse> list = new List<WoWCorpse>();
                foreach (WoWObject obj2 in ObjectList)
                {
                    if (obj2.Type == WoWObjectType.Corpse)
                    {
                        list.Add(new WoWCorpse(obj2.GetBaseAddress));
                    }
                }
                return list;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetObjectWoWCorpse(): " + exception, true);
                return new List<WoWCorpse>();
            }
        }

        public static List<WoWGameObject> GetObjectWoWGameObject()
        {
            List<WoWGameObject> list;
            try
            {
                lock (Locker)
                {
                    list = _gameobjectList.ToList<WoWGameObject>();
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetObjectWoWGameObject(): " + exception, true);
                list = new List<WoWGameObject>();
            }
            return list;
        }

        public static List<WoWItem> GetObjectWoWItem()
        {
            try
            {
                List<WoWItem> list = new List<WoWItem>();
                foreach (WoWObject obj2 in ObjectList)
                {
                    if (obj2.Type == WoWObjectType.Item)
                    {
                        list.Add(new WoWItem(obj2.GetBaseAddress));
                    }
                }
                return list;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetObjectWoWItem(): " + exception, true);
                return new List<WoWItem>();
            }
        }

        public static List<WoWPlayer> GetObjectWoWPlayer()
        {
            List<WoWPlayer> list2;
            try
            {
                lock (Locker)
                {
                    List<WoWPlayer> list = new List<WoWPlayer>();
                    for (int i = 0; i < _playerList.Count; i++)
                    {
                        WoWPlayer item = _playerList[i];
                        if (item.GetBaseAddress != Me.GetBaseAddress)
                        {
                            list.Add(item);
                        }
                    }
                    list2 = list;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetObjectWoWPlayer(): " + exception, true);
                list2 = new List<WoWPlayer>();
            }
            return list2;
        }

        public static WoWPlayer GetObjectWoWPlayer(UInt128 guid)
        {
            WoWPlayer player2;
            try
            {
                lock (Locker)
                {
                    for (int i = 0; i < _playerList.Count; i++)
                    {
                        WoWPlayer player = _playerList[i];
                        if (player.Guid == guid)
                        {
                            return player;
                        }
                    }
                    player2 = null;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetObjectWoWPlayer(UInt128 guid): " + exception, true);
                player2 = null;
            }
            return player2;
        }

        public static List<WoWPlayer> GetObjectWoWPlayerTargetMe()
        {
            try
            {
                List<WoWPlayer> list = new List<WoWPlayer>();
                foreach (WoWPlayer player in GetObjectWoWPlayer())
                {
                    if (player.IsTargetingMe)
                    {
                        list.Add(new WoWPlayer(player.GetBaseAddress));
                    }
                }
                return list;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetObjectWoWPlayerTargetMe(): " + exception, true);
                return new List<WoWPlayer>();
            }
        }

        public static List<WoWUnit> GetObjectWoWUnit()
        {
            List<WoWUnit> list2;
            try
            {
                lock (Locker)
                {
                    List<WoWUnit> list = new List<WoWUnit>();
                    for (int i = 0; i < _unitList.Count; i++)
                    {
                        WoWUnit item = _unitList[i];
                        list.Add(item);
                    }
                    list2 = list;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetObjectWoWUnit(): " + exception, true);
                list2 = new List<WoWUnit>();
            }
            return list2;
        }

        public static List<WoWUnit> GetObjectWoWUnitInCombat()
        {
            try
            {
                List<WoWUnit> list = new List<WoWUnit>();
                foreach (WoWUnit unit in GetObjectWoWUnit())
                {
                    if (unit.InCombat && unit.Attackable)
                    {
                        list.Add(unit);
                    }
                }
                return list;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetObjectWoWUnitInCombat(): " + exception, true);
                return new List<WoWUnit>();
            }
        }

        public static WoWUnit GetUnitInAggroRange()
        {
            foreach (WoWUnit unit in GetObjectWoWUnit())
            {
                if ((((unit.IsValid && unit.IsAlive) && (unit.Attackable && !unit.PlayerControlled)) && ((!unit.NotSelectable && (UnitRelation.GetReaction(Me, unit) == Reaction.Hostile)) && (unit.GetDistance < (unit.AggroDistance * 0.9f)))) && (!unit.InCombat || unit.IsTargetingMe))
                {
                    bool flag;
                    List<Point> listPoints = PathFinder.FindPath(unit.Position, out flag, true, false);
                    if (!flag)
                    {
                        listPoints.Add(unit.Position);
                    }
                    if (nManager.Helpful.Math.DistanceListPoint(listPoints) < (unit.AggroDistance * 3f))
                    {
                        return new WoWUnit(unit.GetBaseAddress);
                    }
                }
            }
            return null;
        }

        public static uint GetUnitInSpellRange(float spellRange = 5f, WoWUnit fromUnit = null)
        {
            if (spellRange < 5f)
            {
                spellRange = 5f;
            }
            uint num = 0;
            foreach (WoWUnit unit in GetObjectWoWUnit())
            {
                if ((!unit.IsValid || !unit.Attackable) || (unit.NotSelectable || !unit.IsHostile))
                {
                    continue;
                }
                if (((fromUnit == null) || !fromUnit.IsValid) || !fromUnit.IsAlive)
                {
                    if (unit.GetDistance <= spellRange)
                    {
                        goto Label_0078;
                    }
                    continue;
                }
                if (unit.Position.DistanceTo(fromUnit.Position) > spellRange)
                {
                    continue;
                }
            Label_0078:
                num++;
            }
            return num;
        }

        public static List<WoWUnit> GetUnitTargetingPlayer()
        {
            List<WoWUnit> list = new List<WoWUnit>();
            List<WoWPlayer> objectWoWPlayer = GetObjectWoWPlayer();
            List<WoWUnit> objectWoWUnit = GetObjectWoWUnit();
            for (int i = 0; i < objectWoWPlayer.Count; i++)
            {
                WoWPlayer item = objectWoWPlayer[i];
                if (((!BlackListMobAttack.Contains(item.Guid) && item.IsValid) && (item.IsAlive && (item.Target != 0))) && ((item.Target == Me.Guid) || (item.Target == Pet.Guid)))
                {
                    list.Add(item);
                }
            }
            for (int j = 0; j < objectWoWUnit.Count; j++)
            {
                WoWUnit unit = objectWoWUnit[j];
                if (((!BlackListMobAttack.Contains(unit.Guid) && unit.IsValid) && (unit.IsAlive && (unit.Target != 0))) && ((unit.Target == Me.Guid) || (unit.Target == Pet.Guid)))
                {
                    list.Add(unit);
                }
            }
            return list;
        }

        public static List<WoWGameObject> GetWoWGameObjectByDisplayId(List<uint> displayId)
        {
            try
            {
                return GetWoWGameObjectByDisplayId(GetObjectWoWGameObject(), displayId);
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWGameObjectByDisplayId(List<int> displayId): " + exception, true);
            }
            return new List<WoWGameObject>();
        }

        public static List<WoWGameObject> GetWoWGameObjectByDisplayId(uint displayId)
        {
            try
            {
                List<uint> list = new List<uint> {
                    displayId
                };
                return GetWoWGameObjectByDisplayId(list);
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWGameObjectByDisplayId(int displayId): " + exception, true);
            }
            return new List<WoWGameObject>();
        }

        public static List<WoWGameObject> GetWoWGameObjectByDisplayId(List<WoWGameObject> listWoWGameObject, List<uint> displayId)
        {
            try
            {
                List<WoWGameObject> list = new List<WoWGameObject>();
                foreach (WoWGameObject obj2 in listWoWGameObject)
                {
                    if (displayId.Contains(obj2.DisplayId))
                    {
                        list.Add(obj2);
                    }
                }
                return list;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWGameObjectByDisplayId(List<WoWGameObject> listWoWGameObject, List<int> displayId): " + exception, true);
            }
            return new List<WoWGameObject>();
        }

        public static List<WoWGameObject> GetWoWGameObjectByEntry(List<int> entry)
        {
            try
            {
                return GetWoWGameObjectByEntry(GetObjectWoWGameObject(), entry);
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWGameObjectByEntry(List<int> entry): " + exception, true);
            }
            return new List<WoWGameObject>();
        }

        public static List<WoWGameObject> GetWoWGameObjectByEntry(int entry)
        {
            try
            {
                List<int> list = new List<int> {
                    entry
                };
                return GetWoWGameObjectByEntry(list);
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWGameObjectByEntry(int entry): " + exception, true);
            }
            return new List<WoWGameObject>();
        }

        public static List<WoWGameObject> GetWoWGameObjectByEntry(List<WoWGameObject> listWoWGameObject, List<int> entry)
        {
            try
            {
                List<WoWGameObject> list = new List<WoWGameObject>();
                foreach (WoWGameObject obj2 in listWoWGameObject)
                {
                    if (entry.Contains(obj2.Entry))
                    {
                        list.Add(obj2);
                    }
                }
                return list;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWGameObjectByEntry(List<WoWGameObject> listWoWGameObject, List<int> entry): " + exception, true);
            }
            return new List<WoWGameObject>();
        }

        public static List<WoWGameObject> GetWoWGameObjectById(int id)
        {
            try
            {
                List<int> list = new List<int> {
                    id
                };
                return GetWoWGameObjectById(list);
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWGameObjectByyId(int id): " + exception, true);
            }
            return new List<WoWGameObject>();
        }

        public static List<WoWGameObject> GetWoWGameObjectById(List<int> id)
        {
            try
            {
                return GetWoWGameObjectById(GetObjectWoWGameObject(), id);
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWGameObjectById(List<int> id): " + exception, true);
            }
            return new List<WoWGameObject>();
        }

        public static List<WoWGameObject> GetWoWGameObjectById(List<WoWGameObject> listWoWGameObject, List<int> id)
        {
            try
            {
                List<WoWGameObject> list = new List<WoWGameObject>();
                foreach (WoWGameObject obj2 in listWoWGameObject)
                {
                    if (id.Contains(obj2.Entry))
                    {
                        list.Add(obj2);
                    }
                }
                return list;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWGameObjectById(List<WoWGameObject> listWoWGameObject, List<int> id): " + exception, true);
            }
            return new List<WoWGameObject>();
        }

        public static List<WoWGameObject> GetWoWGameObjectByName(string name)
        {
            try
            {
                return GetWoWGameObjectByName(GetObjectWoWGameObject(), name);
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWGameObjectByName(string name): " + exception, true);
            }
            return new List<WoWGameObject>();
        }

        public static List<WoWGameObject> GetWoWGameObjectByName(List<string> names)
        {
            try
            {
                return GetWoWGameObjectByName(GetObjectWoWGameObject(), names);
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWGameObjectByName(List<string> names): " + exception, true);
            }
            return new List<WoWGameObject>();
        }

        public static List<WoWGameObject> GetWoWGameObjectByName(List<WoWGameObject> listWoWGameObject, string name)
        {
            try
            {
                List<string> names = new List<string> {
                    name
                };
                return GetWoWGameObjectByName(listWoWGameObject, names);
            }
            catch (Exception exception)
            {
                Logging.WriteError(" GetWoWGameObjectByName(List<WoWGameObject> listWoWGameObject, string name): " + exception, true);
            }
            return new List<WoWGameObject>();
        }

        public static List<WoWGameObject> GetWoWGameObjectByName(List<WoWGameObject> listWoWGameObject, List<string> names)
        {
            try
            {
                List<string> list = new List<string>();
                foreach (string str in names)
                {
                    list.Add(str.ToLower());
                }
                List<WoWGameObject> list2 = new List<WoWGameObject>();
                foreach (WoWGameObject obj2 in listWoWGameObject)
                {
                    if (list.Contains(obj2.Name.ToLower()))
                    {
                        list2.Add(obj2);
                    }
                }
                return list2;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWGameObjectByName(List<WoWGameObject> listWoWGameObject, List<string> names): " + exception, true);
            }
            return new List<WoWGameObject>();
        }

        public static List<WoWGameObject> GetWoWGameObjectForFarm()
        {
            try
            {
                List<WoWGameObject> list = new List<WoWGameObject>();
                foreach (WoWGameObject obj2 in GetObjectWoWGameObject())
                {
                    if ((obj2.GOType == WoWGameObjectType.Chest) || (obj2.GOType == WoWGameObjectType.Goober))
                    {
                        list.Add(obj2);
                    }
                }
                return list;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWGameObjectForFarm(): " + exception, true);
            }
            return new List<WoWGameObject>();
        }

        public static List<WoWGameObject> GetWoWGameObjectOfType(WoWGameObjectType reqtype)
        {
            try
            {
                List<WoWGameObject> list = new List<WoWGameObject>();
                lock (Locker)
                {
                    foreach (WoWGameObject obj2 in _gameobjectList)
                    {
                        if (obj2.GOType == reqtype)
                        {
                            list.Add(obj2);
                        }
                    }
                }
                return list;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWGameObjectOfType(WoWGameObjectType reqtype): " + exception, true);
                return new List<WoWGameObject>();
            }
        }

        public static WoWItem GetWoWItemById(int entry)
        {
            try
            {
                foreach (WoWItem item in GetObjectWoWItem())
                {
                    if (item.Entry == entry)
                    {
                        return new WoWItem(item.GetBaseAddress);
                    }
                }
                return new WoWItem(0);
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWItemById(int entry): " + exception, true);
                return new WoWItem(0);
            }
        }

        public static List<WoWPlayer> GetWoWPlayerDead()
        {
            try
            {
                return GetWoWPlayerDead(GetObjectWoWPlayer());
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWPlayerDead(): " + exception, true);
            }
            return new List<WoWPlayer>();
        }

        public static List<WoWPlayer> GetWoWPlayerDead(List<WoWPlayer> listWoWPlayer)
        {
            try
            {
                List<WoWPlayer> list = new List<WoWPlayer>();
                foreach (WoWPlayer player in listWoWPlayer)
                {
                    if (player.IsDead)
                    {
                        list.Add(player);
                    }
                }
                return list;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWPlayerDead(List<WoWPlayer> listWoWPlayer): " + exception, true);
            }
            return new List<WoWPlayer>();
        }

        public static List<WoWPlayer> GetWoWPlayerLootable()
        {
            try
            {
                return GetWoWPlayerLootable(GetObjectWoWPlayer());
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWPlayerLootable(): " + exception, true);
            }
            return new List<WoWPlayer>();
        }

        public static List<WoWPlayer> GetWoWPlayerLootable(List<WoWPlayer> listWoWPlayer)
        {
            try
            {
                List<WoWPlayer> list = new List<WoWPlayer>();
                foreach (WoWPlayer player in listWoWPlayer)
                {
                    if (player.IsLootable)
                    {
                        list.Add(player);
                    }
                }
                return list;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWPlayerLootable(List<WoWPlayer> listWoWPlayer): " + exception, true);
            }
            return new List<WoWPlayer>();
        }

        public static List<WoWPlayer> GetWoWUnitAlliance()
        {
            try
            {
                return GetWoWUnitAlliance(GetObjectWoWPlayer());
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitAlliance(): " + exception, true);
            }
            return new List<WoWPlayer>();
        }

        public static List<WoWPlayer> GetWoWUnitAlliance(List<WoWPlayer> listWoWUnit)
        {
            try
            {
                List<WoWPlayer> list = new List<WoWPlayer>();
                foreach (WoWPlayer player in listWoWUnit)
                {
                    if (((player.PlayerFaction == "Alliance") && !player.IsDead) && (player.SummonedBy == 0))
                    {
                        list.Add(player);
                    }
                }
                return list;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitAlliance(List<WoWPlayer> listWoWUnit): " + exception, true);
            }
            return new List<WoWPlayer>();
        }

        public static List<WoWPlayer> GetWoWUnitAllianceDead()
        {
            try
            {
                return GetWoWUnitAllianceDead(GetObjectWoWPlayer());
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitAllianceDead(): " + exception, true);
            }
            return new List<WoWPlayer>();
        }

        public static List<WoWPlayer> GetWoWUnitAllianceDead(List<WoWPlayer> listWoWUnit)
        {
            try
            {
                List<WoWPlayer> list = new List<WoWPlayer>();
                foreach (WoWPlayer player in listWoWUnit)
                {
                    if (((player.PlayerFaction == "Alliance") && player.IsDead) && (player.SummonedBy == 0))
                    {
                        list.Add(player);
                    }
                }
                return list;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitAllianceDead(List<WoWPlayer> listWoWUnit): " + exception, true);
            }
            return new List<WoWPlayer>();
        }

        public static List<WoWUnit> GetWoWUnitAuctioneer()
        {
            try
            {
                return GetWoWUnitAuctioneer(GetObjectWoWUnit());
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitAuctioneer(): " + exception, true);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitAuctioneer(List<WoWUnit> listWoWUnit)
        {
            try
            {
                List<WoWUnit> list = new List<WoWUnit>();
                foreach (WoWUnit unit in listWoWUnit)
                {
                    if (unit.IsNpcAuctioneer)
                    {
                        list.Add(unit);
                    }
                }
                return list;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitAuctioneer(List<WoWUnit> listWoWUnit): " + exception, true);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitByEntry(int entry, bool isDead = false)
        {
            try
            {
                return GetWoWUnitByEntry(GetObjectWoWUnit(), entry, isDead);
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitByEntry(int entry): " + exception, true);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitByEntry(List<int> entrys, bool isDead = false)
        {
            try
            {
                return GetWoWUnitByEntry(GetObjectWoWUnit(), entrys, isDead);
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitByEntry(List<int> entrys): " + exception, true);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitByEntry(List<WoWUnit> listWoWUnit, List<int> entrys, bool isDead = false)
        {
            try
            {
                List<WoWUnit> list = new List<WoWUnit>();
                foreach (WoWUnit unit in listWoWUnit)
                {
                    if ((entrys.Contains(unit.Entry) && ((!isDead && !unit.IsDead) || isDead)) && !unit.Invisible)
                    {
                        list.Add(unit);
                    }
                }
                return list;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitByEntry(List<WoWUnit> listWoWUnit, List<int> entrys): " + exception, true);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitByEntry(List<WoWUnit> listWoWUnit, int entry, bool isDead = false)
        {
            try
            {
                List<int> entrys = new List<int> {
                    entry
                };
                return GetWoWUnitByEntry(listWoWUnit, entrys, isDead);
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitByEntry(List<WoWUnit> listWoWUnit, int entry): " + exception, true);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitByFaction(uint faction)
        {
            try
            {
                return GetWoWUnitByFaction(GetObjectWoWUnit(), faction);
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitByFaction(uint faction): " + exception, true);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitByFaction(List<WoWUnit> listWoWUnit, uint faction)
        {
            try
            {
                List<uint> factions = new List<uint> {
                    faction
                };
                return GetWoWUnitByFaction(listWoWUnit, factions, false);
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitByFaction(List<WoWUnit> listWoWUnit, uint faction): " + exception, true);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitByFaction(List<uint> factions, bool pvp = false)
        {
            try
            {
                return GetWoWUnitByFaction(GetObjectWoWUnit(), factions, pvp);
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitByFaction(List<uint> factions, bool pvp = false): " + exception, true);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitByFaction(List<WoWUnit> listWoWUnit, List<uint> factions, bool pvp = false)
        {
            try
            {
                List<WoWUnit> list = new List<WoWUnit>();
                foreach (WoWUnit unit in listWoWUnit)
                {
                    try
                    {
                        if (((factions.Contains(unit.Faction) && !unit.IsDead) && !unit.Invisible) && ((pvp || !unit.InCombat) || unit.InCombatWithMe))
                        {
                            list.Add(unit);
                        }
                    }
                    catch (Exception exception)
                    {
                        Logging.WriteError("GetWoWUnitByFaction(List<WoWUnit> listWoWUnit, List<uint> factions, bool pvp = false)#1: " + exception, true);
                    }
                }
                return list;
            }
            catch (Exception exception2)
            {
                Logging.WriteError("GetWoWUnitByFaction(List<WoWUnit> listWoWUnit, List<uint> factions, bool pvp = false)#2: " + exception2, true);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitByName(string name)
        {
            try
            {
                return GetWoWUnitByName(GetObjectWoWUnit(), name);
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitByName(string name): " + exception, true);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitByName(List<string> names)
        {
            try
            {
                return GetWoWUnitByName(GetObjectWoWUnit(), names);
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitByName(List<string> names): " + exception, true);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitByName(List<WoWUnit> listWoWUnit, string name)
        {
            try
            {
                List<string> names = new List<string> {
                    name
                };
                return GetWoWUnitByName(listWoWUnit, names);
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitByName(List<WoWUnit> listWoWUnit, string name): " + exception, true);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitByName(List<WoWUnit> listWoWUnit, List<string> names)
        {
            try
            {
                List<WoWUnit> list = new List<WoWUnit>();
                foreach (WoWUnit unit in listWoWUnit)
                {
                    if ((names.Contains(unit.Name) && !unit.IsDead) && !unit.Invisible)
                    {
                        list.Add(unit);
                    }
                }
                return list;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitByName(List<WoWUnit> listWoWUnit, List<string> names): " + exception, true);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitByQuestLoot(int lootId)
        {
            try
            {
                return GetWoWUnitByQuestLoot(GetObjectWoWUnit(), lootId);
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitByQuestLoot(int lootId): " + exception, true);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitByQuestLoot(List<WoWUnit> listWoWUnit, int lootId)
        {
            try
            {
                List<WoWUnit> list = new List<WoWUnit>();
                foreach (WoWUnit unit in listWoWUnit)
                {
                    if ((((unit.QuestItem1 == lootId) || (unit.QuestItem2 == lootId)) || ((unit.QuestItem3 == lootId) || (unit.QuestItem4 == lootId))) && !unit.Invisible)
                    {
                        list.Add(unit);
                    }
                }
                return list;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitByQuestLoot(List<WoWUnit> listWoWUnit, int lootId): " + exception, true);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitFlightMaster()
        {
            try
            {
                return GetWoWUnitFlightMaster(GetObjectWoWUnit());
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitFlightMaster(): " + exception, true);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitFlightMaster(List<WoWUnit> listWoWUnit)
        {
            try
            {
                List<WoWUnit> list = new List<WoWUnit>();
                foreach (WoWUnit unit in listWoWUnit)
                {
                    if (unit.IsNpcFlightMaster)
                    {
                        list.Add(unit);
                    }
                }
                return list;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitFlightMaster(List<WoWUnit> listWoWUnit): " + exception, true);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWPlayer> GetWoWUnitHorde()
        {
            try
            {
                return GetWoWUnitHorde(GetObjectWoWPlayer());
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitHorde(): " + exception, true);
            }
            return new List<WoWPlayer>();
        }

        public static List<WoWPlayer> GetWoWUnitHorde(List<WoWPlayer> listWoWUnit)
        {
            try
            {
                List<WoWPlayer> list = new List<WoWPlayer>();
                foreach (WoWPlayer player in listWoWUnit)
                {
                    if (((player.PlayerFaction == "Horde") && !player.IsDead) && (player.SummonedBy == 0))
                    {
                        list.Add(player);
                    }
                }
                return list;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitHorde(List<WoWPlayer> listWoWUnit): " + exception, true);
            }
            return new List<WoWPlayer>();
        }

        public static List<WoWPlayer> GetWoWUnitHordeDead()
        {
            try
            {
                return GetWoWUnitHordeDead(GetObjectWoWPlayer());
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitHordeDead(): " + exception, true);
            }
            return new List<WoWPlayer>();
        }

        public static List<WoWPlayer> GetWoWUnitHordeDead(List<WoWPlayer> listWoWUnit)
        {
            try
            {
                List<WoWPlayer> list = new List<WoWPlayer>();
                foreach (WoWPlayer player in listWoWUnit)
                {
                    if (((player.PlayerFaction == "Horde") && player.IsDead) && (player.SummonedBy == 0))
                    {
                        list.Add(player);
                    }
                }
                return list;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitHordeDead(List<WoWPlayer> listWoWUnit): " + exception, true);
            }
            return new List<WoWPlayer>();
        }

        public static List<WoWUnit> GetWoWUnitHostile()
        {
            try
            {
                List<WoWUnit> list = new List<WoWUnit>();
                foreach (WoWUnit unit in GetObjectWoWUnit())
                {
                    if (((UnitRelation.GetReaction(Me.Faction, unit.Faction) == Reaction.Hostile) && !unit.IsDead) && (!unit.InCombat || unit.InCombatWithMe))
                    {
                        list.Add(unit);
                    }
                }
                return list;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitHostile(): " + exception, true);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitInkeeper()
        {
            try
            {
                return GetWoWUnitInkeeper(GetObjectWoWUnit());
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitInkeeper(): " + exception, true);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitInkeeper(List<WoWUnit> listWoWUnit)
        {
            try
            {
                List<WoWUnit> list = new List<WoWUnit>();
                foreach (WoWUnit unit in listWoWUnit)
                {
                    if (unit.IsNpcInnkeeper)
                    {
                        list.Add(unit);
                    }
                }
                return list;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitInkeeper(List<WoWUnit> listWoWUnit): " + exception, true);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitLootable()
        {
            try
            {
                return GetWoWUnitLootable(GetObjectWoWUnit());
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitLootable(): " + exception, true);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitLootable(List<WoWUnit> listWoWUnit)
        {
            try
            {
                List<WoWUnit> list = new List<WoWUnit>();
                foreach (WoWUnit unit in listWoWUnit)
                {
                    if (unit.IsLootable)
                    {
                        list.Add(unit);
                    }
                }
                return list;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitLootable(List<WoWUnit> listWoWUnit): " + exception, true);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitMailbox()
        {
            try
            {
                return GetWoWUnitMailbox(GetObjectWoWUnit());
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitMailbox(): " + exception, true);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitMailbox(List<WoWUnit> listWoWUnit)
        {
            try
            {
                List<WoWUnit> list = new List<WoWUnit>();
                foreach (WoWUnit unit in listWoWUnit)
                {
                    if (unit.IsNpcMailbox)
                    {
                        list.Add(unit);
                    }
                }
                return list;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitMailbox(List<WoWUnit> listWoWUnit): " + exception, true);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitQuester()
        {
            try
            {
                return GetWoWUnitQuester(GetObjectWoWUnit());
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitQuester(): " + exception, true);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitQuester(List<WoWUnit> listWoWUnit)
        {
            try
            {
                List<WoWUnit> list = new List<WoWUnit>();
                foreach (WoWUnit unit in listWoWUnit)
                {
                    if (unit.IsNpcQuestGiver)
                    {
                        list.Add(unit);
                    }
                }
                return list;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitQuester(List<WoWUnit> listWoWUnit): " + exception, true);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitRepair()
        {
            try
            {
                return GetWoWUnitRepair(GetObjectWoWUnit());
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitRepair(): " + exception, true);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitRepair(List<WoWUnit> listWoWUnit)
        {
            try
            {
                List<WoWUnit> list = new List<WoWUnit>();
                foreach (WoWUnit unit in listWoWUnit)
                {
                    if (unit.IsNpcRepair && !unit.IsNpcInnkeeper)
                    {
                        list.Add(unit);
                    }
                }
                return list;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitRepair(List<WoWUnit> listWoWUnit): " + exception, true);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitSkinnable(List<UInt128> withoutGuid)
        {
            try
            {
                return GetWoWUnitSkinnable(GetObjectWoWUnit(), withoutGuid);
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitSkinnable(List<UInt128> withoutGuid): " + exception, true);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitSkinnable(List<WoWUnit> listWoWUnit, List<UInt128> withoutGuid)
        {
            try
            {
                List<WoWUnit> list = new List<WoWUnit>();
                int num = Skill.GetValue(SkillLine.Skinning);
                int num2 = Skill.GetValue(SkillLine.Herbalism);
                if (num2 > 0)
                {
                    num2 += Skill.GetSkillBonus(SkillLine.Herbalism);
                }
                int num3 = Skill.GetValue(SkillLine.Mining);
                if (num3 > 0)
                {
                    num3 += Skill.GetSkillBonus(SkillLine.Mining);
                }
                int num4 = Skill.GetValue(SkillLine.Engineering);
                if (num4 > 0)
                {
                    num4 += Skill.GetSkillBonus(SkillLine.Engineering);
                }
                foreach (WoWUnit unit in listWoWUnit)
                {
                    if (unit.IsSkinnable && !withoutGuid.Contains(unit.Guid))
                    {
                        if (unit.ExtraLootType.HasFlag(TypeFlag.HERB_LOOT))
                        {
                            if (unit.GetSkillLevelRequired > num2)
                            {
                                continue;
                            }
                            list.Add(unit);
                        }
                        if (unit.ExtraLootType.HasFlag(TypeFlag.MINING_LOOT))
                        {
                            if (unit.GetSkillLevelRequired > num3)
                            {
                                continue;
                            }
                            list.Add(unit);
                        }
                        if (unit.ExtraLootType.HasFlag(TypeFlag.ENGENEERING_LOOT))
                        {
                            if (unit.GetSkillLevelRequired > num4)
                            {
                                continue;
                            }
                            list.Add(unit);
                        }
                        if (num > 0)
                        {
                            list.Add(unit);
                        }
                    }
                }
                return list;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitSkinnable(List<WoWUnit> listWoWUnit, List<UInt128> withoutGuid): " + exception, true);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitSpiritGuide()
        {
            try
            {
                return GetWoWUnitSpiritGuide(GetObjectWoWUnit());
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitSpiritGuide(): " + exception, true);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitSpiritGuide(List<WoWUnit> listWoWUnit)
        {
            try
            {
                List<WoWUnit> list = new List<WoWUnit>();
                foreach (WoWUnit unit in listWoWUnit)
                {
                    if (unit.IsNpcSpiritGuide)
                    {
                        list.Add(unit);
                    }
                }
                return list;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitSpiritGuide(List<WoWUnit> listWoWUnit): " + exception, true);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitSpiritHealer()
        {
            try
            {
                return GetWoWUnitSpiritHealer(GetObjectWoWUnit());
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitSpiritHealer(): " + exception, true);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitSpiritHealer(List<WoWUnit> listWoWUnit)
        {
            try
            {
                List<WoWUnit> list = new List<WoWUnit>();
                foreach (WoWUnit unit in listWoWUnit)
                {
                    if (unit.IsNpcSpiritHealer)
                    {
                        list.Add(unit);
                    }
                }
                return list;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitSpiritHealer(List<WoWUnit> listWoWUnit): " + exception, true);
            }
            return new List<WoWUnit>();
        }

        public static WoWUnit GetWoWUnitSummonedOrCreatedByMeAndFighting()
        {
            try
            {
                return GetWoWUnitSummonedOrCreatedByMeAndFighting(GetObjectWoWUnit());
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitSummonedOrCreatedByMeAndFighting(): " + exception, true);
            }
            return new WoWUnit(0);
        }

        public static WoWUnit GetWoWUnitSummonedOrCreatedByMeAndFighting(List<WoWUnit> listWoWUnit)
        {
            try
            {
                foreach (WoWUnit unit in listWoWUnit)
                {
                    if (((unit.SummonedBy == Me.Guid) || (unit.CreatedBy == Me.Guid)) && (unit.Target != 0))
                    {
                        WoWUnit unit2 = new WoWUnit(GetObjectByGuid(unit.Target).GetBaseAddress);
                        if ((unit2.IsValid && unit2.InCombat) && (unit2.Target == unit.Guid))
                        {
                            return unit;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitSummonedOrCreatedByMeAndFighting(List<WoWUnit> listWoWUnit): " + exception, true);
            }
            return new WoWUnit(0);
        }

        public static List<WoWUnit> GetWoWUnitTrainer()
        {
            try
            {
                return GetWoWUnitTrainer(GetObjectWoWUnit());
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitTrainer(): " + exception, true);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitTrainer(List<WoWUnit> listWoWUnit)
        {
            try
            {
                List<WoWUnit> list = new List<WoWUnit>();
                foreach (WoWUnit unit in listWoWUnit)
                {
                    if (unit.IsNpcTrainer)
                    {
                        list.Add(unit);
                    }
                }
                return list;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitTrainer(List<WoWUnit> listWoWUnit): " + exception, true);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitVendor()
        {
            try
            {
                return GetWoWUnitVendor(GetObjectWoWUnit());
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitVendor(): " + exception, true);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitVendor(List<WoWUnit> listWoWUnit)
        {
            try
            {
                List<WoWUnit> list = new List<WoWUnit>();
                foreach (WoWUnit unit in listWoWUnit)
                {
                    if (unit.IsNpcVendor && !unit.IsNpcInnkeeper)
                    {
                        list.Add(unit);
                    }
                }
                return list;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitVendor(List<WoWUnit> listWoWUnit): " + exception, true);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitVendorFood()
        {
            try
            {
                return GetWoWUnitVendorFood(GetObjectWoWUnit());
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitVendorFood(): " + exception, true);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitVendorFood(List<WoWUnit> listWoWUnit)
        {
            try
            {
                List<WoWUnit> list = new List<WoWUnit>();
                foreach (WoWUnit unit in listWoWUnit)
                {
                    if (unit.IsNpcVendorFood && !unit.IsNpcInnkeeper)
                    {
                        list.Add(unit);
                    }
                }
                return list;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitVendorFood(List<WoWUnit> listWoWUnit): " + exception, true);
            }
            return new List<WoWUnit>();
        }

        public static WoWPlayer GetWoWUnitWGFlagHolder(bool hostileHolder = true)
        {
            List<WoWPlayer> source = new List<WoWPlayer>();
            if (hostileHolder)
            {
                source.AddRange((Me.PlayerFaction.ToLower() == "horde") ? GetWoWUnitAlliance() : GetWoWUnitHorde());
            }
            else
            {
                source.AddRange((Me.PlayerFaction.ToLower() == "horde") ? GetWoWUnitHorde() : GetWoWUnitAlliance());
            }
            return source.FirstOrDefault<WoWPlayer>(wowPlayer => wowPlayer.IsHoldingWGFlag);
        }

        public static bool IsSomeoneHoldingWGFlag(bool hostileHolder = true)
        {
            return (GetWoWUnitWGFlagHolder(hostileHolder) != null);
        }

        internal static void Pulse()
        {
            try
            {
                lock (Locker)
                {
                    foreach (KeyValuePair<UInt128, WoWObject> pair in ObjectDictionary)
                    {
                        pair.Value.UpdateBaseAddress(0);
                    }
                    ReadObjectList();
                    List<UInt128> list = new List<UInt128>();
                    _objectList = new List<WoWObject>();
                    _unitList = new List<WoWUnit>();
                    _playerList = new List<WoWPlayer>();
                    _gameobjectList = new List<WoWGameObject>();
                    foreach (KeyValuePair<UInt128, WoWObject> pair2 in ObjectDictionary)
                    {
                        if (pair2.Value.IsValid)
                        {
                            switch (pair2.Value.Type)
                            {
                                case WoWObjectType.Unit:
                                {
                                    _unitList.Add(new WoWUnit(pair2.Value.GetBaseAddress));
                                    continue;
                                }
                                case WoWObjectType.Player:
                                {
                                    _playerList.Add(new WoWPlayer(pair2.Value.GetBaseAddress));
                                    continue;
                                }
                                case WoWObjectType.GameObject:
                                {
                                    _gameobjectList.Add(new WoWGameObject(pair2.Value.GetBaseAddress));
                                    continue;
                                }
                            }
                            _objectList.Add(pair2.Value);
                        }
                        else
                        {
                            list.Add(pair2.Key);
                        }
                    }
                    foreach (UInt128 num in list)
                    {
                        WoWObject obj2;
                        ObjectDictionary.TryRemove(num, out obj2);
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("ObjectManager > Pulse(): " + exception, true);
            }
        }

        internal static void ReadObjectList()
        {
            try
            {
                int num4;
                while (Addresses.ObjectManagerClass.sCurMgr == 0)
                {
                    Thread.Sleep(10);
                }
                ObjectManagerAddress = Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + Addresses.ObjectManagerClass.sCurMgr);
                UInt128 num = Memory.WowMemory.Memory.ReadUInt128(ObjectManagerAddress + 0xf8);
                Usefuls.ContinentId = Memory.WowMemory.Memory.ReadInt(ObjectManagerAddress + 0x108);
                for (int i = Memory.WowMemory.Memory.ReadInt(ObjectManagerAddress + 0xd8); i != 0; i = num4)
                {
                    try
                    {
                        UInt128 key = Memory.WowMemory.Memory.ReadUInt128((uint) (i + 40));
                        if (!ObjectDictionary.ContainsKey(key))
                        {
                            WoWObjectType type = (WoWObjectType) Memory.WowMemory.Memory.ReadInt((uint) (i + 12));
                            WoWObject obj2 = null;
                            switch (type)
                            {
                                case WoWObjectType.Object:
                                    obj2 = new WoWObject((uint) i);
                                    break;

                                case WoWObjectType.Item:
                                    obj2 = new WoWItem((uint) i);
                                    break;

                                case WoWObjectType.Container:
                                    obj2 = new WoWContainer((uint) i);
                                    break;

                                case WoWObjectType.Unit:
                                    obj2 = new WoWUnit((uint) i);
                                    break;

                                case WoWObjectType.Player:
                                    if (num == key)
                                    {
                                        Me.UpdateBaseAddress((uint) i);
                                    }
                                    obj2 = new WoWPlayer((uint) i);
                                    break;

                                case WoWObjectType.GameObject:
                                    obj2 = new WoWGameObject((uint) i);
                                    break;

                                case WoWObjectType.DynamicObject:
                                    obj2 = new WoWGameObject((uint) i);
                                    break;

                                case WoWObjectType.Corpse:
                                    obj2 = new WoWCorpse((uint) i);
                                    break;
                            }
                            if (obj2 != null)
                            {
                                ObjectDictionary.TryAdd(key, obj2);
                            }
                        }
                        else
                        {
                            ObjectDictionary[key].UpdateBaseAddress((uint) i);
                        }
                    }
                    catch (Exception exception)
                    {
                        Logging.WriteError("ObjectManager >  ReadObjectList()#1: " + exception, true);
                    }
                    num4 = Memory.WowMemory.Memory.ReadInt((uint) (i + 60));
                    if (num4 == i)
                    {
                        return;
                    }
                }
            }
            catch (Exception exception2)
            {
                Logging.WriteError("ObjectManager >  ReadObjectList()#2: " + exception2, true);
            }
        }

        public static WoWPlayer Me
        {
            [CompilerGenerated]
            get
            {
                return <Me>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                <Me>k__BackingField = value;
            }
        }

        public static ConcurrentDictionary<UInt128, WoWObject> ObjectDictionary
        {
            [CompilerGenerated]
            get
            {
                return <ObjectDictionary>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <ObjectDictionary>k__BackingField = value;
            }
        }

        public static List<WoWObject> ObjectList
        {
            get
            {
                lock (Locker)
                {
                    return _objectList.ToList<WoWObject>();
                }
            }
        }

        private static uint ObjectManagerAddress
        {
            [CompilerGenerated]
            get
            {
                return <ObjectManagerAddress>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <ObjectManagerAddress>k__BackingField = value;
            }
        }

        public static WoWUnit Pet
        {
            get
            {
                try
                {
                    UInt128 guid = Memory.WowMemory.Memory.ReadUInt128(Memory.WowProcess.WowModule + 0xe18ba0);
                    if (guid > 0)
                    {
                        return new WoWUnit(GetObjectByGuid(guid).GetBaseAddress);
                    }
                    WoWUnit woWUnitSummonedOrCreatedByMeAndFighting = GetWoWUnitSummonedOrCreatedByMeAndFighting();
                    if (woWUnitSummonedOrCreatedByMeAndFighting.IsValid)
                    {
                        return woWUnitSummonedOrCreatedByMeAndFighting;
                    }
                    return new WoWUnit(0);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit Pet: " + exception, true);
                    return new WoWUnit(0);
                }
            }
        }

        public static WoWUnit Target
        {
            get
            {
                try
                {
                    if (Me.IsValid && (Me.Target > 0))
                    {
                        if (_lastTargetBase > 0)
                        {
                            if (new WoWUnit(_lastTargetBase).Guid == Me.Target)
                            {
                                return new WoWUnit(_lastTargetBase);
                            }
                            _lastTargetBase = 0;
                        }
                        WoWUnit unit = new WoWUnit(GetObjectByGuid(Me.Target).GetBaseAddress);
                        _lastTargetBase = unit.GetBaseAddress;
                        return unit;
                    }
                }
                catch (Exception exception)
                {
                    Logging.WriteError("Target: " + exception, true);
                }
                return new WoWUnit(0);
            }
        }
    }
}

