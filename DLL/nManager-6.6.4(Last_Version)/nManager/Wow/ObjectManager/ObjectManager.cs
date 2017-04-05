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
        private static List<uint> _egoefacoehio = new List<uint>();
        private static List<WoWAreaTrigger> _eviebupeuc;
        private static uint _fowoulowiof;
        private static List<WoWPlayer> _igeaxow;
        private static List<WoWDynamicObject> _ilefanaetineAduek;
        private static List<WoWUnit> _luqiocixuNu;
        private static List<WoWUnit> _nojaepeakicof;
        private static uint _ocikouhadojoUbioc;
        private static List<WoWObject> _omiaweubaw;
        private static List<WoWGameObject> _qeoqiehipigo;
        public static List<UInt128> BlackListMobAttack = new List<UInt128>();
        private static readonly object Locker = new object();
        public static List<int> TotemEntryList;

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

        internal static void Foamei()
        {
            try
            {
                lock (Locker)
                {
                    foreach (KeyValuePair<UInt128, WoWObject> pair in ObjectDictionary)
                    {
                        pair.Value.Aniubekuni(0);
                    }
                    OmisiepapJer();
                    List<UInt128> list = new List<UInt128>();
                    _omiaweubaw = new List<WoWObject>();
                    _luqiocixuNu = new List<WoWUnit>();
                    _nojaepeakicof = new List<WoWUnit>();
                    _ilefanaetineAduek = new List<WoWDynamicObject>();
                    _eviebupeuc = new List<WoWAreaTrigger>();
                    _igeaxow = new List<WoWPlayer>();
                    _qeoqiehipigo = new List<WoWGameObject>();
                    foreach (KeyValuePair<UInt128, WoWObject> pair2 in ObjectDictionary)
                    {
                        if (pair2.Value.IsValid)
                        {
                            switch (pair2.Value.Type)
                            {
                                case WoWObjectType.Unit:
                                {
                                    WoWUnit item = new WoWUnit(pair2.Value.GetBaseAddress);
                                    _luqiocixuNu.Add(item);
                                    if (item.IsInRange(60f))
                                    {
                                        _nojaepeakicof.Add(item);
                                    }
                                    continue;
                                }
                                case WoWObjectType.Player:
                                {
                                    _igeaxow.Add(new WoWPlayer(pair2.Value.GetBaseAddress));
                                    continue;
                                }
                                case WoWObjectType.GameObject:
                                {
                                    _qeoqiehipigo.Add(new WoWGameObject(pair2.Value.GetBaseAddress));
                                    continue;
                                }
                                case WoWObjectType.DynamicObject:
                                {
                                    continue;
                                }
                                case WoWObjectType.AreaTrigger:
                                {
                                    _eviebupeuc.Add(new WoWAreaTrigger(pair2.Value.GetBaseAddress));
                                    continue;
                                }
                            }
                            _omiaweubaw.Add(pair2.Value);
                            continue;
                        }
                        list.Add(pair2.Key);
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

        public static List<WoWUnit> GetFriendlyUnits()
        {
            List<WoWUnit> list3;
            try
            {
                lock (Locker)
                {
                    List<WoWUnit> list = new List<WoWUnit>();
                    List<UInt128> partyPlayersGUID = nManager.Wow.Helpers.Party.GetPartyPlayersGUID();
                    for (int i = 0; i < _luqiocixuNu.Count; i++)
                    {
                        WoWUnit item = _luqiocixuNu[i];
                        if (partyPlayersGUID.Contains(item.Guid))
                        {
                            list.Add(item);
                        }
                    }
                    for (int j = 0; j < _igeaxow.Count; j++)
                    {
                        WoWUnit unit2 = _igeaxow[j];
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
                    if ((unit.IsHostile && unit.InCombat) && unit.Attackable)
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

        public static List<WoWUnit> GetHostileUnitNearPlayer()
        {
            List<WoWUnit> list3;
            try
            {
                List<WoWUnit> list = new List<WoWUnit>();
                List<WoWUnit> objectWoWUnitInCombat = GetObjectWoWUnitInCombat();
                Memory.WowMemory.GameFrameLock();
                foreach (WoWUnit unit in objectWoWUnitInCombat)
                {
                    if ((unit.IsHostile && unit.InCombat) && unit.Attackable)
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
                return GetNearestWoWGameObject(listWoWGameObject, Me.Position, ignoreBlackList);
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

        public static WoWUnit GetNearestWoWUnit(List<WoWUnit> listWoWUnit, bool ignorenotSelectable = false, bool ignoreBlackList = false, bool allowPlayerControlled = false)
        {
            try
            {
                return GetNearestWoWUnit(listWoWUnit, Me.Position, ignorenotSelectable, ignoreBlackList, allowPlayerControlled);
            }
            catch (Exception exception)
            {
                Logging.WriteError("public static WoWUnit GetNearestWoWUnit(List<WoWUnit> listWoWUnit, bool ignorenotSelectable = false, bool ignoreBlackList = false, bool allowPlayerControlled = false): " + exception, true);
            }
            return new WoWUnit(0);
        }

        public static WoWUnit GetNearestWoWUnit(List<WoWUnit> listWoWUnit, Point point, bool ignorenotSelectable = false, bool ignoreBlackList = false, bool allowPlayerControlled = false)
        {
            try
            {
                WoWUnit unit = new WoWUnit(0);
                float num = 9999999f;
                foreach (WoWUnit unit2 in listWoWUnit)
                {
                    if ((((point.DistanceTo(unit2.Position) <= num) && (!nManagerSetting.IsBlackListed(unit2.Guid) || ignoreBlackList)) && (ignorenotSelectable || !unit2.NotSelectable)) && (!unit2.IsTapped && (!unit2.PlayerControlled || allowPlayerControlled)))
                    {
                        unit = unit2;
                        num = point.DistanceTo(unit2.Position);
                    }
                }
                return unit;
            }
            catch (Exception exception)
            {
                Logging.WriteError("public static WoWUnit GetNearestWoWUnit(List<WoWUnit> listWoWUnit, Point point, bool ignorenotSelectable = false, bool ignoreBlackList = false, bool allowPlayerControlled = false): " + exception, true);
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
                List<WoWUnit> list = (distanceSearch > 60) ? GetObjectWoWUnit() : GetObjectWoWUnit60Yards();
                foreach (WoWUnit unit in list)
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

        public static List<WoWAreaTrigger> GetObjectWoWAreaTrigger()
        {
            List<WoWAreaTrigger> list;
            try
            {
                lock (Locker)
                {
                    list = _eviebupeuc.ToList<WoWAreaTrigger>();
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetObjectWoWAreaTrigger(): " + exception, true);
                list = new List<WoWAreaTrigger>();
            }
            return list;
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

        public static List<WoWDynamicObject> GetObjectWoWDynamicObject()
        {
            List<WoWDynamicObject> list;
            try
            {
                lock (Locker)
                {
                    list = _ilefanaetineAduek.ToList<WoWDynamicObject>();
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetObjectWoWDynamicObject(): " + exception, true);
                list = new List<WoWDynamicObject>();
            }
            return list;
        }

        public static List<WoWGameObject> GetObjectWoWGameObject()
        {
            List<WoWGameObject> list;
            try
            {
                lock (Locker)
                {
                    list = _qeoqiehipigo.ToList<WoWGameObject>();
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
                    for (int i = 0; i < _igeaxow.Count; i++)
                    {
                        WoWPlayer item = _igeaxow[i];
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
                    for (int i = 0; i < _igeaxow.Count; i++)
                    {
                        WoWPlayer player = _igeaxow[i];
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
                    if (_luqiocixuNu == null)
                    {
                        return new List<WoWUnit>();
                    }
                    List<WoWUnit> list = new List<WoWUnit>();
                    for (int i = 0; i < _luqiocixuNu.Count; i++)
                    {
                        WoWUnit item = _luqiocixuNu[i];
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

        public static List<WoWUnit> GetObjectWoWUnit60Yards()
        {
            List<WoWUnit> list2;
            try
            {
                lock (Locker)
                {
                    if (_nojaepeakicof == null)
                    {
                        return new List<WoWUnit>();
                    }
                    List<WoWUnit> list = new List<WoWUnit>();
                    for (int i = 0; i < _nojaepeakicof.Count; i++)
                    {
                        WoWUnit item = _nojaepeakicof[i];
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
                foreach (WoWUnit unit in GetObjectWoWUnit60Yards())
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

        public static uint GetPlayerInSpellRange(float spellRange = 5f, bool friendly = true, WoWUnit fromUnitOrPlayer = null)
        {
            return (uint) GetPlayerInSpellRangeList(spellRange, friendly, fromUnitOrPlayer).Count;
        }

        public static List<WoWPlayer> GetPlayerInSpellRangeList(float spellRange = 5f, bool friendly = true, WoWUnit fromUnitOrPlayer = null)
        {
            List<WoWPlayer> list = new List<WoWPlayer>();
            if (spellRange < 5f)
            {
                spellRange = 5f;
            }
            foreach (WoWPlayer player in GetObjectWoWPlayer())
            {
                if ((((!player.IsValid || !player.IsDead) || player.NotSelectable) || (!friendly && (!player.IsHostile || !player.Attackable))) || (friendly && player.IsHostile))
                {
                    continue;
                }
                if (((fromUnitOrPlayer == null) || !fromUnitOrPlayer.IsValid) || !fromUnitOrPlayer.IsAlive)
                {
                    if (player.GetDistance <= spellRange)
                    {
                        goto Label_0092;
                    }
                    continue;
                }
                if (player.Position.DistanceTo(fromUnitOrPlayer.Position) > spellRange)
                {
                    continue;
                }
            Label_0092:
                list.Add(player);
            }
            return list;
        }

        public static WoWUnit GetUnitInAggroRange()
        {
            foreach (WoWUnit unit in GetObjectWoWUnit60Yards())
            {
                if ((((unit.IsValid && unit.IsAlive) && (unit.Attackable && !unit.PlayerControlled)) && (((UnitRelation.GetReaction(Me, unit) < Reaction.Neutral) && (unit.GetDistance < (unit.AggroDistance * 0.85f))) && (!unit.InCombat || unit.IsTargetingMe))) && (unit.GetDistance <= 15f))
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
            foreach (WoWUnit unit in GetObjectWoWUnit60Yards())
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
            List<WoWUnit> list3 = GetObjectWoWUnit60Yards();
            for (int i = 0; i < objectWoWPlayer.Count; i++)
            {
                WoWPlayer item = objectWoWPlayer[i];
                if (((!BlackListMobAttack.Contains(item.Guid) && item.IsValid) && (item.IsAlive && (item.Target != 0))) && ((item.Target == Me.Guid) || (item.Target == Pet.Guid)))
                {
                    list.Add(item);
                }
            }
            for (int j = 0; j < list3.Count; j++)
            {
                WoWUnit unit = list3[j];
                if (((!BlackListMobAttack.Contains(unit.Guid) && unit.IsValid) && (unit.IsAlive && (unit.Target != 0))) && ((unit.Target == Me.Guid) || (unit.Target == Pet.Guid)))
                {
                    list.Add(unit);
                }
            }
            return list;
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
                    if (((obj2.GOType == WoWGameObjectType.Chest) || (obj2.GOType == WoWGameObjectType.Goober)) || (obj2.GOType == WoWGameObjectType.GatheringNode))
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
                    foreach (WoWGameObject obj2 in _qeoqiehipigo)
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
                return GetWoWUnitAuctioneer(GetObjectWoWUnit60Yards());
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
                    if ((entrys.Contains(unit.Entry) && ((!isDead && !unit.IsDead) || isDead)) && !unit.IsInvisible)
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
                return GetWoWUnitByFaction(GetObjectWoWUnit60Yards(), faction);
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
                return GetWoWUnitByFaction(GetObjectWoWUnit60Yards(), factions, pvp);
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
                        if (((factions.Contains(unit.Faction) && !unit.IsDead) && !unit.IsInvisible) && ((pvp || !unit.InCombat) || unit.InCombatWithMe))
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

        public static List<WoWUnit> GetWoWUnitByName(List<WoWUnit> listWoWUnit, List<string> names)
        {
            try
            {
                List<WoWUnit> list = new List<WoWUnit>();
                foreach (WoWUnit unit in listWoWUnit)
                {
                    if ((names.Contains(unit.Name) && !unit.IsDead) && !unit.IsInvisible)
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
                    if ((((unit.QuestItem1 == lootId) || (unit.QuestItem2 == lootId)) || ((unit.QuestItem3 == lootId) || (unit.QuestItem4 == lootId))) && !unit.IsInvisible)
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
                return GetWoWUnitFlightMaster(GetObjectWoWUnit60Yards());
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

        public static List<WoWUnit> GetWoWUnitFlightMasterUndiscovered()
        {
            try
            {
                return GetWoWUnitFlightMasterUndiscovered(GetObjectWoWUnit60Yards());
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitFlightMasterUndiscovered(): " + exception, true);
            }
            return new List<WoWUnit>();
        }

        public static List<WoWUnit> GetWoWUnitFlightMasterUndiscovered(List<WoWUnit> listWoWUnit)
        {
            try
            {
                List<WoWUnit> list = new List<WoWUnit>();
                foreach (WoWUnit unit in listWoWUnit)
                {
                    if (unit.IsNpcFlightMaster && (unit.UnitFlightMasteStatus == UnitFlightMasterStatus.FlightUndiscovered))
                    {
                        list.Add(unit);
                    }
                }
                return list;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetWoWUnitFlightMasterUndiscovered(List<WoWUnit> listWoWUnit): " + exception, true);
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
                foreach (WoWUnit unit in GetObjectWoWUnit60Yards())
                {
                    if (((UnitRelation.GetReaction(Me.Faction, unit.Faction) < Reaction.Neutral) && !unit.IsDead) && (!unit.InCombat || unit.InCombatWithMe))
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
                return GetWoWUnitInkeeper(GetObjectWoWUnit60Yards());
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
                return GetWoWUnitLootable(GetObjectWoWUnit60Yards());
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
                return GetWoWUnitMailbox(GetObjectWoWUnit60Yards());
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
                return GetWoWUnitQuester(GetObjectWoWUnit60Yards());
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
                return GetWoWUnitRepair(GetObjectWoWUnit60Yards());
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
                return GetWoWUnitSkinnable(GetObjectWoWUnit60Yards(), withoutGuid);
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
                int num3 = Skill.GetValue(SkillLine.Mining);
                int num4 = Skill.GetValue(SkillLine.Engineering);
                foreach (WoWUnit unit in listWoWUnit)
                {
                    if (unit.IsSkinnable && !withoutGuid.Contains(unit.Guid))
                    {
                        if (unit.ExtraLootType.HasFlag(TypeFlag.HERB_LOOT))
                        {
                            if (num2 <= 0)
                            {
                                continue;
                            }
                            list.Add(unit);
                        }
                        if (unit.ExtraLootType.HasFlag(TypeFlag.MINING_LOOT))
                        {
                            if (num3 <= 0)
                            {
                                continue;
                            }
                            list.Add(unit);
                        }
                        if (unit.ExtraLootType.HasFlag(TypeFlag.ENGENEERING_LOOT))
                        {
                            if (num4 <= 0)
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
                return GetWoWUnitSpiritGuide(GetObjectWoWUnit60Yards());
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
                return GetWoWUnitSpiritHealer(GetObjectWoWUnit60Yards());
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
                return GetWoWUnitSummonedOrCreatedByMeAndFighting(GetObjectWoWUnit60Yards());
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
                return GetWoWUnitTrainer(GetObjectWoWUnit60Yards());
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
                return GetWoWUnitVendor(GetObjectWoWUnit60Yards());
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
                return GetWoWUnitVendorFood(GetObjectWoWUnit60Yards());
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

        internal static void OmisiepapJer()
        {
            try
            {
                int num4;
                while (Addresses.ObjectManagerClass.sCurMgr == 0)
                {
                    Thread.Sleep(10);
                }
                set_ObjectManagerAddress(Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + Addresses.ObjectManagerClass.sCurMgr));
                UInt128 num = Memory.WowMemory.Memory.ReadUInt128(get_ObjectManagerAddress() + 0xf8);
                Usefuls.ContinentId = Memory.WowMemory.Memory.ReadInt(get_ObjectManagerAddress() + 0x108);
                for (int i = Memory.WowMemory.Memory.ReadInt(get_ObjectManagerAddress() + 0xd8); i != 0; i = num4)
                {
                    try
                    {
                        UInt128 key = Memory.WowMemory.Memory.ReadUInt128((uint) (i + 0x30));
                        if (!ObjectDictionary.ContainsKey(key))
                        {
                            WoWObjectType type = (WoWObjectType) Memory.WowMemory.Memory.ReadByte((uint) (i + 0x10));
                            WoWObject obj2 = null;
                            if (num == key)
                            {
                                Me.Aniubekuni((uint) i);
                                obj2 = new WoWPlayer((uint) i);
                            }
                            switch (type)
                            {
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
                                    obj2 = new WoWPlayer((uint) i);
                                    break;

                                case WoWObjectType.GameObject:
                                    obj2 = new WoWGameObject((uint) i);
                                    break;

                                case WoWObjectType.Corpse:
                                    obj2 = new WoWCorpse((uint) i);
                                    break;

                                case WoWObjectType.AreaTrigger:
                                    obj2 = new WoWAreaTrigger((uint) i);
                                    break;

                                case WoWObjectType.Scene:
                                case WoWObjectType.Conversation:
                                    obj2 = new WoWObject((uint) i);
                                    break;
                            }
                            if (obj2 != null)
                            {
                                ObjectDictionary.TryAdd(key, obj2);
                            }
                        }
                        else
                        {
                            ObjectDictionary[key].Aniubekuni((uint) i);
                        }
                    }
                    catch (Exception exception)
                    {
                        Logging.WriteError("ObjectManager >  ReadObjectList()#1: " + exception, true);
                    }
                    num4 = Memory.WowMemory.Memory.ReadInt((uint) (i + 0x44));
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

        private static uint _seujeocekiu
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

        public static WoWUnit Focus
        {
            get
            {
                try
                {
                    if (Me.IsValid && (Me.Focus > 0))
                    {
                        if (_fowoulowiof > 0)
                        {
                            if (new WoWUnit(_fowoulowiof).Guid == Me.Focus)
                            {
                                return new WoWUnit(_fowoulowiof);
                            }
                            _fowoulowiof = 0;
                        }
                        WoWUnit unit = new WoWUnit(GetObjectByGuid(Me.Focus).GetBaseAddress);
                        _fowoulowiof = unit.GetBaseAddress;
                        return unit;
                    }
                }
                catch (Exception exception)
                {
                    Logging.WriteError("Focus: " + exception, true);
                }
                return new WoWUnit(0);
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
                    return _omiaweubaw.ToList<WoWObject>();
                }
            }
        }

        public static WoWUnit Pet
        {
            get
            {
                try
                {
                    UInt128 guid = Memory.WowMemory.Memory.ReadUInt128(Memory.WowProcess.WowModule + 0xfb2138);
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
                        if (_ocikouhadojoUbioc > 0)
                        {
                            if (new WoWUnit(_ocikouhadojoUbioc).Guid == Me.Target)
                            {
                                return new WoWUnit(_ocikouhadojoUbioc);
                            }
                            _ocikouhadojoUbioc = 0;
                        }
                        WoWUnit unit = new WoWUnit(GetObjectByGuid(Me.Target).GetBaseAddress);
                        _ocikouhadojoUbioc = unit.GetBaseAddress;
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

