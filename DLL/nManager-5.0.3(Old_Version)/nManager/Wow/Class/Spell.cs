namespace nManager.Wow.Class
{
    using nManager;
    using nManager.Helpful;
    using nManager.Products;
    using nManager.Wow;
    using nManager.Wow.Enums;
    using nManager.Wow.Helpers;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class Spell
    {
        public float CastTime;
        public uint CategoryId;
        public int Cost;
        public string Icon;
        public uint Id;
        public List<uint> Ids;
        public bool IsFunnel;
        public bool KnownSpell;
        public float MaxRangeFriend;
        public float MaxRangeHostile;
        public float MinRangeFriend;
        public float MinRangeHostile;
        public string Name;
        public string NameInGame;
        public nManager.Wow.Enums.PowerType PowerType;
        public string Rank;
        public uint StartRecoveryCategoryId;

        public Spell(string spellName)
        {
            this.Ids = new List<uint>();
            this.Icon = "";
            this.Name = "";
            this.NameInGame = "";
            this.Rank = "";
            Spell spell = null;
            try
            {
                uint.TryParse(spellName, out this.Id);
                if (this.Id > 0)
                {
                    spell = new Spell(this.Id);
                }
                else
                {
                    foreach (Spell spell2 in SpellManager.SpellBook())
                    {
                        if ((spell2.Name.ToLower().Trim() == spellName.ToLower().Trim()) || (spell2.NameInGame.ToLower().Trim() == spellName.ToLower().Trim()))
                        {
                            spell = spell2;
                            break;
                        }
                    }
                }
                if (spell == null)
                {
                    Logging.WriteDebug("Spell(string spellName): spellName=" + spellName + " => Failed");
                }
                else
                {
                    Logging.WriteDebug(string.Concat(new object[] { "Spell(string spellName): spellName=", spellName, ", Id found: ", spell.Id, ", Name found: ", spell.Name, ", NameInGame found: ", spell.NameInGame, ", KnownSpell: ", spell.KnownSpell }));
                    this.Id = spell.Id;
                    this.CastTime = spell.CastTime;
                    this.Cost = spell.Cost;
                    this.Icon = spell.Icon;
                    this.IsFunnel = spell.IsFunnel;
                    this.MaxRangeHostile = spell.MaxRangeHostile;
                    this.MinRangeHostile = spell.MinRangeHostile;
                    this.MaxRangeFriend = spell.MaxRangeFriend;
                    this.MinRangeFriend = spell.MinRangeFriend;
                    this.Name = spell.Name;
                    this.NameInGame = spell.NameInGame;
                    this.PowerType = spell.PowerType;
                    this.Rank = spell.Rank;
                    this.KnownSpell = spell.KnownSpell;
                    this.Ids.AddRange(spell.Ids);
                    this.Ids.Add(this.Id);
                    this.CategoryId = spell.CategoryId;
                    this.StartRecoveryCategoryId = spell.StartRecoveryCategoryId;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Spell(string spellName): " + exception, true);
            }
        }

        public Spell(uint spellId)
        {
            this.Ids = new List<uint>();
            this.Icon = "";
            this.Name = "";
            this.NameInGame = "";
            this.Rank = "";
            lock (this)
            {
                try
                {
                    this.Id = spellId;
                    SpellManager.SpellInfoLua spellInfo = SpellManager.GetSpellInfo(this.Id);
                    if ((spellInfo.ID > 0) && (spellId == spellInfo.ID))
                    {
                        this.CastTime = ((float) spellInfo.CastTime) / 1000f;
                        float minRange = spellInfo.MinRange;
                        float maxRange = spellInfo.MaxRange;
                        this.MaxRangeHostile = maxRange;
                        this.MinRangeHostile = minRange;
                        this.MaxRangeFriend = maxRange;
                        this.MinRangeFriend = minRange;
                        this.Name = SpellManager.SpellListManager.SpellNameById(spellId);
                        this.NameInGame = spellInfo.Name;
                        this.Cost = spellInfo.Cost;
                        this.Icon = spellInfo.Icon;
                        this.IsFunnel = spellInfo.IsFunnel;
                        this.PowerType = spellInfo.PowerType;
                        this.Rank = spellInfo.Rank;
                        if (this.MaxRangeHostile < 5f)
                        {
                            this.MaxRangeHostile = 5f;
                        }
                        if (this.MaxRangeFriend < 5f)
                        {
                            this.MaxRangeFriend = 5f;
                        }
                        this.KnownSpell = SpellManager.KnownSpell(this.Id);
                        this.Ids.AddRange(SpellManager.SpellListManager.SpellIdByName(this.Name));
                        this.Ids.Add(this.Id);
                        this.CategoryId = WoWSpellCategories.GetSpellCategoryBySpellId(this.Id);
                        this.StartRecoveryCategoryId = WoWSpellCategories.GetSpellStartRecoverCategoryBySpellId(this.Id);
                        goto Label_0200;
                    }
                }
                catch (Exception exception)
                {
                    Logging.WriteError("Spell(uint spellId): " + exception, true);
                }
                this.CastTime = 0f;
                this.MaxRangeHostile = 5f;
                this.MinRangeHostile = 0f;
                this.MaxRangeFriend = 5f;
                this.MinRangeFriend = 0f;
                this.NameInGame = "";
                this.CategoryId = 0;
            Label_0200:;
            }
        }

        public void Cast()
        {
            this.Launch();
        }

        public void Cast(bool StopMove, bool waitIsCast = true, bool ignoreIfCast = false, string unitId = null)
        {
            this.Launch(StopMove, waitIsCast, ignoreIfCast, unitId);
        }

        public void CastOnSelf()
        {
            this.LaunchOnSelf();
        }

        public void CastOnSelf(bool StopMove, bool waitIsCast = true, bool ignoreIfCast = false)
        {
            this.LaunchOnSelf(StopMove, waitIsCast, ignoreIfCast);
        }

        public void CastOnUnitID(string unitId)
        {
            this.LaunchOnUnitID(unitId);
        }

        public bool CreatedBySpellInRange(uint maxrange = 40)
        {
            try
            {
                List<WoWUnit> woWUnitByName = nManager.Wow.ObjectManager.ObjectManager.GetWoWUnitByName(this.NameInGame);
                if (woWUnitByName.Count > 0)
                {
                    WoWUnit unit = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWUnit(woWUnitByName, false, false, true);
                    if (((unit.IsValid && unit.IsAlive) && ((unit.SummonedBy == nManager.Wow.ObjectManager.ObjectManager.Me.Guid) || (unit.CreatedBy == nManager.Wow.ObjectManager.ObjectManager.Me.Guid))) && (unit.GetDistance <= maxrange))
                    {
                        return true;
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Spell > CreatedBySpellInRange: " + exception, true);
            }
            return false;
        }

        public void Launch()
        {
            try
            {
                this.Launch(!(this.CastTime == 0f), true, false, null);
            }
            catch (Exception exception)
            {
                Logging.WriteError("Spell > Launch(): " + exception, true);
            }
            finally
            {
                Memory.WowMemory.GameFrameUnLock();
            }
        }

        public void Launch(bool StopMove, bool waitIsCast = true, bool ignoreIfCast = false, string unitId = null)
        {
            try
            {
                Memory.WowMemory.GameFrameUnLock();
                if (StopMove)
                {
                    if (!nManagerSetting.CurrentSetting.ActivateMovementsDamageDealer && (nManager.Products.Products.ProductName == "Damage Dealer"))
                    {
                        StopMove = false;
                    }
                    if (!nManagerSetting.CurrentSetting.ActivateMovementsHealerBot && (nManager.Products.Products.ProductName == "Heal Bot"))
                    {
                        StopMove = false;
                    }
                }
                int num = 10;
                while (nManager.Wow.ObjectManager.ObjectManager.Me.IsCast && !ignoreIfCast)
                {
                    Thread.Sleep(100);
                    num--;
                    if (num < 0)
                    {
                        return;
                    }
                }
                Logging.WriteFight("Cast " + this.NameInGame);
                if (StopMove && nManager.Wow.ObjectManager.ObjectManager.Me.GetMove)
                {
                    MovementManager.StopMove();
                }
                if (unitId == null)
                {
                    SpellManager.CastSpellByNameLUA(this.NameInGame);
                }
                else
                {
                    SpellManager.CastSpellByNameLUA(this.NameInGame, unitId);
                }
                while (nManager.Wow.ObjectManager.ObjectManager.Me.IsCast && waitIsCast)
                {
                    Thread.Sleep(100);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Spell > Launch(bool StopMove, bool waitIsCast = true, bool ignoreIfCast = false): " + exception, true);
            }
            finally
            {
                Memory.WowMemory.GameFrameUnLock();
            }
        }

        public void LaunchOnSelf()
        {
            try
            {
                WoWUnit target = nManager.Wow.ObjectManager.ObjectManager.Target;
                if (target.IsValid)
                {
                    Interact.InteractWithBeta(nManager.Wow.ObjectManager.ObjectManager.Me.GetBaseAddress);
                }
                this.Launch(!(this.CastTime == 0f), true, false, null);
                if (target.IsValid)
                {
                    Interact.InteractWith(target.GetBaseAddress, true);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Spell > LaunchOnSelf():" + exception, true);
            }
            finally
            {
                Memory.WowMemory.GameFrameUnLock();
            }
        }

        public void LaunchOnSelf(bool StopMove, bool waitIsCast = true, bool ignoreIfCast = false)
        {
            try
            {
                WoWUnit target = nManager.Wow.ObjectManager.ObjectManager.Target;
                if (target.IsValid)
                {
                    Interact.InteractWithBeta(nManager.Wow.ObjectManager.ObjectManager.Me.GetBaseAddress);
                }
                this.Launch(StopMove, waitIsCast, ignoreIfCast, null);
                if (target.IsValid)
                {
                    Interact.InteractWith(target.GetBaseAddress, true);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Spell > LaunchOnSelf(bool StopMove, bool waitIsCast = true, bool ignoreIfCast = false): " + exception, true);
            }
            finally
            {
                Memory.WowMemory.GameFrameUnLock();
            }
        }

        public void LaunchOnUnitID(string unitId)
        {
            try
            {
                this.Launch(!(this.CastTime == 0f), true, false, unitId);
            }
            catch (Exception exception)
            {
                Logging.WriteError("Spell > Launch(uintId): " + exception, true);
            }
            finally
            {
                Memory.WowMemory.GameFrameUnLock();
            }
        }

        internal void Update()
        {
            try
            {
                this.KnownSpell = SpellManager.KnownSpell(this.Id);
            }
            catch (Exception exception)
            {
                Logging.WriteError("Spell > Update(): " + exception, true);
            }
        }

        public int BuffStack
        {
            get
            {
                try
                {
                    return nManager.Wow.ObjectManager.ObjectManager.Me.BuffStack(this.Ids);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("Spell > BuffStack: " + exception, true);
                }
                return -1;
            }
        }

        public bool CreatedBySpell
        {
            get
            {
                try
                {
                    List<WoWUnit> woWUnitByName = nManager.Wow.ObjectManager.ObjectManager.GetWoWUnitByName(this.NameInGame);
                    if (woWUnitByName.Count > 0)
                    {
                        WoWUnit unit = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWUnit(woWUnitByName, false, false, false);
                        if ((unit.IsValid && unit.IsAlive) && ((unit.SummonedBy == nManager.Wow.ObjectManager.ObjectManager.Me.Guid) || (unit.CreatedBy == nManager.Wow.ObjectManager.ObjectManager.Me.Guid)))
                        {
                            return true;
                        }
                    }
                }
                catch (Exception exception)
                {
                    Logging.WriteError("Spell > CreatedBySpell: " + exception, true);
                }
                return false;
            }
        }

        public int GetSpellCharges
        {
            get
            {
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString(string.Concat(new object[] { randomString, " = tostring(GetSpellCharges(", this.Id, "))" }), false, true);
                return Others.ToInt32(Lua.GetLocalizedText(randomString));
            }
        }

        public bool HaveBuff
        {
            get
            {
                try
                {
                    return nManager.Wow.ObjectManager.ObjectManager.Me.HaveBuff(this.Ids);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("Spell > HaveBuff: " + exception, true);
                }
                return false;
            }
        }

        public bool IsFriendDistanceGood
        {
            get
            {
                try
                {
                    return CombatClass.InSpellRange(nManager.Wow.ObjectManager.ObjectManager.Target, this.MinRangeFriend, this.MaxRangeFriend);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("Spell > IsFriendlyDistanceGood: " + exception, true);
                    return true;
                }
            }
        }

        public bool IsHostileDistanceGood
        {
            get
            {
                try
                {
                    return CombatClass.InSpellRange(nManager.Wow.ObjectManager.ObjectManager.Target, this.MinRangeHostile, this.MaxRangeHostile);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("Spell > IsHostileDistanceGood: " + exception, true);
                    return true;
                }
            }
        }

        public bool IsSpellUsable
        {
            get
            {
                try
                {
                    return (this.KnownSpell && SpellManager.IsSpellUsableLUA(this));
                }
                catch (Exception exception)
                {
                    Logging.WriteError("Spell > IsSpellUsable: " + exception, true);
                }
                return false;
            }
        }

        public int TargetBuffStack
        {
            get
            {
                try
                {
                    return nManager.Wow.ObjectManager.ObjectManager.Target.BuffStack(this.Ids);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("Spell > TargetBuffStack: " + exception, true);
                }
                return 0;
            }
        }

        public bool TargetHaveBuff
        {
            get
            {
                try
                {
                    return nManager.Wow.ObjectManager.ObjectManager.Target.HaveBuff(this.Ids);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("Spell > TargetHaveBuff: " + exception, true);
                    return false;
                }
            }
        }

        public bool TargetHaveBuffFromMe
        {
            get
            {
                try
                {
                    return nManager.Wow.ObjectManager.ObjectManager.Target.UnitAura(this.Ids, nManager.Wow.ObjectManager.ObjectManager.Me.Guid).IsValid;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("Spell > TargetHaveBuffFromMe: " + exception, true);
                    return false;
                }
            }
        }
    }
}

